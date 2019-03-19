using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.redux;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components
{
    public delegate void InputDone(string text);
    public class CustomInput : StatelessWidget
    {
        public CustomInput(
            string replyUserName = null,
            InputDone doneCallBack = null,
            ValueChanged<string> onSubmitted = null,
            Key key = null) : base(key)
        {
            this.replyUserName = replyUserName;
            this.doneCallBack = doneCallBack;
        }
        public readonly string replyUserName;
        public readonly InputDone doneCallBack;
        private string inputText;
        private readonly TextEditingController _controller = new TextEditingController(null);
       
        public override Widget build(BuildContext context)
        {
            return new Container(
                child: new Column(
                    mainAxisAlignment:MainAxisAlignment.end,
                children:new List<Widget>
                {
                    new Container(
                        color:Color.fromRGBO(0,0,0,0.4f),
                        child:new Container(
                        color: Colors.white,
                        padding: EdgeInsets.only(10, 6.5f, 16, MediaQuery.of(context).viewInsets.bottom),
                        child: new Row(
                            children: new List<Widget> {
                                new Expanded(
                                    child: new Container(
                                        decoration: new BoxDecoration(
                                            CColors.Separator2,
                                            border: Border.all(CColors.Separator2),
                                            borderRadius: BorderRadius.circular(22)
                                        ),
                                        
                                        child: new InputField(
                                            controller: _controller,
                                            style: new TextStyle(
                                                fontSize: 16,
                                                color: CColors.TextBody
                                            ),
                                            maxLines:2,
                                            autofocus: true,
                                            hintText: "友好的评论是交流的起点…",
                                            hintStyle: new TextStyle(
                                                fontSize: 16,
                                                color: CColors.TextBody4
                                            ),
                                            cursorColor: CColors.primary,
                                            textInputAction: TextInputAction.send,
                                            onChanged: (text) => { inputText = text; },
                                            onSubmitted: (text) =>
                                            {
                                                doneCallBack(text);
                                            }
                                        )
                                    )
                                ),
                                new Container(width: 8),
                                new CustomButton(
                                    onPressed: () => { doneCallBack(inputText); },
                                    child: new Text("发布",style:new TextStyle(
                                        height: 1.5f,
                                        fontSize: 16,
                                        fontFamily: "PingFang-Regular",
                                        color: CColors.PrimaryBlue
                                    ))
                                )
                            }
                        )
                    )
                    
                )
                })   
            );
        }
    }

}