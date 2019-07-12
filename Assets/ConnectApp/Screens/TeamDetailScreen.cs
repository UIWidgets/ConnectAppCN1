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
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Avatar = ConnectApp.Components.Avatar;
using Config = ConnectApp.Constants.Config;
using Transform = Unity.UIWidgets.widgets.Transform;

namespace ConnectApp.screens {
    public class TeamDetailScreenConnector : StatelessWidget {
        public TeamDetailScreenConnector(
            string teamId,
            Key key = null
        ) : base(key: key) {
            this.teamId = teamId;
        }

        readonly string teamId;
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, TeamDetailScreenViewModel>(
                converter: state => {
                    var team = state.teamState.teamDict.ContainsKey(key: this.teamId)
                        ? state.teamState.teamDict[key: this.teamId] : null;
                    var teamArticleOffset = state.teamState.teamArticleDict.ContainsKey(key: this.teamId)
                        ? state.teamState.teamArticleDict[key: this.teamId].Count
                        : 0;
                    return new TeamDetailScreenViewModel {
                        teamId = this.teamId,
                        teamLoading = state.teamState.teamLoading,
                        teamArticleLoading = state.teamState.teamArticleLoading,
                        team = team,
                        teamArticleDict = state.teamState.teamArticleDict,
                        teamArticleHasMore = state.teamState.teamArticleHasMore,
                        teamArticleOffset = teamArticleOffset,
                        isLoggedIn = state.loginState.isLoggedIn
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new TeamDetailScreenActionModel {
                        startFetchTeam = () => dispatcher.dispatch(new StartFetchTeamAction()),
                        fetchTeam = () => dispatcher.dispatch<IPromise>(Actions.fetchTeam(this.teamId)),
                        fetchTeamArticle = offset => dispatcher.dispatch<IPromise>(Actions.fetchTeamArticle(this.teamId, offset)),
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToArticleDetail = id => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {
                                articleId = id
                            }
                        ),
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
                        pushToTeamFollower = teamId => dispatcher.dispatch(
                            new MainNavigatorPushToTeamFollowerAction {
                                teamId = teamId
                            }
                        ),
                        shareToWechat = (type, title, description, linkUrl, imageUrl) => dispatcher.dispatch<IPromise>(
                            Actions.shareToWechat(type, title, description, linkUrl, imageUrl))
                    };
                    return new TeamDetailScreen(viewModel, actionModel);
                }
            );
        }
    }
    
    public class TeamDetailScreen : StatefulWidget {
        public TeamDetailScreen(
            TeamDetailScreenViewModel viewModel = null,
            TeamDetailScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly TeamDetailScreenViewModel viewModel;
        public readonly TeamDetailScreenActionModel actionModel;
        
        public override State createState() {
            return new _TeamDetailScreenState();
        }
    }

    class _TeamDetailScreenState : State<TeamDetailScreen>, TickerProvider {
        const float headerHeight = 256;
        const float _transformSpeed = 0.005f;
        int _articleOffset;
        RefreshController _refreshController;
        float _factor = 1;
        bool _isHaveTitle;
        bool _showNavBarShadow;
        float _topPadding;
        Animation<RelativeRect> _animation;
        AnimationController _controller;
        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(true);
            this._articleOffset = 0;
            this._refreshController = new RefreshController();
            this._isHaveTitle = false;
            this._showNavBarShadow = true;
            this._controller = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            RelativeRectTween rectTween = new RelativeRectTween(
                RelativeRect.fromLTRB(0, 44, 0, 0),
                RelativeRect.fromLTRB(0, 0, 0, 0)
            );
            this._animation = rectTween.animate(this._controller);
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchTeam();
                this.widget.actionModel.fetchTeam();
            });
        }

        public override void dispose() {
            StatusBarManager.statusBarStyle(false);
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick, () => $"created by {this}");
        }

        void _scrollListener() {
            var scrollController = this._refreshController.scrollController;
            if (scrollController.offset < 0) {
                this._factor = 1 + scrollController.offset.abs() * _transformSpeed;
                this.setState(() => { });
            } else {
                if (this._factor != 1) {
                    this.setState(() => this._factor = 1);
                }
            }
        }

        bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;

            if (pixels >= 44 + this._topPadding) {
                if (this._showNavBarShadow) {
                    this.setState(() => this._showNavBarShadow = false);
                    StatusBarManager.statusBarStyle(false);
                }
            }
            else {
                if (!this._showNavBarShadow) {
                    this.setState(() => this._showNavBarShadow = true);
                    StatusBarManager.statusBarStyle(true);
                }
            }

            if (pixels > headerHeight - 24 - (44 + this._topPadding)) {
                if (!this._isHaveTitle) {
                    this._controller.forward();
                    this.setState(() => this._isHaveTitle = true);
                }
            }
            else {
                if (this._isHaveTitle) {
                    this._controller.reverse();
                    this.setState(() => this._isHaveTitle = false);
                }
            }

            return true;
        }

        void _onRefresh(bool up) {
            this._articleOffset = up ? 0 : this.widget.viewModel.teamArticleOffset;
            this.widget.actionModel.fetchTeamArticle(this._articleOffset)
                .Then(() => this._refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up, RefreshStatus.failed));
        }

        void _share(Article article) {
            ShareUtils.showShareView(new ShareView(
                projectType: ProjectType.article,
                onPressed: type => {
                    string linkUrl = $"{Config.apiAddress}/p/{article.id}";
                    if (type == ShareType.clipBoard) {
                        Clipboard.setData(new ClipboardData(linkUrl));
                        CustomDialogUtils.showToast("复制链接成功", Icons.check_circle_outline);
                    }
                    else if (type == ShareType.block) {
                        ReportManager.block(
                            isLoggedIn: this.widget.viewModel.isLoggedIn,
                            () => this.widget.actionModel.pushToLogin(),
                            () => this.widget.actionModel.pushToBlock(article.id),
                            () => this.widget.actionModel.mainRouterPop()
                        );
                    }
                    else if (type == ShareType.report) {
                        ReportManager.report(
                            isLoggedIn: this.widget.viewModel.isLoggedIn,
                            () => this.widget.actionModel.pushToLogin(),
                            () => this.widget.actionModel.pushToReport(arg1: article.id, arg2: ReportType.article)
                        );
                    }
                    else {
                        CustomDialogUtils.showCustomDialog(
                            child: new CustomLoadingDialog()
                        );
                        string imageUrl = $"{article.thumbnail.url}.200x0x1.jpg";
                        this.widget.actionModel.shareToWechat(type, article.title, article.subTitle,
                                linkUrl,
                                imageUrl).Then(CustomDialogUtils.hiddenCustomDialog)
                            .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                    }
                }
            ));
        }

        public override Widget build(BuildContext context) {
            if (this._topPadding != MediaQuery.of(context).padding.top &&
                Application.platform != RuntimePlatform.Android) {
                this._topPadding = MediaQuery.of(context).padding.top;
            }
            Widget content = new Container();
            if (this.widget.viewModel.teamLoading && this.widget.viewModel.team == null) {
                content = new GlobalLoading();
            } else if (this.widget.viewModel.team != null) {
                content = this._buildContent(context: context);
            }
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Stack(
                        children: new List<Widget> {
                            content,
                            this._buildNavigationBar()
                        }
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            Widget titleWidget = new Container();
            if (this._isHaveTitle) {
                titleWidget = new Row(
                    children: new List<Widget> {
                        Avatar.Team(
                            team: this.widget.viewModel.team
                        ),
                        new SizedBox(width: 16),
                        this._buildFollowButton(true)
                    }
                );
            }
            return new Positioned(
                left: 0,
                top: 0,
                right: 0,
                height: 44 + this._topPadding,
                child: new Container(
                    decoration: new BoxDecoration(
                        this._showNavBarShadow ? CColors.Transparent : CColors.White,
                        border: new Border(
                            bottom: new BorderSide(this._isHaveTitle ? CColors.Separator2 : CColors.Transparent))
                    ),
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: new List<Widget> {
                            new GestureDetector(
                                onTap: () => this.widget.actionModel.mainRouterPop(),
                                child: new Container(
                                    padding: EdgeInsets.only(16, 10, 0, 10),
                                    color: CColors.Transparent,
                                    child: new Icon(
                                        Icons.arrow_back,
                                        size: 24,
                                        color: this._showNavBarShadow ? CColors.White : CColors.Icon
                                    )
                                )
                            ),
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
                            )
                        }
                    )
                )
            );
        }
        
        Widget _buildContent(BuildContext context) {
            var teamId = this.widget.viewModel.teamId;
            var articles = this.widget.viewModel.teamArticleDict.ContainsKey(key: teamId)
                ? this.widget.viewModel.teamArticleDict[key: teamId]
                : new List<Article>();
            var articlesHasMore = this.widget.viewModel.teamArticleHasMore;
            int itemCount;
            if (this.widget.viewModel.teamArticleLoading && articles.Count == 0) {
                itemCount = 2;
            }
            else {
                var articleCount = articlesHasMore ? articles.Count : articles.Count + 1;
                itemCount = 2 + (articles.Count == 0 ? 1 : articleCount);
            }
            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: false,
                        enablePullUp: articlesHasMore,
                        onRefresh: this._onRefresh,
                        onNotification: this._onNotification,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: itemCount,
                            itemBuilder: (cxt, index) => {
                                if (index == 0) {
                                    return Transform.scale(
                                        scale: this._factor,
                                        child: this._buildTeamInfo()
                                    );
                                }

                                if (index == 1) {
                                    return _buildTeamArticleTitle();
                                }

                                if (articles.Count == 0 && index == 2) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight - 44;
                                    return new Container(
                                        height: height,
                                        child: new BlankView(
                                            "哎呀，暂无已发布的文章",
                                            "image/default-article"
                                        )
                                    );
                                }

                                if (index == itemCount - 1 && !articlesHasMore) {
                                    return new EndView();
                                }

                                var article = articles[index - 2];
                                return new ArticleCard(
                                    article,
                                    () => this.widget.actionModel.pushToArticleDetail(article.id),
                                    () => this._share(article: article),
                                    this.widget.viewModel.team.name,
                                    key: new ObjectKey(article.id)
                                );
                            }
                        )
                    )
                )
            );
        }

        Widget _buildTeamInfo() {
            var team = this.widget.viewModel.team;
            
            return new CoverImage(
                coverImage: team.coverImage,
                height: headerHeight,
                new Container(
                    padding: EdgeInsets.only(16, 0, 16, 24),
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: new List<Widget> {
                            new Row(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 16),
                                        child: Avatar.Team(
                                            team: team,
                                            80
                                        )
                                    ),
                                    new Expanded(
                                        child: new Column(
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: new List<Widget> {
                                                new Text(
                                                    team.name,
                                                    style: CTextStyle.H4White,
                                                    maxLines: 1,
                                                    overflow: TextOverflow.ellipsis
                                                )
                                            }
                                        )
                                    )
                                }
                            ),
                            new Container(
                                margin: EdgeInsets.only(top: 16),
                                child: this._buildFollowButton()
                            )
                        }
                    )
                )
            );
        }

        static Widget _buildTeamArticleTitle() {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16),
                height: 44,
                decoration: new BoxDecoration(
                    border: new Border(
                        bottom: new BorderSide(
                            color: CColors.Separator2
                        )
                    )
                ),
                alignment: Alignment.centerLeft,
                child: new Text("文章", style: CTextStyle.PLargeTitle)
            );
        }

        Widget _buildFollowButton(bool isTop = false) {
            var team = this.widget.viewModel.team;
            var titleColor = isTop ? CTextStyle.PRegularBody : CTextStyle.PRegularWhite;
            var subTitleColor = new TextStyle(
                height: 1.27f,
                fontSize: 20,
                fontFamily: "Roboto-Bold",
                color: isTop ? CColors.TextBody : CColors.White
            );
            return new GestureDetector(
                onTap: () => this.widget.actionModel.pushToTeamFollower(this.widget.viewModel.teamId),
                child: new Container(
                    height: 32,
                    alignment: Alignment.center,
                    color: CColors.Transparent,
                    child: new Row(
                        children: new List<Widget> {
                            new Text("粉丝", style: titleColor),
                            new SizedBox(width: 2),
                            new Text(
                                $"{team.stats.followCount}",
                                style: subTitleColor
                            )
                        }
                    )
                )
            );
        }
    }
}