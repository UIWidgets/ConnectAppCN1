using System;
using ConnectApp.Constants;
using ConnectApp.redux.actions;
using Unity.UIWidgets;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.Utils {
    public static class OpenUrlUtil {
        public static void OpenUrl(string url, Dispatcher dispatcher) {
            if (!url.isUrl()) {
                return;
            }

            var uri = new Uri(uriString: url);
            var host_cn = new Uri(uriString: Config.apiAddress_cn).Host;
            var host_com = new Uri(uriString: Config.apiAddress_com).Host;
            if (uri.Host.Equals(value: host_cn) || uri.Host.Equals(value: host_com)) {
                if (uri.AbsolutePath.StartsWith("/p/")) {
                    var articleId = uri.AbsolutePath.Remove(0, "/p/".Length);
                    if (CTemporaryValue.currentPageModelId.isNotEmpty() &&
                        CTemporaryValue.currentPageModelId.Equals(value: articleId)) {
                        return;
                    }

                    dispatcher.dispatch(
                        new MainNavigatorPushToArticleDetailAction {
                            articleId = articleId
                        }
                    );
                }
                else if (uri.AbsolutePath.StartsWith("/u/")) {
                    var userId = uri.AbsolutePath.Remove(0, "/u/".Length);
                    if (CTemporaryValue.currentPageModelId.isNotEmpty() &&
                        CTemporaryValue.currentPageModelId.Equals(value: userId)) {
                        return;
                    }

                    dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {
                        userId = userId
                    });
                }
                else if (uri.AbsolutePath.StartsWith("/t/")) {
                    var teamId = uri.AbsolutePath.Remove(0, "/t/".Length);
                    if (CTemporaryValue.currentPageModelId.isNotEmpty() &&
                        CTemporaryValue.currentPageModelId.Equals(value: teamId)) {
                        return;
                    }

                    dispatcher.dispatch(new MainNavigatorPushToTeamDetailAction {
                        teamId = teamId,
                    });
                }
                else if (uri.AbsolutePath.StartsWith("/channels/")) {
                    var channelId = uri.AbsolutePath.Remove(0, "/channels/".Length);
                    if (CTemporaryValue.currentPageModelId.isNotEmpty() &&
                        CTemporaryValue.currentPageModelId.Equals(value: channelId)) {
                        return;
                    }

                    dispatcher.dispatch(new MainNavigatorPushToChannelShareAction {
                        channelId = channelId
                    });
                }
                else if (uri.AbsolutePath.StartsWith("/mconnect/channels/")) {
                    var channelId = uri.AbsolutePath.Remove(0, "/mconnect/channels/".Length);
                    if (CTemporaryValue.currentPageModelId.isNotEmpty() &&
                        CTemporaryValue.currentPageModelId.Equals(value: channelId)) {
                        return;
                    }

                    dispatcher.dispatch(new MainNavigatorPushToChannelShareAction {
                        channelId = channelId
                    });
                }
                else {
                    dispatcher.dispatch(new MainNavigatorPushToWebViewAction {
                        url = url
                    });
                }
            }
            else {
                // open by default browser
                Application.OpenURL(url: url);
                // dispatcher.dispatch(new MainNavigatorPushToWebViewAction {
                //     url = url
                // });
            }
        }
    }
}