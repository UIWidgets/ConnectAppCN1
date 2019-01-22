using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace Unity.UIWidgets.Samples.ConnectApp.widgets {
    public class EventCard : StatelessWidget {
        public EventCard(
            string imageSrc
        ) {
            this.imageSrc = imageSrc;
        }

        public string imageSrc;

        public override Widget build(BuildContext context) {
            var card = new Container(
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            decoration: new BoxDecoration(
                                CLColors.black
                            ),
                            height: 210,
                            child: new Stack(
                                fit: StackFit.expand,
                                children: new List<Widget> {
                                    Image.network(this.imageSrc,
                                        fit: BoxFit.cover
                                    ),
                                    new Container(
                                        decoration: new BoxDecoration(
                                            CLColors.mask
                                        )
                                    ),
                                    new Flex(
                                        Axis.vertical,
                                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Padding(
                                                padding: EdgeInsets.fromLTRB(16, 12, 16, 0),
                                                child: new Text(
                                                    "如何在Unity中创建ARCore的应用如何在Unity中创建ARCore的应用",
                                                    maxLines: 1,
                                                    overflow: TextOverflow.ellipsis,
                                                    style: new TextStyle(
                                                        fontSize: 20,
                                                        color: CLColors.text1
                                                    )
                                                )
                                            ),
                                            new Container(
                                                height: 40,
                                                padding: EdgeInsets.symmetric(0, 16),
                                                child: new Row(
                                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                                    children: new List<Widget> {
                                                        new Container(
                                                            margin: EdgeInsets.only(right: 3),
                                                            decoration: new BoxDecoration(
                                                                CLColors.redPoint,
                                                                borderRadius: BorderRadius.all(3)
                                                            ),
                                                            height: 6,
                                                            width: 6
                                                        ),
                                                        new Text(
                                                            "590人正在观看",
                                                            style: new TextStyle(
                                                                fontSize: 13,
                                                                color: CLColors.text1
                                                            )),
                                                        new Flexible(child: new Container()),
                                                        new Container(
                                                            height: 20,
                                                            width: 36,
                                                            decoration: new BoxDecoration(
                                                                CLColors.redPoint
                                                            ),
                                                            alignment: Alignment.center,
                                                            child: new Text(
                                                                "直播",
                                                                style: new TextStyle(
                                                                    fontSize: 12,
                                                                    color: CLColors.text1
                                                                )
                                                            )
                                                        )
                                                    }
                                                )
                                            )
                                        }
                                    )
                                }
                            )
                        ),
                        new Container(
                            color: CLColors.background2,
                            height: 66,
                            padding: EdgeInsets.fromLTRB(16, 10, 16, 0),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 10),
                                        decoration: new BoxDecoration(
                                            CLColors.white,
                                            borderRadius: BorderRadius.all(18)
                                        ),
                                        height: 36,
                                        width: 36
                                    ),
                                    new Column(
                                        mainAxisAlignment: MainAxisAlignment.start,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Container(height: 5),
                                            new Text(
                                                "杨栋",
                                                style: new TextStyle(
                                                    fontSize: 13,
                                                    color: CLColors.text1
                                                )
                                            ),
                                            new Container(height: 5),
                                            new Text(
                                                "5天前发布",
                                                style: new TextStyle(
                                                    fontSize: 13,
                                                    color: CLColors.text2
                                                )
                                            )
                                        }
                                    ),
                                    new Flexible(child: new Container()),
                                    new Icon(
                                        Icons.more_vert,
                                        size: 28.0,
                                        color: CLColors.icon2
                                    )
                                }
                            )
                        )
                    }
                )
            );
            return new GestureDetector(
                onTap: () => Navigator.pushName(context, "/detail"),
                child: card
            );
        }
    }
}