using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class QRScanLoginScreenConnector : StatelessWidget {
        public QRScanLoginScreenConnector(
            string token,
            Key key = null
        ) : base(key: key) {
            this.token = token;
        }

        readonly string token;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, QRScanLoginScreenViewModel>(
                converter: state => new QRScanLoginScreenViewModel {
                    userId = state.loginState.loginInfo.userId,
                    userDict = state.userState.userDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    return new QRScanLoginScreen(
                        viewModel: viewModel,
                        () => dispatcher.dispatch<IPromise>(Actions.loginByQr(token: this.token, "confirm")),
                        () => dispatcher.dispatch<IPromise>(Actions.loginByQr(token: this.token, "cancel")),
                        () => dispatcher.dispatch(new MainNavigatorPopAction())
                    );
                }
            );
        }
    }

    public class QRScanLoginScreen : StatefulWidget {
        public QRScanLoginScreen(
            QRScanLoginScreenViewModel viewModel = null,
            Func<IPromise> loginByQr = null,
            Action cancelLoginByQr = null,
            Action mainRouterPop = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.loginByQr = loginByQr;
            this.cancelLoginByQr = cancelLoginByQr;
            this.mainRouterPop = mainRouterPop;
        }

        public readonly QRScanLoginScreenViewModel viewModel;
        public readonly Func<IPromise> loginByQr;
        public readonly Action cancelLoginByQr;
        public readonly Action mainRouterPop;

        public override State createState() {
            return new _QRScanLoginScreenState();
        }
    }

    public class _QRScanLoginScreenState : State<QRScanLoginScreen>, RouteAware {
        bool _needCancel;

        public override void initState() {
            base.initState();
            this._needCancel = true;
            StatusBarManager.statusBarStyle(false);
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        public void didPop() {
            if (this._needCancel) {
                this.widget.cancelLoginByQr();
            }
        }

        public void didPopNext() {
        }

        public void didPush() {
        }

        public void didPushNext() {
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        color: CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                this._buildTopView(),
                                this._buildBottomView()
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomAppBar(
                () => this.widget.mainRouterPop(),
                new Text(
                    "扫描结果",
                    style: CTextStyle.PXLargeMedium
                ),
                bottomSeparatorColor: CColors.Transparent
            );
        }

        Widget _buildTopView() {
            var user = this.widget.viewModel.userDict[key: this.widget.viewModel.userId];
            return new Flexible(
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(bottom: 12),
                            child: new Icon(
                                icon: Icons.computer,
                                size: 120,
                                color: CColors.TextBody
                            )
                        ),
                        new RichText(
                            text: new TextSpan(
                                children: new List<TextSpan> {
                                    new TextSpan(
                                        "即将使用",
                                        style: CTextStyle.PLargeBody5
                                    ),
                                    new TextSpan(
                                        $" {user.fullName ?? user.name} ",
                                        style: CTextStyle.PLargeBlue
                                    ),
                                    new TextSpan(
                                        "登录",
                                        style: CTextStyle.PLargeBody5
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }

        Widget _buildBottomView() {
            return new Container(
                padding: EdgeInsets.only(bottom: 120),
                child: new Column(
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.zero,
                            onPressed: () => {
                                this._needCancel = false;
                                CustomDialogUtils.showCustomDialog(
                                    child: new CustomLoadingDialog(
                                        message: "登录中"
                                    )
                                );
                                this.widget.loginByQr();
                            },
                            child: new Container(
                                height: 48,
                                margin: EdgeInsets.only(16, right: 16),
                                alignment: Alignment.center,
                                decoration: new BoxDecoration(
                                    color: CColors.PrimaryBlue,
                                    borderRadius: BorderRadius.all(24)
                                ),
                                child: new Text(
                                    "确定登录",
                                    style: CTextStyle.PLargeWhite
                                )
                            )
                        ),
                        new Container(height: 12),
                        new CustomButton(
                            padding: EdgeInsets.zero,
                            onPressed: () => this.widget.mainRouterPop(),
                            child: new Container(
                                color: CColors.Transparent,
                                height: 48,
                                margin: EdgeInsets.only(16, right: 16),
                                alignment: Alignment.center,
                                child: new Text(
                                    "取消",
                                    style: CTextStyle.PLargeBody5
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}