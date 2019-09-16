using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
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

        public static object fetchChannelMessages(string channelId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ChannelApi.FetchChannelMessages(channelId).Then(channelMessagesResponse => {
                        dispatcher.dispatch(new ChannelMessagesAction {
                            channelId = channelId,
                            messages = channelMessagesResponse.items,
                            currentPage = channelMessagesResponse.currentPage,
                            pages = channelMessagesResponse.pages,
                            total = channelMessagesResponse.total
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

    public class ChannelMessagesAction {
        public string channelId;
        public List<ChannelMessage> messages;
        public int currentPage;
        public List<int> pages;
        public int total;
    }

    public class ChannelMemberAction {
        public string channelId;
        public List<ChannelMember> members;
    }
    
    public class FetchPublicChannelsSuccessAction : BaseAction {
    }
    
    public class FetchPublicChannelsFailureAction : BaseAction {
    }
    
    public class FetchChannelMessagesSuccessAction : BaseAction {
    }
    
    public class FetchChannelMessagesFailureAction : BaseAction {
    }
    
    public class FetchChannelMemberSuccessAction : BaseAction {
    }
    
    public class FetchChannelMemberFailureAction : BaseAction {
    }
}