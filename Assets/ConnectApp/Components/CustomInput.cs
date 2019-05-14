using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public delegate void InputDone(string text);

    public class CustomInput : StatefulWidget {
        public CustomInput(
            string replyUserName = null,
            InputDone doneCallBack = null,
            Key key = null
        ) : base(key) {
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
        bool _isPublish;
        string _inputText;
        readonly TextEditingController _controller = new TextEditingController("");
        readonly GlobalKey _inputFieldKey = GlobalKey.key();
        readonly EdgeInsets _inputFieldPadding = EdgeInsets.symmetric(horizontal: 16, vertical: 5);
        readonly TextStyle _inputFieldStyle = CTextStyle.PLargeBody;
        float _inputFieldHeight = 22;

        public override void initState() {
            base.initState();
            this._isPublish = false;
            this._controller.addListener(this._controllerListener);
        }

        public override void dispose() {
            this._controller.removeListener(this._controllerListener);
            base.dispose();
        }

        void _controllerListener() {
            var text = this._controller.text ?? "友好的评论是交流的起点…";
            if (!this.mounted) {
                return;
            }

            var inputFieldWidth = this._inputFieldKey.currentContext.size.width;
            var inputFieldHeight = this._calculateTextHeight(text, inputFieldWidth);

            if (this._inputFieldHeight != inputFieldHeight) {
                this.setState(() => { this._inputFieldHeight = inputFieldHeight; });
            }
        }

        float _calculateTextHeight(string text, float textWidth) {
            var textPainter = new TextPainter(
                textDirection: TextDirection.ltr,
                text: new TextSpan(
                    text, this._inputFieldStyle
                ),
                maxLines: 2
            );
            textPainter.layout(maxWidth: textWidth);

            return textPainter.height;
        }

        void _onSubmitted(string text) {
            if (this.widget.doneCallBack != null) {
                this.widget.doneCallBack(text);
            }
        }

        public override Widget build(BuildContext context) {
            var reply = new Container();
            if (!this.widget.replyUserName.isEmpty()) {
                reply = new Container(
                    height: 40,
                    width: MediaQuery.of(context).size.width,
                    padding: EdgeInsets.only(16, right: 16),
                    alignment: Alignment.centerLeft,
                    decoration: new BoxDecoration(
                        CColors.White,
                        border: new Border(
                            bottom: new BorderSide(CColors.Separator)
                        )
                    ),
                    child: new RichText(
                        text: new TextSpan(
                            "回复 ",
                            CTextStyle.PRegularBody3,
                            new List<TextSpan> {
                                new TextSpan(this.widget.replyUserName,
                                    CTextStyle.PMediumBody3
                                )
                            }
                        )
                    )
                );
            }

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
                                        reply,
                                        new Row(
                                            children: new List<Widget> {
                                                new Expanded(
                                                    child: new Container(
                                                        margin: EdgeInsets.only(10, 6.5f, 8, 6.5f),
                                                        decoration: new BoxDecoration(
                                                            CColors.Separator2,
                                                            borderRadius: BorderRadius.circular(
                                                                (this._inputFieldHeight + 13) / 2)
                                                        ),
                                                        child: new Container(
                                                            padding: this._inputFieldPadding,
                                                            child: new InputField(this._inputFieldKey,
                                                                height: this._inputFieldHeight,
                                                                controller: this._controller,
                                                                style: this._inputFieldStyle,
                                                                maxLines: 2,
                                                                autofocus: true,
                                                                hintText: "友好的评论是交流的起点…",
                                                                hintStyle: CTextStyle.PLargeBody4,
                                                                cursorColor: CColors.PrimaryBlue,
                                                                textInputAction: TextInputAction.send,
                                                                onChanged: text => {
                                                                    var isTextEmpty = text.Length > 0;
                                                                    if (this._isPublish != isTextEmpty) {
                                                                        this.setState(() => {
                                                                            this._isPublish = isTextEmpty;
                                                                        });
                                                                    }

                                                                    this._inputText = text;
                                                                },
                                                                onSubmitted: this._onSubmitted
                                                            )
                                                        )
                                                    )
                                                ),
                                                new CustomButton(
                                                    onPressed: () => {
                                                        if (!this._isPublish) {
                                                            return;
                                                        }

                                                        this._onSubmitted(this._inputText);
                                                    },
                                                    child: new Text(
                                                        "发布",
                                                        style: this._isPublish
                                                            ? CTextStyle.PLargeBlue
                                                            : CTextStyle.PLargeDisabled
                                                    )
                                                ),
                                                new Container(width: 8)
                                            }
                                        )
                                    }
                                ),
                                margin: EdgeInsets.only(bottom: MediaQuery.of(context).viewInsets.bottom)
                            )
                        )
                    }
                )
            );
        }
    }
}