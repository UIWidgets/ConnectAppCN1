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

namespace ConnectApp.components
{
    public class RelatedArticleCard : StatelessWidget
    {
        public RelatedArticleCard(
             Article article, 
             GestureTapCallback onTap = null,
             Key key = null
        ) : base(key)
        {
            this.article = article;
            this.onTap = onTap;
        }

        private readonly Article article;
        private GestureTapCallback onTap;
        
        public override Widget build(BuildContext context)
        {
            var user = StoreProvider.store.state.userState.UserDict[article.userId];
            var child = new Container(
                padding:EdgeInsets.only(top:16,bottom:16),
                child:new Row(
                  children:new List<Widget>
                  {
                      new Expanded(
                          child:new Container(
                              child:new Column(
                                  crossAxisAlignment:CrossAxisAlignment.start,
                                  children:new List<Widget>
                                  {
                                      new Text(article.title,style:new TextStyle(
                                          height: 1.5f,
                                          fontSize: 16,
                                          fontFamily: "PingFangSC-Regular",
                                          color: CColors.TextTitle
                                      ),maxLines:2,overflow:TextOverflow.ellipsis,textAlign:TextAlign.left),
                                      new Text($"{ user.username } · { DateConvert.DateStringFromNow(article.updatedTime)} · 阅读 {article.viewCount }",style:new TextStyle(
                                          height: 1.67f,
                                          fontSize: 12,
                                          fontFamily: "PingFangSC-Regular",
                                          color: CColors.TextThird    
                                      ),textAlign:TextAlign.left)
                                  }
                              )
                          )
                      ),
                      new Container(
                          margin:EdgeInsets.only(left:8),
                          child:new ClipRRect(
                              borderRadius: BorderRadius.circular(4),
                              child: new Container(
                                  width:114,
                                  height:76,
                                  child: Image.network(article.thumbnail.url,fit:BoxFit.fill)
                              )
                          )
                      ),
                      
                  }      
                )
            );
            return  new GestureDetector(
                onTap: onTap,
                child: child
            );
            
        }
    }
}