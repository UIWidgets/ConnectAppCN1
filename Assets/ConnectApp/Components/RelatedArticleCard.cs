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
        RelatedArticleCard(
            Article article,
            User user = null,
            Team team = null,
            GestureTapCallback onTap = null,
            Key key = null
        ) : base(key) {
            this.article = article;
            this.onTap = onTap;
            this.user = user;
            this.team = team;
        }


        public static RelatedArticleCard User(
            Article article,
            User user = null,
            GestureTapCallback onTap = null,
            Key key = null
        ) {
            return new RelatedArticleCard(
                article, user, null, onTap, key
            );
        }

        public static RelatedArticleCard Team(
            Article article,
            Team team = null,
            GestureTapCallback onTap = null,
            Key key = null
        ) {
            return new RelatedArticleCard(
                article, null, team, onTap, key
            );
        }

        readonly OwnerType type;
        readonly User user;
        readonly Team team;
        readonly Article article;
        readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context) {
            if (this.article == null) {
                return new Container();
            }

            const float imageWidth = 114;
            const float imageHeight = 76;
            const float borderRadius = 4;

            var username = this.user == null ? this.team.name : this.user.fullName;
            var time = this.article.lastPublishedTime == null
                ? this.article.publishedTime
                : this.article.lastPublishedTime;
            var child = new Container(
                color: CColors.White,
                padding: EdgeInsets.all(16),
                child: new Row(
                    children: new List<Widget> {
                        new Expanded(
                            child: new Container(
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.spaceAround,
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Text(this.article.title,
                                            style: CTextStyle.PLargeTitle,
                                            maxLines: 2,
                                            overflow: TextOverflow.ellipsis,
                                            textAlign: TextAlign.left
                                        ),
                                        new Text(
                                            $"{username} · {DateConvert.DateStringFromNow(time)} · 阅读 {this.article.viewCount}",
                                            style: CTextStyle.PSmallBody3,
                                            textAlign: TextAlign.left
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