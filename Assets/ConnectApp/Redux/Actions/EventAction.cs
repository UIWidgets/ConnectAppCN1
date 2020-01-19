using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class EventMapAction : BaseAction {
        public Dictionary<string, IEvent> eventMap;
    }

    public class ClearEventOngoingAction : RequestAction {
    }

    public class ClearEventCompletedAction : RequestAction {
    }

    public class StartFetchEventOngoingAction : RequestAction {
    }

    public class StartFetchEventCompletedAction : RequestAction {
    }

    public class FetchEventsSuccessAction : BaseAction {
        public List<string> eventIds;
        public int pageNumber = 0;
        public int total;
        public string tab;
    }

    public class FetchEventsFailureAction : BaseAction {
        public string tab;
        public int pageNumber = 0;
    }

    public class StartFetchEventDetailAction : RequestAction {
    }

    public class FetchEventDetailSuccessAction : BaseAction {
        public IEvent eventObj;
    }

    public class FetchEventDetailFailedAction : BaseAction {
    }

    public class SaveEventHistoryAction : BaseAction {
        public IEvent eventObj;
        public EventType eventType;
    }

    public class DeleteEventHistoryAction : BaseAction {
        public string eventId;
    }

    public class DeleteAllEventHistoryAction : BaseAction {
    }

    public class StartJoinEventAction : RequestAction {
    }

    public class JoinEventSuccessAction : BaseAction {
        public string eventId;
    }

    public class JoinEventFailureAction : BaseAction {
    }

    public class StartFetchMessagesAction : RequestAction {
    }

    public class FetchMessagesSuccessAction : BaseAction {
        public bool isFirstLoad;
        public string channelId;
        public List<string> messageIds;
        public Dictionary<string, Message> messageDict;
        public bool hasMore;
        public string currOldestMessageId = "";
    }

    public class FetchMessagesFailureAction : BaseAction {
    }

    public class StartSendMessageAction : RequestAction {
    }

    public class SendMessageSuccessAction : BaseAction {
        public string channelId;
        public string content;
        public string nonce;
        public string parentMessageId = "";
    }

    public class SendMessageFailureAction : BaseAction {
    }

    public static partial class Actions {
        public static object fetchEvents(int pageNumber, string tab, string mode) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return EventApi.FetchEvents(pageNumber: pageNumber, tab: tab, mode: mode)
                    .Then(eventsResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = eventsResponse.userMap});
                        dispatcher.dispatch(new PlaceMapAction {placeMap = eventsResponse.placeMap});
                        var eventIds = new List<string>();
                        var eventMap = new Dictionary<string, IEvent>();
                        eventsResponse.events.items.ForEach(eventObj => {
//                        if (eventObj.mode == "online") return;
                            eventIds.Add(item: eventObj.id);
                            eventMap.Add(key: eventObj.id, value: eventObj);
                        });
                        dispatcher.dispatch(new EventMapAction {eventMap = eventMap});
                        dispatcher.dispatch(new FetchEventsSuccessAction {
                            eventIds = eventIds,
                            tab = tab,
                            pageNumber = pageNumber,
                            total = eventsResponse.events.total
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchEventsFailureAction {
                            tab = tab,
                            pageNumber = pageNumber
                        });
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchEventDetail(string eventId, EventType eventType) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return EventApi.FetchEventDetail(eventId: eventId)
                    .Then(eventObj => {
                        if (getState().loginState.isLoggedIn && eventType == EventType.online) {
                            dispatcher.dispatch(new StartFetchMessagesAction());
                            dispatcher.dispatch(fetchMessages(eventObj.channelId, "", true));
                        }

                        var userMap = new Dictionary<string, User> {
                            {eventObj.user.id, eventObj.user}
                        };
                        eventObj.hosts.ForEach(host => {
                            if (userMap.ContainsKey(host.id)) {
                                userMap[host.id] = host;
                            }
                            else {
                                userMap.Add(host.id, host);
                            }
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        dispatcher.dispatch(new UserLicenseMapAction {userLicenseMap = eventObj.userLicenseMap});
                        if (getState().eventState.eventsDict.ContainsKey(eventObj.id)) {
                            var oldEventObj = getState().eventState.eventsDict[eventObj.id];
                            var newEventObj = eventObj;
                            newEventObj.userId = eventObj.user.id;
                            newEventObj.placeId = oldEventObj.placeId;
                            newEventObj.mode = oldEventObj.mode;
                            newEventObj.avatar = oldEventObj.avatar;
                            newEventObj.type = oldEventObj.type;
                            newEventObj.typeParam = oldEventObj.typeParam;
                            eventObj = newEventObj;
                        }

                        eventObj.isNotFirst = true;
                        dispatcher.dispatch(new EventMapAction {
                            eventMap = new Dictionary<string, IEvent> {
                                {eventObj.id, eventObj}
                            }
                        });
                        dispatcher.dispatch(new FetchEventDetailSuccessAction {eventObj = eventObj});
                        dispatcher.dispatch(new SaveEventHistoryAction {eventObj = eventObj, eventType = eventType});
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchEventDetailFailedAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object joinEvent(string eventId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return EventApi.JoinEvent(eventId: eventId)
                    .Then(id => { dispatcher.dispatch(new JoinEventSuccessAction {eventId = id}); })
                    .Catch(error => {
                        dispatcher.dispatch(new JoinEventFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchMessages(string channelId, string currOldestMessageId, bool isFirstLoad) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return MessageApi.FetchMessages(channelId: channelId, currOldestMessageId: currOldestMessageId)
                    .Then(messagesResponse => {
                        var messageIds = new List<string>();
                        var messageDict = new Dictionary<string, Message>();

                        var channelMessageList = getState().messageState.channelMessageList;
                        var channelMessageDict = getState().messageState.channelMessageDict;
                        if (channelMessageList.ContainsKey(channelId) && !isFirstLoad) {
                            messageIds = channelMessageList[channelId];
                        }

                        if (channelMessageDict.ContainsKey(channelId) && !isFirstLoad) {
                            messageDict = channelMessageDict[channelId];
                        }

                        var userMap = new Dictionary<string, User>();
                        messagesResponse.items.ForEach(message => {
                            if (message.deletedTime == null && message.type == "normal") {
                                if (messageIds.Contains(message.id)) {
                                    messageIds.Remove(message.id);
                                }

                                messageIds.Add(message.id);

                                if (messageDict.ContainsKey(message.id)) {
                                    messageDict[message.id] = message;
                                }
                                else {
                                    messageDict.Add(message.id, message);
                                }
                            }

                            if (userMap.ContainsKey(message.author.id)) {
                                userMap[message.author.id] = message.author;
                            }
                            else {
                                userMap.Add(message.author.id, message.author);
                            }
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        dispatcher.dispatch(new FetchMessagesSuccessAction {
                            isFirstLoad = isFirstLoad,
                            channelId = channelId,
                            messageIds = messageIds,
                            messageDict = messageDict,
                            hasMore = messagesResponse.hasMore,
                            currOldestMessageId = messagesResponse.currOldestMessageId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchMessagesFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object sendMessage(string channelId, string content, string nonce, string parentMessageId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return MessageApi.SendMessage(channelId, content, nonce, parentMessageId)
                    .Then(sendMessageResponse => {
                        dispatcher.dispatch(new SendMessageSuccessAction {
                            channelId = channelId,
                            content = content,
                            nonce = nonce
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new SendMessageFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }
    }
}