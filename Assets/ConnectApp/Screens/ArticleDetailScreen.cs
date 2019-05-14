using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Config = ConnectApp.constants.Config;

namespace ConnectApp.screens {
    public class ArticleDetailScreenConnector : StatelessWidget {
        public ArticleDetailScreenConnector(string articleId) {
            this.articleId = articleId;
        }

        private readonly string articleId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ArticleDetailScreenViewModel>(
                converter: state => new ArticleDetailScreenViewModel {
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
                    var actionModel = new ArticleDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        openUrl = url => dispatcher.dispatch(new MainNavigatorPushToWebViewAction
                        {
                            url = url
                        }),
                        playVideo = url => dispatcher.dispatch(new PlayVideoAction()
                        {
                            url = url
                        }),
                        pushToArticleDetail = id => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {
                                articleId = id
                            }),
                        pushToReport = (reportId, reportType) => dispatcher.dispatch(
                            new MainNavigatorPushToReportAction {
                                reportId = reportId,
                                reportType = reportType
                            }
                        ),
                        pushToBlock = articleId => {
                            dispatcher.dispatch(new BlockArticleAction {articleId = articleId});
                            dispatcher.dispatch(new DeleteArticleHistoryAction {articleId = articleId});
                        },
                        startFetchArticleDetail = () => dispatcher.dispatch(new StartFetchArticleDetailAction()),
                        fetchArticleDetail = id =>
                            dispatcher.dispatch<IPromise>(Actions.FetchArticleDetail(id)),
                        fetchArticleComments = (channelId, currOldestMessageId) =>
                            dispatcher.dispatch<IPromise>(
                                Actions.fetchArticleComments(channelId, currOldestMessageId)
                            ),
                        likeArticle = id => dispatcher.dispatch<IPromise>(Actions.likeArticle(id)),
                        likeComment = id => dispatcher.dispatch<IPromise>(Actions.likeComment(id)),
                        removeLikeComment = id => dispatcher.dispatch<IPromise>(Actions.removeLikeComment(id)),
                        sendComment = (channelId, content, nonce, parentMessageId) => {
                            CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog());
                            return dispatcher.dispatch<IPromise>(
                                Actions.sendComment(channelId, content, nonce, parentMessageId));
                        },
                        shareToWechat = (type, title, description, linkUrl, imageUrl) => dispatcher.dispatch<IPromise>(
                            Actions.shareToWechat(type, title, description, linkUrl, imageUrl))
                    };

