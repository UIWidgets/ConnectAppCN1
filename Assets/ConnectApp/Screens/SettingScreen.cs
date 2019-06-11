using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.Plugins;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class SettingScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, SettingScreenViewModel>(
                converter: state => new SettingScreenViewModel {
                    anonymous = state.loginState.loginInfo.anonymous,
                    hasReviewUrl = state.settingState.hasReviewUrl,
                    reviewUrl = state.settingState.reviewUrl
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new SettingScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        mainRouterPushTo = routeName => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = routeName
                        }),
                        openUrl = url => dispatcher.dispatch(new OpenUrlAction {url = url}),
                        clearCache = () => dispatcher.dispatch(new SettingClearCacheAction()),
                        logout = () => {
                            dispatcher.dispatch(new LogoutAction());
                            dispatcher.dispatch(new MainNavigatorPopAction());
                        }
                    };
                    return new SettingScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class SettingScreen : StatefulWidget {
        public SettingScreen(
            SettingScreenViewModel viewModel = null,
            SettingScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly SettingScreenViewModel viewModel;
        public readonly SettingScreenActionModel actionModel;

        public override State createState() {
            return new _SettingScreenState();
        }
    }

    public class _SettingScreenState : State<SettingScreen> {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(context),
                                this._buildContent(context)
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar(BuildContext context) {
            return new Container(
                decoration: new BoxDecoration(
                    CColors.White,
                    border: new Border(bottom: new BorderSide(CColors.Separator2))
                ),
                width: MediaQuery.of(context).size.width,
                height: 94,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.symmetric(8, 16),
                            onPressed: () => this.widget.actionModel.mainRouterPop(),
                            child: new Icon(
                                Icons.arrow_back,
                                size: 24,
                                color: CColors.Icon
                            )
                        ),
                        new Container(
                            padding: EdgeInsets.only(16, bottom: 12),
                            child: new Text(
                                "设置",
                                style: CTextStyle.H2
                            )
                        )
                    }
                )
            );
        }

        Widget _buildContent(BuildContext context) {
            return new Flexible(
                child: new Container(
                    decoration: new BoxDecoration(
                        CColors.BgGrey
                    ),
                    child: new ListView(
                        physics: new AlwaysScrollableScrollPhysics(),
                        children: new List<Widget> {
                            _buildGapView(),
                            this.widget.viewModel.hasReviewUrl
                                ? _buildCellView("评分",
                                    () => this.widget.actionModel.openUrl(this.widget.viewModel.reviewUrl))
                                : new Container(),
                            this.widget.viewModel.anonymous
                                ? _buildCellView("绑定 Unity ID",
                                    () => this.widget.actionModel.mainRouterPushTo(MainNavigatorRoutes.BindUnity))
                                : new Container(),
                            _buildCellView("关于我们",
                                () => this.widget.actionModel.mainRouterPushTo(MainNavigatorRoutes.AboutUs)),
                            _buildGapView(),
                            _buildCellView("检查更新", () => VersionManager.checkForUpdates(CheckVersionType.setting)),
                            _buildGapView(),
                            _buildCellView("清理缓存", () => {
                                CustomDialogUtils.showCustomDialog(
                                    child: new CustomLoadingDialog(
                                        message: "正在清理缓存"
                                    )
                                );
                                this.widget.actionModel.clearCache();
                                Window.instance.run(TimeSpan.FromSeconds(1), () => {
                                        CustomDialogUtils.hiddenCustomDialog();
                                        CustomDialogUtils.showToast("缓存已清除", Icons.check_circle_outline);
                                    }
                                );
                            }),
                            _buildGapView(),
                            this._buildLogoutBtn(context)
                        }
                    )
                )
            );
        }

        static Widget _buildGapView() {
            return new CustomDivider(
                color: CColors.BgGrey
            );
        }

        Widget _buildLogoutBtn(BuildContext context) {
            return new CustomButton(
                padding: EdgeInsets.zero,
                onPressed: () => {
                    ActionSheetUtils.showModalActionSheet(new ActionSheet(
                        title: "确定退出当前账号吗？",
                        items: new List<ActionSheetItem> {
                            new ActionSheetItem("退出", ActionType.destructive,
                                () => {
                                    this.widget.actionModel.logout();
                                    JPushPlugin.deleteJPushAlias();
                                }),
                            new ActionSheetItem("取消", ActionType.cancel)
                        }
                    ));
                },
                child: new Container(
                    height: 60,
                    decoration: new BoxDecoration(
                        CColors.White
                    ),
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: new List<Widget> {
                            new Text(
                                "退出登录",
                                style: CTextStyle.PLargeError
                            )
                        }
                    )
                )
            );
        }

        static Widget _buildCellView(string title, GestureTapCallback onTap) {
            return new GestureDetector(
                onTap: onTap,
                child: new Container(
                    height: 60,
                    padding: EdgeInsets.symmetric(0, 16),
                    decoration: new BoxDecoration(
                        CColors.White
                    ),
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: new List<Widget> {
                            new Text(
                                title,
                                style: CTextStyle.PLargeBody
                            ),
                            new Flexible(child: new Container()),
                            new Icon(
                                Icons.chevron_right,
                                size: 24,
                                color: Color.fromRGBO(199, 203, 207, 1)
                            )
                        }
                    )
                )
            );
        }
    }
}