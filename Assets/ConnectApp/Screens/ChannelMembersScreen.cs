using System.Collections.Generic;
using System.Linq;
using ConnectApp.Components;
using ConnectApp.Constants;
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
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Avatar = ConnectApp.Components.Avatar;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.screens {
    public class ChannelMembersScreenConnector : StatelessWidget {
        
        public ChannelMembersScreenConnector(string channelId, Key key = null) : base(key : key) {
            this.channelId = channelId;
        }

        public readonly string channelId;
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelMembersScreenViewModel>(
                converter: state => {
                    var hasFollowDict =
                        state.followState.followDict.TryGetValue(state.loginState.loginInfo.userId, out var followDict);
                    var members = state.channelState.channelDict[this.channelId].memberIds.Select(
                        memberId => state.channelState.membersDict[memberId]
                    ).ToList();
                    members.Sort((m1, m2) => {
                        if (m1.role == m2.role) return 0;
                        if (m1.role == "member") return 1;
                        if (m2.role == "member") return -1;
                        if (m1.role == "admin") return 1;
                        if (m2.role == "admin") return -1;
                        if (m1.role == "moderator") return -1;
                        if (m2.role == "moderator") return 1;
                        return 0;
                    });
                    int nAdmin = 0;
                    for (int i = 0; i < members.Count; i++) {
                        if (members[i].role == "member") {
                            nAdmin = i - 1;
                            break;
                        }
                    }
                    return new ChannelMembersScreenViewModel {
                        channel = state.channelState.channelDict[this.channelId],
                        followed = hasFollowDict ? followDict : new Dictionary<string, bool>(),
                        members = members,
                        nAdmin = nAdmin
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelMembersScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        fetchMembers = () => dispatcher.dispatch<IPromise>(Actions.fetchChannelMembers(this.channelId))
                    };
                    return new ChannelMembersScreen(actionModel, viewModel);
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
        TextEditingController _controller;
        string _title;

        public override void initState() {
            base.initState();
            this._controller = new TextEditingController("");
            this.widget.actionModel.fetchMembers();
//            SchedulerBinding.instance.addPostFrameCallback(_ => {
//                if (this.widget.viewModel.searchFollowingKeyword.Length > 0
//                    || this.widget.viewModel.searchFollowingUsers.Count > 0) {
//                    this.widget.actionModel.clearSearchFollowingResult();
//                }
//            });
        }

        public override void dispose() {
            this._controller.dispose();
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        color: new Color(0xFFFAFAFA),
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
            return new Container(
                color: CColors.Background,
                child: ListView.builder(itemCount: this.widget.viewModel.members.Count + 1,
                    itemBuilder: (context, index) => {
                        if (index == this.widget.viewModel.nAdmin + 1) {
                            return new Container(height: 16);
                        }
                        ChannelMember member = null;
                        if (index <= this.widget.viewModel.nAdmin) {
                            member = this.widget.viewModel.members[index];
                        }
                        else {
                            member = this.widget.viewModel.members[index-1];
                        }

                        return buildMemberItem(context, member,
                            this.widget.viewModel.followed.TryGetValue(member.user.id, out bool followed) && followed);
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
                            // this.widget.actionModel.clearSearchFollowingResult();
                        }
                    }
                    // onSubmitted: this._searchFollowing
                )
            );
        }

        public static Widget buildMemberItem(BuildContext context, ChannelMember member, bool followed) {
            Widget fullName = new Text(member.user.fullName, style: CTextStyle.PMediumBody,
                maxLines: 1, overflow: TextOverflow.ellipsis);
            if (member.role != "member") {
                fullName = new Row(
                    children: new List<Widget> {
                        new Flexible(child: fullName),
                        new Container(
                            decoration: new BoxDecoration(
                                color: member.role != "admin" ? CColors.Tan : CColors.Portage,
                                borderRadius: BorderRadius.all(2)
                            ),
                            padding: EdgeInsets.symmetric(0, 4),
                            margin: EdgeInsets.only(4),
                            child: new Text(member.role == "admin" ? "管理员" : "群主",
                                style: CTextStyle.PSmallWhite.copyWith(height: 1.2f))
                        )
                    }
                );
            }
            return new Container(
                color: CColors.White,
                height: 72,
                padding: EdgeInsets.symmetric(12, 16),
                child: new Row(
                    children: new List<Widget> {
                        Avatar.User(member.user, 48),
                        new Expanded(
                            child: new Container(
                                padding: EdgeInsets.symmetric(0, 16),
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        fullName,
                                        new Expanded(
                                            child: new Text(member.user.title ?? "", style: CTextStyle.PRegularBody4,
                                                maxLines: 1, overflow: TextOverflow.ellipsis)
                                        )
                                    }
                                )
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.zero,
                            child: new Container(
                                width: 60,
                                height: 28,
                                decoration: new BoxDecoration(
                                    border: Border.all(color: followed
                                        ? CColors.Disable2
                                        : CColors.PrimaryBlue),
                                    borderRadius: BorderRadius.all(14)
                                ),
                                child: new Center(
                                    child: followed
                                        ? new Text(
                                            "已关注",
                                            style: CTextStyle.PRegularBody5.copyWith(height: 1)
                                        )
                                        : new Text(
                                            "关注",
                                            style: CTextStyle.PRegularBlue.copyWith(height: 1)
                                        )
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}