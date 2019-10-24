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
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Config = ConnectApp.Constants.Config;
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
                    List<ChannelMessageView> getMessages(List<string> messageIds) {
                        if (messageIds.isNullOrEmpty()) {
                            return new List<ChannelMessageView>();
                        }

                        var channelMessageViews = new List<ChannelMessageView>();
                        messageIds.ForEach(messageId => {
                            if (state.channelState.messageDict.ContainsKey(key: messageId)) {
                                channelMessageViews.Add(state.channelState.messageDict[key: messageId]);
                            }
                        });
                        return channelMessageViews;
                    }


                    List<ChannelMessageView> newMessages = null;
                    List<ChannelMessageView> messages;
                    ChannelView channel;
                    var hasChannel = false;
                    if (state.channelState.channelDict.isEmpty() ||
                        !state.channelState.channelDict.ContainsKey(this.channelId)) {
                        messages = new List<ChannelMessageView>();
                        channel = new ChannelView();
                    }
                    else {
                        hasChannel = true;
                        channel = state.channelState.channelDict[this.channelId];
                        if (channel.newMessageIds.isNotEmpty()) {
                            messages = getMessages(messageIds: channel.messageIds);
                            newMessages = getMessages(messageIds: channel.newMessageIds);
                        }
                        else if (channel.oldMessageIds.isNotEmpty()) {
                            messages = getMessages(messageIds: channel.oldMessageIds);
                            messages.AddRange(getMessages(messageIds: channel.messageIds));
                        }
                        else {
                            messages = getMessages(messageIds: channel.messageIds);
                        }
                    }

                    if (messages.isNotEmpty()) {
                        messages = messages
                            .Where(message => message.type != ChannelMessageType.text || message.content != "")
                            .ToList();
                    }

                    return new ChannelScreenViewModel {
                        hasChannel = hasChannel,
                        channelError = state.channelState.channelError,
                        channel = channel,
                        messages = messages,
                        newMessages = newMessages ?? new List<ChannelMessageView>(),
                        me = state.loginState.loginInfo.userId,
                        messageLoading = state.channelState.messageLoading,
                        newMessageCount = hasChannel ? state.channelState.channelDict[this.channelId].unread : 0,
                        socketConnected = state.channelState.socketConnected,
                        mentionAutoFocus = state.channelState.mentionAutoFocus,
                        mentionUserId = state.channelState.mentionUserId,
                        mentionUserName = state.channelState.mentionUserName,
                        mentionSuggestion = state.channelState.mentionSuggestions.getOrDefault(this.channelId, null)
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    if (viewModel.hasChannel) {
                        if (viewModel.channel.oldMessageIds.isNotEmpty()) {
                            SchedulerBinding.instance.addPostFrameCallback(_ => {
                                dispatcher.dispatch(new MergeOldChannelMessages {channelId = this.channelId});
                            });
                        }

                        if (viewModel.channel.sentMessageFailed ||
                            viewModel.channel.sentMessageSuccess ||
                            viewModel.channel.sentImageSuccess) {
                            SchedulerBinding.instance.addPostFrameCallback(_ => {
                                dispatcher.dispatch(new ClearSentChannelMessage {channelId = this.channelId});
                            });
                        }
                    }

                    var actionModel = new ChannelScreenActionModel {
                        mainRouterPop = () => {
                            dispatcher.dispatch(new MainNavigatorPopAction());
                            dispatcher.dispatch(Actions.ackChannelMessage(viewModel.channel.lastMessageId));
                            dispatcher.dispatch(new ChannelScreenLeaveBottom {channelId = this.channelId});
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
                            Actions.fetchChannelMember(channelId: this.channelId, userId: viewModel.me)),
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
                        startSendMessage = () => dispatcher.dispatch(new StartSendChannelMessageAction {
                            channelId = this.channelId
                        }),
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
                        pushToChannelMention = () => {
                            dispatcher.dispatch(new MainNavigatorPushToChannelMentionAction {
                                channelId = this.channelId
                            });
                        },
                        clearLastChannelMention = () => dispatcher.dispatch(new ChannelClearMentionAction())
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
        FocusNode _focusNode;
        GlobalKey _focusNodeKey;

        float messageBubbleWidth = 0;
        bool _showEmojiBoard = false;
        Dictionary<string, string> headers;

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        float inputBarHeight {
            get { return 48 + CCommonUtils.getSafeAreaBottomPadding(this.context); }
        }

        bool showKeyboard {
            get { return MediaQuery.of(this.context).viewInsets.bottom > 50; }
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

                return this._showEmojiBoard && !this.showKeyboard;
            }
        }

        public override void initState() {
            base.initState();
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
            });

            this._focusNode = new FocusNode();
            this._focusNodeKey = GlobalKey.key("_channelFocusNodeKey");
            this.headers = new Dictionary<string, string> {
                {HttpManager.COOKIE, HttpManager.getCookie()},
                {"ConnectAppVersion", Config.versionNumber},
                {"X-Requested-With", "XmlHttpRequest"}
            };
            this._textController.addListener(this._onTextChanged);
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

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            this._textController.removeListener(this._onTextChanged);
            this._textController.dispose();
            SchedulerBinding.instance.addPostFrameCallback(_ => { this.widget.actionModel.clearUnread(); });
            this._focusNode.dispose();
            base.dispose();
        }

        string _lastMessageEditingContent = "";
        readonly Dictionary<string, string> mentionMap = new Dictionary<string, string>();

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

        public override Widget build(BuildContext context) {
            if (this.widget.viewModel.channel.id == null) {
                return this._buildLoadingPage();
            }

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

            if (this.widget.viewModel.channel.sentMessageSuccess) {
                SchedulerBinding.instance.addPostFrameCallback(_ => {
                    this._textController.clear();
                    this._textController.selection = TextSelection.collapsed(0);
                });
            }

            if (this.widget.viewModel.channel.sentMessageFailed) {
                SchedulerBinding.instance.addPostFrameCallback(_ => {
                    CustomDialogUtils.showToast("消息发送失败", Icons.error_outline);
                });
            }

            this.messageBubbleWidth = MediaQuery.of(context).size.width * 0.7f;

            Widget ret = new Stack(
                children: new List<Widget> {
                    this._buildContent(),
                    this.widget.viewModel.messageLoading &&
                    this.widget.viewModel.messages.isEmpty()
                        ? (Widget) new GlobalLoading()
                        : new Container(),
                    this._buildInputBar(),
                    this.widget.viewModel.newMessageCount == 0 ||
                    this.widget.viewModel.messageLoading
                        ? new Container()
                        : this._buildNewMessageNotification()
                }
            );

            ret = new Column(
                children: new List<Widget> {
                    this._buildNavigationBar(),
                    HttpManager.isNetWorkError()
                        ? this._buildNetworkDisconnectedNote()
                        : new Container(),
                    new Flexible(child: ret),
                    this.showEmojiBoard
                        ? this._buildEmojiBoard()
                        : new Container(height: MediaQuery.of(this.context).viewInsets.bottom)
                }
            );

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: ret
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
            Widget ret = new Container(
                height: 40,
                decoration: new BoxDecoration(
                    color: CColors.Error,
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
                            style: CTextStyle.PRegularWhite.copyWith(height: 1f)
                        )
                    })
            );

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

        Widget _buildNavigationBar() {
            return new CustomAppBar(
                () => this.widget.actionModel.mainRouterPop(),
                new Flexible(
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: new List<Widget> {
                            new Flexible(
                                child: new Text(
                                    this.widget.viewModel.socketConnected
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

            Widget ret = new Container(
                color: CColors.White,
                child: new CustomScrollbar(
                    child: new SmartRefresher(
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
                ret = new GestureDetector(
                    onTap: () => this.setState(this._dismissKeyboard),
                    child: ret
                );
            }

            return ret;
        }

        ListView _buildMessageListView() {
            if (this.widget.viewModel.messages.Count == 0) {
                return new ListView(
                    children: new List<Widget> {
                        new Container()
                    });
            }

            return ListView.builder(
                padding: EdgeInsets.only(top: 16, bottom: this.inputBarHeight),
                itemCount: this.widget.viewModel.messages.Count,
                itemBuilder: (context, index) => {
                    index = this.widget.viewModel.messages.Count - 1 - index;
                    var message = this.widget.viewModel.messages[index];
                    return this._buildMessage(message,
                        showTime: index == 0 ||
                                  (message.time - this.widget.viewModel.messages[index - 1].time) >
                                  TimeSpan.FromMinutes(5),
                        left: message.author.id != this.widget.viewModel.me
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

        Widget _buildMessage(ChannelMessageView message, bool showTime, bool left) {
            if (message.shouldSkip()) {
                return new Container();
            }

            Widget ret = new Container(
                constraints: new BoxConstraints(
                    maxWidth: this.messageBubbleWidth
                ),
                padding: message.type == ChannelMessageType.text
                    ? EdgeInsets.symmetric(8, 12)
                    : EdgeInsets.zero,
                decoration: this._messageDecoration(message.type, left),
                child: this._buildMessageContent(message)
            );

            var tipMenuItems = new List<TipMenuItem>();
            if (message.type == ChannelMessageType.text || message.type == ChannelMessageType.embed) {
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

            if (message.author.id == this.widget.viewModel.me) {
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

            if (showTime) {
                ret = new Column(
                    children: new List<Widget> {
                        this._buildTime(message.time),
                        ret
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
                        if (user.id == this.widget.viewModel.me) {
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
                                () => { this.setState(() => { this._showEmojiBoard = false; }); });
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

        Widget _buildTextMessageContent(ChannelMessageView message) {
            if (string.IsNullOrEmpty(message.content)) {
                return new Container();
            }

            return new RichText(text: new TextSpan(children: MessageUtils.messageWithMarkdownToTextSpans(
                message.content, message.mentions, message.mentionEveryone,
                onTap: userId => this.widget.actionModel.pushToUserDetail(obj: userId)).ToList()));
        }

        Widget _buildImageMessageContent(ChannelMessageView message) {
            return new GestureDetector(
                onTap: () => {
                    var imageUrls = this.widget.viewModel.messages
                        .Where(msg => msg.type == ChannelMessageType.image)
                        .Select(msg => CImageUtils.SizeToScreenImageUrl(msg.content))
                        .ToList();
                    var url = CImageUtils.SizeToScreenImageUrl(message.content);
                    this.widget.actionModel.browserImage(url, imageUrls);
                },
                child: new ImageMessage(
                    url: message.content,
                    size: 140,
                    ratio: 16.0f / 9.0f,
                    srcWidth: message.width,
                    srcHeight: message.height,
                    headers: this.headers
                )
            );
        }

        Widget _buildFileMessageContent() {
            return new Container(
                padding: EdgeInsets.symmetric(12, 16),
                child: new Text("[你收到一个文件，请在浏览器上查看]", style: CTextStyle.PLargeBody5)
            );
        }

        Widget _buildEmbedContent(ChannelMessageView message) {
            if (message.embeds[0].embedData.url != null && message.content.Contains(message.embeds[0].embedData.url)) {
                return new RichText(text: new TextSpan(children: MessageUtils.messageWithMarkdownToTextSpans(
                    message.content, message.mentions, message.mentionEveryone,
                    onTap: userId => this.widget.actionModel.pushToUserDetail(obj: userId),
                    url: message.embeds[0].embedData.url,
                    onClickUrl: url => this.widget.actionModel.openUrl(message.embeds[0].embedData.url)).ToList()));
            }

            return this._buildMessageContent(message);
        }

        Widget _buildEmbeddedRect(EmbedData embedData) {
            return new Container(
                padding: EdgeInsets.all(12),
                color: CColors.White,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        this._buildEmbeddedTitle(embedData.title),
                        new Container(height: 4),
                        this._buildEmbeddedDescription(embedData.description),
                        this._buildEmbeddedName(embedData.image,
                            embedData.name)
                    }
                )
            );
        }

        Widget _buildEmbeddedTitle(string title) {
            return new Text(title ?? "", style: CTextStyle.PLargeMediumBlue);
        }

        Widget _buildEmbeddedDescription(string description) {
            return description == null
                ? new Container()
                : new Container(
                    padding: EdgeInsets.only(bottom: 4),
                    child: new Text(description ?? "",
                        style: CTextStyle.PRegularBody3, maxLines: 4,
                        overflow: TextOverflow.ellipsis));
        }

        Widget _buildEmbeddedName(string image, string name) {
            if (image.isEmpty() && name.isEmpty()) {
                return new Container();
            }

            return new Row(
                crossAxisAlignment: CrossAxisAlignment.center,
                children: new List<Widget> {
                    image == null
                        ? (Widget) new Container(width: 14, height: 14)
                        : CachedNetworkImageProvider.cachedNetworkImage(
                            image ?? "",
                            width: 14, height: 14, fit: BoxFit.cover),
                    new Container(width: 4),
                    new Expanded(
                        child: new Text(name ?? "",
                            style: CTextStyle.PMediumBody,
                            overflow: TextOverflow.ellipsis)
                    )
                }
            );
        }

        Widget _buildEmbedMessageContent(ChannelMessageView message) {
            return new Container(
                padding: EdgeInsets.all(12),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        this._buildEmbedContent(message),
                        new Container(height: 12),
                        new GestureDetector(
                            child: this._buildEmbeddedRect(message.embeds[0].embedData),
                            onTap: () => this.widget.actionModel.openUrl(message.embeds[0].embedData.url)
                        )
                    }
                )
            );
        }

        Widget _buildMessageContent(ChannelMessageView message) {
            switch (message.type) {
                case ChannelMessageType.text:
                    return this._buildTextMessageContent(message);
                case ChannelMessageType.image:
                    return this._buildImageMessageContent(message);
                case ChannelMessageType.file:
                    return this._buildFileMessageContent();
                case ChannelMessageType.embed:
                    return this._buildEmbedMessageContent(message);
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

        Widget _buildInputBar() {
            var padding = this.showKeyboard || this.showEmojiBoard ? 0 : MediaQuery.of(this.context).padding.bottom;
            var customTextField = new CustomTextField(
                EdgeInsets.only(bottom: padding),
                new BoxDecoration(
                    border: new Border(new BorderSide(color: CColors.Separator)),
                    color: this.showEmojiBoard ? CColors.White : CColors.TabBarBg
                ),
                textFieldKey: this._focusNodeKey,
                "说点想法…",
                controller: this._textController,
                focusNode: this._focusNode,
                maxLines: 4,
                minLines: 1,
                loading: this.widget.viewModel.channel.sendingMessage,
                showEmojiBoard: this.showEmojiBoard,
                isShowImageButton: true,
                onSubmitted: this._handleSubmit,
                onPressImage: this._pickImage,
                onPressEmoji: () => {
                    this._refreshController.scrollController.jumpTo(0);
                    FocusScope.of(context: this.context).requestFocus(node: this._focusNode);
                    if (this.showEmojiBoard) {
                        TextInputPlugin.TextInputShow();
                        Promise.Delayed(TimeSpan.FromMilliseconds(200)).Then(
                            () => this.setState(() => this._showEmojiBoard = false));
                    }
                    else {
                        this.setState(() => this._showEmojiBoard = true);
                        Promise.Delayed(TimeSpan.FromMilliseconds(100)).Then(
                            onResolved: TextInputPlugin.TextInputHide
                        );
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
            return new EmojiBoard(
                handleEmoji: this._handleEmoji,
                handleDelete: this._handleDelete,
                () => this._handleSubmit(text: this._textController.text)
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
            if (this.widget.viewModel.channel.sendingMessage) {
                return;
            }

            text = ChannelMessageMentionHelper.parseMention(text, this.mentionMap);
            if (string.IsNullOrWhiteSpace(text)) {
                CustomDialogUtils.showToast("不能发送空消息", Icons.error_outline);
                return;
            }

            this.widget.actionModel.startSendMessage();
            this.widget.actionModel.sendMessage(
                    this.widget.viewModel.channel.id,
                    text.Trim(), Snowflake.CreateNonce(), "")
                .Catch(_ => CustomDialogUtils.showToast("消息发送失败", Icons.error_outline));
            this._refreshController.scrollTo(0);
            FocusScope.of(this.context).requestFocus(this._focusNode);
        }

        void _onRefresh(bool up) {
            if (!up) {
                string id = this.widget.viewModel.messages.isNotEmpty()
                    ? this.widget.viewModel.messages.first().id
                    : null;
                this.widget.actionModel.fetchMessages(id, null).Then(
                    () => this._refreshController.sendBack(false, RefreshStatus.idle)
                ).Catch(
                    (error) => this._refreshController.sendBack(false, RefreshStatus.idle)
                );
            }
        }

        float? _lastScrollPosition = null;

        const float bottomThreshold = 50;

        void _dismissKeyboard() {
            this._showEmojiBoard = false;
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
                                      TimeSpan.FromMinutes(5)
                                    : message.time - this.widget.viewModel.newMessages[i - 1].time >
                                      TimeSpan.FromMinutes(5),
                                this.messageBubbleWidth);
                        }

                        this._refreshController.scrollController.jumpTo(
                            this._refreshController.scrollController.offset + offset);
                    }

                    this.widget.actionModel.reportHitBottom();
                }
            }
            else if (this._refreshController.offset > bottomThreshold) {
                if (this._lastScrollPosition == null || this._lastScrollPosition <= bottomThreshold) {
                    this.widget.actionModel.reportLeaveBottom();
                }
            }

            if (this._lastScrollPosition == null || this._lastScrollPosition < this._refreshController.offset) {
                if (this.showEmojiBoard || this.showKeyboard) {
                    this.setState(this._dismissKeyboard);
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
            this.widget.actionModel.startSendMessage();
            this.widget.actionModel.sendImage(
                arg1: this.widget.viewModel.channel.id,
                arg2: pickImage,
                Snowflake.CreateNonce());
        }

        public void didPop() {
            this.mentionMap.Clear();
            if (this._focusNode.hasFocus) {
                this._focusNode.unfocus();
            }
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

        public static float calculateMessageHeight(ChannelMessageView message, bool showTime, float width) {
            float height = 20 + 6 + 16 + (showTime ? 36 : 0); // Name + Internal + Bottom padding + time
            switch (message.type) {
                case ChannelMessageType.text:
                    height += 16 + CTextUtils.CalculateTextHeight(
                                  message.content,
                                  CTextStyle.PLargeBody,
                                  width - 24, maxLines: null);
                    break;
                case ChannelMessageType.image:
                    height += message.width > message.height * 16.0f / 9.0f
                        ? 140.0f * 9.0f / 16.0f
                        : message.width > message.height
                            ? 140.0f * message.height / message.width
                            : 140.0f;
                    break;
                case ChannelMessageType.file:
                    height += 16 + CTextUtils.CalculateTextHeight(
                                  "[你收到一个文件，请在浏览器上查看]",
                                  CTextStyle.PLargeBody5,
                                  width - 24, maxLines: null);
                    break;
                case ChannelMessageType.embed:
                    height += 24 + CTextUtils.CalculateTextHeight(
                                  message.content,
                                  CTextStyle.PLargeBody,
                                  width - 24, maxLines: null) + 24 +
                              CTextUtils.CalculateTextHeight(
                                  message.embeds[0].embedData.title,
                                  CTextStyle.PLargeMediumBlue,
                                  width - 48, maxLines: null) + 4 +
                              CTextUtils.CalculateTextHeight(
                                  message.embeds[0].embedData.description,
                                  CTextStyle.PRegularBody3,
                                  width - 48, maxLines: 4) + 4 + 22 + 12;
                    break;
            }

            return height;
        }
    }
}