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
            GestureTapCallback onTap = null,
            Key key = null
        ) : base(key) {
            this.article = article;
            this.onTap = onTap;
        }

        private readonly Article article;
        private readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context) {
            var card = new Container(
                child: new Padding(
                    padding: EdgeInsets.all(16),
                    child: new Container(
                        child: new Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                new Container(
                                    child: new Text(article.title, style: CTextStyle.H5, maxLines: 2)
                                ),
                                new Container(
                                    height: 66,
                                    margin: EdgeInsets.only(top: 8, right: 8),
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.end,
                                        children: new List<Widget> {
                                            new Expanded(
                                                child: new Container(
                                                    child: new Text(
                                                        article.subTitle,
                                                        style: CTextStyle.PRegular, maxLines: 3,
                                                        overflow: TextOverflow.ellipsis)
                                                )
                                            ),
                                            new Padding(
                                                padding: EdgeInsets.only(8.0f),
                                                child: new ClipRRect(
                                                    borderRadius: BorderRadius.all(4),
                                                    child: new Container(
                                                        width: 100,
                                                        height: 66,
                                                        color: Color.fromRGBO(245, 245, 245, 1),
                                                        child: Image.network(article.thumbnail.url, fit: BoxFit.cover)
                                                    )
                                                )
                                            )
                                        }
                                    )
                                ),
                                new Container(
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                        children: new List<Widget> {
                                            new Text(
                                                $" username · {DateConvert.DateStringFromNow(article.publishedTime)} · {article.viewCount}",
                                                style: CTextStyle.PSmall),
                                            new CustomButton(
                                                child: new Container(
                                                    child: new Icon(
                                                        Icons.ellipsis,
                                                        size: 28,
                                                        color: Color.fromRGBO(181, 181, 181, 1)
                                                    )
                                                ),
                                                onPressed: () => {
                                                    ShareUtils.showShareView(context, new ShareView());
                                                }
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