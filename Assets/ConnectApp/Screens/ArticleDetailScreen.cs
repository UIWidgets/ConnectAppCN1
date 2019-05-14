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
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
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

        readonly string articleId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ArticleDetailScreenViewModel>(
                converter: state => new ArticleDetailScreenViewModel {
                    articleId = this.articleId,
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
                        openUrl = url => dispatcher.dispatch(new MainNavigatorPushToWebViewAction {
                            url = url
                        }),
                        playVideo = url => dispatcher.dispatch(new PlayVideoAction() {
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

    class ArticleDetailScreen : StatefulWidget {
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

    class _ArticleDetailScreenState : State<ArticleDetailScreen> {
        Article _article = new Article();
        User _user = new User();
        Team _team = new Team();
        string _channelId = "";
        List<string> _channelComments = new List<string>();
        List<Article> _relArticles = new List<Article>();
        Dictionary<string, ContentMap> _contentMap = new Dictionary<string, ContentMap>();
        string _lastCommentId = "";
        bool _hasMore = false;
        RefreshController _refreshController;
        string _loginSubId;

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchArticleDetail();
                this.widget.actionModel.fetchArticleDetail(this.widget.viewModel.articleId);
            });
            this._loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
                this.widget.actionModel.startFetchArticleDetail();
                this.widget.actionModel.fetchArticleDetail(this.widget.viewModel.articleId);
            });
        }

        public override void dispose() {
            EventBus.unSubscribe(EventBusConstant.login_success, this._loginSubId);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            this.widget.viewModel.articleDict.TryGetValue(this.widget.viewModel.articleId, out this._article);
            if (this.widget.viewModel.articleDetailLoading && (this._article == null || !this._article.isNotFirst)) {
                return new Container(
                    color: CColors.White,
                    child: new CustomSafeArea(
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new ArticleDetailLoading()
                            }
                        )
                    )
                );
            }

            if (this._article == null || this._article.channelId == null) {
                return new Container();
            }

            if (this._article.ownerType == "user") {
                if (this._article.userId != null &&
                    this.widget.viewModel.userDict.TryGetValue(this._article.userId, out this._user)) {
                    this._user = this.widget.viewModel.userDict[this._article.userId];
                }
            }

            if (this._article.ownerType == "team") {
                if (this._article.teamId != null &&
                    this.widget.viewModel.teamDict.TryGetValue(this._article.teamId, out this._team)) {
                    this._team = this.widget.viewModel.teamDict[this._article.teamId];
                }
            }

            this._channelId = this._article.channelId;
            this._relArticles = this._article.projects.FindAll(item => item.type == "article");
            if (this.widget.viewModel.channelMessageList.ContainsKey(this._article.channelId)) {
                this._channelComments = this.widget.viewModel.channelMessageList[this._article.channelId];
            }

            this._contentMap = this._article.contentMap;
            this._lastCommentId = this._article.currOldestMessageId ?? "";
            this._hasMore = this._article.hasMore;

            var originItems = this._article == null ? new List<Widget>() : this._buildItems(context);

            var child = new Container(
                color: CColors.background3,
                child: new Column(
                    children: new List<Widget> {
                        this._buildNavigationBar(),
                        new Expanded(
                            child: new SmartRefresher(
                                controller: this._refreshController,
                                enablePullDown: false,
                                enablePullUp: this._hasMore,
                                onRefresh: this._onRefresh,
                                child: ListView.builder(
                                    physics: new AlwaysScrollableScrollPhysics(),
                                    itemCount: originItems.Count,
                                    itemBuilder: (cxt, index) => originItems[index]
                                )
                            )
                        ),
                        new ArticleTabBar(this._article.like,
                            () => {
                                if (!this.widget.viewModel.isLoggedIn) {
                                    this.widget.actionModel.pushToLogin();
                                }
                                else {
                                    ActionSheetUtils.showModalActionSheet(new CustomInput(
                                        doneCallBack: text => {
                                            ActionSheetUtils.hiddenModalPopup();
                                            this.widget.actionModel.sendComment(this._article.channelId,
                                                text,
                                                Snowflake.CreateNonce(),
                                                null
                                            );
                                        })
                                    );
                                }
                            },
                            () => {
                                if (!this.widget.viewModel.isLoggedIn) {
                                    this.widget.actionModel.pushToLogin();
                                }
                                else {
                                    ActionSheetUtils.showModalActionSheet(new CustomInput(
                                        doneCallBack: text => {
                                            ActionSheetUtils.hiddenModalPopup();
                                            this.widget.actionModel.sendComment(this._article.channelId,
                                                text,
                                                Snowflake.CreateNonce(),
                                                null
                                            );
                                        })
                                    );
                                }
                            },
                            () => {
                                if (!this.widget.viewModel.isLoggedIn) {
                                    this.widget.actionModel.pushToLogin();
                                }
                                else {
                                    if (!this._article.like) {
                                        this.widget.actionModel.likeArticle(this._article.id);
                                    }
                                }
                            },
                            shareCallback: this.share
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

        List<Widget> _buildItems(BuildContext context) {
            var originItems = new List<Widget>();
            originItems.Add(this._buildContentHead());
            originItems.Add(this._buildSubTitle());
            originItems.AddRange(
                ContentDescription.map(context, this._article.body, this._contentMap, this.widget.actionModel.openUrl,
                    this.widget.actionModel.playVideo));
            originItems.Add(this._buildActionCards(this._article.like));
            originItems.Add(this._buildRelatedArticles());
            originItems.Add(this._comments());
            originItems.AddRange(this._buildComments());
            if (!this._article.hasMore) {
                originItems.Add(this._buildEnd());
            }

            return originItems;
        }

        Widget _buildNavigationBar() {
            return new Container(
                height: 44,
                color: CColors.White,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new GestureDetector(
                            onTap: () => this.widget.actionModel.mainRouterPop(),
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

        void _onRefresh(bool up) {
            if (!up) {
                this.widget.actionModel.fetchArticleComments(this._channelId, this._lastCommentId)
                    .Then(() => { this._refreshController.sendBack(up, RefreshStatus.idle); })
                    .Catch(err => { this._refreshController.sendBack(up, RefreshStatus.failed); });
            }
        }

        Widget _buildContentHead() {
            Widget _avatar;
            if (this._article.ownerType == OwnerType.user.ToString()) {
                _avatar = Avatar.User(this._user.id, this._user, 32);
            }
            else {
                _avatar = Avatar.Team(this._team.id, this._team, 32);
            }

            var text = this._article.ownerType == "user" ? this._user.fullName : this._team.name;
            var description = this._article.ownerType == "user" ? this._user.title : "";
            var time = this._article.lastPublishedTime == null
                ? this._article.publishedTime
                : this._article.lastPublishedTime;
            Widget descriptionWidget = new Container();
            if (description.isNotEmpty()) {
                descriptionWidget = new Text(
                    description,
                    style: CTextStyle.PSmallBody3
                );
            }

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, right: 16, top: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text(this._article.title,
                            style: CTextStyle.H3
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 8),
                            child: new Text(
                                $"阅读 {this._article.viewCount} · {DateConvert.DateStringFromNow(time)}",
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

        Widget _buildSubTitle() {
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
                        child: new Text($"{this._article.subTitle}", style: CTextStyle.PLargeBody4)
                    ))
            );
        }


        Widget _buildActionCards(bool like) {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(bottom: 40),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new ActionCard(like ? Icons.favorite : Icons.favorite_border, like ? "已赞" : "点赞", like, () => {
                            if (!this.widget.viewModel.isLoggedIn) {
                                this.widget.actionModel.pushToLogin();
                            }
                            else {
                                if (!like) {
                                    this.widget.actionModel.likeArticle(this._article.id);
                                }
                            }
                        }),
                        new Container(width: 16),
                        new ActionCard(Icons.share, "分享", false, this.share)
                    }
                )
            );
        }

        Widget _buildRelatedArticles() {
            if (this._relArticles.Count == 0) {
                return new Container();
            }

            var widgets = new List<Widget>();
            this._relArticles.ForEach(article => {
                //对文章进行过滤
                if (article.id != this._article.id) {
                    Widget card;
                    if (article.ownerType == OwnerType.user.ToString()) {
                        card = RelatedArticleCard.User(article, this._user,
                            () => { this.widget.actionModel.pushToArticleDetail(article.id); },
                            new ObjectKey(article.id));
                    }
                    else {
                        card = RelatedArticleCard.Team(article, this._team,
                            () => { this.widget.actionModel.pushToArticleDetail(article.id); },
                            new ObjectKey(article.id));
                    }

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

        Widget _comments() {
            if (this._channelComments.Count == 0) {
                return new Container();
            }

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

        List<Widget> _buildComments() {
            if (this._channelComments.isEmpty()) {
                return new List<Widget>();
            }

            var comments = new List<Widget>();
            if (!this.widget.viewModel.channelMessageDict.ContainsKey(this._channelId)) {
                return comments;
            }

            var messageDict = this.widget.viewModel.channelMessageDict[this._channelId];
            foreach (var commentId in this._channelComments) {
                if (!messageDict.ContainsKey(commentId)) {
                    break;
                }

                var message = messageDict[commentId];
                bool isPraised = _isPraised(message, this.widget.viewModel.loginUserId);
                var parentName = "";
                if (message.parentMessageId.isNotEmpty()) {
                    if (messageDict.ContainsKey(message.parentMessageId)) {
                        var parentMessage = messageDict[message.parentMessageId];
                        parentName = parentMessage.author.fullName;
                    }
                }

                var card = new CommentCard(
                    message,
                    isPraised,
                    parentName,
                    () => ReportManager.showReportView(this.widget.viewModel.isLoggedIn,
                        commentId,
                        ReportType.comment, this.widget.actionModel.pushToLogin, this.widget.actionModel.pushToReport
                    ),
                    replyCallBack: () => {
                        if (!this.widget.viewModel.isLoggedIn) {
                            this.widget.actionModel.pushToLogin();
                        }
                        else {
                            ActionSheetUtils.showModalActionSheet(new CustomInput(
                                message.author.fullName.isEmpty() ? "" : message.author.fullName,
                                text => {
                                    ActionSheetUtils.hiddenModalPopup();
                                    this.widget.actionModel.sendComment(this._channelId,
                                        text,
                                        Snowflake.CreateNonce(),
                                        commentId
                                    );
                                })
                            );
                        }
                    },
                    praiseCallBack: () => {
                        if (!this.widget.viewModel.isLoggedIn) {
                            this.widget.actionModel.pushToLogin();
                        }
                        else {
                            if (isPraised) {
                                this.widget.actionModel.removeLikeComment(commentId);
                            }
                            else {
                                this.widget.actionModel.likeComment(commentId);
                            }
                        }
                    });
                comments.Add(card);
            }

            return comments;
        }

        static bool _isPraised(Message message, string loginUserId) {
            foreach (var reaction in message.reactions) {
                if (reaction.user.id == loginUserId) {
                    return true;
                }
            }

            return false;
        }

        Widget _buildEnd() {
            if (this._channelComments.Count == 0) {
                return new Container();
            }

            return new Container(
                height: 52,
                alignment: Alignment.center,
                child: new Text("一 已经全部加载完毕 一", style: CTextStyle.PRegularBody4, textAlign: TextAlign.center
                )
            );
        }

        void share() {
            ShareUtils.showShareView(new ShareView(
                projectType: ProjectType.article,
                onPressed: type => {
                    string linkUrl = $"{Config.apiAddress}/p/{this._article.id}";
                    if (type == ShareType.clipBoard) {
                        Clipboard.setData(new ClipboardData(linkUrl));
                        CustomDialogUtils.showToast("复制链接成功", Icons.check_circle_outline);
                    }
                    else if (type == ShareType.block) {
                        ReportManager.block(this.widget.viewModel.isLoggedIn, this._article.id,
                            this.widget.actionModel.pushToLogin, this.widget.actionModel.pushToBlock,
                            this.widget.actionModel.mainRouterPop
                        );
                    }
                    else if (type == ShareType.report) {
                        ReportManager.report(this.widget.viewModel.isLoggedIn, this._article.id,
                            ReportType.article, this.widget.actionModel.pushToLogin,
                            this.widget.actionModel.pushToReport
                        );
                    }
                    else {
                        CustomDialogUtils.showCustomDialog(
                            child: new CustomLoadingDialog()
                        );
                        string imageUrl = $"{this._article.thumbnail.url}.200x0x1.jpg";
                        this.widget.actionModel.shareToWechat(type, this._article.title, this._article.subTitle,
                                linkUrl,
                                imageUrl).Then(CustomDialogUtils.hiddenCustomDialog)
                            .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                    }
                }
            ));
        }
    }
}