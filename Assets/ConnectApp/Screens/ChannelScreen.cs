using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Avatar = ConnectApp.Components.Avatar;
using Config = ConnectApp.Constants.Config;
using Icons = ConnectApp.Constants.Icons;
using Image = Unity.UIWidgets.widgets.Image;
using Transform = Unity.UIWidgets.widgets.Transform;

namespace ConnectApp.screens {
    public class ChannelScreenConnector : StatelessWidget {
        public ChannelScreenConnector(
            string channelId,
            Key key = null
        ) : base(key: key) {
            this.channelId = channelId;
        }

        public readonly string channelId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelScreenViewModel>(
                converter: state => {
                    return new ChannelScreenViewModel {
                        channelInfo = state.channelState.channelDict[this.channelId],
                        messages = state.channelState.channelDict[this.channelId].messageIds.Select(
                            messageId => {
                                var message = state.channelState.messageDict[messageId];
                                message.content = MessageUtils.AnalyzeMessage(
                                        message.content, message.mentions, message.mentionEveryone);
                                return message;
                            }).ToList(),
                        me = state.loginState.loginInfo.userId,
                        messageLoading = state.channelState.messageLoading,
                        newMessageCount = state.channelState.channelDict[this.channelId].unread
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        fetchMessages = (before, after) => {
                            return dispatcher.dispatch<IPromise>(
                                Actions.fetchChannelMessages(this.channelId, before, after));
                        },
                        fetchMembers = () => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelMembers(this.channelId, 0)),
                        pushToChannelDetail = () => {
                            dispatcher.dispatch(new MainNavigatorPushToChannelDetailAction {
                                channelId = this.channelId
                            });
                        },
                        sendMessage = (channelId, content, nonce, parentMessageId) => dispatcher.dispatch<IPromise>(
                            Actions.sendMessage(channelId, content, nonce, parentMessageId)),
                        startSendMessage = () => dispatcher.dispatch(new StartSendChannelMessageAction()),
                        sendImage = (channelId, data, nonce) => dispatcher.dispatch<IPromise>(
                            Actions.sendImage(channelId, nonce, data)),
                        clearUnread = () => dispatcher.dispatch(new ClearChannelUnreadAction {
                            channelId = this.channelId
                        }),
                        updateScrollOffset = (bottom, top) => dispatcher.dispatch(new UpdateChannelScrollOffsetAction {
                            channelId = this.channelId,
                            bottom = bottom,
                            top = top
                        }),
                        reportHitBottom = () => dispatcher.dispatch(new ChannelScreenHitBottom {channelId = this.channelId}),
                        reportLeaveBottom = () => dispatcher.dispatch(new ChannelScreenLeaveBottom {channelId = this.channelId})
                        
                    };
                    return new ChannelScreen(viewModel, actionModel);
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

    class _ChannelScreenState : TickerProviderStateMixin<ChannelScreen> {
        readonly TextEditingController _textController = new TextEditingController();
        readonly RefreshController _refreshController = new RefreshController();
        TabController _emojiTabController;
        FocusNode _focusNode;
        GlobalKey _focusNodeKey;

        Dictionary<string, string> _jobRole;
        float messageBubbleWidth = 0;
        long lastSavedNonce;
        bool _showEmojiBoard = false;
        string _pickImageSubId;
        string _pickedImage;
        Dictionary<string, string> headers;


        float inputBarHeight {
            get { return this.showKeyboard || this.showEmojiBoard ? 48 : 48 + 34; }
        }

        bool showKeyboard {
            get { return MediaQuery.of(this.context).viewInsets.bottom > 10; }
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

        const string COOKIE = "Cookie";
        static string _cookieHeader() {
            if (PlayerPrefs.GetString(COOKIE).isNotEmpty()) {
                return PlayerPrefs.GetString(COOKIE);
            }

            return "";
        }

        public override void initState() {
            base.initState();
            this._emojiTabController = new TabController(
                length: (this.emojiList.Count-1) / (24-1) + 1,
                vsync: this);
            this._pickImageSubId = EventBus.subscribe(sName: EventBusConstant.pickAvatarSuccess, args => {
                this._pickedImage = (string) args[0];
                this.widget.actionModel.sendImage(
                    this.widget.viewModel.channelInfo.id,
                    this._pickedImage,
                    Snowflake.CreateNonceLocal());
                this.setState();
            });
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this._refreshController.scrollController.addListener(this._handleScrollListener);
                this.widget.actionModel.fetchMessages(null, null);
                this.widget.actionModel.fetchMembers();
            });
            this._focusNode = new FocusNode();
            this._focusNode.addListener(this._handleFocusNodeFocused);
            this._focusNodeKey = GlobalKey.key("_channelFocusNodeKey");
            this.headers = new Dictionary<string, string> {
                {COOKIE, _cookieHeader()},
                {"AppVersion", Config.versionNumber},
                {"X-Requested-With", "XmlHttpRequest"}
            };
        }

        public override void dispose() {
            this._textController.dispose();
            this._emojiTabController.dispose();
            EventBus.unSubscribe(EventBusConstant.pickAvatarSuccess, this._pickImageSubId);
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.clearUnread();
            });
            this._focusNode.dispose();
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            this.messageBubbleWidth = MediaQuery.of(context).size.width * 0.7f;
            Widget newMessage = Positioned.fill(
                child: new Align(
                    alignment: Alignment.bottomCenter,
                    child: new Column(
                        mainAxisSize: MainAxisSize.min,
                        children: new List<Widget> {
                            new GestureDetector(
                                onTap: () => this._refreshController.scrollTo(0),
                                child: new Container(
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
                                    child: new Text(
                                        $"{CStringUtils.CountToString(this.widget.viewModel.newMessageCount)}条新消息未读",
                                        style: CTextStyle.PRegularWhite.copyWith(height: 1.2f)
                                    )
                                )
                            ),
                            new Container(height: this.inputBarHeight + 16)
                        }
                    )
                )
            );

