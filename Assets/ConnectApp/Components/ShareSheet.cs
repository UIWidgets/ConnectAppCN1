using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.constants;
using ConnectApp.plugins;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public enum ShareType {
        friends,
        moments,
        clipBoard,
        block,
        report
    }

    public enum ProjectType {
        article,
        iEvent
    }

    public delegate void OnPressed(ShareType type);

    public static class ShareUtils {
        public static void showShareView(
            Widget child
        ) {
            ActionSheetUtils.showModalActionSheet(child);
        }
    }

    public class ShareView : StatelessWidget {
        public ShareView(
            Key key = null,
            OnPressed onPressed = null,
            ProjectType projectType = ProjectType.article
        ) : base(key) {
            this.onPressed = onPressed;
            this.projectType = projectType;
        }

        private readonly OnPressed onPressed;
        private readonly ProjectType projectType;

        public override Widget build(BuildContext context) {
            var mediaQueryData = MediaQuery.of(context);
            return new Column(
                mainAxisAlignment: MainAxisAlignment.end,
                children: new List<Widget> {
                    new Container(
                        color: CColors.White,
                        width: mediaQueryData.size.width,
                        height: projectType == ProjectType.article ? 319 : 211 + mediaQueryData.padding.bottom,
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
                                            new BorderSide(CColors.Separator2),
                                            bottom: new BorderSide(CColors.Separator2)
                                        )
                                    ),
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.start,
                                        children: _buildShareItems()
                                    )
                                ),
                                projectType == ProjectType.article 
                                    ?
                                    new Container(
                                        height: 108,
                                        padding: EdgeInsets.only(top: 16),
                                        decoration: new BoxDecoration(
                                            border: new Border(
                                                bottom: new BorderSide(CColors.Separator2)
                                            )
                                        ),
                                        child: new Row(
                                            mainAxisAlignment: MainAxisAlignment.start,
                                            children: _buildOtherItems()
                                        )
                                    ) 
                                    :
                                    new Container(),
                                new GestureDetector(
                                    onTap: () => {
                                        if (Router.navigator.canPop()) Router.navigator.pop();
                                    },
                                    child: new Container(
                                        height: 49,
                                        color: CColors.Transparent,
                                        alignment: Alignment.center,
                                        child: new Text("取消", style: CTextStyle.PLargeBody)
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

        private List<Widget> _buildShareItems() {
            var shareItems = new List<Widget>();
            if (WechatPlugin.instance().inInstalled()) {
                shareItems.Add(
                    _buildShareItem(
                        Icons.WechatIcon, 
                        "微信好友",
                        CColors.White,
                        CColors.WechatGreen,
                        () => onPressed(ShareType.friends)
                    )
                );
                shareItems.Add(
                    _buildShareItem(
                        Icons.WechatMoment, 
                        "朋友圈",
                        CColors.White,
                        CColors.WechatGreen,
                        () => onPressed(ShareType.moments)
                    )
                );
            }
            shareItems.Add(
                _buildShareItem(
                    Icons.insert_link,
                    "复制链接",
                    CColors.White,
                    CColors.PrimaryBlue,
                    () => onPressed(ShareType.clipBoard)
                )
            );
            return shareItems;
        }
        
        private List<Widget> _buildOtherItems() {
            return new List<Widget> {
                _buildShareItem(
                    Icons.block,
                    "屏蔽",
                    CColors.BrownGrey,
                    CColors.White,
                    () => onPressed(ShareType.block),
                    true
                ),
                _buildShareItem(
                    Icons.report,
                    "投诉",
                    CColors.BrownGrey,
                    CColors.White,
                    () => onPressed(ShareType.report),
                    true
                )
            };
        }
        
        private static Widget _buildShareItem(IconData icon, string title, Color color, Color background, GestureTapCallback onTap, bool isBorder = false) {
            var border = isBorder
                ? Border.all(
                    Color.fromRGBO(216, 216, 216, 1)
                )
                : new Border();
            return new GestureDetector(
                onTap:() => {
                    if (Router.navigator.canPop()) Router.navigator.pop();
                    onTap();
                },
                child: new Container(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    decoration: new BoxDecoration(
                        CColors.White
                    ),
                    child: new Column(children: new List<Widget> {
                        new Container(
                            width: 48,
                            height: 48,
                            decoration: new BoxDecoration(
                                background,
                                borderRadius: BorderRadius.all(24),
                                border: border
                            ),
                            child: new Icon(
                                icon,
                                size: 30,
                                color: color
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 8),
                            height: 20,
                            child: new Text(
                                title,
                                style: CTextStyle.PSmallBody4
                            )
                        )
                    })
                )
            );
        }
    }
}