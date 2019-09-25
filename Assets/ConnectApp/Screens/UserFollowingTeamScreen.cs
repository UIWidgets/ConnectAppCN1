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
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class UserFollowingTeamScreenConnector : StatelessWidget {
        public UserFollowingTeamScreenConnector(
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
                    var followingTeams = user.followingTeams ?? new List<Team>();
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followMap = state.followState.followDict.ContainsKey(key: currentUserId)
                        ? state.followState.followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new UserFollowingScreenViewModel {
                        userId = this.userId,
                        followingTeamLoading = state.userState.followingTeamLoading,
                        followingTeams = followingTeams,
                        followingTeamsHasMore = user.followingTeamsHasMore ?? false,
                        followingTeamOffset = followingTeams.Count,
                        teamDict = state.teamState.teamDict,
                        followMap = followMap,
                        currentUserId = currentUserId,
                        isLoggedIn = state.loginState.isLoggedIn
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new UserFollowingScreenActionModel {
                        startFetchFollowingTeam = () => dispatcher.dispatch(new StartFetchFollowingTeamAction()),
                        fetchFollowingTeam = offset =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFollowingTeam(userId: this.userId, offset: offset)),
                        startFollowTeam = followTeamId => dispatcher.dispatch(new StartFetchFollowTeamAction {
                            followTeamId = followTeamId
                        }),
                        followTeam = followTeamId =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFollowTeam(followTeamId: followTeamId)),
                        startUnFollowTeam = unFollowTeamId => dispatcher.dispatch(new StartFetchUnFollowTeamAction {
                            unFollowTeamId = unFollowTeamId
                        }),
                        unFollowTeam = unFollowTeamId =>
                            dispatcher.dispatch<IPromise>(Actions.fetchUnFollowTeam(unFollowTeamId: unFollowTeamId)),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToTeamDetail = teamId => dispatcher.dispatch(
                            new MainNavigatorPushToTeamDetailAction {
                                teamId = teamId
                            }
                        )
                    };
                    return new UserFollowingTeamScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class UserFollowingTeamScreen : StatefulWidget {
        public UserFollowingTeamScreen(
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
            return new _UserFollowingTeamScreenState();
        }
    }

    class _UserFollowingTeamScreenState : State<UserFollowingTeamScreen> {
        int _offset;
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            this._offset = 0;
            this._refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchFollowingTeam();
                this.widget.actionModel.fetchFollowingTeam(0);
            });
        }

        void _onRefresh(bool up) {
            this._offset = up ? 0 : this.widget.viewModel.followingTeamOffset;
            this.widget.actionModel.fetchFollowingTeam(arg: this._offset)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        void _onFollow(UserType userType, string teamId) {
            if (this.widget.viewModel.isLoggedIn) {
                if (userType == UserType.follow) {
                    ActionSheetUtils.showModalActionSheet(
                        new ActionSheet(
                            title: "确定不再关注？",
                            items: new List<ActionSheetItem> {
                                new ActionSheetItem("确定", type: ActionType.normal,
                                    () => {
                                        this.widget.actionModel.startUnFollowTeam(obj: teamId);
                                        this.widget.actionModel.unFollowTeam(arg: teamId);
                                    }),
                                new ActionSheetItem("取消", type: ActionType.cancel)
                            }
                        )
                    );
                }

                if (userType == UserType.unFollow) {
                    this.widget.actionModel.startFollowTeam(obj: teamId);
                    this.widget.actionModel.followTeam(arg: teamId);
                }
            }
            else {
                this.widget.actionModel.pushToLogin();
            }
        }

        public override Widget build(BuildContext context) {
            Widget content;
            var followingTeams = this.widget.viewModel.followingTeams;
            if (this.widget.viewModel.followingTeamLoading && followingTeams.isEmpty()) {
                content = new GlobalLoading();
            }
            else if (followingTeams.Count <= 0) {
                content = new BlankView(
                    "没有关注的公司，去首页看看吧",
                    "image/default-following"
                );
            }
            else {
                var enablePullUp = this.widget.viewModel.followingTeamsHasMore;
                content = new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: enablePullUp,
                    onRefresh: this._onRefresh,
                    itemCount: followingTeams.Count,
                    itemBuilder: this._buildTeamCard,
                    headerWidget: CustomListViewConstant.defaultHeaderWidget,
                    footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                );
            }

            return new Container(
                color: CColors.Background,
                child: content
            );
        }

        Widget _buildTeamCard(BuildContext context, int index) {
            var followingTeams = this.widget.viewModel.followingTeams;

            var followingTeam = followingTeams[index: index];
            UserType userType = UserType.unFollow;
            if (!this.widget.viewModel.isLoggedIn) {
                userType = UserType.unFollow;
            }
            else {
                bool followTeamLoading;
                if (this.widget.viewModel.teamDict.ContainsKey(key: followingTeam.id)) {
                    var team = this.widget.viewModel.teamDict[key: followingTeam.id];
                    followTeamLoading = team.followTeamLoading ?? false;
                }
                else {
                    followTeamLoading = false;
                }

                if (this.widget.viewModel.currentUserId == followingTeam.id) {
                    userType = UserType.me;
                }
                else if (followTeamLoading) {
                    userType = UserType.loading;
                }
                else if (this.widget.viewModel.followMap.ContainsKey(key: followingTeam.id)) {
                    userType = UserType.follow;
                }
            }

            return new TeamCard(
                team: followingTeam,
                () => this.widget.actionModel.pushToTeamDetail(obj: followingTeam.id),
                userType: userType,
                () => this._onFollow(userType: userType, teamId: followingTeam.id),
                key: new ObjectKey(value: followingTeam.id)
            );
        }
    }
}