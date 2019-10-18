using System;
using ConnectApp.redux.actions;
using Unity.UIWidgets;

namespace ConnectApp.Utils {
    public static class OpenUrlUtil {
        public static void OpenUrl(string url, Dispatcher dispatcher) {
            if (url.isUrl()) {
                var uri = new Uri(url);
                if ((uri.Host.Equals("connect.unity.com") || uri.Host.Equals("connect-test.unity.com")) &&
                    uri.AbsolutePath.StartsWith("/p/")) {
                    var articleId = uri.AbsolutePath.Remove(0, "/p/".Length);
                    dispatcher.dispatch(
                        new MainNavigatorPushToArticleDetailAction {
                            articleId = articleId
                        }
                    );
                }
                else if ((uri.Host.Equals("connect.unity.com") || uri.Host.Equals("connect-test.unity.com")) &&
                         uri.AbsolutePath.StartsWith("/u/")) {
                    var userId = uri.AbsolutePath.Remove(0, "/u/".Length);
                    dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {
                        userId = userId,
                        isSlug = true
                    });
                }
                else if ((uri.Host.Equals("connect.unity.com") || uri.Host.Equals("connect-test.unity.com")) &&
                         uri.AbsolutePath.StartsWith("/t/")) {
                    var teamId = uri.AbsolutePath.Remove(0, "/t/".Length);
                    dispatcher.dispatch(new MainNavigatorPushToTeamDetailAction {
                        teamId = teamId,
                        isSlug = true
                    });
                }
                else {
                    dispatcher.dispatch(new MainNavigatorPushToWebViewAction {
                        url = url
                    });
                }
            }
        }
    }
}