using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class GlobalLoading : StatelessWidget {
        public GlobalLoading(
            Color color = null,
            LoadingColor loadingColor = LoadingColor.black,
            Key key = null
        ) : base(key: key) {
            this.color = color ?? CColors.White;
            this.loadingColor = loadingColor;
        }

        readonly Color color;
        readonly LoadingColor loadingColor;

        public override Widget build(BuildContext context) {
            return new Container(
                color: this.color,
                child: new Center(
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: new List<Widget> {
                            new CustomActivityIndicator(loadingColor: this.loadingColor)
                        }
                    )
                )
            );
        }
    }
}