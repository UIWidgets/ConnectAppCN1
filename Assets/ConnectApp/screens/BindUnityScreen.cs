using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.screens {
    public enum FromPage {
        weChat,
        login
    }

    public class BindUnityScreen : StatefulWidget {
        public BindUnityScreen(
            Key key = null
        ) : base(key) {
        }

        public override State createState() {
            return new _BindUnityScreenState();
        }
    }

    public class _BindUnityScreenState : State<BindUnityScreen> {
        private readonly FocusNode _emailFocusNode = new FocusNode();
        private readonly FocusNode _passwordFocusNode = new FocusNode();
        private bool _isEmailFocus;
        private bool _isPasswordFocus;

        public override void initState() {
            base.initState();
            _isEmailFocus = true;
            _isPasswordFocus = false;
            _emailFocusNode.addListener(_emailFocusNodeListener);
            _passwordFocusNode.addListener(_passwordFocusNodeListener);
        }

        public override void dispose() {
            _emailFocusNode.removeListener(_emailFocusNodeListener);
            _passwordFocusNode.removeListener(_passwordFocusNodeListener);
            base.dispose();
        }

        private void _emailFocusNodeListener() {
            if (_isEmailFocus == false) setState(() => { _isEmailFocus = true; });
            if (_isPasswordFocus) setState(() => { _isPasswordFocus = false; });
        }

        private void _passwordFocusNodeListener() {
            if (_isPasswordFocus == false) setState(() => { _isPasswordFocus = true; });
            if (_isEmailFocus) setState(() => { _isEmailFocus = false; });
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new SafeArea(
                    child: _buildContent(context)
                )
            );
        }

        private Widget _buildContent(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        _buildTopView(context),
                        _buildMiddleView(context),
                        _buildBottomView(context)
                    }
                )
            );
        }

        private static Widget _buildTopView(BuildContext context) {
            var fromPage = StoreProvider.store.state.loginState.fromPage;
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget> {
                    new Container(
                        height: 44,
                        padding: EdgeInsets.only(8, 8, 8),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget> {
                                fromPage == FromPage.weChat
                                    ? new CustomButton(
                                        onPressed: () => {
                                            Router.navigator.pop();
                                        },
                                        child: new Text(
                                            "跳过",
                                            style: CTextStyle.PLargeBody4
                                        )
                                    )
                                    : new CustomButton(
                                        onPressed: () => { LoginScreen.navigator.pop(); },
                                        child: new Icon(
                                            Icons.arrow_back,
                                            size: 24,
                                            color: CColors.icon3
                                        )
                                    ),
                                new CustomButton(
                                    child: new Text(
                                        "创建 Unity ID",
                                        style: CTextStyle.PLargeMediumBlue
                                    )
                                )
                            }
                        )
                    ),
                    new Container(height: 16),
                    new Container(
                        padding: EdgeInsets.symmetric(horizontal: 16),
                        child: new Text(
                            fromPage == FromPage.weChat ? "绑定你的Unity账号" : "登陆你的Unity账号",
                            style: CTextStyle.H2
                        )
                    )
                }
            );
        }

        private Widget _buildMiddleView(BuildContext context) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 32),
                        new Container(
                            child: new Text(
                                "邮箱",
                                style: CTextStyle.PMediumBody4
                            )
                        ),
                        new Container(
                            decoration: new BoxDecoration(
                                CColors.Transparent,
                                border: new Border(
                                    bottom: new BorderSide(
                                        _isEmailFocus ? CColors.PrimaryBlue : CColors.Separator,
                                        _isEmailFocus ? 2 : 1
                                    )
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
                                            autofocus: true,
                                            style: CTextStyle.PLargeBody,
                                            cursorColor: CColors.PrimaryBlue,
                                            clearButtonMode: InputFieldClearButtonMode.whileEditing,
                                            keyboardType: TextInputType.emailAddress,
                                            onChanged: text => {
                                                StoreProvider.store.Dispatch(new LoginChangeEmailAction
                                                    {changeText = text});
                                            },
                                            onSubmitted: text => {
                                                _emailFocusNode.unfocus();
                                                FocusScope.of(context).requestFocus(_passwordFocusNode);
                                            }
                                        )
                                    );
                                }
                            )
                        ),
                        new Container(height: 16),
                        new Container(
                            child: new Text(
                                "密码",
                                style: CTextStyle.PMediumBody4
                            )
                        ),
                        new Container(
                            decoration: new BoxDecoration(
                                CColors.Transparent,
                                border: new Border(
                                    bottom: new BorderSide(
                                        _isPasswordFocus ? CColors.PrimaryBlue : CColors.Separator,
                                        _isPasswordFocus ? 2 : 1
                                    )
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
                                            style: CTextStyle.PLargeBody,
                                            cursorColor: CColors.PrimaryBlue,
                                            clearButtonMode: InputFieldClearButtonMode.whileEditing,
                                            onChanged: text => {
                                                StoreProvider.store.Dispatch(new LoginChangePasswordAction
                                                    {changeText = text});
                                            },
                                            onSubmitted: text => {
                                                _emailFocusNode.unfocus();
                                                _passwordFocusNode.unfocus();
                                                StoreProvider.store.Dispatch(new LoginByEmailAction
                                                    {context = context});
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

        private Widget _buildBottomView(BuildContext context) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 32),
                        new StoreConnector<AppState, Dictionary<string, object>>(
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
                                Widget right = new Container();
                                if (loading)
                                    right = new Padding(
                                        padding: EdgeInsets.only(right: 24),
                                        child: new CustomActivityIndicator(
                                            animationImage: AnimationImage.white,
                                            size: 20
                                        )
                                    );
                                return new CustomButton(
                                    onPressed: () => {
                                        if (!btnEnable || loading) return;
                                        _emailFocusNode.unfocus();
                                        _passwordFocusNode.unfocus();
                                        StoreProvider.store.Dispatch(new LoginByEmailAction {context = context});
                                    },
                                    padding: EdgeInsets.zero,
                                    child: new Container(
                                        height: 48,
                                        decoration: new BoxDecoration(
                                            btnEnable
                                                ? CColors.PrimaryBlue
                                                : CColors.Disable,
                                            borderRadius: BorderRadius.all(24)
                                        ),
                                        child: new Stack(
                                            children: new List<Widget> {
                                                new Align(
                                                    alignment: Alignment.center,
                                                    child: new Text(
                                                        "确定",
                                                        maxLines: 1,
                                                        style: CTextStyle.PLargeWhite
                                                    )
                                                ),
                                                new Align(
                                                    alignment: Alignment.centerRight,
                                                    child: right
                                                )
                                            }
                                        )
                                    )
                                );
                            }
                        ),
                        new Container(height: 8),
                        new CustomButton(
                            child: new Text(
                                "忘记密码",
                                style: CTextStyle.PRegularBody3
                            )
                        )
                    }
                )
            );
        }
    }
}