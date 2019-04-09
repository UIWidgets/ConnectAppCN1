using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    public enum FromPage {
        wechat,
        login,
        setting
    }

    public class BindUnityScreenConnector : StatelessWidget {
        public BindUnityScreenConnector(FromPage fromPage) {
            this.fromPage = fromPage;
        }

        private readonly FromPage fromPage;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, BindUnityScreenViewModel>(
                converter: (state) => new BindUnityScreenViewModel {
                    fromPage = fromPage,
                    loginLoading = state.loginState.loading,
                    loginEmail = state.loginState.email,
                    loginPassword = state.loginState.password,
                    loginBtnEnable = state.loginState.email.Length > 0 && state.loginState.password.Length > 0
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new BindUnityScreenActionModel {
                        mainRouterPop = () => {
                            dispatcher.dispatch(new MainNavigatorPopAction());
                            dispatcher.dispatch(new CleanEmailAndPasswordAction());
                        },
                        loginRouterPop = () => {
                            dispatcher.dispatch(new LoginNavigatorPopAction());
                            dispatcher.dispatch(new CleanEmailAndPasswordAction());
                        },
                        openUrl = (url) =>
                            dispatcher.dispatch(new OpenUrlAction {url = url}),
                        openCreateUnityIdUrl = () =>
                            dispatcher.dispatch<IPromise>(Actions.openCreateUnityIdUrl()),
                        changeEmail = (text) =>
                            dispatcher.dispatch(new LoginChangeEmailAction {changeText = text}),
                        changePassword = (text) =>
                            dispatcher.dispatch(new LoginChangePasswordAction {changeText = text}),
                        loginByEmail = () => dispatcher.dispatch(new LoginByEmailAction())
                    };
                    return new BindUnityScreen(viewModel, actionModel);
                }
            );
        }
    }


    public class BindUnityScreen : StatefulWidget {
        public BindUnityScreen(
            BindUnityScreenViewModel viewModel = null,
            BindUnityScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly BindUnityScreenViewModel viewModel;
        public readonly BindUnityScreenActionModel actionModel;
        
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

        private void _login() {
            if (!widget.viewModel.loginBtnEnable || widget.viewModel.loginLoading) return;
            _emailFocusNode.unfocus();
            _passwordFocusNode.unfocus();
            widget.actionModel.loginByEmail();
        }

        public override Widget build(BuildContext context) {
            Debug.Log($"_ArticleDetailScreenState build");
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
                        _buildTopView(),
                        _buildMiddleView(context),
                        _buildBottomView()
                    }
                )
            );
        }

        private Widget _buildTopView() {
            Widget leftWidget;
            switch (widget.viewModel.fromPage) {
                case FromPage.login: {
                    leftWidget = new CustomButton(
                        onPressed: () => widget.actionModel.loginRouterPop(),
                        child: new Icon(
                            Icons.arrow_back,
                            size: 24,
                            color: CColors.icon3
                        )
                    );
                    break;
                }
                case FromPage.wechat: {
                    leftWidget = new CustomButton(
                        onPressed: () => widget.actionModel.mainRouterPop(),
                        child: new Text(
                            "跳过",
                            style: CTextStyle.PLargeBody4
                        )
                    );
                    break;
                }
                case FromPage.setting: {
                    leftWidget = new CustomButton(
                        onPressed: () => widget.actionModel.mainRouterPop(),
                        child: new Icon(
                            Icons.arrow_back,
                            size: 24,
                            color: CColors.icon3
                        )
                    );
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget> {
                    new Container(
                        height: 44,
                        padding: EdgeInsets.only(8, 8, 8),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget> {
                                leftWidget,
                                new CustomButton(
                                    onPressed: () => widget.actionModel.openCreateUnityIdUrl(),
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
                            widget.viewModel.fromPage == FromPage.login ? "登陆你的Unity账号" : "绑定你的Unity账号",
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
                            height: 46,
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
                            child: new IgnorePointer(
                                ignoring: widget.viewModel.loginLoading,
                                child: new InputField(
                                    focusNode: _emailFocusNode,
                                    maxLines: 1,
                                    autofocus: true,
                                    style: CTextStyle.PLargeBody,
                                    cursorColor: CColors.PrimaryBlue,
                                    clearButtonMode: InputFieldClearButtonMode.whileEditing,
                                    keyboardType: TextInputType.emailAddress,
                                    onChanged: text => widget.actionModel.changeEmail(text),
                                    onSubmitted: _ => {
                                        _emailFocusNode.unfocus();
                                        FocusScope.of(context).requestFocus(_passwordFocusNode);
                                    }
                                )
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
                            height: 46,
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
                            child: new IgnorePointer(
                                ignoring: widget.viewModel.loginLoading,
                                child: new InputField(
                                    focusNode: _passwordFocusNode,
                                    maxLines: 1,
                                    autofocus: false,
                                    obscureText: true,
                                    style: CTextStyle.PLargeBody,
                                    cursorColor: CColors.PrimaryBlue,
                                    clearButtonMode: InputFieldClearButtonMode.whileEditing,
                                    onChanged: text => widget.actionModel.changePassword(text),
                                    onSubmitted: _ => _login()
                                )
                            )
                        )
                    }
                )
            );
        }

        private Widget _buildBottomView() {
            Widget right = new Container();
            if (widget.viewModel.loginLoading)
                right = new Padding(
                    padding: EdgeInsets.only(right: 24),
                    child: new CustomActivityIndicator(
                        animationImage: AnimationImage.white
                    )
                );
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 32),
                        new CustomButton(
                            onPressed: _login,
                            padding: EdgeInsets.zero,
                            child: new Container(
                                height: 48,
                                decoration: new BoxDecoration(
                                    widget.viewModel.loginBtnEnable
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
                        ),
                        new Container(height: 8),
                        new CustomButton(
                            onPressed: () => widget.actionModel.openUrl($"{Config.idBaseUrl}/password/new"),
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