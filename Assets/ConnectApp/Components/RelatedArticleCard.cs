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
    public class RelatedArticleCard : StatelessWidget {
        public RelatedArticleCard(
            Article article,
            string fullName,
            GestureTapCallback onTap = null,
            bool topPadding = false,
            Key key = null
        ) : base(key: key) {
            this.article = article;
            this.fullName = fullName;
            this.topPadding = topPadding;
            this.onTap = onTap;
        }

        readonly Article article;
        readonly string fullName;
        readonly bool topPadding;
        readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context) {
            if (this.article == null) {
                return new Container();
            }

            const float imageWidth = 114;
            const float imageHeight = 76;
            const float borderRadius = 4;

            var gap = this.topPadding ? 16 : 0;
            var time = this.article.publishedTime;
            var child = new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, 16 + gap, 16, 16),
                height: 108 + gap,
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Expanded(
                            child: new Container(
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Text(this.article.title,
                                            style: CTextStyle.PLargeTitle,
                                            maxLines: 2,
                                            overflow: TextOverflow.ellipsis,
                                            textAlign: TextAlign.left
                                        ),
                                        new ArticleCardInfo(
                                            fullName: this.fullName,
                                            time: time,
                                            viewCount: this.article.viewCount
                                        )
                                    }
                                )
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(8),
                            child: new PlaceholderImage(this.article.thumbnail.url.EndsWith(".gif")
                                    ? this.article.thumbnail.url
                                    : CImageUtils.SuitableSizeImageUrl(imageWidth, this.article.thumbnail.url),
                                imageWidth,
                                imageHeight,
                                borderRadius,
                                BoxFit.cover
                            )
                        )
                    }
                )
            );
            return new GestureDetector(
                onTap: this.onTap,
                child: child
            );
        }
    }
}