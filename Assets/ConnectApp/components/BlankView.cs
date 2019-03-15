using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.widgets;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.painting;

namespace ConnectApp.components {
    public class BlankView : StatelessWidget {
        public BlankView(
            Key key,
            string title
        ) : base(key) {
            this.title = title;
        }

        public readonly string title;

        public override Widget build(BuildContext context) {
            var width = MediaQuery.of(context).size.width;
            return new Container(
                color: CColors.background1,
                width: width,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget>{
                        new Text(
                            title ?? "",
                            style: new TextStyle(
                                color: CColors.text2,
                                fontFamily: "PingFang-Regular",
                                fontSize: 15
                            )
                        )
                    }
                )
            );
        }
    }
}