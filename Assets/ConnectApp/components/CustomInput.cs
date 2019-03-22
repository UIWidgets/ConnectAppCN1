using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public delegate void InputDone(string text);

    public class CustomInput : StatelessWidget {
        public CustomInput(
            string replyUserName = null,
            InputDone doneCallBack = null,
            Key key = null) : base(key) {
            this.replyUserName = replyUserName;
            this.doneCallBack = doneCallBack;
        }

        public readonly string replyUserName;
        public readonly InputDone doneCallBack;
        private string inputText;
        private readonly TextEditingController _controller = new TextEditingController(null);

        
        public override Widget build(BuildContext context) {
            var reply = new Container();
            if (!replyUserName.isEmpty())
            {
                reply = new Container(
                    height:40,
                    alignment:Alignment.centerLeft,
                    width:MediaQuery.of(context).size.width,
                    decoration:new BoxDecoration(color:CColors.White,border:new Border(bottom:new BorderSide(CColors.Separator2))),
                    child: new Container(
                        padding:EdgeInsets.only(left:16,right:16),
                        child:new RichText(text:new TextSpan("回复",style:CTextStyle.PRegularBody3,children:new List<TextSpan>
                        {
                            new TextSpan(replyUserName,style:CTextStyle.PRegular)
                        }))) 
                );
            }
            return new Container(
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: new List<Widget> {
                        new Container(
                            color: Color.fromRGBO(0, 0, 0, 0.4f),
                            child: new Container(
                                color: Colors.white,
                                child:new Column(
                                    children:new List<Widget>
                                    {    
                                        reply,
                                        new Row(
                                            children: new List<Widget> {
                                                new Expanded(
                                                    child: new Container( 
                                                        margin: EdgeInsets.only(10, 6.5f, 8, 6.5f),
                                                        decoration: new BoxDecoration(
                                                            CColors.Separator2,
                                                            border: Border.all(CColors.Separator2),
                                                            borderRadius: BorderRadius.circular(22)
                                                        ),
                                                        child: new Container(
                                                            padding:EdgeInsets.only(left:16,right:16),
                                                            child:new InputField(
                                                                height:36,
                                                                controller: _controller,
                                                                style: CTextStyle.PLarge,
                                                                maxLines: 1,
                                                                autofocus: true,
                                                                hintText: "友好的评论是交流的起点…",
                                                                hintStyle: CTextStyle.PLargeBody4,
                                                                cursorColor: CColors.PrimaryBlue,
                                                                textInputAction: TextInputAction.send,
                                                                onChanged: (text) => { inputText = text; },
                                                                onSubmitted: (text) => { doneCallBack(text); }
                                                            )
                                                        ) 
                                                    )
                                                ),
                                                new CustomButton(
                                                    onPressed: () => { doneCallBack(inputText); },
                                                    child: new Text("发布", style:CTextStyle.PLargeBlue)
                                                ),
                                                new Container(width:16),
                                            }
                                        )
                                    }),
                                margin:EdgeInsets.only(bottom:250)
                                
                            )
                        )
                    })
            );
            
            
        }
    }
}