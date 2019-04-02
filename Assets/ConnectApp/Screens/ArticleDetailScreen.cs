using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Avatar = ConnectApp.components.Avatar;

namespace ConnectApp.screens {
    public class ArticleDetailScreen : StatefulWidget {
        public ArticleDetailScreen(
            Key key = null,
            string articleId = null
        ) : base(key) {
            D.assert(articleId != null);
            this.articleId = articleId;
        }

        public readonly string articleId;

        public override State createState() {
            return new _ArticleDetailScreenState();
        }
    }

    internal class _ArticleDetailScreenState : State<ArticleDetailScreen> {
        private Article _article = new Article();
        private User _user = new User();
        private Team _team = new Team();
        private string _channelId = "";
        private List<string> _channelComments = new List<string>();
        private List<Article> _relArticles = new List<Article>();
        private Dictionary<string, ContentMap> _contentMap = new Dictionary<string, ContentMap>();
        private string _lastCommentId = "";
        private bool _hasMore = false;
        private RefreshController _refreshController;


        public override void initState() {
            base.initState();
            _refreshController = new RefreshController();
            StoreProvider.store.Dispatch(new FetchArticleDetailAction
                {articleId = widget.articleId});
        }

        private static void _toLogin() {
            StoreProvider.store.Dispatch(new MainNavigatorPushToAction {routeName = MainNavigatorRoutes.Login});
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, Dictionary<string, object>>(
                converter: (state, dispatcher) => new Dictionary<string, object> {
                    {"articleDetailLoading", state.articleState.articleDetailLoading},
                    {"articleDict", state.articleState.articleDict},
                    {"channelMessageDict", state.messageState.channelMessageDict},
                    {"channelMessageList", state.messageState.channelMessageList},
                    {"userDict", state.userState.userDict},
                    {"teamDict", state.teamState.teamDict}
                },
                builder: (context1, viewModel) => {
                    var articleDetailLoading = (bool) viewModel["articleDetailLoading"];
                    if (articleDetailLoading)
                        return new Container(
                            color: CColors.White,
                            child: new SafeArea(
                                child: new Column(
                                    children: new List<Widget> {
                                        _buildNavigationBar(),
                                        new ArticleDetailLoading()
                                    }
                                )
                            )
                        );
                    var articleDict = (Dictionary<string, Article>) viewModel["articleDict"];
                    articleDict.TryGetValue(widget.articleId, out _article);
                    if (_article == null || _article.channelId == null) return new Container();
                    var channelMessageList = (Dictionary<string, List<string>>) viewModel["channelMessageList"];

                    if (_article.ownerType == "user") {
                        var userDict = (Dictionary<string, User>) viewModel["userDict"];
                        if (_article.userId != null && userDict.TryGetValue(_article.userId, out _user))
                            _user = userDict[_article.userId];
                    }

                    if (_article.ownerType == "team") {
                        var teamDict = (Dictionary<string, Team>) viewModel["teamDict"];
                        if (_article.teamId != null && teamDict.TryGetValue(_article.teamId, out _team))
                            _team = teamDict[_article.teamId];
                    }

                    _channelId = _article.channelId;
                    _relArticles = _article.projects;
                    if (channelMessageList.ContainsKey(_article.channelId))
                        _channelComments = channelMessageList[_article.channelId];
                    _contentMap = _article.contentMap;
                    _lastCommentId = _article.currOldestMessageId ?? "";
                    _hasMore = _article.hasMore;

                    var originItems = _article == null ? new List<Widget>() : _buildItems(context);

                    var child = new Container(
                        color: CColors.background3,
                        child: new Column(
                            children: new List<Widget> {
                                _buildNavigationBar(),
                                new Expanded(
                                    child: new SmartRefresher(
                                        controller: _refreshController,
                                        enablePullDown: false,
                                        enablePullUp: _hasMore,
                                        footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                                        onRefresh: _onRefresh,
                                        child: ListView.builder(
                                            physics: new AlwaysScrollableScrollPhysics(),
                                            itemCount: originItems.Count,
                                            itemBuilder: (cxt, index) => originItems[index]
                                        )
                                    )
                                ),
                                new ArticleTabBar(
                                    _article.like,
                                    () => {
                                        if (!StoreProvider.store.state.loginState.isLoggedIn)
                                            _toLogin();
                                        else
                                            ActionSheetUtils.showModalActionSheet(new CustomInput(
                                                doneCallBack: text => {
                                                    ActionSheetUtils.hiddenModalPopup();
                                                    StoreProvider.store.Dispatch(new SendCommentAction {
                                                        channelId = _article.channelId,
                                                        content = text,
                                                        nonce = Snowflake.CreateNonce()
                                                    });
                                                })
                                            );
                                    },
                                    () => {
                                        if (!StoreProvider.store.state.loginState.isLoggedIn)
                                            _toLogin();
                                        else
                                            ActionSheetUtils.showModalActionSheet(new CustomInput(
                                                doneCallBack: text => {
                                                    ActionSheetUtils.hiddenModalPopup();
                                                    StoreProvider.store.Dispatch(new SendCommentAction {
                                                        channelId = _article.channelId,
                                                        content = text,
                                                        nonce = Snowflake.CreateNonce()
                                                    });
                                                })
                                            );
                                    },
                                    () => {
                                        if (!StoreProvider.store.state.loginState.isLoggedIn) {
                                            _toLogin();
                                        }
                                        else {
                                            if (!_article.like)
                                                StoreProvider.store.Dispatch(new LikeArticleAction {
                                                    articleId = _article.id
                                                });
                                        }
                                    },
                                    shareCallback: () => { ShareUtils.showShareView(new ShareView()); }
                                )
                            }
                        )
                    );
                    return new Container(
                        color: CColors.White,
                        child: new SafeArea(
                            child: child
                        )
                    );
                }
            );
        }

