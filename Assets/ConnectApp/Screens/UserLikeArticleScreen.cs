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
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class UserLikeArticleScreenConnector : StatelessWidget {
        public UserLikeArticleScreenConnector(
            string userId,
            Key key = null
        ) : base(key: key) {
            this.userId = userId;
        }

        readonly string userId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, UserLikeArticleScreenViewModel>(
                converter: state => {
                    var user = state.userState.userDict.ContainsKey(key: this.userId)
                        ? state.userState.userDict[key: this.userId]
                        : new User();
                    var likeArticleIds = user.likeArticleIds ?? new List<string>();
                    return new UserLikeArticleScreenViewModel {
                        likeArticleLoading = state.userState.userLikeArticleLoading,
                        likeArticleIds = likeArticleIds,
                        likeArticlePage = user.likeArticlesPage ?? 1,
                        likeArticleHasMore = user.likeArticlesHasMore ?? false,
                        isLoggedIn = state.loginState.isLoggedIn,
                        articleDict = state.articleState.articleDict,
                        userDict = state.userState.userDict,
                        teamDict = state.teamState.teamDict
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new UserLikeArticleScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        startFetchUserLikeArticle = () => dispatcher.dispatch(new StartFetchUserLikeArticleAction()),
                        fetchUserLikeArticle = pageNumber =>
                            dispatcher.dispatch<IPromise>(Actions.fetchUserLikeArticle(userId: this.userId, pageNumber: pageNumber)),
                        pushToArticleDetail = articleId => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {
                                articleId = articleId
                            }
                        ),
                        pushToReport = reportId => dispatcher.dispatch(
                            new MainNavigatorPushToReportAction {
                                reportId = reportId,
                                reportType = ReportType.article
                            }
                        ),
                        pushToBlock = articleId => {
                            dispatcher.dispatch(new BlockArticleAction {articleId = articleId});
                            dispatcher.dispatch(new DeleteArticleHistoryAction {articleId = articleId});
                        },
                        shareToWechat = (type, title, description, linkUrl, imageUrl) => dispatcher.dispatch<IPromise>(
                            Actions.shareToWechat(type: type, title: title, description: description, linkUrl: linkUrl,
                                imageUrl: imageUrl))
                    };
                    return new UserLikeArticleScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class UserLikeArticleScreen : StatefulWidget {
        public UserLikeArticleScreen(
            UserLikeArticleScreenViewModel viewModel,
            UserLikeArticleScreenActionModel actionModel,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly UserLikeArticleScreenViewModel viewModel;
        public readonly UserLikeArticleScreenActionModel actionModel;

        public override State createState() {
            return new _UserLikeArticleScreenState();
        }
    }

    class _UserLikeArticleScreenState : State<UserLikeArticleScreen>, RouteAware {
        const int firstPageNumber = 1;
        int likeArticlePageNumber = firstPageNumber;
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchUserLikeArticle();
                this.widget.actionModel.fetchUserLikeArticle(arg: firstPageNumber);
            });
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        void _onRefresh(bool up) {
            this.likeArticlePageNumber = up ? firstPageNumber : this.likeArticlePageNumber + 1;
            this.widget.actionModel.fetchUserLikeArticle(arg: this.likeArticlePageNumber)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            var likeArticleIds = this.widget.viewModel.likeArticleIds;
            Widget content;
            if (this.widget.viewModel.likeArticleLoading && likeArticleIds.isEmpty()) {
                content = new GlobalLoading();
            }
            else if (likeArticleIds.Count <= 0) {
                content = new BlankView(
                    "暂无点赞的文章",
                    "image/default-article",
                    true,
                    () => {
                        this.widget.actionModel.startFetchUserLikeArticle();
                        this.widget.actionModel.fetchUserLikeArticle(arg: firstPageNumber);
                    }
                );
            }
            else {
                var enablePullUp = this.widget.viewModel.likeArticleHasMore;
                content = new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: enablePullUp,
                    onRefresh: this._onRefresh,
                    itemCount: likeArticleIds.Count,
                    itemBuilder: this._buildUserCard,
                    footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                );
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Expanded(
                                    child: content
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomNavigationBar(
                new Text(
                    "点赞的文章",
                    style: CTextStyle.H2
                ),
                onBack: () => this.widget.actionModel.mainRouterPop()
            );
        }

        Widget _buildUserCard(BuildContext context, int index) {
            var articleId = this.widget.viewModel.likeArticleIds[index: index];
            var articleDict = this.widget.viewModel.articleDict;
            if (!articleDict.ContainsKey(key: articleId)) {
                return new Container();
            }

            var article = articleDict[key: articleId];
            var linkUrl = CStringUtils.JointProjectShareLink(projectId: article.id);
            var fullName = "";
            if (article.ownerType == OwnerType.user.ToString()) {
                if (this.widget.viewModel.userDict.ContainsKey(key: article.userId)) {
                    fullName = this.widget.viewModel.userDict[key: article.userId].fullName
                               ?? this.widget.viewModel.userDict[key: article.userId].name;
                }
            }

            if (article.ownerType == OwnerType.team.ToString()) {
                if (this.widget.viewModel.teamDict.ContainsKey(key: article.teamId)) {
                    fullName = this.widget.viewModel.teamDict[key: article.teamId].name;
                }
            }

            return new ArticleCard(
                article: article,
                () => this.widget.actionModel.pushToArticleDetail(obj: article.id),
                () => ShareManager.showArticleShareView(
                    false,
                    isLoggedIn: this.widget.viewModel.isLoggedIn,
                    () => {
                        Clipboard.setData(new ClipboardData(text: linkUrl));
                        CustomDialogUtils.showToast("复制链接成功", iconData: Icons.check_circle_outline);
                    },
                    () => this.widget.actionModel.pushToLogin(),
                    () => this.widget.actionModel.pushToBlock(obj: article.id),
                    () => this.widget.actionModel.pushToReport(obj: article.id),
                    type => {
                        CustomDialogUtils.showCustomDialog(
                            child: new CustomLoadingDialog()
                        );
                        string imageUrl = CImageUtils.SizeTo200ImageUrl(imageUrl: article.thumbnail.url);
                        this.widget.actionModel.shareToWechat(arg1: type, arg2: article.title,
                                arg3: article.subTitle, arg4: linkUrl, arg5: imageUrl)
                            .Then(onResolved: CustomDialogUtils.hiddenCustomDialog)
                            .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                    }
                ),
                fullName: fullName,
                new ObjectKey(value: article.id)
            );
        }

        public void didPopNext() {
            StatusBarManager.statusBarStyle(false);
        }

        public void didPush() {
        }

        public void didPop() {
        }

        public void didPushNext() {
        }
    }
}