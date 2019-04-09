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
    public class ArticleCard : StatelessWidget {
        public ArticleCard(
            Article article,
            string userName,
            GestureTapCallback onTap = null,
            GestureTapCallback moreCallBack = null,
            Key key = null
        ) : base(key) {
            this.article = article;
            this.userName = userName;
            this.onTap = onTap;
            this.moreCallBack = moreCallBack;
        }

        private readonly Article article;
        private readonly string userName;
        private readonly GestureTapCallback onTap;
        private readonly GestureTapCallback moreCallBack;

        public override Widget build(BuildContext context) {
//            var username = "";
//            if (article.ownerType == "user") {
//                var userDict = StoreProvider.store.getState().userState.userDict;
//                if (userDict.ContainsKey(article.userId)) username = userDict[article.userId].fullName;
//            }
//
//            if (article.ownerType == "team") {
//                var teamDict = StoreProvider.store.getState().teamState.teamDict;
//                if (teamDict.ContainsKey(article.teamId)) username = teamDict[article.teamId].name;
//            }

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
                                                    $"{userName} · {DateConvert.DateStringFromNow(article.publishedTime)} · 阅读 {article.viewCount}",
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