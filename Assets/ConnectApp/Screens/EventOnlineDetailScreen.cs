using System;
using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Config = ConnectApp.constants.Config;

namespace ConnectApp.screens {
    public class EventOnlineDetailScreenConnector : StatelessWidget {
        public EventOnlineDetailScreenConnector(string eventId) {
            this.eventId = eventId;
        }

        readonly string eventId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, EventDetailScreenViewModel>(
                converter: state => {
                    var channelId = state.eventState.channelId;
                    var channelMessageList = state.messageState.channelMessageList;
                    var messageList = new List<string>();
                    if (channelMessageList.ContainsKey(channelId)) {
                        messageList = channelMessageList[channelId];
                    }

                    return new EventDetailScreenViewModel {
                        eventId = this.eventId,
                        currOldestMessageId = state.messageState.currOldestMessageId,
                        isLoggedIn = state.loginState.isLoggedIn,
                        eventDetailLoading = state.eventState.eventDetailLoading,
                        joinEventLoading = state.eventState.joinEventLoading,
                        showChatWindow = state.eventState.showChatWindow,
                        channelId = state.eventState.channelId,
                        messageList = messageList,
                        messageLoading = state.messageState.messageLoading,
                        hasMore = state.messageState.hasMore,
                        sendMessageLoading = state.messageState.sendMessageLoading,
                        channelMessageDict = state.messageState.channelMessageDict,
                        eventsDict = state.eventState.eventsDict
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new EventDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        openUrl = url => {
                            EventBus.publish(EventBusConstant.pauseVideoPlayer, new List<object>());
                            dispatcher.dispatch(new MainNavigatorPushToWebViewAction {
                                url = url
                            });
                        },
                        copyText = text => dispatcher.dispatch(new CopyTextAction {text = text}),
                        startFetchEventDetail = () => dispatcher.dispatch(new StartFetchEventDetailAction()),
                        fetchEventDetail = (id, eventType) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchEventDetail(id, eventType)),
                        startJoinEvent = () => dispatcher.dispatch(new StartJoinEventAction()),
                        joinEvent = id => dispatcher.dispatch<IPromise>(Actions.joinEvent(id)),
                        startSendMessage = () => dispatcher.dispatch(new StartSendMessageAction()),
                        sendMessage = (channelId, content, nonce, parentMessageId) => dispatcher.dispatch<IPromise>(
                            Actions.sendMessage(channelId, content, nonce, parentMessageId)),
                        showChatWindow = show => dispatcher.dispatch(new ShowChatWindowAction {show = show}),
                        startFetchMessages = () => dispatcher.dispatch(new StartFetchMessagesAction()),
                        fetchMessages = (channelId, currOldestMessageId, isFirstLoad) =>
                            dispatcher.dispatch<IPromise>(
                                Actions.fetchMessages(channelId, currOldestMessageId, isFirstLoad)
                            ),
                        shareToWechat = (type, title, description, linkUrl, imageUrl) => dispatcher.dispatch<IPromise>(
                            Actions.shareToWechat(type, title, description, linkUrl, imageUrl))
                    };
                    return new EventOnlineDetailScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class EventOnlineDetailScreen : StatefulWidget {
        public EventOnlineDetailScreen(
            EventDetailScreenViewModel viewModel = null,
            EventDetailScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly EventDetailScreenViewModel viewModel;
        public readonly EventDetailScreenActionModel actionModel;

        public override State createState() {
            return new _EventDetailScreenState();
        }
    }

    class _EventDetailScreenState : State<EventOnlineDetailScreen>, TickerProvider {
        AnimationController _controller;
        Animation<Offset> _position;
        readonly TextEditingController _textController = new TextEditingController("");
        readonly FocusNode _focusNode = new FocusNode();
        readonly RefreshController _refreshController = new RefreshController();
        string _loginSubId;
        bool _showNavBarShadow;
        bool _isFullScreen;


        public override void initState() {
            base.initState();
            this._showNavBarShadow = true;
            this._controller = new AnimationController(
                duration: new TimeSpan(0, 0, 0, 0, 300),
                vsync: this
            );
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.showChatWindow(false);
                this.widget.actionModel.startFetchEventDetail();
                this.widget.actionModel.fetchEventDetail(this.widget.viewModel.eventId, EventType.online);
            });
            this._loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
                this.widget.actionModel.startFetchEventDetail();
                this.widget.actionModel.fetchEventDetail(this.widget.viewModel.eventId, EventType.online);
            });
        }

