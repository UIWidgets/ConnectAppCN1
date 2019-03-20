using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.constants;
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
            return _content(context);
        }

        private Widget _content(BuildContext context) {
            return new Container(
                decoration: new BoxDecoration(
                    CColors.background1
                ),
                child: new Column(
                    children: new List<Widget> {
                        _topView(context),
                        _middleView(),
                        new Flexible(child: new Container()),
                        _bottomView(context)
                    }
                )
            );
        }

        private Widget _topView(BuildContext context) {
            return new Container(
                padding: EdgeInsets.only(20, 8),
                child: new Row(
                    children: new List<Widget> {
                        new CustomButton(
                            onPressed: () => { Router.navigator.pop(context); },
                            child: new Icon(
                                Icons.close,
                                size: 28,
                                color: CColors.icon1
                            )
                        )
                    }
                )
            );
        }

        private Widget _middleView() {
            return new Column(
                children: new List<Widget> {
                    new Container(
                        margin: EdgeInsets.only(top: 100),
                        child: Image.asset(
                            "logo-unity",
                            height: 80,
                            width: 80,
                            fit: BoxFit.fill
                        )
                    ),
                    new Container(height: 40),
                    new Text(
                        "欢迎来到 Unity Connect",
                        maxLines: 1,
                        style: new TextStyle(
                            fontSize: 24,
                            color: CColors.text1
                        )
                    )
                }
            );
        }

        private Widget _bottomView(BuildContext context) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 24),
                child: new Column(
                    children: new List<Widget> {
                        new CustomButton(
                            child: new Container(
                                height: 48,
                                decoration: new BoxDecoration(
                                    CColors.primary,
                                    borderRadius: BorderRadius.all(24)
                                ),
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: new List<Widget> {
                                        Image.asset(
                                            "icon-wechat",
                                            width: 24,
                                            height: 20,
                                            fit: BoxFit.fill
                                        ),
                                        new Container(width: 8),
                                        new Text(
                                            "使用微信账号登陆",
                                            maxLines: 1,
                                            style: new TextStyle(
                                                fontSize: 16,
                                                color: CColors.text1
                                            )
                                        )
                                    })
                            )
                        ),
                        new Container(height: 16),
                        new CustomButton(
                            onPressed: () => Navigator.pushNamed(context, "/bind-unity"),
                            child: new Container(
                                height: 48,
                                decoration: new BoxDecoration(
                                    CColors.background1,
                                    border: Border.all(CColors.White),
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
                                                color: CColors.text1
                                            )
                                        )
                                    })
                            )
                        ),
                        new Container(height: 65)
                    }
                )
            );
        }
    }
}