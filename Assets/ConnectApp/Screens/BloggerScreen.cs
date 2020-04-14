using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class BloggerScreenConnector : StatelessWidget {
        public BloggerScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, BloggerScreenViewModel>(
                converter: state => {
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followMap = state.followState.followDict.ContainsKey(key: currentUserId)
                        ? state.followState.followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new BloggerScreenViewModel {
                        bloggerLoading = state.leaderBoardState.homeBloggerLoading,
                        bloggerIds = state.leaderBoardState.homeBloggerIds,
                        bloggerHasMore = state.leaderBoardState.homeBloggerHasMore,
                        bloggerPageNumber = state.leaderBoardState.homeBloggerPageNumber,
                        isLoggedIn = state.loginState.isLoggedIn,
                        currentUserId = currentUserId,
                        userDict = state.userState.userDict,
                        followMap = followMap,
                        userLicenseDict = state.userState.userLicenseDict
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new BloggerScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        startFetchBlogger = () => dispatcher.dispatch(new StartFetchHomeBloggerAction()),
                        fetchBlogger = page => dispatcher.dispatch<IPromise>(Actions.fetchHomeBlogger(page: page)),
                        startFollowUser = followUserId => dispatcher.dispatch(new StartFollowUserAction {
                            followUserId = followUserId
                        }),
                        followUser = followUserId =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(followUserId: followUserId)),
                        startUnFollowUser = unFollowUserId => dispatcher.dispatch(new StartUnFollowUserAction {
                            unFollowUserId = unFollowUserId
                        }),
                        unFollowUser = unFollowUserId =>
                            dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(unFollowUserId: unFollowUserId)),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToUserDetail = userId => dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {
                            userId = userId
                        })
                    };
                    return new BloggerScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }
    public class BloggerScreen : StatefulWidget {
        public BloggerScreen(
            BloggerScreenViewModel viewModel,
            BloggerScreenActionModel actionModel,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly BloggerScreenViewModel viewModel;
        public readonly BloggerScreenActionModel actionModel;

        public override State createState() {
            return new _BloggerScreenState();
        }
    }

    class _BloggerScreenState : State<BloggerScreen>, RouteAware {
        const int firstPageNumber = 1;
        int bloggerPageNumber = firstPageNumber;
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchBlogger();
                this.widget.actionModel.fetchBlogger(arg: firstPageNumber);
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
            this.bloggerPageNumber = up ? firstPageNumber : this.bloggerPageNumber + 1;
            this.widget.actionModel.fetchBlogger(arg: this.bloggerPageNumber)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        void _onFollow(UserType userType, string userId) {
            if (this.widget.viewModel.isLoggedIn) {
                if (userType == UserType.follow) {
                    ActionSheetUtils.showModalActionSheet(
                        new ActionSheet(
                            title: "确定不再关注？",
                            items: new List<ActionSheetItem> {
                                new ActionSheetItem("确定", type: ActionType.normal,
                                    () => {
                                        this.widget.actionModel.startUnFollowUser(obj: userId);
                                        this.widget.actionModel.unFollowUser(arg: userId);
                                    }),
                                new ActionSheetItem("取消", type: ActionType.cancel)
                            }
                        )
                    );
                }

                if (userType == UserType.unFollow) {
                    this.widget.actionModel.startFollowUser(obj: userId);
                    this.widget.actionModel.followUser(arg: userId);
                }
            }
            else {
                this.widget.actionModel.pushToLogin();
            }
        }

        public override Widget build(BuildContext context) {
            var bloggerIds = this.widget.viewModel.bloggerIds;
            Widget content;
            if (this.widget.viewModel.bloggerLoading && bloggerIds.isEmpty()) {
                content = new GlobalLoading();
            }
            else if (bloggerIds.Count <= 0) {
                content = new BlankView(
                    "暂无博主",
                    "image/default-following",
                    true,
                    () => {
                        this.widget.actionModel.startFetchBlogger();
                        this.widget.actionModel.fetchBlogger(arg: firstPageNumber);
                    }
                );
            }
            else {
                var enablePullUp = this.widget.viewModel.bloggerHasMore;
                content = new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: enablePullUp,
                    onRefresh: this._onRefresh,
                    itemCount: bloggerIds.Count,
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
                                new Flexible(
                                    child: content
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomAppBar(
                () => this.widget.actionModel.mainRouterPop(),
                new Text(
                    "博主",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildUserCard(BuildContext context, int index) {
            var bloggerId = this.widget.viewModel.bloggerIds[index: index];
            if (!this.widget.viewModel.userDict.ContainsKey(key: bloggerId)) {
                return new Container();
            }

            var user = this.widget.viewModel.userDict[key: bloggerId];
            UserType userType = UserType.unFollow;
            if (!this.widget.viewModel.isLoggedIn) {
                userType = UserType.unFollow;
            }
            else {
                if (this.widget.viewModel.currentUserId == user.id) {
                    userType = UserType.me;
                }
                else if (user.followUserLoading ?? false) {
                    userType = UserType.loading;
                }
                else if (this.widget.viewModel.followMap.ContainsKey(key: user.id)) {
                    userType = UserType.follow;
                }
            }

            return new UserCard(
                user: user,
                CCommonUtils.GetUserLicense(userId: user.id, userLicenseMap: this.widget.viewModel.userLicenseDict),
                () => this.widget.actionModel.pushToUserDetail(obj: user.id),
                userType: userType,
                () => this._onFollow(userType: userType, userId: user.id),
                key: new ObjectKey(value: user.id)
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