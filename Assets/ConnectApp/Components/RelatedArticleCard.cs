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
        private RelatedArticleCard(
            Article article,
            User user = null,
            Team team = null,
            OwnerType type = OwnerType.user,
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
        )
        {
            return new RelatedArticleCard(
                article,user,null,OwnerType.user,onTap,key
            );
        }
        public static RelatedArticleCard Team(
            Article article,
            Team team = null,
            GestureTapCallback onTap = null,
            Key key = null
        )
        {
            return new RelatedArticleCard(
                article,null,team,OwnerType.team,onTap,key
            );
        }

        private readonly OwnerType type;
        private readonly User user;
        private readonly Team team;
        private readonly Article article;
        private readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context) {
            var username = user==null?team.name:user.fullName;
//            if (article.ownerType == "user") {
//                var userDict = StoreProvider.store.getState().userState.userDict;
//                if (userDict.ContainsKey(article.userId)) username = userDict[article.userId].fullName;
//            }
//
//            if (article.ownerType == "team") {
//                var teamDict = StoreProvider.store.getState().teamState.teamDict;
//                if (teamDict.ContainsKey(article.teamId)) username = teamDict[article.teamId].name;
//            }

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
                            child: new PlaceholderImage(
                                article.thumbnail.url,
                                114,
                                76,
                                4,
                                BoxFit.fill
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