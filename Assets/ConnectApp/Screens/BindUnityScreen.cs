using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;

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

        readonly FromPage fromPage;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, BindUnityScreenViewModel>(
                converter: state => new BindUnityScreenViewModel {
                    fromPage = this.fromPage,
                    loginLoading = state.loginState.loading,
                    loginEmail = state.loginState.email,
                    loginPassword = state.loginState.password,
                    loginBtnEnable = state.loginState.email.Length > 0 && state.loginState.password.Length > 0
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new BindUnityScreenActionModel {
                        mainRouterPop = () => {
                            dispatcher.dispatch(new MainNavigatorPopAction {index = 1});
                            dispatcher.dispatch(new CleanEmailAndPasswordAction());
                        },
                        loginRouterPop = () => {
                            dispatcher.dispatch(new LoginNavigatorPopAction {index = 1});
                            dispatcher.dispatch(new CleanEmailAndPasswordAction());
                        },
                        openUrl = url =>
                            dispatcher.dispatch(new OpenUrlAction {url = url}),
                        openCreateUnityIdUrl = () =>
                            dispatcher.dispatch<IPromise>(Actions.openCreateUnityIdUrl()),
                        changeEmail = text =>
                            dispatcher.dispatch(new LoginChangeEmailAction {changeText = text}),
                        changePassword = text =>
                            dispatcher.dispatch(new LoginChangePasswordAction {changeText = text}),
                        startLoginByEmail = () => dispatcher.dispatch(new StartLoginByEmailAction()),
                        clearEmailAndPassword = () => dispatcher.dispatch(new CleanEmailAndPasswordAction()),
                        loginByEmail = () => dispatcher.dispatch<IPromise>(Actions.loginByEmail())
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
        readonly FocusNode _emailFocusNode = new FocusNode();
        readonly FocusNode _passwordFocusNode = new FocusNode();
        FocusScopeNode _focusScopeNode;
        
        bool _isEmailFocus;
        bool _isPasswordFocus;

        public override void initState() {
            base.initState();
            this._isEmailFocus = true;
            this._isPasswordFocus = false;
            this._emailFocusNode.addListener(this._focusNodeListener);
            this._passwordFocusNode.addListener(this._focusNodeListener);
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (this.widget.viewModel.loginEmail.Length > 0 || this.widget.viewModel.loginPassword.Length > 0) {
                    this.widget.actionModel.clearEmailAndPassword();
                }
            });
        }

        void _focusNodeListener() {
            if (this._isEmailFocus == this._emailFocusNode.hasFocus && this._isPasswordFocus == this._passwordFocusNode.hasFocus ) {
                return;
            }
            if (!(this._emailFocusNode.hasFocus && this._passwordFocusNode.hasFocus)) {
                this._isEmailFocus = this._emailFocusNode.hasFocus;
                this._isPasswordFocus = this._passwordFocusNode.hasFocus;
                this.setState(() => {});
            }
        }

        void _login() {
            if (!this.widget.viewModel.loginBtnEnable || this.widget.viewModel.loginLoading) {
                return;
            }
            this._emailFocusNode.unfocus();
            this._passwordFocusNode.unfocus();
            this.widget.actionModel.startLoginByEmail();
            this.widget.actionModel.loginByEmail().Catch(_ => {
                var customSnackBar = new CustomSnackBar(
                    "邮箱或密码不正确，请稍后再试。"
                );
                customSnackBar.show();
            });
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: this._buildContent(context)
                )
            );
        }

        Widget _buildContent(BuildContext context) {
            return new GestureDetector(
                onTap: () => {
                    if (this._emailFocusNode.hasFocus) {
                        this._emailFocusNode.unfocus();
                    }
                    if (this._passwordFocusNode.hasFocus) {
                        this._passwordFocusNode.unfocus();
                    }
                },
                child: new Container(
                    color: CColors.White,
                    child: new Column(
                        children: new List<Widget> {
                            this._buildTopView(),
                            this._buildMiddleView(context),
                            this._buildBottomView()
                        }
                    )
                )
            );
        }

        Widget _buildTopView() {
            Widget leftWidget;
            switch (this.widget.viewModel.fromPage) {
                case FromPage.login: {
                    leftWidget = new CustomButton(
                        onPressed: () => this.widget.actionModel.loginRouterPop(),
                        child: new Icon(
                            Icons.arrow_back,
                            size: 24,
                            color: CColors.Icon
                        )
                    );
                    break;
                }

                case FromPage.wechat: {
                    leftWidget = new CustomButton(
                        onPressed: () => this.widget.actionModel.mainRouterPop(),
                        child: new Text(
                            "跳过",
                            style: CTextStyle.PLargeBody4
                        )
                    );
                    break;
                }

                case FromPage.setting: {
                    leftWidget = new CustomButton(
                        onPressed: () => this.widget.actionModel.mainRouterPop(),
                        child: new Icon(
                            Icons.arrow_back,
                            size: 24,
                            color: CColors.Icon
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
                                    onPressed: () => this.widget.actionModel.openCreateUnityIdUrl(),
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
                            this.widget.viewModel.fromPage == FromPage.login ? "登录你的Unity账号" : "绑定你的Unity账号",
                            style: CTextStyle.H2
                        )
                    )
                }
            );
        }

        Widget _buildMiddleView(BuildContext context) {
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
                                        this._isEmailFocus ? CColors.PrimaryBlue : CColors.Separator,
                                        this._isEmailFocus ? 2 : 1
                                    )
                                )
                            ),
                            alignment: Alignment.center,
                            child: new InputField(
                                focusNode: this._emailFocusNode,
                                maxLines: 1,
                                autofocus: true,
                                enabled: !this.widget.viewModel.loginLoading,
                                style: CTextStyle.PLargeBody,
                                cursorColor: CColors.PrimaryBlue,
                                clearButtonMode: InputFieldClearButtonMode.whileEditing,
                                keyboardType: TextInputType.emailAddress,
                                onChanged: text => this.widget.actionModel.changeEmail(text),
                                onSubmitted: _ => {
                                    if (null == this._focusScopeNode) {
                                        this._focusScopeNode = FocusScope.of(context);
                                    }
                                    this._focusScopeNode.requestFocus(this._passwordFocusNode);
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
                            height: 46,
                            decoration: new BoxDecoration(
                                CColors.Transparent,
                                border: new Border(
                                    bottom: new BorderSide(
                                        this._isPasswordFocus ? CColors.PrimaryBlue : CColors.Separator,
                                        this._isPasswordFocus ? 2 : 1
                                    )
                                )
                            ),
                            alignment: Alignment.center,
                            child: new InputField(
                                focusNode: this._passwordFocusNode,
                                maxLines: 1,
                                autofocus: false,
                                obscureText: true,
                                enabled: !this.widget.viewModel.loginLoading,
                                style: CTextStyle.PLargeBody,
                                cursorColor: CColors.PrimaryBlue,
                                clearButtonMode: InputFieldClearButtonMode.whileEditing,
                                onChanged: text => this.widget.actionModel.changePassword(text),
                                onSubmitted: _ => this._login()
                            )
                        )
                    }
                )
            );
        }

        Widget _buildBottomView() {
            Widget right = new Container();
            if (this.widget.viewModel.loginLoading) {
                right = new CustomActivityIndicator(
                    loadingColor: LoadingColor.white,
                    size: LoadingSize.small
                );
            }

            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 32),
                        new CustomButton(
                            onPressed: this._login,
                            padding: EdgeInsets.zero,
                            child: new Container(
                                height: 48,
                                decoration: new BoxDecoration(this.widget.viewModel.loginBtnEnable
                                        ? this.widget.viewModel.loginLoading
                                            ? CColors.ButtonActive
                                            : CColors.PrimaryBlue
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
                                        new Positioned(
                                            right: 24,
                                            height: 48,
                                            child: right
                                        )
                                    }
                                )
                            )
                        ),
                        new Container(height: 8),
                        new CustomButton(
                            onPressed: () => this.widget.actionModel.openUrl($"{Config.idBaseUrl}/password/new"),
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