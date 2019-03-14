using System.Collections.Generic;
using System.Linq;
using ConnectApp.api;
using ConnectApp.components;
using ConnectApp.components.refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Icons = ConnectApp.constants.Icons;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.screens
{
    public class ArticleDetailScreen : StatefulWidget
    {
        public ArticleDetailScreen(
            Key key = null
        ) : base(key)
        {
            
        }
        
        public override State createState()
        {
            return new _ArticleDetailScreenState();
        }
        
    }

    internal class _ArticleDetailScreenState : State<ArticleDetailScreen>
    {

        private Article _article = null;
        private User _user = null;
        private string _channelId = null;
        private List<string> _channelComments = new List<string>();
        private List<Article> _relArticles = new List<Article>();
        private Dictionary<string, ContentMap> _contentMap = new Dictionary<string, ContentMap>();
        private string _lastCommentId = null;
        
        
        public override void initState()
        {
            base.initState();
            StoreProvider.store.Dispatch(new FetchArticleDetailAction()
                {articleId = StoreProvider.store.state.articleState.detailId});
        }

        public override Widget build(BuildContext context)
        {
            return new StoreConnector<AppState, Dictionary<string, object>>(
                converter: (state, dispatcher) => new Dictionary<string, object> {
                    {"articleDetail", state.articleState.articleDetail},
                    {"channelMessageDict",state.messageState.channelMessageDict},
                    {"channelMessageList",state.messageState.channelMessageList},
                    {"userDict",state.userState.UserDict}
                },
                builder: (context1, viewModel) => {
                    if (StoreProvider.store.state.articleState.articleDetailLoading)
                    {
                        return new Container(
                            color: CColors.White,
                            child: new Container(child: new CustomActivityIndicator())
                        ); 
                    }
                    var articleDetail = (Project) viewModel["articleDetail"];
                    var channelMessageList = (Dictionary<string, List<string>>) viewModel["channelMessageList"];
                    var userDict = (Dictionary<string, User>) viewModel["userDict"];
                    if (articleDetail == null) return new Container();
                    _article = articleDetail.projectData;
                    if (_article.userId!=null&&userDict.TryGetValue(_article.userId,out _user))
                    {
                        _user = userDict[_article.userId];
                    }

                    _channelId = articleDetail.channelId;
                    _relArticles = articleDetail.projects;
                    if (channelMessageList.ContainsKey(articleDetail.channelId))
                    {
                        _channelComments = channelMessageList[articleDetail.channelId];
                    }
                    _contentMap = articleDetail.contentMap;
                    _lastCommentId = articleDetail.comments.currOldestMessageId;
                    var child = new Container(
                        color: CColors.White,
                        child: new Stack(
                            children: new List<Widget> {
                                new Positioned(
                                    top:0,
                                    left:0,
                                    right:0,
                                    child:_navigationBar(context)
                                ),
                                new Container(
                                    padding:EdgeInsets.only(top:98,bottom:45),
                                    child:articleDetail.comments.hasMore?new Refresh(
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
                                    ):new Refresh(
                                        child: new ListView(
                                            physics: new AlwaysScrollableScrollPhysics(),
                                            children: new List<Widget> {
                                                _contentHead(context),
                                                _subTitle(context),
                                                _contentDetail(context),
                                                _actionCards(context),
                                                _relatedArticles(context),
                                                _comments(context),
                                                _buildEnd(context)
                                            }
                                        )
                                    )
                                ),
                                
                                                            
                                new Positioned(
                                    bottom: 0,
                                    left: 0,
                                    right: 0,
                                    child: new ArticleTabBar(
                                        commentCallback: () => { },
                                        favorCallback: () => { },
                                        bookmarkCallback: () => { },
                                        shareCallback: () => { }
                                    )
                                )
                            }
                        )
                    );
                    return new SafeArea(
                        top: false,
                        child: child
                    );
                }
            );
        }
        private Widget _navigationBar(BuildContext context) {
            return new CustomNavigationBar(
                new GestureDetector(
                    onTap: () => {
                        Navigator.pop(context);
                        StoreProvider.store.Dispatch(new ClearEventDetailAction());
                    },
                    child: new Icon(Icons.arrow_back, size: 28, color: CColors.icon3)
                ), new List<Widget> {
                    new Container(
                        padding: EdgeInsets.all(1),
                        width: 88,
                        height: 28,
                        decoration: new BoxDecoration(
                            borderRadius: BorderRadius.all(14),
                            border: Border.all(CColors.PrimaryBlue)
                        ),
                        alignment: Alignment.center,
                        child: new Text("说点想法",
                            style: new TextStyle(color: CColors.PrimaryBlue, fontSize: 14,
                                fontFamily: "PingFangSC-Medium"))
                    )
                }, CColors.White, 52);
        }
        
        private IPromise onFooterRefresh() {
            return ArticleApi.FetchArticleComments(_channelId, _lastCommentId)
                .Then((responseComments) =>
                {
                    var channelMessageList = new Dictionary<string,List<string>>();
                    var channelMessageDict = new Dictionary<string,Dictionary<string, Message>>();
                    var itemIds = new List<string>();
                    var messageItem = new Dictionary<string,Message>();
                    responseComments.items.ForEach((message) =>
                    {
                        itemIds.Add(message.id);
                        messageItem[message.id] = message;
                    });
                    channelMessageList.Add(_channelId,itemIds);
                    channelMessageDict.Add(_channelId,messageItem);
                            
                    StoreProvider.store.Dispatch(new FetchArticleCommentsSuccessAction()
                    {
                        channelMessageDict = channelMessageDict,
                        channelMessageList = channelMessageList
                    });
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
//                                                    "123",
                                                    _user==null?"头像":_user.avatar,
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
                                                _user==null?"昵称":_user.username,
                                                style: CTextStyle.PRegular
                                            ),
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
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new EventDescription(content: _article.body, contentMap: _contentMap)
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
            if (_relArticles.Count==0)
            {
                return widgets;
            }
            _relArticles.ForEach((article) =>
            {
                widgets.Add(new RelatedArticleCard(article)); 
            });
            return widgets;
        }

        private Widget _comments(BuildContext context)
        {
            if (_channelComments.Count==0)
            {
                return new Container();
            }
            return new Container(
                padding:EdgeInsets.only(left:16,right:16),
                child: new Column(
                    crossAxisAlignment:CrossAxisAlignment.start,
                    children:new List<Widget>{
                    new Text("评论",style:CTextStyle.H5,textAlign:TextAlign.left),
                    new Container(
                        child: new Column(
                            children: _buildComments()
                        )
                    )
                }) 

            );
        }

        private List<Widget> _buildComments()
        {
            var comments = new List<Widget>();
            var channelMessageDict = StoreProvider.store.state.messageState.channelMessageDict;
            var messageDict = channelMessageDict[_channelId];
            _channelComments.ForEach((commentId) =>
            {
                comments.Add(new CommentCard(messageDict[commentId])); 
            });
            
            return comments;
        }


        private Widget _buildEnd(BuildContext context)
        {
            if (_channelComments.Count==0)
            {
                return new Container();
            }
            return new Container(
                height:52,
                child:new Text("一 已经全部加载完毕 一",style:new TextStyle(height: 1.57f,
                    fontSize: 14,
                    fontFamily: "PingFang-Regular",
                    color: CColors.TextBody4
                ),textAlign:TextAlign.center));
        }

    }

    


}