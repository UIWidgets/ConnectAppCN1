using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
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
                                    child: _buildContentView()
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

        static Widget _buildContentView() {
            return new CustomSegmentedControl(
                new List<object> {"即将开始", "往期活动"},
                new List<Widget> {
                    new MyFutureEventsScreenConnector(),
                    new MyPastEventsScreenConnector()
                },
                newValue => AnalyticsManager.ClickEventSegment(
                    "MineEvent", 0 == newValue ? "ongoing" : "completed")
            );
        }
    }
}