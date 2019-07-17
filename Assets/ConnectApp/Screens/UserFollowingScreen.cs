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
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class UserFollowingScreenConnector : StatelessWidget {
        public UserFollowingScreenConnector(
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
                    var followings = user.followings ?? new List<User>();
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followMap = state.followState.followDict.ContainsKey(key: currentUserId)
                        ? state.followState.followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new UserFollowingScreenViewModel {
                        userId = this.userId,
                        followingLoading = state.userState.followingLoading,
                        searchFollowingLoading = state.searchState.searchFollowingLoading,
                        followings = followings,
                        searchFollowings = state.searchState.searchFollowings,
                        searchFollowingKeyword = state.searchState.searchFollowingKeyword,
                        searchFollowingHasMore = state.searchState.searchFollowingHasMore,
                        followingsHasMore = user.followingsHasMore ?? false,
                        userOffset = followings.Count,
                        userDict = state.userState.userDict,
                        followMap = followMap,
                        currentUserId = currentUserId,
                        isLoggedIn = state.loginState.isLoggedIn
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new UserFollowingScreenActionModel {
                        startFetchFollowing = () => dispatcher.dispatch(new StartFetchFollowingAction()),
                        fetchFollowing = offset => dispatcher.dispatch<IPromise>(Actions.fetchFollowing(this.userId, offset)),
                        startFollowUser = followUserId => dispatcher.dispatch(new StartFetchFollowUserAction {
                            followUserId = followUserId
                        }),
                        followUser = followUserId => dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(followUserId)),
                        startUnFollowUser = unFollowUserId => dispatcher.dispatch(new StartFetchUnFollowUserAction {
                            unFollowUserId = unFollowUserId
                        }),
                        unFollowUser = unFollowUserId => dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(unFollowUserId)),
                        startSearchFollowing = () => dispatcher.dispatch(new StartSearchFollowingAction()),
                        searchFollowing = (keyword, pageNumber) => dispatcher.dispatch<IPromise>(
                            Actions.searchFollowings(keyword, pageNumber)),
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToUserDetail = userId => dispatcher.dispatch(
                            new MainNavigatorPushToUserDetailAction {
                                userId = userId
                            }
                        ),
                        clearSearchFollowingResult = () => dispatcher.dispatch(new ClearSearchFollowingResultAction())
                    };
                    return new UserFollowingScreen(viewModel, actionModel);
                }
            );
        }
    }
    
    public class UserFollowingScreen : StatefulWidget {
        public UserFollowingScreen(
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
            return new _UserFollowingScreenState();
        }
    }
    
    class _UserFollowingScreenState : State<UserFollowingScreen> {
        readonly TextEditingController _controller = new TextEditingController("");
        int _userOffset;
        int _pageNumber;
        RefreshController _refreshController;
        FocusNode _focusNode;
        string _title;
        
        public override void initState() {
            base.initState(); 
            StatusBarManager.statusBarStyle(false);
            this._userOffset = 0;
            this._pageNumber = 0;
            this._refreshController = new RefreshController();
            this._focusNode = new FocusNode();
            this._title = this.widget.viewModel.currentUserId == this.widget.viewModel.userId 
                ? "我关注的"
                : "全部关注";
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (this.widget.viewModel.searchFollowingKeyword.Length > 0
                    || this.widget.viewModel.searchFollowings.Count > 0) {
                    this.widget.actionModel.clearSearchFollowingResult();
                }
                this.widget.actionModel.startFetchFollowing();
                this.widget.actionModel.fetchFollowing(0);
            });
        }

        void _searchFollowing(string text) {
            if (text.isEmpty()) {
                return;
            }

            if (this._focusNode.hasFocus) {
                this._focusNode.unfocus();
            }

            this._controller.text = text;
            this.widget.actionModel.startSearchFollowing();
            this.widget.actionModel.searchFollowing(arg1: text, 0);
        }
        
        void _onRefreshFollowing(bool up) {
            this._userOffset = up ? 0 : this.widget.viewModel.userOffset;
            this.widget.actionModel.fetchFollowing(arg: this._userOffset)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }
        
        void _onRefreshSearchFollowing(bool up) {
            if (up) {
                this._pageNumber = 0;
            }
            else {
                this._pageNumber++;
            }
            this.widget.actionModel.searchFollowing(arg1: this.widget.viewModel.searchFollowingKeyword, arg2: this._pageNumber)
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
            if (this.widget.viewModel.searchFollowingLoading) {
                content = new GlobalLoading();
            } else if (this.widget.viewModel.searchFollowingKeyword.Length > 0) {
                if (this.widget.viewModel.searchFollowings.Count > 0) {
                    content = new Container(
                        color: CColors.Background,
                        child: new CustomScrollbar(
                            new SmartRefresher(
                                controller: this._refreshController,
                                enablePullDown: false,
                                enablePullUp: this.widget.viewModel.searchFollowingHasMore,
                                onRefresh: this._onRefreshSearchFollowing,
                                child: ListView.builder(
                                    physics: new AlwaysScrollableScrollPhysics(),
                                    itemCount: this.widget.viewModel.searchFollowings.Count,
                                    itemBuilder: (cxt, index) => {
                                        var searchUser = this.widget.viewModel.searchFollowings[index: index];
                                        return new UserCard(
                                            user: searchUser,
                                            () => this.widget.actionModel.pushToUserDetail(obj: searchUser.id),
                                            key: new ObjectKey(value: searchUser.id)
                                        );
                                    }
                                )
                            )
                        )
                    );
                }
                else {
                    content = new BlankView(
                        "哎呀，换个关键词试试吧",
                        "image/default-search",
                        true,
                        () => {
                            this.widget.actionModel.startFetchFollowing();
                            this.widget.actionModel.fetchFollowing(0);
                        }
                    );
                }
            }
            else if (this.widget.viewModel.followingLoading && this.widget.viewModel.followings.isEmpty()) {
                content = new GlobalLoading();
            } else if (this.widget.viewModel.followings.Count <= 0) {
                content = new BlankView(
                    "没有关注的人，去首页看看吧",
                    "image/default-following",
                    true,
                    () => {
                        this.widget.actionModel.startFetchFollowing();
                        this.widget.actionModel.fetchFollowing(0);
                    }
                );
            }
            else {
                content = new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: true,
                        enablePullUp: this.widget.viewModel.followingsHasMore,
                        onRefresh: this._onRefreshFollowing,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: this.widget.viewModel.followings.Count,
                            itemBuilder: (cxt, index) => {
                                var following = this.widget.viewModel.followings[index: index];
                                UserType userType = UserType.unFollow;
                                if (!this.widget.viewModel.isLoggedIn) {
                                    userType = UserType.unFollow;
                                }
                                else {
                                    var followUserLoading = false;
                                    if (this.widget.viewModel.userDict.ContainsKey(key: following.id)) {
                                        var user = this.widget.viewModel.userDict[key: following.id];
                                        followUserLoading = user.followUserLoading ?? false;
                                    }
                                    if (this.widget.viewModel.currentUserId == following.id) {
                                        userType = UserType.me;
                                    } else if (followUserLoading) {
                                        userType = UserType.loading;
                                    } else if (this.widget.viewModel.followMap.ContainsKey(key: following.id)) {
                                        userType = UserType.follow;
                                    }
                                }
                                return new UserCard(
                                    user: following,
                                    () => this.widget.actionModel.pushToUserDetail(obj: following.id),
                                    userType: userType,
                                    () => this._onFollow(userType: userType, userId: following.id),
                                    new ObjectKey(value: following.id)
                                );
                            }
                        )
                    )
                );
            }
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(context: context),
                                // this._buildSearchBar(),
                                new Expanded(
                                    child: content
                                )
                            }
                        )
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

        Widget _buildSearchBar() {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, 16, 16, 12),
                child: new InputField(
                    decoration: new BoxDecoration(
                        color: CColors.Separator2,
                        borderRadius: BorderRadius.all(8)
                    ),
                    height: 40,
                    controller: this._controller,
                    focusNode: this._focusNode,
                    style: CTextStyle.PLargeBody2,
                    prefix: new Container(
                        padding: EdgeInsets.only(11, 9, 7, 9),
                        child: new Icon(
                            icon: Icons.search,
                            color: CColors.BrownGrey
                        )
                    ),
                    hintText: "搜索",
                    hintStyle: CTextStyle.PLargeBody4,
                    cursorColor: CColors.PrimaryBlue,
                    textInputAction: TextInputAction.search,
                    clearButtonMode: InputFieldClearButtonMode.whileEditing,
                    onChanged: text => {
                        if (text == null || text.Length <= 0) {
                            this.widget.actionModel.clearSearchFollowingResult();
                        }
                    },
                    onSubmitted: this._searchFollowing
                )
            );
        }
    }
}