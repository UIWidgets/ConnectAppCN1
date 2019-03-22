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

namespace ConnectApp.components {
    public class CommentCard : StatelessWidget {
        public CommentCard(
            Message message,
            bool isPraised,
            GestureTapCallback moreCallBack = null,
            GestureTapCallback praiseCallBack = null,
            GestureTapCallback replyCallBack = null,
            Key key = null
        ) : base(key) {
            this.message = message;
            this.isPraised = isPraised;
            this.moreCallBack = moreCallBack;
            this.praiseCallBack = praiseCallBack;
            this.replyCallBack = replyCallBack;
        }

        public readonly Message message;
        public readonly bool isPraised;
        public readonly GestureTapCallback moreCallBack;
        public readonly GestureTapCallback praiseCallBack;
        public readonly GestureTapCallback replyCallBack;


        public override Widget build(BuildContext context) {
            Widget _content = null;
            if (message.parentMessageId == null) {
                _content = new Container(
                    child: new Text(message.content, style: CTextStyle.PLarge),
                    alignment: Alignment.centerLeft
                );
            }
            else {
                var parentMessage =
                    StoreProvider.store.state.messageState.channelMessageDict[message.channelId][
                        message.parentMessageId];
                _content = new Container(alignment: Alignment.centerLeft, child: new RichText(text: new TextSpan("回复@",
                    children: new List<TextSpan> {
                        new TextSpan(
                            $"{parentMessage.author.fullName}",
                            children: new List<TextSpan> {
                                new TextSpan(
                                    $":{message.content}", style: CTextStyle.PLarge
                                )
                            },
                            style: CTextStyle.PLargeBlue
                        )
                    }, style: CTextStyle.PLargeBody4)));
            }


            return new Container(
                color:CColors.White,
                padding: EdgeInsets.only(top: 8,left:16,right:16),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(right: 8, top: 2),
                            child: new ClipRRect(
                                borderRadius: BorderRadius.circular(12),
                                child: new Avatar(message.author.id, null, 24)
                            )
                        ),

                        new Expanded(
                            child: new Container(
                                child: new Column(
                                    children: new List<Widget> {
                                        new Row(
                                            children: new List<Widget> {
                                                new Expanded(
                                                    child: new Text(message.author.fullName, style: CTextStyle.PRegularBody3)),
                                                new GestureDetector(
                                                    onTap: moreCallBack,
                                                    child: new Icon(Icons.ellipsis, size: 20, color: CColors.BrownGrey)
                                                )
                                            }
                                        ),
                                        _content,
                                        new Row(
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            children: new List<Widget> {
                                                new Text($"{DateConvert.DateStringFromNonce(message.nonce)}",
                                                    style: CTextStyle.TextBody4),
                                                new Container(
                                                    child: new Row(
                                                        children: new List<Widget> {
                                                            new GestureDetector(
                                                                onTap: praiseCallBack,
                                                                child: new Text($"点赞 {message.reactions.Count}",
                                                                    style: isPraised
                                                                        ? new TextStyle(
                                                                            height: 1.25f,
                                                                            fontSize: 12,
                                                                            fontFamily: "PingFang-Regular",
                                                                            color: CColors.PrimaryBlue
                                                                        )
                                                                        : CTextStyle.TextBody4)),
                                                            new Container(width: 10),
                                                            new GestureDetector(
                                                                onTap: replyCallBack,
                                                                child: new Text($"回复 {message.replyMessageIds.Count}",
                                                                    style: CTextStyle.TextBody4)
                                                            ),
                                                        }
                                                    )
                                                )
                                            }
                                        ),
                                        new Container(margin: EdgeInsets.only(top: 12), height: 1,
                                            color: CColors.Separator2)
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