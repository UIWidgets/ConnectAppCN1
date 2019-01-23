using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Samples.ConnectApp.widgets;
using Unity.UIWidgets.widgets;

namespace Unity.UIWidgets.Samples.ConnectApp {
    public class BindUnityScreen : StatelessWidget {
        private readonly FocusNode _emailFocusNode = new FocusNode();
        private readonly FocusNode _passwordFocusNode = new FocusNode();

        public override Widget build(BuildContext context) {
            return _content(context);
        }

        private Widget _content(BuildContext context) {
            return new Container(
                decoration: new BoxDecoration(
                    CLColors.background1
                ),
                child: new Column(
                    children: new List<Widget> {
                        _topView(context),
                        _middleView(),
                        _bottomView()
                    }
                )
            );
        }

        private Widget _topView(BuildContext context) {
            return new Column(
                children: new List<Widget> {
                    new Container(
                        height: 44,
                        padding: EdgeInsets.symmetric(horizontal: 16),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget> {
                                new CustomButton(
                                    onPressed: () => Navigator.pop(context),
                                    child: new Text(
                                        "跳过",
                                        style: new TextStyle(
                                            fontSize: 17,
                                            color: CLColors.text2
                                        )
                                    )
                                ),
                                new CustomButton(
                                    child: new Text(
                                        "创建 Unity ID",
                                        style: new TextStyle(
                                            fontSize: 17,
                                            color: CLColors.text2
                                        )
                                    )
                                )
                            }
                        )
                    ),
                    new Container(height: 30),
                    new Row(
                        children: new List<Widget> {
                            new Container(
                                padding: EdgeInsets.symmetric(horizontal: 24),
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Text(
                                            "绑定你的 Unity 账号",
                                            style: new TextStyle(
                                                fontSize: 28,
                                                color: CLColors.text1
                                            )
                                        ),
                                        new Container(height: 8),
                                        new Text(
                                            "绑定账号可对直播进行评论",
                                            style: new TextStyle(
                                                fontSize: 14,
                                                color: CLColors.text2
                                            )
                                        )
                                    }
                                )
                            ),
                            new Container()
                        }
                    )
                }
            );
        }

        private Widget _middleView() {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 24),
                child: new Column(
                    children: new List<Widget> {
                        new Container(height: 42),
                        new Container(
                            height: 44,
                            decoration: new BoxDecoration(
                                border: new Border(
                                    new BorderSide(CLColors.transparent),
                                    new BorderSide(CLColors.transparent),
                                    new BorderSide(CLColors.dividingLine1),
                                    new BorderSide(CLColors.transparent)
                                )
                            ),
                            alignment: Alignment.center,
                            child: new InputField(
                                focusNode: _emailFocusNode,
                                maxLines: 1,
                                autofocus: false,
                                style: new TextStyle(
                                    fontSize: 16,
                                    color: CLColors.text1
                                ),
                                hintText: "邮箱",
                                hintStyle: new TextStyle(
                                    fontSize: 16,
                                    color: CLColors.text2
                                ),
                                labelStyle: new TextStyle(
                                    fontSize: 16,
                                    color: CLColors.text2
                                ),
                                cursorColor: CLColors.primary
                            )
                        ),
                        new Container(height: 42),
                        new Container(
                            height: 44,
                            decoration: new BoxDecoration(
                                border: new Border(
                                    new BorderSide(CLColors.transparent),
                                    new BorderSide(CLColors.transparent),
                                    new BorderSide(CLColors.dividingLine1),
                                    new BorderSide(CLColors.transparent)
                                )
                            ),
                            alignment: Alignment.center,
                            child: new InputField(
                                focusNode: _passwordFocusNode,
                                maxLines: 1,
                                autofocus: false,
                                obscureText: true,
                                style: new TextStyle(
                                    fontSize: 16,
                                    color: CLColors.text1
                                ),
                                hintText: "密码",
                                hintStyle: new TextStyle(
                                    fontSize: 16,
                                    color: CLColors.text2
                                ),
                                labelStyle: new TextStyle(
                                    fontSize: 16,
                                    color: CLColors.text2
                                ),
                                cursorColor: CLColors.primary
                            )
                        )
                    }
                )
            );
        }

        private Widget _bottomView() {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 24),
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
                                        new Text(
                                            "确定",
                                            maxLines: 1,
                                            style: new TextStyle(
                                                fontSize: 16,
                                                color: CLColors.text1
                                            )
                                        )
                                    })
                            )
                        ),
                        new Container(height: 8),
                        new CustomButton(
                            child: new Text(
                                "忘记密码",
                                style: new TextStyle(
                                    fontSize: 16,
                                    color: CLColors.text2
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}