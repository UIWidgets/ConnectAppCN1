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
        private bool _isPublish;
        private string _inputText;
        private readonly TextEditingController _controller = new TextEditingController("");
        private readonly GlobalKey _inputFieldKey = GlobalKey.key();
        private readonly EdgeInsets _inputFieldPadding = EdgeInsets.symmetric(horizontal: 16, vertical: 5);
        private readonly TextStyle _inputFieldStyle = CTextStyle.PLargeBody;
        private float _inputFieldHeight = 22;

        public override void initState() {
            base.initState();
            _isPublish = false;
            _controller.addListener(_controllerListener);
        }

        public override void dispose() {
            _controller.removeListener(_controllerListener);
            base.dispose();
        }

        private void _controllerListener() {
            var text = _controller.text ?? "友好的评论是交流的起点…";
            if (!mounted) return;
            var inputFieldHeight = 22.0f;
            var inputFieldWidth = _inputFieldKey.currentContext.size.width;
            inputFieldHeight = _calculateTextHeight(text, inputFieldWidth);

            if (_inputFieldHeight != inputFieldHeight)
                setState(() => { _inputFieldHeight = inputFieldHeight; });
        }

        private float _calculateTextHeight(string text, float textWidth) {
            var textPainter = new TextPainter(
                textDirection: TextDirection.ltr,
                text: new TextSpan(
                    text,
                    _inputFieldStyle
                ),
                maxLines: 2
            );
            textPainter.layout(maxWidth: textWidth);

            return textPainter.height;
        }

        private void _onSubmitted(string text) {
            if (widget.doneCallBack != null)
                widget.doneCallBack(text);
        }

        public override Widget build(BuildContext context) {
            var reply = new Container();
            if (!widget.replyUserName.isEmpty())
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
                                new TextSpan(
                                    widget.replyUserName,
                                    CTextStyle.PMediumBody3
                                )
                            }
                        )
                    )
                );
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
                                                                (_inputFieldHeight + 13) / 2)
                                                        ),
                                                        child: new Container(
                                                            padding: _inputFieldPadding,
                                                            child: new InputField(
                                                                _inputFieldKey,
                                                                height: _inputFieldHeight,
                                                                controller: _controller,
                                                                style: _inputFieldStyle,
                                                                maxLines: 2,
                                                                autofocus: true,
                                                                hintText: "友好的评论是交流的起点…",
                                                                hintStyle: CTextStyle.PLargeBody4,
                                                                cursorColor: CColors.PrimaryBlue,
                                                                textInputAction: TextInputAction.send,
                                                                onChanged: text => {
                                                                    var isTextEmpty = text.Length > 0;
                                                                    if (_isPublish != isTextEmpty)
                                                                        setState(() => { _isPublish = isTextEmpty; });
                                                                    _inputText = text;
                                                                },
                                                                onSubmitted: _onSubmitted
                                                            )
                                                        )
                                                    )
                                                ),
                                                new CustomButton(
                                                    onPressed: () => {
                                                        if (!_isPublish) return;
                                                        _onSubmitted(_inputText);
                                                    },
                                                    child: new Text(
                                                        "发布",
                                                        style: _isPublish
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