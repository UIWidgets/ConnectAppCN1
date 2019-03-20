using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class MainScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new SafeArea(
                top: false,
                child: new CustomTabBar(
                    new List<Widget> {
                        new ArticleScreen(),
                        new EventsScreen(),
                        new NotificationScreen(),
                        new PersonalScreen()
                    },
                    new List<CustomTabBarItem> {
                        new CustomTabBarItem(
                            0,
                            Icons.Description,
                            "文章",
                            CColors.PrimaryBlue,
                            CColors.BrownGrey,
                            24
                        ),
                        new CustomTabBarItem(
                            1,
                            Icons.ievent,
                            "活动",
                            CColors.PrimaryBlue,
                            CColors.BrownGrey,
                            24
                        ),
                        new CustomTabBarItem(
                            2,
                            Icons.Notification,
                            "通知",
                            CColors.PrimaryBlue,
                            CColors.BrownGrey,
                            24
                        ),
                        new CustomTabBarItem(
                            3,
                            Icons.Mood,
                            "我的",
                            CColors.PrimaryBlue,
                            CColors.BrownGrey,
                            24
                        ),
                    },
                    CColors.White
                ));
        }
    }
}