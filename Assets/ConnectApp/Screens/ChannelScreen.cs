using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Avatar = ConnectApp.Components.Avatar;
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
                            messageId => state.channelState.messageDict[messageId]
                        ).ToList(),
                        me = state.loginState.loginInfo.userId,
                        messageLoading = state.channelState.messageLoading,
                        newMessageCount = 0
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        fetchMessages = (before, after) => {
                            return dispatcher.dispatch<IPromise>(Actions.fetchChannelMessages(this.channelId, before, after));
                        },
                        pushToChannelDetail = () => {
                            dispatcher.dispatch(new MainNavigatorPushToChannelDetailAction {
                                channelId = this.channelId
                            });
                        },
                        sendMessage = (channelId, content, nonce, parentMessageId) => dispatcher.dispatch<IPromise>(
                            Actions.sendMessage(channelId, content, nonce, parentMessageId)),
                        startSendMessage = () => dispatcher.dispatch(new StartSendChannelMessageAction()),
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

    class _ChannelScreenState : State<ChannelScreen> {
        readonly TextEditingController _textController = new TextEditingController();
        readonly FocusNode _focusNode = new FocusNode();
        readonly RefreshController _refreshController = new RefreshController();

        Dictionary<string, string> _jobRole;
        float messageBubbleWidth = 0;

        public override void initState() {
            base.initState();
            this.widget.actionModel.fetchMessages(null, null);
        }

        public override void dispose() {
            this._textController.dispose();
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            this.messageBubbleWidth = MediaQuery.of(context).size.width * 0.7f;
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Stack(
                        children: new List<Widget> {
                            new Container(
                                color: CColors.Background,
                                child: new Column(
                                    children: new List<Widget> {
                                        this._buildNavigationBar(),
                                        this._buildContent(),
                                    }
                                )
                            ),
                            this._buildInputBar(),
                            this.widget.viewModel.newMessageCount == 0
                                ? (Widget) new Container()
                                : Positioned.fill(
                                    child: new Align(
                                        alignment: Alignment.bottomCenter,
                                        child: new Transform(
                                            transform: Matrix3.makeTrans(0, -99),
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
                                        )
                                    )
                                ),
                        }
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
            if (this.widget.viewModel.messageLoading) {
                return new Flexible(
                    child: new GlobalLoading()
                );
            }
            
            List<Widget> messages = new List<Widget>();
            for (int i = 0; i < this.widget.viewModel.messages.Count; i++) {
                var message = this.widget.viewModel.messages[i];
                messages.Add(this._buildMessage(
                    message,
                    showTime: i == 0 || (message.time -
                                         this.widget.viewModel.messages[i - 1].time) > TimeSpan.FromMinutes(5),
                    left: message.author.id != this.widget.viewModel.me
                ));
            }

            Widget ret = new Container(
                color: CColors.White,
                padding: EdgeInsets.only(top: 16, bottom: 99),
                child: new SmartRefresher(
                    controller: this._refreshController,
                    enablePullDown: false,
                    enablePullUp: true,
                    onRefresh: this._onRefresh,
                    reverse: true,
                    headerBuilder: (context, mode) => new SmartRefreshHeader(mode), 
                    child: ListView.builder(
                        padding: EdgeInsets.symmetric(16, 0),
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
                    )
                )
            );

            ret = new Flexible(child: ret);

            return ret;
        }

        Widget _buildMessage(ChannelMessageView message, bool showTime, bool left) {
            if (message.deleted) {
                return new Container();
            }
            Widget avatar = new Container(
                padding: EdgeInsets.symmetric(0, 10),
                child: Avatar.User(message.author, size: 40)
            );
            if (message.type == ChannelMessageType.image) {
                Debug.Log($"Image: {message.content}");
            }

            Widget messageContent = new Container();
            switch (message.type) {
                case ChannelMessageType.text:
                    messageContent = new Text(message.content, style: CTextStyle.PLargeBody);
                    break;
                case ChannelMessageType.image:
                    messageContent = new _ImageMessage(
                        url: message.content,
                        size: 140,
                        ratio: 16.0f / 9.0f
                    );
                    break;
                case ChannelMessageType.file:
                    messageContent = new Container(
                        padding: EdgeInsets.symmetric(12, 16),
                        child: new Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                new Expanded(
                                    child: new Column(
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Text(message.content, style: CTextStyle.PLargeBody),
                                            new Container(height: 4),
                                            new Text(CStringUtils.FileSize(message.fileSize),
                                                style: CTextStyle.PSmallBody4)
                                        }
                                    )
                                ),
                                new Container(width: 16),
                                new Container(
                                    decoration: new BoxDecoration(
                                        color: CColors.VeryLightPinkThree,
                                        borderRadius: BorderRadius.all(16)
                                    ),
                                    width: 41.9f,
                                    height: 47.9f,
                                    child: Image.asset("image/pdf-file-icon")
                                )
                            }
                        )
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
                                                    new Text(message.embeds[0].embedData.name ?? "",
                                                        style: CTextStyle.PMediumBody)
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
                                    message.time.Date == DateTime.Today
                                        ? message.time.ToString("HH:mm")
                                        : message.time.Year == DateTime.Today.Year
                                            ? message.time.ToString("M月d日 HH:mm")
                                            : message.time.ToString("yyyy年M月d日 HH:mm"),
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
                padding: EdgeInsets.only(16, right: 10, bottom: 34),
                decoration: new BoxDecoration(
                    border: new Border(new BorderSide(CColors.Separator)),
                    color: CColors.TabBarBg
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
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
                                    // key: _textFieldKey,
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
                            onPressed: () => { },
                            child: new Container(
                                width: 44,
                                height: 49,
                                child: new Center(
                                    child: new Icon(Icons.mood, size: 28, color: CColors.Icon)
                                )
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.zero,
                            onPressed: () => { },
                            child: new Container(
                                width: 44,
                                height: 49,
                                child: new Center(
                                    child: new Icon(Icons.outline_photo_size_select_actual, size: 28,
                                        color: CColors.Icon)
                                )
                            )
                        ),
                    }
                )
            );

            ret = new BackdropFilter(
                filter: ImageFilter.blur(10, 10),
                child: ret
            );
            
            ret = new Positioned(
                left: 0,
                right: 0,
                bottom: 0,
                height: 83,
                child: ret
            );

            return ret;
        }

        void _handleSubmit(string text) {
            this.widget.actionModel.startSendMessage();
            this.widget.actionModel.sendMessage(this.widget.viewModel.channelInfo.id, text, Snowflake.CreateNonce(), "")
                .Catch(_ => { CustomDialogUtils.showToast("消息发送失败", Icons.error_outline); })
                .Then(
                    () => {
                        if (this.widget.viewModel.messages.isNotEmpty()) {
                            this.widget.actionModel.fetchMessages(null, this.widget.viewModel.messages.last().id);
                        }
                        else {
                            this.widget.actionModel.fetchMessages(null, null);
                        }

                        this.setState(() => this._textController.clear());
                    });
        }

        void _onRefresh(bool up) {
            if (!up) {
                string id = this.widget.viewModel.messages.isNotEmpty()
                    ? this.widget.viewModel.messages.first().id
                    : null;
                this.widget.actionModel.fetchMessages(id, null).Then(
                    () => this._refreshController.sendBack(true, RefreshStatus.completed)
                ).Catch(
                    (error) => this._refreshController.sendBack(true, RefreshStatus.failed)
                );
            }
        }
    }

    class _ImageMessage : StatefulWidget {
        public readonly string url;
        public readonly float size;
        public readonly float ratio;
        public readonly float radius;

        public _ImageMessage(string url, float size, float ratio, float radius = 10) {
            this.url = url;
            this.size = size;
            this.ratio = ratio;
            this.radius = radius;
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
            this.image = Image.network(this.widget.url);
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