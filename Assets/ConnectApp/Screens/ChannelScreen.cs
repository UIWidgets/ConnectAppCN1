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
using ConnectApp.Plugins;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
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
using Config = ConnectApp.Constants.Config;
using Icons = ConnectApp.Constants.Icons;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class ChannelScreenConnector : StatelessWidget {
        public ChannelScreenConnector(
            string channelId,
            Key key = null
        ) : base(key: key) {
            this.channelId = channelId;
        }

        readonly string channelId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelScreenViewModel>(
                converter: state => {
                    List<ChannelMessageView> newMessages = new List<ChannelMessageView>();
                    List<ChannelMessageView> messages = new List<ChannelMessageView>();

                    ChannelView channel = !state.channelState.channelDict.ContainsKey(this.channelId)
                        ? ChannelView.fromChannel(new Channel())
                        : state.channelState.channelDict[this.channelId];

                    foreach (var messageId in channel.oldMessageIds) {
                        if (state.channelState.messageDict.ContainsKey(key: messageId)) {
                            messages.Add(state.channelState.messageDict[key: messageId]);
                        }
                    }

                    foreach (var messageId in channel.messageIds) {
                        if (state.channelState.messageDict.ContainsKey(key: messageId)) {
                            messages.Add(state.channelState.messageDict[key: messageId]);
                        }
                    }

                    ChannelMessageView waitingMessage = null;
                    ChannelMessageView sendingMessage = null;
                    foreach (var messageId in channel.localMessageIds) {
                        var key = $"{state.loginState.loginInfo.userId}:{this.channelId}:{messageId}";
                        if (state.channelState.localMessageDict.ContainsKey(key: key)) {
                            var message = state.channelState.localMessageDict[key: key];
                            if (message.status == "sending") {
                                sendingMessage = sendingMessage ?? message;
                            }
                            else if (message.status == "waiting") {
                                waitingMessage = waitingMessage ?? message;
                            }

                            messages.Add(message);
                        }
                    }

                    foreach (var messageId in channel.newMessageIds) {
                        if (state.channelState.messageDict.ContainsKey(key: messageId)) {
                            newMessages.Add(state.channelState.messageDict[key: messageId]);
                        }
                    }

                    if (messages.isNotEmpty()) {
                        messages = messages.Where(message => !message.shouldSkip()).ToList();
                        if (channel.localMessageIds.isNotEmpty()) {
                            messages.Sort((m1, m2) => {
                                if ((m1.status != "sending" && m1.status != "waiting") &&
                                    (m2.status == "sending" || m2.status == "waiting")) {
                                    return -1;
                                }

                                if ((m2.status != "sending" && m2.status != "waiting") &&
                                    (m1.status == "sending" || m1.status == "waiting")) {
                                    return 1;
                                }

                                if (m1.id.hexToLong() < m2.id.hexToLong()) {
                                    return -1;
                                }

                                if (m1.id.hexToLong() > m2.id.hexToLong()) {
                                    return 1;
                                }

                                return 0;
                            });
                        }
                    }

                    if (newMessages.isNotEmpty()) {
                        newMessages = newMessages
                            .Where(message => !message.shouldSkip())
                            .ToList();
                    }

                    return new ChannelScreenViewModel {
                        hasChannel = state.channelState.channelDict.ContainsKey(this.channelId),
                        channelError = state.channelState.channelError,
                        channel = channel,
                        messages = messages,
                        newMessages = newMessages,
                        me = new User {
                            id = state.loginState.loginInfo.userId,
                            avatar = state.loginState.loginInfo.userAvatar,
                            fullName = state.loginState.loginInfo.userFullName
                        },
                        messageLoading = state.channelState.messageLoading,
                        newMessageCount = channel.unread,
                        socketConnected = state.channelState.socketConnected,
                        netWorkConnected = state.channelState.netWorkConnected,
                        mentionAutoFocus = state.channelState.mentionAutoFocus,
                        mentionUserId = state.channelState.mentionUserId,
                        mentionUserName = state.channelState.mentionUserName,
                        mentionSuggestion = state.channelState.mentionSuggestions.getOrDefault(this.channelId, null),
                        waitingMessage = waitingMessage,
                        sendingMessage = sendingMessage
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    if (viewModel.hasChannel) {
                        if (viewModel.channel.oldMessageIds.isNotEmpty()) {
                            SchedulerBinding.instance.addPostFrameCallback(_ => {
                                dispatcher.dispatch(new MergeOldChannelMessages {channelId = this.channelId});
                            });
                        }
                    }

                    if (viewModel.waitingMessage != null && viewModel.sendingMessage == null) {
                        SchedulerBinding.instance.addPostFrameCallback(_ => {
                            dispatcher.dispatch(new StartSendChannelMessageAction {
                                message = viewModel.waitingMessage
                            });
                            if (viewModel.waitingMessage.type == ChannelMessageType.text) {
                                dispatcher.dispatch<IPromise>(Actions.sendChannelMessage(
                                    this.channelId,
                                    viewModel.waitingMessage.content,
                                    nonce: viewModel.waitingMessage.id,
                                    parentMessageId: ""));
                            }
                            else {
                                dispatcher.dispatch<IPromise>(Actions.sendImage(
                                    this.channelId,
                                    nonce: viewModel.waitingMessage.id,
                                    imageData: viewModel.waitingMessage.content));
                            }
                        });
                    }

                    var actionModel = new ChannelScreenActionModel {
                        mainRouterPop = () => {
                            dispatcher.dispatch(new MainNavigatorPopAction());
                        },
                        openUrl = url => OpenUrlUtil.OpenUrl(url: url, dispatcher: dispatcher),
                        browserImage = (url, imageUrls) => dispatcher.dispatch(new MainNavigatorPushToPhotoViewAction {
                            url = url,
                            urls = imageUrls
                        }),
                        fetchChannelInfo = () => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelInfo(channelId: this.channelId)),
                        fetchMessages = (before, after) => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMessages(channelId: this.channelId, before: before, after: after)),
                        fetchMembers = () => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMembers(channelId: this.channelId)),
                        fetchMember = () => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMember(channelId: this.channelId, userId: viewModel.me.id)),
                        deleteChannelMessage = messageId => dispatcher.dispatch<IPromise>(
                            Actions.deleteChannelMessage(messageId: messageId)),
                        pushToChannelDetail = () => dispatcher.dispatch(new MainNavigatorPushToChannelDetailAction {
                            channelId = this.channelId
                        }),
                        pushToUserDetail = userId => dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {
                            userId = userId
                        }),
                        sendMessage = (channelId, content, nonce, parentMessageId) => dispatcher.dispatch<IPromise>(
                            Actions.sendChannelMessage(channelId, content, nonce, parentMessageId)),
                        ackMessage = () => dispatcher.dispatch(Actions.ackChannelMessage(viewModel.channel.lastMessageId)),
                        sendImage = (channelId, data, nonce) => dispatcher.dispatch<IPromise>(
                            Actions.sendImage(channelId, nonce, data)),
                        clearUnread = () => dispatcher.dispatch(new ClearChannelUnreadAction {
                            channelId = this.channelId
                        }),
                        reportHitBottom = () => {
                            dispatcher.dispatch(new ChannelScreenHitBottom {channelId = this.channelId});
                            dispatcher.dispatch(Actions.ackChannelMessage(viewModel.channel.lastMessageId));
                            dispatcher.dispatch(new MergeNewChannelMessages {channelId = this.channelId});
                        },
                        reportLeaveBottom = () => dispatcher.dispatch(new ChannelScreenLeaveBottom {
                            channelId = this.channelId
                        }),
                        popFromScreen = () => {
                            dispatcher.dispatch(Actions.ackChannelMessage(viewModel.channel.lastMessageId));
                            dispatcher.dispatch(new SetChannelInactive {channelId = this.channelId});
                        },
                        pushToChannelMention = () => {
                            dispatcher.dispatch(new MainNavigatorPushToChannelMentionAction {
                                channelId = this.channelId
                            });
                        },
                        clearLastChannelMention = () => dispatcher.dispatch(new ChannelClearMentionAction()),
                        addLocalMessage = message => dispatcher.dispatch(new AddLocalMessageAction {
                            message = message
                        }),
                        resendMessage = message => dispatcher.dispatch(new ResendMessageAction {
                            message = message
                        })
                    };
                    return new ChannelScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class ChannelScreen : StatefulWidget {
        public ChannelScreen(
            ChannelScreenViewModel viewModel = null,
            ChannelScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ChannelScreenViewModel viewModel;
        public readonly ChannelScreenActionModel actionModel;

        public override State createState() {
            return new _ChannelScreenState();
        }
    }

    class _ChannelScreenState : TickerProviderStateMixin<ChannelScreen>, RouteAware {
        readonly TextEditingController _textController = new TextEditingController();
        readonly RefreshController _refreshController = new RefreshController();
        readonly GlobalKey _smartRefresherKey = GlobalKey<State<SmartRefresher>>.key("SmartRefresher");
        readonly FocusNode _focusNode = new FocusNode();
        readonly GlobalKey _textFieldKey = GlobalKey.key("_channelTextFieldKey");
        readonly TimeSpan _showTimeThreshold = TimeSpan.FromMinutes(5);

        readonly Dictionary<string, string> _headers = new Dictionary<string, string> {
            {HttpManager.COOKIE, HttpManager.getCookie()},
            {"ConnectAppVersion", Config.versionNumber},
            {"X-Requested-With", "XmlHttpRequest"}
        };

        bool _showEmojiBoard;
        string _lastMessageEditingContent = "";
        readonly Dictionary<string, string> mentionMap = new Dictionary<string, string>();
        string _lastReadMessageId = null;
        AnimationController _unreadNotificationController;
        AnimationController _newMessageNotificationController;
        AnimationController _emojiBoardController;
        AnimationController _viewInsetsBottomController;
        Animation<float> _viewInsetsBottomAnimation;

        bool _showUnreadMessageNotification = false;
        bool _showNewMessageNotification = false;
        float _inputFieldHeight;

        public bool showUnreadMessageNotification {
            get { return this._showUnreadMessageNotification; }
            set {
                if (this._showUnreadMessageNotification == value) {
                    return;
                }
                this._showUnreadMessageNotification = value;
                if (this._showUnreadMessageNotification) {
                    Promise.Delayed(TimeSpan.FromMilliseconds(500)).Then(() => {
                        this._unreadNotificationController.animateTo(1.0f);
                    });
                }
                else {
                    this._unreadNotificationController.animateBack(0.0f, TimeSpan.FromMilliseconds(100));
                }
            }
        }
        
        public bool showNewMessageNotification {
            get { return this._showNewMessageNotification; }
            set {
                if (this._showNewMessageNotification == value) {
                    return;
                }
                this._showNewMessageNotification = value;
                if (this._showNewMessageNotification) {
                    Promise.Delayed(TimeSpan.FromMilliseconds(100)).Then(() => {
                        this._newMessageNotificationController.animateTo(1.0f);
                    });
                }
                else {
                    this._newMessageNotificationController.animateBack(0.0f, TimeSpan.FromMilliseconds(100));
                }
            }
        }
        

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        float messageBubbleWidth {
            get { return MediaQuery.of(context: this.context).size.width * 0.7f; }
        }

        float inputBarHeight {
            get { return this._inputFieldHeight + 24 + CCommonUtils.getSafeAreaBottomPadding(context: this.context); }
        }

        bool showKeyboard {
            get { return this.viewInsetsBottom > 50; }
        }

        bool showEmojiBoard {
            get {
                if (this.showKeyboard && this._showEmojiBoard) {
                    Promise.Delayed(TimeSpan.FromMilliseconds(300)).Then(() => {
                        if (this.showKeyboard && this._showEmojiBoard) {
                            this._showEmojiBoard = false;
                        }
                    });
                }

                return (this._showEmojiBoard || this._emojiBoardController.value > 0.1f) && !this.showKeyboard;
            }

            set {
                if (this.showEmojiBoard == value) {
                    return;
                }

                this._showEmojiBoard = value;
                if (this._showEmojiBoard == value) {
                    if (value) {
                        this._emojiBoardController.animateTo(1);
                    }
                    else {
                        this._emojiBoardController.animateBack(0, TimeSpan.FromMilliseconds(200));
                    }
                }
            }
        }

        float _viewInsetsBottom = 0;

        public float viewInsetsBottom {
            get {
                var target = MediaQuery.of(this.context).viewInsets.bottom;
                if (this._viewInsetsBottom != target) {
                    this._viewInsetsBottomAnimation = new FloatTween(begin: this._viewInsetsBottom, end: target)
                        .animate(this._viewInsetsBottomController);
                    this._viewInsetsBottomController.reset();
                    this._viewInsetsBottomController.animateTo(1);
                }

                this._viewInsetsBottom = target;
                return this._viewInsetsBottomAnimation.value;
            }
        }

        public override void initState() {
            base.initState();
            this._lastReadMessageId = this.widget.viewModel.channel.lastReadMessageId;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (this.widget.viewModel.hasChannel) {
                    this.fetchMessagesAndMembers();
                    this.addScrollListener();
                }
                else {
                    this.widget.actionModel.fetchChannelInfo().Then(() => {
                        this.fetchMessagesAndMembers();
                        this.addScrollListener();
                    });
                }

                this.widget.actionModel.clearUnread();
            });

            this._showEmojiBoard = false;
            this._inputFieldHeight = 24;
            this._textController.addListener(this._onTextChanged);
            this._unreadNotificationController = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            this._newMessageNotificationController = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            this._emojiBoardController = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            this._viewInsetsBottomController = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            this._viewInsetsBottomAnimation =
                new FloatTween(begin: 0, end: 0).animate(this._viewInsetsBottomController);
            this._unreadNotificationController.addListener(() => {
                this.setState(() => {});
            });
            this._newMessageNotificationController.addListener(() => {
                this.setState(() => {});
            });
            this._emojiBoardController.addListener(() => {
                this.setState(() => {});
            });
            this._viewInsetsBottomController.addListener(() => {
                this.setState(() => {});
            });
        }

        bool _onMessageLoadedCalled = false;
        void _onMessageLoaded() {
            if (this._onMessageLoadedCalled) {
                return;
            }

            this._onMessageLoadedCalled = true;
            
            this.showUnreadMessageNotification = this._lastReadMessageId != null &&
                                                 this.calculateOffsetFromMessage(this._lastReadMessageId) > 0;
        }

        void fetchMessagesAndMembers() {
            this.widget.actionModel.fetchMessages(null, null);
            this.widget.actionModel.fetchMembers();
            this.widget.actionModel.fetchMember();
            this.widget.actionModel.reportHitBottom();
        }

        void addScrollListener() {
            this._refreshController.scrollController.addListener(this._handleScrollListener);
        }

        void jumpToLastReadMessage() {
            if (this._lastReadMessageId != null) {
                this.jumpToMessage(this._lastReadMessageId);
            }
        }

        void jumpToMessage(string id) {
            var index = this.widget.viewModel.messages.FindIndex(message => {
                return message.id.hexToLong() > id.hexToLong();
            });
            if (index >= 0) {
                this.jumpToIndex(index);
            }
        }

        float calculateTotalHeightFromMessage(string id) {
            var index = this.widget.viewModel.messages.FindIndex(message => {
                return message.id.hexToLong() > id.hexToLong();
            });
            if (index >= 0) {
                return this.calculateMessageHeightFromIndex(index);
            }

            return 0;
        }

        float calculateOffsetFromMessage(string id) {
            var index = this.widget.viewModel.messages.FindIndex(message => {
                return message.id.hexToLong() > id.hexToLong();
            });
            if (index >= 0) {
                return this.calculateOffsetFromIndex(index);
            }

            return 0;
        }

        float calculateMessageHeightFromIndex(int index) {
            float height = 0;
            for (int i = index; i < this.widget.viewModel.messages.Count; i++) {
                var message = this.widget.viewModel.messages[i];
                height += calculateMessageHeight(message,
                    i == 0 || message.time - this.widget.viewModel.messages[i - 1].time > this._showTimeThreshold,
                    this.messageBubbleWidth);
            }

            return height;
        }

        float calculateOffsetFromIndex(int index) {
            return this.calculateMessageHeightFromIndex(index) -
                   (MediaQuery.of(this.context).size.height - CustomAppBarUtil.appBarHeight - 80);
        }

        void jumpToIndex(int index) {
            float offset = this.calculateOffsetFromIndex(index);
            if (offset < 0) {
                this.showUnreadMessageNotification = false;
            }
            else {
                this._refreshController.scrollTo(offset);
            }
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            this._textController.dispose();
            SchedulerBinding.instance.addPostFrameCallback(_ => { this.widget.actionModel.clearUnread(); });
            this._focusNode.dispose();
            this._unreadNotificationController.dispose();
            this._newMessageNotificationController.dispose();
            this._emojiBoardController.dispose();
            this._viewInsetsBottomController.dispose();
            base.dispose();
        }

        void _onTextChanged() {
            var curTextContent = this._textController.text;
            if (curTextContent != this._lastMessageEditingContent) {
                var isDelete = curTextContent.Length < this._lastMessageEditingContent.Length;
                this._lastMessageEditingContent = curTextContent;

                if (!isDelete &&
                    this._lastMessageEditingContent.isNotEmpty() &&
                    this._lastMessageEditingContent[this._lastMessageEditingContent.Length - 1] == '@') {
                    this.widget.actionModel.pushToChannelMention();
                }
            }

            var inputFieldWidth = this._textFieldKey.currentContext.size.width;
            if (curTextContent.isNotEmpty()) {
                var inputFieldHeight = CTextUtils.CalculateTextHeight(
                    text: curTextContent, textStyle: CTextStyle.PLargeBody, textWidth: inputFieldWidth, 4);
                if (this._inputFieldHeight != inputFieldHeight) {
                    this.setState(() => this._inputFieldHeight = inputFieldHeight);
                }
            }
            else {
                if (this._inputFieldHeight != 24) {
                    this.setState(() => this._inputFieldHeight = 24);
                }
            }
        }

        void _deleteMessage(ChannelMessageView message) {
            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                title: "是否删除这条消息？",
                items: new List<ActionSheetItem> {
                    new ActionSheetItem(
                        "删除",
                        type: ActionType.destructive,
                        () => this.widget.actionModel.deleteChannelMessage(message.id)
                    ),
                    new ActionSheetItem("取消", type: ActionType.cancel)
                }
            ));
        }

        void _browserImage(string imageUrl) {
            var imageUrls = new List<string>();
            this.widget.viewModel.messages.ForEach(msg => {
                if (msg.type == ChannelMessageType.image) {
                    imageUrls.Add(CImageUtils.SizeToScreenImageUrl(imageUrl: msg.content));
                }
                if (msg.type == ChannelMessageType.embedImage) {
                    imageUrls.Add(CImageUtils.SizeToScreenImageUrl(imageUrl: msg.embeds[0].embedData.imageUrl));
                }
            });
            var url = CImageUtils.SizeToScreenImageUrl(imageUrl: imageUrl);
            this.widget.actionModel.browserImage(arg1: url, arg2: imageUrls);
        }

        public override Widget build(BuildContext context) {
            if (this.widget.viewModel.channel.id == null) {
                return this._buildLoadingPage();
            }

            if (this.widget.viewModel.channel.lastMessageId == this._lastReadMessageId) {
                this._lastReadMessageId = null;
            }

            this.showNewMessageNotification = this.widget.viewModel.newMessageCount > 0;

            if (this.widget.viewModel.mentionAutoFocus) {
                SchedulerBinding.instance.addPostFrameCallback(_ => {
                    FocusScope.of(this.context)?.requestFocus(this._focusNode);
                    if (!this.widget.viewModel.mentionUserId.isEmpty()) {
                        var userName = this.widget.viewModel.mentionUserName;
                        var newContent = this._textController.text + userName + " ";

                        this.mentionMap[userName] = this.widget.viewModel.mentionUserId;

                        this._textController.value = new TextEditingValue(
                            text: newContent,
                            TextSelection.collapsed(newContent.Length));
                    }

                    this.widget.actionModel.clearLastChannelMention();
                });
            }

            if ((this.showKeyboard || this.showEmojiBoard) && this._refreshController.offset > 0) {
                SchedulerBinding.instance.addPostFrameCallback(_ => this._refreshController.scrollTo(0));
            }

            Widget ret = new Stack(
                children: new List<Widget> {
                    this._buildContent(),
                    this.widget.viewModel.messageLoading &&
                    this.widget.viewModel.messages.isEmpty()
                        ? (Widget) new GlobalLoading()
                        : new Container(),
                    this._buildInputBar(),
                    this.widget.viewModel.messageLoading
                        ? new Container()
                        : this._buildNewMessageNotification(),
                    this._lastReadMessageId == null
                        ? new Container()
                        : this._buildUnreadMessageNotification()
                }
            );

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                !this.widget.viewModel.netWorkConnected
                                    ? this._buildNetworkDisconnectedNote()
                                    : new Container(),
                                new Flexible(child: ret),
                                this.showEmojiBoard
                                    ? this._buildEmojiBoard()
                                    : new Container(height: this.viewInsetsBottom)
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNetworkDisconnectedNote() {
            return new Container(
                height: 48,
                color: CColors.Error.withAlpha((int) (255 * 0.16)),
                child: new Center(
                    child: new Text(
                        "网络未连接",
                        style: CTextStyle.PRegularError.copyWith(height: 1f)
                    )
                )
            );
        }

        Widget _buildNewMessageNotification() {
            if (this._newMessageNotificationController.value < 0.1f) {
                return new Container();
            }
            Widget ret = new Container(
                height: 40,
                decoration: new BoxDecoration(
                    color: CColors.White,
                    borderRadius: BorderRadius.all(20),
                    boxShadow: new List<BoxShadow> {
                        new BoxShadow(
                            color: CColors.Black.withOpacity(0.2f),
                            blurRadius: 8,
                            spreadRadius: 0,
                            offset: new Offset(0, 2))
                    }
                ),
                padding: EdgeInsets.symmetric(9, 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    mainAxisAlignment: MainAxisAlignment.center,
                    mainAxisSize: MainAxisSize.min,
                    children: new List<Widget> {
                        new Text(
                            $"{CStringUtils.CountToString(this.widget.viewModel.newMessageCount)}条新消息未读",
                            style: CTextStyle.PRegularBlue.copyWith(height: 1f)
                        )
                    })
            );
            
            ret = new FractionalTranslation(
                translation: new OffsetTween(new Offset(0, 1), Offset.zero)
                    .animate(this._newMessageNotificationController).value,
                child: ret);

            ret = new Positioned(
                bottom: this.inputBarHeight + 16,
                left: 0,
                right: 0,
                height: 40,
                child: new Align(
                    alignment: Alignment.center,
                    child: new GestureDetector(
                        onTap: () => {
                            this.widget.actionModel.reportHitBottom();
                            if (this.lastReadMessageLoaded()) {
                                this.showUnreadMessageNotification = false;
                            }
                            SchedulerBinding.instance.addPostFrameCallback(_ => {
                                this._refreshController.scrollTo(0);
                            });
                        },
                        child: ret
                    )
                )
            );

            return ret;
        }

        bool lastReadMessageLoaded() {
            return this.widget.viewModel.messages.isNotEmpty() &&
                   this.widget.viewModel.messages.first().id.hexToLong() <= this._lastReadMessageId.hexToLong();
        }

        Widget _buildUnreadMessageNotification() {
            if (this._unreadNotificationController.value < 0.1f) {
                return new Container();
            }
            
            var index = this.widget.viewModel.messages.FindIndex(message => {
                return message.id.hexToLong() > this._lastReadMessageId.hexToLong();
            });
            if (index < 0) {
                return new Container();
            }

            Widget ret = new Container(
                height: 40,
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: Border.all(color: CColors.Separator2, width: 1),
                    borderRadius: BorderRadius.only(topLeft: 20, bottomLeft: 20),
                    boxShadow: new List<BoxShadow> {
                        new BoxShadow(
                            color: CColors.Black.withOpacity(0.08f),
                            blurRadius: 6,
                            spreadRadius: 0,
                            offset: new Offset(0, 1))
                    }
                ),
                padding: EdgeInsets.only(left: 16, top: 9, right: 10, 9),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    mainAxisSize: MainAxisSize.min,
                    children: new List<Widget> {
                        new Text(
                            $"{this.widget.viewModel.messages.Count - index}" +
                            $"{(index == 0 && this.widget.viewModel.channel.hasMore ? "+" : "")} 条新消息",
                            style: CTextStyle.PRegularBlue.copyWith(height: 1f)
                        ),
                        new SizedBox(width: 4),
                        new Icon(
                            icon: Icons.keyboard_arrow_up,
                            color: CColors.PrimaryBlue,
                            size: 20
                        )
                    }
                )
            );
            
            ret = new FractionalTranslation(
                translation: new OffsetTween(new Offset(1.0f, 0.0f), Offset.zero)
                    .animate(this._unreadNotificationController).value,
                child: ret
            );

            ret = new Positioned(
                top: 16,
                left: 0,
                right: 0,
                height: 40,
                child: new Align(
                    alignment: Alignment.topRight,
                    child: new GestureDetector(
                        onTap: () => {
                            this.jumpToLastReadMessage();
                            if (index == 0 && this.widget.viewModel.channel.hasMore) {
                                this._refreshController.requestRefresh(false);
                            }
                            else {
                                this.showUnreadMessageNotification = false;
                            }
                        },
                        child: ret
                    )
                )
            );

            return ret;
        }

        Widget _buildNavigationBar() {
            return new CustomAppBar(
                () => this.widget.actionModel.mainRouterPop(),
                new Flexible(
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: new List<Widget> {
                            new Flexible(
                                child: new Text(
                                    !this.widget.viewModel.netWorkConnected
                                        ? this.widget.viewModel.channel.name + "(未连接)"
                                        : this.widget.viewModel.socketConnected
                                            ? this.widget.viewModel.channel.name
                                            : "收取中...",
                                    style: CTextStyle.PXLargeMedium,
                                    maxLines: 1,
                                    overflow: TextOverflow.ellipsis
                                )
                            ),
                            this.widget.viewModel.channel.isMute
                                ? new Container(
                                    margin: EdgeInsets.only(4),
                                    child: new Icon(
                                        icon: Icons.notifications_off,
                                        size: 16,
                                        color: CColors.MuteIcon
                                    )
                                )
                                : new Container()
                        }
                    )
                ),
                new CustomButton(
                    onPressed: () => this.widget.actionModel.pushToChannelDetail(),
                    child: new Container(
                        width: 28,
                        height: 28,
                        color: CColors.Transparent,
                        child: new Icon(icon: Icons.ellipsis, color: CColors.Icon, size: 28)
                    )
                )
            );
        }

        Widget _buildContent() {
            if (this.widget.viewModel.channelError) {
                return this._buildErrorPage();
            }

            Widget content = new Container(
                color: CColors.White,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        key: this._smartRefresherKey,
                        controller: this._refreshController,
                        enablePullDown: false,
                        enablePullUp: this.widget.viewModel.channel.hasMore,
                        onRefresh: this._onRefresh,
                        reverse: true,
                        headerBuilder: (context, mode) => new SmartRefreshHeader(mode: mode),
                        child: this.widget.viewModel.messageLoading &&
                               this.widget.viewModel.messages.isEmpty()
                            ? this._buildLoadingPage()
                            : this._buildMessageListView()
                    )
                )
            );

            if (this.showKeyboard || this.showEmojiBoard) {
                return new GestureDetector(
                    onTap: () => this.setState(fn: this._dismissKeyboard),
                    child: content
                );
            }

            return content;
        }

        ListView _buildMessageListView() {
            if (this.widget.viewModel.messages.Count == 0) {
                return new ListView(
                    children: new List<Widget> {
                        new Container()
                    });
            }

            this._onMessageLoaded();

            return ListView.builder(
                padding: EdgeInsets.only(top: 16, bottom: this.inputBarHeight),
                itemCount: this.widget.viewModel.messages.Count,
                itemBuilder: (context, index) => {
                    index = this.widget.viewModel.messages.Count - 1 - index;
                    var message = this.widget.viewModel.messages[index];
                    return this._buildMessage(message,
                        showTime: index == 0 ||
                                  (message.time - this.widget.viewModel.messages[index - 1].time) >
                                  this._showTimeThreshold,
                        left: message.author.id != this.widget.viewModel.me.id,
                        isBottom: index == this.widget.viewModel.messages.Count - 1
                    );
                }
            );
        }

        ListView _buildLoadingPage() {
            return new ListView(
                children: new List<Widget> {
                    new Container(
                        child: new GlobalLoading(),
                        width: MediaQuery.of(this.context).size.width,
                        height: MediaQuery.of(this.context).size.height
                    )
                });
        }

        ListView _buildErrorPage() {
            return new ListView(
                children: new List<Widget> {
                    new Container(
                        child: new Center(child: new Text("你已不在该群组", style: CTextStyle.PLargeBody.copyWith(height: 1))),
                        width: MediaQuery.of(this.context).size.width,
                        height: MediaQuery.of(this.context).size.height
                    )
                });
        }

        BoxDecoration _messageDecoration(ChannelMessageType type, bool left) {
            return type == ChannelMessageType.image
                ? null
                : new BoxDecoration(
                    color: left || type == ChannelMessageType.file
                        ? CColors.GreyMessage
                        : CColors.BlueMessage,
                    borderRadius: BorderRadius.all(10)
                );
        }

        Widget _buildMessage(ChannelMessageView message, bool showTime, bool left, bool isBottom = false) {
            if (message.shouldSkip() || message.type == ChannelMessageType.skip) {
                return new Container();
            }

            Widget ret = new Container(
                constraints: new BoxConstraints(
                    maxWidth: this.messageBubbleWidth
                ),
                decoration: this._messageDecoration(message.type, left),
                child: this._buildMessageContent(message: message)
            );

            if (message.status != "normal") {
                Widget symbol = message.status == "sending" || message.status == "waiting"
                    ? (Widget) new CustomActivityIndicator(size: LoadingSize.small)
                    : new GestureDetector(
                        onTap: () => { this.widget.actionModel.resendMessage(message); },
                        child: new Icon(icon: Icons.error, color: CColors.Error, size: 24)
                    );
                ret = new Row(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    mainAxisSize: MainAxisSize.min,
                    children: new List<Widget> {symbol, new SizedBox(width: 8), ret}
                );
            }

            var tipMenuItems = new List<TipMenuItem>();
            if (message.type == ChannelMessageType.text
                || message.type == ChannelMessageType.embedExternal
                || message.type == ChannelMessageType.embedImage) {
                tipMenuItems.Add(new TipMenuItem(
                    "复制",
                    () => {
                        var content = MessageUtils.AnalyzeMessage(
                            content: message.content,
                            mentions: message.mentions,
                            mentionEveryone: message.mentionEveryone
                        );
                        Clipboard.setData(new ClipboardData(text: content));
                    }
                ));
            }

            if (message.author.id == this.widget.viewModel.me.id) {
                tipMenuItems.Add(new TipMenuItem(
                    "删除",
                    () => this._deleteMessage(message: message)
                ));
            }

            ret = new TipMenu(
                tipMenuItems: tipMenuItems,
                child: ret
            );

            ret = new Expanded(
                child: new Column(
                    crossAxisAlignment: left ? CrossAxisAlignment.start : CrossAxisAlignment.end,
                    children: new List<Widget> {
                        new Container(
                            padding: EdgeInsets.only(bottom: 6),
                            child: new Text(
                                data: message.author.fullName,
                                style: CTextStyle.PSmallBody4,
                                maxLines: 1,
                                overflow: TextOverflow.ellipsis)
                        ),
                        ret
                    }
                )
            );

            ret = new Container(
                padding: EdgeInsets.only(left: 2, right: 2, bottom: 16),
                child: new Row(
                    mainAxisAlignment: left ? MainAxisAlignment.start : MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: left
                        ? new List<Widget> {this._buildAvatar(message.author), ret}
                        : new List<Widget> {ret, this._buildAvatar(message.author)}
                )
            );

            if (showTime || (message.id == this._lastReadMessageId && !isBottom)) {
                ret = new Column(
                    children: new List<Widget> {
                        showTime ? this._buildTime(message.time) : new Container(),
                        ret,
                        message.id == this._lastReadMessageId && !isBottom
                            ? this._buildUnreadMessageLine()
                            : new Container()
                    }
                );
            }

            return ret;
        }

        Widget _buildAvatar(User user) {
            const float avatarSize = 40;

            // fix Android 9 http request error 
            var httpsUrl = user.avatar.httpToHttps();

            return new Container(
                padding: EdgeInsets.symmetric(0, 10),
                child: new GestureDetector(
                    onTap: () => this.widget.actionModel.pushToUserDetail(user.id),
                    onLongPress: () => {
                        if (user.id == this.widget.viewModel.me.id) {
                            return;
                        }

                        var userName = user.fullName;
                        var userId = user.id;
                        var newContent = this._textController.text + "@" + userName + " ";
                        this.mentionMap[userName] = userId;
                        this._textController.value = new TextEditingValue(
                            text: newContent,
                            TextSelection.collapsed(newContent.Length)
                        );
                        if (!this._focusNode.hasFocus || !this.showKeyboard) {
                            FocusScope.of(this.context).requestFocus(this._focusNode);
                            TextInputPlugin.TextInputShow();
                            Promise.Delayed(TimeSpan.FromMilliseconds(200)).Then(
                                () => { this.setState(() => { this.showEmojiBoard = false; }); });
                        }
                    },
                    child: new Container(
                        width: avatarSize,
                        height: avatarSize,
                        color: CColors.Disable,
                        child: new Stack(
                            children: new List<Widget> {
                                user.avatar.isEmpty()
                                    ? new Container(
                                        padding: EdgeInsets.all(1.0f / Window.instance.devicePixelRatio),
                                        color: CColors.White,
                                        child: new _Placeholder(
                                            user.id ?? "",
                                            user.fullName ?? "",
                                            size: avatarSize
                                        )
                                    )
                                    : new Container(
                                        padding: EdgeInsets.all(1.0f / Window.instance.devicePixelRatio),
                                        color: CColors.White,
                                        child: CachedNetworkImageProvider.cachedNetworkImage(src: httpsUrl)
                                    ),
                                Positioned.fill(
                                    Image.asset(
                                        "image/avatar-circle-1",
                                        fit: BoxFit.cover
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildImageMessageContent(ChannelMessageView message) {
            return new GestureDetector(
                onTap: () => this._browserImage(imageUrl: message.content),
                child: new ImageMessage(
                    url: message.content,
                    size: 140,
                    ratio: 16.0f / 9.0f,
                    srcWidth: message.width,
                    srcHeight: message.height,
                    headers: this._headers
                )
            );
        }

        Widget _buildMessageContent(ChannelMessageView message) {
            switch (message.type) {
                case ChannelMessageType.text:
                    return new TextMessage(
                        message: message,
                        userId => this.widget.actionModel.pushToUserDetail(obj: userId)
                    );
                case ChannelMessageType.image:
                    return this._buildImageMessageContent(message);
                case ChannelMessageType.file:
                    return new FileMessage(
                        message: message,
                        () => this.widget.actionModel.openUrl(obj: message.attachments.first().url)
                    );
                case ChannelMessageType.embedExternal:
                case ChannelMessageType.embedImage:
                    return new EmbedMessage(
                        message: message,
                        userId => this.widget.actionModel.pushToUserDetail(obj: userId),
                        url => this.widget.actionModel.openUrl(obj: url),
                        onClickImage: this._browserImage,
                        headers: this._headers
                    );
                case ChannelMessageType.skip:
                    return new Container();
                default:
                    return new Container();
            }
        }

        Widget _buildTime(DateTime time) {
            return new Container(
                height: 36,
                padding: EdgeInsets.only(bottom: 16),
                child: new Center(
                    child: new Text(
                        time.DateTimeString(),
                        style: CTextStyle.PSmallBody5
                    )
                )
            );
        }

        Widget _buildUnreadMessageLine() {
            return new Row(
                crossAxisAlignment: CrossAxisAlignment.center,
                children: new List<Widget> {
                    new Expanded(
                        flex: 1,
                        child: new Container()
                    ),
                    new Container(
                        padding: EdgeInsets.only(top: 8, bottom: 16),
                        child: new Text("- 以下为新消息 -", style: CTextStyle.PSmallBody5.copyWith(height: 1))
                    ),
                    new Expanded(
                        flex: 1,
                        child: new Container()
                    )
                }
            );
        }

        Widget _buildInputBar() {
            var padding = this.showKeyboard || this.showEmojiBoard ? 0 : MediaQuery.of(this.context).padding.bottom;
            var customTextField = new CustomTextField(
                EdgeInsets.only(bottom: padding),
                new BoxDecoration(
                    border: new Border(new BorderSide(color: CColors.Separator)),
                    color: this.showEmojiBoard ? CColors.White : CColors.TabBarBg
                ),
                textFieldKey: this._textFieldKey,
                "说点想法…",
                controller: this._textController,
                focusNode: this._focusNode,
                maxLines: 4,
                minLines: 1,
                loading: false,
                showEmojiBoard: this.showEmojiBoard,
                isShowImageButton: true,
                onSubmitted: this._handleSubmit,
                onPressImage: this._pickImage,
                onPressEmoji: () => {
                    this._refreshController.scrollController.jumpTo(0);
                    FocusScope.of(context: this.context).requestFocus(node: this._focusNode);
                    if (this.showEmojiBoard) {
                        TextInputPlugin.TextInputShow();
                        Promise.Delayed(TimeSpan.FromMilliseconds(300)).Then(
                            onResolved: () => {
                                this._showEmojiBoard = false;
                                this._emojiBoardController.setValue(0);
                            }
                        );
                    }
                    else {
                        this.showEmojiBoard = true;
                        // If keyboard is present now, just hide it. If keyboard is not present now,
                        // it may pop out later (because of the focus), wait for a while and pop it
                        if (!this.showKeyboard) {
                            Promise.Delayed(TimeSpan.FromMilliseconds(100)).Then(
                                onResolved: TextInputPlugin.TextInputHide
                            );
                        }
                        else {
                            TextInputPlugin.TextInputHide();
                        }
                    }
                }
            );

            Widget backdropFilterWidget;
            if (!this.showEmojiBoard && !this.showKeyboard) {
                backdropFilterWidget = new BackdropFilter(
                    filter: ImageFilter.blur(10, 10),
                    child: customTextField
                );
            }
            else {
                backdropFilterWidget = customTextField;
            }

            return new Positioned(
                left: 0,
                right: 0,
                bottom: 0,
                child: backdropFilterWidget
            );
        }

        Widget _buildEmojiBoard() {
            return new ClipRect(
                child: new Align(
                    alignment: Alignment.topCenter,
                    heightFactor: this._emojiBoardController.value,
                    child: new EmojiBoard(
                        handleEmoji: this._handleEmoji,
                        handleDelete: this._handleDelete,
                        () => this._handleSubmit(text: this._textController.text)
                    )
                )
            );
        }

        int codeUnitLengthAt(TextEditingValue value) {
            return value.selection.start > 1 && char.IsSurrogate(value.text[value.selection.start - 1]) ? 2 : 1;
        }

        void _handleEmoji(string emojiText) {
            if (emojiText.isEmpty()) {
                return;
            }

            var selection = this._textController.selection;
            this._textController.value = new TextEditingValue(
                this._textController.text.Substring(0, length: selection.start) +
                emojiText + this._textController.text.Substring(startIndex: selection.end),
                TextSelection.collapsed(selection.start + emojiText.Length));
        }

        void _handleDelete() {
            var selection = this._textController.selection;
            if (selection.isCollapsed) {
                if (selection.start > 0) {
                    this._textController.value = new TextEditingValue(
                        text: this._textController.text.Substring(startIndex: 0,
                                  length: selection.start - this.codeUnitLengthAt(this._textController.value)) +
                              this._textController.text.Substring(selection.end),
                        TextSelection.collapsed(selection.start - this.codeUnitLengthAt(this._textController.value)));
                }
            }
            else {
                this._textController.value = new TextEditingValue(
                    this._textController.text.Substring(0, selection.start) +
                    this._textController.text.Substring(selection.end),
                    TextSelection.collapsed(selection.start));
            }
        }

        void _handleSubmit(string text) {
            var plainText = text.Trim();
            text = text.parseMention(replacements: this.mentionMap);
            if (string.IsNullOrWhiteSpace(text)) {
                CustomDialogUtils.showToast("不能发送空消息", iconData: Icons.error_outline);
                return;
            }

            var nonce = Snowflake.CreateNonce();

//            this.widget.actionModel.startSendMessage();
//            this.widget.actionModel.sendMessage(this.widget.viewModel.channel.id, text.Trim(), nonce, "")
//                .Catch(_ => CustomDialogUtils.showToast("消息发送失败", iconData: Icons.error_outline));
            this._refreshController.scrollTo(0);
            this.widget.actionModel.addLocalMessage(new ChannelMessageView {
                id = nonce,
                author = this.widget.viewModel.me,
                channelId = this.widget.viewModel.channel.id,
                nonce = nonce.hexToLong(),
                type = ChannelMessageType.text,
                content = text.Trim(),
                plainText = plainText,
                time = DateTime.UtcNow,
                status = "waiting"
            });
            this._textController.clear();
            this._textController.selection = TextSelection.collapsed(0);
            FocusScope.of(this.context).requestFocus(this._focusNode);
        }

        void _onRefresh(bool up) {
            if (!up) {
                string id = this.widget.viewModel.messages.isNotEmpty()
                    ? this.widget.viewModel.messages.first().id
                    : null;
                this.widget.actionModel.fetchMessages(arg1: id, null)
                    .Then(() => this._refreshController.sendBack(up: up,
                        up ? RefreshStatus.completed : RefreshStatus.idle))
                    .Catch(error => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed))
                    .Then(() => { Promise.Delayed(TimeSpan.FromMilliseconds(500)).Then(() => {
                            if (this._lastReadMessageId != null &&
                                this.calculateOffsetFromMessage(this._lastReadMessageId) < this._refreshController.offset + 10) {
                                this.showUnreadMessageNotification = false;
                            }
                        });
                    });
            }
        }

        float? _lastScrollPosition = null;

        const float bottomThreshold = 50;

        void _dismissKeyboard() {
            if (this._showEmojiBoard && !this.showKeyboard) {
                this.showEmojiBoard = false;
            }

            TextInputPlugin.TextInputHide();
        }

        void _handleScrollListener() {
            if (this._refreshController.offset <= bottomThreshold) {
                if (this._lastScrollPosition == null || this._lastScrollPosition > bottomThreshold) {
                    if (this.widget.viewModel.channel.newMessageIds.isNotEmpty()) {
                        float offset = 0;
                        for (int i = 0; i < this.widget.viewModel.newMessages.Count; i++) {
                            var message = this.widget.viewModel.newMessages[i];
                            offset += calculateMessageHeight(message,
                                showTime: i == 0
                                    ? message.time - this.widget.viewModel.messages.last().time >
                                      this._showTimeThreshold
                                    : message.time - this.widget.viewModel.newMessages[i - 1].time >
                                      this._showTimeThreshold,
                                this.messageBubbleWidth);
                        }

                        this._refreshController.scrollController.jumpTo(
                            this._refreshController.scrollController.offset + offset);
                    }

                    this.widget.actionModel.reportHitBottom();
                    this.showNewMessageNotification = false;
                }
            }
            else if (this._refreshController.offset > bottomThreshold) {
                if (this._lastScrollPosition == null || this._lastScrollPosition <= bottomThreshold) {
                    this.widget.actionModel.reportLeaveBottom();
                }
            }

            if (this._lastScrollPosition == null || this._lastScrollPosition < this._refreshController.offset) {
                if (this.showEmojiBoard || this.showKeyboard) {
                    this._dismissKeyboard();
                }
            }

            if (this._lastReadMessageId != null && this.showUnreadMessageNotification) {
                var index = this.widget.viewModel.messages.FindIndex(message => {
                    return message.id.hexToLong() > this._lastReadMessageId.hexToLong();
                });
                if (index > 0 && this.calculateOffsetFromIndex(index) < this._refreshController.offset + 10) {
                    this.showUnreadMessageNotification = false;
                }
            }

            this._lastScrollPosition = this._refreshController.offset;
        }

        void _pickImage() {
            var items = new List<ActionSheetItem> {
                new ActionSheetItem(
                    "拍照",
                    onTap: () => PickImagePlugin.PickImage(
                        source: ImageSource.camera,
                        imageCallBack: this._pickImageCallback,
                        false
                    )
                ),
                new ActionSheetItem(
                    "从相册选择",
                    onTap: () => PickImagePlugin.PickImage(
                        source: ImageSource.gallery,
                        imageCallBack: this._pickImageCallback,
                        false
                    )
                ),
                new ActionSheetItem("取消", type: ActionType.cancel)
            };

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                title: "发送图片",
                items: items
            ));
        }

        void _pickImageCallback(string pickImage) {
            var nonce = Snowflake.CreateNonce();
            this._refreshController.scrollTo(0);
            this.widget.actionModel.addLocalMessage(new ChannelMessageView {
                id = nonce,
                author = this.widget.viewModel.me,
                channelId = this.widget.viewModel.channel.id,
                nonce = nonce.hexToLong(),
                type = ChannelMessageType.image,
                content = pickImage,
                time = DateTime.UtcNow,
                status = "waiting"
            });
        }

        public void didPop() {
            this.mentionMap.Clear();
            if (this._focusNode.hasFocus) {
                this._focusNode.unfocus();
            }
            this.widget.actionModel.popFromScreen();
        }

        public void didPopNext() {
            StatusBarManager.statusBarStyle(false);
        }

        public void didPush() {
        }

        public void didPushNext() {
            if (this._focusNode.hasFocus) {
                this._focusNode.unfocus();
            }
        }

        static float calculateMessageHeight(ChannelMessageView message, bool showTime, float width) {
            float height = 20 + 6 + 16 + (showTime ? 36 : 0); // Name + Internal + Bottom padding + time
            switch (message.type) {
                case ChannelMessageType.text:
                    height += TextMessage.CalculateTextHeight(content: message.content, width: width);
                    break;
                case ChannelMessageType.image:
                    height += ImageMessage.CalculateTextHeight(message: message);
                    break;
                case ChannelMessageType.file:
                    height += FileMessage.CalculateTextHeight(message: message, width: width);
                    break;
                case ChannelMessageType.embedExternal:
                case ChannelMessageType.embedImage:
                    height += EmbedMessage.CalculateTextHeight(message: message, width: width);
                    break;
            }

            return height;
        }
    }
}