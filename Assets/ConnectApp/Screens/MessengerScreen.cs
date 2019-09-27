using System;
using System.Collections.Generic;
using System.Linq;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
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
                            ChannelView channel = state.channelState.channelDict[channelId];
                            channel.isTop = state.channelState.channelTop.TryGetValue(channelId, out var isTop) &&
                                            isTop;
                            return channel;
                        }).ToList();
                    joinedChannels.Sort((c1, c2) => { return c1.isTop == c2.isTop ? 0 : (c1.isTop ? -1 : 1); });
                    return new MessengerScreenViewModel {
                        joinedChannels = joinedChannels,
                        discoverPage = state.channelState.discoverPage,
                        popularChannels = state.channelState.publicChannels
                            .Select(channelId => state.channelState.channelDict[channelId])
                            .Take(state.channelState.publicChannels.Count > 0
                                ? 8
                                : state.channelState.publicChannels.Count)
                            .ToList(),
                        publicChannels = state.channelState.publicChannels
                            .Select(channelId => state.channelState.channelDict[channelId])
                            .Take(state.channelState.publicChannels.Count > 0
                                ? 8
                                : state.channelState.publicChannels.Count)
                            .ToList(),
                        currentTabBarIndex = state.tabBarState.currentTabIndex
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MessengerScreenActionModel {
                        pushToNotifications = () => {
                            dispatcher.dispatch(new MainNavigatorPushToNotificationAction());
                        },
                        pushToDiscoverChannels = () => {
                            dispatcher.dispatch(new MainNavigatorPushToDiscoverChannelsAction());
                        },
                        pushToChannel = channelId => {
                            dispatcher.dispatch(new MainNavigatorPushToChannelAction {
                                channelId = channelId
                            });
                        },
                        fetchChannels = () => dispatcher.dispatch<IPromise>(Actions.fetchChannels(
                            viewModel.discoverPage + 1
                        )),
                        joinChannel = (channelId, groupId) => {
                            dispatcher.dispatch<IPromise>(Actions.joinChannel(channelId, groupId));
                        }
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
        const int firstPageNumber = 1;
        int _pageNumber = firstPageNumber;
        RefreshController _refreshController;
        TextStyle titleStyle;
        float navBarHeight;
        string _loginSubId;
        string _refreshSubId;


        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            var enablePullUp = this.widget.viewModel.joinedChannels.isEmpty();
            return new Container(
                color: CColors.Background,
                child: new Column(
                    children: new List<Widget> {
                        this._buildNavigationBar(),
                        new Container(color: CColors.Separator2, height: 1),
                        new Flexible(
                            child: new NotificationListener<ScrollNotification>(
                                child: new SectionView(
                                    controller: this._refreshController,
                                    enablePullDown: false,
                                    enablePullUp: enablePullUp,
                                    onRefresh: this._onRefresh,
                                    sectionCount: 2,
                                    numOfRowInSection: section => {
                                        if (section == 0) {
                                            return this.widget.viewModel.joinedChannels.isEmpty()
                                                ? 1
                                                : this.widget.viewModel.joinedChannels.Count;
                                        }

                                        return this.widget.viewModel.publicChannels.Count;
                                    },
                                    headerInSection: this._headerInSection,
                                    cellAtIndexPath: this._buildMessageItem,
                                    footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                                )
                            )
                        )
                    }
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomNavigationBar(
                new Text("群聊", style: CTextStyle.H2),
                new List<Widget> {
                    new CustomButton(
                        onPressed: () => this.widget.actionModel.pushToNotifications(),
                        child: new Container(
                            width: 28,
                            height: 28,
                            child: new Icon(icon: Icons.outline_notification, color: CColors.Icon, size: 28)
                        )
                    )
                },
                backgroundColor: CColors.White,
                0
            );
        }

        Widget _headerInSection(int section) {
            if (section == 0) {
                return null;
            }

            Widget rightWidget;
            if (this.widget.viewModel.joinedChannels.isEmpty()) {
                rightWidget = new Container();
            }
            else {
                rightWidget = new GestureDetector(
                    onTap: () => this.widget.actionModel.pushToDiscoverChannels(),
                    child: new Container(
                        color: CColors.Transparent,
                        child: new Row(
                            children: new List<Widget> {
                                new Text(
                                    "查看全部",
                                    style: new TextStyle(
                                        fontSize: 12,
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
                );
            }

            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        this.widget.viewModel.joinedChannels.isEmpty()
                            ? new Container(height: 24, color: CColors.White)
                            : new Container(height: 16),
                        new Container(
                            color: CColors.White,
                            padding: this.widget.viewModel.joinedChannels.isEmpty()
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
                if (joinedChannels.isEmpty()) {
                    return new Container(
                        color: CColors.White,
                        child: new Column(
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
                                                    MessengerBuildUtils.buildPopularChannelItem
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
                return MessengerBuildUtils.buildChannelItem(
                    channel: joinedChannel,
                    () => this.widget.actionModel.pushToChannel(obj: joinedChannel.id)
                );
            }

            var publicChannel = this.widget.viewModel.publicChannels[index: row];
            return new ChannelCard(
                channel: publicChannel,
                () => this.widget.actionModel.joinChannel(arg1: publicChannel.id, arg2: publicChannel.groupId)
            );
        }

        public void didPopNext() {
            if (this.widget.viewModel.currentTabBarIndex == 0) {
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
            if (!up) {
                this.widget.actionModel.fetchChannels()
                    .Then(() => this._refreshController.sendBack(false, mode: RefreshStatus.idle))
                    .Catch(e => this._refreshController.sendBack(false, mode: RefreshStatus.idle));
            }
        }
    }

    public static class MessengerBuildUtils {
        public static Widget buildPopularChannelItem(ChannelView channel) {
            Widget image = Positioned.fill(
                child: CachedNetworkImageProvider.cachedNetworkImage(
                    channel?.thumbnail ?? "",
                    fit: BoxFit.cover
                )
            );
            Widget gradient = Positioned.fill(
                child: new Container(
                    decoration: new BoxDecoration(
                        gradient: new LinearGradient(
                            begin: Alignment.topCenter,
                            end: Alignment.bottomCenter,
                            colors: new List<Color> {
                                Color.fromARGB(20, 0, 0, 0),
                                Color.fromARGB(80, 0, 0, 0),
                            }
                        )
                    )
                )
            );

            Widget title = new Container(
                height: 72,
                padding: EdgeInsets.symmetric(0, 8),
                child: new Align(
                    alignment: Alignment.bottomLeft,
                    child: new Text(channel.name,
                        style: CTextStyle.PLargeMediumWhite)
                )
            );

            Widget count = new Container(
                padding: EdgeInsets.only(top: 4, left: 8),
                child: new Row(
                    children: new List<Widget> {
                        new Container(
                            width: 8,
                            height: 8,
                            decoration: new BoxDecoration(
                                color: CColors.AquaMarine,
                                borderRadius: BorderRadius.all(4)
                            )
                        ),
                        new Container(width: 4),
                        new Text($"{channel.memberCount}人",
                            style: CTextStyle.PSmallWhite)
                    }
                )
            );

            Widget content = Positioned.fill(
                child: new Column(
                    children: new List<Widget> {
                        title, count
                    }
                )
            );

            return new Container(
                width: 120,
                height: 120,
                margin: EdgeInsets.only(right: 16),
                child: new ClipRRect(
                    borderRadius: BorderRadius.all(8),
                    child: new Container(
                        child: new Stack(
                            children: new List<Widget> {
                                image, gradient, content
                            }
                        )
                    )
                )
            );
        }

        public static Widget buildChannelItem(ChannelView channel, Action onTap = null) {
            Widget title = new Text(channel.name,
                style: CTextStyle.PLargeMedium,
                overflow: TextOverflow.ellipsis);

            string text = "";
            if (channel.lastMessage != null) {
                text = channel.lastMessage.content;
                if (channel.lastMessage.type == ChannelMessageType.image) {
                    text = "[图片]";
                }
                else if (channel.lastMessage.type == ChannelMessageType.file) {
                    text = "[文件]";
                }

                text = text ?? "";
                if (!string.IsNullOrEmpty(channel.lastMessage.author?.fullName) &&
                    !string.IsNullOrEmpty(text)) {
                    text = $"{channel.lastMessage.author?.fullName}: {text}";
                }
            }

            Widget message = new RichText(
                text: new TextSpan(
                    channel.atMe
                        ? "[有人@我] "
                        : channel.atAll
                            ? "[@所有人] "
                            : "",
                    style: CTextStyle.PRegularError,
                    children: new List<TextSpan> {
                        new TextSpan(MessageUtils.AnalyzeMessage(
                                text, channel.lastMessage?.mentions,
                                channel.lastMessage?.mentionEveryone ?? false),
                            style: CTextStyle.PRegularBody4)
                    }
                ),
                overflow: TextOverflow.ellipsis,
                maxLines: 1
            );

            // Don't show the time if nonce is 0, i.e. the time is not loaded yet.
            // Otherwise, the time would be like 0001/01/01 8:00
            string timeString = channel.lastMessage?.nonce != 0
                ? channel.lastMessage?.time.DateTimeString() ?? ""
                : "";

            Widget titleLine = new Container(
                padding: EdgeInsets.only(left: 16),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Expanded(
                            child: title
                        ),
                        new Container(width: 16),
                        new Text(timeString,
                            style: CTextStyle.PSmallBody4)
                    }
                )
            );

            Widget icon = new Align(
                alignment: Alignment.centerRight,
                child: channel.isMute
                    ? (Widget) new Icon(
                        Icons.notifications_off,
                        size: 16, color: CColors.MuteIcon)
                    : new NotificationDot(
                        channel.unread > 0
                            ? channel.mentioned > 0
                                ? $"{channel.mentioned}"
                                : ""
                            : null
                    )
            );

            var messageIcon = new Row(
                children: new List<Widget> {
                    new Expanded(
                        child: new Container(
                            padding: EdgeInsets.symmetric(0, 16),
                            child: message
                        )
                    ),
                    channel.isMute || channel.unread > 0 ? icon : new Container()
                }
            );

            Widget avatar = new ClipRRect(
                borderRadius: BorderRadius.all(4),
                child: new Container(
                    width: 48,
                    height: 48,
                    child: CachedNetworkImageProvider.cachedNetworkImage(channel?.thumbnail ?? "", fit: BoxFit.cover)
                )
            );

            Widget ret = new Container(
                color: channel.isTop ? CColors.PrimaryBlue.withOpacity(0.04f) : CColors.White,
                height: 72,
                padding: EdgeInsets.symmetric(12, 16),
                child: new Row(
                    children: new List<Widget> {
                        avatar,
                        new Expanded(
                            child: new Column(
                                children: new List<Widget> {
                                    titleLine,
                                    new Expanded(
                                        child: messageIcon
                                    )
                                }
                            )
                        )
                    }
                )
            );

            if (onTap != null) {
                ret = new GestureDetector(
                    onTap: () => { onTap(); },
                    child: ret
                );
            }

            return ret;
        }
    }
}