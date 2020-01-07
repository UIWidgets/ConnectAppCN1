using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class EventsScreen : StatefulWidget {
        public EventsScreen(
            Key key = null
        ) : base(key: key) {
            
        }

        public override State createState() {
            return new _EventsScreenState();
        }
    }

    class _EventsScreenState : AutomaticKeepAliveClientMixin<EventsScreen> {
        string _selectValue = "all";

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            var mode = this._selectValue == "all" ? "" : this._selectValue;
            return new Container(
                padding: EdgeInsets.only(top: CCommonUtils.getSafeAreaTopPadding(context: context)),
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new CustomNavigationBar(
                            new Text("活动", style: CTextStyle.H2)
                        ),
                        new Expanded(
                            child: new CustomSegmentedControl(
                                new List<object> {"即将开始", "往期活动"},
                                new List<Widget> {
                                    new EventOngoingScreenConnector(mode: mode),
                                    new EventCompletedScreenConnector(mode: mode)
                                },
                                newValue => AnalyticsManager.ClickEventSegment("Event",
                                    0 == newValue ? "ongoing" : "completed"),
                                1,
                                trailing: new Container(
                                    padding: EdgeInsets.only(right: 12),
                                    child: new CustomDropdownButton<string>(
                                        value: this._selectValue,
                                        items: new List<CustomDropdownMenuItem<string>> {
                                            new CustomDropdownMenuItem<string>(
                                                value: "all",
                                                child: new Text("全部")
                                            ),
                                            new CustomDropdownMenuItem<string>(
                                                value: "online",
                                                child: new Text("线上")
                                            ),
                                            new CustomDropdownMenuItem<string>(
                                                value: "offline",
                                                child: new Text("线下")
                                            )
                                        },
                                        onChanged: newValue => {
                                            if (this._selectValue != newValue) {
                                                this.setState(() => this._selectValue = newValue);
                                            }
                                        },
                                        headerWidget: new Container(height: 6, color: CColors.White),
                                        footerWidget: new Container(height: 6, color: CColors.White)
                                    )
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}