        public override Widget build(BuildContext context) {
            this._setAnimationPosition(context);
            var eventObj = new IEvent();
            if (this.widget.viewModel.eventsDict.ContainsKey(this.widget.viewModel.eventId)) {
                eventObj = this.widget.viewModel.eventsDict[this.widget.viewModel.eventId];
            }

            if ((this.widget.viewModel.eventDetailLoading || eventObj?.user == null) && !eventObj.isNotFirst) {
                return new EventDetailLoading(mainRouterPop: this.widget.actionModel.mainRouterPop);
            }

            var eventStatus = DateConvert.GetEventStatus(eventObj.begin);
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    top: !this._isFullScreen,
                    bottom: !this._isFullScreen,
                    child: new Container(
                        color: this._isFullScreen ? CColors.Black : CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildEventHeader(eventObj, EventType.online, eventStatus,
                                    this.widget.viewModel.isLoggedIn),
                                this._buildEventDetail(context, eventObj, EventType.online, eventStatus,
                                    this.widget.viewModel.isLoggedIn),
                                this._buildEventBottom(eventObj, EventType.online, eventStatus,
                                    this.widget.viewModel.isLoggedIn)
                            }
                        )
                    )
                )
            );
        }

        public override void dispose() {
            EventBus.unSubscribe(EventBusConstant.login_success, this._loginSubId);
            this._textController.dispose();
            this._controller.dispose();
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick, () => $"created by {this}");
        }

        void _setAnimationPosition(BuildContext context) {
            if (this._position != null) {
                return;
            }

            var screenHeight = MediaQuery.of(context).size.height;
            var screenWidth = MediaQuery.of(context).size.width;
            var ratio = 1.0f - 64.0f / (screenHeight - screenWidth * 9.0f / 16.0f);

            this._position = new OffsetTween(
                new Offset(0, ratio),
                new Offset(0, 0)
            ).animate(new CurvedAnimation(this._controller,
                Curves.easeInOut
            ));
        }


        void _onRefresh(bool up) {
            if (up) {
                this.widget.actionModel
                    .fetchMessages(this.widget.viewModel.channelId, this.widget.viewModel.currOldestMessageId, false)
                    .Then(() => this._refreshController.sendBack(true, RefreshStatus.completed))
                    .Catch(_ => this._refreshController.sendBack(true, RefreshStatus.failed));
            }
        }

        void _handleSubmitted(string text) {
            this.widget.actionModel.startSendMessage();
            this.widget.actionModel.sendMessage(this.widget.viewModel.channelId, text, Snowflake.CreateNonce(), "")
                .Catch(_ => { CustomDialogUtils.showToast("消息发送失败", Icons.error_outline); });
            this._refreshController.scrollTo(0);
        }

        Widget _buildHeadTop(bool isShowShare, IEvent eventObj) {
            Widget shareWidget = new Container();
            if (isShowShare) {
                shareWidget = new CustomButton(
                    onPressed: () => this._showShareView(eventObj),
                    child: new Container(
                        alignment: Alignment.topRight,
                        width: 64,
                        height: 64,
                        color: CColors.Transparent,
                        child: new Icon(Icons.share, size: 28,
                            color: this._showNavBarShadow ? CColors.White : CColors.icon3))
                );
            }

            return new AnimatedContainer(
                height: 44,
                duration: TimeSpan.FromSeconds(0),
                padding: EdgeInsets.symmetric(horizontal: 8),
                decoration: new BoxDecoration(
                    CColors.White,
                    gradient: this._showNavBarShadow
                        ? new LinearGradient(
                            colors: new List<Color> {
                                new Color(0x80000000),
                                new Color(0x0)
                            },
                            begin: Alignment.topCenter,
                            end: Alignment.bottomCenter
                        )
                        : null
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new CustomButton(
                            onPressed: () => { this.widget.actionModel.mainRouterPop(); },
                            child: new Icon(
                                Icons.arrow_back,
                                size: 28,
                                color: this._showNavBarShadow ? CColors.White : CColors.icon3
                            )
                        ),
                        shareWidget
                    }
                )
            );
        }

        Widget _buildEventHeader(IEvent eventObj, EventType eventType, EventStatus eventStatus,
            bool isLoggedIn) {
            if (this.widget.viewModel.isLoggedIn && eventStatus == EventStatus.past && eventObj.record.isNotEmpty()) {
                return new CustomVideoPlayer(
                    eventObj.record, this.context, this._buildHeadTop(true, eventObj),
                    fullScreenCallback: isFullScreen => {
                        this.setState(() => { this._isFullScreen = isFullScreen; });
                    },
                    eventObj.recordDuration
                );
            }

            if (eventStatus == EventStatus.past &&eventObj.record.isEmpty()) {
                return new Container();
            }
            return new Stack(
                children: new List<Widget> {
                    new EventHeader(eventObj, eventType, eventStatus, isLoggedIn),
                    new Positioned(
                        left: 0,
                        top: 0,
                        right: 0,
                        child: this._buildHeadTop(true, eventObj)
                    )
                }
            );
        }

        Widget _buildEventDetail(BuildContext context, IEvent eventObj, EventType eventType, EventStatus eventStatus,
            bool isLoggedIn) {
            if (eventObj.record.isEmpty()&&eventStatus == EventStatus.past) {
                return new Expanded(
                    child: new Stack(children:new List<Widget> {
                            new EventDetail(false, eventObj, this.widget.actionModel.openUrl,topWidget:new EventHeader(eventObj, eventType, eventStatus, isLoggedIn)),
                            new Positioned(
                                left: 0,
                                top: 0,
                                right: 0,
                                child: this._buildHeadTop(true, eventObj)
                            )
                        }
                    )
                );
            }
            return new Expanded(
                child: new EventDetail(false, eventObj, this.widget.actionModel.openUrl)
            );
        }

        Widget _buildEventBottom(IEvent eventObj, EventType eventType, EventStatus eventStatus,
            bool isLoggedIn) {
            if (eventStatus != EventStatus.future && eventType == EventType.online && isLoggedIn) {
                return new Container();
            }

            var onlineCount = eventObj.onlineMemberCount;
            var recordWatchCount = eventObj.recordWatchCount;
            var userIsCheckedIn = eventObj.userIsCheckedIn;
            var title = "";
            var subTitle = "";
            if (eventStatus == EventStatus.live) {
                title = "正在直播";
                subTitle = $"{onlineCount}人正在观看";
            }

            if (eventStatus == EventStatus.past) {
                title = "回放";
                subTitle = $"{recordWatchCount}次观看";
            }

            if (eventStatus == EventStatus.future || eventStatus == EventStatus.countDown) {
                var begin = eventObj.begin != null ? eventObj.begin : new TimeMap();
                var startTime = begin.startTime;
                if (startTime.isNotEmpty()) {
                    subTitle = DateConvert.GetFutureTimeFromNow(startTime);
                }

                title = "距离开始还有";
            }

            var backgroundColor = CColors.PrimaryBlue;
            var joinInText = "立即加入";
            var textStyle = CTextStyle.PLargeMediumWhite;
            if (userIsCheckedIn && isLoggedIn) {
                backgroundColor = CColors.Disable;
                joinInText = "已加入";
                textStyle = CTextStyle.PLargeMediumWhite;
            }

            Widget child = new Text(
                joinInText,
                style: textStyle
            );

            if (this.widget.viewModel.joinEventLoading) {
                child = new CustomActivityIndicator(
                    loadingColor: LoadingColor.white
                );
            }

            return new Container(
                height: 64,
                padding: EdgeInsets.symmetric(horizontal: 16),
                decoration: new BoxDecoration(
                    CColors.White,
                    border: new Border(new BorderSide(CColors.Separator))
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new Column(
                            mainAxisAlignment: MainAxisAlignment.center,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                new Text(
                                    title,
                                    style: CTextStyle.PSmallBody4
                                ),
                                new Container(height: 2),
                                new Text(
                                    subTitle,
                                    style: CTextStyle.H5Body
                                )
                            }
                        ),
                        new CustomButton(
                            onPressed: () => {
                                if (this.widget.viewModel.joinEventLoading) {
                                    return;
                                }

                                if (!this.widget.viewModel.isLoggedIn) {
                                    this.widget.actionModel.pushToLogin();
                                }
                                else {
                                    if (!userIsCheckedIn) {
                                        this.widget.actionModel.startJoinEvent();
                                        this.widget.actionModel.joinEvent(this.widget.viewModel.eventId);
                                    }
                                }
                            },
                            child: new Container(
                                width: 96,
                                height: 40,
                                decoration: new BoxDecoration(
                                    backgroundColor,
                                    borderRadius: BorderRadius.all(4)
                                ),
                                alignment: Alignment.center,
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: new List<Widget> {
                                        child
                                    }
                                )
                            )
                        )
                    }
                )
            );
        }

        Widget _buildChatWindow() {
            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        this._buildChatBar(this.widget.viewModel.showChatWindow),
                        this._buildChatList(),
                        new CustomDivider(
                            height: 1,
                            color: CColors.Separator
                        ),
                        this._buildTextField()
                    }
                )
            );
        }

        Widget _buildChatBar(bool showChatWindow) {
            IconData iconData;
            Widget bottomWidget;
            if (showChatWindow) {
                iconData = Icons.expand_more;
                bottomWidget = new Container();
            }
            else {
                iconData = Icons.expand_less;
                bottomWidget = new Text(
                    "轻点展开聊天",
                    style: CTextStyle.PSmallBody4
                );
            }

            return new GestureDetector(
                onTap: () => {
                    this._focusNode.unfocus();
                    if (!showChatWindow) {
                        this._controller.forward();
                    }
                    else {
                        this._controller.reverse();
                    }

                    this.widget.actionModel.showChatWindow(!this.widget.viewModel.showChatWindow);
                },
                child: new Container(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    color: CColors.White,
                    height: showChatWindow ? 44 : 64,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: new List<Widget> {
                            new Column(
                                mainAxisAlignment: MainAxisAlignment.center,
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Row(
                                        children: new List<Widget> {
                                            new Container(
                                                margin: EdgeInsets.only(right: 6),
                                                width: 6,
                                                height: 6,
                                                decoration: new BoxDecoration(
                                                    CColors.SecondaryPink,
                                                    borderRadius: BorderRadius.circular(3)
                                                )
                                            ),
                                            new Text(
                                                "直播聊天",
                                                style: new TextStyle(
                                                    height: 1.09f,
                                                    fontSize: 16,
                                                    fontFamily: "Roboto-Medium",
                                                    color: CColors.TextBody
                                                )
                                            )
                                        }
                                    ),
                                    bottomWidget
                                }
                            ),
                            new Icon(
                                iconData,
                                color: CColors.BrownGrey,
                                size: 28
                            )
                        }
                    )
                )
            );
        }

        Widget _buildChatList() {
            object child = new Container();
            if (this.widget.viewModel.messageLoading) {
                child = new GlobalLoading();
            }
            else {
                if (this.widget.viewModel.messageList.Count <= 0) {
                    child = new BlankView("暂无聊天内容");
                }
                else {
                    child = new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: this.widget.viewModel.hasMore,
                        enablePullUp: false,
                        onRefresh: this._onRefresh,
                        child: ListView.builder(
                            padding: EdgeInsets.only(16, right: 16, bottom: 10),
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: this.widget.viewModel.messageList.Count,
                            itemBuilder: (cxt, index) => {
                                var messageId =
                                    this.widget.viewModel.messageList[
                                        this.widget.viewModel.messageList.Count - index - 1];
                                var messageDict = new Dictionary<string, Message>();
                                if (this.widget.viewModel.channelMessageDict.ContainsKey(
                                    this.widget.viewModel.channelId)) {
                                    messageDict =
                                        this.widget.viewModel.channelMessageDict[this.widget.viewModel.channelId];
                                }

                                var message = new Message();
                                if (messageDict.ContainsKey(messageId)) {
                                    message = messageDict[messageId];
                                }

                                return new ChatMessage(
                                    message
                                );
                            }
                        )
                    );
                }
            }

            return new Flexible(
                child: new GestureDetector(
                    onTap: () => this._focusNode.unfocus(),
                    child: new Container(
                        color: CColors.White,
                        child: (Widget) child
                    )
                )
            );
        }

        Widget _buildTextField() {
            var sendMessageLoading = this.widget.viewModel.sendMessageLoading;
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.symmetric(horizontal: 16),
                height: 40,
                child: new Row(
                    children: new List<Widget> {
                        new Expanded(
                            child: new InputField(
                                // key: _textFieldKey,
                                controller: this._textController,
                                focusNode: this._focusNode,
                                enabled: !sendMessageLoading,
                                height: 40,
                                style: new TextStyle(
                                    color: sendMessageLoading
                                        ? CColors.TextBody3
                                        : CColors.TextBody,
                                    fontFamily: "Roboto-Regular",
                                    fontSize: 16
                                ),
                                hintText: "说点想法…",
                                hintStyle: CTextStyle.PLargeBody4,
                                keyboardType: TextInputType.multiline,
                                maxLines: 1,
                                cursorColor: CColors.PrimaryBlue,
                                textInputAction: TextInputAction.send,
                                onSubmitted: this._handleSubmitted
                            )
                        ),
                        sendMessageLoading
                            ? new Container(
                                width: 32,
                                height: 32,
                                child: new CustomActivityIndicator()
                            )
                            : new Container()
                    }
                )
            );
        }

        void _showShareView(IEvent eventObj) {
            ShareUtils.showShareView(
                new ShareView(
                    projectType: ProjectType.iEvent,
                    onPressed: type => {
                        var linkUrl =
                            $"{Config.apiAddress}/events/{eventObj.id}";
                        if (type == ShareType.clipBoard) {
                            this.widget.actionModel.copyText(linkUrl);
                            CustomDialogUtils.showToast("复制链接成功", Icons.check_circle_outline);
                        }
                        else {
                            var imageUrl = $"{eventObj.avatar}.200x0x1.jpg";
                            CustomDialogUtils.showCustomDialog(
                                child: new CustomLoadingDialog()
                            );
                            this.widget.actionModel.shareToWechat(type, eventObj.title, eventObj.shortDescription,
                                    linkUrl,
                                    imageUrl).Then(CustomDialogUtils.hiddenCustomDialog)
                                .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                        }
                    }
                )
            );
        }
    }
}