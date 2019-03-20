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
            GestureTapCallback moreCallBack = null,
            Key key = null
        ) : base(key) {
            this.article = article;
            this.onTap = onTap;
            this.moreCallBack = moreCallBack;
            this.user = user;
        }

        private readonly Article article;
        private readonly User user;
        private readonly GestureTapCallback onTap;
        public readonly GestureTapCallback moreCallBack;

        public override Widget build(BuildContext context) {
            var username = user != null ? user.fullName : "";
            var card = new Container(
                color: CColors.Transparent,
                child: new Padding(
                    padding: EdgeInsets.all(16),
                    child: new Container(
                        child: new Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            crossAxisAlignment: CrossAxisAlignment.start,
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
                                    margin: EdgeInsets.only(top: 8, bottom: 8),
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
                                                        color: new Color(0xFFD8D8D8),
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
                                            new Expanded(
                                                child: new Text(
                                                    $" {username} · {DateConvert.DateStringFromNow(article.publishedTime)} · 阅读 {article.viewCount}",
                                                    style: CTextStyle.TextBody3
                                                )
                                            ),
                                            new CustomButton(
                                                child: new Container(
                                                    height: 28,
                                                    child: new Icon(
                                                        Icons.ellipsis,
                                                        size: 20,
                                                        color: CColors.BrownGrey
                                                    )
                                                ),
                                                onPressed: moreCallBack
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