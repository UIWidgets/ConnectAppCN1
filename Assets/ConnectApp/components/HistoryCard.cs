using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class HistoryCard : StatefulWidget {
        public HistoryCard(
            Key key = null
        ) : base(key) {
        }

        public override State createState() {
            return new _HistoryCardState();
        }
    }

    internal class _HistoryCardState : State<HistoryCard> {
        public override Widget build(BuildContext context) {
            var card = new Container(
                height: 100,
                padding: EdgeInsets.fromLTRB(16, 16, 16, 0),
                decoration: new BoxDecoration(
                    CColors.background2
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        Image.asset(
                            "yoshi",
                            height: 84,
                            width: 150,
                            fit: BoxFit.fill
                        ),
                        new Container(width: 16),
                        new Flexible(
                            child: new Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Text(
                                        "迪士尼电视动画与Unity联袂合作，开创实时动画新纪元",
                                        maxLines: 3,
                                        style: new TextStyle(
                                            fontSize: 15,
                                            fontWeight: FontWeight.w700,
                                            color: CColors.text1,
                                            height: 1.3f
                                        )
                                    ),
                                    new Container(height: 8),
                                    new Text(
                                        "1920次观看",
                                        maxLines: 1,
                                        style: new TextStyle(
                                            fontSize: 12,
                                            color: CColors.text2
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            );

            return new GestureDetector(
                onTap: () => Router.navigator.pushNamed("/detail"),
                child: card
            );
        }
    }
}