using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.Plugins;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

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
                        viewModel: viewModel,
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
            base.build(context: context);
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        this.widget.viewModel.isLoggedIn
                            ? this._buildLoginInNavigationBar()
                            : this._buildNotLoginInNavigationBar(context),
                        this.widget.viewModel.isLoggedIn
                            ? (Widget) new Container()
                            : new CustomDivider(
                                color: CColors.Separator2,
                                height: 1
                            ),
                        new Container(height: 16),
                        new Flexible(
                            child: new Column(
                                children: this._buildItems()
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
                                onPressed: () => this.widget.mainRouterPushTo(obj: MainNavigatorRoutes.Login),
                                child: new Container(
                                    padding: EdgeInsets.symmetric(horizontal: 24, vertical: 8),
                                    decoration: new BoxDecoration(
                                        border: Border.all(color: CColors.PrimaryBlue),
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
            var user = this.widget.viewModel.userDict[key: this.widget.viewModel.user.userId];
            Widget titleWidget = new Container();
            if (user.title != null && user.title.isNotEmpty()) {
                titleWidget = new Text(
                    data: user.title,
                    style: CTextStyle.PRegularBody4,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis
                );
            }

            return new GestureDetector(
                onTap: () => this.widget.pushToUserDetail(obj: user.id),
                child: new Container(
                    height: 184,
                    padding: EdgeInsets.only(16, bottom: 16),
                    color: CColors.White,
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: new List<Widget> {
                            new Row(
                                mainAxisAlignment: MainAxisAlignment.end,
                                children: new List<Widget> {
                                    new GestureDetector(
                                        onTap: QRScanPlugin.PushToQRScan,
                                        child: new Container(
                                            padding: EdgeInsets.only(16, 16, 16, 28),
                                            color: CColors.Red,
                                            child: Image.asset(
                                                "image/scan-qr-code",
                                                width: 20,
                                                height: 20
                                            )
                                        )
                                    )
                                }
                            ),
                            new Row(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 12),
                                        child: Avatar.User(
                                            user: user,
                                            64
                                        )
                                    ),
                                    new Expanded(
                                        child: new Column(
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: new List<Widget> {
                                                new Text(
                                                    user.fullName ?? user.name,
                                                    style: CTextStyle.H4,
                                                    maxLines: 1,
                                                    overflow: TextOverflow.ellipsis
                                                ),
                                                titleWidget
                                            }
                                        )
                                    ),
                                    new Container(
                                        padding: EdgeInsets.only(right: 16),
                                        margin: EdgeInsets.only(12),
                                        child: new Row(
                                            mainAxisSize: MainAxisSize.min,
                                            children: new List<Widget> {
                                                new Text(
                                                    "个人主页",
                                                    style: new TextStyle(
                                                        fontSize: 14,
                                                        fontFamily: "Roboto-Regular",
                                                        color: CColors.TextBody4
                                                    )
                                                ),
                                                new Icon(
                                                    icon: Icons.chevron_right,
                                                    size: 24,
                                                    color: Color.fromRGBO(199, 203, 207, 1)
                                                )
                                            }
                                        )
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