using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class MyEventsScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, object>(
                converter: state => null,
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MyEventsScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction())
                    };
                    return new MyEventsScreen(actionModel: actionModel);
                }
            );
        }
    }

    public class MyEventsScreen : StatefulWidget {
        public MyEventsScreen(
            MyEventsScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.actionModel = actionModel;
        }

        public readonly MyEventsScreenActionModel actionModel;

        public override State createState() {
            return new _MyEventsScreenState();
        }
    }

    class _MyEventsScreenState : State<MyEventsScreen> {
        string _selectValue = "all";

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Expanded(
                                    child: this._buildContentView()
                                )
                            }
                        )
                    )
                )
            );
        }


        Widget _buildNavigationBar() {
            return new CustomNavigationBar(
                new Text(
                    "我的活动",
                    style: CTextStyle.H2
                ),
                onBack: () => this.widget.actionModel.mainRouterPop()
            );
        }

        Widget _buildContentView() {
            var mode = this._selectValue == "all" ? "" : this._selectValue;
            return new CustomSegmentedControl(
                new List<object> {"即将开始", "往期活动"},
                new List<Widget> {
                    new MyFutureEventsScreenConnector(mode: mode),
                    new MyPastEventsScreenConnector(mode: mode)
                },
                newValue => AnalyticsManager.ClickEventSegment(
                    "MineEvent", 0 == newValue ? "ongoing" : "completed"),
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
            );
        }
    }
}