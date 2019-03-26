using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class PersonalScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new StoreConnector<AppState, LoginState>(
                    converter: (state, dispatch) => state.loginState,
                    builder: (_context, viewModel) => {
                        var isLoggedIn = viewModel.isLoggedIn;
                        var navigationBar = isLoggedIn
                            ? _buildLoginInNavigationBar(viewModel)
                            : _buildNotLoginInNavigationBar(context);

                        return new Column(
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
                                            children: _buildItems(context)
                                        )
                                    )
                                )
                            }
                        );
                    }
                )
            );
        }

        private static Widget _buildNotLoginInNavigationBar(BuildContext context) {
            return new Container(
                color: CColors.White,
                width: MediaQuery.of(context).size.width,
                height: 240,
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
                                onPressed: () => { Router.navigator.pushNamed("/login"); },
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

        private static Widget _buildLoginInNavigationBar(LoginState loginState) {
            var loginInfo = loginState.loginInfo;
            return new CustomNavigationBar(
                new Expanded(
                    child: new Text(loginInfo.userFullName, style: CTextStyle.H2)
                ),
                new List<Widget> {
                    new Avatar(loginInfo.userId,40)
                },
                CColors.White,
                0
            );
        }

        private static List<Widget> _buildItems(BuildContext context) {
            List<PersonalCardItem> personalCardItems = new List<PersonalCardItem> {
//                new PersonalCardItem(Icons.book, "我的收藏", () => {
//                    var isLoginIn = StoreProvider.store.state.loginState.isLoggedIn;
//                    if (isLoginIn) {
//                    }
//                    else {
//                        Navigator.pushNamed(context, "/login");
//                    }
//                }),
                new PersonalCardItem(Icons.ievent, "我的活动", () => {
                    var isLoginIn = StoreProvider.store.state.loginState.isLoggedIn;
                    var routeName = isLoginIn ? "/my-event" : "/login";
                    Router.navigator.pushNamed(routeName);
                }),
                new PersonalCardItem(Icons.eye, "浏览历史", () => Router.navigator.pushNamed("/history")),
                new PersonalCardItem(Icons.settings, "设置", () => {
                    var isLoginIn = StoreProvider.store.state.loginState.isLoggedIn;
                    var routeName = isLoginIn ? "/setting" : "/login";
                    Router.navigator.pushNamed(routeName);
                })
            };
            List<Widget> widgets = new List<Widget>();
            personalCardItems.ForEach(item => widgets.Add(new PersonalCard(item)));
            return widgets;
        }
    }
}