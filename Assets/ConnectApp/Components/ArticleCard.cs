using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class ArticleCard : StatelessWidget {
        public ArticleCard(
            Article article,
            GestureTapCallback onTap = null,
            GestureTapCallback moreCallBack = null,
            string fullName = null,
            Key key = null
        ) : base(key: key) {
            this.article = article;
            this.fullName = fullName;
            this.onTap = onTap;
            this.moreCallBack = moreCallBack;
        }

        readonly Article article;
        readonly string fullName;
        readonly GestureTapCallback onTap;
        readonly GestureTapCallback moreCallBack;

        public override Widget build(BuildContext context) {
            if (this.article == null) {
                return new Container();
            }

            const float imageWidth = 100;
            const float imageHeight = 66;
            const float borderRadius = 4;

            var time = this.article.lastPublishedTime == null
                ? this.article.publishedTime
                : this.article.lastPublishedTime;
            var imageUrl = this.article.thumbnail.url.EndsWith(".gif")
                ? this.article.thumbnail.url
                : CImageUtils.SuitableSizeImageUrl(imageWidth + 1, this.article.thumbnail.url);
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
                                    child: new Text(this.article.title,
                                        style: CTextStyle.H5,
                                        maxLines: 2,
                                        textAlign: TextAlign.left,
                                        overflow: TextOverflow.ellipsis
                                    )
                                ),
                                new Container(
                                    margin: EdgeInsets.only(top: 8, bottom: 8),
                                    child: new Row(
                                        children: new List<Widget> {
                                            new Expanded(
                                                child: new Text(this.article.subTitle,
                                                    style: CTextStyle.PRegularBody,
                                                    maxLines: 3,
                                                    overflow: TextOverflow.ellipsis
                                                )
                                            ),
                                            new Container(
                                                margin: EdgeInsets.only(8.0f),
                                                child: new PlaceholderImage(
                                                    imageUrl,
                                                    imageWidth,
                                                    imageHeight,
                                                    borderRadius,
                                                    BoxFit.cover
                                                )
                                            )
                                        }
                                    )
                                ),
                                new Container(
                                    height: 20,
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                        children: new List<Widget> {
                                            new Expanded(
                                                child: new ArticleInfoCard(
                                                    fullName: this.fullName,
                                                    time: time,
                                                    viewCount: this.article.viewCount
                                                )
                                            ),
                                            new SizedBox(width: 8),
                                            new GestureDetector(
                                                child: new Container(
                                                    height: 20,
                                                    width: 20,
                                                    color:CColors.White,
                                                    child: new Icon(
                                                        Icons.ellipsis,
                                                        size: 20,
                                                        color: CColors.BrownGrey
                                                    )
                                                ),
                                                onTap: this.moreCallBack
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
                onTap: this.onTap
            );
        }
    }
}