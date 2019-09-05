using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class MainScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            var child = new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new CustomTabBar(
                        new List<Widget> {
                            new ArticlesScreenConnector(),
                            new EventsScreen(),
                            new MessageScreenConnector(),
                            new PersonalScreenConnector()
                        },
                        new List<CustomTabBarItem> {
                            new CustomTabBarItem(
                                0,
                                Icons.UnityTabIcon,
                                Icons.UnityTabIcon,
                                "首页"
                            ),
                            new CustomTabBarItem(
                                1,
                                Icons.outline_event,
                                Icons.eventIcon,
                                "活动"
                            ),
                            new CustomTabBarItem(
                                2,
                                Icons.outline_notification,
                                Icons.notification,
                                "群聊"
                            ),
                            new CustomTabBarItem(
                                3,
                                Icons.mood,
                                Icons.mood,
                                "我的"
                            )
                        },
                        backgroundColor: CColors.TabBarBg,
                        (fromIndex, toIndex) => {
                            AnalyticsManager.ClickHomeTab(fromIndex: fromIndex, toIndex: toIndex);

                            // if (toIndex != 2 || StoreProvider.store.getState().loginState.isLoggedIn) {
                            return true;
                            // }

                            // Router.navigator.pushNamed(routeName: MainNavigatorRoutes.Login);
                            // return false;
                        }
                    )
                )
            );
            return new VersionUpdater(
                child: child
            );
        }
    }
}