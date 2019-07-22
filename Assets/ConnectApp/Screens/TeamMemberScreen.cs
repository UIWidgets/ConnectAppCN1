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
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class TeamMemberScreenConnector : StatelessWidget {
        public TeamMemberScreenConnector(
            string teamId,
            Key key = null
        ) : base(key: key) {
            this.teamId = teamId;
        }

        readonly string teamId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, TeamMemberScreenViewModel>(
                converter: state => {
                    var team = state.teamState.teamDict.ContainsKey(key: this.teamId)
                        ? state.teamState.teamDict[key: this.teamId]
                        : new Team();
                    var members = team.members ?? new List<Member>();
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followMap = state.followState.followDict.ContainsKey(key: currentUserId)
                        ? state.followState.followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new TeamMemberScreenViewModel {
                        teamId = this.teamId,
                        memberLoading = state.teamState.memberLoading,
                        members = members,
                        membersHasMore = team.membersHasMore ?? false,
                        userDict = state.userState.userDict,
                        followMap = followMap,
                        currentUserId = currentUserId,
                        isLoggedIn = state.loginState.isLoggedIn
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new TeamMemberScreenActionModel {
                        startFetchMember = () => dispatcher.dispatch(new StartFetchTeamMemberAction()),
                        fetchMember = pageNumber =>
                            dispatcher.dispatch<IPromise>(Actions.fetchTeamMember(this.teamId, pageNumber)),
                        startFollowUser = followUserId => dispatcher.dispatch(new StartFetchFollowUserAction {
                            followUserId = followUserId
                        }),
                        followUser = followUserId =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(followUserId)),
                        startUnFollowUser = unFollowUserId => dispatcher.dispatch(new StartFetchUnFollowUserAction {
                            unFollowUserId = unFollowUserId
                        }),
                        unFollowUser = unFollowUserId =>
                            dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(unFollowUserId)),
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
                    return new TeamMemberScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class TeamMemberScreen : StatefulWidget {
        public TeamMemberScreen(
            TeamMemberScreenViewModel viewModel = null,
            TeamMemberScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly TeamMemberScreenViewModel viewModel;
        public readonly TeamMemberScreenActionModel actionModel;

        public override State createState() {
            return new _TeamMemberScreenState();
        }
    }

    class _TeamMemberScreenState : State<TeamMemberScreen> {
        int _pageNumber;
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._pageNumber = 1;
            this._refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchMember();
                this.widget.actionModel.fetchMember(1);
            });
        }

        void _onRefresh(bool up) {
            if (up) {
                this._pageNumber = 1;
            }
            else {
                this._pageNumber++;
            }

            this.widget.actionModel.fetchMember(arg: this._pageNumber)
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
            var members = this.widget.viewModel.members;
            Widget content;
            if (this.widget.viewModel.memberLoading && members.isEmpty()) {
                content = new GlobalLoading();
            }
            else if (members.Count <= 0) {
                content = new BlankView(
                    "暂无更多公司成员",
                    "image/default-following"
                );
            }
            else {
                content = new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: true,
                        enablePullUp: this.widget.viewModel.membersHasMore,
                        onRefresh: this._onRefresh,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: members.Count,
                            itemBuilder: this._buildMemberCard
                        )
                    )
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
                                this._buildNavigationBar(context: context),
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
                                "公司成员",
                                style: CTextStyle.H2
                            )
                        )
                    }
                )
            );
        }

        Widget _buildMemberCard(BuildContext context, int index) {
            var members = this.widget.viewModel.members[index: index];
            if (!this.widget.viewModel.userDict.ContainsKey(key: members.userId)) {
                return new Container();
            }

            var user = this.widget.viewModel.userDict[key: members.userId];
            UserType userType = UserType.unFollow;
            if (!this.widget.viewModel.isLoggedIn) {
                userType = UserType.unFollow;
            }
            else {
                if (this.widget.viewModel.currentUserId == user.id) {
                    userType = UserType.me;
                } else if (user.followUserLoading ?? false) {
                    userType = UserType.loading;
                }
                else if (this.widget.viewModel.followMap.ContainsKey(key: user.id)) {
                    userType = UserType.follow;
                }
            }

            return new UserCard(
                user: user,
                () => this.widget.actionModel.pushToUserDetail(obj: user.id),
                userType: userType,
                () => this._onFollow(userType: userType, userId: user.id),
                new ObjectKey(value: user.id)
            );
        }
    }
}