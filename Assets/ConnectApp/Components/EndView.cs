using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class EndView : StatelessWidget {
        public EndView(
            string title = "THE END",
            bool hasBottomMargin = false,
            Key key = null
        ) : base(key: key) {
            this.title = title;
            this.hasBottomMargin = hasBottomMargin;
        }

        readonly string title;
        readonly bool hasBottomMargin;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.Background,
                padding: EdgeInsets.only(0, 16, 0,
                    this.hasBottomMargin
                        ? 16 + CConstant.TabBarHeight + MediaQuery.of(context).padding.bottom
                        : 16
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: new List<Widget> {
                        new Container(
                            width: 20,
                            height: 1,
                            color: CColors.Separator
                        ),
                        new Padding(
                            padding: EdgeInsets.only(8, right: 8),
                            child: new Text(
                                data: this.title,
                                style: CTextStyle.PSmallBody4,
                                textAlign: TextAlign.center
                            )
                        ),
                        new Container(
                            width: 20,
                            height: 1,
                            color: CColors.Separator
                        )
                    }
                )
            );
        }
    }
}