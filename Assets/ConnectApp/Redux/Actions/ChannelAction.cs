using System;
using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public static partial class Actions {
        public static object fetchChannels(int page, bool fetchMessagesAfterSuccess = false, bool joined = true,
            bool discoverAll = false) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                dispatcher.dispatch(new StartFetchChannelsAction());
                return ChannelApi.FetchChannels(page: page, joined: joined, discoverAll: discoverAll)
                    .Then(channelResponse => {
                        dispatcher.dispatch(new FetchChannelsSuccessAction {
                            discoverList = channelResponse.discoverList ?? new List<string>(),
                            joinedList = channelResponse.joinedList ?? new List<string>(),
                            discoverPage = channelResponse.discoverPage,
                            channelMap = channelResponse.channelMap ?? new Dictionary<string, Channel>(),
                            joinedMemberMap =
                                channelResponse.joinedMemberMap ?? new Dictionary<string, ChannelMember>(),
                            groupMap = channelResponse.groupFullMap ?? new Dictionary<string, Group>(),
                            groupMemberMap = channelResponse.groupMemberMap ?? new Dictionary<string, GroupMember>(),
                            updateJoined = joined,
                            discoverHasMore = channelResponse.discoverHasMore
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
                        Debuger.LogError(message: error);
                        dispatcher.dispatch(loadReadyStateFromDB());
                    });
            });
        }

        public static object fetchCreateChannelFilter() {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchChannels(page: 1, joined: false, discoverAll: true)
                    .Then(channelResponse => {
                        dispatcher.dispatch(new FetchDiscoverChannelFilterSuccessAction {
                            discoverList = channelResponse.discoverList ?? new List<string>()
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchDiscoverChannelFilterFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchStickChannel(string channelId) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog(message: "置顶中"));

            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchStickChannel(channelId: channelId)
                    .Then(stickChannelResponse => {
                        CustomDialogUtils.hiddenCustomDialog();
                        dispatcher.dispatch(new FetchStickChannelSuccessAction {
                            channelId = channelId
                        });
                    })
                    .Catch(error => {
                        CustomDialogUtils.hiddenCustomDialog();
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchUnStickChannel(string channelId) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog(message: "取消置顶中"));

            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchUnStickChannel(channelId: channelId)
                    .Then(unStickChannelResponse => {
                        CustomDialogUtils.hiddenCustomDialog();
                        dispatcher.dispatch(new FetchUnStickChannelSuccessAction {
                            channelId = channelId
                        });
                    })
                    .Catch(error => {
                        CustomDialogUtils.hiddenCustomDialog();
                        Debuger.LogError(message: error);
                    });
            });
        }


        public static object fetchMuteChannel(string channelId) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog(message: "免打扰中"));

            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchMuteChannel(channelId: channelId)
                    .Then(muteChannelResponse => {
                        CustomDialogUtils.hiddenCustomDialog();
                        dispatcher.dispatch(new FetchMuteChannelSuccessAction {
                            channelId = channelId
                        });
                    })
                    .Catch(error => {
                        CustomDialogUtils.hiddenCustomDialog();
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchUnMuteChannel(string channelId) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog(message: "取消免打扰中"));

            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchUnMuteChannel(channelId: channelId)
                    .Then(unMuteChannelResponse => {
                        CustomDialogUtils.hiddenCustomDialog();
                        dispatcher.dispatch(new FetchUnMuteChannelSuccessAction {
                            channelId = channelId
                        });
                    })
                    .Catch(error => {
                        CustomDialogUtils.hiddenCustomDialog();
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchChannelInfo(string channelId, bool isInfoPage = false) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                dispatcher.dispatch(new StartFetchChannelInfoAction {channelId = channelId, isInfoPage = isInfoPage});
                return ChannelApi.FetchChannelInfo(channelId: channelId)
                    .Then(channelInfoResponse => {
                        dispatcher.dispatch(new FetchChannelInfoSuccessAction {
                            channel = channelInfoResponse.channel,
                            isInfoPage = isInfoPage
                        });
                    })
                    .Catch(error => {
                        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(value: error.Message);
                        var errorCode = errorResponse.errorCode;
                        dispatcher.dispatch(new FetchChannelInfoErrorAction {
                            isInfoPage = isInfoPage,
                            channelId = channelId,
                            errorCode = errorCode
                        });
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchChannelMessages(string channelId, string before = null, string after = null) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                dispatcher.dispatch(new StartFetchChannelMessageAction {channelId = channelId});
                return ChannelApi.FetchChannelMessages(channelId: channelId, before: before, after: after)
                    .Then(channelMessagesResponse => {
                        dispatcher.dispatch(new UserLicenseMapAction
                            {userLicenseMap = channelMessagesResponse.userLicenseMap});
                        var userMap = new Dictionary<string, User>();
                        (channelMessagesResponse.items ?? new List<ChannelMessage>()).ForEach(channelMessage => {
                            userMap[key: channelMessage.author.id] = channelMessage.author;
                            (channelMessage.mentions ?? new List<User>()).ForEach(mention => {
                                userMap[key: mention.id] = mention;
                            });
                            (channelMessage.replyUsers ?? new List<User>()).ForEach(replyUser => {
                                userMap[key: replyUser.id] = replyUser;
                            });
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
                                : loadMessagesFromDB(channelId, before.hexToLong()));
                        }
                        catch (Exception e) {
                            Debuger.LogWarning(message: e);
                        }
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchChannelMessagesFailureAction {channelId = channelId});
                        Debuger.LogError(message: error);
                        dispatcher.dispatch(loadMessagesFromDB(channelId, before.hexToLong()));
                    });
            });
        }

        public static object fetchChannelMembers(string channelId, int offset = 0) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var channel = getState().channelState.channelDict.ContainsKey(key: channelId)
                    ? getState().channelState.channelDict[key: channelId]
                    : new ChannelView();
                var memberOffset = (channel.memberIds ?? new List<string>()).Count;
                if (offset != 0 && offset != memberOffset) {
                    offset = memberOffset;
                }

                return ChannelApi.FetchChannelMembers(channelId: channelId, offset: offset)
                    .Then(channelMemberResponse => {
                        dispatcher.dispatch(new FollowMapAction {followMap = channelMemberResponse.followeeMap});
                        var userMap = new Dictionary<string, User>();
                        (channelMemberResponse.list ?? new List<ChannelMember>()).ForEach(member => {
                            userMap[key: member.user.id] = member.user;
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        dispatcher.dispatch(new FetchChannelMembersSuccessAction {
                            channelId = channelId,
                            offset = channelMemberResponse.offset,
                            total = channelMemberResponse.total,
                            members = channelMemberResponse.list
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchChannelMembersFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchChannelMember(string channelId, string userId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchChannelMember(channelId: channelId, userId: userId)
                    .Then(channelMemberResponse => {
                        dispatcher.dispatch(new FetchChannelMemberSuccessAction {
                            channelId = channelId,
                            member = channelMemberResponse.member
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchChannelMemberFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchChannelMentionSuggestions(string channelId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchChannelMemberSuggestions(channelId: channelId)
                    .Then(channelMemberResponse => {
                        dispatcher.dispatch(new FollowMapAction {followMap = channelMemberResponse.followeeMap});
                        var userMap = new Dictionary<string, User>();
                        var channelMemberMap = new Dictionary<string, ChannelMember>();
                        (channelMemberResponse.list ?? new List<ChannelMember>()).ForEach(member => {
                            channelMemberMap[key: member.user.id] = member;
                            userMap[key: member.user.id] = member.user;
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        dispatcher.dispatch(new FetchChannelMentionSuggestionsSuccessAction {
                            channelId = channelId,
                            channelMemberMap = channelMemberMap
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchChannelMentionSuggestionsFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object queryChannelMentionSuggestions(string channelId, string query) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchChannelMemberQuery(channelId: channelId, query: query)
                    .Then(channelMemberResponse => {
                        var searchMembers = channelMemberResponse.searchMembers ?? new List<ChannelMember>();
                        dispatcher.dispatch(new FetchChannelMentionQuerySuccessAction {
                            channelId = channelId,
                            members = searchMembers
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchChannelMentionQueryFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object joinChannel(string channelId, string groupId = null, bool loading = false) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            if (loading) {
                CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog(message: "正在加入群聊"));
            }

            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.JoinChannel(channelId: channelId, groupId: groupId)
                    .Then(joinChannelResponse => {
                        if (loading) {
                            CustomDialogUtils.hiddenCustomDialog();
                        }

                        dispatcher.dispatch(new JoinChannelSuccessAction {
                            channelId = channelId,
                            member = joinChannelResponse.member
                        });
                        dispatcher.dispatch(fetchChannelMessages(channelId: channelId));
                        dispatcher.dispatch(new FetchChannelMemberSuccessAction {
                            channelId = channelId,
                            member = joinChannelResponse.member
                        });
                    })
                    .Catch(error => {
                        if (loading) {
                            CustomDialogUtils.hiddenCustomDialog();
                        }

                        dispatcher.dispatch(new JoinChannelFailureAction {channelId = channelId});
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object leaveChannel(string channelId, string memberId = null, string groupId = null) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog(message: "正在退出群聊"));

            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.LeaveChannel(channelId: channelId, memberId: memberId, groupId: groupId)
                    .Then(leaveChannelResponse => {
                        dispatcher.dispatch(new LeaveChannelSuccessAction {channelId = channelId});
                        CustomDialogUtils.hiddenCustomDialog();
                        dispatcher.dispatch(new MainNavigatorPopAction());
                        dispatcher.dispatch(new MainNavigatorPopAction());
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new LeaveChannelFailureAction());
                        CustomDialogUtils.hiddenCustomDialog();
                        Debuger.LogError(message: error);
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
                            nonce = nonce,
                            isImage = false
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new SendChannelMessageFailureAction {
                            channelId = channelId,
                            messageId = nonce
                        });
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object deleteChannelMessage(string channelId, string messageId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.DeleteChannelMessage(messageId: messageId)
                    .Then(ackMessageResponse => {
                        dispatcher.dispatch(new DeleteChannelMessageSuccessAction {
                            channelId = channelId,
                            messageId = messageId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new DeleteChannelMessageFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object ackChannelMessage(string messageId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.AckChannelMessage(messageId: messageId)
                    .Then(ackMessageResponse => { dispatcher.dispatch(new AckChannelMessageSuccessAction()); })
                    .Catch(error => {
                        dispatcher.dispatch(new AckChannelMessageFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        static object sendAttachment(string channelId, string nonce, byte[] attachmentData, string filename,
            string fileType) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.SendAttachment(channelId: channelId, "", nonce: nonce, attachmentData: attachmentData,
                        filename: filename, fileType: fileType)
                    .Then(responseText => {
                        dispatcher.dispatch(new SendChannelMessageSuccessAction {
                            channelId = channelId,
                            content = "",
                            nonce = nonce,
                            isImage = false
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new SendChannelMessageFailureAction {
                            channelId = channelId,
                            messageId = nonce
                        });
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object sendImage(string channelId, string nonce, byte[] imageData) {
            return sendAttachment(channelId: channelId, nonce: nonce, attachmentData: imageData, "image.png",
                "image/png");
        }

        public static object sendVideo(string channelId, string nonce, byte[] videoData, string fileName) {
            return sendAttachment(channelId: channelId, nonce: nonce, attachmentData: videoData, filename: fileName,
                "video/mp4");
        }

        public static object addReaction(string messageId, string likeEmoji) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.UpdateReaction(messageId: messageId, likeEmoji: likeEmoji)
                    .Then(ackMessageResponse => {
                        dispatcher.dispatch(
                            new AddChannelMessageReactionSuccessAction());
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new AddChannelMessageReactionFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object cancelReaction(string messageId, string type) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.UpdateReaction(messageId: messageId, likeEmoji: type, true)
                    .Then(ackMessageResponse => {
                        dispatcher.dispatch(
                            new CancelChannelMessageReactionSuccessAction());
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new CancelChannelMessageReactionFailureAction());
                        Debuger.LogError(message: error);
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

    public class StartFetchChannelsAction : BaseAction {
    }

    public class FetchChannelsSuccessAction : BaseAction {
        public List<string> discoverList;
        public List<string> joinedList;
        public int discoverPage;
        public Dictionary<string, Channel> channelMap;
        public Dictionary<string, ChannelMember> joinedMemberMap;
        public Dictionary<string, Group> groupMap;
        public Dictionary<string, GroupMember> groupMemberMap;
        public bool updateJoined;
        public bool discoverHasMore;
    }

    public class FetchChannelsFailureAction : BaseAction {
    }

    public class FetchDiscoverChannelFilterSuccessAction : BaseAction {
        public List<string> discoverList;
    }

    public class FetchDiscoverChannelFilterFailureAction : BaseAction {
    }

    public class FetchStickChannelSuccessAction : BaseAction {
        public string channelId;
    }

    public class FetchUnStickChannelSuccessAction : BaseAction {
        public string channelId;
    }

    public class FetchMuteChannelSuccessAction : BaseAction {
        public string channelId;
    }

    public class FetchUnMuteChannelSuccessAction : BaseAction {
        public string channelId;
    }

    public class StartFetchChannelInfoAction : BaseAction {
        public string channelId;
        public bool isInfoPage = false;
    }

    public class FetchChannelInfoSuccessAction : BaseAction {
        public Channel channel;
        public bool isInfoPage = false;
    }

    public class FetchChannelInfoErrorAction : BaseAction {
        public bool isInfoPage = false;
        public string channelId;
        public string errorCode;
    }

    public class StartFetchChannelMessageAction : BaseAction {
        public string channelId;
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
        public string channelId;
    }

    public class FetchChannelMembersSuccessAction : BaseAction {
        public string channelId;
        public List<ChannelMember> members;
        public int offset;
        public int total;
    }

    public class FetchChannelMembersFailureAction : BaseAction {
    }

    public class FetchChannelMemberSuccessAction : BaseAction {
        public string channelId;
        public ChannelMember member;
    }

    public class FetchChannelMemberFailureAction : BaseAction {
    }

    public class StartJoinChannelAction : RequestAction {
        public string channelId;
    }

    public class JoinChannelSuccessAction : BaseAction {
        public string channelId;
        public ChannelMember member;
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
        public ChannelMessageView message;
    }

    public class SendChannelMessageSuccessAction : BaseAction {
        public string channelId;
        public string content;
        public string nonce;
        public bool isImage;
    }

    public class SendChannelMessageFailureAction : BaseAction {
        public string channelId;
        public string messageId;
    }

    public class DeleteLocalMessageAction : BaseAction {
        public ChannelMessageView message;
    }

    public class DeleteChannelMessageSuccessAction : BaseAction {
        public string channelId;
        public string messageId;
    }

    public class DeleteChannelMessageFailureAction : BaseAction {
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

    public class SetChannelInactive : BaseAction {
        public string channelId;
    }

    public class AddChannelMessageReactionSuccessAction : BaseAction {
    }

    public class AddChannelMessageReactionFailureAction : BaseAction {
        public string messageId;
    }

    public class CancelChannelMessageReactionSuccessAction : BaseAction {
    }

    public class CancelChannelMessageReactionFailureAction : BaseAction {
        public string messageId;
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

    public class PushChannelCreateChannelAction : BaseAction {
        public SocketResponseUpdateChannelData channelData;
    }

    public class PushChannelDeleteChannelAction : BaseAction {
        public SocketResponseUpdateChannelData channelData;
    }

    public class PushChannelUpdateChannelAction : BaseAction {
        public SocketResponseUpdateChannelData channelData;
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

    public class NetworkAvailableStateAction : BaseAction {
        public bool available;
    }

    public class DismissNoNetworkBannerAction : BaseAction {
        public bool isDismiss;
    }

    public class ChannelChooseMentionConfirmAction : BaseAction {
        public string channelId;
        public string mentionUserId;
        public string mentionUserName;
        public ChannelMember member;
    }

    public class ChannelChooseMentionCancelAction : BaseAction {
    }

    public class ChannelClearMentionAction : BaseAction {
    }

    public class StartFetchChannelMentionSuggestionAction : BaseAction {
    }

    public class StartSearchChannelMentionSuggestionAction : BaseAction {
    }

    public class FetchChannelMentionSuggestionsSuccessAction : BaseAction {
        public string channelId;
        public Dictionary<string, ChannelMember> channelMemberMap;
    }

    public class FetchChannelMentionSuggestionsFailureAction : BaseAction {
    }

    public class UpdateNewNotificationAction : BaseAction {
        public string notification;
    }

    public class PushChannelMessageAckAction : BaseAction {
        public SocketResponseMessageAckData ackData;
    }

    public class AddLocalMessageAction : BaseAction {
        public ChannelMessageView message;
    }

    public class ResendMessageAction : BaseAction {
        public ChannelMessageView message;
    }

    public class UpdateMyReactionToMessage : BaseAction {
        public string messageId;
    }

    public class FetchChannelMentionQuerySuccessAction : BaseAction {
        public string channelId;
        public List<ChannelMember> members;
    }

    public class FetchChannelMentionQueryFailureAction : BaseAction {
    }

    public class ChannelUpdateMentionQueryAction : BaseAction {
        public string mentionQuery;
    }

    public class ChannelClearMentionQueryAction : BaseAction {
    }
}