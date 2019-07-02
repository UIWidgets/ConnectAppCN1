using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.Components {
    public class BlankView : StatelessWidget {
        public BlankView(
            string title,
            string imageName,
            bool canRefresh = false,
            GestureTapCallback tapCallback = null,
            Key key = null
        ) : base(key: key) {
            this.title = title;
            this.imageName = imageName;
            this.canRefresh = canRefresh;
            this.tapCallback = tapCallback;
        }

        readonly string title;
        readonly string imageName;
        readonly bool canRefresh;
        readonly GestureTapCallback tapCallback;

        public override Widget build(BuildContext context) {
            var isNetWorkError = Application.internetReachability == NetworkReachability.NotReachable;
            var imageName = isNetWorkError ? "image/default-network" : this.imageName;
            var message = isNetWorkError ? "数据不见了，快检查下网络吧" : $"{this.title}";
            return new Container(
                color: CColors.White,
                width: MediaQuery.of(context).size.width,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(bottom: 24),
                            child: Image.asset(
                                name: imageName,
                                width: 128,
                                height: 128
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(bottom: 24),
                            child: new Text(
                                data: message,
                                style: new TextStyle(
                                    height: 1.33f,
                                    fontSize: 16,
                                    fontFamily: "Roboto-Medium",
                                    color: new Color(0xFF959595)
                                )
                            )
                        ),
                        this._buildRefreshButton()
                    }
                )
            );
        }

        Widget _buildRefreshButton() {
            if (!this.canRefresh) {
                return new Container();
            }
            return new CustomButton(
                onPressed: this.tapCallback,
                padding: EdgeInsets.zero,
                child: new Container(
                    width: 128,
                    height: 48,
                    alignment: Alignment.center,
                    decoration: new BoxDecoration(
                        color: CColors.White,
                        borderRadius: BorderRadius.circular(24),
                        border: Border.all(color: CColors.PrimaryBlue)
                    ),
                    child: new Text(
                        "刷新",
                        style: CTextStyle.PLargeBlue
                    )
                )
            );
        }
    }
}