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
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class SettingScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, SettingScreenViewModel>(
                converter: state => new SettingScreenViewModel {
                    isLoggedIn = state.loginState.isLoggedIn,
                    anonymous = state.loginState.loginInfo.anonymous,
                    hasReviewUrl = state.settingState.hasReviewUrl,
                    reviewUrl = state.settingState.reviewUrl,
                    vibrate = state.settingState.vibrate
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new SettingScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        mainRouterPushTo = routeName => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = routeName
                        }),
                        updateVibrate = vibrate => dispatcher.dispatch(new SettingVibrateAction {
                            vibrate = vibrate
                        }),
                        openUrl = url => dispatcher.dispatch(new OpenUrlAction {url = url}),
                        clearCache = () => dispatcher.dispatch(new SettingClearCacheAction()),
                        logout = () => {
                            dispatcher.dispatch(new LogoutAction());
                            dispatcher.dispatch(new MainNavigatorPopAction());
                        }
                    };
                    return new SettingScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class SettingScreen : StatefulWidget {
        public SettingScreen(
            SettingScreenViewModel viewModel = null,
            SettingScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
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

        bool fpsLabelIsOpen;
        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this.fpsLabelIsOpen = LocalDataManager.getFPSLabelStatus();
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                this._buildContent()
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomNavigationBar(
                new Text(
                    "设置",
                    style: CTextStyle.H2
                ),
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: new Border(bottom: new BorderSide(color: CColors.Separator2))
                ),
                onBack: () => this.widget.actionModel.mainRouterPop()
            );
        }

        Widget _buildContent() {
            return new Flexible(
                child: new Container(
                    color: CColors.Background,
                    child: new ListView(
                        physics: new AlwaysScrollableScrollPhysics(),
                        children: new List<Widget> {
                            this.widget.viewModel.hasReviewUrl || this.widget.viewModel.anonymous 
                                ? _buildGapView()
                                : new Container(),
                            this.widget.viewModel.hasReviewUrl
                                ? _buildCellView("评分",
                                    () => {
                                        AnalyticsManager.ClickSetGrade();
                                        this.widget.actionModel.openUrl(obj: this.widget.viewModel.reviewUrl);
                                    })
                                : new Container(),
                            this.widget.viewModel.anonymous
                                ? _buildCellView("绑定 Unity ID",
                                    () => this.widget.actionModel.mainRouterPushTo(MainNavigatorRoutes.BindUnity))
                                : new Container(),
                            _buildGapView(),
                            _buildCellView("检查更新", () => {
                                AnalyticsManager.ClickCheckUpdate();
                                VersionManager.checkForUpdates(CheckVersionType.setting);
                            }),
                            _buildGapView(),
                            _buildCellView("清理缓存", () => {
                                AnalyticsManager.ClickClearCache();
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
                            _switchRow(
                                CCommonUtils.isIPhone ? "帧率监测" : "帧率监测 (暂仅支持 TinyGame)",
                                value: this.fpsLabelIsOpen,
                                value => {
                                    LocalDataManager.setFPSLabelStatus(isOpen: value);
                                    this.fpsLabelIsOpen = value;
                                    this.setState(() => {});
                                    FPSLabelPlugin.SwitchFPSLabelShowStatus(isOpen: value);
                                }
                            ),
                            this.widget.viewModel.isLoggedIn ? _buildGapView() : new Container(),
                            this.widget.viewModel.isLoggedIn ? this._buildLogoutBtn() : new Container()
                        }
                    )
                )
            );
        }

        static Widget _buildGapView() {
            return new CustomDivider(
                color: CColors.Background
            );
        }

        Widget _buildLogoutBtn() {
            return new CustomButton(
                padding: EdgeInsets.zero,
                onPressed: () => {
                    ActionSheetUtils.showModalActionSheet(new ActionSheet(
                        title: "确定退出当前账号吗？",
                        items: new List<ActionSheetItem> {
                            new ActionSheetItem("退出", type: ActionType.destructive,
                                () => {
                                    AnalyticsManager.ClickLogout();
                                    this.widget.actionModel.logout();
                                }),
                            new ActionSheetItem("取消", type: ActionType.cancel)
                        }
                    ));
                },
                child: new Container(
                    height: 60,
                    color: CColors.White,
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
            return new CustomListTile(
                title: title,
                trailing: CustomListTileConstant.defaultTrailing,
                onTap: onTap
            );
        }
        
        static Widget _switchRow(string content, bool value, ValueChanged<bool> onChanged) {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.symmetric(16, 18),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new Expanded(
                            child: new Text(
                                data: content,
                                style: CTextStyle.PLargeBody,
                                overflow: TextOverflow.ellipsis
                            )
                        ),
                        new CustomSwitch(value: value, onChanged: onChanged, activeColor: CColors.PrimaryBlue)
                    }
                )
            );
        }
    }
}