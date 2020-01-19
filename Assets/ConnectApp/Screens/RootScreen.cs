using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    public class RootScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    top: false,
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: new List<Widget> {
                            new Container(
                                width: 182,
                                height: 32,
                                child: Image.asset("image/iOS/unityConnectBlack.imageset/unityConnectBlack")
                            ),
                            new Container(
                                width: 101,
                                height: 22,
                                margin: EdgeInsets.only(top: 6, bottom: 20),
                                child: Image.asset("image/iOS/madeWithUnity.imageset/madeWithUnity")
                            )
                        })
                ));
        }
    }
}