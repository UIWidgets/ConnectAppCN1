using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    public class MainScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new SafeArea(
                    top: false,
                    child: new CustomTabBar(
                        new List<Widget> {
                            new ArticlesScreenConnector(),
                            new MyEventsScreenConnector(),
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
                                Icons.ievent,
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
                            ),
                        },
                        CColors.White,
                        index => {
//                            Debug.Log($"index == {index}");
                        }
                    )
                )
            );
        }
    }
}