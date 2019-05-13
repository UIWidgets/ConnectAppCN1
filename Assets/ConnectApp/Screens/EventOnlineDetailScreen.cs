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
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
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

        private readonly string eventId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, EventDetailScreenViewModel>(
                converter: state => {
                    var channelId = state.eventState.channelId;
                    var channelMessageList = state.messageState.channelMessageList;
                    var messageList = new List<string>();
                    if (channelMessageList.ContainsKey(channelId))
                        messageList = channelMessageList[channelId];
                    return new EventDetailScreenViewModel {
                        eventId = eventId,
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

    internal class _EventDetailScreenState : State<EventOnlineDetailScreen>, TickerProvider {
        private AnimationController _controller;
        private Animation<Offset> _position;
        private readonly TextEditingController _textController = new TextEditingController("");
        private readonly FocusNode _focusNode = new FocusNode();
        private readonly RefreshController _refreshController = new RefreshController();
        private string _loginSubId;
        private bool _showNavBarShadow;
        private bool _isFullScreen;


        public override void initState() {
            base.initState();
            _showNavBarShadow = true;
            _controller = new AnimationController(
                duration: new TimeSpan(0, 0, 0, 0, 300),
                vsync: this
            );
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                widget.actionModel.showChatWindow(false);
                widget.actionModel.startFetchEventDetail();
                widget.actionModel.fetchEventDetail(widget.viewModel.eventId, EventType.online);
            });
            _loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
                widget.actionModel.startFetchEventDetail();
                widget.actionModel.fetchEventDetail(widget.viewModel.eventId, EventType.online);
            });
        }

        public override Widget build(BuildContext context) {
            _setAnimationPosition(context);
            var eventObj = new IEvent();
            if (widget.viewModel.eventsDict.ContainsKey(widget.viewModel.eventId))
                eventObj = widget.viewModel.eventsDict[widget.viewModel.eventId];
            if ((widget.viewModel.eventDetailLoading || eventObj?.user == null)&&!eventObj.isNotFirst)
                return new EventDetailLoading(mainRouterPop: widget.actionModel.mainRouterPop);
            var eventStatus = DateConvert.GetEventStatus(eventObj.begin);
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    top:!_isFullScreen,
                    bottom:!_isFullScreen,
                    child: new Container(
                        color: _isFullScreen?CColors.Black:CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                _buildEventHeader(eventObj, EventType.online, eventStatus,
                                    widget.viewModel.isLoggedIn),
                                _buildEventDetail(context, eventObj, EventType.online, eventStatus,
                                    widget.viewModel.isLoggedIn),
                                _buildEventBottom(eventObj, EventType.online, eventStatus,
                                    widget.viewModel.isLoggedIn)
                            }
                            
                        )
                    )
                )
            );
            
        }
      
        public override void dispose() {
            EventBus.unSubscribe(EventBusConstant.login_success, _loginSubId);
            _textController.dispose();
            _controller.dispose();
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick, () => $"created by {this}");
        }

        private void _setAnimationPosition(BuildContext context) {
            if (_position != null) return;
            var screenHeight = MediaQuery.of(context).size.height;
            var screenWidth = MediaQuery.of(context).size.width;
            var ratio = 1.0f - 64.0f / (screenHeight - screenWidth * 9.0f / 16.0f);

            _position = new OffsetTween(
                new Offset(0, ratio),
                new Offset(0, 0)
            ).animate(new CurvedAnimation(
                _controller,
                Curves.easeInOut
            ));
        }


        private void _onRefresh(bool up) {
            if (up)
                widget.actionModel
                    .fetchMessages(widget.viewModel.channelId, widget.viewModel.currOldestMessageId, false)
                    .Then(() => _refreshController.sendBack(true, RefreshStatus.completed))
                    .Catch(_ => _refreshController.sendBack(true, RefreshStatus.failed));
        }

        private void _handleSubmitted(string text) {
            widget.actionModel.startSendMessage();
            widget.actionModel.sendMessage(widget.viewModel.channelId, text, Snowflake.CreateNonce(), "")
                .Catch(_ => { CustomDialogUtils.showToast("消息发送失败", Icons.error_outline); });
            _refreshController.scrollTo(0);
        }

        private Widget _buildHeadTop(bool isShowShare, IEvent eventObj) {
            Widget shareWidget = new Container();
            if (isShowShare)
                shareWidget = new CustomButton(
                    onPressed:()=> _showShareView(eventObj),
                    child: new Container(
                        alignment: Alignment.topRight,
                        width: 64,
                        height: 64,
                        color: CColors.Transparent,
                        child: new Icon(Icons.share, size: 28, color: _showNavBarShadow ? CColors.White : CColors.icon3))
                );
            return new AnimatedContainer(
                height: 44,
                duration: new TimeSpan(0, 0, 0, 0, 0),
                padding: EdgeInsets.symmetric(horizontal: 8),
                decoration: new BoxDecoration(
                    CColors.White,
                    gradient: _showNavBarShadow ? new LinearGradient(
                        colors: new List<Color> {
                            new Color(0x80000000),
                            new Color(0x0)
                        },
                        begin: Alignment.topCenter,
                        end: Alignment.bottomCenter
                    ) : null 
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new CustomButton(
                            onPressed: () =>
                            {
                                widget.actionModel.mainRouterPop();
                            },
                            child: new Icon(
                                Icons.arrow_back,
                                size: 28,
                                color: _showNavBarShadow ? CColors.White : CColors.icon3
                            )
                        ),
                        shareWidget
                    }
                )
            );
        }

        private Widget _buildEventHeader(IEvent eventObj, EventType eventType, EventStatus eventStatus,
            bool isLoggedIn)
        {

            if (widget.viewModel.isLoggedIn&&eventStatus == EventStatus.past&&eventObj.record.isNotEmpty())
            {
                return new CustomVideoPlayer(
                    eventObj.record,
                    context,
                    _buildHeadTop(true,eventObj),
                    fullScreenCallback: isFullScreen =>
                    {
                        setState(() => { _isFullScreen = isFullScreen; });
                    },                    
                    eventObj.recordDuration

                ); 
            }
            return new Stack(
                children: new List<Widget> {
                    new EventHeader(eventObj, eventType, eventStatus, isLoggedIn),
                    new Positioned(
                        left: 0,
                        top: 0,
                        right: 0,
                        child: _buildHeadTop(true, eventObj)  
                    )
                }
            ); 
            
        }

        private Widget _buildEventDetail(BuildContext context, IEvent eventObj, EventType eventType, EventStatus eventStatus,
            bool isLoggedIn) {
            return new Expanded(
                child: new Stack(
                    children: new List<Widget> {
                        new EventDetail(false,eventObj, widget.actionModel.openUrl)
                    }
                )
            );
        }

        private Widget _buildEventBottom(IEvent eventObj, EventType eventType, EventStatus eventStatus,
            bool isLoggedIn) {
            if (eventStatus != EventStatus.future && eventType == EventType.online && isLoggedIn)
                return new Container();

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
                if (startTime.isNotEmpty()) subTitle = DateConvert.GetFutureTimeFromNow(startTime);
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

            if (widget.viewModel.joinEventLoading)
                child = new CustomActivityIndicator(
                    loadingColor: LoadingColor.white
                );
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
                                if (widget.viewModel.joinEventLoading) return;
                                if (!widget.viewModel.isLoggedIn) {
                                    widget.actionModel.pushToLogin();
                                }
                                else {
                                    if (!userIsCheckedIn) {
                                        widget.actionModel.startJoinEvent();
                                        widget.actionModel.joinEvent(widget.viewModel.eventId);
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

        private Widget _buildChatWindow() {
            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        _buildChatBar(widget.viewModel.showChatWindow),
                        _buildChatList(),
                        new CustomDivider(
                            height: 1,
                            color: CColors.Separator
                        ),
                        _buildTextField()
                    }
                )
            );
        }

        private Widget _buildChatBar(bool showChatWindow) {
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
                    _focusNode.unfocus();
                    if (!showChatWindow)
                        _controller.forward();
                    else
                        _controller.reverse();
                    widget.actionModel.showChatWindow(!widget.viewModel.showChatWindow);
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

        private Widget _buildChatList() {
            object child = new Container();
            if (widget.viewModel.messageLoading) {
                child = new GlobalLoading();
            }
            else {
                if (widget.viewModel.messageList.Count <= 0)
                    child = new BlankView("暂无聊天内容");
                else
                    child = new SmartRefresher(
                        controller: _refreshController,
                        enablePullDown: widget.viewModel.hasMore,
                        enablePullUp: false,
                        onRefresh: _onRefresh,
                        child: ListView.builder(
                            padding: EdgeInsets.only(16, right: 16, bottom: 10),
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: widget.viewModel.messageList.Count,
                            itemBuilder: (cxt, index) => {
                                var messageId =
                                    widget.viewModel.messageList[widget.viewModel.messageList.Count - index - 1];
                                var messageDict = new Dictionary<string, Message>();
                                if (widget.viewModel.channelMessageDict.ContainsKey(widget.viewModel.channelId))
                                    messageDict = widget.viewModel.channelMessageDict[widget.viewModel.channelId];
                                var message = new Message();
                                if (messageDict.ContainsKey(messageId))
                                    message = messageDict[messageId];
                                return new ChatMessage(
                                    message
                                );
                            }
                        )
                    );
            }

            return new Flexible(
                child: new GestureDetector(
                    onTap: () => _focusNode.unfocus(),
                    child: new Container(
                        color: CColors.White,
                        child: (Widget) child
                    )
                )
            );
        }

        private Widget _buildTextField() {
            var sendMessageLoading = widget.viewModel.sendMessageLoading;
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.symmetric(horizontal: 16),
                height: 40,
                child: new Row(
                    children: new List<Widget> {
                        new Expanded(
                            child: new InputField(
                                // key: _textFieldKey,
                                controller: _textController,
                                focusNode: _focusNode,
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
                                onSubmitted: _handleSubmitted
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

        private void _showShareView(IEvent eventObj) {
            ShareUtils.showShareView(
                new ShareView(
                    projectType: ProjectType.iEvent,
                    onPressed: type =>
                    {
                        var linkUrl =
                            $"{Config.apiAddress}/events/{eventObj.id}";
                        if (type == ShareType.clipBoard)
                        {
                            widget.actionModel.copyText(linkUrl);
                            CustomDialogUtils.showToast("复制链接成功", Icons.check_circle_outline);
                        }
                        else
                        {
                            var imageUrl = $"{eventObj.avatar}.200x0x1.jpg";
                            CustomDialogUtils.showCustomDialog(
                                child: new CustomDialog()
                            );
                            widget.actionModel.shareToWechat(type, eventObj.title, eventObj.shortDescription,
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