using System;
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
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class MessengerScreenConnector : StatelessWidget {
        public MessengerScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MessengerScreenViewModel>(
                converter: state => {
                    var joinedChannels = state.channelState.joinedChannels.Select(
                        channelId => {
                            ChannelView channel = state.channelState.channelDict[key: channelId];
                            channel.isTop = state.channelState.channelTop.TryGetValue(key: channelId, out var isTop) &&
                                            isTop;
                            return channel;
                        }).ToList();
                    if (joinedChannels.isNotNullAndEmpty() && joinedChannels.Count > 1) {
                        joinedChannels.Sort(
                            (c1, c2) => {
                                if (c1 == null) {
                                    return -1;
                                }

                                if (c2 == null) {
                                    return 1;
                                }

                                if (c1.isTop && !c2.isTop) {
                                    return -1;
                                }

                                if (!c1.isTop && c2.isTop) {
                                    return 1;
                                }

                                return DateTime.Compare(t1: c2.lastMessage.time, t2: c1.lastMessage.time);
                            });
                    }

                    var lastMessageMap = new Dictionary<string, string>();
                    foreach (var channel in joinedChannels) {
                        if (channel.lastMessageId.isNotEmpty()) {
                            lastMessageMap[key: channel.id] = channel.lastMessageId;
                        }
                    }

                    return new MessengerScreenViewModel {
                        channelLoading = state.channelState.channelLoading,
                        myUserId = state.loginState.loginInfo.userId,
                        joinedChannels = joinedChannels,
                        lastMessageMap = lastMessageMap,
                        page = state.channelState.discoverPage,
                        hasMore = state.channelState.discoverHasMore,
                        popularChannels = state.channelState.publicChannels
                            .Select(channelId => state.channelState.channelDict[key: channelId])
                            .Take(state.channelState.publicChannels.Count > 0
                                ? 8
                                : state.channelState.publicChannels.Count)
                            .ToList(),
                        publicChannels = state.channelState.publicChannels
                            .Select(channelId => state.channelState.channelDict[key: channelId])
                            .Take(joinedChannels.Count > 0
                                ? 8
                                : state.channelState.publicChannels.Count)
                            .ToList(),
                        currentTabBarIndex = state.tabBarState.currentTabIndex,
                        socketConnected = state.channelState.socketConnected,
                        networkConnected = state.networkState.networkConnected,
                        dismissNoNetworkBanner = state.networkState.dismissNoNetworkBanner
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MessengerScreenActionModel {
                        pushToDiscoverChannels = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.DiscoverChannel
                        }),
                        pushToChannel = channelId => {
                            dispatcher.dispatch(new MainNavigatorPushToChannelAction {
                                channelId = channelId
                            });
                            if (viewModel.lastMessageMap.TryGetValue(key: channelId, out var messageId)) {
                                dispatcher.dispatch(Actions.ackChannelMessage(messageId: messageId));
                            }
                        },
                        pushToChannelDetail = channelId => dispatcher.dispatch(
                            new MainNavigatorPushToChannelDetailAction {
                                channelId = channelId
                            }),
                        fetchChannels = (pageNumber, joined) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchChannels(page: pageNumber, joined: joined)),
                        fetchCreateChannelFilterIds = () =>
                            dispatcher.dispatch<IPromise>(Actions.fetchCreateChannelFilter()),
                        startJoinChannel = channelId => dispatcher.dispatch(new StartJoinChannelAction {
                            channelId = channelId
                        }),
                        joinChannel = (channelId, groupId) =>
                            dispatcher.dispatch<IPromise>(Actions.joinChannel(channelId: channelId, groupId: groupId))
                    };
                    return new MessengerScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class MessengerScreen : StatefulWidget {
        public MessengerScreen(
            MessengerScreenViewModel viewModel = null,
            MessengerScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly MessengerScreenViewModel viewModel;
        public readonly MessengerScreenActionModel actionModel;

        public override State createState() {
            return new _MessageScreenState();
        }
    }

    public class _MessageScreenState : AutomaticKeepAliveClientMixin<MessengerScreen>, RouteAware {
        RefreshController _refreshController;
        string _newNotificationSubId;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        bool _hasJoinedChannel() {
            return this.widget.viewModel.joinedChannels.isNotEmpty();
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            Widget content;
            if (this.widget.viewModel.publicChannels.isNullOrEmpty() && this.widget.viewModel.channelLoading) {
                content = new Container(
                    child: new GlobalLoading(),
                    padding: EdgeInsets.only(bottom: CConstant.TabBarHeight +
                                                     CCommonUtils.getSafeAreaBottomPadding(context: context))
                );
            }
            else {
                content = new NotificationListener<ScrollNotification>(
                    child: new Container(
                        color: CColors.Background,
                        child: new SectionView(
                            controller: this._refreshController,
                            enablePullDown: true,
                            enablePullUp: this.widget.viewModel.hasMore,
                            onRefresh: this._onRefresh,
                            hasBottomMargin: true,
                            sectionCount: 2,
                            numOfRowInSection: section => {
                                if (section == 0) {
                                    return !this._hasJoinedChannel()
                                        ? 1
                                        : this.widget.viewModel.joinedChannels.Count;
                                }

                                return this.widget.viewModel.publicChannels.Count;
                            },
                            headerInSection: this._headerInSection,
                            cellAtIndexPath: this._buildMessageItem,
                            footerWidget: !this.widget.viewModel.hasMore
                                ? new EndView(hasBottomMargin: true)
                                : null
                        )
                    )
                );
            }

            return new Container(
                padding: EdgeInsets.only(top: CCommonUtils.getSafeAreaTopPadding(context: context)),
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        this._buildNavigationBar(),
                        !this.widget.viewModel.dismissNoNetworkBanner
                            ? this._buildNetworkDisconnectedNote()
                            : new Container(color: CColors.Separator2, height: 1),
                        new Flexible(child: content)
                    }
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomNavigationBar(
                !this.widget.viewModel.networkConnected
                    ? new Text("群聊 (未连接)", style: CTextStyle.H2)
                    : this.widget.viewModel.socketConnected
                        ? new Text("群聊", style: CTextStyle.H2)
                        : new Text("收取中...", style: CTextStyle.H2Body4),
                padding: EdgeInsets.only(16, bottom: 8)
            );
        }

        Widget _buildNetworkDisconnectedNote() {
            return new Container(
                height: 48,
                color: CColors.Error.withAlpha((int) (255 * 0.16)),
                child: new Center(
                    child: new Text(
                        "网络连接不可用，请检查你的网络设置",
                        style: CTextStyle.PRegularError.copyWith(height: 1f)
                    )
                )
            );
        }

        Widget _headerInSection(int section) {
            if (section == 0) {
                return null;
            }

            if (this.widget.viewModel.publicChannels.isEmpty()) {
                return null;
            }

            Widget rightWidget;
            if (!this._hasJoinedChannel()) {
                rightWidget = new Container();
            }
            else {
                rightWidget = new GestureDetector(
                    onTap: () => this.widget.actionModel.pushToDiscoverChannels(),
                    child: new Container(
                        color: CColors.Transparent,
                        child: new Row(
                            children: new List<Widget> {
                                new Padding(
                                    padding: EdgeInsets.only(top: 2),
                                    child: new Text(
                                        "查看全部",
                                        style: new TextStyle(
                                            fontSize: 12,
                                            fontFamily: "Roboto-Regular",
                                            color: CColors.TextBody4
                                        )
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
                );
            }

            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        !this._hasJoinedChannel()
                            ? new Container(height: 24, color: CColors.White)
                            : new Container(height: 16),
                        new Container(
                            color: CColors.White,
                            padding: !this._hasJoinedChannel()
                                ? EdgeInsets.all(16)
                                : EdgeInsets.only(16, 16, 8, 16),
                            child: new Row(
                                children: new List<Widget> {
                                    new Text("发现群聊", style: CTextStyle.H5),
                                    new Expanded(
                                        child: new Container()
                                    ),
                                    rightWidget
                                }
                            )
                        )
                    }
                )
            );
        }

        Widget _buildMessageItem(BuildContext context, int section, int row) {
            var joinedChannels = this.widget.viewModel.joinedChannels;
            if (section == 0) {
                if (!this._hasJoinedChannel()) {
                    return new Container(
                        color: CColors.White,
                        child: new Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                new Container(
                                    padding: EdgeInsets.only(16, right: 16, top: 20),
                                    child: new Text("热门群聊", style: CTextStyle.H5)
                                ),
                                new Container(
                                    padding: EdgeInsets.only(top: 16),
                                    color: CColors.White,
                                    child: new SingleChildScrollView(
                                        scrollDirection: Axis.horizontal,
                                        child: new Container(
                                            padding: EdgeInsets.only(16),
                                            child: new Row(
                                                children: this.widget.viewModel.popularChannels.Select(
                                                    popularChannel => (Widget) new PopularChannelCard(
                                                        channel: popularChannel,
                                                        () => this.widget.actionModel.pushToChannelDetail(
                                                            obj: popularChannel.id)
                                                    )
                                                ).ToList()
                                            )
                                        )
                                    )
                                )
                            }
                        )
                    );
                }

                var joinedChannel = joinedChannels[index: row];
                return new JoinedChannelCard(
                    channel: joinedChannel,
                    myUserId: this.widget.viewModel.myUserId,
                    () => this.widget.actionModel.pushToChannel(obj: joinedChannel.id)
                );
            }

            var publicChannel = this.widget.viewModel.publicChannels[index: row];
            return new DiscoverChannelCard(
                channel: publicChannel,
                () => {
                    if (publicChannel.joined) {
                        this.widget.actionModel.pushToChannel(obj: publicChannel.id);
                    }
                    else {
                        this.widget.actionModel.pushToChannelDetail(obj: publicChannel.id);
                    }
                },
                () => {
                    this.widget.actionModel.startJoinChannel(obj: publicChannel.id);
                    this.widget.actionModel.joinChannel(arg1: publicChannel.id, arg2: publicChannel.groupId);
                }
            );
        }

        public void didPopNext() {
            if (this.widget.viewModel.currentTabBarIndex == 2) {
                StatusBarManager.statusBarStyle(false);
            }
        }

        public void didPush() {
        }

        public void didPop() {
        }

        public void didPushNext() {
        }

        void _onRefresh(bool up) {
            this.widget.actionModel.fetchChannels(up ? 1 : this.widget.viewModel.page, up)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(e => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
            if (up) {
                this.widget.actionModel.fetchCreateChannelFilterIds();
            }
        }
    }
}