using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components
{
    public class CommentCard : StatelessWidget
    {
        public CommentCard(
            Message message,
            GestureTapCallback moreCallBack = null,
            GestureTapCallback praiseCallBack = null,
            GestureTapCallback replyCallBack = null,
            Key key = null
        ) : base(key)
        {
            this.message = message;
            this.moreCallBack = moreCallBack;
            this.moreCallBack = praiseCallBack;
            this.moreCallBack = replyCallBack;

        }

        public readonly Message message;
        public readonly GestureTapCallback moreCallBack;
        public readonly GestureTapCallback praiseCallBack;
        public readonly GestureTapCallback replyCallBack;

        
        

        public override Widget build(BuildContext context)
        {

            
            
            Widget _content = null;
            if (message.parentMessageId ==null )
            {
                _content = new Text(message.content, style: CTextStyle.PLarge);
            }
            else
            {
                var parentMessage =
                    StoreProvider.store.state.messageState.channelMessageDict[message.channelId][message.parentMessageId];
                _content = new Container(child:new RichText(text:new TextSpan("回复@", children: new List<TextSpan>
                {
                    new TextSpan(
                        $"{parentMessage.author.username}",
                        children: new List<TextSpan>
                        {
                            new TextSpan(
                                $":{message.content}", style: CTextStyle.PLarge
                            )
                        },
                        style:new TextStyle(
                            height: 1.5f,
                            fontSize: 16,
                            fontFamily: "PingFang-Regular",
                            color: CColors.PrimaryBlue
                        )

                    )
                },style:new TextStyle(
                    height: 1.5f,
                    fontSize: 16,
                    fontFamily: "PingFang-Regular",
                    color: CColors.TextBody4
                )))); 
            }
            
            
            return new Container(
                padding:EdgeInsets.only(top:8),
                child:new Row(
                    crossAxisAlignment:CrossAxisAlignment.start,
                    children:new List<Widget>
                    {
                        new Container(
                            margin:EdgeInsets.only(right:8,top:2),
                            child:new ClipRRect(
                                borderRadius:BorderRadius.circular(12),
                                child:new Container(
                                    width:24,
                                    height:24,
                                    child:Image.network(message.author.avatar,fit:BoxFit.fill)
                                )
                            )
                        ),
                        
                        new Expanded(
                            child:new Container(
                                child:new Column(
                                children:new List<Widget>
                                {
                                    new Row(
                                        children:new List<Widget>
                                        {
                                            new Expanded(
                                                child:new Text(message.author.username,style:new TextStyle(
                                                    height: 1.57f,
                                                    fontSize: 14,
                                                    fontFamily: "PingFangSC-Regular",
                                                    color: CColors.TextThird))),
                                            new GestureDetector(
                                                onTap:moreCallBack,
                                                child:new Icon(Icons.ellipsis,size:20,color:CColors.BrownGrey)
                                            )
                                        }
                                    ),
                                    _content,
                                    new Row(
                                        mainAxisAlignment:MainAxisAlignment.spaceBetween,
                                        children:new List<Widget>
                                        {
                                            new Text($"{DateConvert.DateStringFromNonce(message.nonce)}",style:CTextStyle.TextBody4),
                                            new Container(
                                               child: new Row(
                                                    children:new List<Widget>
                                                    {
                                                        new GestureDetector(
                                                            onTap:praiseCallBack,
                                                            child:new Text($"点赞 {message.reactions.Count}",style:CTextStyle.TextBody4)
                                                        ),
                                                        new Container(width:10),
                                                        new GestureDetector(
                                                            onTap:replyCallBack,
                                                            child:new Text($"回复 { message.replyMessageIds.Count }",style:CTextStyle.TextBody4)
                                                        ),
                                                    }
                                                )
                                            )
                                        }
                                    ),
                                    new Container(margin:EdgeInsets.only(top:12),height:1,color:CColors.Separator2)
                                    
                                }
                            )
                         )
                            
                        )
                    }
                )
            );
        }
    }
}