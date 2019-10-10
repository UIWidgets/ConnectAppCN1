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
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ChannelMembersScreenConnector : StatelessWidget {
        public ChannelMembersScreenConnector(
            string channelId,
            Key key = null
        ) : base(key : key) {
            this.channelId = channelId;
        }

        readonly string channelId;

        static int _compareMember(ChannelMember m1, ChannelMember m2) {
            if (m1.role == m2.role) return 0;
            if (m1.role == "admin") return 1;
            if (m2.role == "admin") return -1;
            if (m1.role == "moderator") return -1;
            if (m2.role == "moderator") return 1;
            return 0;
        }

        public override Widget build(BuildContext context) {
            Dictionary<string, bool> followDict = new Dictionary<string, bool>();
            return new StoreConnector<AppState, ChannelMembersScreenViewModel>(
                converter: state => {
                    var members = state.channelState.channelDict[key: this.channelId].memberIds.Select(
                        memberId => state.channelState.membersDict[key: memberId]
                    ).ToList();
                    List<ChannelMember> specialMembers = members.Where(member => member.role != "member").ToList();
                    List<ChannelMember> normalMembers = members.Where(member => member.role == "member").ToList();
                    specialMembers.Sort(comparison: _compareMember);
                    if (state.loginState.isLoggedIn) {
                        state.followState.followDict.TryGetValue(key: state.loginState.loginInfo.userId,
                            value: out followDict);
                    }

                    return new ChannelMembersScreenViewModel {
                        channel = state.channelState.channelDict[key: this.channelId],
                        followed = followDict,
                        userDict = state.userState.userDict,
                        normalMembers = normalMembers,
                        specialMembers = specialMembers,
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
                        pushToUserDetail = userId => dispatcher.dispatch(
                            new MainNavigatorPushToUserDetailAction {
                                userId = userId
                            }
                        ),
                        fetchMembers = () => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMembers(channelId: this.channelId,
                                viewModel.normalMembers.Count + viewModel.specialMembers.Count)),
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

    class _ChannelMembersScreenState : State<ChannelMembersScreen> {
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
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
                new Text(
                    $"群聊成员({this.widget.viewModel.channel.memberCount})",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent() {
            var enablePullUp = this.widget.viewModel.normalMembers.Count + this.widget.viewModel.specialMembers.Count
                               < this.widget.viewModel.channel.memberCount;
            return new Container(
                color: CColors.Background,
                child: new SectionView(
                    controller: this._refreshController,
                    enablePullDown: false,
                    enablePullUp: enablePullUp,
                    onRefresh: this._onRefresh,
                    sectionCount: 2,
                    numOfRowInSection: section => section == 0
                            ? this.widget.viewModel.specialMembers.Count : this.widget.viewModel.normalMembers.Count,
                    headerInSection: section => section == 0 ? null : new Container(height: 16),
                    cellAtIndexPath: this._buildMemberItem,
                    footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                )
            );
        }

        Widget _buildMemberItem(BuildContext context, int section, int row) {
            ChannelMember member = section == 0
                ? this.widget.viewModel.specialMembers[index: row]
                : this.widget.viewModel.normalMembers[index: row];

            var userDict = this.widget.viewModel.userDict;
            if (!userDict.ContainsKey(key: member.user.id)) {
                return new Container();
            }

            var user = userDict[key: member.user.id];
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
                else if (this.widget.viewModel.followed != null
                         &&this.widget.viewModel.followed.ContainsKey(key: user.id)) {
                    userType = UserType.follow;
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
            if (!up) {
                this.widget.actionModel.fetchMembers().Then(
                    () => this._refreshController.sendBack(false, mode: RefreshStatus.idle)
                ).Catch(
                    e => this._refreshController.sendBack(false, mode: RefreshStatus.idle)
                );
            }
        }
    }
}