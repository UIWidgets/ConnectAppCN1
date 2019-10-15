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
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
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
                    ChannelMessageView getMessage(string messageId) {
                        return state.channelState.messageDict[messageId];
                    }

                    var channel = state.channelState.channelDict[this.channelId];
                    List<ChannelMessageView> newMessages = null;
                    List<ChannelMessageView> messages;
                    if (channel.newMessageIds.isNotEmpty()) {
                        messages = channel.messageIds.Select(getMessage).ToList();
                        newMessages = channel.newMessageIds.Select(getMessage).ToList();
                    }
                    else if (channel.oldMessageIds.isNotEmpty()) {
                        messages = channel.oldMessageIds.Select(getMessage).ToList();
                        messages.AddRange(channel.messageIds.Select(getMessage));
                    }
                    else {
                        messages = channel.messageIds.Select(getMessage).ToList();
                    }

                    messages = messages
                        .Where(message => message.type != ChannelMessageType.text || message.content != "")
                        .ToList();

                    return new ChannelScreenViewModel {
                        channel = state.channelState.channelDict[this.channelId],
                        messages = messages,
                        newMessages = newMessages ?? new List<ChannelMessageView>(),
                        me = state.loginState.loginInfo.userId,
                        messageLoading = state.channelState.messageLoading,
                        newMessageCount = state.channelState.channelDict[this.channelId].unread,
                        socketConnected = state.channelState.socketConnected,
                        mentionAutoFocus = state.channelState.mentionAutoFocus,
                        mentionUserId = state.channelState.mentionUserId,
                        mentionSuggestion = state.channelState.mentionSuggestions.getOrDefault(this.channelId, null)
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
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
                        fetchMessages = (before, after) => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMessages(channelId: this.channelId, before: before, after: after)),
                        fetchMembers = () => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMembers(channelId: this.channelId)),
                        fetchMember = () => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMember(channelId: this.channelId, userId: viewModel.me)),
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
        readonly ChannelMessageInputManager _inputContentManager = new ChannelMessageInputManager();
        TabController _emojiTabController;
        FocusNode _focusNode;
        GlobalKey _focusNodeKey;

        float messageBubbleWidth = 0;
        bool _showEmojiBoard = false;
        Dictionary<string, string> headers;
        float mPaddingBottom = 0;

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
            this._emojiTabController = new TabController(
                length: (emojiList.Count - 1) / emojiBoardPageSize + 1,
                vsync: this);
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this._refreshController.scrollController.addListener(this._handleScrollListener);
                this.widget.actionModel.fetchMessages(null, null);
                this.widget.actionModel.fetchMembers();
                this.widget.actionModel.fetchMember();
                this.widget.actionModel.reportHitBottom();
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

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            this._textController.removeListener(this._onTextChanged);
            this._textController.dispose();
            this._emojiTabController.dispose();
            SchedulerBinding.instance.addPostFrameCallback(_ => { this.widget.actionModel.clearUnread(); });
            this._focusNode.dispose();
            base.dispose();
        }

        string _lastMessageEditingContent = "";
        bool omitTextChange;

        void _onTextChanged() {
            if (this.omitTextChange) {
                this.omitTextChange = false;
                return;
            }

            var curTextContent = this._textController.text;
            if (curTextContent != this._lastMessageEditingContent) {
                var isDelete = curTextContent.Length < this._lastMessageEditingContent.Length;
                this._lastMessageEditingContent = curTextContent;

                if (!isDelete) {
                    this._inputContentManager.AddContent(this._textController.selection.start,
                        this._lastMessageEditingContent);
                }
                else {
                    var jumpForward = 0;
                    this._lastMessageEditingContent = this._inputContentManager.DeleteContent(
                        this._textController.selection.end, this._lastMessageEditingContent, ref jumpForward);
                    if (this._textController.text != this._lastMessageEditingContent) {
                        var selection = this._textController.selection;
                        this.omitTextChange = true;
                        this._textController.value = new TextEditingValue(
                            text: this._lastMessageEditingContent,
                            TextSelection.collapsed(selection.start - jumpForward));
                    }
                }

                if (!isDelete &&
                    this._lastMessageEditingContent.isNotEmpty() &&
                    this._lastMessageEditingContent[this._lastMessageEditingContent.Length - 1] == '@') {
                    this.widget.actionModel.pushToChannelMention();
                }
            }
        }

        public override Widget build(BuildContext context) {
            this.mPaddingBottom = MediaQuery.of(context).padding.bottom;
            if (this.widget.viewModel.mentionAutoFocus) {
                SchedulerBinding.instance.addPostFrameCallback(_ => {
                    FocusScope.of(this.context)?.requestFocus(this._focusNode);
                    if (!this.widget.viewModel.mentionUserId.isEmpty()) {
                        var userDict = this.widget.viewModel.mentionSuggestion ??
                                       this.widget.viewModel.channel.membersDict;
                        if (userDict.ContainsKey(this.widget.viewModel.mentionUserId)) {
                            var userName = userDict[this.widget.viewModel.mentionUserId].user.fullName;
                            var newContent = this._textController.text + userName + " ";
                            this._inputContentManager.AddMention(userName + " ", this.widget.viewModel.mentionUserId,
                                newContent);
                            this._textController.value = new TextEditingValue(
                                text: newContent,
                                TextSelection.collapsed(newContent.Length));
                        }
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
                    this._buildInputBar(),
                    this.widget.viewModel.newMessageCount == 0 ||
                    this.widget.viewModel.messageLoading
                        ? new Container()
                        : this._buildNewMessageNotification(),
                    this.widget.viewModel.socketConnected
                        ? new Container()
                        : this._buildNetworkDisconnectedNote()
                }
            );

            ret = new Column(
                children: new List<Widget> {
                    this._buildNavigationBar(),
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
            Widget ret = new Container(
                color: CColors.Error,
                child: new Center(
                    child: new Text(
                        $"网络未连接",
                        style: CTextStyle.PRegularWhite.copyWith(height: 1f)
                    )
                )
            );

            ret = new Positioned(
                top: 0,
                left: 0,
                right: 0,
                height: 24,
                child: ret
            );

            return ret;
        }

        Widget _buildNewMessageNotification() {
            Widget ret = new Container(
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
                child: new Text(
                    $"{CStringUtils.CountToString(this.widget.viewModel.newMessageCount)}条新消息未读",
                    style: CTextStyle.PRegularWhite.copyWith(height: 1f)
                )
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
                new Text(
                    data: this.widget.viewModel.channel.name,
                    style: CTextStyle.PXLargeMedium
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
            return ListView.builder(
                padding: EdgeInsets.only(top: 16, bottom: this.inputBarHeight + 16),
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

            if (message.type == ChannelMessageType.text) {
                ret = new TipMenu(
                    new List<TipMenuItem> {
                        new TipMenuItem(
                            "复制",
                            () => Clipboard.setData(new ClipboardData(text: message.content))
                        )
                    },
                    child: ret
                );
            }

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

            var httpsUrl = user.avatar ?? "";
            // fix Android 9 http request error 
            if (httpsUrl.Contains("http://")) {
                httpsUrl = httpsUrl.Replace("http://", "https://");
            }

            return new Container(
                padding: EdgeInsets.symmetric(0, 10),
                child: new GestureDetector(
                    onTap: () => this.widget.actionModel.pushToUserDetail(user.id),
                    onLongPress: () => {
                        var userName = user.fullName;
                        var userId = user.id;
                        var newContent = this._textController.text + "@" + userName + " ";
                        this._inputContentManager.AddMention(userName + " ", userId,
                            newContent);
                        this._textController.value = new TextEditingValue(
                            text: newContent,
                            TextSelection.collapsed(newContent.Length)
                        );
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
                                        color: Colors.white,
                                        child: new _Placeholder(
                                            user.id ?? "",
                                            user.fullName ?? "",
                                            size: avatarSize
                                        )
                                    )
                                    : new Container(
                                        padding: EdgeInsets.all(1.0f / Window.instance.devicePixelRatio),
                                        color: Colors.white,
                                        child: CachedNetworkImageProvider.cachedNetworkImage(src: httpsUrl)
                                    ),
                                Positioned.fill(
                                    Image.asset(
                                        "image/avatar-circle-1",
                                        fit: BoxFit.cover
                                    )
                                ),
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
                child: new _ImageMessage(
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

        Widget _buildPickImageButton() {
            return new CustomButton(
                padding: EdgeInsets.zero,
                onPressed: this._pickImage,
                child: new Container(
                    width: 44,
                    height: 49,
                    child: new Center(
                        child: new Icon(Icons.outline_photo_size_select_actual,
                            size: 28, color: CColors.Icon)
                    )
                )
            );
        }

        Widget _buildShowEmojiBoardButton() {
            return new CustomButton(
                padding: EdgeInsets.zero,
                onPressed: () => {
                    this.setState(() => {
                        this._refreshController.scrollController.jumpTo(0);
                        FocusScope.of(this.context).requestFocus(this._focusNode);
                        if (this.showEmojiBoard) {
                            TextInputPlugin.TextInputShow();
                            Promise.Delayed(TimeSpan.FromMilliseconds(200)).Then(
                                () => { this.setState(() => { this._showEmojiBoard = false; }); });
                        }
                        else {
                            this.setState(() => { this._showEmojiBoard = true; });
                            Promise.Delayed(TimeSpan.FromMilliseconds(100)).Then(
                                TextInputPlugin.TextInputHide
                            );
                        }
                    });
                },
                child: new Container(
                    width: 44,
                    height: 49,
                    child: new Center(
                        child: new Icon(this.showEmojiBoard
                                ? Icons.outline_keyboard
                                : Icons.mood,
                            size: 28, color: CColors.Icon)
                    )
                )
            );
        }

        Widget _buildInputBar() {
            var padding = this.showKeyboard || this.showEmojiBoard ? 0 : this.mPaddingBottom;
            Widget ret = new Container(
                padding: EdgeInsets.symmetric(0, 16),
                height: 32,
                decoration: new BoxDecoration(
                    CColors.Separator2,
                    borderRadius: BorderRadius.all(16)
                ),
                alignment: Alignment.centerLeft,
                child: new InputField(
                    key: this._focusNodeKey,
                    controller: this._textController,
                    focusNode: this._focusNode,
                    height: 32,
                    style: CTextStyle.PRegularBody,
                    hintText: "说点想法…",
                    hintStyle: CTextStyle.PRegularBody4,
                    keyboardType: TextInputType.multiline,
                    maxLines: 1,
                    cursorColor: CColors.PrimaryBlue,
                    textInputAction: TextInputAction.send,
                    onSubmitted: this._handleSubmit
                )
            );

            if (this.widget.viewModel.channel.sendingMessage) {
                ret = new Stack(
                    children: new List<Widget> {
                        ret,
                        new Positioned(
                            right: 8,
                            top: 0,
                            bottom: 0,
                            child: new Align(
                                alignment: Alignment.center,
                                child: new CustomActivityIndicator()
                            )
                        )
                    });
            }


            ret = new Container(
                padding: EdgeInsets.only(bottom: padding),
                decoration: new BoxDecoration(
                    border: new Border(new BorderSide(CColors.Separator)),
                    color: this.showEmojiBoard ? CColors.White : CColors.TabBarBg
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new Container(width: 16),
                        new Expanded(child: ret),
                        this._buildShowEmojiBoardButton(),
                        this._buildPickImageButton(),
                        new Container(width: 10)
                    }
                )
            );

            if (!this.showEmojiBoard && !this.showKeyboard) {
                ret = new BackdropFilter(
                    filter: ImageFilter.blur(10, 10),
                    child: ret
                );
            }

            ret = new Positioned(
                left: 0,
                right: 0,
                bottom: 0,
                child: ret
            );

            return ret;
        }

        static readonly List<int> emojiList = new List<int> {
            0x1F600, 0x1F603, 0x1F604, 0x1F601, 0x1F606, 0x1F605, 0x1F602, 0x1F923, 0x1F62D,
            0x1F617, 0x1F619, 0x1F61A, 0x1F618, 0x263A, 0x1F60A, 0x1F970, 0x1F60D, 0x1F929,
            0x1F917, 0x1F642, 0x1F643, 0x1F609, 0x1F60B, 0x1F61B, 0x1F61D, 0x1F61C, 0x1F92A,
            0x1F914, 0x1F928, 0x1F9D0, 0x1F644, 0x1F60F, 0x1F612, 0x1F623, 0x1F614, 0x1F60C,
            0x2639, 0x1F641, 0x1F615, 0x1F61F, 0x1F97A, 0x1F62C, 0x1F910, 0x1F92B, 0x1F92D,
            0x1F630, 0x1F628, 0x1F627, 0x1F626, 0x1F62E, 0x1F62F, 0x1F632, 0x1F633, 0x1F92F,
            0x1F622, 0x1F625, 0x1F613, 0x1F61E, 0x1F616, 0x1F629, 0x1F62B, 0x1F635, 0x1F631,
            0x1F922, 0x1F92E, 0x1F927, 0x1F637, 0x1F974, 0x1F912, 0x1F915, 0x1F975, 0x1F976,
            0x1F636, 0x1F610, 0x1F611, 0x1F624, 0x1F924, 0x1F634, 0x1F62A, 0x1F607, 0x1F920,
            0x1F973, 0x1F60E, 0x1F913, 0x1F925
        };

        static readonly int emojiBoardRowSize = 8;
        static readonly int emojiBoardColumSize = 3;

        static int emojiBoardPageSize {
            get { return emojiBoardRowSize * emojiBoardColumSize - 1; }
        }

        float emojiSize {
            get {
                return (MediaQuery.of(this.context).size.width - 42 - (emojiBoardRowSize - 1) * 2) / emojiBoardRowSize;
            }
        }

        List<Widget> _buildEmojiBoardPages() {
            List<Widget> emojiPages = new List<Widget>();
            for (int i = 0; i < emojiList.Count; i += emojiBoardPageSize) {
                List<Widget> rows = new List<Widget>();
                for (int j = 0; j < emojiBoardColumSize; j++) {
                    List<Widget> emojis = new List<Widget>();
                    emojis.Add(new Container(width: 21));
                    for (int k = 0; k < emojiBoardRowSize; k++) {
                        emojis.Add(j == emojiBoardColumSize - 1 && k == emojiBoardRowSize - 1
                            ? this._buildDeleteKey(EdgeInsets.only(left: 2))
                            : this._buildEmojiButton(i, j, k));
                    }

                    emojis.Add(new Container(width: 21));
                    if (j > 0) {
                        rows.Add(new Container(height: 8));
                    }

                    rows.Add(new Row(
                        children: emojis
                    ));
                }

                emojiPages.Add(new Container(
                    width: MediaQuery.of(this.context).size.width,
                    child: new Column(
                        children: rows
                    )
                ));
            }

            return emojiPages;
        }

        Widget _buildEmojiBoard() {
            return new Column(
                children: new List<Widget> {
                    new Container(
                        decoration: new BoxDecoration(
                            color: CColors.White,
                            border: new Border(new BorderSide(color: CColors.Separator))
                        ),
                        padding: EdgeInsets.symmetric(24),
                        child: new Column(
                            children: new List<Widget> {
                                new Container(
                                    height: 136,
                                    child: new TabBarView(
                                        controller: this._emojiTabController,
                                        children: this._buildEmojiBoardPages()
                                    )
                                ),
                                new Container(height: 16),
                                new CustomTabPageSelector(
                                    controller: this._emojiTabController,
                                    indicatorSize: 8,
                                    selectedColor: CColors.PrimaryBlue,
                                    color: CColors.Disable2
                                )
                            }
                        )
                    ),
                    new Container(
                        color: CColors.EmojiBottomBar,
                        height: 36,
                        child: new Row(
                            children: new List<Widget> {
                                new Container(width: 16),
                                new GestureDetector(
                                    child: new Container(height: 36, width: 44,
                                        decoration: new BoxDecoration(color: CColors.White),
                                        child: new Center(
                                            child: new Text(
                                                char.ConvertFromUtf32(0x1f642),
                                                style: new TextStyle(fontSize: 24, height: 1))))
                                ),
                                new Expanded(child: new Container()),
                                new GestureDetector(
                                    onTap: () => this._handleSubmit(this._textController.text),
                                    child: new Container(
                                        width: 60, height: 16,
                                        decoration: new BoxDecoration(
                                            border: new Border(left: new BorderSide(color: CColors.Separator))
                                        ),
                                        child: new Center(
                                            child: new Text("发送",
                                                style: CTextStyle.PMediumBlue.copyWith(height: 1)))
                                    )
                                )
                            }
                        )
                    ),
                    new Container(
                        color: CColors.EmojiBottomBar,
                        height: this.mPaddingBottom
                    )
                }
            );
        }

        Widget _buildDeleteKey(EdgeInsets padding) {
            return new GestureDetector(
                onTap: this._handleDelete,
                child: new Container(
                    width: 40,
                    height: 40,
                    padding: padding,
                    child: new Center(
                        child: new Icon(
                            Icons.outline_delete_keyboard,
                            size: 24,
                            color: CColors.Icon
                        )
                    )
                )
            );
        }

        string getEmojiText(int index) {
            if (index < emojiList.Count) {
                return emojiList[index] > 0x10000
                    ? char.ConvertFromUtf32(emojiList[index])
                    : $"{(char) emojiList[index]}";
            }

            return "";
        }

        Widget _buildEmojiButton(int i, int j, int k) {
            int index = i + j * emojiBoardRowSize + k;
            return new GestureDetector(
                onTap: this.getEmojiText(index) != ""
                    ? (GestureTapCallback) (() => {
                        var selection = this._textController.selection;
                        this._textController.value = new TextEditingValue(
                            this._textController.text.Substring(0, selection.start) +
                            this.getEmojiText(index) + this._textController.text.Substring(selection.end),
                            TextSelection.collapsed(selection.start + this.getEmojiText(index).Length));
                    })
                    : null,
                child: new Container(
                    width: this.emojiSize,
                    height: this.emojiSize,
                    padding: k == 0 ? EdgeInsets.zero : EdgeInsets.only(left: 2),
                    child: new Center(
                        child: new Text(
                            this.getEmojiText(index),
                            style: new TextStyle(fontSize: 24, height: 1)
                        )
                    )
                )
            );
        }

        int codeUnitLengthAt(TextEditingValue value) {
            return value.selection.start > 1 && char.IsSurrogate(value.text[value.selection.start - 1]) ? 2 : 1;
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

            text = this._inputContentManager.ToMessage(
                this.widget.viewModel.mentionSuggestion, this.widget.viewModel.channel.membersDict, text);
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
            this._inputContentManager.Clear();
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

    class _ImageMessage : StatefulWidget {
        public readonly string url;
        public readonly float size;
        public readonly float ratio;
        public readonly float radius;
        public readonly float srcWidth;
        public readonly float srcHeight;
        public readonly Dictionary<string, string> headers;

        public _ImageMessage(
            string url,
            float size,
            float ratio,
            float srcWidth = 0,
            float srcHeight = 0,
            Dictionary<string, string> headers = null,
            float radius = 10) {
            this.url = url;
            this.size = size;
            this.ratio = ratio;
            this.radius = radius;
            this.headers = headers;
            this.srcWidth = srcWidth;
            this.srcHeight = srcHeight;
        }

        public Size srcSize {
            get {
                if (this.srcWidth != 0 && this.srcHeight != 0) {
                    return new Size(this.srcWidth, this.srcHeight);
                }

                return null;
            }
        }

        public override State createState() {
            return new _ImageMessageState();
        }
    }

    class _ImageMessageState : State<_ImageMessage> {
        Image image;
        Size size;
        ImageStream stream;

        Size _finalSize(Size size) {
            if (size.width > size.height * this.widget.ratio) {
                return new Size(
                    width: this.widget.size,
                    height: this.widget.size / this.widget.ratio);
            }

            if (size.width > size.height) {
                return new Size(
                    width: this.widget.size,
                    height: this.widget.size / size.width * size.height);
            }

            if (size.width > size.height / this.widget.ratio) {
                return new Size(
                    width: this.widget.size / size.height * size.width,
                    height: this.widget.size);
            }

            return new Size(
                width: this.widget.size / this.widget.ratio,
                height: this.widget.size);
        }

        void _updateSize(ImageInfo info, bool _) {
            this.size = this._finalSize(new Size(info.image.width, info.image.height));

            this.setState(() => { });
        }

        public override void initState() {
            base.initState();
            if (this.widget.srcSize == null) {
                this.image = CachedNetworkImageProvider.cachedNetworkImage(
                    src: CImageUtils.SizeToScreenImageUrl(this.widget.url),
                    headers: this.widget.headers);
                this.stream = this.image.image
                    .resolve(new ImageConfiguration());
                this.stream.addListener(this._updateSize);
            }
            else {
                this.size = this._finalSize(this.widget.srcSize);
            }
        }

        public override void dispose() {
            this.stream?.removeListener(this._updateSize);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            return this.size == null || this.widget.url == null
                ? new Container(
                    width: this.widget.size,
                    height: this.widget.size,
                    decoration: new BoxDecoration(
                        color: CColors.Disable,
                        borderRadius: BorderRadius.all(this.widget.radius)
                    ))
                : (Widget) new ClipRRect(
                    borderRadius: BorderRadius.all(this.widget.radius),
                    child: new Container(
                        width: this.size.width,
                        height: this.size.height,
                        color: CColors.Disable,
                        child: CachedNetworkImageProvider.cachedNetworkImage(
                            CImageUtils.SizeToScreenImageUrl(this.widget.url),
                            headers: this.widget.headers,
                            fit: BoxFit.cover))
                );
        }
    }
}