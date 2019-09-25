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
            Key key = null
        ) : base(key: key) {
            this.article = article;
            this.fullName = fullName;
            this.onTap = onTap;
        }

        readonly Article article;
        readonly string fullName;
        readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context) {
            if (this.article == null) {
                return new Container();
            }

            const float imageWidth = 114;
            const float imageHeight = 76;
            const float borderRadius = 4;

            var time = this.article.publishedTime;
            var thumbnailUrl = this.article.thumbnail?.url ?? "";
            var imageUrl = thumbnailUrl.EndsWith(".gif")
                ? thumbnailUrl
                : CImageUtils.SuitableSizeImageUrl(imageWidth: imageWidth, imageUrl: thumbnailUrl);
            var child = new Container(
                color: CColors.White,
                padding: EdgeInsets.all(16),
                height: 108,
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Expanded(
                            child: new Container(
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Text(
                                            data: this.article.title,
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
                            child: new PlaceholderImage(
                                imageUrl: imageUrl,
                                width: imageWidth,
                                height: imageHeight,
                                borderRadius: borderRadius,
                                fit: BoxFit.cover
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