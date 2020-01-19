using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Plugins;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public delegate void InputDone(string text);

    public class CustomInput : StatefulWidget {
        public CustomInput(
            string replyUserName = null,
            InputDone doneCallBack = null,
            Key key = null
        ) : base(key: key) {
            this.replyUserName = replyUserName;
            this.doneCallBack = doneCallBack;
        }

        public readonly string replyUserName;
        public readonly InputDone doneCallBack;

        public override State createState() {
            return new _CustomInputState();
        }
    }

    public class _CustomInputState : State<CustomInput> {
        readonly TextEditingController _textController = new TextEditingController("");
        readonly GlobalKey _inputFieldKey = GlobalKey.key("inputFieldKey");
        bool _showEmojiBoard;

        static int _codeUnitLengthAt(TextEditingValue value) {
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
                        this._textController.text.Substring( 0,
                                  selection.start - _codeUnitLengthAt(value: this._textController.value)) +
                              this._textController.text.Substring(startIndex: selection.end),
                        TextSelection.collapsed(selection.start - _codeUnitLengthAt(value: this._textController.value)));
                }
            }
            else {
                this._textController.value = new TextEditingValue(
                    this._textController.text.Substring(0, length: selection.start) +
                    this._textController.text.Substring(startIndex: selection.end),
                    TextSelection.collapsed(offset: selection.start));
            }
        }

        void _onSubmitted(string text) {
            if (string.IsNullOrWhiteSpace(value: text)) {
                CustomDialogUtils.showToast("不能发送空消息", Icons.error_outline);
                return;
            }

            this.widget.doneCallBack?.Invoke(text: text);
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

        bool showKeyboard {
            get { return MediaQuery.of(context: this.context).viewInsets.bottom > 50; }
        }

        public override Widget build(BuildContext context) {
            return new Container(
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: new List<Widget> {
                        new Container(
                            color: Color.fromRGBO(0, 0, 0, 0.4f),
                            child: new Container(
                                color: CColors.White,
                                child: new Column(
                                    children: new List<Widget> {
                                        this._buildReplyWidget(context: context),
                                        this._buildTextField()
                                    }
                                )
                            )
                        ),
                        this.showEmojiBoard
                            ? this._buildEmojiBoard()
                            : new Container(height: MediaQuery.of(context: context).viewInsets.bottom)
                    }
                )
            );
        }

        Widget _buildReplyWidget(BuildContext context) {
            if (this.widget.replyUserName.isEmpty()) {
                return new Container();
            }

            return new Container(
                height: 40,
                width: MediaQuery.of(context: context).size.width,
                padding: EdgeInsets.only(16, right: 16),
                alignment: Alignment.centerLeft,
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: new Border(bottom: new BorderSide(color: CColors.Separator))
                ),
                child: new RichText(
                    text: new TextSpan(
                        "回复 ",
                        style: CTextStyle.PRegularBody3,
                        new List<TextSpan> {
                            new TextSpan(
                                text: this.widget.replyUserName,
                                style: CTextStyle.PMediumBody3
                            )
                        }
                    )
                )
            );
        }

        Widget _buildTextField() {
            return new CustomTextField(
                textFieldKey: this._inputFieldKey,
                hintText: "友好的评论是交流的起点…",
                controller: this._textController,
                autofocus: true,
                maxLines: 2,
                minLines: 1,
                showEmojiBoard: this.showEmojiBoard,
                isShowImageButton: false,
                onSubmitted: this._onSubmitted,
                onPressEmoji: () => {
                    if (this.showEmojiBoard) {
                        TextInputPlugin.TextInputShow();
                        Promise.Delayed(TimeSpan.FromMilliseconds(200)).Then(
                            () => {
                                if (this.mounted) {
                                    this.setState(() => this._showEmojiBoard = false);
                                }
                            });
                    }
                    else {
                        this.setState(() => this._showEmojiBoard = true);
                        Promise.Delayed(TimeSpan.FromMilliseconds(100)).Then(
                            onResolved: TextInputPlugin.TextInputHide
                        );
                    }
                }
            );
        }

        Widget _buildEmojiBoard() {
            return new EmojiBoard(
                handleEmoji: this._handleEmoji,
                handleDelete: this._handleDelete,
                () => this._onSubmitted(text: this._textController.text)
            );
        }
    }
}