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

            var time = this.article.publishedTime;
            var thumbnailUrl = this.article.thumbnail?.url ?? "";
            var imageUrl = CImageUtils.SuitableSizeImageUrl(imageWidth: imageWidth, imageUrl: thumbnailUrl);
            // var imageUrl = thumbnailUrl.EndsWith(".gif")
            //     ? thumbnailUrl
            //     : CImageUtils.SuitableSizeImageUrl(imageWidth: imageWidth, imageUrl: thumbnailUrl);
            var card = new Container(
                color: CColors.White,
                padding: EdgeInsets.only(top: 16),
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            padding: EdgeInsets.symmetric(horizontal: 16),
                            child: new Text(
                                data: this.article.title,
                                style: CTextStyle.PXLargeMedium.copyWith(height:1.44f),
                                maxLines: 2,
                                textAlign: TextAlign.left,
                                overflow: TextOverflow.ellipsis
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 8, bottom: 8),
                            padding: EdgeInsets.symmetric(horizontal: 16),
                            child: new Row(
                                children: new List<Widget> {
                                    new Expanded(
                                        child: new Container(
                                            height: imageHeight,
                                            child: new Column(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: new List<Widget> {
                                                    new Text(
                                                        data: this.article.subTitle,
                                                        style: CTextStyle.PRegularBody2,
                                                        maxLines: 3,
                                                        overflow: TextOverflow.ellipsis
                                                    )
                                                }
                                            )
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(8.0f),
                                        child: new PlaceholderImage(
                                            imageUrl: imageUrl,
                                            width: imageWidth,
                                            height: imageHeight,
                                            borderRadius: borderRadius,
                                            fit: BoxFit.cover,
                                            true,
                                            CColorUtils.GetSpecificDarkColorFromId(id: this.article.id)
                                        )
                                    )
                                }
                            )
                        ),
                        new Container(
                            height: 36,
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Expanded(
                                        child: new Container(
                                            height: 20,
                                            padding: EdgeInsets.only(16),
                                            alignment: Alignment.topLeft,
                                            child: new ArticleCardInfo(
                                                fullName: this.fullName,
                                                time: time,
                                                viewCount: this.article.viewCount
                                            )
                                        )
                                    ),
                                    new CustomButton(
                                        padding: EdgeInsets.only(16, right: 16, bottom: 16),
                                        child: new Icon(
                                            icon: Icons.ellipsis,
                                            size: 20,
                                            color: CColors.BrownGrey
                                        ),
                                        onPressed: this.moreCallBack
                                    )
                                }
                            )
                        )
                    }
                )
            );

            return new GestureDetector(
                child: card,
                onTap: this.onTap
            );
        }
    }
}