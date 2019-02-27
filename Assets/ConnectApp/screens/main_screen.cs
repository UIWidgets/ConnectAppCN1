using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class MainScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                child: new CustomTabBar(
                    new List<Widget> {
                        new EventsScreen(),
                        new LoginScreen(),
                        new MineScreen()
                    },
                    new List<CustomTabBarItem> {
                        new CustomTabBarItem(
                            0,
                            Icons.Description,
                            "首页",
                            CColors.PrimaryBlue,
                            CColors.BrownGrey,
                            24
                        ),
                        new CustomTabBarItem(
                            1,
                            Icons.Notification,
                            "消息",
                            CColors.PrimaryBlue,
                            CColors.BrownGrey,
                            24
                        ),
                        new CustomTabBarItem(
                            2,
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