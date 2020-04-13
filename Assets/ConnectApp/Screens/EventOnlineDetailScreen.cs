using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.Plugins;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
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
using Config = ConnectApp.Constants.Config;

namespace ConnectApp.screens {
    public class EventOnlineDetailScreenConnector : StatelessWidget {
        public EventOnlineDetailScreenConnector(
            string eventId,
            Key key = null
        ) : base(key: key) {
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
                        eventsDict = state.eventState.eventsDict,
                        userLicenseDict = state.userState.userLicenseDict
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new EventDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToUserDetail = userId => {
                            dispatcher.dispatch(
                                new MainNavigatorPushToUserDetailAction {
                                    userId = userId
                                }
                            );
                        },
                        openUrl = url => { OpenUrlUtil.OpenUrl(url, dispatcher); },
                        browserImage = url => {
                            dispatcher.dispatch(new MainNavigatorPushToPhotoViewAction {
                                urls = ContentDescription.imageUrls,
                                url = url
                            });
                        },
                        copyText = text => dispatcher.dispatch(new CopyTextAction {text = text}),
                        startFetchEventDetail = () => dispatcher.dispatch(new StartFetchEventDetailAction()),
                        fetchEventDetail = (id, eventType) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchEventDetail(id, eventType)),
                        startSendMessage = () => dispatcher.dispatch(new StartSendMessageAction()),
                        sendMessage = (channelId, content, nonce, parentMessageId) => dispatcher.dispatch<IPromise>(
                            Actions.sendMessage(channelId, content, nonce, parentMessageId)),
                        showChatWindow = show => dispatcher.dispatch(new ShowChatWindowAction {show = show}),
                        startFetchMessages = () => dispatcher.dispatch(new StartFetchMessagesAction()),
                        fetchMessages = (channelId, currOldestMessageId, isFirstLoad) =>
                            dispatcher.dispatch<IPromise>(
                                Actions.fetchMessages(channelId, currOldestMessageId, isFirstLoad)
                            ),
                        shareToWechat = (type, title, description, linkUrl, imageUrl, path) =>
                            dispatcher.dispatch<IPromise>(
                                Actions.shareToWechat(type, title, description, linkUrl, imageUrl, path, true))
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
            return new _EventOnlineDetailScreenState();
        }
    }

    class _EventOnlineDetailScreenState : State<EventOnlineDetailScreen>, TickerProvider, RouteAware {
        AnimationController _controller;
        Animation<Offset> _position;
        Animation<RelativeRect> _titleAnimation;
        AnimationController _titleAnimationController;
        readonly TextEditingController _textController = new TextEditingController("");
        readonly FocusNode _focusNode = new FocusNode();
        readonly RefreshController _refreshController = new RefreshController();
        static readonly GlobalKey eventTitleKey = GlobalKey.key("event-title");
        bool _isHaveTitle;
        string _loginSubId;
        string _shareActionSubId;
        bool _showNavBarShadow;
        bool _showPlayer;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._showNavBarShadow = true;
            this._isHaveTitle = false;
            this._controller = new AnimationController(
                duration: new TimeSpan(0, 0, 0, 0, 300),
                vsync: this
            );
            this._titleAnimationController = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            RelativeRectTween rectTween = new RelativeRectTween(
                RelativeRect.fromLTRB(0, 44, 0, 0),
                RelativeRect.fromLTRB(0, 13, 0, 0)
            );
            this._titleAnimation = rectTween.animate(this._titleAnimationController);
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                AVPlayerPlugin.initVideoPlayer("", "",
                    0, (int) MediaQuery.of(this.context).padding.top, MediaQuery.of(this.context).size.width,
                    MediaQuery.of(this.context).size.width * 9 / 16, true);
                AVPlayerPlugin.hiddenPlayer();
                this.widget.actionModel.showChatWindow(false);
                this.widget.actionModel.startFetchEventDetail();
                this.widget.actionModel.fetchEventDetail(this.widget.viewModel.eventId, EventType.online);
            });
            this._loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
                this.widget.actionModel.startFetchEventDetail();
                this.widget.actionModel.fetchEventDetail(this.widget.viewModel.eventId, EventType.online);
            });
            this._shareActionSubId = EventBus.subscribe(EventBusConstant.shareAction, args => {
                if (this.widget.viewModel.eventsDict.ContainsKey(this.widget.viewModel.eventId)) {
                    this._showShareView(this.widget.viewModel.eventsDict[this.widget.viewModel.eventId]);
                }
            });
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(this.context));
        }

        public override void dispose() {
            EventBus.unSubscribe(EventBusConstant.login_success, this._loginSubId);
            EventBus.unSubscribe(EventBusConstant.shareAction, this._shareActionSubId);
            Router.routeObserve.unsubscribe(this);
            this._textController.dispose();
            this._controller.dispose();
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick, () => $"created by {this}");
        }

        public override Widget build(BuildContext context) {
            this._setAnimationPosition(context);
            var eventObj = new IEvent();
            if (this.widget.viewModel.eventsDict.ContainsKey(this.widget.viewModel.eventId)) {
                eventObj = this.widget.viewModel.eventsDict[this.widget.viewModel.eventId];
            }

            if ((this.widget.viewModel.eventDetailLoading || eventObj?.user == null) &&
                !(eventObj?.isNotFirst ?? false)) {
                return new EventDetailLoading(eventType: EventType.online,
                    mainRouterPop: this.widget.actionModel.mainRouterPop);
            }

            var eventStatus = DateConvert.GetEventStatus(eventObj.begin);
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildEventHeader(context, eventObj, EventType.online, eventStatus,
                                    this.widget.viewModel.isLoggedIn),
                                this._buildEventDetail(context, eventObj, EventType.online, eventStatus,
                                    this.widget.viewModel.isLoggedIn),
                                this._buildEventBottom(eventObj, EventType.online, eventStatus,
                                    this.widget.viewModel.isLoggedIn, context)
                            }
                        )
                    )
                )
            );
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

        Widget _buildHeadTop(bool isShowTitle, IEvent eventObj) {
            Widget shareWidget = new CustomButton(
                onPressed: () => this._showShareView(eventObj),
                child: new Container(
                    color: CColors.Transparent,
                    child: new Icon(Icons.share, size: 24,
                        color: this._showNavBarShadow ? CColors.White : CColors.Icon))
            );
            Widget titleWidget = new Container();
            if (isShowTitle) {
                Widget child = new Container();
                if (this._isHaveTitle) {
                    child = new Text(
                        eventObj.title,
                        style: CTextStyle.PXLargeMedium,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        textAlign: TextAlign.center
                    );
                }

                titleWidget = new Expanded(
                    child: new Stack(
                        fit: StackFit.expand,
                        children: new List<Widget> {
                            new PositionedTransition(
                                rect: this._titleAnimation,
                                child: child
                            )
                        }
                    )
                );
            }

            return new AnimatedContainer(
                height: 44,
                duration: TimeSpan.FromSeconds(0),
                padding: EdgeInsets.symmetric(horizontal: 8),
                decoration: new BoxDecoration(
                    CColors.White,
                    border: new Border(
                        bottom: new BorderSide(this._isHaveTitle ? CColors.Separator2 : CColors.Transparent)),
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
                                size: 24,
                                color: this._showNavBarShadow ? CColors.White : CColors.Icon
                            )
                        ),
                        titleWidget,
                        shareWidget
                    }
                )
            );
        }

        Widget _buildEventHeader(BuildContext context, IEvent eventObj, EventType eventType, EventStatus eventStatus,
            bool isLoggedIn) {
            if (isLoggedIn && eventStatus == EventStatus.past && eventObj.record.isNotEmpty()) {
                if (eventObj.record.isNotEmpty() && !AVPlayerPlugin.isConfigPlayer && AVPlayerPlugin.isExistPlayer) {
                    AVPlayerPlugin.configVideoPlayer(eventObj.record, HttpManager.getCookie());
                    AVPlayerPlugin.showPlayer();
                    this._showPlayer = true;
                }

                return new Container(
                    color: CColors.Black,
                    height: MediaQuery.of(this.context).size.width * 9 / 16
                );
            }

            return new Stack(
                children: new List<Widget> {
                    new EventHeader(eventObj, eventType, eventStatus, isLoggedIn),
                    new Positioned(
                        left: 0,
                        top: 0,
                        right: 0,
                        child: this._buildHeadTop(false, eventObj)
                    )
                }
            );
        }

        Widget _buildEventDetail(BuildContext context, IEvent eventObj, EventType eventType, EventStatus eventStatus,
            bool isLoggedIn) {
            return new Expanded(
                child: new EventDetail(
                    false,
                    userLicenseDict: this.widget.viewModel.userLicenseDict,
                    eventObj: eventObj,
                    openUrl: this.widget.actionModel.openUrl,
                    this.widget.actionModel.browserImage,
                    pushToUserDetail: this.widget.actionModel.pushToUserDetail,
                    titleKey: eventTitleKey
                )
            );
        }

        Widget _buildEventBottom(IEvent eventObj, EventType eventType, EventStatus eventStatus,
            bool isLoggedIn, BuildContext context) {
            if (!WechatPlugin.instance().isInstalled() || eventType == EventType.offline) {
                return new Container();
            }

            return new Container(
                height: 56 + CCommonUtils.getSafeAreaBottomPadding(context: context),
                padding: EdgeInsets.only(16, 8, 16, 8 + CCommonUtils.getSafeAreaBottomPadding(context: context)),
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: new Border(new BorderSide(color: CColors.Separator))
                ),
                child: new CustomButton(
                    onPressed: () => {
                        CustomDialogUtils.showCustomDialog(
                            barrierColor: Color.fromRGBO(0, 0, 0, 0.5f),
                            child: new CustomAlertDialog(
                                "即将前往微信小程序开始观看",
                                null,
                                new List<Widget> {
                                    new CustomButton(
                                        child: new Text(
                                            "稍后再说",
                                            style: new TextStyle(
                                                height: 1.33f,
                                                fontSize: 16,
                                                fontFamily: "Roboto-Regular",
                                                color: new Color(0xFF959595)
                                            ),
                                            textAlign: TextAlign.center
                                        ),
                                        onPressed: CustomDialogUtils.hiddenCustomDialog
                                    ),
                                    new CustomButton(
                                        child: new Text(
                                            "立即前往",
                                            style: CTextStyle.PLargeBlue,
                                            textAlign: TextAlign.center
                                        ),
                                        onPressed: () => {
                                            CustomDialogUtils.hiddenCustomDialog();
                                            WechatPlugin.instance().context = this.context;
                                            WechatPlugin.instance().currentEventId = eventObj.id;
                                            var path = CStringUtils.CreateMiniPath(id: eventObj.id,
                                                title: eventObj.title);
                                            if (path.isNotEmpty()) {
                                                WechatPlugin.instance().toOpenMiNi(path);
                                            }
                                        }
                                    )
                                }
                            )
                        );
                    },
                    padding: EdgeInsets.zero,
                    child: new Container(
                        decoration: new BoxDecoration(
                            color: CColors.PrimaryBlue,
                            borderRadius: BorderRadius.all(4)
                        ),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: new List<Widget> {
                                new Text(
                                    "进入微信小程序",
                                    style: CTextStyle.PLargeMediumWhite.merge(new TextStyle(height: 1))
                                )
                            }
                        )
                    )
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
            Widget child = new Container();
            if (this.widget.viewModel.messageLoading) {
                child = new GlobalLoading();
            }
            else {
                if (this.widget.viewModel.messageList.Count <= 0) {
                    child = new BlankView("暂无聊天内容", null);
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
                        child: child
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
            ActionSheetUtils.showModalActionSheet(
                new ShareView(
                    projectType: ProjectType.iEvent,
                    onPressed: type => {
                        AnalyticsManager.ClickShare(shareType: type, "Event", "Event_" + eventObj.id,
                            title: eventObj.title);
                        var linkUrl = CStringUtils.JointEventShareLink(eventId: eventObj.id);
                        var path = CStringUtils.CreateMiniPath(id: eventObj.id, title: eventObj.title);
                        if (type == ShareType.clipBoard) {
                            this.widget.actionModel.copyText(obj: linkUrl);
                            CustomDialogUtils.showToast("复制链接成功", iconData: Icons.check_circle_outline);
                        }
                        else {
                            var imageUrl = CImageUtils.SizeTo200ImageUrl(imageUrl: eventObj.avatar);
                            CustomDialogUtils.showCustomDialog(
                                child: new CustomLoadingDialog()
                            );
                            this.widget.actionModel.shareToWechat(
                                    arg1: type,
                                    arg2: eventObj.title,
                                    arg3: eventObj.shortDescription,
                                    arg4: linkUrl,
                                    arg5: imageUrl,
                                    arg6: path)
                                .Then(onResolved: CustomDialogUtils.hiddenCustomDialog)
                                .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                        }
                    }
                )
            );
        }

        public void didPopNext() {
            if (this.widget.viewModel.eventId.isNotEmpty()) {
                CTemporaryValue.currentPageModelId = this.widget.viewModel.eventId;
            }

            StatusBarManager.statusBarStyle(false);
            if (this._showPlayer) {
                AVPlayerPlugin.showPlayer();
            }
        }

        public void didPush() {
            if (this.widget.viewModel.eventId.isNotEmpty()) {
                CTemporaryValue.currentPageModelId = this.widget.viewModel.eventId;
            }
        }

        public void didPop() {
            if (CTemporaryValue.currentPageModelId.isNotEmpty() &&
                this.widget.viewModel.eventId == CTemporaryValue.currentPageModelId) {
                CTemporaryValue.currentPageModelId = null;
            }

            AVPlayerPlugin.removePlayer();
        }

        public void didPushNext() {
            CTemporaryValue.currentPageModelId = null;
            AVPlayerPlugin.hiddenPlayer();
        }
    }
}