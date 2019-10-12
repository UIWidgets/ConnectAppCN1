using System;
using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public static partial class Actions {
        public static object fetchChannels(int page, bool fetchMessagesAfterSuccess = false) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchChannels(page: page).Then(channelResponse => {
                        dispatcher.dispatch(new FetchChannelsSuccessAction {
                            discoverList = channelResponse.discoverList ?? new List<string>(),
                            joinedList = channelResponse.joinedList ?? new List<string>(),
                            discoverPage = channelResponse.discoverPage,
                            channelMap = channelResponse.channelMap ?? new Dictionary<string, Channel>(),
                            joinedChannelMap = channelResponse.joinedChannelMap ?? new Dictionary<string, bool>()
                        });
                        if (fetchMessagesAfterSuccess) {
                            channelResponse.joinedList.ForEach(joinedChannelId => {
                                dispatcher.dispatch(fetchChannelMessages(channelId: joinedChannelId));
                                dispatcher.dispatch(fetchChannelMembers(channelId: joinedChannelId));
                            });
                        }
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchChannelsFailureAction());
                        Debug.Log(error);
                        dispatcher.dispatch(loadReadyStateFromDB());
                    });
            });
        }

        public static object fetchChannelMessages(string channelId, string before = null, string after = null) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                dispatcher.dispatch(new StartFetchChannelMessageAction());
                return ChannelApi.FetchChannelMessages(channelId: channelId, before: before, after: after)
                    .Then(channelMessagesResponse => {
                        dispatcher.dispatch(new UserLicenseMapAction
                            {userLicenseMap = channelMessagesResponse.userLicenseMap});
                        var userMap = new Dictionary<string, User>();
                        (channelMessagesResponse.items ?? new List<ChannelMessage>()).ForEach(channelMessage => {
                            userMap[key: channelMessage.author.id] = channelMessage.author;
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        dispatcher.dispatch(new FetchChannelMessagesSuccessAction {
                            channelId = channelId,
                            messages = channelMessagesResponse.items ?? new List<ChannelMessage>(),
                            before = before,
                            after = after,
                            hasMore = channelMessagesResponse.hasMore,
                            hasMoreNew = channelMessagesResponse.hasMoreNew
                        });
                        try {
                            dispatcher.dispatch(channelMessagesResponse.items?.isNotEmpty() ?? false
                                ? saveMessagesToDB(channelMessagesResponse.items)
                                : loadMessagesFromDB(channelId, CStringUtils.HexToLong(before)));
                        }
                        catch (Exception e) {
                            Debug.LogError(e);
                        }
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchChannelMessagesFailureAction());
                        Debug.LogError(error);
                        dispatcher.dispatch(loadMessagesFromDB(channelId, CStringUtils.HexToLong(before)));
                    });
            });
        }

        public static object fetchChannelMembers(string channelId, int offset = 0) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchChannelMembers(channelId: channelId, offset: offset)
                    .Then(channelMemberResponse => {
                        dispatcher.dispatch(new FollowMapAction {followMap = channelMemberResponse.followeeMap});
                        var userMap = new Dictionary<string, User>();
                        (channelMemberResponse.list ?? new List<ChannelMember>()).ForEach(member => {
                            userMap[key: member.user.id] = member.user;
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        dispatcher.dispatch(new FetchChannelMemberSuccessAction {
                            channelId = channelId,
                            offset = channelMemberResponse.offset,
                            total = channelMemberResponse.total,
                            members = channelMemberResponse.list
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchChannelMemberFailureAction());
                        Debug.Log(error);
                    });
            });
        }

        public static object joinChannel(string channelId, string groupId = null, bool loading = false) {
            if (loading) {
                CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog(message: "正在加入群聊"));
            }

            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.JoinChannel(channelId: channelId, groupId: groupId)
                    .Then(joinChannelResponse => {
                        if (loading) {
                            CustomDialogUtils.hiddenCustomDialog();
                        }
                        dispatcher.dispatch(new JoinChannelSuccessAction {channelId = channelId});
                        dispatcher.dispatch(fetchChannelMessages(channelId: channelId));
                        dispatcher.dispatch(fetchChannelMembers(channelId: channelId));
                    })
                    .Catch(error => {
                        if (loading) {
                            CustomDialogUtils.hiddenCustomDialog();
                        }
                        dispatcher.dispatch(new JoinChannelFailureAction {channelId = channelId});
                        Debug.Log(error);
                    });
            });
        }

        public static object leaveChannel(string channelId, string groupId = null) {
            CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog(message: "正在退出群聊"));

            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.LeaveChannel(channelId: channelId, groupId: groupId)
                    .Then(leaveChannelResponse => {
                        dispatcher.dispatch(new LeaveChannelSuccessAction {channelId = channelId});
                        CustomDialogUtils.hiddenCustomDialog();
                        dispatcher.dispatch(new MainNavigatorPopAction());
                        dispatcher.dispatch(new MainNavigatorPopAction());
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new LeaveChannelFailureAction());
                        CustomDialogUtils.hiddenCustomDialog();
                        Debug.Log(error);
                    });
            });
        }

        public static object sendChannelMessage(string channelId, string content, string nonce,
            string parentMessageId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return MessageApi.SendMessage(channelId, content, nonce, parentMessageId)
                    .Then(sendMessageResponse => {
                        dispatcher.dispatch(new SendChannelMessageSuccessAction {
                            channelId = channelId,
                            content = content,
                            nonce = nonce
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new SendChannelMessageFailureAction {
                            channelId = channelId
                        });
                        Debug.Log(error);
                    });
            });
        }

        public static object ackChannelMessage(string messageId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.AckChannelMessage(messageId: messageId)
                    .Then(ackMessageResponse => {
                        dispatcher.dispatch(new AckChannelMessageSuccessAction());
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new AckChannelMessageFailureAction());
                        Debug.Log(error);
                    });
            });
        }

        public static object sendImage(string channelId, string nonce, string imageData) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.SendImage(channelId, "", nonce, imageData)
                    .Then(responseText => {
                        dispatcher.dispatch(new SendChannelMessageSuccessAction {
                            channelId = channelId,
                            content = "",
                            nonce = nonce
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new SendChannelMessageFailureAction());
                        Debug.Log(error);
                    });
            });
        }

        public static object saveMessagesToDB(List<ChannelMessage> messages) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                MessengerDBApi.SyncSaveMessages(messages);
                dispatcher.dispatch(new SaveMessagesToDBSuccessAction());
                return Promise.Resolved();
            });
        }

        public static object loadMessagesFromDB(string channelId, long before) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var messages = MessengerDBApi.SyncLoadMessages(channelId, before, 10);
                dispatcher.dispatch(new LoadMessagesFromDBSuccessAction {
                    messages = messages,
                    before = before,
                    channelId = channelId
                });
                return Promise.Resolved();
            });
        }


        public static object saveReadyStateToDB(SocketResponseSessionData data) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                MessengerDBApi.SyncSaveReadyState(data);
                dispatcher.dispatch(new SaveReadyStateToDBSuccessAction());
                return Promise.Resolved();
            });
        }

        public static object loadReadyStateFromDB() {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var data = MessengerDBApi.SyncLoadReadyState();
                dispatcher.dispatch(new LoadReadyStateFromDBSuccessAction {data = data});
                return Promise.Resolved();
            });
        }

    }

    public class FetchChannelsSuccessAction : BaseAction {
        public List<string> discoverList;
        public List<string> joinedList;
        public int discoverPage;
        public Dictionary<string, Channel> channelMap;
        public Dictionary<string, bool> joinedChannelMap;
    }

    public class FetchChannelsFailureAction : BaseAction {
    }

    public class StartFetchChannelMessageAction : BaseAction {
    }

    public class FetchChannelMessagesSuccessAction : BaseAction {
        public string channelId;
        public List<ChannelMessage> messages;
        public string before;
        public string after;
        public bool hasMore;
        public bool hasMoreNew;
    }

    public class FetchChannelMessagesFailureAction : BaseAction {
    }

    public class FetchChannelMemberSuccessAction : BaseAction {
        public string channelId;
        public List<ChannelMember> members;
        public int offset;
        public int total;
    }

    public class FetchChannelMemberFailureAction : BaseAction {
    }

    public class StartJoinChannelAction : RequestAction {
        public string channelId;
    }

    public class JoinChannelSuccessAction : BaseAction {
        public string channelId;
    }

    public class JoinChannelFailureAction : BaseAction {
        public string channelId;
    }

    public class LeaveChannelSuccessAction : BaseAction {
        public string channelId;
    }

    public class LeaveChannelFailureAction : BaseAction {
    }

    public class StartSendChannelMessageAction : RequestAction {
        public string channelId;
    }

    public class SendChannelMessageSuccessAction : BaseAction {
        public string channelId;
        public string content;
        public string nonce;
    }

    public class SendChannelMessageFailureAction : BaseAction {
        public string channelId;
    }
    
    public class AckChannelMessageSuccessAction : BaseAction {
    }
    
    public class AckChannelMessageFailureAction : BaseAction {
    }

    public class ClearSentChannelMessage : BaseAction {
        public string channelId;
    }

    public class ClearChannelUnreadAction : BaseAction {
        public string channelId;
    }

    public class ChannelScreenHitBottom : BaseAction {
        public string channelId;
    }

    public class ChannelScreenLeaveBottom : BaseAction {
        public string channelId;
    }

    public class UpdateChannelTopAction : BaseAction {
        public string channelId;
        public bool value;
    }

    public class PushReadyAction : BaseAction {
        public SocketResponseSessionData readyData;
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

    public class PushPresentUpdateAction : BaseAction {
        public SocketResponsePresentUpdateData presentUpdateData;
    }

    public class PushChannelAddMemberAction : BaseAction {
        public SocketResponseChannelMemberChangeData memberData;
    }

    public class PushChannelRemoveMemberAction : BaseAction {
        public SocketResponseChannelMemberChangeData memberData;
    }

    public class SaveMessagesToDBSuccessAction : BaseAction {
        
    }

    public class LoadMessagesFromDBSuccessAction : BaseAction {
        public List<ChannelMessageView> messages;
        public long before;
        public string channelId;
    }

    public class SaveReadyStateToDBSuccessAction : BaseAction {
        
    }

    public class LoadReadyStateFromDBSuccessAction : BaseAction {
        public SocketResponseSessionData data;
    }

    public class MergeNewChannelMessages : BaseAction {
        public string channelId;
    }

    public class MergeOldChannelMessages : BaseAction {
        public string channelId;
    }

    public class SocketConnectStateAction : BaseAction {
        public bool connected;
    }
}