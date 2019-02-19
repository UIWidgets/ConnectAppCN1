using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class EventCard : StatelessWidget {
        public EventCard(
            Key key = null,
            IEvent model = null
        ) : base(key) {
            this.model = model;
        }

        public readonly IEvent model;

        public override Widget build(BuildContext context) {
            var card = new Container(
                key,
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            decoration: new BoxDecoration(
                                CColors.black
                            ),
                            height: 210,
                            child: new Stack(
                                fit: StackFit.expand,
                                children: new List<Widget> {
                                    Image.network(
                                        model.background,
                                        fit: BoxFit.cover
                                    ),
                                    new Container(
                                        decoration: new BoxDecoration(
                                            CColors.mask
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
                                                    model.title,
                                                    maxLines: 1,
                                                    overflow: TextOverflow.ellipsis,
                                                    style: new TextStyle(
                                                        fontSize: 20,
                                                        color: CColors.text1
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
                                                                CColors.redPoint,
                                                                borderRadius: BorderRadius.all(3)
                                                            ),
                                                            height: 6,
                                                            width: 6
                                                        ),
                                                        new Text(
                                                            model.participantsCount + "人正在观看",
                                                            style: new TextStyle(
                                                                fontSize: 13,
                                                                color: CColors.text1
                                                            )),
                                                        new Flexible(child: new Container()),
                                                        new Container(
                                                            height: 20,
                                                            width: 36,
                                                            decoration: new BoxDecoration(
                                                                model.live ? CColors.redPoint : CColors.black
                                                            ),
                                                            alignment: Alignment.center,
                                                            child: new Text(
                                                                model.live ? "直播" : "录播",
                                                                style: new TextStyle(
                                                                    fontSize: 12,
                                                                    color: CColors.text1
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
                            color: CColors.background2,
                            height: 66,
                            padding: EdgeInsets.fromLTRB(16, 10, 16, 0),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 10),
                                        decoration: new BoxDecoration(
                                            CColors.white
                                        ),
                                        height: 36,
                                        width: 36,
                                        child: Image.network(
                                            model.user.avatar,
                                            fit: BoxFit.cover
                                        )
                                    ),
                                    new Column(
                                        mainAxisAlignment: MainAxisAlignment.start,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Container(height: 5),
                                            new Text(
                                                model.user.username,
                                                style: new TextStyle(
                                                    fontSize: 13,
                                                    color: CColors.text1
                                                )
                                            ),
                                            new Container(height: 5),
                                            new Text(
                                                "5天前发布",
                                                style: new TextStyle(
                                                    fontSize: 13,
                                                    color: CColors.text2
                                                )
                                            )
                                        }
                                    ),
                                    new Flexible(child: new Container()),
                                    new Icon(
                                        Icons.more_vert,
                                        size: 28.0,
                                        color: CColors.icon2
                                    )
                                }
                            )
                        )
                    }
                )
            );
            return new GestureDetector(
                onTap: () => {
                    StoreProvider.store.Dispatch(new NavigatorToLiveAction {eventId = model.id});
                    Navigator.pushName(context, "/detail");
                },
                child: card
            );
        }
    }
}