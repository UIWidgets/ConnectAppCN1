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
        private ArticleCard(
            Article article,
            GestureTapCallback onTap = null,
            GestureTapCallback moreCallBack = null,
            Key key = null,
            User user = null,
            Team team = null
        ) : base(key) {
            this.article = article;
            this.user = user;
            this.team = team;
            this.onTap = onTap;
            this.moreCallBack = moreCallBack;
        }

        private readonly Article article;
        private readonly User user;
        private readonly Team team;
        private readonly GestureTapCallback onTap;
        private readonly GestureTapCallback moreCallBack;

        public static ArticleCard User(
            Article article,
            GestureTapCallback onTap = null,
            GestureTapCallback moreCallBack = null,
            Key key = null,
            User user = null
        ) {
            return new ArticleCard(
                article, onTap, moreCallBack, key, user
            );
        }

        public static ArticleCard Team(
            Article article,
            GestureTapCallback onTap = null,
            GestureTapCallback moreCallBack = null,
            Key key = null,
            Team team = null
        ) {
            return new ArticleCard(
                article, onTap, moreCallBack, key, null, team
            );
        }

        public override Widget build(BuildContext context) {
            var userName = article.ownerType == OwnerType.team.ToString() ? team.name : user.fullName;
            var time = article.lastPublishedTime == null ? article.publishedTime : article.lastPublishedTime;
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
                                                    article.bodyPlain,
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
                                                    article.thumbnail.url,
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
                                                    $"{userName} · {DateConvert.DateStringFromNow(time)} · 阅读 {article.viewCount}",
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