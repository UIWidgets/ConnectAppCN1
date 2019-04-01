using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.components {
    public class RelatedArticleCard : StatelessWidget {
        public RelatedArticleCard(
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
            var username = "";
            if (article.ownerType == "user") {
                var userDict = StoreProvider.store.state.userState.userDict;
                if (userDict.ContainsKey(article.userId)) username = userDict[article.userId].fullName;
            }

            if (article.ownerType == "team") {
                var teamDict = StoreProvider.store.state.teamState.teamDict;
                if (teamDict.ContainsKey(article.teamId)) username = teamDict[article.teamId].name;
            }

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
                                        new Text(
                                            article.title,
                                            style: CTextStyle.PLargeTitle,
                                            maxLines: 2,
                                            overflow: TextOverflow.ellipsis,
                                            textAlign: TextAlign.left
                                        ),
                                        new Text(
                                            $"{username} · {DateConvert.DateStringFromNow(article.updatedTime)} · 阅读 {article.viewCount}",
                                            style: CTextStyle.PSmallBody3,
                                            textAlign: TextAlign.left
                                        )
                                    }
                                )
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(8),
                            child: new ClipRRect(
                                borderRadius: BorderRadius.circular(4),
                                child: new Container(
                                    width: 114,
                                    height: 76,
                                    color: new Color(0xFFD8D8D8),
                                    child: Image.network(article.thumbnail.url, fit: BoxFit.fill)
                                )
                            )
                        )
                    }
                )
            );
            return new GestureDetector(
                onTap: onTap,
                child: child
            );
        }
    }
}