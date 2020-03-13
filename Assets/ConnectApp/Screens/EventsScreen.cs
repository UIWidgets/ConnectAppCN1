using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class EventsScreenConnector : StatelessWidget {
        public EventsScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, EventsScreenViewModel>(
                converter: state => new EventsScreenViewModel {
                    currentTabBarIndex = state.tabBarState.currentTabIndex
                },
                builder: (context1, viewModel, dispatcher) =>
                    new EventsScreen(viewModel: viewModel));
        }
    }


    public class EventsScreen : StatefulWidget {
        public EventsScreen(
            EventsScreenViewModel viewModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
        }

        public readonly EventsScreenViewModel viewModel;

        public override State createState() {
            return new _EventsScreenState();
        }
    }

    class _EventsScreenState : AutomaticKeepAliveClientMixin<EventsScreen>, RouteAware {
        string _selectValue = "all";

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            base.dispose();
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

        public void didPopNext() {
            if (this.widget.viewModel.currentTabBarIndex == 1) {
                StatusBarManager.statusBarStyle(false);
            }
        }

        public void didPush() {
        }

        public void didPop() {
        }

        public void didPushNext() {
        }
    }
}