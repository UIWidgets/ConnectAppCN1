using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
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
                        () => dispatcher.dispatch<IPromise>(Actions.loginByQr(token: this.token)),
                        () => dispatcher.dispatch(new MainNavigatorPopAction())
                    );
                }
            );
        }
    }

    public class QRScanLoginScreen : StatelessWidget {
        public QRScanLoginScreen(
            QRScanLoginScreenViewModel viewModel = null,
            Func<IPromise> loginByQr = null,
            Action mainRouterPop = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.loginByQr = loginByQr;
            this.mainRouterPop = mainRouterPop;
        }

        readonly QRScanLoginScreenViewModel viewModel;
        readonly Func<IPromise> loginByQr;
        readonly Action mainRouterPop;
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        color: CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Expanded(
                                    child: this._buildContent()
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomAppBar(
                () => this.mainRouterPop(),
                new Text(
                    "扫描结果",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent() {
            var user = this.viewModel.userDict[key: this.viewModel.userId];
            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(top: 120, bottom: 26),
                            child: Image.asset(
                                "image/computer",
                                width: 110,
                                height: 96
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
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 120),
                            child: new CustomButton(
                                padding: EdgeInsets.zero,
                                onPressed: () => {
                                    CustomDialogUtils.showCustomDialog(
                                        child: new CustomLoadingDialog(
                                            message: "登录中"
                                        )
                                    );
                                    this.loginByQr();
                                },
                                child: new Container(
                                    height: 40,
                                    margin: EdgeInsets.only(16, right: 16),
                                    alignment: Alignment.center,
                                    decoration: new BoxDecoration(
                                        color: CColors.PrimaryBlue,
                                        borderRadius: BorderRadius.all(5)
                                    ),
                                    child: new Text(
                                        "登录",
                                        style: CTextStyle.PLargeMediumWhite
                                    )
                                )
                            )
                        ),
                        new CustomButton(
                            onPressed: () => this.mainRouterPop(),
                            child: new Container(
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