                    return new ArticleDetailScreen(viewModel, actionModel);
                });
        }
    }

    internal class ArticleDetailScreen : StatefulWidget {
        public ArticleDetailScreen(
            ArticleDetailScreenViewModel viewModel = null,
            ArticleDetailScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ArticleDetailScreenViewModel viewModel;
        public readonly ArticleDetailScreenActionModel actionModel;

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
        private string _loginSubId;

        public override void initState() {
            base.initState();
            _refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                widget.actionModel.startFetchArticleDetail();
                widget.actionModel.fetchArticleDetail(widget.viewModel.articleId);
            });
            _loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
                widget.actionModel.startFetchArticleDetail();
                widget.actionModel.fetchArticleDetail(widget.viewModel.articleId);
            });
        }

        public override void dispose() {
            EventBus.unSubscribe(EventBusConstant.login_success, _loginSubId);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            widget.viewModel.articleDict.TryGetValue(widget.viewModel.articleId, out _article);
            if (widget.viewModel.articleDetailLoading&&(_article==null||!_article.isNotFirst))
                return new Container(
                    color: CColors.White,
                    child: new CustomSafeArea(
                        child: new Column(
                            children: new List<Widget> {
                                _buildNavigationBar(),
                                new ArticleDetailLoading()
                            }
                        )
                    )
                );
            if (_article == null || _article.channelId == null) return new Container();
            if (_article.ownerType == "user")
                if (_article.userId != null && widget.viewModel.userDict.TryGetValue(_article.userId, out _user))
                    _user = widget.viewModel.userDict[_article.userId];

            if (_article.ownerType == "team")
                if (_article.teamId != null && widget.viewModel.teamDict.TryGetValue(_article.teamId, out _team))
                    _team = widget.viewModel.teamDict[_article.teamId];
            _channelId = _article.channelId;
            _relArticles = _article.projects.FindAll(item => item.type == "article");
            if (widget.viewModel.channelMessageList.ContainsKey(_article.channelId))
                _channelComments = widget.viewModel.channelMessageList[_article.channelId];
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
                                if (!widget.viewModel.isLoggedIn)
                                    widget.actionModel.pushToLogin();
                                else
                                    ActionSheetUtils.showModalActionSheet(new CustomInput(
                                        doneCallBack: text => {
                                            ActionSheetUtils.hiddenModalPopup();
                                            widget.actionModel.sendComment(
                                                _article.channelId,
                                                text,
                                                Snowflake.CreateNonce(),
                                                null
                                            );
                                        })
                                    );
                            },
                            () => {
                                if (!widget.viewModel.isLoggedIn)
                                    widget.actionModel.pushToLogin();
                                else
                                    ActionSheetUtils.showModalActionSheet(new CustomInput(
                                        doneCallBack: text => {
                                            ActionSheetUtils.hiddenModalPopup();
                                            widget.actionModel.sendComment(
                                                _article.channelId,
                                                text,
                                                Snowflake.CreateNonce(),
                                                null
                                            );
                                        })
                                    );
                            },
                            () => {
                                if (!widget.viewModel.isLoggedIn) {
                                    widget.actionModel.pushToLogin();
                                }
                                else {
                                    if (!_article.like)
                                        widget.actionModel.likeArticle(_article.id);
                                }
                            },
                            shareCallback: share
                        )
                    }
                )
            );
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: child
                )
            );
        }

        private List<Widget> _buildItems(BuildContext context) {
            var originItems = new List<Widget>();
            originItems.Add(_buildContentHead());
            originItems.Add(_buildSubTitle());
            originItems.AddRange(
                ContentDescription.map(context, _article.body, _contentMap, widget.actionModel.openUrl,widget.actionModel.playVideo));
            originItems.Add(_buildActionCards(_article.like));
            originItems.Add(_buildRelatedArticles());
            originItems.Add(_comments());
            originItems.AddRange(_buildComments());
            if (!_article.hasMore) originItems.Add(_buildEnd());
            return originItems;
        }

        private Widget _buildNavigationBar() {
            return new Container(
                height: 44,
                color: CColors.White,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new GestureDetector(
                            onTap: () => widget.actionModel.mainRouterPop(),
                            child: new Container(
                                padding: EdgeInsets.symmetric(10, 16),
                                color: CColors.Transparent,
                                child: new Icon(Icons.arrow_back, size: 24, color: CColors.icon3))
                        )
//                        new CustomButton(
//                            padding: EdgeInsets.zero,
//                            onPressed: () => { },
//                            child: new Container(
//                                width: 88,
//                                height: 28,
//                                alignment: Alignment.center,
//                                decoration: new BoxDecoration(
//                                    border: Border.all(CColors.PrimaryBlue),
//                                    borderRadius: BorderRadius.all(14)
//                                ),
//                                child: new Text(
//                                    "说点想法",
//                                    style: new TextStyle(
//                                        fontSize: 14,
//                                        fontFamily: "Roboto-Medium",
//                                        color: CColors.PrimaryBlue
//                                    )
//                                )
//                            )
//                        )
                    }
                )
            );
        }

        private void _onRefresh(bool up) {
            if (!up)
                widget.actionModel.fetchArticleComments(_channelId, _lastCommentId)
                    .Then(() => { _refreshController.sendBack(up, RefreshStatus.idle); })
                    .Catch(err => { _refreshController.sendBack(up, RefreshStatus.failed); });
        }

        private Widget _buildContentHead() {
            Widget _avatar;
            if (_article.ownerType == OwnerType.user.ToString())
                _avatar = Avatar.User(_user.id, _user, 32);
            else
                _avatar = Avatar.Team(_team.id, _team, 32);
            var text = _article.ownerType == "user" ? _user.fullName : _team.name;
            var description = _article.ownerType == "user" ? _user.title : "";
            var time = _article.lastPublishedTime == null ? _article.publishedTime : _article.lastPublishedTime;
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
                                $"阅读 {_article.viewCount} · {DateConvert.DateStringFromNow(time)}",
                                style: CTextStyle.PSmallBody4
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 24, bottom: 24),
                            child: new Row(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 8),
                                        child: _avatar
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
                        new ActionCard(like ? Icons.favorite : Icons.favorite_border, like ? "已赞" : "点赞", like, () => {
                            if (!widget.viewModel.isLoggedIn) {
                                widget.actionModel.pushToLogin();
                            }
                            else {
                                if (!like)
                                    widget.actionModel.likeArticle(_article.id);
                            }
                        }),
                        new Container(width: 16),
                        new ActionCard(Icons.share, "分享", false, share)
                    }
                )
            );
        }

        private Widget _buildRelatedArticles() {
            if (_relArticles.Count == 0) return new Container();
            var widgets = new List<Widget>();
            _relArticles.ForEach(article => {
                //对文章进行过滤
                if (article.id != _article.id) {
                    Widget card;
                    if (article.ownerType == OwnerType.user.ToString())
                        card = RelatedArticleCard.User(article, _user,
                            () => { widget.actionModel.pushToArticleDetail(article.id); }, new ObjectKey(article.id));
                    else
                        card = RelatedArticleCard.Team(article, _team,
                            () => { widget.actionModel.pushToArticleDetail(article.id); }, new ObjectKey(article.id));

                    widgets.Add(card);
                }
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
            if (!widget.viewModel.channelMessageDict.ContainsKey(_channelId)) return comments;
            var messageDict = widget.viewModel.channelMessageDict[_channelId];
            foreach (var commentId in _channelComments) {
                if (!messageDict.ContainsKey(commentId)) break;
                var message = messageDict[commentId];
                bool isPraised = _isPraised(message, widget.viewModel.loginUserId);
                var parentName = "";
                if (message.parentMessageId.isNotEmpty())
                    if (messageDict.ContainsKey(message.parentMessageId)) {
                        var parentMessage = messageDict[message.parentMessageId];
                        parentName = parentMessage.author.fullName;
                    }

                var card = new CommentCard(
                    message,
                    isPraised,
                    parentName,
                    () => ReportManager.showReportView(
                        widget.viewModel.isLoggedIn,
                        commentId,
                        ReportType.comment,
                        widget.actionModel.pushToLogin,
                        widget.actionModel.pushToReport
                    ),
                    replyCallBack: () => {
                        if (!widget.viewModel.isLoggedIn)
                            widget.actionModel.pushToLogin();
                        else
                            ActionSheetUtils.showModalActionSheet(new CustomInput(
                                message.author.fullName.isEmpty() ? "" : message.author.fullName,
                                text => {
                                    ActionSheetUtils.hiddenModalPopup();
                                    widget.actionModel.sendComment(
                                        _channelId,
                                        text,
                                        Snowflake.CreateNonce(),
                                        commentId
                                    );
                                })
                            );
                    },
                    praiseCallBack: () => {
                        if (!widget.viewModel.isLoggedIn) {
                            widget.actionModel.pushToLogin();
                        }
                        else {
                            if (isPraised)
                                widget.actionModel.removeLikeComment(commentId);
                            else
                                widget.actionModel.likeComment(commentId);
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

        private void share() {
            ShareUtils.showShareView(new ShareView(
                projectType: ProjectType.article,
                onPressed: type => {
                    string linkUrl = $"{Config.apiAddress}/p/{_article.id}";
                    if (type == ShareType.clipBoard) {
                        Clipboard.setData(new ClipboardData(linkUrl));
                        CustomDialogUtils.showToast("复制链接成功", Icons.check_circle_outline);
                    }
                    else if (type == ShareType.block) {
                        ReportManager.block(
                            widget.viewModel.isLoggedIn,
                            _article.id,
                            widget.actionModel.pushToLogin,
                            widget.actionModel.pushToBlock,
                            widget.actionModel.mainRouterPop
                        );
                    }
                    else if (type == ShareType.report) {
                        ReportManager.report(
                            widget.viewModel.isLoggedIn,
                            _article.id,
                            ReportType.article,
                            widget.actionModel.pushToLogin,
                            widget.actionModel.pushToReport
                        );
                    }
                    else {
                        CustomDialogUtils.showCustomDialog(
                            child: new CustomLoadingDialog()
                        );
                        string imageUrl = $"{_article.thumbnail.url}.200x0x1.jpg";
                        widget.actionModel.shareToWechat(type, _article.title, _article.subTitle, linkUrl,
                                imageUrl).Then(CustomDialogUtils.hiddenCustomDialog)
                            .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                    }
                }
            ));
        }
    }
}