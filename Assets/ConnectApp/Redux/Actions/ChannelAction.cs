using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public static partial class Actions {
        public static object fetchPublicChannels() {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchPublicChannels().Then(channelResponse => {
                        dispatcher.dispatch(new PublicChannelsAction {
                            channels = channelResponse.items,
                            currentPage = channelResponse.currentPage,
                            pages = channelResponse.pages,
                            total = channelResponse.total
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchPublicChannelsFailureAction());
                        Debug.Log(error);
                    });
            });
        }
        
        public static object fetchJoinedChannels() {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchJoinedChannels().Then(channelResponse => {
                        dispatcher.dispatch(new JoinedChannelsAction {
                            channels = channelResponse.items,
                            currentPage = channelResponse.currentPage,
                            pages = channelResponse.pages,
                            total = channelResponse.total
                        });
                        for (int i = 0; i < channelResponse.items.Count; i++) {
                            dispatcher.dispatch(fetchChannelMessages(channelResponse.items[i].id));
                        }
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchJoinedChannelsFailureAction());
                        Debug.Log(error);
                    });
            });
        }

        public static object fetchChannelMessages(string channelId, string before = null, string after = null) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchChannelMessages(channelId, before, after)
                    .Then(channelMessagesResponse => {
                        dispatcher.dispatch(new ChannelMessagesAction {
                            channelId = channelId,
                            messages = channelMessagesResponse.items,
                            before = before,
                            after = after
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchChannelMessagesFailureAction());
                        Debug.Log(error);
                    });
            });
        }
        
        public static object fetchChannelMembers(string channelId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchChannelMembers(channelId).Then(channelMemberResponse => {
                        dispatcher.dispatch(new ChannelMemberAction {
                            channelId = channelId,
                            members = channelMemberResponse
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchChannelMemberFailureAction());
                        Debug.Log(error);
                    });
            });
        }
    }

    public class PublicChannelsAction {
        public List<Channel> channels;
        public int currentPage;
        public List<int> pages;
        public int total;
    }
    
    public class JoinedChannelsAction {
        public List<Channel> channels;
        public int currentPage;
        public List<int> pages;
        public int total;
    }

    public class ChannelMessagesAction {
        public string channelId;
        public List<ChannelMessage> messages;
        public string before;
        public string after;
        public bool hasMore;
        public bool hasMoreNew;
    }

    public class ChannelMemberAction {
        public string channelId;
        public List<ChannelMember> members;
    }
    
    public class FetchPublicChannelsSuccessAction : BaseAction {
    }
    
    public class FetchPublicChannelsFailureAction : BaseAction {
    }
    
    public class FetchJoinedChannelsSuccessAction : BaseAction {
    }
    
    public class FetchJoinedChannelsFailureAction : BaseAction {
    }
    
    public class FetchChannelMessagesSuccessAction : BaseAction {
    }
    
    public class FetchChannelMessagesFailureAction : BaseAction {
    }
    
    public class FetchChannelMemberSuccessAction : BaseAction {
    }
    
    public class FetchChannelMemberFailureAction : BaseAction {
    }

    public class StartSendChannelMessageAction : RequestAction {
    }

    public class MarkChannelMessageAsRead : RequestAction {
        public string channelId;
        public long nonce;
    }

    public class PushReadyAction : BaseAction {
    }

    public class PushNewMessageAction : BaseAction {
        public SocketResponseMessageData messageData;
    }

    public class PushModifyMessageAction : BaseAction {
        public SocketResponseMessageData messageData;
    }

    public class PushDeleteMessageAction : BaseAction {
        public SocketResponseMessageData messageData;
    }
}