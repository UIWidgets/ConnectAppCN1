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

namespace ConnectApp.components {
    public class ArticleCard : StatelessWidget {
        public ArticleCard(
            Article article,
            GestureTapCallback onTap = null,
            GestureTapCallback moreCallBack = null,
            string fullName = null,
            Key key = null
        ) : base(key) {
            this.article = article;
            this.fullName = fullName;
            this.onTap = onTap;
            this.moreCallBack = moreCallBack;
        }

        private readonly Article article;
        private readonly string fullName;
        private readonly GestureTapCallback onTap;
        private readonly GestureTapCallback moreCallBack;

        public override Widget build(BuildContext context) {
            if (article == null) return new Container();

            var time = article.lastPublishedTime == null ? article.publishedTime : article.lastPublishedTime;
            var imageUrl = article.thumbnail.url.EndsWith(".gif")
                ? article.thumbnail.url
                : $"{article.thumbnail.url}.300x0x1.jpg";
            var card = new Container(
                color: CColors.White,
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
                                                    article.subTitle,
                                                    style: CTextStyle.PRegularBody,
                                                    maxLines: 3,
                                                    overflow: TextOverflow.ellipsis
                                                )
                                            ),
                                            new Container(
                                                margin: EdgeInsets.only(8.0f),
                                                width: 100,
                                                height: 66,
                                                child: new PlaceholderImage(
                                                    imageUrl,
                                                    100,
                                                    66,
                                                    4,
                                                    BoxFit.cover
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
                                                    $"{fullName} · {DateConvert.DateStringFromNow(time)} · 阅读 {article.viewCount}",
                                                    style: CTextStyle.PSmallBody3
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