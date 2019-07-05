using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
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
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followDict = state.followState.followDict;
                    var followMap = followDict.ContainsKey(key: currentUserId)
                        ? followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new SearchScreenViewModel {
                        searchUserLoading = state.searchState.searchUserLoading,
                        followUserLoading = state.personalState.followUserLoading,
                        searchKeyword = state.searchState.keyword,
                        searchUsers = state.searchState.searchUsers,
                        searchUserHasMore = state.searchState.searchUserHasMore,
                        followMap = followMap,
                        currentFollowId = state.personalState.currentFollowId,
                        currentUserId = currentUserId,
                        isLoggedIn = state.loginState.isLoggedIn
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new SearchScreenActionModel {
                        pushToPersonalDetail = personalId => dispatcher.dispatch(
                            new MainNavigatorPushToPersonalDetailAction {personalId = personalId}),
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
        int _pageNumber;
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            this._pageNumber = 1;
            this._refreshController = new RefreshController();
        }

        void _onRefresh(bool up) {
            if (up) {
                this._pageNumber = 1;
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
                    this.widget.actionModel.startUnFollowUser(userId);
                    this.widget.actionModel.unFollowUser(userId);
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
            Widget child = new Container();
            if (this.widget.viewModel.searchUserLoading) {
                child = new GlobalLoading();
            }
            else if (this.widget.viewModel.searchKeyword.Length > 0) {
                child = this.widget.viewModel.searchUsers.Count > 0
                    ? this._buildContent()
                    : new BlankView(
                        "哎呀，换个关键词试试吧",
                        "image/default-search",
                        true,
                        () => {
                            this.widget.actionModel.startSearchUser();
                            this.widget.actionModel.searchUser(this.widget.viewModel.searchKeyword, 1);
                        }
                    );
            }

            return new Container(
                color: CColors.Background,
                child: child
            );
        }
        
        Widget _buildContent() {
            return new Container(
                color: CColors.White,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: false,
                        enablePullUp: this.widget.viewModel.searchUserHasMore,
                        onRefresh: this._onRefresh,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            padding: EdgeInsets.only(top: 16),
                            itemExtent: 72,
                            itemCount: this.widget.viewModel.searchUsers.Count,
                            itemBuilder: this._buildUserCard
                        )
                    )
                )
            );
        }
        
        Widget _buildUserCard(BuildContext context, int index) {
            var searchUser = this.widget.viewModel.searchUsers[index];
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
                } else if (this.widget.viewModel.followMap.ContainsKey(searchUser.id)) {
                    userType = UserType.follow;
                }
            }
            return new UserCard(
                searchUser,
                () => this.widget.actionModel.pushToPersonalDetail(searchUser.id),
                userType,
                () => this._onFollow(userType: userType, userId: searchUser.id),
                new ObjectKey(searchUser.id)
            );
        }
    }
}