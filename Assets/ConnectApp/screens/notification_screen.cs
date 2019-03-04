using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class NotificationScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Stack(
                    children: new List<Widget> {
                        new Positioned(
                            top: 0,
                            left: 0,
                            right: 0,
                            child: new CustomNavigationBar(new Text("通知", style: CTextStyle.H2), new List<Widget>
                                { }, CColors.White, 0)
                        ),
                        new Container(
                            padding: EdgeInsets.only(0, 140, 0, 49),
                            child: new ListView(
                                scrollDirection: Axis.vertical,
                                children: new List<Widget> {
                                    new NotificationCard(),
                                    new NotificationCard(),
                                    new NotificationCard(),
                                    new NotificationCard(),
                                    new NotificationCard(),
                                    new NotificationCard(),
                                    new NotificationCard(),
                                    new NotificationCard(),
                                    new NotificationCard()
                                }
                            )
                        )
                    }
                )
            );
        }
    }
}