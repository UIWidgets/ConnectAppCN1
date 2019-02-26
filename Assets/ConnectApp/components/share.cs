using System;
using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class Share {
        public static void showShareView(BuildContext context,
            bool barrierDismissible = true, WidgetBuilder builder = null) {
            DialogUtils.showGeneralDialog(
                context: context,
                pageBuilder: (BuildContext buildContext, Animation<float> animation,
                    Animation<float> secondaryAnimation) => {
                    return builder(buildContext);
                },
                barrierDismissible: barrierDismissible,
                barrierLabel: "",
                barrierColor: new Color(0x8A000000),
                transitionDuration: TimeSpan.FromMilliseconds(150),
                transitionBuilder: _buildMaterialDialogTransitions
            );
        }

        private static Widget _buildMaterialDialogTransitions(BuildContext context,
            Animation<float> animation, Animation<float> secondaryAnimation, Widget child) {
            return new ModalPageTransition(
                routeAnimation: animation,
                child: child
            );
        }
    }

    public class ShareView : StatelessWidget {
        public override Widget build(BuildContext context) {
            var mediaQueryData = MediaQuery.of(context);
            return new Column(
                mainAxisAlignment: MainAxisAlignment.end,
                children: new List<Widget> {
                    new Container(
                        decoration: new BoxDecoration(color: CColors.background1),
                        width: mediaQueryData.size.width,
                        height: 165,
                        child: new Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            children: new List<Widget> {
                                new Padding(
                                    padding: EdgeInsets.only(top: 15),
                                    child: new Text("分享至", style: new TextStyle(fontSize: 17, color: Colors.white))
                                ),
                                new Container(
                                    padding: EdgeInsets.only(top: 10),
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.start,
                                        children: new List<Widget> {
                                            _shareItem("shareWeChat", "微信好友", onTap: () => { }),
                                            _shareItem("shareFriends", "朋友圈", onTap: () => { }),
                                        })),
                                new GestureDetector(
                                    onTap: () => { },
                                    child: new Text("取消", style: new TextStyle(fontSize: 17, color: Colors.white))
                                )
                            }
                        )
                    )
                }
            );
        }

        private Widget _shareItem(string assetName, string title, VoidCallback onTap) {
            return new GestureDetector(
                onTap: () => { },
                child: new Container(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    decoration: new BoxDecoration(
                        color: CColors.background1
                    ),
                    child: new Column(children: new List<Widget> {
                        new Container(
                            width: 48,
                            height: 48,
                            decoration: new BoxDecoration(
                                borderRadius: BorderRadius.circular(24),
                                image: new DecorationImage(image: new AssetImage(assetName))
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 8),
                            height: 20,
                            child: new Text(
                                title,
                                style: new TextStyle(
                                    fontSize: 12,
                                    color: CColors.text2,
                                    decoration: TextDecoration.none,
                                    fontWeight: FontWeight.w400
                                )
                            )
                        ),
                    })
                )
            );
        }
    }
}