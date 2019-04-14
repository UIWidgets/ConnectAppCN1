using System;
using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class PersonalScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, PersonalScreenViewModel>(
                converter: state => new PersonalScreenViewModel {
                    isLoggedIn = state.loginState.isLoggedIn,
                    userId = state.loginState.loginInfo.userId,
                    userFullName = state.loginState.loginInfo.userFullName,
                    userDict = state.userState.userDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    return new PersonalScreen(
                        viewModel,
                        routeName => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = routeName
                        })
                    );
                }
            );
        }
    }

    public class PersonalScreen : StatefulWidget {
        public PersonalScreen(
            PersonalScreenViewModel viewModel = null,
            Action<string> mainRouterPushTo = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.mainRouterPushTo = mainRouterPushTo;
        }

        public readonly PersonalScreenViewModel viewModel;
        public readonly Action<string> mainRouterPushTo;

        public override State createState() {
            return new _PersonalScreenState();
        }
    }

    public class _PersonalScreenState : State<PersonalScreen> {
        public override Widget build(BuildContext context) {
            var navigationBar = widget.viewModel.isLoggedIn
                ? _buildLoginInNavigationBar()
                : _buildNotLoginInNavigationBar(context);

            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        navigationBar,
                        new CustomDivider(
                            color: CColors.Separator2,
                            height: 1
                        ),
                        new Flexible(
                            child: new Container(
                                padding: EdgeInsets.only(bottom: 49),
                                child: new ListView(
                                    children: _buildItems()
                                )
                            )
                        )
                    }
                )
            );
        }

        private Widget _buildNotLoginInNavigationBar(BuildContext context) {
            return new Container(
                color: CColors.White,
                width: MediaQuery.of(context).size.width,
                height: 196,
                padding: EdgeInsets.only(16, right: 16, bottom: 16),
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text("欢迎来到", style: CTextStyle.H2),
                        new Text("Unity Connect", style: CTextStyle.H2),
                        new Container(
                            margin: EdgeInsets.only(top: 16),
                            child: new CustomButton(
                                padding: EdgeInsets.zero,
                                onPressed: () => widget.mainRouterPushTo(MainNavigatorRoutes.Login),
                                child: new Container(
                                    padding: EdgeInsets.symmetric(horizontal: 24, vertical: 8),
                                    decoration: new BoxDecoration(
                                        border: Border.all(CColors.PrimaryBlue),
                                        borderRadius: BorderRadius.all(20)
                                    ),
                                    child: new Text(
                                        "登录/注册",
                                        style: CTextStyle.PLargeMediumBlue
                                    )
                                )
                            )
                        )
                    }
                )
            );
        }

        private Widget _buildLoginInNavigationBar() {
            return new CustomNavigationBar(
                new Expanded(
                    child: new Text(widget.viewModel.userFullName, style: CTextStyle.H2)
                ),
                new List<Widget> {
                    Avatar.User(widget.viewModel.userId, widget.viewModel.userDict[widget.viewModel.userId], 40)
                },
                CColors.White,
                0
            );
        }

        private List<Widget> _buildItems() {
            var personalCardItems = new List<PersonalCardItem> {
                new PersonalCardItem(
                    Icons.ievent,
                    "我的活动",
                    () => {
                        var routeName = widget.viewModel.isLoggedIn
                            ? MainNavigatorRoutes.MyEvent
                            : MainNavigatorRoutes.Login;
                        widget.mainRouterPushTo(routeName);
                    }
                ),
                new PersonalCardItem(
                    Icons.eye,
                    "浏览历史",
                    () => widget.mainRouterPushTo(MainNavigatorRoutes.History)
                ),
                new PersonalCardItem(
                    Icons.settings,
                    "设置",
                    () => {
                        var routeName = widget.viewModel.isLoggedIn
                            ? MainNavigatorRoutes.Setting
                            : MainNavigatorRoutes.Login;
                        widget.mainRouterPushTo(routeName);
                    }
                )
            };
            var widgets = new List<Widget>();
            personalCardItems.ForEach(item => widgets.Add(new PersonalCard(item)));
            return widgets;
        }
    }
}