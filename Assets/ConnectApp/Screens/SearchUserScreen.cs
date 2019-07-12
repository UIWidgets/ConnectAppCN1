using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class SearchUserScreenConnector : StatelessWidget {
        public SearchUserScreenConnector(
            Key key = null
        ) : base(key: key) {
        }
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, SearchScreenViewModel>(
                converter: state => {
                    var searchUsers = state.searchState.searchUsers.ContainsKey(key: state.searchState.keyword)
                        ? state.searchState.searchUsers[key: state.searchState.keyword]
                        : null;
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followMap = state.followState.followDict.ContainsKey(key: currentUserId)
                        ? state.followState.followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new SearchScreenViewModel {
                        searchUserLoading = state.searchState.searchUserLoading,
                        followUserLoading = state.userState.followUserLoading,
                        searchKeyword = state.searchState.keyword,
                        searchUsers = searchUsers,
                        searchUserHasMore = state.searchState.searchUserHasMore,
                        followMap = followMap,
                        currentFollowId = state.userState.currentFollowId,
                        currentUserId = currentUserId,
                        isLoggedIn = state.loginState.isLoggedIn
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new SearchScreenActionModel {
                        pushToUserDetail = userId => dispatcher.dispatch(
                            new MainNavigatorPushToUserDetailAction {userId = userId}),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        startSearchUser = () => dispatcher.dispatch(new StartSearchUserAction()),
                        searchUser = (keyword, pageNumber) => dispatcher.dispatch<IPromise>(
                            Actions.searchUsers(keyword, pageNumber)),
                        startFollowUser = followUserId => dispatcher.dispatch(new StartFetchFollowUserAction {
                            followUserId = followUserId
                        }),
                        followUser = followUserId => dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(followUserId)),
                        startUnFollowUser = unFollowUserId => dispatcher.dispatch(new StartFetchUnFollowUserAction {
                            unFollowUserId = unFollowUserId
                        }),
                        unFollowUser = unFollowUserId => dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(unFollowUserId))
                    };
                    return new SearchUserScreen(viewModel, actionModel);
                }
            );
        }
    }
    
    public class SearchUserScreen : StatefulWidget {
        public SearchUserScreen(
            SearchScreenViewModel viewModel = null,
            SearchScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly SearchScreenViewModel viewModel;
        public readonly SearchScreenActionModel actionModel;

        public override State createState() {
            return new _SearchUserScreenState();
        }
    }

    class _SearchUserScreenState : State<SearchUserScreen> {
        const int _initPageNumber = 1;
        int _pageNumber = _initPageNumber;
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
        }

        void _onRefresh(bool up) {
            if (up) {
                this._pageNumber = _initPageNumber;
            }
            else {
                this._pageNumber++;
            }

            this.widget.actionModel.searchUser(this.widget.viewModel.searchKeyword, this._pageNumber)
                .Then(() => this._refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up, RefreshStatus.failed));
        }
        
        void _onFollow(UserType userType, string userId) {
            if (this.widget.viewModel.isLoggedIn) {
                if (userType == UserType.follow) {
                    ActionSheetUtils.showModalActionSheet(
                        new ActionSheet(
                            title: "确定不再关注？",
                            items: new List<ActionSheetItem> {
                                new ActionSheetItem("确定", ActionType.normal,
                                    () => {
                                        this.widget.actionModel.startUnFollowUser(userId);
                                        this.widget.actionModel.unFollowUser(userId);
                                    }),
                                new ActionSheetItem("取消", ActionType.cancel)
                            }
                        )
                    );
                }
                if (userType == UserType.unFollow) {
                    this.widget.actionModel.startFollowUser(userId);
                    this.widget.actionModel.followUser(userId);
                }
            }
            else {
                this.widget.actionModel.pushToLogin();
            }
        }

        public override Widget build(BuildContext context) {
            var searchUsers = this.widget.viewModel.searchUsers;
            var searchKeyword = this.widget.viewModel.searchKeyword ?? "";
            Widget child = new Container();
            if (this.widget.viewModel.searchUserLoading && searchUsers == null) {
                child = new GlobalLoading();
            }
            else if (searchKeyword.Length > 0) {
                child = searchUsers != null && searchUsers.Count > 0
                    ? this._buildContent()
                    : new BlankView(
                        "哎呀，换个关键词试试吧",
                        "image/default-search"
                    );
            }

            return new Container(
                color: CColors.White,
                child: child
            );
        }
        
        Widget _buildContent() {
            var itemCount = this.widget.viewModel.searchUserHasMore
                ? this.widget.viewModel.searchUsers.Count + 1
                : this.widget.viewModel.searchUsers.Count + 2;
            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: false,
                        enablePullUp: this.widget.viewModel.searchUserHasMore,
                        onRefresh: this._onRefresh,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: itemCount,
                            itemBuilder: this._buildUserCard
                        )
                    )
                )
            );
        }
        
        Widget _buildUserCard(BuildContext context, int index) {
            if (index == 0) {
                return new CustomDivider(
                    color: CColors.White
                );
            }
            var searchUsers = this.widget.viewModel.searchUsers;
            if (index == searchUsers.Count + 1) {
                return new EndView();
            }

            var searchUser = searchUsers[index - 1];
            UserType userType = UserType.unFollow;
            if (!this.widget.viewModel.isLoggedIn) {
                userType = UserType.unFollow;
            }
            else {
                if (this.widget.viewModel.currentUserId == searchUser.id) {
                    userType = UserType.me;
                }  else if (this.widget.viewModel.followUserLoading
                           && this.widget.viewModel.currentFollowId == searchUser.id) {
                    userType = UserType.loading;
                } else if (this.widget.viewModel.followMap.ContainsKey(key: searchUser.id)) {
                    userType = UserType.follow;
                }
            }
            return new UserCard(
                user: searchUser,
                () => this.widget.actionModel.pushToUserDetail(searchUser.id),
                userType: userType,
                () => this._onFollow(userType: userType, userId: searchUser.id),
                new ObjectKey(value: searchUser.id)
            );
        }
    }
}