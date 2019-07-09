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
            Key key = null
        ) : base(key: key) {
            this.title = title;
        }

        readonly string title;

        public override Widget build(BuildContext context) {
            return new Container(
                height: 52,
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