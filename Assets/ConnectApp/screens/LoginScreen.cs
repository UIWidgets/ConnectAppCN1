using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class LoginScreen : StatelessWidget {
        private static readonly GlobalKey globalKey = GlobalKey.key(debugLabel: "login-router");
        public static NavigatorState navigator => globalKey.currentState as NavigatorState;

        private static Dictionary<string, WidgetBuilder> loginRoutes => new Dictionary<string, WidgetBuilder> {
            {"/", context => new LoginSwitchScreen()},
            {"/bind-unity", context => new BindUnityScreen()}
        };

        public override Widget build(BuildContext context) {
            return new Navigator(
                globalKey,
                onGenerateRoute: (RouteSettings settings) => {
                    return new PageRouteBuilder(
                        settings,
                        (context1, animation, secondaryAnimation) => loginRoutes[settings.name](context1),
                        (context1, animation, secondaryAnimation, child) => new PushPageTransition(
                            routeAnimation: animation,
                            child: child
                        ));
                }
            );
        }
    }

    public class LoginSwitchScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new SafeArea(
                    child: _buildContent(context)
                )
            );
        }

        private static Widget _buildContent(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        _buildTopView(context),
                        _buildMiddleView(context),
                        new Flexible(child: new Container()),
                        _buildBottomView(context)
                    }
                )
            );
        }

        private static Widget _buildTopView(BuildContext context) {
            return new Container(
                height: 44,
                padding: EdgeInsets.only(8, 8),
                child: new Row(
                    children: new List<Widget> {
                        new CustomButton(
                            onPressed: () => Navigator.pop(context),
                            child: new Icon(
                                Icons.close,
                                size: 28,
                                color: CColors.icon3
                            )
                        )
                    }
                )
            );
        }

        private static Widget _buildMiddleView(BuildContext context) {
            var mediaQuery = MediaQuery.of(context);
            var height = mediaQuery.size.height - mediaQuery.padding.top - -mediaQuery.padding.bottom;
            return new Column(
                children: new List<Widget> {
                    new Container(
                        margin: EdgeInsets.only(top: height * 0.25f - 44),
                        child: Image.asset(
                            "black-logo-unity",
                            height: 80,
                            width: 80,
                            fit: BoxFit.fill
                        )
                    ),
                    new Container(height: 40),
                    new Text(
                        "欢迎来到 Unity Connect",
                        maxLines: 1,
                        style: CTextStyle.H4
                    )
                }
            );
        }

        private static Widget _buildBottomView(BuildContext context) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    children: new List<Widget> {
                        new CustomButton(
                            onPressed: () => {
                                StoreProvider.store.Dispatch(new NavigatorToLoginAction {fromPage = FromPage.weChat});
                                Navigator.pushNamed(context, "/bind-unity");
                            },
                            padding: EdgeInsets.zero,
                            child: new Container(
                                height: 48,
                                decoration: new BoxDecoration(
                                    CColors.PrimaryBlue,
                                    borderRadius: BorderRadius.all(24)
                                ),
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: new List<Widget> {
                                        Image.asset(
                                            "icon-wechat",
                                            width: 24,
                                            height: 24,
                                            fit: BoxFit.fill
                                        ),
                                        new Container(width: 8),
                                        new Text(
                                            "使用微信账号登陆",
                                            maxLines: 1,
                                            style: new TextStyle(
                                                fontSize: 16,
                                                fontFamily: "PingFang-Regular",
                                                color: CColors.White
                                            )
                                        )
                                    }
                                )
                            )
                        ),
                        new Container(height: 16),
                        new CustomButton(
                            onPressed: () => {
                                StoreProvider.store.Dispatch(new NavigatorToLoginAction {fromPage = FromPage.login});
                                Navigator.pushNamed(context, "/bind-unity");
                            },
                            padding: EdgeInsets.zero,
                            child: new Container(
                                height: 48,
                                decoration: new BoxDecoration(
                                    CColors.White,
                                    border: Border.all(CColors.PrimaryBlue),
                                    borderRadius: BorderRadius.all(24)
                                ),
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: new List<Widget> {
                                        new Text(
                                            "使用 Unity ID 登录",
                                            maxLines: 1,
                                            style: new TextStyle(
                                                fontSize: 16,
                                                fontFamily: "PingFang-Regular",
                                                color: CColors.PrimaryBlue
                                            )
                                        )
                                    }
                                )
                            )
                        ),
                        new Container(
                            height: 65 + MediaQuery.of(context).padding.bottom
                        )
                    }
                )
            );
        }
    }
}