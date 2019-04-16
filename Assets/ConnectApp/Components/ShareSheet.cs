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
        clipBoard
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
            OnPressed onPressed = null
        ) : base(key) {
            this.onPressed = onPressed;
        }

        private readonly OnPressed onPressed;

        public override Widget build(BuildContext context) {
            var mediaQueryData = MediaQuery.of(context);
            return new Column(
                mainAxisAlignment: MainAxisAlignment.end,
                children: new List<Widget> {
                    new Container(
                        color: CColors.White,
                        width: mediaQueryData.size.width,
                        height: 211 + mediaQueryData.padding.bottom,
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
            if (WechatPlugin.instance().inInstalled()) {
                return new List<Widget> {
                    _buildShareItem("wechat-share", "微信好友",
                        () => { onPressed(ShareType.friends); }),
                    _buildShareItem("wechat-moment", "朋友圈",
                        () => { onPressed(ShareType.moments); }),
                    _buildClipBoardItem("复制链接", () => { onPressed(ShareType.clipBoard); })
                };
            }
            return new List<Widget> {
                _buildClipBoardItem("复制链接", () => { onPressed(ShareType.clipBoard); })
            };
        }

        private static Widget _buildShareItem(string assetName, string title, GestureTapCallback onTap) {
            return new GestureDetector(
                onTap: () => {
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
                                borderRadius: BorderRadius.circular(24),
                                image: new DecorationImage(new AssetImage(assetName))
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
        
        private static Widget _buildClipBoardItem(string title, GestureTapCallback onTap) {
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
                            child: new ClipRRect(
                                borderRadius: BorderRadius.all(Radius.circular(24)),
                                child: new Container(
                                    color:CColors.PrimaryBlue,
                                    child:new Icon(Icons.insert_link,size:24,color:CColors.White)
                                )
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