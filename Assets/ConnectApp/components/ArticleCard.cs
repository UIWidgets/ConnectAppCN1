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
            var username = user != null ? user.username : "";
            var card = new Container(
                color: CColors.Transparent,
                child: new Padding(
                    padding: EdgeInsets.all(16),
                    child: new Container(
                        child: new Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            crossAxisAlignment:CrossAxisAlignment.start,
                            children: new List<Widget> {
                                new Container(
                                    child: new Text(
                                        article.title,
                                        style: CTextStyle.H5,
                                        maxLines: 2,
                                        textAlign: TextAlign.left
                                    )
                                ),
                                new Container(
                                    margin: EdgeInsets.only(top: 8, bottom:8),
                                    child: new Row(
                                        children: new List<Widget> {
                                            new Expanded(
                                                child: new Text(
                                                    article.bodyPlain,
                                                    style: CTextStyle.PRegular,
                                                    maxLines: 3,
                                                    overflow: TextOverflow.ellipsis
                                                )
                                            ),
                                            new Container(
                                                margin: EdgeInsets.only(8.0f),
                                                width: 100,
                                                height: 66,
                                                child: new ClipRRect(
                                                    borderRadius: BorderRadius.all(4),
                                                    child: new Container(
                                                        color: Color.fromRGBO(245, 245, 245, 1),
                                                        child: Image.network(article.thumbnail.url, fit: BoxFit.cover)
                                                    )
                                                )
                                            ),
                                        }
                                    )
                                ),
                                new Container(
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                        children: new List<Widget> {
                                            new Text(
                                                $" {username} · {DateConvert.DateStringFromNow(article.publishedTime)} · {article.viewCount}",
                                                style: CTextStyle.PSmall
                                            ),
                                            new CustomButton(
                                                child: new Container(
                                                    height: 28,
                                                    child: new Icon(
                                                        Icons.ellipsis,
                                                        size:20,
                                                        color:Color.fromRGBO(181, 181, 181, 1)
                                                    )
                                                ),
                                                onPressed: () => ShareUtils.showShareView(context, new ShareView())
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