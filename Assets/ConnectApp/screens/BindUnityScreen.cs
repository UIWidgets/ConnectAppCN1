using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    public class BindUnityScreen : StatelessWidget {
        private readonly FocusNode _emailFocusNode = new FocusNode();
        private readonly FocusNode _passwordFocusNode = new FocusNode();

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
                        _middleView(context),
                        _bottomView(context)
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
                                    onPressed: () => {
                                        Navigator.pop(context);
                                    },
                                    child: new Text(
                                        "跳过",
                                        style: new TextStyle(
                                            fontSize: 17,
                                            color: CColors.text2
                                        )
                                    )
                                ),
                                new CustomButton(
                                    child: new Text(
                                        "创建 Unity ID",
                                        style: new TextStyle(
                                            fontSize: 17,
                                            color: CColors.text2
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
                                                color: CColors.text1
                                            )
                                        ),
                                        new Container(height: 8),
                                        new Text(
                                            "绑定账号可对直播进行评论",
                                            style: new TextStyle(
                                                fontSize: 14,
                                                color: CColors.text2
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

        private Widget _middleView(BuildContext context) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 24),
                child: new Column(
                    children: new List<Widget> {
                        new Container(height: 42),
                        new Container(
                            height: 64,
                            decoration: new BoxDecoration(
                                CColors.Transparent,
                                border: new Border(
                                    new BorderSide(CColors.Transparent),
                                    new BorderSide(CColors.Transparent),
                                    new BorderSide(CColors.dividingLine1),
                                    new BorderSide(CColors.Transparent)
                                )
                            ),
                            alignment: Alignment.center,
                            child: new StoreConnector<AppState, bool>(
                                converter: (state, dispatch) => state.loginState.loading,
                                builder: (context1, loading) => {
                                    return new IgnorePointer(
                                        ignoring: loading,
                                        child: new InputField(
                                            focusNode: _emailFocusNode,
                                            maxLines: 1,
                                            autofocus: false,
                                            style: new TextStyle(
                                                fontSize: 16,
                                                color: CColors.text1
                                            ),
                                            hintText: "邮箱",
                                            hintStyle: new TextStyle(
                                                fontSize: 16,
                                                color: CColors.text2
                                            ),
                                            labelText: "邮箱",
                                            labelStyle: new TextStyle(
                                                fontSize: 16,
                                                color: CColors.text2
                                            ),
                                            cursorColor: CColors.primary,
                                            onChanged: (text) => {
                                                StoreProvider.store.Dispatch(new LoginChangeEmailAction
                                                    {changeText = text});
                                            },
                                            onSubmitted: (text) => {
                                                _emailFocusNode.unfocus();
                                                FocusScope.of(context).requestFocus(_passwordFocusNode);
                                            }
                                        )
                                    );
                                }
                            )
                        ),
                        new Container(height: 42),
                        new Container(
                            height: 64,
                            decoration: new BoxDecoration(
                                CColors.Transparent,
                                border: new Border(
                                    new BorderSide(CColors.Transparent),
                                    new BorderSide(CColors.Transparent),
                                    new BorderSide(CColors.dividingLine1),
                                    new BorderSide(CColors.Transparent)
                                )
                            ),
                            alignment: Alignment.center,
                            child: new StoreConnector<AppState, bool>(
                                converter: (state, dispatch) => state.loginState.loading,
                                builder: (context1, loading) => {
                                    return new IgnorePointer(
                                        ignoring: loading,
                                        child: new InputField(
                                            focusNode: _passwordFocusNode,
                                            maxLines: 1,
                                            autofocus: false,
                                            obscureText: true,
                                            style: new TextStyle(
                                                fontSize: 16,
                                                color: CColors.text1
                                            ),
                                            hintText: "密码",
                                            hintStyle: new TextStyle(
                                                fontSize: 16,
                                                color: CColors.text2
                                            ),
                                            labelText: "密码",
                                            labelStyle: new TextStyle(
                                                fontSize: 16,
                                                color: CColors.text2
                                            ),
                                            cursorColor: CColors.primary,
                                            onChanged: (text) => {
                                                StoreProvider.store.Dispatch(new LoginChangePasswordAction
                                                    {changeText = text});
                                            },
                                            onSubmitted: (text) => {
                                                _emailFocusNode.unfocus();
                                                _passwordFocusNode.unfocus();
                                                StoreProvider.store.Dispatch(new LoginByEmailAction{context = context});
                                            }
                                        )
                                    );
                                }
                            )
                        )
                    }
                )
            );
        }

        private Widget _bottomView(BuildContext context) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 24),
                        new CustomButton(
                            onPressed: () => {
                                _emailFocusNode.unfocus();
                                _passwordFocusNode.unfocus();
                                StoreProvider.store.Dispatch(new LoginByEmailAction{context = context});
                            },
                            child: new StoreConnector<AppState, Dictionary<string, object>>(
                                converter: (state, dispatch) => new Dictionary<string, object> {
                                    {"loading", state.loginState.loading},
                                    {"email", state.loginState.email},
                                    {"password", state.loginState.password}
                                },
                                builder: (context1, viewModel) => {
                                    var loading = (bool) viewModel["loading"];
                                    var email = viewModel["email"] as string;
                                    var password = viewModel["password"] as string;
                                    var btnEnable = email.Length > 0 && password.Length > 0;
                                    Debug.Log($"{email}, {password}, {btnEnable}");
                                    Widget right = new Container();
                                    if (loading)
                                        right = new Padding(
                                            padding: EdgeInsets.only(right: 24),
                                            child: new CustomActivityIndicator(size: 20)
                                        );
                                    return new Container(
                                        height: 48,
                                        decoration: new BoxDecoration(
                                            btnEnable
                                                ? (loading ? CColors.primary2 : CColors.primary)
                                                : CColors.BrownGrey,
                                            borderRadius: BorderRadius.all(24)
                                        ),
                                        child: new Stack(
                                            children: new List<Widget> {
                                                new Align(
                                                    alignment: Alignment.center,
                                                    child: new Text(
                                                        "确定",
                                                        maxLines: 1,
                                                        style: new TextStyle(
                                                            fontSize: 16,
                                                            color: CColors.text1
                                                        )
                                                    )
                                                ),
                                                new Align(
                                                    alignment: Alignment.centerRight,
                                                    child: right
                                                )
                                            }
                                        )
                                    );
                                }
                            )
                        ),
                        new Container(height: 8),
                        new CustomButton(
                            child: new Text(
                                "忘记密码",
                                style: new TextStyle(
                                    fontSize: 16,
                                    color: CColors.text2
                                )
                            )
                        ),
                        new Container(height: 20),
                        new StoreConnector<AppState, bool>(
                            converter: (state, dispatch) => state.loginState.isLoggedIn,
                            builder: (context1, isLoggedIn) => new Text(
                                isLoggedIn ? "已登陆" : "未登录",
                                style: CTextStyle.H2White
                            )
                        )
                    }
                )
            );
        }
    }
}