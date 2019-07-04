using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Avatar = ConnectApp.Components.Avatar;
using Config = ConnectApp.Constants.Config;

namespace ConnectApp.screens {
    public class ArticleDetailScreenConnector : StatelessWidget {
        public ArticleDetailScreenConnector(
            string articleId,
            Key key = null
        ) : base(key: key) {
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
                        playVideo = url => dispatcher.dispatch(new PlayVideoAction {
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
                        likeArticle = id => {
                            AnalyticsManager.ClickLike("Article", this.articleId);
                            return dispatcher.dispatch<IPromise>(Actions.likeArticle(id));
                        },
                        likeComment = message => {
                            AnalyticsManager.ClickLike("Article_Comment", this.articleId, message.id);
                            return dispatcher.dispatch<IPromise>(Actions.likeComment(message));
                        },
                        removeLikeComment =
                            message => {
                                AnalyticsManager.ClickLike("Article_Remove_Comment", this.articleId, message.id);
                                return dispatcher.dispatch<IPromise>(Actions.removeLikeComment(message));
                            },
                        sendComment = (channelId, content, nonce, parentMessageId) => {
                            AnalyticsManager.ClickPublishComment(
                                parentMessageId == null ? "Article" : "Article_Comment", channelId, parentMessageId);
                            CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog());
                            return dispatcher.dispatch<IPromise>(
                                Actions.sendComment(this.articleId, channelId, content, nonce, parentMessageId));
                        },
                        shareToWechat = (type, title, description, linkUrl, imageUrl) => dispatcher.dispatch<IPromise>(
                            Actions.shareToWechat(type, title, description, linkUrl, imageUrl))
                    };
                    return new ArticleDetailScreen(viewModel, actionModel);
                }
            );
        }
    }

    class ArticleDetailScreen : StatefulWidget {
        public ArticleDetailScreen(
            ArticleDetailScreenViewModel viewModel = null,
            ArticleDetailScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ArticleDetailScreenViewModel viewModel;
        public readonly ArticleDetailScreenActionModel actionModel;

        public override State createState() {
            return new _ArticleDetailScreenState();
        }
    }

    enum _ArticleJumpToCommentState {
        Inactive,
        active
    }

    class _ArticleDetailScreenState : State<ArticleDetailScreen>, TickerProvider {
        const float navBarHeight = 44;
        Article _article = new Article();
        User _user = new User();
        Team _team = new Team();
        bool _isHaveTitle;
        float _titleHeight;
        Animation<RelativeRect> _animation;
        AnimationController _controller;
        RefreshController _refreshController;
        string _loginSubId;
        _ArticleJumpToCommentState _jumpState;

        float? _cachedCommentPosition;

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
            this._isHaveTitle = false;
            this._titleHeight = 0.0f;
            this._controller = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            RelativeRectTween rectTween = new RelativeRectTween(
                RelativeRect.fromLTRB(0, navBarHeight, 0, 0),
                RelativeRect.fromLTRB(0, 13, 0, 0)
            );
            this._animation = rectTween.animate(this._controller);
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchArticleDetail();
                this.widget.actionModel.fetchArticleDetail(this.widget.viewModel.articleId);
            });
            this._loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
                this.widget.actionModel.startFetchArticleDetail();
                this.widget.actionModel.fetchArticleDetail(this.widget.viewModel.articleId);
            });
            this._jumpState = _ArticleJumpToCommentState.Inactive;
            this._cachedCommentPosition = null;
        }

        public override void dispose() {
            EventBus.unSubscribe(EventBusConstant.login_success, this._loginSubId);
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick, () => $"created by {this}");
        }

        public override Widget build(BuildContext context) {
            this.widget.viewModel.articleDict.TryGetValue(this.widget.viewModel.articleId, out this._article);
            if (this.widget.viewModel.articleDetailLoading && (this._article == null || !this._article.isNotFirst)) {
                return new Container(
                    color: CColors.White,
                    child: new CustomSafeArea(
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(false),
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

            if (this._titleHeight == 0f && this._article.title.isNotEmpty()) {
                this._titleHeight = CTextUtils.CalculateTextHeight(
                                        text: this._article.title,
                                        textStyle: CTextStyle.H3,
                                        MediaQuery.of(context).size.width - 16 * 2, // 16 is horizontal padding
                                        null
                                    ) + 16; // 16 is top padding
                this.setState(() => { });
            }

            var commentIndex = 0;
            var originItems = this._article == null ? new List<Widget>() : this._buildItems(context, out commentIndex);
            commentIndex = this._jumpState == _ArticleJumpToCommentState.active ? commentIndex : 0;
            this._jumpState = _ArticleJumpToCommentState.Inactive;

            var child = new Container(
                color: CColors.Background,
                child: new Column(
                    children: new List<Widget> {
                        this._buildNavigationBar(),
                        new Expanded(
                            child: new CustomScrollbar(
                                new CenteredRefresher(
                                    controller: this._refreshController,
                                    enablePullDown: false,
                                    enablePullUp: this._article.hasMore,
                                    onRefresh: this._onRefresh,
                                    onNotification: this._onNotification,
                                    children: originItems,
                                    centerIndex: commentIndex
                                )
                            )
                        ),
                        new ArticleTabBar(this._article.like,
                            () => {
                                if (!this.widget.viewModel.isLoggedIn) {
                                    this.widget.actionModel.pushToLogin();
                                }
                                else {
                                    AnalyticsManager.ClickComment("Article", this._article.channelId,
                                        this._article.title);
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
                                    AnalyticsManager.ClickComment("Article", this._article.channelId,
                                        this._article.title);
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

        List<Widget> _buildItems(BuildContext context, out int commentIndex) {
            var originItems = new List<Widget> {
                this._buildContentHead()
            };
            originItems.AddRange(
                ContentDescription.map(context, this._article.body, this._article.contentMap, this.widget.actionModel.openUrl,
                    this.widget.actionModel.playVideo));
            // originItems.Add(this._buildActionCards(this._article.like));
            originItems.Add(this._buildRelatedArticles());
            commentIndex = originItems.Count;
            originItems.AddRange(this._buildComments(context: context));

            return originItems;
        }

        Widget _buildNavigationBar(bool isShowRightWidget = true) {
            Widget titleWidget = new Container();
            if (this._isHaveTitle) {
                titleWidget = new Text(
                    this._article.title,
                    style: CTextStyle.PXLargeMedium,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    textAlign: TextAlign.center
                );
            }

            Widget rightWidget = new Container();
            if (isShowRightWidget) {
                string rightWidgetTitle = this._article.commentCount > 0
                    ? $"{this._article.commentCount}个评论"
                    : "评论";
                rightWidget = new Container(
                    margin: EdgeInsets.only(8, right: 16),
                    child: new CustomButton(
                        padding: EdgeInsets.zero,
                        onPressed: () => {
                            //do not jump if we are already at the exact comment position
                            if (this._refreshController.scrollController.position.pixels ==
                                this._cachedCommentPosition) {
                                return;
                            }

                            //first frame: create a new scroll view in which the center of the viewport is the comment widget
                            this.setState(
                                () => { this._jumpState = _ArticleJumpToCommentState.active; });

                            SchedulerBinding.instance.addPostFrameCallback((TimeSpan value2) => {
                                //calculate the comment position = curPixel(0) - minScrollExtent
                                var commentPosition = -this._refreshController.scrollController.position
                                    .minScrollExtent;

                                //cache the current comment position  
                                this._cachedCommentPosition = commentPosition;

                                //second frame: create a new scroll view which starts from the default first widget
                                //and then jump to the calculated comment position
                                this.setState(() => {
                                    this._refreshController.scrollController.jumpTo(commentPosition);

                                    //assume that when we jump to the comment, the title should always be shown as the header
                                    //this assumption will fail when an article is shorter than 16 pixels in height (as referred to in _onNotification
                                    this._controller.forward();
                                    this._isHaveTitle = true;
                                });
                            });
                        },
                        child: new Container(
                            width: 88,
                            height: 28,
                            alignment: Alignment.center,
                            decoration: new BoxDecoration(
                                border: Border.all(CColors.PrimaryBlue),
                                borderRadius: BorderRadius.all(14)
                            ),
                            child: new Text(
                                data: rightWidgetTitle,
                                style: new TextStyle(
                                    fontSize: 14,
                                    fontFamily: "Roboto-Medium",
                                    color: CColors.PrimaryBlue
                                )
                            )
                        )
                    )
                );
            }
            return new CustomAppBar(
                () => this.widget.actionModel.mainRouterPop(),
                new Expanded(
                    child: new Stack(
                        fit: StackFit.expand,
                        children: new List<Widget> {
                            new PositionedTransition(
                                rect: this._animation,
                                child: titleWidget
                            )
                        }
                    )
                ),
                rightWidget: rightWidget,
                this._isHaveTitle ? CColors.Separator2 : CColors.Transparent
            );
        }

        void _onRefresh(bool up) {
            if (!up) {
                this.widget.actionModel.fetchArticleComments(this._article.channelId, this._article.currOldestMessageId)
                    .Then(() => { this._refreshController.sendBack(up, RefreshStatus.idle); })
                    .Catch(err => { this._refreshController.sendBack(up, RefreshStatus.failed); });
            }
        }

        bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels - notification.metrics.minScrollExtent;
            if (pixels > this._titleHeight) {
                if (this._isHaveTitle == false) {
                    this._controller.forward();
                    this.setState(() => { this._isHaveTitle = true; });
                }
            }
            else {
                if (this._isHaveTitle) {
                    this._controller.reverse();
                    this.setState(() => { this._isHaveTitle = false; });
                }
            }

            return true;
        }

        Widget _buildContentHead() {
            Widget _avatar = this._article.ownerType == OwnerType.user.ToString()
                ? Avatar.User(this._user.id, this._user, 32)
                : Avatar.Team(this._team.id, this._team, 32);

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
                padding: EdgeInsets.only(16, 16, 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text(
                            this._article.title,
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
                        ),
                        this._article.subTitle.isEmpty()
                            ? new Container()
                            : new Container(
                                margin: EdgeInsets.only(bottom: 24),
                                decoration: new BoxDecoration(
                                    CColors.Separator2,
                                    borderRadius: BorderRadius.all(4)
                                ),
                                padding: EdgeInsets.only(16, 12, 16, 12),
                                width: Screen.width - 32,
                                child: new Text($"{this._article.subTitle}", style: CTextStyle.PLargeBody4)
                            )
                    }
                )
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
            var relatedArticles = this._article.projects.FindAll(item => item.type == "article");
            if (relatedArticles.Count == 0) {
                return new Container();
            }

            var widgets = new List<Widget>();
            relatedArticles.ForEach(article => {
                //对文章进行过滤
                if (article.id != this._article.id) {
                    Widget card;
                    if (article.ownerType == OwnerType.user.ToString()) {
                        card = RelatedArticleCard.User(article, this._user,
                            () => {
                                AnalyticsManager.ClickEnterArticleDetail("ArticleDetail_Related", article.id,
                                    article.title);
                                this.widget.actionModel.pushToArticleDetail(article.id);
                            },
                            new ObjectKey(article.id));
                    }
                    else {
                        card = RelatedArticleCard.Team(article, this._team,
                            () => {
                                AnalyticsManager.ClickEnterArticleDetail("ArticleDetail_Related", article.id,
                                    article.title);
                                this.widget.actionModel.pushToArticleDetail(article.id);
                            },
                            new ObjectKey(article.id));
                    }

                    widgets.Add(card);
                }
            });
            if (widgets.isNotEmpty()) {
                widgets.InsertRange(0, new List<Widget> {
                    new Container(
                        height: 1,
                        color: CColors.Separator2,
                        margin: EdgeInsets.only(16, 16, 16, 40)
                    ),
                    new Container(
                        margin: EdgeInsets.only(16, bottom: 16),
                        child: new Text(
                            "推荐阅读",
                            style: CTextStyle.PLargeMedium
                        )
                    )
                });
            }

            return new Container(
                color: CColors.White,
                margin: EdgeInsets.only(bottom: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: widgets
                )
            );
        }

        IEnumerable<Widget> _buildComments(BuildContext context) {
            List<string> channelComments = new List<string>();
            if (this.widget.viewModel.channelMessageList.ContainsKey(this._article.channelId)) {
                channelComments = this.widget.viewModel.channelMessageList[this._article.channelId];
            }
            var mediaQuery = MediaQuery.of(context);
            var comments = new List<Widget> {
                new Container(
                    color: CColors.White,
                    width: mediaQuery.size.width,
                    padding: EdgeInsets.only(16, 16, 16),
                    child: new Text(
                        "评论",
                        style: CTextStyle.H5,
                        textAlign: TextAlign.left
                    )
                )
            };

            var titleHeight = CTextUtils.CalculateTextHeight(
                "评论",
                CTextStyle.H5,
                mediaQuery.size.width - 16 * 2, // 16 is horizontal padding
                null
            ) + 16; // 16 is top padding

            var height = mediaQuery.size.height - navBarHeight - 44 - mediaQuery.padding.vertical;
            if (channelComments.Count == 0) {
                var blankView = new Container(
                    height: height - titleHeight,
                    child: new BlankView(
                        "快来写下第一条评论吧",
                        "image/default-comment"
                    )
                );
                comments.Add(item: blankView);
                return comments;
            }

            var messageDict = this.widget.viewModel.channelMessageDict[this._article.channelId];
            float contentHeights = 0;
            foreach (var commentId in channelComments) {
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

                var content = MessageUtils.AnalyzeMessage(message.content, message.mentions,
                    message.mentionEveryone) + (parentName.isEmpty() ? "" : $"回复@{parentName}");
                var contentHeight = CTextUtils.CalculateTextHeight(
                    content,
                    CTextStyle.PLargeBody,
                    // 16 is horizontal padding, 24 is avatar size, 8 is content left margin to avatar
                    mediaQuery.size.width - 16 * 2 - 24 - 8,
                    null
                ) + 16 + 24 + 3 + 5 + 22 + 12;
                // 16 is top padding, 24 is avatar size, 3 is content top margin to avatar, 5 is content bottom margin to commentTime
                // 22 is commentTime height, 12 is commentTime bottom margin
                contentHeights += contentHeight;
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
                            AnalyticsManager.ClickComment("Article_Comment", this._article.channelId,
                                this._article.title, commentId);
                            ActionSheetUtils.showModalActionSheet(new CustomInput(
                                message.author.fullName.isEmpty() ? "" : message.author.fullName,
                                text => {
                                    ActionSheetUtils.hiddenModalPopup();
                                    this.widget.actionModel.sendComment(this._article.channelId,
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
                                this.widget.actionModel.removeLikeComment(message);
                            }
                            else {
                                this.widget.actionModel.likeComment(message);
                            }
                        }
                    });
                comments.Add(card);
            }

            float endHeight = 0;
            if (!this._article.hasMore) {
                comments.Add(new Container(
                    height: 52,
                    alignment: Alignment.center,
                    child: new Text(
                        "一 已经全部加载完毕 一",
                        style: CTextStyle.PRegularBody4,
                        textAlign: TextAlign.center
                    )
                ));
                endHeight = 52;
            }
            if (titleHeight + contentHeights + endHeight < height) {
                return new List<Widget> {
                    new Container(
                        height: height,
                        child: new Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: comments
                        )
                    )
                };
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