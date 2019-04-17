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
                    _buildShareItem(
                        Icons.WechatIcon, 
                       "微信好友",
                        CColors.White,
                        CColors.WechatGreen,
                       () => onPressed(ShareType.friends)
                    ),
                    _buildShareItem(
                        Icons.WechatMoment, 
                        "朋友圈",
                        CColors.White,
                        CColors.WechatGreen,
                        () => onPressed(ShareType.moments)
                    ),
                    _buildShareItem(
                        Icons.insert_link,
                        "复制链接", 
                        CColors.White,
                        CColors.PrimaryBlue,
                        () => onPressed(ShareType.clipBoard)
                    )
                };
            }
            return new List<Widget> {
                _buildShareItem(
                    Icons.insert_link,
                    "复制链接",
                    CColors.White,
                    CColors.PrimaryBlue,
                    () => onPressed(ShareType.clipBoard)
                )
            };
        }
        
        private Widget _buildShareItem(IconData icon, string title, Color color, Color background, GestureTapCallback onTap) {
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
                                    color: background,
                                    child:new Icon(
                                        icon,
                                        size: 30,
                                        color: color
                                    )
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