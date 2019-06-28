using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.redux;
using ConnectApp.Utils;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class MainScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            var child = new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new CustomTabBar(
                        new List<Widget> {
                            new ArticlesScreenConnector(),
                            new EventsScreen(),
                            new NotificationScreenConnector(),
                            new PersonalScreenConnector()
                        },
                        new List<CustomTabBarItem> {
                            new CustomTabBarItem(
                                0,
                                Icons.outline_description,
                                "文章"
                            ),
                            new CustomTabBarItem(
                                1,
                                Icons.outline_event,
                                "活动"
                            ),
                            new CustomTabBarItem(
                                2,
                                Icons.outline_notification,
                                "通知"
                            ),
                            new CustomTabBarItem(
                                3,
                                Icons.Mood,
                                "我的"
                            )
                        },
                        backgroundColor: CColors.White,
                        (fromIndex, toIndex) => {
                            AnalyticsManager.ClickHomeTab(fromIndex, toIndex);

                            if (toIndex != 2 || StoreProvider.store.getState().loginState.isLoggedIn) {
                                return true;
                            }

                            Router.navigator.pushNamed(routeName: MainNavigatorRoutes.Login);
                            return false;
                        }
                    )
                )
            );
            return new VersionUpdater(
                child
            );
        }
    }
}