            Widget mainPart = new Stack(
                children: new List<Widget> {
                    this._buildContent(),
                    this._buildInputBar(),
                    this.widget.viewModel.newMessageCount == 0 || this.widget.viewModel.messageLoading
                        ? new Container()
                        : newMessage
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
                                new Flexible(child: mainPart),
                                this.showEmojiBoard
                                    ? this._buildEmojiBoard()
                                    : new Container(height: this.showKeyboard
                                        ? MediaQuery.of(this.context).viewInsets.bottom
                                        : 0)
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomAppBar(
                onBack: () => this.widget.actionModel.mainRouterPop(),
                new Text(
                    this.widget.viewModel.channelInfo.name,
                    style: CTextStyle.PXLargeMedium
                ),
                rightWidget: new CustomButton(
                    onPressed: () => { this.widget.actionModel.pushToChannelDetail(); },
                    child: new Container(
                        width: 28,
                        height: 28,
                        child: new Icon(Icons.ellipsis, color: CColors.Icon, size: 28)
                    )
                )
            );
        }

        Widget _buildContent() {
            Widget ret = new Container(
                color: CColors.White,
                child: new SmartRefresher(
                    controller: this._refreshController,
                    enablePullDown: false,
                    enablePullUp: true,
                    onRefresh: this._onRefresh,
                    // onOffsetChange: this._handleScrollListener,
                    reverse: true,
                    headerBuilder: (context, mode) => new SmartRefreshHeader(mode),
                    child: this.widget.viewModel.messageLoading && this.widget.viewModel.messages.isEmpty()
                           ? this._buildLoadingPage()
                           : this._buildMessageListView()
                )
            );
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
                        showTime: index == 0 || (message.time -
                                                 this.widget.viewModel.messages[index - 1].time) >
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
                        height: MediaQuery.of(this.context).size.height - 100
                    )
                });
        }

        Widget _buildMessage(ChannelMessageView message, bool showTime, bool left) {
            if (message.deleted) {
                return new Container();
            }

            Widget avatar = new Container(
                padding: EdgeInsets.symmetric(0, 10),
                child: Avatar.User(message.author, size: 40)
            );

            Widget messageContent = new Container();
            switch (message.type) {
                case ChannelMessageType.text:
                    if (string.IsNullOrEmpty(message.content)) {
                        return new Container();
                    }
                    messageContent = new Text(message.content, style: CTextStyle.PLargeBody);
                    break;
                case ChannelMessageType.image:
                    messageContent = new _ImageMessage(
                        url: message.content,
                        size: 140,
                        ratio: 16.0f / 9.0f,
                        headers: this.headers
                    );
                    break;
                case ChannelMessageType.file:
                    messageContent = new Container(
                        padding: EdgeInsets.symmetric(12, 16),
                        child: new Text("[你收到一个文件，请在浏览器上查看]", style: CTextStyle.PLargeBody5)
                    );
                    break;
                case ChannelMessageType.embed:
                    var content = message.content;
                    Widget contentWidget = new Text(content, style: CTextStyle.PLargeBody);
                    if (message.embeds[0].embedData.url != null && content.Contains(message.embeds[0].embedData.url)) {
                        int startIndex = content.IndexOf(message.embeds[0].embedData.url, StringComparison.Ordinal);
                        int endIndex = startIndex + message.embeds[0].embedData.url.Length;
                        string prev = content.Substring(0, startIndex);
                        string post = content.Substring(endIndex);
                        contentWidget = new RichText(text: new TextSpan(
                            prev,
                            style: CTextStyle.PLargeBody,
                            children: new List<TextSpan> {
                                new TextSpan(message.embeds[0].embedData.url, style: CTextStyle.PLargeBlue),
                                new TextSpan(post, style: CTextStyle.PLargeBody),
                            }
                        ));
                    }

                    messageContent = new Container(
                        padding: EdgeInsets.all(12),
                        child: new Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                contentWidget,
                                new Container(height: 12),
                                new Container(
                                    padding: EdgeInsets.all(12),
                                    color: CColors.White,
                                    child: new Column(
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Text(message.embeds[0].embedData.title ?? "",
                                                style: CTextStyle.PLargeMediumBlue),
                                            new Container(height: 4),
                                            message.embeds[0].embedData.description == null
                                                ? new Container()
                                                : new Container(
                                                    padding: EdgeInsets.only(bottom: 4),
                                                    child: new Text(message.embeds[0].embedData.description ?? "",
                                                        style: CTextStyle.PRegularBody3, maxLines: 4,
                                                        overflow: TextOverflow.ellipsis)),
                                            new Row(
                                                crossAxisAlignment: CrossAxisAlignment.center,
                                                children: new List<Widget> {
                                                    message.embeds[0].embedData.image == null
                                                        ? (Widget) new Container(width: 14, height: 14)
                                                        : Image.network(message.embeds[0].embedData.image ?? "",
                                                            width: 14, height: 14, fit: BoxFit.cover),
                                                    new Container(width: 4),
                                                    new Expanded(
                                                        child: new Text(message.embeds[0].embedData.name ?? "",
                                                            style: CTextStyle.PMediumBody,
                                                            overflow: TextOverflow.ellipsis)
                                                    )
                                                }
                                            )
                                        }
                                    )
                                )
                            }
                        )
                    );
                    break;
            }

            messageContent = new Column(
                crossAxisAlignment: left ? CrossAxisAlignment.start : CrossAxisAlignment.end,
                children: new List<Widget> {
                    new Container(
                        padding: EdgeInsets.only(bottom: 6),
                        child: new Text(message.author.fullName, style: CTextStyle.PSmallBody4)
                    ),
                    new Container(
                        constraints: new BoxConstraints(
                            maxWidth: this.messageBubbleWidth
                        ),
                        padding: message.type == ChannelMessageType.text
                            ? EdgeInsets.symmetric(8, 12)
                            : EdgeInsets.zero,
                        decoration: message.type == ChannelMessageType.image
                            ? null
                            : new BoxDecoration(
                                color: left || message.type == ChannelMessageType.file
                                    ? CColors.VeryLightPinkThree
                                    : CColors.PaleSkyBlue,
                                borderRadius: BorderRadius.all(10)
                            ),
                        child: messageContent
                    )
                }
            );

            return new Column(
                crossAxisAlignment: left ? CrossAxisAlignment.start : CrossAxisAlignment.end,
                children: new List<Widget> {
                    showTime
                        ? new Container(
                            height: 36,
                            padding: EdgeInsets.only(bottom: 16),
                            child: new Center(
                                child: new Text(
                                    message.time.DateTimeString(),
                                    style: CTextStyle.PSmallBody5
                                )
                            )
                        )
                        : new Container(),
                    new Container(
                        padding: EdgeInsets.only(left: 2, right: 2, bottom: 16),
                        child: new Row(
                            mainAxisAlignment: left ? MainAxisAlignment.start : MainAxisAlignment.end,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: left
                                ? new List<Widget> {avatar, messageContent}
                                : new List<Widget> {messageContent, avatar}
                        )
                    )
                }
            );
        }

        Widget _buildInputBar() {
            Widget ret = new Container(
                padding: EdgeInsets.only(bottom: this.showKeyboard || this.showEmojiBoard ? 0 : 34),
                decoration: new BoxDecoration(
                    border: new Border(new BorderSide(CColors.Separator)),
                    color: this.showEmojiBoard ? CColors.White : CColors.TabBarBg
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new Container(width: 16),
                        new Expanded(
                            child: new Container(
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
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.zero,
                            onPressed: () => { this.setState(() => {
                                this._refreshController.scrollController.jumpTo(0);
                                FocusScope.of(this.context).requestFocus(this._focusNode);
                                if (this.showEmojiBoard) {
#if UNITY_ANDROID || UNITY_IOS
                                    UIWidgetsTextInputShow();
#endif
                                    Promise.Delayed(TimeSpan.FromMilliseconds(200)).Then(
                                        () => {
                                            this.setState(() => { this._showEmojiBoard = false; });
                                        });
                                }
                                else {
                                    this.setState(() => {
                                        this._showEmojiBoard = true;
                                    });
                                    Promise.Delayed(TimeSpan.FromMilliseconds(100)).Then(
                                        () => {
#if UNITY_ANDROID || UNITY_IOS
                                            UIWidgetsTextInputHide();
#endif
                                        }
                                    );
                                }
                            }); },
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
                        ),
                        new CustomButton(
                            padding: EdgeInsets.zero,
                            onPressed: () => {
                                PickImageManager.showActionSheet();
                            },
                            child: new Container(
                                width: 44,
                                height: 49,
                                child: new Center(
                                    child: new Icon(Icons.outline_photo_size_select_actual, size: 28,
                                        color: CColors.Icon)
                                )
                            )
                        ),
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

        List<int> emojiList = new List<int> {
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

        Widget _buildEmojiBoard() {
            List<Widget> emojiPages = new List<Widget>();
            for (int i = 0; i < this.emojiList.Count; i += 23) {
                List<Widget> rows = new List<Widget>();
                for (int j = 0; j < 3; j++) {
                    List<Widget> emojis = new List<Widget>();
                    emojis.Add(new Flexible(child: new Container()));
                    for (int k = 0; k < 8; k++) {
                        int index = i + j * 8 + k;
                        string text;
                        if (index < this.emojiList.Count) {
                            if (this.emojiList[index] > 0x10000) {
                                text = char.ConvertFromUtf32(this.emojiList[index]);
                            }
                            else {
                                text = $"{(char) this.emojiList[index]}";
                            }
                        }
                        else {
                            text = "";
                        }

                        if (j == 2 && k == 7) {
                            emojis.Add(new GestureDetector(
                                onTap: this._handleDelete,
                                child: new Container(
                                    width: 40,
                                    height: 40,
                                    padding: k == 0 ? EdgeInsets.zero : EdgeInsets.only(left: 2),
                                    child: new Center(
                                        child: new Icon(
                                            Icons.outline_delete_keyboard,
                                            size: 24,
                                            color: CColors.Icon
                                        )
                                    )
                                )
                            ));
                        }
                        else {
                            emojis.Add(new GestureDetector(
                                onTap: text != ""
                                    ? (GestureTapCallback) (() => {
                                        var selection = this._textController.selection;
                                        this._textController.text =
                                            this._textController.text.Substring(0, selection.start) +
                                            text +
                                            this._textController.text.Substring(selection.end);
                                        this._textController.selection =
                                            TextSelection.collapsed(selection.start + text.Length);
                                    })
                                    : null,
                                child: new Container(
                                    width: 40,
                                    height: 40,
                                    padding: k == 0 ? EdgeInsets.zero : EdgeInsets.only(left: 2),
                                    child: new Center(
                                        child: new Text(
                                            text,
                                            style: new TextStyle(fontSize: 24, height: 1)
                                        )
                                    )
                                )
                            ));
                        }
                    }
                    emojis.Add(new Flexible(child: new Container()));
                    if (j > 0) {
                        rows.Add(new Container(height: 8));
                    }
                    rows.Add(new Row(
                        children: emojis
                    ));
                }
                Widget page = new Container(
                    width: MediaQuery.of(this.context).size.width,
                    child: new Column(
                        children: rows
                    )
                );
                emojiPages.Add(page);
            }
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
                                        children: emojiPages
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
                                        child: new Center(child: new Icon(Icons.outline_time, size: 20)))
                                ),
                                new GestureDetector(
                                    child: new Container(height: 36, width: 44,
                                        decoration: new BoxDecoration(color: CColors.White),
                                        child: new Center(child: new Text(char.ConvertFromUtf32(0x1f642),
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
                    )
                }
            );
        }

        void _handleDelete() {
            var selection = this._textController.selection;
            if (selection.isCollapsed) {
                if (selection.start > 0) {
                    int deleteLength = 1;
                    if (selection.start > 1 && char.IsSurrogate(this._textController.text[selection.start - 1])) {
                        deleteLength = 2;
                    }
                    this._textController.text =
                        this._textController.text.Substring(0, selection.start-deleteLength) +
                        this._textController.text.Substring(selection.end);
                    this._textController.selection = TextSelection.collapsed(selection.start-deleteLength);
                }
            }
            else {
                this._textController.text =
                    this._textController.text.Substring(0, selection.start) +
                    this._textController.text.Substring(selection.end);
                this._textController.selection =
                    TextSelection.collapsed(selection.start);
            }
        }

        void _handleSubmit(string text) {
            if (text == "") {
                return;
            }
            this.widget.actionModel.startSendMessage();
            this.widget.actionModel.sendMessage(this.widget.viewModel.channelInfo.id, text, Snowflake.CreateNonceLocal(), "")
                .Catch(_ => { CustomDialogUtils.showToast("消息发送失败", Icons.error_outline); })
                .Then(
                    () => {
                        this.setState(() => this._textController.clear());
                    });
            this._refreshController.scrollTo(0);
        }

        void _handleFocusNodeFocused() {
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
        void _handleScrollListener() {
            if (this._refreshController.offset <= 0) {
                if (this._lastScrollPosition == null || this._lastScrollPosition > 0) {
                    this.widget.actionModel.reportHitBottom();
                }
            }
            else if (this._refreshController.offset > 0) {
                if (this._lastScrollPosition == null || this._lastScrollPosition <= 0) {
                    this.widget.actionModel.reportLeaveBottom();
                }
            }

            if (this._lastScrollPosition != null && this._lastScrollPosition < this._refreshController.offset) {
                this.setState(() => {
                    this._showEmojiBoard = false;
#if UNITY_ANDROID || UNITY_IOS
                    UIWidgetsTextInputHide();
#endif
                });
            }
            this._lastScrollPosition = this._refreshController.offset;
        }
        
#if UNITY_IOS
        [DllImport ("__Internal")]
        internal static extern void UIWidgetsTextInputShow();
        
        [DllImport ("__Internal")]
        internal static extern void UIWidgetsTextInputHide();
        
        [DllImport ("__Internal")]
        internal static extern void UIWidgetsTextInputSetClient(int client, string configuration);
        
        [DllImport ("__Internal")]
        internal static extern void UIWidgetsTextInputSetTextInputEditingState(string jsonText);
        
        [DllImport ("__Internal")]
        internal static extern void UIWidgetsTextInputClearTextInputClient();
#elif UNITY_ANDROID
        internal static void UIWidgetsTextInputShow() {
            using (
                AndroidJavaClass pluginClass = new AndroidJavaClass("com.unity.uiwidgets.plugin.editing.TextInputPlugin")
            ) {
                pluginClass.CallStatic("show");
            }
        }
        
        internal static void UIWidgetsTextInputHide() {
            using (
                AndroidJavaClass pluginClass = new AndroidJavaClass("com.unity.uiwidgets.plugin.editing.TextInputPlugin")
            ) {
                pluginClass.CallStatic("hide");
            }
        }
#endif
    }

    class _ImageMessage : StatefulWidget {
        public readonly string url;
        public readonly float size;
        public readonly float ratio;
        public readonly float radius;
        public readonly Dictionary<string, string> headers;

        public _ImageMessage(
            string url,
            float size,
            float ratio,
            Dictionary<string, string> headers = null,
            float radius = 10) {
            this.url = url;
            this.size = size;
            this.ratio = ratio;
            this.radius = radius;
            this.headers = headers;
        }

        public override State createState() {
            return new _ImageMessageState();
        }
    }

    class _ImageMessageState : State<_ImageMessage> {
        Image image;
        Size size;
        ImageStream stream;

        void _updateSize(ImageInfo info, bool _) {
            if (info.image.width > info.image.height * this.widget.ratio) {
                this.size = new Size(this.widget.size, this.widget.size / this.widget.ratio);
            }
            else if (info.image.width > info.image.height) {
                this.size = new Size(this.widget.size,
                    this.widget.size / info.image.width * info.image.height);
            }
            else if (info.image.width > info.image.height / this.widget.ratio) {
                this.size = new Size(this.widget.size / info.image.height * info.image.width,
                    this.widget.size);
            }
            else {
                this.size = new Size(this.widget.size / this.widget.ratio, this.widget.size);
            }

            this.setState(() => { });
        }

        public override void initState() {
            base.initState();
            this.image = Image.network(this.widget.url, headers: this.widget.headers);
            this.stream = this.image.image
                .resolve(new ImageConfiguration());
            this.stream.addListener(this._updateSize);
        }

        public override void dispose() {
            this.stream.removeListener(this._updateSize);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            return this.size == null || this.widget.url == null
                ? new Container(width: this.widget.size, height: this.widget.size, decoration: new BoxDecoration(
                    color: CColors.Disable,
                    borderRadius: BorderRadius.all(this.widget.radius)
                ))
                : (Widget) new ClipRRect(
                    borderRadius: BorderRadius.all(this.widget.radius),
                    child: new Container(
                        width: this.size.width,
                        height: this.size.height,
                        child: Image.network(this.widget.url,
                            fit: BoxFit.cover))
                );
        }
    }
}