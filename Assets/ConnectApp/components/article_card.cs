using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.components {
    public class ArticleCard : StatefulWidget {
        public ArticleCard(
            IEvent model = null,
            Key key = null
        ) : base(key) {
            this._model = model;
        }

        private readonly IEvent _model;
        public override State createState() {
            return new _ArticleCardState();
        }
    }

    internal class _ArticleCardState : State<ArticleCard> {
        public override Widget build(BuildContext context) {
            var card = new Container(
                child: new Padding(
                    padding: EdgeInsets.all(16),
                    child: new Container(
                        child: new Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            children: new List<Widget> {
                                new Row(
                                    children: new List<Widget> {
                                        new Container(
                                            width: MediaQuery.of(context).size.width - 32,
                                            child: new Text("粒子特效教程 | 多重GPU粒子力场", style: CTextStyle.H5, maxLines: 2)
                                        ),
                                    }
                                ),
                                //title
                                //content
                                new Container(
                                    height: 66,
                                    margin: EdgeInsets.only(0, 8.0f, 0, 8.0f),
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.end,
                                        children: new List<Widget> {
                                            new Container(
                                                width: MediaQuery.of(context).size.width - 139,
                                                child: new Text(
                                                    "我们将分享加拿大游戏特效大神Mirza Beig的粒子特效的系列教程,该系列教程将帮助你了解如何使用粒子系统制作精美的特效。",
                                                    style: CTextStyle.PRegular, maxLines: 3,
                                                    overflow: TextOverflow.ellipsis)
                                            ),
                                            new Padding(
                                                padding: EdgeInsets.only(left: 8.0f),
                                                child: new ClipRRect(
                                                    borderRadius: BorderRadius.all(4),
                                                    child: new Container(
                                                        width: 99,
                                                        child: Image.asset("pikachu", fit: BoxFit.cover)
                                                    )
                                                )
                                            ),
                                        }
                                    )
                                ),
                                //bottom
                                new Container(
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                        children: new List<Widget> {
                                            new Text("Unity China · 刚刚 · 390", style: CTextStyle.PSmall),
                                            new GestureDetector(
                                                child: new Container(
                                                    height: 20,
                                                    child: new Icon(Icons.ellipsis, null, 20,
                                                        Color.fromRGBO(181, 181, 181, 1))),
                                                onTap: () => { }
                                            )
                                        }
                                    )
                                )
                            }
                        )
                    )
                )
            );
            return new GestureDetector(
                child: card,
                onTap: () =>
                {
//                    StoreProvider.store.Dispatch(new NavigatorToLiveAction {eventId = _model.id});
                    Navigator.pushName(context, "/detail");
                }
            );
        }
    }
}