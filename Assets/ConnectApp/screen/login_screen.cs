using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Samples.ConnectApp.widgets;
using Unity.UIWidgets.widgets;

namespace Unity.UIWidgets.Samples.ConnectApp {
    public class LoginScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return this._content(context);
        }

        Widget _content(BuildContext context) {
            return new Container(
                decoration: new BoxDecoration(
                    CLColors.background1
                ),
                child: new Column(
                    children: new List<Widget> {
                        this._topView(context),
                        this._middleView(),
                        new Flexible(child: new Container()),
                        this._bottomView(context)
                    }
                )
            );
        }

        Widget _topView(BuildContext context) {
            return new Container(
                padding: EdgeInsets.only(20, 8),
                child: new Row(
                    children: new List<Widget> {
                        new CustomButton(
                            onPressed: () => { Navigator.pop(context); },
                            child: new Icon(
                                Icons.close,
                                size: 28.0,
                                color: CLColors.icon1
                            )
                        )
                    }
                )
            );
        }

        Widget _middleView() {
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
                            color: CLColors.text1
                        )
                    )
                }
            );
        }

        Widget _bottomView(BuildContext context) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 24),
                child: new Column(
                    children: new List<Widget> {
                        new CustomButton(
                            child: new Container(
                                height: 48,
                                decoration: new BoxDecoration(
                                    CLColors.primary,
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
                                                color: CLColors.text1
                                            )
                                        )
                                    })
                            )
                        ),
                        new Container(height: 16),
                        new CustomButton(
                            onPressed: () => Navigator.pushName(context, "/setting-unity"),
                            child: new Container(
                                height: 48,
                                decoration: new BoxDecoration(
                                    CLColors.background1,
                                    border: Border.all(CLColors.white),
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
                                                color: CLColors.text1
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