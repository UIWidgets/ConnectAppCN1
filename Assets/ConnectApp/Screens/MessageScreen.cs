using System;
using System.Collections.Generic;
using System.Linq;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
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
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class MessageScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MessageScreenViewModel>(
                converter: state => new MessageScreenViewModel {
                    notificationLoading = state.notificationState.loading,
                    page = state.notificationState.page,
                    pageTotal = state.notificationState.pageTotal,
                    notifications = state.notificationState.notifications,
                    mentions = state.notificationState.mentions,
                    userDict = state.userState.userDict,
#if false
                    channelInfo = new List<ChannelInfo> {},
#else
                    channelInfo = new List<ChannelInfo> {
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "UI Widgets 技术交流",
                            size = 420,
                            isHot = true,
                            latestMessage = "kgu: 嗨，大家好",
                            time = "11:43",
                            isTop = true,
                            atMe = true,
                            atAll = true
                        },
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "游戏开发日常吐槽",
                            size = 208,
                            isHot = true,
                            latestMessage = "搬砖大王: 如何使用rider断调试Android",
                            time = "11:43",
                            unread = 9,
                            atAll = true
                        },
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "我们都爱玩游戏",
                            size = 199,
                            latestMessage = "Vatary: 去年发布的第一季实时动画课程",
                            time = "11:43",
                            unread = 99
                        },
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "今天你学Unity了吗",
                            size = 134,
                            latestMessage = "@海边的孙小鱼 视频中的项目可以分为以下几种",
                            time = "11:43",
                            unread = 100
                        },
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "Unity深圳Meetup",
                            size = 199,
                            latestMessage = "码农小哥: 这个Demo可以下载吗？",
                            time = "11:43",
                            unread = 15,
                            silenced = true
                        }
                    },
