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
using Unity.UIWidgets.debugger;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class MessageScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MessageScreenViewModel>(
                converter: state => {
                    var joinedChannels = state.channelState.joinedChannels.Select(
                        channelId => {
                            ChannelView channel = state.channelState.channelDict[channelId];
                            channel.isTop = state.channelState.channelTop.TryGetValue(channelId, out var isTop) &&
                                            isTop;
                            return channel;
                        }).ToList();
                    joinedChannels.Sort((c1, c2) => { return c1.isTop == c2.isTop ? 0 : (c1.isTop ? -1 : 1); });
                    return new MessageScreenViewModel {
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
                    var actionModel = new MessageScreenActionModel {
                        pushToNotificatioins = () => {
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
                        fetchMessages = () => {
                            for (int i = 0; i < viewModel.joinedChannels.Count; i++) {
                                var channel = viewModel.joinedChannels[i];
                                if (!channel.upToDate) {
                                    dispatcher.dispatch<IPromise>(
                                        Actions.fetchChannelMessages(channel.id, null,
                                            channel.messageIds.isNotEmpty()
                                            ? channel.messageIds.last()
                                            : null));
                                }
                            }

                        },
                        joinChannel = (channelId, groupId) => {
                            dispatcher.dispatch<IPromise>(Actions.joinChannel(channelId, groupId));
                        }
                    };
                    return new MessageScreen(viewModel, actionModel);
                }
            );
        }
    }


    public class MessageScreen : StatefulWidget {
        public MessageScreen(
            MessageScreenViewModel viewModel = null,
            MessageScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly MessageScreenViewModel viewModel;
        public readonly MessageScreenActionModel actionModel;

        public override State createState() {
            return new _MessageScreenState();
        }
    }

    public class _MessageScreenState : AutomaticKeepAliveClientMixin<MessageScreen>, RouteAware {
        const int firstPageNumber = 1;
        int _pageNumber = firstPageNumber;
        RefreshController _refreshController;
        TextStyle titleStyle;
        const float maxNavBarHeight = 96;
        const float minNavBarHeight = 44;
        float navBarHeight;
        string _loginSubId;
        string _refreshSubId;


        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
            // this.widget.actionModel.fetchChannels();
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            Widget viewAll = new Row(
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
            );
            ListView content = new ListView(
                children: new List<Widget> {
                    this.widget.viewModel.joinedChannels.isEmpty()
                        ? new Container(
                            padding: EdgeInsets.only(left: 16, right: 16, top: 20),
                            color: CColors.White,
                            child: new Text("热门群聊", style: CTextStyle.H5)
                        )
                        : new Container(),
                    this.widget.viewModel.joinedChannels.isEmpty()
                        ? new Container(
                            padding: EdgeInsets.only(top: 16),
                            color: CColors.White,
                            child: new SingleChildScrollView(
                                scrollDirection: Axis.horizontal,
                                child: new Container(
                                    padding: EdgeInsets.only(left: 16),
                                    child: new Row(
                                        children: this.widget.viewModel.popularChannels.Select(
                                            MessageBuildUtils.buildPopularChannelImage
                                        ).ToList()
                                    )
                                )
                            )
                        )
                        : new Container(),
                    this.widget.viewModel.joinedChannels.isEmpty()
                        ? (Widget) new Container()
                        : new Column(
                            children: this.widget.viewModel.joinedChannels.Select((channelInfo) => {
                                return MessageBuildUtils.buildChannelItem(
                                    channelInfo,
                                    () => this.widget.actionModel.pushToChannel(channelInfo.id));
                            }).ToList()
                        ),
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
                                this.widget.viewModel.joinedChannels.isEmpty()
                                    ? (Widget) new Container()
                                    : new GestureDetector(
                                        onTap: () => { this.widget.actionModel.pushToDiscoverChannels(); },
                                        child: new Container(
                                            color: CColors.Transparent,
                                            child: viewAll
                                        )
                                    )
                            }
                        )
                    ),
                    new Column(
                        children: this.widget.viewModel.publicChannels.Select(
                            (channel) => MessageBuildUtils.buildDiscoverChannelItem(channel,
                                    this.widget.actionModel.joinChannel)
                            ).ToList()
                    ),
                    new Container(height: 40)
                }
            );
            return new Container(
                color: CColors.Background,
                child: new Column(
                    children: new List<Widget> {
                        this._buildNavigationBar(),
                        new Container(color: CColors.Separator2, height: 1),
                        new Flexible(
                            child: new NotificationListener<ScrollNotification>(
                                child: new SmartRefresher(
                                    controller: this._refreshController,
                                    enablePullUp: this.widget.viewModel.joinedChannels.isEmpty(),
                                    enablePullDown: false,
                                    onRefresh: this._onRefresh,
                                    child: content
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
                        onPressed: () => { this.widget.actionModel.pushToNotificatioins(); },
                        child: new Container(
                            width: 28,
                            height: 28,
                            child: new Icon(Icons.outline_notification, color: CColors.Icon, size: 28)
                        )
                    )
                },
                backgroundColor: CColors.White,
                0
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
                this.widget.actionModel.fetchChannels().Then(
                    () => {
                        this._refreshController.sendBack(false, RefreshStatus.idle);
                        Debug.Log("Completed");
                    }).Catch((e) => {
                        this._refreshController.sendBack(false, RefreshStatus.idle);
                        Debug.Log("Failed");
                    });
            }
        }
    }

    public static class MessageBuildUtils {
        public static Widget buildPopularChannelImage(ChannelView channel) {
            return new Container(
                width: 120,
                height: 120,
                margin: EdgeInsets.only(right: 16),
                child: new ClipRRect(
                    borderRadius: BorderRadius.all(8),
                    child: new Container(
                        child: new Stack(
                            children: new List<Widget> {
                                Positioned.fill(
                                    child: Image.network(
                                        channel?.thumbnail ?? "",
                                        fit: BoxFit.cover
                                    )
                                ),
                                Positioned.fill(
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
                                ),
                                Positioned.fill(
                                    child: new Column(
                                        children: new List<Widget> {
                                            new Container(
                                                height: 72,
                                                padding: EdgeInsets.symmetric(0, 8),
                                                child: new Align(
                                                    alignment: Alignment.bottomLeft,
                                                    child: new Text(channel.name,
                                                        style: CTextStyle.PLargeMediumWhite)
                                                )
                                            ),
                                            new Container(
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
                                            )
                                        }
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }

        public static Widget buildChannelItem(ChannelView channel, Action onTap = null) {
            Widget title = new Text(channel.name, style: CTextStyle.PLargeMedium, overflow: TextOverflow.ellipsis);
            string text = channel.lastMessage == null || string.IsNullOrEmpty(channel.lastMessage.content)
                ? ""
                : (channel.lastMessage.author?.fullName == null
                       ? "" : channel.lastMessage.author?.fullName + ": ") +
                   (channel.lastMessage.content ?? "");
            Widget message = new RichText(
                text: new TextSpan(
                    channel.atMe
                        ? "[有人@我] "
                        : channel.atAll
                            ? "[@所有人] "
                            : "",
                    children: new List<TextSpan> {
                        new TextSpan(MessageUtils.AnalyzeMessage(
                                text, channel.lastMessage?.mentions, channel.lastMessage?.mentionEveryone ?? false),
                            style: CTextStyle.PRegularBody4)
                    },
                    style: CTextStyle.PRegularError
                ),
                overflow: TextOverflow.ellipsis,
                maxLines: 1);

            Widget titleLine = new Container(
                padding: EdgeInsets.only(left: 16),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Expanded(
                            child: title
                        ),
                        new Container(width: 16),
                        new Text(channel.lastMessage?.time.DateTimeString() ?? "", style: CTextStyle.PSmallBody4)
                    }
                )
            );
            
            Widget icon = new Align(
                alignment: Alignment.centerRight,
                child: channel.isMute
                    ? (Widget) new Icon(
                        Icons.notifications_off,
                        size: 16, color: CColors.LighBlueGrey)
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
                            child:message
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
                    child: Image.network(channel?.thumbnail ?? "", fit: BoxFit.cover)
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

        public static Widget buildDiscoverChannelItem(ChannelView channel, Action<string, string> joinChannel) {
            Widget title = new Text(channel.name,
                style: CTextStyle.PLargeMedium,
                maxLines: 1, overflow: TextOverflow.ellipsis);
            return new Container(
                color: CColors.White,
                height: 72,
                padding: EdgeInsets.symmetric(12, 16),
                child: new Row(
                    children: new List<Widget> {
                        new ClipRRect(
                            borderRadius: BorderRadius.all(4),
                            child: new Container(
                                width: 48,
                                height: 48,
                                child: Image.network(channel?.thumbnail ?? "", fit: BoxFit.cover)
                            )
                        ),
                        new Expanded(
                            child: new Container(
                                padding: EdgeInsets.symmetric(0, 16),
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        channel.live
                                            ? new Row(
                                                children: new List<Widget> {
                                                    new Icon(Icons.whatshot, color: CColors.Error, size: 18),
                                                    new Container(width: 7),
                                                    new Expanded(child: title)
                                                }
                                            )
                                            : title,
                                        new Expanded(
                                            child: new Text($"{channel.memberCount}成员",
                                                style: CTextStyle.PRegularBody4,
                                                maxLines: 1)
                                        )
                                    }
                                )
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.zero,
                            onPressed: channel.joined
                                ? null
                                : (GestureTapCallback) (() => { joinChannel(channel.id, channel.groupId); }),
                            child: new Container(
                                width: 60,
                                height: 28,
                                decoration: new BoxDecoration(
                                    border: Border.all(color: channel.joined
                                        ? CColors.Disable2
                                        : CColors.PrimaryBlue),
                                    borderRadius: BorderRadius.all(14)
                                ),
                                child: new Center(
                                    child: channel.joined
                                        ? new Text(
                                            "已加入",
                                            style: CTextStyle.PRegularBody5.copyWith(height: 1)
                                        )
                                        : new Text(
                                            "加入",
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