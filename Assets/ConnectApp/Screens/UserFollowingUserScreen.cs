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
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class UserFollowingUserScreenConnector : StatelessWidget {
        public UserFollowingUserScreenConnector(
            string userId,
            Key key = null
        ) : base(key: key) {
            this.userId = userId;
        }

        readonly string userId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, UserFollowingScreenViewModel>(
                converter: state => {
                    var user = state.userState.userDict.ContainsKey(key: this.userId)
                        ? state.userState.userDict[key: this.userId]
                        : new User();
                    var followingUsers = user.followingUsers ?? new List<User>();
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followMap = state.followState.followDict.ContainsKey(key: currentUserId)
                        ? state.followState.followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new UserFollowingScreenViewModel {
                        userId = this.userId,
                        followingUserLoading = state.userState.followingUserLoading,
                        searchFollowingUserLoading = state.searchState.searchFollowingLoading,
                        followingUsers = followingUsers,
                        searchFollowingUsers = state.searchState.searchFollowings,
                        searchFollowingKeyword = state.searchState.searchFollowingKeyword,
                        searchFollowingUserHasMore = state.searchState.searchFollowingHasMore,
                        followingUsersHasMore = user.followingUsersHasMore ?? false,
                        followingUserOffset = followingUsers.Count,
                        userDict = state.userState.userDict,
                        userLicenseDict = state.userState.userLicenseDict,
                        followMap = followMap,
                        currentUserId = currentUserId,
                        isLoggedIn = state.loginState.isLoggedIn
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new UserFollowingScreenActionModel {
                        startFetchFollowingUser = () => dispatcher.dispatch(new StartFetchFollowingUserAction()),
                        fetchFollowingUser = offset =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFollowingUser(userId: this.userId, offset: offset)),
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
                        startSearchFollowingUser = () => dispatcher.dispatch(new StartSearchFollowingAction()),
                        searchFollowingUser = (keyword, pageNumber) => dispatcher.dispatch<IPromise>(
                            Actions.searchFollowings(keyword: keyword, pageNumber: pageNumber)),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToUserDetail = userId => dispatcher.dispatch(
                            new MainNavigatorPushToUserDetailAction {
                                userId = userId
                            }
                        )
                    };
                    return new UserFollowingUserScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class UserFollowingUserScreen : StatefulWidget {
        public UserFollowingUserScreen(
            UserFollowingScreenViewModel viewModel = null,
            UserFollowingScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly UserFollowingScreenViewModel viewModel;
        public readonly UserFollowingScreenActionModel actionModel;

        public override State createState() {
            return new _UserFollowingUserScreenState();
        }
    }

    class _UserFollowingUserScreenState : State<UserFollowingUserScreen> {
        int _offset;
        int _pageNumber;
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            this._offset = 0;
            this._pageNumber = 0;
            this._refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchFollowingUser();
                this.widget.actionModel.fetchFollowingUser(0);
            });
        }

        void _onRefreshFollowing(bool up) {
            this._offset = up ? 0 : this.widget.viewModel.followingUserOffset;
            this.widget.actionModel.fetchFollowingUser(arg: this._offset)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        void _onRefreshSearchFollowing(bool up) {
            if (up) {
                this._pageNumber = 1;
            }
            else {
                this._pageNumber++;
            }

            this.widget.actionModel
                .searchFollowingUser(arg1: this.widget.viewModel.searchFollowingKeyword, arg2: this._pageNumber)
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
            Widget content;
            if (this.widget.viewModel.searchFollowingUserLoading) {
                content = new GlobalLoading();
            }
            else if (this.widget.viewModel.searchFollowingKeyword.Length > 0) {
                if (this.widget.viewModel.searchFollowingUsers.Count > 0) {
                    var enablePullUp = this.widget.viewModel.searchFollowingUserHasMore;
                    content = new Container(
                        color: CColors.Background,
                        child: new CustomListView(
                            controller: this._refreshController,
                            enablePullDown: false,
                            enablePullUp: enablePullUp,
                            onRefresh: this._onRefreshSearchFollowing,
                            itemCount: this.widget.viewModel.searchFollowingUsers.Count,
                            itemBuilder: (cxt, index) => {
                                var searchUser = this.widget.viewModel.searchFollowingUsers[index: index];
                                return new UserCard(
                                    user: searchUser,
                                    CCommonUtils.GetUserLicense(userId: searchUser.id,
                                        userLicenseMap: this.widget.viewModel.userLicenseDict),
                                    () => this.widget.actionModel.pushToUserDetail(obj: searchUser.id),
                                    key: new ObjectKey(value: searchUser.id)
                                );
                            },
                            headerWidget: CustomListViewConstant.defaultHeaderWidget,
                            footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                        )
                    );
                }
                else {
                    content = new BlankView(
                        "哎呀，换个关键词试试吧",
                        "image/default-search"
                    );
                }
            }
            else if (this.widget.viewModel.followingUserLoading && this.widget.viewModel.followingUsers.isEmpty()) {
                content = new GlobalLoading();
            }
            else if (this.widget.viewModel.followingUsers.Count <= 0) {
                content = new BlankView(
                    "没有关注的人，去首页看看吧",
                    "image/default-following"
                );
            }
            else {
                var followingUsers = this.widget.viewModel.followingUsers;
                var enablePullUp = this.widget.viewModel.followingUsersHasMore;
                content = new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: enablePullUp,
                    onRefresh: this._onRefreshFollowing,
                    itemCount: followingUsers.Count,
                    itemBuilder: this._buildUserCard,
                    headerWidget: CustomListViewConstant.defaultHeaderWidget,
                    footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                );
            }

            return new Container(
                color: CColors.Background,
                child: content
            );
        }

        Widget _buildUserCard(BuildContext context, int index) {
            var followingUsers = this.widget.viewModel.followingUsers;

            var followingUser = followingUsers[index: index];
            UserType userType = UserType.unFollow;
            if (!this.widget.viewModel.isLoggedIn) {
                userType = UserType.unFollow;
            }
            else {
                bool followUserLoading;
                if (this.widget.viewModel.userDict.ContainsKey(key: followingUser.id)) {
                    var user = this.widget.viewModel.userDict[key: followingUser.id];
                    followUserLoading = user.followUserLoading ?? false;
                }
                else {
                    followUserLoading = false;
                }

                if (this.widget.viewModel.currentUserId == followingUser.id) {
                    userType = UserType.me;
                }
                else if (followUserLoading) {
                    userType = UserType.loading;
                }
                else if (this.widget.viewModel.followMap.ContainsKey(key: followingUser.id)) {
                    userType = UserType.follow;
                }
            }

            return new UserCard(
                user: followingUser,
                CCommonUtils.GetUserLicense(userId: followingUser.id,
                    userLicenseMap: this.widget.viewModel.userLicenseDict),
                () => this.widget.actionModel.pushToUserDetail(obj: followingUser.id),
                userType: userType,
                () => this._onFollow(userType: userType, userId: followingUser.id),
                key: new ObjectKey(value: followingUser.id)
            );
        }
    }
}