using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class PersonalScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, PersonalScreenViewModel>(
                converter: state => new PersonalScreenViewModel {
                    isLoggedIn = state.loginState.isLoggedIn,
                    user = state.loginState.loginInfo,
                    userDict = state.userState.userDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    return new PersonalScreen(
                        viewModel,
                        routeName => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = routeName
                        }),
                        userId => dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {
                            userId = userId
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
            Action<string> pushToUserDetail = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.mainRouterPushTo = mainRouterPushTo;
            this.pushToUserDetail = pushToUserDetail;
        }

        public readonly PersonalScreenViewModel viewModel;
        public readonly Action<string> mainRouterPushTo;
        public readonly Action<string> pushToUserDetail;

        public override State createState() {
            return new _PersonalScreenState();
        }
    }

    public class _PersonalScreenState : AutomaticKeepAliveClientMixin<PersonalScreen> {
        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override Widget build(BuildContext context) {
            base.build(context);
            var navigationBar = this.widget.viewModel.isLoggedIn
                ? this._buildLoginInNavigationBar()
                : this._buildNotLoginInNavigationBar(context);

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
                                child: new ListView(
                                    children: this._buildItems()
                                )
                            )
                        )
                    }
                )
            );
        }

        Widget _buildNotLoginInNavigationBar(BuildContext context) {
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
                                onPressed: () => this.widget.mainRouterPushTo(MainNavigatorRoutes.Login),
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

        Widget _buildLoginInNavigationBar() {
            var user = this.widget.viewModel.user;
            Widget titleWidget = new Container();
            if (user.title != null && user.title.isNotEmpty()) {
                titleWidget = new Text(
                    user.title,
                    style: new TextStyle(
                        height: 1.46f,
                        fontSize: 14,
                        fontFamily: "Roboto-Regular",
                        color: new Color(0xFFCCCCCC)
                    ),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis
                );
            }
            return new GestureDetector(
                onTap: () => this.widget.pushToUserDetail(user.userId),
                child: new Container(
                    height: 184,
                    padding: EdgeInsets.only(16, right: 16, bottom: 16),
                    color: CColors.Red,
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: new List<Widget> {
                            new Row(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 12),
                                        child: Avatar.User(
                                            user.userId,
                                            new User {
                                                id = user.userId,
                                                avatar = user.userAvatar,
                                                fullName = user.userFullName
                                            },
                                            64
                                        )
                                    ),
                                    new Expanded(
                                        child: new Column(
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: new List<Widget> {
                                                new Text(
                                                    user.userFullName,
                                                    style: CTextStyle.H4White,
                                                    maxLines: 1,
                                                    overflow: TextOverflow.ellipsis
                                                ),
                                                titleWidget
                                            }
                                        )
                                    ),
                                    new Row(
                                        mainAxisSize: MainAxisSize.min,
                                        children: new List<Widget> {
                                            new Text(
                                                "个人主页",
                                                style: new TextStyle(
                                                    fontSize: 14,
                                                    fontFamily: "Roboto-Regular",
                                                    color: new Color(0xFFCCCCCC)
                                                )
                                            ),
                                            new Icon(
                                                Icons.chevron_right,
                                                size: 24,
                                                color: Color.fromRGBO(199, 203, 207, 1)
                                            )
                                        }
                                    )
                                }
                            )
                        }
                    )
                )
            );
        }

        List<Widget> _buildItems() {
            var personalCardItems = new List<PersonalCardItem> {
                new PersonalCardItem(
                    Icons.outline_event,
                    "我的活动",
                    () => {
                        var routeName = this.widget.viewModel.isLoggedIn
                            ? MainNavigatorRoutes.MyEvent
                            : MainNavigatorRoutes.Login;
                        this.widget.mainRouterPushTo(routeName);
                    }
                ),
                new PersonalCardItem(
                    Icons.eye,
                    "浏览历史",
                    () => this.widget.mainRouterPushTo(MainNavigatorRoutes.History)
                ),
                new PersonalCardItem(
                    Icons.settings,
                    "设置",
                    () => {
                        var routeName = this.widget.viewModel.isLoggedIn
                            ? MainNavigatorRoutes.Setting
                            : MainNavigatorRoutes.Login;
                        this.widget.mainRouterPushTo(routeName);
                    }
                )
            };
            var widgets = new List<Widget>();
            personalCardItems.ForEach(item => widgets.Add(new PersonalCard(item)));
            return widgets;
        }
    }
}