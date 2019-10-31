using System.Collections.Generic;
using System.Linq;
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
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ChannelMembersScreenConnector : StatelessWidget {
        public ChannelMembersScreenConnector(
            string channelId,
            Key key = null
        ) : base(key: key) {
            this.channelId = channelId;
        }

        readonly string channelId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelMembersScreenViewModel>(
                converter: state => {
                    var channel = state.channelState.channelDict[key: this.channelId];
                    var members = channel.memberIds.Select(
                        memberId => channel.membersDict[key: memberId]
                    ).ToList();
                    Dictionary<string, bool> followDict = new Dictionary<string, bool>();
                    if (state.loginState.isLoggedIn) {
                        state.followState.followDict.TryGetValue(key: state.loginState.loginInfo.userId,
                            value: out followDict);
                    }

                    return new ChannelMembersScreenViewModel {
                        channel = channel,
                        followed = followDict,
                        userDict = state.userState.userDict,
                        members = members,
                        isLoggedIn = state.loginState.isLoggedIn,
                        currentUserId = state.loginState.loginInfo.userId ?? ""
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelMembersScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToUserDetail = userId => dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {
                            userId = userId
                        }),
                        fetchMembers = offset => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMembers(channelId: this.channelId, offset: offset)),
                        startFollowUser = followUserId => dispatcher.dispatch(new StartFollowUserAction {
                            followUserId = followUserId
                        }),
                        followUser = followUserId =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(followUserId: followUserId)),
                        startUnFollowUser = unFollowUserId => dispatcher.dispatch(new StartUnFollowUserAction {
                            unFollowUserId = unFollowUserId
                        }),
                        unFollowUser = unFollowUserId =>
                            dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(unFollowUserId: unFollowUserId))
                    };
                    return new ChannelMembersScreen(actionModel: actionModel, viewModel: viewModel);
                }
            );
        }
    }

    public class ChannelMembersScreen : StatefulWidget {
        public ChannelMembersScreen(
            ChannelMembersScreenActionModel actionModel = null,
            ChannelMembersScreenViewModel viewModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ChannelMembersScreenActionModel actionModel;
        public readonly ChannelMembersScreenViewModel viewModel;

        public override State createState() {
            return new _ChannelMembersScreenState();
        }
    }

    class _ChannelMembersScreenState : State<ChannelMembersScreen>, RouteAware {
        RefreshController _refreshController;
        int _memberOffset;

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
            this._memberOffset = 0;
            SchedulerBinding.instance.addPostFrameCallback(_ => this.widget.actionModel.fetchMembers(0));
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
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
                                    child: this._buildContent()
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
                new Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: new List<Widget> {
                        new Text(
                            "群聊成员",
                            style: CTextStyle.PXLargeMedium
                        ),
                        new SizedBox(height: 5),
                        new Text(
                            "按活跃度排序",
                            style: new TextStyle(
                                fontSize: 10,
                                fontFamily: "Roboto-Regular",
                                color: CColors.Black
                            )
                        )
                    }
                )
            );
        }

        Widget _buildContent() {
            var enablePullUp = this.widget.viewModel.members.Count < this.widget.viewModel.channel.memberCount;
            return new Container(
                color: CColors.Background,
                child: new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: false,
                    enablePullUp: enablePullUp,
                    onRefresh: this._onRefresh,
                    itemCount: this.widget.viewModel.members.Count,
                    itemBuilder: this._buildMemberItem,
                    footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                )
            );
        }

        Widget _buildMemberItem(BuildContext context, int index) {
            ChannelMember member = this.widget.viewModel.members[index: index];

            var userDict = this.widget.viewModel.userDict;
            if (!userDict.ContainsKey(key: member.user.id)) {
                return new Container();
            }

            var user = userDict[key: member.user.id];
            UserType userType;
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
                else if (this.widget.viewModel.followed != null
                         && this.widget.viewModel.followed.ContainsKey(key: user.id)) {
                    userType = UserType.follow;
                }
                else {
                    userType = UserType.unFollow;
                }
            }

            return new MemberCard(
                member: member,
                () => this.widget.actionModel.pushToUserDetail(obj: user.id),
                userType: userType,
                () => this._onFollow(userType: userType, userId: user.id)
            );
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

        void _onRefresh(bool up) {
            this._memberOffset = up
                ? 0
                : this.widget.viewModel.members.Count;

            this.widget.actionModel.fetchMembers(arg: this._memberOffset)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(e => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        public void didPop() {
        }

        public void didPopNext() {
            StatusBarManager.statusBarStyle(false);
        }

        public void didPush() {
        }

        public void didPushNext() {
        }
    }
}