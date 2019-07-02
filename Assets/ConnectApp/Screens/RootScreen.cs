using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.screens {
    public class RootScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            if (Application.platform == RuntimePlatform.Android) {
                return new Container(
                    color: new Color(0x231F20));
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.end,
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: new List<Widget> {
                            new Container(
                                width: 228,
                                height: 40,
                                child: Image.asset("image/iOS/unityConnectBlack.imageset/unityConnectBlack",
                                    fit: BoxFit.cover)
                            ),
                            new Container(
                                width: 102,
                                height: 22,
                                margin: EdgeInsets.only(bottom: 16, top: 16),
                                child: Image.asset("image/iOS/madeWithUnity.imageset/madeWithUnity", fit: BoxFit.cover))
                        })
                ));
        }
    }
}