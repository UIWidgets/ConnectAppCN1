using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class SettingScreen : StatefulWidget {
        public override State createState() {
            return new _SettingScreenState();
        }
    }

    internal class _SettingScreenState : State<SettingScreen> {
        public override void initState() {
            base.initState();
            StoreProvider.store.Dispatch(new SettingReviewUrlAction {
                platform = Config.platform,
                store = Config.store
            });
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new SafeArea(
                    child: new Container(
                        child: new Column(
                            children: new List<Widget> {
                                _buildNavigationBar(context),
                                _buildContent(context)
                            }
                        )
                    )
                )
            );
        }

        private static Widget _buildNavigationBar(BuildContext context) {
            return new Container(
                decoration: new BoxDecoration(
                    CColors.White,
                    border: new Border(bottom: new BorderSide(CColors.Separator2))
                ),
                width: MediaQuery.of(context).size.width,
                height: 140,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 44,
                            child: new CustomButton(
                                padding: EdgeInsets.only(16),
                                onPressed: () => StoreProvider.store.Dispatch(new MainNavigatorPopAction()),
                                child: new Icon(
                                    Icons.arrow_back,
                                    size: 24,
                                    color: CColors.icon3
                                )
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

        private static Widget _buildContent(BuildContext context) {
            return new Flexible(
                child: new Container(
                    decoration: new BoxDecoration(
                        CColors.BgGrey
                    ),
                    child: new ListView(
                        physics: new AlwaysScrollableScrollPhysics(),
                        children: new List<Widget> {
                            _buildGapView(),
                            new StoreConnector<AppState, string>(
                                converter: (state, dispatch) => state.settingState.reviewUrl,
                                builder: (cxt, reviewUrl) => {
                                    if (reviewUrl.Length <= 0) return new Container();
                                    return _buildCellView("评分",
                                        () => StoreProvider.store.Dispatch(new OpenUrlAction {url = reviewUrl}));
                                }
                            ),
                            _buildCellView("意见反馈", () => { }),
                            _buildCellView("关于我们", () => { }),
                            _buildGapView(),
                            _buildCellView("清理缓存", () => {
                                StoreProvider.store.Dispatch(new SettingClearCacheAction());
                            }),
                            _buildGapView(),
                            _buildLogoutBtn(context)
                        }
                    )
                )
            );
        }

        private static Widget _buildGapView() {
            return new CustomDivider(
                color: CColors.BgGrey
            );
        }

        private static Widget _buildLogoutBtn(BuildContext context) {
            return new CustomButton(
                padding: EdgeInsets.zero,
                onPressed: () => {
                    ActionSheetUtils.showModalActionSheet(new ActionSheet(
                        title: "确定退出当前账号吗？",
                        items: new List<ActionSheetItem> {
                            new ActionSheetItem("退出", ActionType.destructive,
                                () => { StoreProvider.store.Dispatch(new LogoutAction {context = context}); }),
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

        private static Widget _buildCellView(string title, GestureTapCallback onTap) {
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