        private List<Widget> _buildItems(BuildContext context) {
            var originItems = new List<Widget>();
            originItems.Add(_buildContentHead(context));
            originItems.Add(_buildSubTitle());
            originItems.AddRange(ArticleDescription.map(context, _article.body, _contentMap));
            originItems.Add(_buildActionCards(context, _article.like));
            originItems.Add(_buildRelatedArticles());
            originItems.Add(_comments(context));
            originItems.AddRange(_buildComments(context));
            if (!_article.hasMore) originItems.Add(_buildEnd(context));
            return originItems;
        }

        private static Widget _buildNavigationBar() {
            return new Container(
                decoration: new BoxDecoration(
                    border: new Border(
                        bottom: new BorderSide(
                            CColors.Separator2
                        )
                    )
                ),
                child: new CustomNavigationBar(
                    new GestureDetector(
                        onTap: () => { StoreProvider.store.Dispatch(new MainNavigatorPopAction()); },
                        child: new Icon(Icons.arrow_back, size: 24, color: CColors.icon3)
                    ), new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.zero,
                            onPressed: () => { },
                            child: new Container(
                                width: 88,
                                height: 28,
                                alignment: Alignment.center,
                                decoration: new BoxDecoration(
                                    border: Border.all(CColors.PrimaryBlue),
                                    borderRadius: BorderRadius.all(14)
                                ),
                                child: new Text(
                                    "说点想法",
                                    style: new TextStyle(
                                        fontSize: 14,
                                        fontFamily: "Roboto-Medium",
                                        color: CColors.PrimaryBlue
                                    )
                                )
                            )
                        )
                    }, CColors.White, 52
                )
            );
        }

        private void _onRefresh(bool up) {
            if (!up)
                ArticleApi.FetchArticleComments(_channelId, _lastCommentId)
                    .Then(responseComments => {
                        _lastCommentId = responseComments.currOldestMessageId;
                        _hasMore = responseComments.hasMore;

                        StoreProvider.store.Dispatch(new FetchArticleCommentsSuccessAction {
                            channelId = _channelId,
                            commentsResponse = responseComments,
                            isRefreshList = false,
                            hasMore = _hasMore,
                            currOldestMessageId = _lastCommentId
                        });
                        _refreshController.sendBack(up, RefreshStatus.idle);
                    })
                    .Catch(error => {
                        _refreshController.sendBack(up, RefreshStatus.failed);
                        Debug.Log(error);
                    });
        }

        private Widget _buildContentHead(BuildContext context) {
            var avatar = new Avatar(
                _article.ownerType == "user" ? _user.id : _team.id,
                32,
                _article.ownerType == "user" ? OwnerType.user : OwnerType.team
            );
            var text = _article.ownerType == "user" ? _user.fullName : _team.name;
            var description = _article.ownerType == "user" ? _user.title : "";
            Widget descriptionWidget = new Container();
            if (description.isNotEmpty())
                descriptionWidget = new Text(
                    description,
                    style: CTextStyle.PSmallBody3
                );
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, right: 16, top: 16),
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
                                $"阅读 {_article.viewCount} · {DateConvert.DateStringFromNow(_article.createdTime)}",
                                style: CTextStyle.PSmallBody4
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 24, bottom: 24),
                            child: new Row(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 8),
                                        child: avatar
                                    ),
                                    new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Text(
                                                text,
                                                style: CTextStyle.PRegularBody
                                            ),
                                            descriptionWidget
                                        }
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }

        private Widget _buildSubTitle() {
            return new Container(
                color: CColors.White,
                child: new Container(
                    margin: EdgeInsets.only(bottom: 24, left: 16, right: 16),
                    child: new Container(
                        decoration: new BoxDecoration(
                            CColors.Separator2,
                            borderRadius: BorderRadius.all(4)
                        ),
                        padding: EdgeInsets.only(16, 12, 16, 12),
                        child: new Text($"{_article.subTitle}", style: CTextStyle.PLargeBody4)
                    ))
            );
        }


        private Widget _buildActionCards(BuildContext context, bool like) {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(bottom: 40),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new ActionCard(Icons.favorite, like ? "已赞" : "点赞", like, () => {
                            if (!StoreProvider.store.state.loginState.isLoggedIn) {
                                _toLogin();
                            }
                            else {
                                if (!like)
                                    StoreProvider.store.Dispatch(new LikeArticleAction {
                                        articleId = _article.id
                                    });
                            }
                        }),
                        new Container(width: 16),
                        new ActionCard(Icons.share, "分享", false, () => { ShareUtils.showShareView(new ShareView()); })
                    }
                )
            );
        }

        private Widget _buildRelatedArticles() {
            if (_relArticles.Count == 0) return new Container();
            var widgets = new List<Widget>();
            _relArticles.ForEach(article => {
                widgets.Add(new RelatedArticleCard(
                    article,
                    () => {
                        StoreProvider.store.Dispatch(
                            new MainNavigatorPushToArticleDetailAction {articleId = article.id});
                    }
                ));
            });
            return new Container(
                color: CColors.White,
                margin: EdgeInsets.only(bottom: 16),
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            height: 1,
                            color: CColors.Separator2,
                            margin: EdgeInsets.only(16, right: 16, bottom: 24)
                        ),
                        new Container(
                            child: new Column(
                                children: widgets
                            )
                        )
                    })
            );
        }

        private Widget _comments(BuildContext context) {
            if (_channelComments.Count == 0) return new Container();
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, 16, 16),
                child: new Text(
                    "评论",
                    style: CTextStyle.H5,
                    textAlign: TextAlign.left
                )
            );
        }

        private List<Widget> _buildComments(BuildContext context) {
            if (_channelComments.isEmpty()) return new List<Widget>();
            var comments = new List<Widget>();
            var channelMessageDict = StoreProvider.store.state.messageState.channelMessageDict;
            if (!channelMessageDict.ContainsKey(_channelId)) return comments;
            var messageDict = channelMessageDict[_channelId];
            foreach (var commentId in _channelComments) {
                if (!messageDict.ContainsKey(commentId)) break;
                var message = messageDict[commentId];
                bool isPraised = _isPraised(message);
                var card = new CommentCard(
                    message,
                    isPraised,
                    () => {
                        ActionSheetUtils.showModalActionSheet(new ActionSheet(
                            items: new List<ActionSheetItem> {
                                new ActionSheetItem("举报", ActionType.destructive, () => { }),
                                new ActionSheetItem("取消", ActionType.cancel)
                            }
                        ));
                    },
                    replyCallBack: () => {
                        if (!StoreProvider.store.state.loginState.isLoggedIn)
                            _toLogin();
                        else
                            ActionSheetUtils.showModalActionSheet(new CustomInput(
                                message.author.fullName.isEmpty() ? "" : message.author.fullName,
                                text => {
                                    ActionSheetUtils.hiddenModalPopup();
                                    StoreProvider.store.Dispatch(new SendCommentAction {
                                        channelId = _channelId,
                                        content = text,
                                        nonce = Snowflake.CreateNonce(),
                                        parentMessageId = commentId
                                    });
                                })
                            );
                    },
                    praiseCallBack: () => {
                        if (!StoreProvider.store.state.loginState.isLoggedIn) {
                            _toLogin();
                        }
                        else {
                            if (isPraised)
                                StoreProvider.store.Dispatch(new RemoveLikeCommentAction {messageId = commentId});
                            else
                                StoreProvider.store.Dispatch(new LikeCommentAction {messageId = commentId});
                        }
                    });
                comments.Add(card);
            }

            return comments;
        }

        private static bool _isPraised(Message message) {
            foreach (var reaction in message.reactions)
                if (reaction.user.id == StoreProvider.store.state.loginState.loginInfo.userId)
                    return true;
            return false;
        }

        private Widget _buildEnd(BuildContext context) {
            if (_channelComments.Count == 0) return new Container();

            return new Container(
                height: 52,
                alignment: Alignment.center,
                child: new Text("一 已经全部加载完毕 一", style: CTextStyle.PRegularBody4, textAlign: TextAlign.center
                )
            );
        }
    }
}