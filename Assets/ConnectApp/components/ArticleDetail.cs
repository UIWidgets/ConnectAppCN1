using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using ConnectApp.api;
using ConnectApp.components.refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class ArticleDetail : StatelessWidget {
        public ArticleDetail(
            Project articleDetail,
            Key key = null
        ) : base(key) {
            this.articleDetail = articleDetail;
        }

        public readonly Project articleDetail;
        private Article _article;
        private List<Article> _relArticles;
        
        public override Widget build(BuildContext context) {
            _article = articleDetail.projectData;
            _relArticles = articleDetail.projects;
            return new Container(child: _content(context));
        }

        private Widget _content(BuildContext context) {
            return new Flexible(
                child:new Refresh(
                    onFooterRefresh:onFooterRefresh,
                    child: new ListView(
                        physics: new AlwaysScrollableScrollPhysics(),
                        children: new List<Widget> {
                            _contentHead(context),
                            _subTitle(context),
                            _contentDetail(context),
                            _actionCards(context),
                            _relatedArticles(context),
                            _comments(context)
                        }
                    )
                )
                
            );
        }

        private IPromise onFooterRefresh() {
//            pageNumber++;
            return ArticleApi.FetchArticles(1)
                .Then((articlesResponse) => {
                    if (articlesResponse.items.Count != 0) {
                        var articleList = StoreProvider.store.state.articleState.articleList;
                        var articleDict = StoreProvider.store.state.articleState.articleDict;
                        articlesResponse.items.ForEach((item) => {
//                            if (!articleDict.Keys.Contains(item.id)) {
//                                articleList.Add(item.id);
//                                articleDict.Add(item.id, item);
//                            }
                        });
//                        StoreProvider.store.Dispatch(new FetchArticleSuccessAction
//                            {ArticleDict = articleDict, ArticleList = articleList});
                    }
                })
                .Catch(error => { Debug.Log(error); });
        }
        
        
        private Widget _contentHead(BuildContext context)
        {
            
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text(
                            _article.title,
                            style: CTextStyle.H3
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 8),
                            child: new Text(
                                $"阅读 { _article.viewCount } · {DateConvert.DateStringFromNow(_article.createdTime)}",
                                style: new TextStyle(
                                    height: 1.67f,
                                    fontSize: 12,
                                    fontFamily: "PingFang-Regular",
                                    color: CColors.TextBody4
                                    )
                                )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 24, bottom: 24),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.start,
                                crossAxisAlignment: CrossAxisAlignment.center,
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 8),
                                        child: new ClipRRect(
                                            borderRadius: BorderRadius.circular(16),
                                            child: new Container(
                                                height: 32,
                                                width: 32,
                                                child: Image.network(
                                                    _article.headerImage.url,
                                                    fit: BoxFit.cover
                                                )
                                            )
                                        )
                                    ),


                                    new Column(
                                        mainAxisAlignment: MainAxisAlignment.start,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Container(height: 5),
                                            new Text(
//                                                articleDetail.projectData..fullName,
                                                "user",
                                                style: CTextStyle.PRegular
                                            ),
                                            new Container(height: 5),
                                            new Text(
                                                DateConvert.DateStringFromNow(_article.createdTime),
                                                style: CTextStyle.PSmall
                                            )
                                        }
                                    )
                                }
                            )),
                    }
                )
            );
        }


        private Widget _subTitle(BuildContext context)
        {
            
            return new Container(
                decoration:new BoxDecoration(
                   color:CColors.Separator2,
                   borderRadius:BorderRadius.all(4)
                ),
                   
                margin:EdgeInsets.only(bottom:24,left:16,right:16),
                child:new Container(
                    padding:EdgeInsets.only(16,12,16,12), 
                    child:new Text($"{_article.subTitle}",style:CTextStyle.PLargeGray)
                    )  
            );
        }


        private Widget _contentDetail(BuildContext context) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new EventDescription(content: _article.body, contentMap: articleDetail.contentMap)
                    }
                )
            );
        }
        
        private Widget _actionCards(BuildContext context)
        {
            return new Container(
                padding:EdgeInsets.only(top:40,bottom:40),
                child: new Row(
                    mainAxisAlignment:MainAxisAlignment.center,
                    crossAxisAlignment:CrossAxisAlignment.center,
                    children:new List<Widget>{
                        new ActionCard(Icons.favorite,"点赞",false),
                        new Container(width:16),
                        new ActionCard(Icons.bookmark,"收藏",false),
                    }
                ) 

            );
        }

        private Widget _relatedArticles(BuildContext context)
        {
            return new Container(
                padding:EdgeInsets.only(left:16,right:16),
                child: new Column(children:new List<Widget>
                {
                    new Container(height:1,color:CColors.Separator2,margin:EdgeInsets.only(bottom:24)),
                    new Container(
                        child: new Column(
                            children: _buildArticles()
                        )
                    )
                }) 

            );
        }

        List<Widget> _buildArticles()
        {
            var widgets = new List<Widget>();
            _relArticles.ForEach((article) =>
            {
                widgets.Add(new RelatedArticleCard(article)); 
            });
            return widgets;
        }

        private Widget _comments(BuildContext context)
        {
            return new Container(
                padding:EdgeInsets.only(left:16,right:16),
                child: new Column(
                    crossAxisAlignment:CrossAxisAlignment.start,
                    children:new List<Widget>{
                    new Text("评论",style:CTextStyle.H5,textAlign:TextAlign.left),
                    new Container(
                        child: new Column(
                            children: new List<Widget>
                            {
                                new CommentCard(),
                                new CommentCard(),
                                new CommentCard(),
                                new CommentCard(),
                            }
                        )
                    )
                }) 

            );
        }

    }
}