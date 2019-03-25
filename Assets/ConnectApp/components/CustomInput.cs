using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
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
        private string inputText;
        private readonly TextEditingController _controller = new TextEditingController(null);

        public override void initState() {
            base.initState();
            _isPublish = false;
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
                                color: Colors.white,
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
                                                            borderRadius: BorderRadius.circular(22)
                                                        ),
                                                        child: new Container(
                                                            padding: EdgeInsets.only(16, right: 16),
                                                            child: new InputField(
                                                                height: 36,
                                                                controller: _controller,
                                                                style: CTextStyle.PLarge,
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
                                                                    inputText = text;
                                                                },
                                                                onSubmitted: text => { widget.doneCallBack(text); }
                                                            )
                                                        )
                                                    )
                                                ),
                                                new CustomButton(
                                                    onPressed: () => {
                                                        if (!_isPublish) return;
                                                        widget.doneCallBack(inputText);
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
                                margin: EdgeInsets.only(bottom: 250)
                            )
                        )
                    }
                )
            );
        }
    }
}