using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Plugins;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class MainScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            var child = new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    top: false,
                    bottom: false,
                    child: new CustomTabBarConnector(
                        new List<Widget> {
                            new ArticlesScreenConnector(),
                            new EventsScreenConnector(),
                            new MessengerScreenConnector(),
                            new PersonalScreenConnector()
                        },
                        new List<CustomTabBarItem> {
                            new CustomTabBarItem(
                                0,
                                normalIcon: Icons.tab_home_line,
                                selectedIcon: Icons.tab_home_fill,
                                _getSelectedImages("home"),
                                "首页"
                            ),
                            new CustomTabBarItem(
                                1,
                                normalIcon: Icons.tab_events_line,
                                selectedIcon: Icons.tab_events_fill,
                                _getSelectedImages("event"),
                                "活动"
                            ),
                            new CustomTabBarItem(
                                2,
                                normalIcon: Icons.tab_messenger_line,
                                selectedIcon: Icons.tab_messenger_fill,
                                _getSelectedImages("messenger"),
                                "群聊"
                            ),
                            new CustomTabBarItem(
                                3,
                                normalIcon: Icons.tab_mine_line,
                                selectedIcon: Icons.tab_mine_fill,
                                _getSelectedImages("mine"),
                                "我的"
                            )
                        },
                        backgroundColor: CColors.TabBarBg,
                        (fromIndex, toIndex) => {
                            AnalyticsManager.ClickHomeTab(fromIndex: fromIndex, toIndex: toIndex);

                            if (toIndex != 2 || UserInfoManager.isLogin()) {
                                var myUserId = UserInfoManager.getUserInfo().userId;
                                if (toIndex == 3 && myUserId.isNotEmpty()) {
                                    // mine page
                                    StoreProvider.store.dispatcher.dispatch(Actions.fetchUserProfile(userId: myUserId));
                                }

                                StatusBarManager.statusBarStyle(toIndex == 3 && UserInfoManager.isLogin());
                                StoreProvider.store.dispatcher.dispatch(new SwitchTabBarIndexAction {index = toIndex});
                                JPushPlugin.showPushAlert(toIndex != 2);
                                return true;
                            }

                            Router.navigator.pushNamed(routeName: MainNavigatorRoutes.Login);
                            return false;
                        }
                    )
                )
            );
            return new VersionUpdater(
                child: child
            );
        }

        static List<string> _getSelectedImages(string name) {
            List<string> loadingImages = new List<string>();
            for (int index = 0; index <= 60; index++) {
                loadingImages.Add($"image/tab-loading/{name}-tab-loading/{name}-tab-loading{index}");
            }

            return loadingImages;
        }
    }
}