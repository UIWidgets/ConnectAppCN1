using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using Unity.UIWidgets.gestures;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.screens {
    public class SettingScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        new CustomAppBar(
                            title: new Text(
                                "设置",
                                style: new TextStyle(
                                    fontSize: 17,
                                    color: CColors.TextTitle
                                )
                            )
                        ),
                        _buildContent(context)
                    }
                )
            );
        }

        private static Widget _buildContent(BuildContext context) {
            return new Flexible(
                child: new Container(
                    decoration: new BoxDecoration(
                        CColors.White
                    ),
                    child: new ListView(
                        physics: new AlwaysScrollableScrollPhysics(),
                        children: new List<Widget> {
                            _buildGapView(),
                            _buildCellView("修改手机", () => { }),
                            _buildCellView("修改密码", () => { }),
                            _buildGapView(),
                            _buildCellView("评分", () => { }),
                            _buildCellView("意见反馈", () => { }),
                            _buildCellView("关于我们", () => { }),
                            _buildCellView("清理缓存", () => {
                                CustomDialogUtils.showCustomDialog(
                                    context, 
                                    child: new CustomDialog(
                                        message: "正在清理缓存"
                                    )
                                );
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
                color: CColors.background3
            );
        }

        private static Widget _buildLogoutBtn(BuildContext context) {
            return new CustomButton(
                padding: EdgeInsets.zero,
                onPressed: () => {
                    ActionSheetUtils.showModalActionSheet(context, new ActionSheet(
                        title: "确定退出当前账号吗？",
                        items: new List<ActionSheetItem> {
                            new ActionSheetItem("退出", ActionType.destructive, () => { }),
                            new ActionSheetItem("取消", ActionType.cancel, () => { })
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
                                style: new TextStyle(
                                    fontSize: 16,
                                    color: CColors.warning
                                )
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
                                style: CTextStyle.PLarge
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