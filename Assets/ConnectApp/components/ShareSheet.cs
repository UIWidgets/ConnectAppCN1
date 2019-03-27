using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.constants;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public enum ShareType {
        friends,
        moments
    }

    public delegate void OnPressed(ShareType type);

    public static class ShareUtils {
        public static IPromise<object> showShareView(
            Widget child
        ) {
            return ActionSheetUtils.showModalActionSheet(child);
        }
    }

    public class ShareView : StatelessWidget {
        public ShareView(
            Key key = null,
            OnPressed onPressed = null
        ) : base(key) {
            this.onPressed = onPressed;
        }

        public readonly OnPressed onPressed;

        public override Widget build(BuildContext context) {
            var mediaQueryData = MediaQuery.of(context);
            return new Column(
                mainAxisAlignment: MainAxisAlignment.end,
                children: new List<Widget> {
                    new Container(
                        color:CColors.White,
                        width: mediaQueryData.size.width,
                        height: 185,
                        child: new Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                new Padding(
                                    padding: EdgeInsets.only(top: 16, left: 16),
                                    child: new Text("分享至", style: CTextStyle.TextBody1)
                                ),
                                new Container(
                                    padding: EdgeInsets.only(top: 16),
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.start,
                                        children: new List<Widget> {
                                            _buildShareItem("shareWeChat", "微信好友",
                                                () => { onPressed(ShareType.friends); }),
                                            _buildShareItem("shareFriends", "朋友圈",
                                                () => { onPressed(ShareType.moments); })
                                        }
                                    )
                                ),
                                new GestureDetector(
                                    onTap: () => { Router.navigator.pop(); },
                                    child: new Container(
                                        height: 49,
                                        color: CColors.Transparent,
                                        alignment:Alignment.center,
                                        child: new Text("取消", style: CTextStyle.TextBody1)
                                    )
                                )
                            }
                        )
                    )
                }
            );
        }

        private static Widget _buildShareItem(string assetName, string title, GestureTapCallback onTap) {
            return new GestureDetector(
                onTap: onTap,
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
                                style: CTextStyle.PSmallBody3)
                        ),
                    })
                )
            );
        }
    }
}