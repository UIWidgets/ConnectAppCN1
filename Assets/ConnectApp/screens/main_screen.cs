using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens
{
    public class MainScreen : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                child: new CustomTabBar(
                    new List<Widget>
                    {
                        new EventsScreen(),
                        new LoginScreen(),
                        new MineScreen()
                    },
                    new List<CustomTabBarItem>
                    {
                        new CustomTabBarItem(
                            0,
                            Icons.TabHome,
                            "首页",
                            CColors.primaryBlue,
                            CColors.brownGrey,
                            24
                        ),
                        new CustomTabBarItem(
                            1,
                            Icons.TabNotification,
                            "消息",
                            CColors.primaryBlue,
                            CColors.brownGrey,
                            24
                        ),
                        new CustomTabBarItem(
                            2,
                            Icons.TabMood,
                            "我的",
                            CColors.primaryBlue,
                            CColors.brownGrey,
                            24
                        ),
                    },
                    CColors.white
                ));
        }
    }
}