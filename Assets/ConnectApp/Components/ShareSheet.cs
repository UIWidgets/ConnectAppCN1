using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Plugins;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public enum ShareType {
        friends,
        moments,
        miniProgram,
        clipBoard,
        block,
        report
    }

    public enum ProjectType {
        article,
        iEvent
    }

    public delegate void OnShareType(ShareType type);

    public class ShareView : StatelessWidget {
        public ShareView(
            Key key = null,
            OnShareType onPressed = null,
            ProjectType projectType = ProjectType.article,
            bool showReportAndBlock = true
        ) : base(key: key) {
            this.onPressed = onPressed;
            this.projectType = projectType;
            this.showReportAndBlock = showReportAndBlock;
        }

        readonly OnShareType onPressed;
        readonly ProjectType projectType;
        readonly bool showReportAndBlock;

        public override Widget build(BuildContext context) {
            var mediaQueryData = MediaQuery.of(context: context);
            return new Column(
                mainAxisAlignment: MainAxisAlignment.end,
                children: new List<Widget> {
                    new Container(
                        decoration: new BoxDecoration(
                            color: CColors.White,
                            borderRadius: BorderRadius.only(12, 12)
                        ),
                        width: mediaQueryData.size.width,
                        height: this.projectType == ProjectType.article && this.showReportAndBlock
                            ? 319 + mediaQueryData.padding.bottom
                            : 211 + mediaQueryData.padding.bottom,
                        child: new Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                new Container(
                                    height: 54,
                                    padding: EdgeInsets.only(top: 16, left: 16),
                                    child: new Text("分享至", style: CTextStyle.PRegularBody4)
                                ),
                                new Container(
                                    height: 108,
                                    padding: EdgeInsets.only(top: 16),
                                    decoration: new BoxDecoration(
                                        border: new Border(
                                            new BorderSide(color: CColors.Separator2),
                                            bottom: new BorderSide(color: CColors.Separator2)
                                        )
                                    ),
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.start,
                                        children: this._buildShareItems()
                                    )
                                ),
                                this.projectType == ProjectType.article && this.showReportAndBlock
                                    ? new Container(
                                        height: 108,
                                        padding: EdgeInsets.only(top: 16),
                                        decoration: new BoxDecoration(
                                            border: new Border(
                                                bottom: new BorderSide(color: CColors.Separator2)
                                            )
                                        ),
                                        child: new Row(
                                            mainAxisAlignment: MainAxisAlignment.start,
                                            children: this._buildOtherItems()
                                        )
                                    )
                                    : new Container(),
                                new GestureDetector(
                                    onTap: () => {
                                        if (Router.navigator.canPop()) {
                                            Router.navigator.pop();
                                        }
                                    },
                                    child: new Container(
                                        height: 49,
                                        color: CColors.Transparent,
                                        alignment: Alignment.center,
                                        child: new Text("取消", style: CTextStyle.PLargeBody.copyWith(color: CColors.Cancel))
                                    )
                                ),
                                new Container(
                                    height: mediaQueryData.padding.bottom
                                )
                            }
                        )
                    )
                }
            );
        }

        List<Widget> _buildShareItems() {
            var shareItems = new List<Widget>();
            if (WechatPlugin.instance().isInstalled()) {
                shareItems.Add(
                    _buildShareItem(
                        icon: Icons.WechatIcon,
                        "微信好友",
                        color: CColors.White,
                        background: CColors.WeChatGreen,
                        () => this.onPressed(type: ShareType.friends)
                    )
                );
                shareItems.Add(
                    _buildShareItem(
                        icon: Icons.WechatMoment,
                        "朋友圈",
                        color: CColors.White,
                        background: CColors.WeChatGreen,
                        () => this.onPressed(type: ShareType.moments)
                    )
                );
            }

            shareItems.Add(
                _buildShareItem(
                    icon: Icons.insert_link,
                    "复制链接",
                    color: CColors.White,
                    background: CColors.PrimaryBlue,
                    () => this.onPressed(type: ShareType.clipBoard)
                )
            );
            return shareItems;
        }

        List<Widget> _buildOtherItems() {
            return new List<Widget> {
                _buildShareItem(
                    icon: Icons.block,
                    "屏蔽",
                    color: CColors.BrownGrey,
                    background: CColors.White,
                    () => this.onPressed(type: ShareType.block),
                    true
                ),
                _buildShareItem(
                    icon: Icons.report,
                    "投诉",
                    color: CColors.BrownGrey,
                    background: CColors.White,
                    () => this.onPressed(type: ShareType.report),
                    true
                )
            };
        }

        static Widget _buildShareItem(IconData icon, string title, Color color, Color background,
            GestureTapCallback onTap, bool isBorder = false) {
            var border = isBorder
                ? Border.all(
                    Color.fromRGBO(216, 216, 216, 1)
                )
                : null;
            return new GestureDetector(
                onTap: () => {
                    if (Router.navigator.canPop()) {
                        Router.navigator.pop();
                    }

                    onTap();
                },
                child: new Container(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    color: CColors.White,
                    child: new Column(children: new List<Widget> {
                        new Container(
                            width: 48,
                            height: 48,
                            decoration: new BoxDecoration(
                                color: background,
                                borderRadius: BorderRadius.all(24),
                                border: border
                            ),
                            child: new Icon(
                                icon: icon,
                                size: 30,
                                color: color
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 8),
                            height: 20,
                            child: new Text(
                                data: title,
                                style: CTextStyle.PSmallBody4
                            )
                        )
                    })
                )
            );
        }
    }
}