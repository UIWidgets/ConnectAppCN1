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
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class UserFollowerScreenConnector : StatelessWidget {
        public UserFollowerScreenConnector(
            string userId,
            Key key = null
        ) : base(key: key) {
            this.userId = userId;
        }
        
        readonly string userId;
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, UserFollowerScreenViewModel>(
                converter: state => {
                    var user = state.userState.userDict.ContainsKey(key: this.userId)
                        ? state.userState.userDict[key: this.userId]
                        : new User();
                    var followers = user.followers ?? new List<User>();
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followMap = state.followState.followDict.ContainsKey(key: currentUserId)
                        ? state.followState.followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new UserFollowerScreenViewModel {
                        userId = this.userId,
                        followerLoading = state.userState.followerLoading,
                        followUserLoading = state.userState.followUserLoading,
                        followers = followers,
                        followersHasMore = user.followersHasMore,
                        userOffset = followers.Count,
                        followMap = followMap,
                        currentFollowId = state.userState.currentFollowId,
                        currentUserId = currentUserId,
                        isLoggedIn = state.loginState.isLoggedIn
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new UserFollowerScreenActionModel {
                        startFetchFollower = () => dispatcher.dispatch(new StartFetchFollowerAction()),
                        fetchFollower = offset => dispatcher.dispatch<IPromise>(Actions.fetchFollower(this.userId, offset)),
                        startFollowUser = followUserId => dispatcher.dispatch(new StartFetchFollowUserAction {
                            followUserId = followUserId
                        }),
                        followUser = followUserId => dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(followUserId)),
                        startUnFollowUser = unFollowUserId => dispatcher.dispatch(new StartFetchUnFollowUserAction {
                            unFollowUserId = unFollowUserId
                        }),
                        unFollowUser = unFollowUserId => dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(unFollowUserId)),
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToUserDetail = userId => dispatcher.dispatch(
                            new MainNavigatorPushToUserDetailAction {
                                userId = userId
                            }
                        )
                    };
                    return new UserFollowerScreen(viewModel, actionModel);
                }
            );
        }
    }
    
    public class UserFollowerScreen : StatefulWidget {
        public UserFollowerScreen(
            UserFollowerScreenViewModel viewModel = null,
            UserFollowerScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly UserFollowerScreenViewModel viewModel;
        public readonly UserFollowerScreenActionModel actionModel;
        
        public override State createState() {
            return new _UserFollowerScreenState();
        }
    }
    
    class _UserFollowerScreenState : State<UserFollowerScreen> {
        int _userOffset;
        RefreshController _refreshController;
        string _title;
        
        public override void initState() {
            base.initState();
            this._userOffset = 0;
            this._refreshController = new RefreshController();
            this._title = this.widget.viewModel.currentUserId == this.widget.viewModel.userId 
                ? "我的粉丝"
                : "全部粉丝";
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchFollower();
                this.widget.actionModel.fetchFollower(0);
            });
        }
        
        void _onRefreshFollower(bool up) {
            this._userOffset = up ? 0 : this.widget.viewModel.userOffset;
            this.widget.actionModel.fetchFollower(this._userOffset)
                .Then(() => this._refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up, RefreshStatus.failed));
        }

        void _onFollow(UserType userType, string userId) {
            if (this.widget.viewModel.isLoggedIn) {
                if (userType == UserType.follow) {
                    ActionSheetUtils.showModalActionSheet(
                        new ActionSheet(
                            title: "确定不太关注？",
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
            Widget content = new Container();
            if (this.widget.viewModel.followerLoading && this.widget.viewModel.followers.isEmpty()) {
                content = new GlobalLoading();
            } else if (this.widget.viewModel.followers.Count <= 0) {
                content = new BlankView(
                    $"暂无{this._title}用户",
                    "image/default-following",
                    true,
                    () => {
                        this.widget.actionModel.startFetchFollower();
                        this.widget.actionModel.fetchFollower(0);
                    }
                );
            }
            else {
                content = new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: true,
                        enablePullUp: this.widget.viewModel.followersHasMore,
                        onRefresh: this._onRefreshFollower,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: this.widget.viewModel.followers.Count,
                            itemBuilder: this._buildUserCard
                        )
                    )
                );
            }
            return new Container(
                color: CColors.Background,
                child: new CustomSafeArea(
                    child: new Column(
                        children: new List<Widget> {
                            this._buildNavigationBar(context),
                            new Expanded(
                                child: content
                            )
                        }
                    )
                )
            );
        }
        
        Widget _buildNavigationBar(BuildContext context) {
            return new Container(
                color: CColors.White,
                width: MediaQuery.of(context: context).size.width,
                height: 94,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.symmetric(8, 16),
                            onPressed: () => this.widget.actionModel.mainRouterPop(),
                            child: new Icon(
                                icon: Icons.arrow_back,
                                size: 24,
                                color: CColors.Icon
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(16, bottom: 8),
                            child: new Text(
                                data: this._title,
                                style: CTextStyle.H2
                            )
                        )
                    }
                )
            );
        }

        Widget _buildUserCard(BuildContext context, int index) {
            var user = this.widget.viewModel.followers[index: index];
            UserType userType = UserType.unFollow;
            if (!this.widget.viewModel.isLoggedIn) {
                userType = UserType.unFollow;
            }
            else {
                if (this.widget.viewModel.currentUserId == user.id) {
                    userType = UserType.me;
                }  else if (this.widget.viewModel.followUserLoading
                           && this.widget.viewModel.currentFollowId == user.id) {
                    userType = UserType.loading;
                } else if (this.widget.viewModel.followMap.ContainsKey(key: user.id)) {
                    userType = UserType.follow;
                }
            }
            return new UserCard(
                user: user,
                () => this.widget.actionModel.pushToUserDetail(obj: user.id),
                userType: userType,
                () => this._onFollow(userType: userType, userId: user.id),
                new ObjectKey(user.id)
            );
        }
    }
}