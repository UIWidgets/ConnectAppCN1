using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.animation;
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
        ) : base(key) {
            this.actionModel = actionModel;
        }

        public readonly MyEventsScreenActionModel actionModel;

        public override State createState() {
            return new _MyEventsScreenState();
        }
    }

    class _MyEventsScreenState : State<MyEventsScreen> {
        PageController _pageController;
        int _selectedIndex;

        public override void initState() {
            base.initState();
            this._pageController = new PageController();
            this._selectedIndex = 0;
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        color: CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(context),
                                this._buildSelectView(),
                                this._buildContentView()
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
                                    color: CColors.icon3
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

        Widget _buildSelectView() {
            return new CustomSegmentedControl(
                new List<string> {"即将开始", "往期活动"},
                newValue => {
                    this.setState(() => this._selectedIndex = newValue);
                    this._pageController.animateToPage(
                        newValue,
                        new TimeSpan(0, 0, 0, 0, 250),
                        Curves.ease
                    );
                }, this._selectedIndex
            );
        }

        Widget _buildContentView() {
            return new Flexible(
                child: new Container(
                    child: new PageView(
                        physics: new BouncingScrollPhysics(),
                        controller: this._pageController,
                        onPageChanged: index => { this.setState(() => { this._selectedIndex = index; }); },
                        children: new List<Widget> {
                            new MyFutureEventsScreenConnector(),
                            new MyPastEventsScreenConnector()
                        }
                    )
                )
            );
        }

        public override void dispose() {
            this._pageController.dispose();
            base.dispose();
        }
    }
}