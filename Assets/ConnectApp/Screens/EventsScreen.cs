using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class EventsScreen : StatefulWidget {
        public override State createState() {
            return new _EventsScreenState();
        }
    }

    class _EventsScreenState : AutomaticKeepAliveClientMixin<EventsScreen> {
        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new CustomNavigationBar(
                            new Text("活动", style: CTextStyle.H2),
                            null,
                            backgroundColor: CColors.White,
                            0
                        ),
                        new Expanded(
                            child: new CustomSegmentedControl(
                                new List<string> {"即将开始", "往期活动"},
                                new List<Widget> {
                                    new EventOngoingScreenConnector(),
                                    new EventCompletedScreenConnector()
                                },
                                newValue => AnalyticsManager.ClickEventSegment("Event", 0 == newValue ? "ongoing" : "completed"),
                                1
                            )
                        )
                    }
                )
            );
        }
    }
}