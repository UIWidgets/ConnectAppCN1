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
using Unity.UIWidgets.rendering;
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
                    return new MyEventsScreen(actionModel);
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
                                this._buildNavigationBar(context),
                                new Expanded(
                                    child: _buildContentView()
                                )
                            }
                        )
                    )
                )
            );
        }


        Widget _buildNavigationBar(BuildContext context) {
            return new Container(
                decoration: new BoxDecoration(CColors.White),
                width: MediaQuery.of(context).size.width,
                height: 96,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            child: new CustomButton(
                                padding: EdgeInsets.only(16, 10, 16),
                                onPressed: () => this.widget.actionModel.mainRouterPop(),
                                child: new Icon(
                                    Icons.arrow_back,
                                    size: 24,
                                    color: CColors.Icon
                                )
                            ),
                            height: 44
                        ),
                        new Container(
                            margin: EdgeInsets.only(16, bottom: 12),
                            child: new Text(
                                "我的活动",
                                style: CTextStyle.H2
                            )
                        )
                    }
                )
            );
        }

        static Widget _buildContentView() {
            return new CustomSegmentedControl(
                new List<string> {"即将开始", "往期活动"},
                new List<Widget> {
                    new MyFutureEventsScreenConnector(),
                    new MyPastEventsScreenConnector()
                },
                newValue => AnalyticsManager.ClickEventSegment("MineEvent", 0 == newValue ? "ongoing" : "completed")
            );
        }
    }
}