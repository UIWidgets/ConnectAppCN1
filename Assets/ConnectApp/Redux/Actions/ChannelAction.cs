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
                        dispatcher.dispatch(new FetchArticleFailureAction());
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
}