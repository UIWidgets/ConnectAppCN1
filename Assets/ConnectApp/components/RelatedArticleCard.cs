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
using TextStyle = Unity.UIWidgets.painting.TextStyle;

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
            var user = new User();
            if (StoreProvider.store.state.userState.userDict.ContainsKey(article.userId)) {
                user = StoreProvider.store.state.userState.userDict[article.userId];
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
                                          style:new TextStyle(
                                              height: 1.5f,
                                              fontSize: 16,
                                              fontFamily: "PingFang-Regular",
                                              color: CColors.TextTitle
                                            ),
                                          maxLines:2,
                                          overflow:TextOverflow.ellipsis,
                                          textAlign:TextAlign.left
                                      ),
                                      new Text(
                                          $"{ user.fullName } · { DateConvert.DateStringFromNow(article.updatedTime)} · 阅读 {article.viewCount }",
                                          style:CTextStyle.TextBody3,
                                          textAlign:TextAlign.left
                                      )
                                  }
                              )
                          )
                      ),
                      new Container(
                          margin:EdgeInsets.only(8),
                          child:new ClipRRect(
                              borderRadius: BorderRadius.circular(4),
                              child: new Container(
                                  width: 114,
                                  height: 76,
                                  color: new Color(0xFFD8D8D8),
                                  child: Image.network(article.thumbnail.url,fit:BoxFit.fill)
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