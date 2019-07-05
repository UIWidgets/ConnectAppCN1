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
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class FollowingUserScreenConnector : StatelessWidget {
        public FollowingUserScreenConnector(
            string personalId,
            Key key = null
        ) : base(key: key) {
            this.personalId = personalId;
        }

        readonly string personalId;
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, FollowingUserScreenViewModel>(
                converter: state => {
                    Personal personal = new Personal();
                    if (state.personalState.personalDict.ContainsKey(key: this.personalId)) {
                        personal = state.personalState.personalDict[key: this.personalId];
                    }
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followDict = state.followState.followDict;
                    var followMap = followDict.ContainsKey(currentUserId)
                        ? followDict[currentUserId]
                        : new Dictionary<string, bool>();
                    return new FollowingUserScreenViewModel {
                        personalId = this.personalId,
                        followingLoading = state.personalState.followingLoading,
                        searchFollowingLoading = state.searchState.searchFollowingLoading,
                        followUserLoading = state.personalState.followUserLoading,
                        followings = personal.followings,
                        searchFollowings = state.searchState.searchFollowings,
                        searchFollowingKeyword = state.searchState.searchFollowingKeyword,
                        searchFollowingHasMore = state.searchState.searchFollowingHasMore,
                        followingsHasMore = personal.followingsHasMore,
                        userOffset = personal.followings.Count,
                        followMap = followMap,
                        currentFollowId = state.personalState.currentFollowId,
                        currentUserId = currentUserId,
                        isLoggedIn = state.loginState.isLoggedIn
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new FollowingUserScreenActionModel {
                        startFetchFollowing = () => dispatcher.dispatch(new StartFetchFollowingAction()),
                        fetchFollowing = offset => dispatcher.dispatch<IPromise>(Actions.fetchFollowing(this.personalId, offset)),
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
                        pushToPersonalDetail = personalId => dispatcher.dispatch(
                            new MainNavigatorPushToPersonalDetailAction {
                                personalId = personalId
                            }
                        ),
                        clearSearchFollowingResult = () => dispatcher.dispatch(new ClearSearchFollowingResultAction())
                    };
                    return new FollowingUserScreen(viewModel, actionModel);
                }
            );
        }
    }
    
    public class FollowingUserScreen : StatefulWidget {
        public FollowingUserScreen(
            FollowingUserScreenViewModel viewModel = null,
            FollowingUserScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly FollowingUserScreenViewModel viewModel;
        public readonly FollowingUserScreenActionModel actionModel;
        
        public override State createState() {
            return new _FollowingUserScreenState();
        }
    }
    
    class _FollowingUserScreenState : State<FollowingUserScreen> {
        readonly TextEditingController _controller = new TextEditingController("");
        int _userOffset;
        int _pageNumber;
        RefreshController _refreshController;
        FocusNode _focusNode;
        string _title;
        
        public override void initState() {
            base.initState();
            this._userOffset = 0;
            this._pageNumber = 0;
            this._refreshController = new RefreshController();
            this._focusNode = new FocusNode();
            this._title = this.widget.viewModel.currentUserId == this.widget.viewModel.personalId 
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
            this.widget.actionModel.searchFollowing(text, 0);
        }
        
        void _onRefreshFollowing(bool up) {
            this._userOffset = up ? 0 : this.widget.viewModel.userOffset;
            this.widget.actionModel.fetchFollowing(this._userOffset)
                .Then(() => this._refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up, RefreshStatus.failed));
        }
        
        void _onRefreshSearchFollowing(bool up) {
            if (up) {
                this._pageNumber = 0;
            }
            else {
                this._pageNumber++;
            }
            this.widget.actionModel.searchFollowing(this.widget.viewModel.searchFollowingKeyword, this._pageNumber)
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
            Widget content = new Container();
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
                                        var searchUser = this.widget.viewModel.searchFollowings[index];
                                        return new UserCard(
                                            user: searchUser,
                                            () => this.widget.actionModel.pushToPersonalDetail(searchUser.id),
                                            key: new ObjectKey(searchUser.id)
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
                                var user = this.widget.viewModel.followings[index];
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
                                    () => this.widget.actionModel.pushToPersonalDetail(user.id),
                                    userType: userType,
                                    () => this._onFollow(userType: userType, userId: user.id),
                                    new ObjectKey(user.id)
                                );
                            }
                        )
                    )
                );
            }
            return new Container(
                color: CColors.Background,
                child: new CustomSafeArea(
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
                            Icons.search,
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