using System.Collections.Generic;
using System.Linq;
using ConnectApp.Components;
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
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ChannelDetailScreenConnector : StatelessWidget {
        public ChannelDetailScreenConnector(
            string channelId,
            Key key = null
        ) : base(key : key) {
            this.channelId = channelId;
        }

        readonly string channelId;
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelDetailScreenViewModel>(
                converter: state => {
                    ChannelView channel = state.channelState.channelDict[key: this.channelId];
                    channel.isTop = state.channelState.channelTop.TryGetValue(key: this.channelId, out var isTop) && isTop;
                    return new ChannelDetailScreenViewModel {
                        channel = channel,
                        members = state.channelState.channelDict[key: this.channelId].memberIds.Select(
                            memberId => channel.membersDict[key: memberId]
                        ).ToList()
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToUserDetail = userId => dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {
                            userId = userId
                        }),
                        pushToChannelMembers = () => dispatcher.dispatch(new MainNavigatorPushToChannelMembersAction {
                            channelId = this.channelId
                        }),
                        pushToChannelIntroduction = () =>
                            dispatcher.dispatch(new MainNavigatorPushToChannelIntroductionAction {
                                channelId = this.channelId
                            }
                        ),
                        fetchMembers = () => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMembers(channelId: this.channelId)),
                        joinChannel = () => dispatcher.dispatch<IPromise>(
                            Actions.joinChannel(channelId: this.channelId, groupId: viewModel.channel.groupId, true)),
                        leaveChannel = () => dispatcher.dispatch<IPromise>(
                            Actions.leaveChannel(channelId: this.channelId, groupId: viewModel.channel.groupId)),
                        updateTop = isTop => dispatcher.dispatch<IPromise>(isTop
                            ? Actions.fetchStickChannel(channelId: this.channelId)
                            : Actions.fetchUnStickChannel(channelId: this.channelId))
                    };
                    return new ChannelDetailScreen(actionModel: actionModel, viewModel: viewModel);
                }
            );
        }
    }

    public class ChannelDetailScreen : StatefulWidget {
        public ChannelDetailScreen(
            ChannelDetailScreenActionModel actionModel = null,
            ChannelDetailScreenViewModel viewModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ChannelDetailScreenActionModel actionModel;
        public readonly ChannelDetailScreenViewModel viewModel;

        public override State createState() {
            return new _ChannelDetailScreenState();
        }
    }
    
    class _ChannelDetailScreenState : State<ChannelDetailScreen>, RouteAware {
        const int _avatarNumber = 5;

        public override void initState() {
            base.initState();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (!this.widget.viewModel.channel.joined) {
                    this.widget.actionModel.fetchMembers();
                }
            });
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        float _getAvatarSize() {
            return (MediaQuery.of(context: this.context).size.width - 16 * 2 - 16 * (_avatarNumber - 1)) / _avatarNumber;
        }

        void _leaveChannel() {
            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                title: "确定退出当前群聊吗？",
                items: new List<ActionSheetItem> {
                    new ActionSheetItem(
                        "退出",
                        type: ActionType.destructive,
                        () => this.widget.actionModel.leaveChannel()
                    ),
                    new ActionSheetItem("取消", type: ActionType.cancel)
                }
            ));
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
                    "群聊资料",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildChannelIntroduction() {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, right: 16, top: 16),
                height: 64,
                child: new Row(
                    children: new List<Widget> {
                        new PlaceholderImage(
                            this.widget.viewModel.channel?.thumbnail ?? "",
                            48,
                            48,
                            4,
                            fit: BoxFit.cover
                        ),
                        new Expanded(
                            child: new Container(
                                padding: EdgeInsets.only(16),
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Text(
                                            this.widget.viewModel.channel?.name ?? "",
                                            style: CTextStyle.PLargeMedium,
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis
                                        ),
                                        new Text(
                                            $"{this.widget.viewModel.channel?.memberCount ?? 0}名群成员",
                                            style: CTextStyle.PSmallBody4
                                        )
                                    }
                                )
                            )
                        )
                    }
                )
            );
        }

        Widget _buildChannelMember() {
            List<Widget> avatars = new List<Widget>();
            for (int i = 0; i < _avatarNumber; i++) {
                avatars.Add(this.widget.viewModel.members.Count > i
                    ? this._buildAvatar(index: i)
                    : new Container(width: 56, height: 56)
                );
            }
            return new Column(
                children: new List<Widget> {
                    new Container(height: 16),
                    new Container(
                        color: CColors.White,
                        padding: EdgeInsets.only(16, 16, 8),
                        child: new Row(
                            children: new List<Widget> {
                                new Text("群聊成员", style: CTextStyle.PLargeBody),
                                new Expanded(child: new Container()),
                                new GestureDetector(
                                    onTap: () => this.widget.actionModel.pushToChannelMembers(),
                                    child: new Container(
                                        color: CColors.Transparent,
                                        child: new Row(
                                            children: new List<Widget> {
                                                new Text(
                                                    $"查看{this.widget.viewModel.channel?.memberCount ?? 0}名群成员",
                                                    style: new TextStyle(
                                                        fontSize: 14,
                                                        fontFamily: "Roboto-Regular",
                                                        color: CColors.TextBody4
                                                    )
                                                ),
                                                new Icon(
                                                    icon: Icons.chevron_right,
                                                    size: 20,
                                                    color: Color.fromRGBO(199, 203, 207, 1)
                                                )
                                            }
                                        )
                                    )
                                )
                            }
                        )
                    ),
                    new Container(
                        color: CColors.White,
                        padding: EdgeInsets.all(16),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: avatars
                        )
                    )
                }
            );
        }

        Widget _buildChannelSetting() {
            var channel = this.widget.viewModel.channel;
            if (!channel.joined) {
                return new Column(
                    children: new List<Widget> {
                        new Container(height: 16),
                        new GestureDetector(
                            onTap: () => this.widget.actionModel.joinChannel(),
                            child: new Container(
                                color: CColors.White,
                                height: 60,
                                child: new Center(
                                    child: new Text("加入群聊", style: CTextStyle.PLargeBlue)
                                )
                            )
                        )
                    }
                );
            }

            Widget leaveContainer;
            if (!channel.currentMember.role.Equals("owner")) {
                leaveContainer = new GestureDetector(
                    onTap: this._leaveChannel,
                    child: new Container(
                        color: CColors.White,
                        height: 60,
                        child: new Center(
                            child: new Text("退出群聊", style: CTextStyle.PLargeError)
                        )
                    )
                );
            }
            else {
                leaveContainer = new Container();
            }

            return new Column(
                children: new List<Widget> {
                    new Container(height: 16),
                    _switchRow(
                        "设为置顶",
                        this.widget.viewModel.channel?.isTop ?? false,
                        value => this.widget.actionModel.updateTop(obj: value)
                    ),
#if false
                    _switchRow(
                        "消息免打扰",
                        this.widget.viewModel.channel?.isMute ?? false,
                        value => { }
                    ),
#endif
                    new Container(height: 16),
                    leaveContainer
                }
            );
        }

        Widget _buildAvatar(int index) {
            var user = this.widget.viewModel.members[index: index].user;
            return new GestureDetector(
                onTap: () => this.widget.actionModel.pushToUserDetail(obj: user.id),
                child: Avatar.User(user: user, this._getAvatarSize())
            );
        }

        Widget _buildContent() {
            return new Container(
                color: CColors.Background,
                child: new ListView(
                    children: new List<Widget> {
                        this._buildChannelIntroduction(),
                        new GestureDetector(
                            onTap: () => this.widget.actionModel.pushToChannelIntroduction(),
                            child: new Container(
                                color: CColors.White,
                                padding: EdgeInsets.only(16, 16, 16, 21),
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    children: new List<Widget> {
                                        new Expanded(
                                            child: new Text(
                                                this.widget.viewModel.channel?.topic ?? "",
                                                style: CTextStyle.PRegularBody2,
                                                overflow: TextOverflow.ellipsis,
                                                maxLines: 2
                                            )
                                        ),
                                        new Icon(
                                            icon: Icons.arrow_forward,
                                            size: 16,
                                            color: CColors.Icon
                                        )
                                    }
                                )
                            )
                        ),
                        this._buildChannelMember(),
                        this._buildChannelSetting()
                    }
                )
            );
        }

        static Widget _switchRow(string content, bool value, ValueChanged<bool> onChanged) {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.symmetric(16, 18),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new Expanded(
                            child: new Text(
                                data: content,
                                style: CTextStyle.PLargeBody,
                                overflow: TextOverflow.ellipsis
                            )
                        ),
                        new CustomSwitch(value: value, onChanged: onChanged, activeColor: CColors.PrimaryBlue)
                    }
                )
            );
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