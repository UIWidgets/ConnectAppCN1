using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
    public class BlankView : StatelessWidget {
        public BlankView(
            string title,
            string imageName = null,
            bool canRefresh = false,
            GestureTapCallback tapCallback = null,
            Decoration decoration = null,
            Key key = null
        ) : base(key: key) {
            this.title = title;
            this.imageName = imageName;
            this.canRefresh = canRefresh;
            this.tapCallback = tapCallback;
            this.decoration = decoration ?? new BoxDecoration(color: CColors.White);
        }

        readonly string title;
        readonly string imageName;
        readonly bool canRefresh;
        readonly GestureTapCallback tapCallback;
        readonly Decoration decoration;

        public override Widget build(BuildContext context) {
            var imageName = HttpManager.isNetWorkError() ? "image/default-network" : this.imageName;
            var message = HttpManager.isNetWorkError() ? "数据不见了，快检查下网络吧" : $"{this.title}";
            return new Container(
                decoration: this.decoration,
                width: MediaQuery.of(context).size.width,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        imageName != null
                            ? new Container(
                                margin: EdgeInsets.only(bottom: 24),
                                child: Image.asset(
                                    name: imageName,
                                    width: 128,
                                    height: 128
                                )
                            )
                            : new Container(),
                        new Container(
                            margin: EdgeInsets.only(bottom: 24),
                            child: new Text(
                                data: message,
                                style: new TextStyle(
                                    fontSize: 16,
                                    fontFamily: "Roboto-Regular",
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