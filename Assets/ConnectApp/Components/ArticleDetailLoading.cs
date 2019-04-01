using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class ArticleDetailLoading : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                height: MediaQuery.of(context).size.height,
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
                            color: new Color(0xFFF8F8F8)),
                    }
                )
            );
        }
    }
}