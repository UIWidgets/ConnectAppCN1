using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public static class CustomListTileConstant {
        public static readonly Icon defaultTrailing = new Icon(
            icon: Icons.baseline_forward_arrow,
            size: 16,
            color: CColors.LightBlueGrey
        );
    }

    public class CustomListTile : StatelessWidget {
        public CustomListTile(
            Widget leading = null,
            string title = null,
            Widget trailing = null,
            EdgeInsets padding = null,
            GestureTapCallback onTap = null,
            Key key = null
        ) : base(key: key) {
            this.leading = leading;
            this.title = title;
            this.trailing = trailing;
            this.padding = padding ?? EdgeInsets.symmetric(horizontal: 16);
            this.onTap = onTap;
        }

        readonly Widget leading;
        readonly string title;
        readonly Widget trailing;
        readonly EdgeInsets padding;
        readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context) {
            var content = new Container(
                height: 60,
                padding: this.padding,
                color: CColors.White,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        this.leading ?? new Container(),
                        this.leading != null ? new Container(width: 12) : new Container(), 
                        new Text(
                            data: this.title,
                            style: CTextStyle.PLargeBody
                        ),
                        new Flexible(child: new Container()),
                        this.trailing ?? new Container()
                    }
                )
            );

            if (this.onTap != null) {
                return new GestureDetector(
                    onTap: this.onTap,
                    child: content
                );
            }

            return content;
        }
    }
}