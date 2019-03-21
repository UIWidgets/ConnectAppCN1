using System.Collections.Generic;
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
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Avatar = ConnectApp.components.Avatar;
using Icons = ConnectApp.constants.Icons;
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

        private Article _article = new Article();
        private User _user = new User();
        private string _channelId = "";
        private List<string> _channelComments = new List<string>();
        private List<Article> _relArticles = new List<Article>();
        private Dictionary<string, ContentMap> _contentMap = new Dictionary<string, ContentMap>();
        private string _lastCommentId = "";
        private bool _hasMore = false;

        
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
                    {"userDict",state.userState.userDict}
                },
                builder: (context1, viewModel) => {
                    if (StoreProvider.store.state.articleState.articleDetailLoading)
                    {
                        return new SafeArea(
                            top:false,
                            child:new Column(
                                children: new List<Widget>
                                {
                                    new Container(
                                        child: _navigationBar(context)
                                    ),
                                    new ArticleDetailLoading()
                                }
                            )
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
                    _hasMore = articleDetail.comments.hasMore;
                    
                    var originItems = _buildItems(context, articleDetail);

                    RefresherCallback footerCallback = null;
                    if (_hasMore)
                    {
                        footerCallback = onFooterRefresh;
                    }
                    
                    var child = new Container(
                        color: CColors.background3,
                        child: new Stack(
                            children: new List<Widget> {
                                new Positioned(
                                    top:0,
                                    left:0,
                                    right:0,
                                    child:_navigationBar(context)
                                ),
                                new Container(
                                    padding:EdgeInsets.only(top:88,bottom:45),
                                    child:new Refresh(
                                        onFooterRefresh:footerCallback,
                                        child:ListView.builder(
                                            physics: new AlwaysScrollableScrollPhysics(),
                                            itemCount:originItems.Count,
                                            itemBuilder: (cxt,index) =>
                                            {
                                                return originItems[index];
                                            }
                                            
                                        )  
                                    )
                                ),
                                
                                                            
                                new Positioned(
                                    bottom: 0,
                                    left: 0,
                                    right: 0,
                                    child: new ArticleTabBar(
                                        articleDetail.like,
                                        addommentCallback: () =>
                                        {
                                            ActionSheetUtils.showModalActionSheet(context, new CustomInput(
                                                doneCallBack: (text) => { 
                                                   StoreProvider.store.Dispatch(new SendCommentAction()
                                                {
                                                    channelId = articleDetail.channelId,
                                                    content = text,
                                                    nonce = Snowflake.CreateNonce()
                                                }); }));

                                        },
                                        commentCallback: () =>
                                        {
                                            ActionSheetUtils.showModalActionSheet(context, new CustomInput());
                                        },
                                        favorCallback: () =>
                                        {
                                            if (!articleDetail.like)
                                            {
                                                StoreProvider.store.Dispatch(new LikeArticleAction()
                                                {
                                                    articleId = _article.id
                                                });
                                            }
                                        },
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

        private List<Widget> _buildItems(BuildContext context,Project articleDetail)
        {
            var originItems = new List<Widget>();
            originItems.Add(_contentHead(context));
            originItems.Add(_subTitle(context));
            originItems.AddRange(ArticleDescription.map(context,_article.body,_contentMap));
            originItems.Add(_actionCards(context,articleDetail.like));
            originItems.Add(_relatedArticles(context));
            originItems.Add( _comments(context));
            originItems.AddRange(_buildComments(context));
            if (!articleDetail.comments.hasMore)
            {
               originItems.Add(_buildEnd(context)); 
            }
            return originItems;
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
                    StoreProvider.store.state.articleState.articleDetail.comments = responseComments;
                    _lastCommentId = responseComments.currOldestMessageId;
                    _hasMore = responseComments.hasMore;
                    var channelMessageList = new Dictionary<string,List<string>>();
                    var channelMessageDict = new Dictionary<string,Dictionary<string, Message>>();
                    var itemIds = new List<string>();
                    var messageItem = new Dictionary<string,Message>();
                    responseComments.items.ForEach((message) =>
                    {
                        itemIds.Add(message.id);
                        messageItem[message.id] = message;
                    });
                    responseComments.parents.ForEach((message) =>
                    {
                        messageItem[message.id] = message;
                    });
                    channelMessageList.Add(_channelId,itemIds);
                    channelMessageDict.Add(_channelId,messageItem);
                            
                    StoreProvider.store.Dispatch(new FetchArticleCommentsSuccessAction()
                    {
                        channelMessageDict = channelMessageDict,
                        channelMessageList = channelMessageList,
                        isRefreshList = false
                    });
                })
                .Catch(error => { Debug.Log(error); });
        }
        
        
        private Widget _contentHead(BuildContext context)
        {
            
            return new Container(
                color:CColors.White,
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
                                        child: new Avatar(_user==null?"1234":_user.id,null,32)
                                    ),
                                    new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Container(height: 5),
                                            new Text(
                                                _user==null?"昵称":_user.fullName,
                                                style: CTextStyle.PRegular
                                            ),
                                            new Text(
                                                DateConvert.DateStringFromNow(_article.createdTime),
                                                style: new TextStyle(
                                                    height: 1.67f,
                                                    fontSize: 12,
                                                    fontFamily: "PingFang-Regular",
                                                    color: CColors.TextBody3
                                                )
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
                color:CColors.White,
                child:new Container(
                    margin:EdgeInsets.only(bottom:24,left:16,right:16),
                    child:new Container(
                    decoration:new BoxDecoration(
                        color:CColors.Separator2,
                        borderRadius:BorderRadius.all(4)
                    ),
                    padding:EdgeInsets.only(16,12,16,12), 
                    child:new Text($"{_article.subTitle}",style:CTextStyle.PLargeGray)
                )  ) 
            );
        }


        private Widget _contentDetail(BuildContext context)
        {
            return new Container(
                color: CColors.White,
                child: new EventDescription(content: _article.body, contentMap: _contentMap)
            );
        }
        
        private Widget _actionCards(BuildContext context,bool like)
        {
            return new Container(
                color:CColors.White,
                padding:EdgeInsets.only(bottom:40),
                child: new Row(
                    mainAxisAlignment:MainAxisAlignment.center,
                    crossAxisAlignment:CrossAxisAlignment.center,
                    children:new List<Widget>{
                        new ActionCard(Icons.favorite,like?"已赞":"点赞",like,onTap: () =>
                        {
                            if (!like)
                            {
                                StoreProvider.store.Dispatch(new LikeArticleAction()
                                {
                                    articleId = _article.id
                                });
                            }

                        }),
                        new Container(width:16),
                        new ActionCard(Icons.share,"分享",false,onTap: () =>
                        {
                            
                        }),
                    }
                ) 

            );
        }

        private Widget _relatedArticles(BuildContext context)
        {
            return new Container(
                color:CColors.White,
                padding:EdgeInsets.only(left:16,right:16),
                margin:EdgeInsets.only(bottom:16),
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
                widgets.Add(new RelatedArticleCard(article,onTap: () =>
                {
                    
                })); 
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
                color:CColors.White,
                padding:EdgeInsets.only(left:16,right:16),
                child: new Text("评论",style:CTextStyle.H5,textAlign:TextAlign.left)

            );
        }

        private List<Widget> _buildComments(BuildContext context)
        {
            if (_channelComments.isEmpty())
            {
                return new List<Widget>();
            }
            var comments = new List<Widget>();
            var channelMessageDict = StoreProvider.store.state.messageState.channelMessageDict;
            var messageDict = channelMessageDict[_channelId];
            _channelComments.ForEach((commentId) =>
            {
                var message = messageDict[commentId];
                bool isPraised = _isPraised(message);
                var card = new CommentCard(
                    message,
                    isPraised,
                    moreCallBack: () =>
                    {
                        ActionSheetUtils.showModalActionSheet(context, new ActionSheet(
                            items: new List<ActionSheetItem> {
                                new ActionSheetItem("举报", ActionType.destructive, () => { }),
                                new ActionSheetItem("取消", ActionType.cancel)
                            }
                        ));
                    }, 
                    replyCallBack: () =>
                    {
                        ActionSheetUtils.showModalActionSheet(context, new CustomInput(
                            message.author.fullName.isEmpty()?"":message.author.fullName,
                            doneCallBack: (text) => { 
                                StoreProvider.store.Dispatch(new SendCommentAction()
                                {
                                    channelId = _channelId,
                                    content = text,
                                    nonce = Snowflake.CreateNonce(),
                                    parentMessageId = commentId
                                }); }));
                    },
                    praiseCallBack: () => {
                        if (isPraised)
                        {
                            StoreProvider.store.Dispatch(new RemoveLikeCommentAction(){messageId = commentId});
                        }
                        else
                        {
                            StoreProvider.store.Dispatch(new LikeCommentAction(){messageId = commentId});
                        }


                    });
                comments.Add(card); 
            });
            return comments;
        }

        private bool _isPraised(Message message)
        {
            foreach (var reaction in message.reactions)
            {
                if (reaction.user.id == StoreProvider.store.state.loginState.loginInfo.userId)
                {
                    return true;
                }
            }
            return false;
        }

        private Widget _buildEnd(BuildContext context)
        {
            if (_channelComments.Count==0)
            {
                return new Container();
            }
            return new Container(
                height:52,
                alignment:Alignment.center,
                child:new Text("一 已经全部加载完毕 一",style:new TextStyle(height: 1.57f,
                    fontSize: 14,
                    fontFamily: "PingFang-Regular",
                    color: CColors.TextBody4
                ),textAlign:TextAlign.center));
        }

    }

    


}