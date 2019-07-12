using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class ArticleDetailLoading : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, right: 16, top: 16),
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            height: 12,
                            margin: EdgeInsets.only(bottom: 24),
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Container(
                            height: 12,
                            margin: EdgeInsets.only(bottom: 40, right: 100),
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Container(
                            height: 6,
                            margin: EdgeInsets.only(bottom: 24),
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Container(
                            height: 6,
                            margin: EdgeInsets.only(bottom: 24),
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Container(
                            height: 6,
                            margin: EdgeInsets.only(bottom: 24),
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Container(
                            height: 6,
                            margin: EdgeInsets.only(right: 100),
                            color: new Color(0xFFF8F8F8)
                        )
                    }
                )
            );
        }
    }
}