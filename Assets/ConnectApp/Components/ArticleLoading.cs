using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class ArticleLoading : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                height: 162,
                padding: EdgeInsets.only(16, 16, 16),
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            height: 16,
                            margin: EdgeInsets.only(bottom: 16),
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Row(
                            children: new List<Widget> {
                                new Expanded(
                                    child: new Column(
                                        children: new List<Widget> {
                                            new Container(
                                                height: 6,
                                                color: new Color(0xFFF8F8F8)
                                            ),
                                            new Container(
                                                height: 6,
                                                margin: EdgeInsets.only(top: 16),
                                                color: new Color(0xFFF8F8F8)
                                            )
                                        }
                                    )
                                ),
                                new Container(
                                    width: 100,
                                    height: 66,
                                    margin: EdgeInsets.only(16),
                                    color: new Color(0xFFF8F8F8)
                                )
                            }
                        )
                    }
                )
            );
        }
    }
}