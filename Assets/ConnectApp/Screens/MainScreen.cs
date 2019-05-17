using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.Components;
using ConnectApp.constants;
using ConnectApp.redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class MainScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
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
                                Icons.Description,
                                "文章"
                            ),
                            new CustomTabBarItem(
                                1,
                                Icons.IEvent,
                                "活动"
                            ),
                            new CustomTabBarItem(
                                2,
                                Icons.Notification,
                                "通知"
                            ),
                            new CustomTabBarItem(
                                3,
                                Icons.Mood,
                                "我的"
                            )
                        },
                        backgroundColor: CColors.White,
                        index => {
                            if (index != 2 || StoreProvider.store.getState().loginState.isLoggedIn) {
                                return true;
                            }
                            Navigator.pushNamed(context: context, routeName: MainNavigatorRoutes.Login);
                            return false;
                        }
                    )
                )
            );
        }
    }
}