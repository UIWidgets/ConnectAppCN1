using System.Collections.Generic;
using System.Linq;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class DiscoverChannelsScreenConnector : StatelessWidget {
        public DiscoverChannelsScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, DiscoverChannelsScreenViewModel>(
                converter: state => {
                    var joinedChannels = state.channelState.joinedChannels.Select(
                        channelId => {
                            var channel = state.channelState.channelDict[key: channelId];
                            channel.isTop = state.channelState.channelTop.TryGetValue(channelId, out var isTop) &&
                                            isTop;
                            return channel;
                        }).ToList();
                    joinedChannels.Sort((c1, c2) => c1.isTop == c2.isTop ? 0 : (c1.isTop ? -1 : 1));

                    var lastMessageMap = new Dictionary<string, string>();
                    foreach (var channel in joinedChannels) {
                        if (!string.IsNullOrEmpty(value: channel.lastMessageId)) {
                            lastMessageMap[key: channel.id] = channel.lastMessageId;
                        }
                    }
                    return new DiscoverChannelsScreenViewModel {
                        publicChannels = state.channelState.publicChannels.Select(
                            channelId => state.channelState.channelDict[key: channelId]
                        ).ToList(),
                        lastMessageMap = lastMessageMap,
                        page = state.channelState.discoverPage,
                        hasMore = state.channelState.discoverHasMore
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new DiscoverChannelsScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToChannel = channelId => {
                            dispatcher.dispatch(new MainNavigatorPushToChannelAction {
                                channelId = channelId
                            });
                            if (viewModel.lastMessageMap.TryGetValue(key: channelId, out var messageId)) {
                                dispatcher.dispatch(Actions.ackChannelMessage(messageId: messageId));
                            }
                        },
                        pushToChannelDetail = channelId => dispatcher.dispatch(new MainNavigatorPushToChannelDetailAction {
                            channelId = channelId
                        }),
                        startJoinChannel = channelId => dispatcher.dispatch(new StartJoinChannelAction {
                            channelId = channelId
                        }),
                        joinChannel = (channelId, groupId) => dispatcher.dispatch<IPromise>(
                            Actions.joinChannel(channelId: channelId, groupId: groupId)),
                        fetchChannels = pageNumber => {
                            return dispatcher.dispatch<IPromise>(Actions.fetchChannels(page: pageNumber, joined: false));
                        }
                    };
                    return new DiscoverChannelsScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class DiscoverChannelsScreen : StatefulWidget {
        public DiscoverChannelsScreen(
            DiscoverChannelsScreenViewModel viewModel = null,
            DiscoverChannelsScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly DiscoverChannelsScreenViewModel viewModel;
        public readonly DiscoverChannelsScreenActionModel actionModel;

        public override State createState() {
            return new _DiscoverChannelsScreenState();
        }
    }

    class _DiscoverChannelsScreenState : State<DiscoverChannelsScreen> {
        RefreshController _refreshController;
        int _pageNumber;

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
        }

        void _onRefresh(bool up) {
            this.widget.actionModel.fetchChannels(arg: up ? 1 : this.widget.viewModel.page)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(e => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            Widget content;
            var publicChannels = this.widget.viewModel.publicChannels;

            if (publicChannels.Count == 0) {
                content = new BlankView(
                    "哎呀，暂无发现群聊",
                    "image/default-notification"
                );
            }
            else {
                content = this._buildContent();
            }
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Flexible(
                                    child: content
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
                    "发现群聊",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent() {
            return new Container(
                color: CColors.Background,
                child: new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: this.widget.viewModel.hasMore,
                    onRefresh: this._onRefresh,
                    itemCount: this.widget.viewModel.publicChannels.Count,
                    itemBuilder: (cxt, index) => {
                        var channel = this.widget.viewModel.publicChannels[index: index];
                        return new DiscoverChannelCard(
                            channel: channel,
                            () => {
                                if (channel.joined) {
                                    this.widget.actionModel.pushToChannel(obj: channel.id);
                                }
                                else {
                                    this.widget.actionModel.pushToChannelDetail(obj: channel.id);
                                }
                            },
                            () => {
                                this.widget.actionModel.startJoinChannel(obj: channel.id);
                                this.widget.actionModel.joinChannel(arg1: channel.id, arg2: channel.groupId);
                            }
                        );
                    },
                    footerWidget: this.widget.viewModel.hasMore ? null : CustomListViewConstant.defaultFooterWidget
                )
            );
        }
    }
}