using System;
using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.Screen;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ArticleDetailScreenConnector : StatelessWidget {
        public ArticleDetailScreenConnector(string articleId) {
            this.articleId = articleId;
        }

        private readonly string articleId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ArticleDetailScreenModel>(
                pure: true,
                converter: (state) => new ArticleDetailScreenModel {
                    articleId = articleId,
                    loginUserId = state.loginState.loginInfo.userId,
                    isLoggedIn = state.loginState.isLoggedIn,
                    articleDetailLoading = state.articleState.articleDetailLoading,
                    articleDict = state.articleState.articleDict,
                    channelMessageList = state.messageState.channelMessageList,
                    channelMessageDict = state.messageState.channelMessageDict,
                    userDict = state.userState.userDict,
                    teamDict = state.teamState.teamDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    return new ArticleDetailScreen(
                        viewModel,
                        popAction: () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        fetchArticleDetailAction: (id) =>
                            dispatcher.dispatch(new FetchArticleDetailAction {articleId = id}),
                        sendCommentAction: (channelId, content, nonce, parentMessageId) => dispatcher.dispatch(
                            new SendCommentAction {
                                channelId = channelId,
                                content = content,
                                nonce = nonce,
                                parentMessageId = parentMessageId
                            }),
                        pushToLoginAction: () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        likeArticleAction: (id) => dispatcher.dispatch(new LikeArticleAction {
                            articleId = id
                        }),
                        pushToArticleDetailAction: (id) => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {
                                articleId = id
                            }),
                        likeCommentAction: (commentId) => dispatcher.dispatch(new LikeCommentAction {
                            messageId = commentId
                        }),
                        removeLikeCommentAction: (commentId) => dispatcher.dispatch(new RemoveLikeCommentAction {
                            messageId = commentId
                        }),
                        fetchArticleCommentsAction: (channelId, currOldestMessageId) =>
                            dispatcher.dispatch<IPromise<FetchCommentsResponse>>(
                                Actions.fetchArticleComments(channelId, currOldestMessageId)
                            )
                    );
                });
        }
    }

    internal class ArticleDetailScreen : StatefulWidget {
        public ArticleDetailScreen(
            ArticleDetailScreenModel screenModel = null,
            Action popAction = null,
            Action pushToLoginAction = null,
            Action<string> pushToArticleDetailAction = null,
            Action<string> fetchArticleDetailAction = null,
            Func<string, string, IPromise<FetchCommentsResponse>> fetchArticleCommentsAction = null,
            Action<string> likeArticleAction = null,
            Action<string> likeCommentAction = null,
            Action<string> removeLikeCommentAction = null,
            Action<string, string, string, string> sendCommentAction = null,
            Key key = null
        ) : base(key) {
            D.assert(screenModel != null);
            this.screenModel = screenModel;
            this.popAction = popAction;
            this.pushToLoginAction = pushToLoginAction;
            this.pushToArticleDetailAction = pushToArticleDetailAction;
            this.fetchArticleDetailAction = fetchArticleDetailAction;
            this.fetchArticleCommentsAction = fetchArticleCommentsAction;
            this.likeArticleAction = likeArticleAction;
            this.likeCommentAction = likeCommentAction;
            this.removeLikeCommentAction = removeLikeCommentAction;
            this.sendCommentAction = sendCommentAction;
        }

        public readonly string articleId;
        public readonly ArticleDetailScreenModel screenModel;
        public readonly Action popAction;
        public readonly Action pushToLoginAction;
        public readonly Action<string> pushToArticleDetailAction;
        public readonly Action<string> fetchArticleDetailAction;
        public readonly Func<string, string, IPromise<FetchCommentsResponse>> fetchArticleCommentsAction;
        public readonly Action<string> likeArticleAction;
        public readonly Action<string> likeCommentAction;
        public readonly Action<string> removeLikeCommentAction;
        public readonly Action<string, string, string, string> sendCommentAction;

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
            widget.fetchArticleDetailAction(widget.articleId);
        }

        public override Widget build(BuildContext context) {
            if (widget.screenModel.articleDetailLoading)
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
            widget.screenModel.articleDict.TryGetValue(widget.articleId, out _article);
            if (_article == null || _article.channelId == null) return new Container();
            if (_article.ownerType == "user")
                if (_article.userId != null && widget.screenModel.userDict.TryGetValue(_article.userId, out _user))
                    _user = widget.screenModel.userDict[_article.userId];

            if (_article.ownerType == "team")
                if (_article.teamId != null && widget.screenModel.teamDict.TryGetValue(_article.teamId, out _team))
                    _team = widget.screenModel.teamDict[_article.teamId];
            _channelId = _article.channelId;
            _relArticles = _article.projects;
            if (widget.screenModel.channelMessageList.ContainsKey(_article.channelId))
                _channelComments = widget.screenModel.channelMessageList[_article.channelId];
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
                                if (!widget.screenModel.isLoggedIn)
                                    widget.pushToLoginAction();
                                else
                                    ActionSheetUtils.showModalActionSheet(new CustomInput(
                                        doneCallBack: text => {
                                            ActionSheetUtils.hiddenModalPopup();
                                            widget.sendCommentAction(
                                                _article.channelId,
                                                text,
                                                Snowflake.CreateNonce(),
                                                null
                                            );
                                        })
                                    );
                            },
                            () => {
                                if (!widget.screenModel.isLoggedIn)
                                    widget.pushToLoginAction();
                                else
                                    ActionSheetUtils.showModalActionSheet(new CustomInput(
                                        doneCallBack: text => {
                                            ActionSheetUtils.hiddenModalPopup();
                                            widget.sendCommentAction(
                                                _article.channelId,
                                                text,
                                                Snowflake.CreateNonce(),
                                                null
                                            );
                                        })
                                    );
                            },
                            () => {
                                if (!widget.screenModel.isLoggedIn) {
                                    widget.pushToLoginAction();
                                }
                                else {
                                    if (!_article.like)
                                        widget.likeArticleAction(_article.id);
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

        private List<Widget> _buildItems(BuildContext context) {
            var originItems = new List<Widget>();
            originItems.Add(_buildContentHead());
            originItems.Add(_buildSubTitle());
            originItems.AddRange(ArticleDescription.map(context, _article.body, _contentMap));
            originItems.Add(_buildActionCards(_article.like));
            originItems.Add(_buildRelatedArticles());
            originItems.Add(_comments());
            originItems.AddRange(_buildComments());
            if (!_article.hasMore) originItems.Add(_buildEnd());
            return originItems;
        }

        private Widget _buildNavigationBar() {
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
                        onTap: () => widget.popAction(),
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
                widget.fetchArticleCommentsAction(_channelId, _lastCommentId)
                    .Then(_ => { _refreshController.sendBack(up, RefreshStatus.idle); })
                    .Catch(err => { _refreshController.sendBack(up, RefreshStatus.failed); });
        }

        private Widget _buildContentHead() {
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


        private Widget _buildActionCards(bool like) {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(bottom: 40),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new ActionCard(Icons.favorite, like ? "已赞" : "点赞", like, () => {
                            if (!widget.screenModel.isLoggedIn) {
                                widget.pushToLoginAction();
                            }
                            else {
                                if (!like)
                                    widget.likeArticleAction(_article.id);
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
                    () => widget.pushToArticleDetailAction(article.id)
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

        private Widget _comments() {
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

        private List<Widget> _buildComments() {
            if (_channelComments.isEmpty()) return new List<Widget>();
            var comments = new List<Widget>();
            if (!widget.screenModel.channelMessageDict.ContainsKey(_channelId)) return comments;
            var messageDict = widget.screenModel.channelMessageDict[_channelId];
            foreach (var commentId in _channelComments) {
                if (!messageDict.ContainsKey(commentId)) break;
                var message = messageDict[commentId];
                bool isPraised = _isPraised(message, widget.screenModel.loginUserId);
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
                        if (!widget.screenModel.isLoggedIn)
                            widget.pushToLoginAction();
                        else
                            ActionSheetUtils.showModalActionSheet(new CustomInput(
                                message.author.fullName.isEmpty() ? "" : message.author.fullName,
                                text => {
                                    ActionSheetUtils.hiddenModalPopup();
                                    widget.sendCommentAction(
                                        _channelId,
                                        text,
                                        Snowflake.CreateNonce(),
                                        commentId
                                    );
                                })
                            );
                    },
                    praiseCallBack: () => {
                        if (!widget.screenModel.isLoggedIn) {
                            widget.pushToLoginAction();
                        }
                        else {
                            if (isPraised)
                                widget.removeLikeCommentAction(commentId);
                            else
                                widget.likeCommentAction(commentId);
                        }
                    });
                comments.Add(card);
            }

            return comments;
        }

        private static bool _isPraised(Message message, string loginUserId) {
            foreach (var reaction in message.reactions)
                if (reaction.user.id == loginUserId)
                    return true;
            return false;
        }

        private Widget _buildEnd() {
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