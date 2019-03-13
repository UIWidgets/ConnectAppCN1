using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.components {
    public class ArticleCard : StatelessWidget {
        public ArticleCard(
            Article article,
            User user = null,
            GestureTapCallback onTap = null,
            Key key = null
        ) : base(key) {
            this.article = article;
            this.onTap = onTap;
            this.user = user;
        }

        private readonly Article article;
        private readonly User user;
        private readonly GestureTapCallback onTap;

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
                                            child: new Text(article.title, style: CTextStyle.H5, maxLines: 2)
                                        ),
                                    }
                                ),
                                //title
                                //content
                                new Container(
                                    height: 66,
                                    margin: EdgeInsets.only(0, 8.0f, 0, 8.0f),
                                    child: new Row(
                                        children: new List<Widget> {
                                            new Expanded(
                                                child: new Text(
                                                    article.subTitle,
                                                    style: CTextStyle.PRegular, maxLines: 3,
                                                    overflow: TextOverflow.ellipsis)
                                            ),
                                            new Padding(
                                                padding: EdgeInsets.only(left: 8.0f),
                                                child: new ClipRRect(
                                                    borderRadius: BorderRadius.all(4),
                                                    child: new Container(
                                                        width: 99,
                                                        child: Image.network(article.thumbnail.url, fit: BoxFit.cover)
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
                                            new Text(
                                                $" {user.username} · {DateConvert.DateStringFromNow(article.publishedTime)} · {article.viewCount}",
                                                style: CTextStyle.PSmall),
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
                onTap: onTap
            );
        }
    }
}