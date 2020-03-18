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
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using Config = ConnectApp.Constants.Config;
using Image = Unity.UIWidgets.widgets.Image;
using Transform = Unity.UIWidgets.widgets.Transform;

namespace ConnectApp.screens {
    public class  ChannelScreenConnector : StatelessWidget {
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
                    else {
                        var dbMessages = MessengerDBApi.SyncLoadMessages(channelId: this.channelId);
                        dbMessages.Reverse();
                        messages = dbMessages;
                    }

                    var channelInfoLoading = false;
                    var channelMessageLoading = false;
                    if (state.channelState.channelInfoLoadingDict.TryGetValue(this.channelId, out var infoLoading)) {
                        channelInfoLoading = infoLoading;
                    }

                    if (state.channelState.channelMessageLoadingDict.TryGetValue(this.channelId, out var messageLoading)
                    ) {
                        channelMessageLoading = messageLoading;
                    }

                    return new ChannelScreenViewModel {
                        hasChannel = state.channelState.channelDict.ContainsKey(this.channelId),
                        channelInfoLoading = channelInfoLoading,
                        channel = channel,
                        channelId = this.channelId,
                        messages = messages,
                        newMessages = newMessages,
                        me = new User {
                            id = state.loginState.loginInfo.userId,
                            avatar = state.loginState.loginInfo.userAvatar,
                            fullName = state.loginState.loginInfo.userFullName
                        },
                        messageLoading = channelMessageLoading,
                        socketConnected = state.channelState.socketConnected,
                        networkConnected = state.networkState.networkConnected,
                        dismissNoNetworkBanner = state.networkState.dismissNoNetworkBanner,
                        mentionAutoFocus = state.channelState.mentionAutoFocus,
                        mentionUserId = state.channelState.mentionUserId,
                        mentionUserName = state.channelState.mentionUserName,
                        mentionSuggestion = state.channelState.mentionSuggestions.getOrDefault(this.channelId, null),
                        waitingMessage = waitingMessage,
                        sendingMessage = sendingMessage,
                        userLicenseDict = state.userState.userLicenseDict
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

                    if (viewModel.channel.needFetchMessages) {
                        SchedulerBinding.instance.addPostFrameCallback(_ => {
                            dispatcher.dispatch<IPromise>(
                                Actions.fetchChannelMessages(channelId: this.channelId));
                        });
                    }

                    if (viewModel.waitingMessage != null && viewModel.sendingMessage == null) {
                        SchedulerBinding.instance.addPostFrameCallback(_ => {
                            dispatcher.dispatch(new StartSendChannelMessageAction {
                                message = viewModel.waitingMessage
                            });
                            if (CTemporaryValue.lastWaitingMessageId.isNotEmpty() &&
                                viewModel.waitingMessage.id == CTemporaryValue.lastWaitingMessageId) {
                                return;
                            }

                            CTemporaryValue.lastWaitingMessageId = viewModel.waitingMessage.id;
                            if (viewModel.waitingMessage.type == ChannelMessageType.text) {
                                dispatcher.dispatch<IPromise>(Actions.sendChannelMessage(
                                    this.channelId,
                                    viewModel.waitingMessage.content,
                                    nonce: viewModel.waitingMessage.id,
                                    parentMessageId: ""));
                            }
                            else if (viewModel.waitingMessage.type == ChannelMessageType.file) {
                                dispatcher.dispatch<IPromise>(Actions.sendVideo(
                                    channelId: this.channelId,
                                    nonce: viewModel.waitingMessage.id,
                                    videoData: viewModel.waitingMessage.videoData,
                                    fileName: viewModel.waitingMessage.attachments.first().filename)
                                );
                            }
                            else {
                                dispatcher.dispatch<IPromise>(Actions.sendImage(
                                    channelId: this.channelId,
                                    nonce: viewModel.waitingMessage.id,
                                    imageData: viewModel.waitingMessage.imageData)
                                );
                            }
                        });
                    }

                    var actionModel = new ChannelScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        openUrl = url => OpenUrlUtil.OpenUrl(url: url, dispatcher: dispatcher),
                        browserImage = (url, imageUrls, imageData) => dispatcher.dispatch(
                            new MainNavigatorPushToPhotoViewAction {
                            url = url,
                            urls = imageUrls,
                            imageData = imageData
                        }),
                        playVideo = url => {
                            dispatcher.dispatch(new MainNavigatorPushToVideoPlayerAction {
                                url = url,
                                needUpdate = false,
                                limitSeconds = 0
                            });
                        },
                        fetchChannelInfo = () => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelInfo(channelId: this.channelId)),
                        fetchMessages = (before, after) => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMessages(channelId: this.channelId, before: before, after: after)),
                        fetchMembers = () => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMembers(channelId: this.channelId)),
                        fetchMember = () => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMember(channelId: this.channelId, userId: viewModel.me.id)),
                        deleteChannelMessage = messageId => dispatcher.dispatch<IPromise>(
                            Actions.deleteChannelMessage(channelId: this.channelId, messageId: messageId)),
                        deleteLocalMessage = message => dispatcher.dispatch(new DeleteLocalMessageAction {
                            message = message
                        }),
                        pushToChannelDetail = () => dispatcher.dispatch(new MainNavigatorPushToChannelDetailAction {
                            channelId = this.channelId
                        }),
                        pushToUserDetail = userId => dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {
                            userId = userId
                        }),
                        pushToReactionsDetail = messageId => dispatcher.dispatch(new MainNavigatorPushToReactionsDetailAction {
                            messageId = messageId
                        }),
                        sendMessage = (channelId, content, nonce, parentMessageId) => dispatcher.dispatch<IPromise>(
                            Actions.sendChannelMessage(channelId, content, nonce, parentMessageId)),
                        ackMessage = () =>
                            dispatcher.dispatch(Actions.ackChannelMessage(viewModel.channel.lastMessageId)),
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
                            dispatcher.dispatch(new MergeNewChannelMessages {channelId = this.channelId});
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
                        }),
                        selectReactionFromMe = (message, type) => {
                            MyReactionsManager.updateMyReaction(message: message, type: type);
                            dispatcher.dispatch(new UpdateMyReactionToMessage {messageId = message.id});
                        },
                        cancelReactionFromMe = (message, type) => {
                            MyReactionsManager.updateMyReaction(message: message, type: type);
                            dispatcher.dispatch(new UpdateMyReactionToMessage {messageId = message.id});
                        }
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
            {"ConnectAppVersion", Config.versionName},
            {"X-Requested-With", "XmlHttpRequest"}
        };

        public static readonly Dictionary<string, string> reactionStaticIcons = new Dictionary<string, string> {
            {"thumb", "image/reaction-thumb"},
            {"oppose", "image/reaction-oppose"},
            {"coverface", "image/reaction-coverface"},
            {"heartbeat", "image/reaction-heartbeat"},
            {"doubt", "image/reaction-doubt"},
        };

        bool _showEmojiBoard;
        string _lastMessageEditingContent = "";
        readonly Dictionary<string, string> mentionMap = new Dictionary<string, string>();
        string _lastReadMessageId = null;
        string _lastMessageWhenScreenIsOpened = null;
        AnimationController _unreadNotificationController;
        AnimationController _newMessageNotificationController;
        AnimationController _emojiBoardController;
        AnimationController _viewInsetsBottomController;
        Animation<float> _viewInsetsBottomAnimation;
        AnimationController _messageActivityIndicatorController;
        AnimationController _reactionAppearAnimationController;

        bool _showUnreadMessageNotification = false;
        bool _showNewMessageNotification = false;
        bool _showReactionOverlay = false;
        float _inputFieldHeight;
        CustomTextField customTextField;
        Dictionary<string, GlobalKey> _fullMessageKeys;
        Dictionary<string, GlobalKey> _messageBubbleKeys;
        string _animatingMessageReaction;
        string _animationMessageReactionType;
        float _bottomPaddingWhenShowingPopupBar;

        GlobalKey _getMessageKey(string id) {
            GlobalKey key;
            if (!this._messageBubbleKeys.TryGetValue(id, out key)) {
                key = GlobalKey.key(id);
                this._messageBubbleKeys[id] = key;
            }

            return key;
        }

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
            get { return this._showEmojiBoard || this._emojiBoardController.value > 0.01f; }

            set {
                if (this._showEmojiBoard == value) {
                    return;
                }

                this._showEmojiBoard = value;
                if (value) {
                    this._emojiBoardController.animateTo(1);
                }
                else {
                    this._emojiBoardController.animateBack(0, TimeSpan.FromMilliseconds(200));
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
                    this._viewInsetsBottomController.animateTo(1).Finally(() => {
                        if (target > 50 && this._showEmojiBoard) {
                            this._showEmojiBoard = false;
                            this._emojiBoardController.setValue(0);
                        }
                    });
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
                    this.widget.actionModel.clearUnread();
                }
                else {
                    this.widget.actionModel.fetchChannelInfo().Then(() => {
                        this.fetchMessagesAndMembers();
                        this.addScrollListener();
                        this.widget.actionModel.clearUnread();
                    });
                }
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
            this._unreadNotificationController.addListener(() => { this.setState(() => { }); });
            this._newMessageNotificationController.addListener(() => { this.setState(() => { }); });
            this._emojiBoardController.addListener(() => { this.setState(() => { }); });
            this._viewInsetsBottomController.addListener(() => { this.setState(() => { }); });
            this._messageActivityIndicatorController = new AnimationController(
                duration: new TimeSpan(0, 0, 2),
                vsync: this);
            this._reactionAppearAnimationController =
                new AnimationController(duration: TimeSpan.FromMilliseconds(1000), vsync: this);
            this._reactionAppearAnimationController.addListener(() => { this.setState(() => { }); });
            this._messageBubbleKeys = new Dictionary<string, GlobalKey>();
            this._fullMessageKeys = new Dictionary<string, GlobalKey>();
        }

        bool _onMessageLoadedCalled = false;

        void _startIndicator() {
            if (!this._messageActivityIndicatorController.isAnimating) {
                this._messageActivityIndicatorController.repeat();
            }
        }

        void _stopIndicator() {
            if (this._messageActivityIndicatorController.isAnimating) {
                this._messageActivityIndicatorController.stop();
            }
        }

        void _onMessageLoaded() {
            if (this._onMessageLoadedCalled) {
                return;
            }

            this._onMessageLoadedCalled = true;
            if (this._lastMessageWhenScreenIsOpened == null) {
                this._lastMessageWhenScreenIsOpened = this.widget.viewModel.messages.last().id;
            }

            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.showUnreadMessageNotification = this._lastReadMessageId != null &&
                                                     this.calculateOffsetFromMessage(this._lastReadMessageId) > 0;
            });
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
            var index = this.widget.viewModel.messages.FindIndex(message =>
                message.id.hexToLong() > id.hexToLong());
            if (index >= 0) {
                this.jumpToIndex(index);
            }
        }

        float calculateTotalHeightFromMessage(string id) {
            var index = this.widget.viewModel.messages.FindIndex(message =>
                message.id.hexToLong() > id.hexToLong());
            if (index >= 0) {
                return this.calculateMessageHeightFromIndex(index);
            }

            return 0;
        }

        float calculateOffsetFromMessage(string id) {
            var index = this.widget.viewModel.messages.FindIndex(message =>
                message.id.hexToLong() > id.hexToLong());
            if (index >= 0) {
                return this.calculateOffsetFromIndex(index);
            }

            return 0;
        }

        float calculateMessageHeightFromIndex(int index) {
            float height = 0;
            for (int i = index; i < this.widget.viewModel.messages.Count; i++) {
                var message = this.widget.viewModel.messages[i];
                height += this.calculateMessageHeight(message,
                    i == 0 || message.time - this.widget.viewModel.messages[i - 1].time > this._showTimeThreshold,
                    this.messageBubbleWidth);
            }

            return height;
        }

        float calculateOffsetFromIndex(int index) {
            return this.calculateMessageHeightFromIndex(index) -
                   (MediaQuery.of(this.context).size.height - CustomAppBarUtil.appBarHeight - 120);
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

        GlobalKey getFullMessageKey(string messageId) {
            GlobalKey key;
            if (!this._fullMessageKeys.TryGetValue(messageId, out key)) {
                key = GlobalKey.key("full" + messageId);
                this._fullMessageKeys[messageId] = key;
            }

            return key;
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
            this._messageActivityIndicatorController.dispose();
            this._reactionAppearAnimationController.dispose();
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
                        () => {
                            if (message.status == "normal") {
                                this.widget.actionModel.deleteChannelMessage(message.id);
                            }
                            else {
                                this.widget.actionModel.deleteLocalMessage(message);
                            }
                        }),
                    new ActionSheetItem("取消", type: ActionType.cancel)
                }
            ));
        }

        void _browserImage(string imageUrl) {
            var imageUrls = new List<string>();
            var imageData = new Dictionary<string, byte[]>();
            this.widget.viewModel.messages.ForEach(msg => {
                if (msg.type == ChannelMessageType.image) {
                    var sizedUrl = CImageUtils.SizeToScreenImageUrl(imageUrl: msg.content);
                    imageUrls.Add(sizedUrl);
                    if (msg.imageData != null) {
                        imageData[sizedUrl] = msg.imageData;
                    }
                }
            });
            var url = CImageUtils.SizeToScreenImageUrl(imageUrl: imageUrl);
            this.widget.actionModel.browserImage(arg1: url, arg2: imageUrls, arg3: imageData);
        }

        public override Widget build(BuildContext context) {
            if (this.widget.viewModel.channel.lastMessageId == this._lastReadMessageId) {
                this._lastReadMessageId = null;
            }

            if (this.widget.viewModel.channel.needFetchMessages) {
                SchedulerBinding.instance.addPostFrameCallback(_ => { this._refreshController.scrollTo(0); });
            }

            if (this.widget.viewModel.waitingMessage != null ||
                this.widget.viewModel.sendingMessage != null) {
                this._startIndicator();
            }
            else {
                this._stopIndicator();
            }

            this.showNewMessageNotification = this.widget.viewModel.newMessages.Count > 0;

            if (this.widget.viewModel.mentionAutoFocus) {
                SchedulerBinding.instance.addPostFrameCallback(_ => {
                    FocusScope.of(context).requestFocus(this._focusNode);
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
                    new Container(
                        child: this._buildContent(),
                        padding: EdgeInsets.only(bottom: this._bottomPaddingWhenShowingPopupBar)
                    ),
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
                                !this.widget.viewModel.dismissNoNetworkBanner
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
                        "网络连接不可用，请检查你的网络设置",
                        style: CTextStyle.PRegularError.copyWith(height: 1f)
                    )
                )
            );
        }

        int _newMessageCount = 0;

        Widget _buildNewMessageNotification() {
            if (this.widget.viewModel.newMessages.Count > 0) {
                this._newMessageCount = this.widget.viewModel.newMessages.Count;
            }

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
                            $"{CStringUtils.CountToString(this._newMessageCount, "0")}条新消息未读",
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

            var lastMessageIndex =
                this.widget.viewModel.messages.FindIndex(message => message.id == this._lastMessageWhenScreenIsOpened);
            if (lastMessageIndex == -1) {
                lastMessageIndex = this.widget.viewModel.messages.Count - 1;
            }

            Widget ret = new Container(
                height: 40,
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: Border.all(color: CColors.Separator2),
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
                            $"{lastMessageIndex - index + 1}" +
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
                            if (index == 0 && this.widget.viewModel.channel.hasMore) {
                                this._refreshController.requestRefresh(false);
                                SchedulerBinding.instance.addPostFrameCallback(_ => {
                                    this._refreshController.scrollTo(this.calculateOffsetFromIndex(index) + 100);
                                });
                            }
                            else {
                                this.jumpToLastReadMessage();
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
                                    !this.widget.viewModel.networkConnected
                                        ? this.widget.viewModel.channel.name + " (未连接)"
                                        : this.widget.viewModel.socketConnected &&
                                          !this.widget.viewModel.channel.needFetchMessages &&
                                          !this.widget.viewModel.messageLoading
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
                    width: 58,
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
            ListView listView = this._buildMessageListView();
            var enablePull = true;
            if (!this.widget.viewModel.channel.joined &&
                this.widget.viewModel.networkConnected) {
                listView = this._buildNotJoinPage();
                enablePull = false;
            }

            if ((this.widget.viewModel.messageLoading &&
                 this.widget.viewModel.messages.isEmpty()) ||
                (this.widget.viewModel.channel.id == null && this.widget.viewModel.channelInfoLoading)) {
                listView = this._buildLoadingPage();
                enablePull = false;
            }

            Widget content = new Container(
                color: CColors.White,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        key: this._smartRefresherKey,
                        controller: this._refreshController,
                        enablePullDown: false,
                        enablePullUp: enablePull && this.widget.viewModel.channel.hasMore,
                        onRefresh: this._onRefresh,
                        reverse: true,
                        headerBuilder: (context, mode) => new SmartRefreshHeader(mode: mode),
                        child: listView
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
                        showUnreadLine: index < this.widget.viewModel.messages.Count - 1 &&
                                        this._lastReadMessageId != null &&
                                        message.id.hexToLong() <= this._lastReadMessageId.hexToLong() &&
                                        this.widget.viewModel.messages[index + 1].id.hexToLong() >
                                        this._lastReadMessageId.hexToLong()
                    );
                }
            );
        }

        ListView _buildLoadingPage() {
            return new ListView(
                children: new List<Widget> {
                    new Container(
                        padding: EdgeInsets.only(
                            top: 44 + MediaQuery.of(this.context).padding.top,
                            bottom: 49 + MediaQuery.of(this.context).padding.bottom
                        ),
                        height: MediaQuery.of(this.context).size.height,
                        child: new Center(
                            child: new GlobalLoading()
                        )
                    )
                });
        }

        ListView _buildNotJoinPage() {
            return new ListView(
                children: new List<Widget> {
                    new Container(
                        padding: EdgeInsets.only(
                            top: 44 + MediaQuery.of(this.context).padding.top,
                            bottom: 49 + MediaQuery.of(this.context).padding.bottom
                        ),
                        height: MediaQuery.of(this.context).size.height,
                        child: new Center(child: new Text("你已不在该群组", style: CTextStyle.PLargeBody.copyWith(height: 1))))
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


        List<TipMenuItem> _buildTipMenus(ChannelMessageView message, bool showDeleteButton) {
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

            if (message.type == ChannelMessageType.text) {
                tipMenuItems.Add(new TipMenuItem(
                    "引用",
                    () => {
                        var content = MessageUtils.AnalyzeMessage(
                            content: message.content,
                            mentions: message.mentions,
                            mentionEveryone: message.mentionEveryone
                        );
                        var newContent = this._textController.text + "「 " + message.author.fullName + ": " + content +
                                         " 」" + "\n" + "- - - - - - - - - - - - - - -" + "\n";
                        this._textController.value = new TextEditingValue(
                            text: newContent,
                            TextSelection.collapsed(offset: newContent.Length)
                        );
                        if (this._refreshController.scrollController.offset > 0) {
                            this._refreshController.scrollController.animateTo(0, TimeSpan.FromMilliseconds(100),
                                curve: Curves.linear);
                        }

                        if (!this._focusNode.hasFocus || !this.showKeyboard) {
                            FocusScope.of(context: this.context).requestFocus(node: this._focusNode);
                            TextInputPlugin.TextInputShow();
                            Promise.Delayed(TimeSpan.FromMilliseconds(200)).Then(
                                () => {
                                    if (this.mounted) {
                                        this.setState(() => this.showEmojiBoard = false);
                                    }
                                });
                        }
                    }
                ));
            }

            if (message.author.id == this.widget.viewModel.me.id
                && showDeleteButton
                && message.type != ChannelMessageType.deleted) {
                tipMenuItems.Add(new TipMenuItem(
                    "删除",
                    () => this._deleteMessage(message: message)
                ));
            }

            return tipMenuItems;
        }

        Widget _buildMessageTitle(ChannelMessageView message) {
            return new Container(
                padding: EdgeInsets.only(left: 0, right: 16, bottom: 6),
                child: new Row(
                    children: new List<Widget> {
                        new Flexible(
                            child: new Text(
                                data: message.author.fullName,
                                style: CTextStyle.PSmallBody4,
                                maxLines: 1,
                                overflow: TextOverflow.ellipsis
                            )
                        ),
                        CImageUtils.GenBadgeImage(
                            badges: message.author.badges,
                            CCommonUtils.GetUserLicense(userId: message.author.id,
                                userLicenseMap: this.widget.viewModel.userLicenseDict),
                            EdgeInsets.only(4),
                            false
                        )
                    }
                )
            );
        }

        Widget _buildMessageStateIndicator(ChannelMessageView message) {
            if (message.status == "normal" || message.status == "local") {
                return new Container();
            }

            return message.status == "sending" || message.status == "waiting"
                ? (Widget) new CustomActivityIndicator(
                    size: LoadingSize.small,
                    controller: this._messageActivityIndicatorController)
                : new GestureDetector(
                    onTap: () => { this.widget.actionModel.resendMessage(message); },
                    child: new Icon(icon: Icons.error, color: CColors.Error, size: 24)
                );
        }

        List<ActionSheetItem> _buildMessageActionSheet(ChannelMessageView message, bool showDeleteButton) {
            var list = new List<ActionSheetItem>();

            if (message.canCopy()) {
                list.Add(new ActionSheetItem(
                    "复制",
                    onTap: () => {
                        var content = MessageUtils.AnalyzeMessage(
                            content: message.content,
                            mentions: message.mentions,
                            mentionEveryone: message.mentionEveryone
                        );
                        Clipboard.setData(new ClipboardData(text: content));
                    }
                ));
                list.Add(new ActionSheetItem(
                    "引用",
                    onTap: () => {
                        var content = MessageUtils.AnalyzeMessage(
                            content: message.content,
                            mentions: message.mentions,
                            mentionEveryone: message.mentionEveryone
                        );
                        var newContent = this._textController.text + "「 " + message.author.fullName + ": " +
                                         content +
                                         " 」" + "\n" + "- - - - - - - - - - - - - - -" + "\n";
                        this._textController.value = new TextEditingValue(
                            text: newContent,
                            TextSelection.collapsed(offset: newContent.Length)
                        );
                        if (this._refreshController.scrollController.offset > 0) {
                            this._refreshController.scrollController.animateTo(0, TimeSpan.FromMilliseconds(100),
                                curve: Curves.linear);
                        }

                        if (!this._focusNode.hasFocus || !this.showKeyboard) {
                            FocusScope.of(context: this.context).requestFocus(node: this._focusNode);
                            TextInputPlugin.TextInputShow();
                            Promise.Delayed(TimeSpan.FromMilliseconds(200)).Then(
                                () => {
                                    if (this.mounted) {
                                        this.setState(() => this.showEmojiBoard = false);
                                    }
                                });
                        }
                    }
                ));
            }

            if (showDeleteButton) {
                list.Add(
                    new ActionSheetItem(
                        "删除",
                        onTap: () => this._deleteMessage(message: message)
                    )
                );
            }

            return list;
        }

        Widget _buildReactionOverlay(ChannelMessageView message, Offset offset, Size size, bool left) {
            return new _ReactionOverlay(
                message: message,
                size: size,
                offset: offset,
                left: left,
                userId: this.widget.viewModel.me.id,
                messageBubble: new Container(
                    constraints: new BoxConstraints(
                        maxWidth: this.messageBubbleWidth
                    ),
                    decoration: this._messageDecoration(message.type, left),
                    child: this._buildMessageContent(message: message)
                ),
                onTap: type => {
                    if (message.isReactionSelectedByLocalAndServer(type)) {
                        if (message.isOnlyMeSelected()) {
                            this._animatingMessageReaction = message.id;
                            this._animationMessageReactionType = type;
                            this._reactionAppearAnimationController.reset();
                            this._reactionAppearAnimationController.setValue(1);
                        }
                        this.widget.actionModel.cancelReactionFromMe(message, type);
                    } else {
                        if (message.isReactionsEmpty()) {
                            this._animatingMessageReaction = message.id;
                            this._animationMessageReactionType = type;
                            this._reactionAppearAnimationController.reset();
                            this._reactionAppearAnimationController.setValue(0);
                        }
                        this.widget.actionModel.selectReactionFromMe(message, type);
                    }
                    ActionSheetUtils.hiddenModalPopup();
                });
        }

        Widget _buildReactionBox(string type, bool selected, int count) {
            return new Container(
                height: 28,
                padding: EdgeInsets.symmetric(4, 8),
                decoration: new BoxDecoration(
                    border: Border.all(
                        selected
                            ? CColors.PrimaryBlue
                            : CColors.Transparent,
                        1.5f
                    ),
                    borderRadius: BorderRadius.all(14),
                    color: CColors.MessageReaction
                ),
                child: new Row(
                    mainAxisSize: MainAxisSize.min,
                    children: new List<Widget> {
                        Image.asset(reactionStaticIcons[type], width: 20, height: 20),
                        new SizedBox(width: 4),
                        new Text(
                            $"{CStringUtils.CountToString(count)}",
                            style: selected
                                ? CTextStyle.PRegularBody.copyWith(color: CColors.MessageReactionCount,
                                    height: 1.1f)
                                : CTextStyle.PRegularBody.copyWith(height: 1.1f))
                    }
                )
            );
        }
        
        Widget _buildReaction(ChannelMessageView message, string type) {
            return new GestureDetector(
                onTap: () => {
                    if (message.isReactionSelectedByLocalAndServer(type)) {
                        this.widget.actionModel.cancelReactionFromMe(message, type);
                    }
                    else {
                        this.widget.actionModel.selectReactionFromMe(message, type);
                    }
                },
                child: this._buildReactionBox(
                    type,
                    message.isReactionSelectedByLocalAndServer(type),
                    message.adjustedReactionCount(type)
                )
            );
        }

        Widget _buildReactionBar(ChannelMessageView message, bool left) {
            if (message.isReactionsEmpty() && this._animatingMessageReaction != message.id) {
                return new Container();
            }

            message.fillReactionsCountDict();
            var reactionsCountItem = message.reactionsCountDict.Where(pair => 
                pair.Value > 0 || message.isReactionSelectedByLocalAndServer(pair.Key)).ToArray();
            
            Widget result = new Container(
                padding: EdgeInsets.only(top: 8),
                child: new Wrap(
                    alignment: left ? WrapAlignment.start : WrapAlignment.end,
                    spacing: 8,
                    runSpacing: 8,
                    children: reactionsCountItem
                        .Select(pair => this._buildReaction(message, pair.Key))
                        .ToList()
                )
            );

            if (this._animatingMessageReaction == message.id) {
                if (this._reactionAppearAnimationController.status == AnimationStatus.forward) {
                    if (this._reactionAppearAnimationController.value < 0.25f) {
                        result = new SizedBox(height: this._reactionAppearAnimationController.value * 36);
                    }
                    else {
                        var imageTranslation = Matrix3.makeTrans(
                            (this._reactionAppearAnimationController.value - 0.75f).clamp(0, 0.25f) / 0.25f * 8,
                            this._reactionAppearAnimationController.value < 0.5f
                                ? (this._reactionAppearAnimationController.value - 0.25f) / 0.25f * (-50) - 52
                                : this._reactionAppearAnimationController.value < 0.75f
                                    ? ((this._reactionAppearAnimationController.value - 0.5f) / 0.15f).clamp(0, 1) *
                                      50 - 102
                                    : (this._reactionAppearAnimationController.value - 0.75f) / 0.25f * 52 - 52
                        );
                        var imageScale = Matrix3.makeScale(
                            sx: this._reactionAppearAnimationController.value < 0.75f
                                ? 2.2f
                                : (this._reactionAppearAnimationController.value - 0.75f) / 0.25f * (-1.2f) + 2.2f,
                            sy: this._reactionAppearAnimationController.value < 0.65f
                                ? 2.2f
                                : this._reactionAppearAnimationController.value < 0.7f
                                    ? ((this._reactionAppearAnimationController.value - 0.65f) / 0.05f * (-0.1f) +
                                       1.0f) * 2.2f
                                    : this._reactionAppearAnimationController.value < 0.75f
                                        ? ((this._reactionAppearAnimationController.value - 0.7f) / 0.05f * 0.1f +
                                           0.9f) * 2.2f
                                        : (this._reactionAppearAnimationController.value - 0.75f) / 0.25f * (-1.2f) +
                                          2.2f
                        );
                        var imageTransform = imageScale;
                        imageTransform.postConcat(imageTranslation);
                        result = new Stack(children: new List<Widget> {
                            new Opacity(
                                opacity: (this._reactionAppearAnimationController.value / 0.25f - 3).clamp(0, 1),
                                child: result
                            ),
                            new Transform(
                                transform: Matrix3.makeTrans(
                                    60 * (2 - this._reactionAppearAnimationController.value / 0.25f).clamp(0, 1),
                                    0),
                                child: new Opacity(
                                    opacity: ((1.0f - this._reactionAppearAnimationController.value) / 0.25f).clamp(0,
                                        1),
                                    child: new Container(
                                        height: 28,
                                        width: 51,
                                        padding: EdgeInsets.symmetric(4, 8),
                                        decoration: new BoxDecoration(
                                            borderRadius: BorderRadius.all(14),
                                            color: CColors.PrimaryBlue
                                        ),
                                        child: new Center(
                                            child: new Text("+1", style: CTextStyle.PSmallWhite)
                                        )
                                    )
                                )
                            ),
                            new Transform(
                                transform: imageTransform,
                                child: new Opacity(
                                    opacity: ((this._reactionAppearAnimationController.value - 0.25f) / 0.25f).clamp(0,
                                        1),
                                    child: Image.asset(
                                        reactionStaticIcons[reactionsCountItem.Single().Key],
                                        width: 20,
                                        height: 20
                                    )
                                )
                            )
                        });
                    }
                }
                else if(this._reactionAppearAnimationController.status == AnimationStatus.reverse) {
                    result = new Container(
                        padding: EdgeInsets.only(top: 8),
                        child: new Wrap(
                            alignment: left ? WrapAlignment.start : WrapAlignment.end,
                            spacing: 8,
                            children: new List<Widget> {
                                this._buildReactionBox(this._animationMessageReactionType, true, 1)
                            }
                        )
                    );
                    result = new SizedBox(
                        height: this._reactionAppearAnimationController.value * 36,
                        child: new Opacity(
                            opacity: this._reactionAppearAnimationController.value,
                            child: result
                        )
                    );
                }
            }

            return result;
        }

        void _onMessageLongPress(ChannelMessageView message, bool left) {
            if (message.status != "normal") {
                return;
            }

            if (this.showKeyboard || this.showEmojiBoard) {
                this._dismissKeyboard();
                Promise.Delayed(TimeSpan.FromMilliseconds(200)).Then(() => {
                    this._onMessageLongPress(message, left);
                });
                return;
            }

            var renderBox = (RenderBox) this._getMessageKey(message.id).currentContext.findRenderObject();
            var messageBoxSize = renderBox.size;
            var originalMessageBoxOffset = renderBox.localToGlobal(Offset.zero);
            var items = this._buildMessageActionSheet(message, message.author.id == this.widget.viewModel.me.id && message.type != ChannelMessageType.deleted);
            var messageBoxOffset = new Offset(
                originalMessageBoxOffset.dx,
                originalMessageBoxOffset.dy
                    .clamp(float.NegativeInfinity,
                        MediaQuery.of(this.context).size.height -
                        MediaQuery.of(this.context).padding.bottom - 8 -
                        items.Count * 50 - messageBoxSize.height)
                    .clamp(60 + CCommonUtils.getSafeAreaTopPadding(this.context),
                        float.PositiveInfinity));
            var offsetDiff = messageBoxOffset - originalMessageBoxOffset;
            var originalScrollOffset = this._refreshController.offset;
            if (offsetDiff.dy > 0) {
                this._refreshController.animateTo(
                    this._refreshController.offset + offsetDiff.dy,
                    TimeSpan.FromMilliseconds(100),
                    Curves.linear);
            }
            else if (offsetDiff.dy < 0) {
                this.setState(() => { this._bottomPaddingWhenShowingPopupBar = -offsetDiff.dy; });
            }

            this.widget.actionModel.reportLeaveBottom();
            this._showReactionOverlay = true;
            ActionSheetUtils.showModalActionSheet(
                child: new ActionSheet(items: items),
                overlay: this._buildReactionOverlay(message, messageBoxOffset, messageBoxSize, left),
                onPop: () => {
                    if (offsetDiff.dy > 0) {
                        this._refreshController.animateTo(
                            originalScrollOffset,
                            TimeSpan.FromMilliseconds(100),
                            Curves.linear);
                    }
                    else if (offsetDiff.dy < 0) {
                        this.setState(() => { this._bottomPaddingWhenShowingPopupBar = 0; });
                    }

                    if (this._animatingMessageReaction != null) {
                        (this._reactionAppearAnimationController.value < 0.5f
                                ? this._reactionAppearAnimationController.animateTo(1)
                                : this._reactionAppearAnimationController.animateBack(
                                    0,
                                    duration: TimeSpan.FromMilliseconds(250),
                                    curve: Curves.easeInQuart))
                            .whenCompleteOrCancel(() => {
                                this.setState(
                                    () => {
                                        this._animatingMessageReaction = null;
                                    }
                                );
                            });
                    }

                    if (this._refreshController.offset < bottomThreshold) {
                        this.widget.actionModel.reportHitBottom();
                    }
                    this._showReactionOverlay = false;
                }
            );
        }

        Widget _buildMessage(ChannelMessageView message, bool showTime, bool left, bool showUnreadLine = false) {
            if (message.type == ChannelMessageType.skip || message.type == ChannelMessageType.deleted) {
                return new Container();
            }

            Widget ret = new Container(
                key: this._getMessageKey(message.id),
                constraints: new BoxConstraints(
                    maxWidth: this.messageBubbleWidth
                ),
                decoration: this._messageDecoration(message.type, left),
                child: this._buildMessageContent(message: message)
            );

            bool normalOrLocalMessage = message.status == "normal" || message.status == "local";
            if (!normalOrLocalMessage) {
                ret = new Row(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    mainAxisSize: MainAxisSize.min,
                    children: new List<Widget> {
                        this._buildMessageStateIndicator(message),
                        new SizedBox(width: 8),
                        ret
                    }
                );
            }

            ret = new GestureDetector(
                onLongPress: () => this._onMessageLongPress(message, left),
                child: ret
            );

            if (left || !message.isReactionsEmpty() || this._animatingMessageReaction == message.id) {
                ret = new Expanded(
                    child: new Column(
                        crossAxisAlignment: left ? CrossAxisAlignment.start : CrossAxisAlignment.end,
                        children: new List<Widget> {
                            left ? this._buildMessageTitle(message) : new Container(),
                            ret,
                            new GestureDetector(
                                onLongPress: () => this.widget.actionModel.pushToReactionsDetail(message.id),
                                child: new Container(
                                    color: CColors.Transparent,
                                    child: this._buildReactionBar(message, left)
                                )
                            )
                        }
                    )
                );
            }

            ret = new Container(
                padding: EdgeInsets.only(left: 2, right: 2, bottom: 16),
                child: new Row(
                    mainAxisAlignment: left ? MainAxisAlignment.start : MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: left
                        ? new List<Widget> {this._buildAvatar(message.author), ret}
                        : new List<Widget> {ret, this._buildAvatar(this.widget.viewModel.me)}
                )
            );

            if (showTime || showUnreadLine) {
                ret = new Column(
                    children: new List<Widget> {
                        showTime ? this._buildTime(message.time) : new Container(),
                        ret,
                        showUnreadLine ? this._buildUnreadMessageLine() : new Container()
                    }
                );
            }

            ret = new Container(key: this.getFullMessageKey(message.id), child: ret);

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
                        color: CColors.LoadingGrey,
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
                                        child: new CachedNetworkImage(src: httpsUrl)
                                    ),
                                Positioned.fill(
                                    Image.asset(
                                        "image/avatar-circle",
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
                child: new Hero(
                    tag: CImageUtils.SizeToScreenImageUrl(imageUrl: message.content),
                    child: new ImageMessage(
                        id: message.id,
                        url: message.content,
                        data: message.imageData,
                        size: 140,
                        ratio: 16.0f / 9.0f,
                        srcWidth: message.width,
                        srcHeight: message.height,
                        headers: this._headers
                    )
                )
            );
        }

        Widget _buildMessageContent(ChannelMessageView message) {
            switch (message.type) {
                case ChannelMessageType.deleted:
                    return new DeletedMessage();
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
                        () => {
                            var attachment = message.attachments.first();
                            var contentType = attachment.contentType;
                            if (CCommonUtils.isAndroid && contentType != "video/mp4") {
                                CustomToast.show(new CustomToastItem(
                                    context: this.context,
                                    "暂不支持该文件",
                                    TimeSpan.FromMilliseconds(2000)
                                ));
                                return;
                            }

                            if (contentType == "video/mp4") {
                                if (!this._showReactionOverlay) {
                                    this.widget.actionModel.playVideo(obj: attachment.url);
                                }
                            }
                            else {
                                this.widget.actionModel.openUrl(obj: attachment.url);
                            }
                        }
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
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: new List<Widget> {
                                new Container(
                                    margin: EdgeInsets.only(right: 10),
                                    color: new Color(0x20959595),
                                    height: 1,
                                    width: 80
                                ),
                                new Text(
                                    "以下为新消息",
                                    style: new TextStyle(
                                        fontSize: 12,
                                        fontFamily: "Roboto-Regular",
                                        color: new Color(0x88959595)
                                    )
                                ),
                                new Container(
                                    margin: EdgeInsets.only(left: 10),
                                    color: new Color(0x20959595),
                                    height: 1,
                                    width: 80
                                )
                            }
                        )
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
            this.customTextField = new CustomTextField(
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
                    if (this._textController.text.isNotEmpty()) {
                        this._textController.selection = TextSelection.collapsed(this._textController.text.Length);
                    }

                    if (this.showEmojiBoard) {
                        this._focusNode.unfocus();
                        FocusScope.of(this.context).requestFocus(node: this._focusNode);
                        TextInputPlugin.TextInputShow();
                    }
                    else {
                        // If keyboard is present now, just hide it. If keyboard is not present now,
                        // it may pop out later (because of the focus), wait for a while and pop it
                        if (!this.showKeyboard) {
                            this.showEmojiBoard = true;
                            Promise.Delayed(TimeSpan.FromMilliseconds(100)).Then(
                                onResolved: () => {
                                    TextInputPlugin.TextInputHide();
                                    this.showEmojiBoard = true;
                                });
                        }
                        else {
                            this._showEmojiBoard = true;
                            this._emojiBoardController.setValue(1);
                            TextInputPlugin.TextInputHide();
                        }
                    }
                }
            );

            Widget backdropFilterWidget;
            if (!this.showEmojiBoard && !this.showKeyboard) {
                backdropFilterWidget = new BackdropFilter(
                    filter: ImageFilter.blur(10, 10),
                    child: this.customTextField
                );
            }
            else {
                backdropFilterWidget = this.customTextField;
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
            var start = selection.start < 0 ? 0 : selection.start;
            var end = selection.end < 0 ? 0 : selection.end;
            this._textController.value = new TextEditingValue(
                this._textController.text.Substring(0, length: start) +
                emojiText + this._textController.text.Substring(startIndex: end),
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
                status = "waiting",
                reactionsCountDict = new SortedDictionary<string, int>(),
                reactions = new List<Reaction>(),
                allUserReactionsDict = new Dictionary<string, Dictionary<string, int>>()
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
                    .Then(() => {
                        Promise.Delayed(TimeSpan.FromMilliseconds(500)).Then(() => {
                            if (this._lastReadMessageId != null &&
                                this.calculateOffsetFromMessage(this._lastReadMessageId) <
                                this._refreshController.offset + 10) {
                                this.showUnreadMessageNotification = false;
                            }
                        });
                    });
            }
        }

        float? _lastScrollPosition = null;

        public _ChannelScreenState() {
            this._bottomPaddingWhenShowingPopupBar = 0;
        }

        const float bottomThreshold = 50;

        void _dismissKeyboard() {
            if (this._showEmojiBoard && !this.showKeyboard) {
                this.showEmojiBoard = false;
            }

            this._focusNode.unfocus();
            TextInputPlugin.TextInputHide();
        }

        void _handleScrollListener() {
            if (this._refreshController.offset <= bottomThreshold) {
                if (this._lastScrollPosition == null || this._lastScrollPosition > bottomThreshold) {
                    if (this.widget.viewModel.channel.newMessageIds.isNotEmpty()) {
                        float offset = 0;
                        for (int i = 0; i < this.widget.viewModel.newMessages.Count; i++) {
                            var message = this.widget.viewModel.newMessages[i];
                            offset += this.calculateMessageHeight(message,
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
            if (this.showKeyboard || this.showEmojiBoard) {
                this.setState(fn: this._dismissKeyboard);
            }

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
                    "从相册选择照片",
                    onTap: () => PickImagePlugin.PickImage(
                        source: ImageSource.gallery,
                        imageCallBack: this._pickImageCallback,
                        false
                    )
                ),
                new ActionSheetItem(
                    "从相册选择视频",
                    onTap: () => PickImagePlugin.PickVideo(
                        source: ImageSource.gallery,
                        videoCallBack: this._pickVideoCallback
                    )
                ),
                new ActionSheetItem("取消", type: ActionType.cancel)
            };

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                title: "发送图片或视频",
                items: items
            ));
        }

        void _pickImageCallback(byte[] pickImage) {
            var nonce = Snowflake.CreateNonce();
            this._refreshController.scrollTo(0);
            this.widget.actionModel.addLocalMessage(new ChannelMessageView {
                id = nonce,
                author = this.widget.viewModel.me,
                channelId = this.widget.viewModel.channel.id,
                nonce = nonce.hexToLong(),
                type = ChannelMessageType.image,
                imageData = pickImage,
                time = DateTime.UtcNow,
                status = "waiting",
                reactionsCountDict = new SortedDictionary<string, int>(),
                allUserReactionsDict = new Dictionary<string, Dictionary<string, int>>()
            });
        }

        void _pickVideoCallback(byte[] videoData) {
            var nonce = Snowflake.CreateNonce();
            this._refreshController.scrollTo(0);
            this.widget.actionModel.addLocalMessage(new ChannelMessageView {
                id = nonce,
                author = this.widget.viewModel.me,
                channelId = this.widget.viewModel.channel.id,
                nonce = nonce.hexToLong(),
                type = ChannelMessageType.file,
                videoData = videoData,
                attachments = new List<Attachment> {
                    new Attachment {
                        filename = $"VIDEO_{DateTime.Now:yyyyMMddHHmmss}.mp4",
                        contentType = "video/mp4",
                        size = videoData.Length
                    }
                },
                time = DateTime.UtcNow,
                status = "waiting",
                reactionsCountDict = new SortedDictionary<string, int>(),
                allUserReactionsDict = new Dictionary<string, Dictionary<string, int>>()
            });
        }

        float calculateReactionBarHeight(ChannelMessageView message, float maxWidth) {
            float reactionBarWidth = 0;
            message.fillReactionsCountDict();
            foreach (var pair in message.reactionsCountDict) {
                if (pair.Value > 0 || message.isReactionSelectedByLocalAndServer(pair.Key)) {
                    if (reactionBarWidth > 0) {
                        reactionBarWidth += 8;
                    }

                    reactionBarWidth += 40 + CTextUtils.CalculateTextWidth(
                                            $"{message.adjustedReactionCount(pair.Key)}",
                                            CTextStyle.PRegularBody,
                                            float.PositiveInfinity);
                }
            }
            return (reactionBarWidth / maxWidth).ceil() * 36;
        }

        float calculateMessageHeight(ChannelMessageView message, bool showTime, float width) {
            if (message.type == ChannelMessageType.skip || message.type == ChannelMessageType.deleted) {
                return 0;
            }

            if (message.buildHeight != null && message.buildHeight > 0) {
                return message.buildHeight.Value;
            }

            var context = this.getFullMessageKey(message.id).currentContext;
            if (context != null) {
                RenderBox renderBox = (RenderBox) context.findRenderObject();
                message.buildHeight = renderBox.size.height;
                return message.buildHeight.Value;
            }

            if (message.buildHeight != null) {
                return -message.buildHeight.Value;
            }

            float height = 20 + 6 + 16 + (showTime ? 36 : 0) + // Name + Internal + Bottom padding + time
                           this.calculateReactionBarHeight(message,
                               MediaQuery.of(this.context).size.width - 74); // Reaction bar
            switch (message.type) {
                case ChannelMessageType.deleted:
//                    height += DeletedMessage.CalculateTextHeight(width: width);
                    break;
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

            // Store a negative value to mark that it is not accurate
            message.buildHeight = -height;
            return height;
        }

        public void didPopNext() {
            if (this.widget.viewModel.channelId.isNotEmpty()) {
                CTemporaryValue.currentPageModelId = this.widget.viewModel.channelId;
            }

            StatusBarManager.statusBarStyle(false);
        }

        public void didPush() {
            if (this.widget.viewModel.channelId.isNotEmpty()) {
                CTemporaryValue.currentPageModelId = this.widget.viewModel.channelId;
            }
        }

        public void didPop() {
            if (CTemporaryValue.currentPageModelId.isNotEmpty() &&
                this.widget.viewModel.channelId == CTemporaryValue.currentPageModelId) {
                CTemporaryValue.currentPageModelId = null;
            }

            this.mentionMap.Clear();
            if (this._focusNode.hasFocus) {
                this._focusNode.unfocus();
            }

            this.widget.actionModel.popFromScreen();
        }

        public void didPushNext() {
            CTemporaryValue.currentPageModelId = null;
            if (this.showKeyboard || this.showEmojiBoard) {
                this.setState(fn: this._dismissKeyboard);
            }
        }
    }

    class _ReactionOverlay : StatefulWidget {
        public _ReactionOverlay(
            ChannelMessageView message,
            Size size,
            Offset offset,
            bool left,
            Widget messageBubble,
            PopupLikeButtonBar.OnTapPopupLikeButtonCallback onTap,
            string userId,
            Key key = null) : base(key: key) {
            this.message = message;
            this.size = size;
            this.offset = offset;
            this.left = left;
            this.messageBubble = messageBubble;
            this.onTap = onTap;
            this.userId = userId;
        }

        public readonly ChannelMessageView message;
        public readonly Size size;
        public readonly Offset offset;
        public readonly bool left;
        public readonly Widget messageBubble;
        public readonly PopupLikeButtonBar.OnTapPopupLikeButtonCallback onTap;
        public string userId;

        public override State createState() {
            return new _ReactionOverlayState();
        }
    }

    class _ReactionOverlayState : TickerProviderStateMixin<_ReactionOverlay> {
        AnimationController _highlightMessageController;
        Animation<float> _highlightMessageAnimation;

        public override void initState() {
            base.initState();
            this._highlightMessageController = new AnimationController(
                duration: TimeSpan.FromMilliseconds(500),
                vsync: this);
            this._highlightMessageController.addListener(() => this.setState(() => { }));
            Promise.Delayed(TimeSpan.FromMilliseconds(250)).Then(() => {
                this._highlightMessageController.animateTo(1);
            });
            this._highlightMessageAnimation = new FloatTween(0.7f, 1).animate(this._highlightMessageController);
        }

        public override void dispose() {
            this._highlightMessageController.dispose();
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            return new Column(
                mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget> {
                    new SizedBox(height: this.widget.offset.dy - 60),
                    new Row(
                        mainAxisAlignment: this.widget.left ? MainAxisAlignment.start : MainAxisAlignment.end,
                        children: this.widget.left
                            ? new List<Widget> {
                                new SizedBox(width: this.widget.offset.dx),
                                this._buildPopupLikeButtonBar(this.widget.message)
                            }
                            : new List<Widget> {
                                this._buildPopupLikeButtonBar(this.widget.message),
                                new SizedBox(width: MediaQuery.of(this.context).size.width -
                                                    this.widget.offset.dx -
                                                    this.widget.size.width)
                            }
                    ),
                    new SizedBox(height: 8),
                    new Row(
                        mainAxisSize: MainAxisSize.min,
                        children: new List<Widget> {
                            new SizedBox(width: this.widget.offset.dx),
                            new Opacity(
                                opacity: this._highlightMessageAnimation.value,
                                child: this.widget.messageBubble
                            )
                        }
                    )
                }
            );
        }

        Widget _buildPopupLikeButtonBar(ChannelMessageView message) {
            return new PopupLikeButtonBar(
                onTap: this.widget.onTap,
                ReactionType.typesList.Select(type => 
                    new PopupLikeButtonItem {
                        content = type.gifImagePath,
                        selected = message.isReactionSelectedByLocalAndServer(type: type.value),
                        type = type.value
                    }
                ).ToList()
            );
        }
    }
}