#endif
                    popularChannelInfo = new List<ChannelInfo> {
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "VR/AR开发者",
                            size = 117,
                        },
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "Unity\n教学联盟",
                            size = 58,
                        },
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "Unity 2020\n校园招聘",
                            size = 20,
                        }
                    },
                    discoverChannelInfo = new List<ChannelInfo> {
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "Unite Shanghai 技术会场",
                            size = 420,
                            isHot = true,
                            joined = true,
                        },
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "游戏开发日常吐槽",
                            size = 208,
                            isHot = true,
                        },
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "我们都爱玩游戏",
                            size = 199,
                            joined = true,
                        },
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "今天你学Unity了吗",
                            size = 134,
                        },
                        new ChannelInfo {
                            imageUrl = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "Unity深圳Meetup",
                            size = 199,
                        }
                    }
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MessageScreenActionModel {
                        pushToNotificatioins = () => {
                            dispatcher.dispatch(new MainNavigatorPushToNotificationAction());
                        },
                        pushToDiscoverChannels = () => {
                            dispatcher.dispatch(new MainNavigatorPushToDiscoverChannelsAction());
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
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
        }

        public override void dispose() {
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            Widget content = new ListView(
                children: new List<Widget> {
                    this.widget.viewModel.channelInfo.isEmpty() ? new Container(
                        padding: EdgeInsets.only(left: 16, right: 16, top: 20),
                        color: CColors.White,
                        child: new Text("热门群聊", style: CTextStyle.H5)
                    ) : new Container(),
                    this.widget.viewModel.channelInfo.isEmpty() ? new Container(
                        padding: EdgeInsets.only(top: 16),
                        color: CColors.White,
                        child: new SingleChildScrollView(
                            scrollDirection: Axis.horizontal,
                            child: new Container(
                                padding: EdgeInsets.only(left: 16),
                                child: new Row(
                                    children: this.widget.viewModel.popularChannelInfo.Select(
                                        MessageBuildUtils.buildPopularChannelImage
                                    ).ToList()
                                )
                            )
                        )
                    ) : new Container(),
                    this.widget.viewModel.channelInfo.isEmpty() ? (Widget) new Container()
                        : new Column(
                            children: this.widget.viewModel.channelInfo.Select(MessageBuildUtils.buildChannelItem).ToList()
                        ),
                    this.widget.viewModel.channelInfo.isEmpty() ? new Container(height: 24, color: CColors.White)
                        : new Container(height: 16),
                    new Container(
                        color: CColors.White,
                        padding: this.widget.viewModel.channelInfo.isEmpty()
                            ? EdgeInsets.all(16)
                            : EdgeInsets.only(16, 16, 8, 16),
                        child: new Row(
                            children: new List<Widget> {
                                new Text("发现群聊", style: CTextStyle.H5),
                                new Expanded(
                                    child: new Container()
                                ),
                                this.widget.viewModel.channelInfo.isEmpty()
                                    ? (Widget) new Container()
                                    : new GestureDetector(
                                        onTap: () => { this.widget.actionModel.pushToDiscoverChannels(); },
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
                                    )
                            }
                        )
                    ),
                    new Column(
                        children: this.widget.viewModel.discoverChannelInfo.Select(
                            MessageBuildUtils.buildDiscoverChannelItem
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
                                child: new CustomScrollbar(child: content)
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
                        onPressed: () => {
                            this.widget.actionModel.pushToNotificatioins();
                        },
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
        }

        public void didPush() {
        }

        public void didPop() {
        }

        public void didPushNext() {
        }
    }

    public static class MessageBuildUtils {
        public static Widget buildPopularChannelImage(ChannelInfo channelInfo) {
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
                                        channelInfo.imageUrl,
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
                                                    child: new Text(channelInfo.name,
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
                                                        new Text($"{channelInfo.size}人", style: CTextStyle.PSmallWhite)
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

        public static Widget buildChannelItem(ChannelInfo channelInfo) {
            Widget title = new Text(channelInfo.name, style: CTextStyle.PLargeMedium);
            return new Container(
                color: channelInfo.isTop ? CColors.PrimaryBlue.withOpacity(0.04f) : CColors.White,
                height: 72,
                padding: EdgeInsets.symmetric(12, 16),
                child: new Row(
                    children: new List<Widget> {
                        new ClipRRect(
                            borderRadius: BorderRadius.all(4),
                            child: new Container(
                                width: 48,
                                height: 48,
                                child: Image.network(channelInfo.imageUrl, fit: BoxFit.cover)
                            )
                        ),
                        new Expanded(
                            child: new Container(
                                padding: EdgeInsets.only(left: 16),
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        title,
                                        new Expanded(
                                            child: new RichText(
                                                text: new TextSpan(
                                                    channelInfo.atMe
                                                        ? "[有人@我] "
                                                        : channelInfo.atAll
                                                            ? "[@所有人] "
                                                            : "",
                                                    children: new List<TextSpan> {
                                                        new TextSpan(channelInfo.latestMessage,
                                                            style: CTextStyle.PRegularBody4
                                                        )
                                                    },
                                                    style: CTextStyle.PRegularError
                                                ),
                                                overflow: TextOverflow.ellipsis,
                                                maxLines: 1)
                                        )
                                    }
                                )
                            )
                        ),
                        new Container(
                            width: 32,
                            child: new Container(
                                child: new Column(
                                    children: new List<Widget> {
                                        new Text(channelInfo.time, style: CTextStyle.PSmallBody4),
                                        new Expanded(
                                            child: channelInfo.silenced || channelInfo.unread > 0
                                                    ? (Widget) new Align(
                                                        alignment: Alignment.centerRight,
                                                        child: channelInfo.silenced
                                                            ? (Widget) new Icon(
                                                                Icons.notification,
                                                                size: 16, color: CColors.LighBlueGrey)
                                                            : new NotificationDot(
                                                                channelInfo.unread > 99
                                                                    ? ""
                                                                    : $"{channelInfo.unread}"
                                                            )
                                                    )
                                                    : new Container()
                                        )
                                    }
                                )
                            )
                        )
                    }
                )
            );
        }

        public static Widget buildDiscoverChannelItem(ChannelInfo channelInfo) {
            Widget title = new Text(channelInfo.name,
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
                                child: Image.network(channelInfo.imageUrl, fit: BoxFit.cover)
                            )
                        ),
                        new Expanded(
                            child: new Container(
                                padding: EdgeInsets.symmetric(0, 16),
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        channelInfo.isHot
                                            ? new Row(
                                                children: new List<Widget> {
                                                    new Icon(Icons.favorite, color: CColors.Error, size: 18),
                                                    new Container(width: 7),
                                                    new Expanded(child: title)
                                                }
                                            )
                                            : title,
                                        new Expanded(
                                            child: new Text($"{channelInfo.size}成员",
                                                style: CTextStyle.PRegularBody4,
                                                maxLines: 1)
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
                                   border: Border.all(color: channelInfo.joined
                                       ? CColors.Disable2
                                       : CColors.PrimaryBlue),
                                   borderRadius: BorderRadius.all(14)
                               ),
                               child: new Center(
                                   child: channelInfo.joined
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
