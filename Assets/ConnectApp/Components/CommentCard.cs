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

        private readonly Message message;
        private readonly bool isPraised;
        private readonly GestureTapCallback moreCallBack;
        private readonly GestureTapCallback praiseCallBack;
        private readonly GestureTapCallback replyCallBack;


        public override Widget build(BuildContext context) {
            Widget _content = null;
            var messageDict = StoreProvider.store.state.messageState.channelMessageDict[message.channelId];
            var content = MessageUtil.AnalyzeMessage(message.content, message.mentions, message.mentionEveryone);
            if (message.parentMessageId == null) {
                _content = new Container(
                    child: new Text(
                        content,
                        style: CTextStyle.PLargeBody
                    ),
                    alignment: Alignment.centerLeft
                );
            }
            else {
                if (messageDict.ContainsKey(message.parentMessageId)) {
                    var parentMessage = messageDict[message.parentMessageId];
                    _content = new Container(alignment: Alignment.centerLeft, child: new RichText(text: new TextSpan(
                                "回复@",
                                children: new List<TextSpan> {
                                    new TextSpan(
                                        $"{parentMessage.author.fullName}",
                                        children: new List<TextSpan> {
                                            new TextSpan(
                                                $": {content}",
                                                CTextStyle.PLargeBody
                                            )
                                        },
                                        style: CTextStyle.PLargeBlue)
                                },
                                style: CTextStyle.PLargeBody4
                            )
                        )
                    );
                }
                else {
                    _content = new Container(
                        child: new Text(message.content, style: CTextStyle.PLargeBody),
                        alignment: Alignment.centerLeft
                    );
                }
            }

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, 16, 16),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(right: 8),
                            child: new Avatar(message.author.id, 24)
                        ),
                        new Expanded(
                            child: new Container(
                                child: new Column(
                                    children: new List<Widget> {
                                        new Row(
                                            children: new List<Widget> {
                                                new Expanded(
                                                    child: new Text(message.author.fullName,
                                                        style: CTextStyle.PMediumBody3)),
                                                new GestureDetector(
                                                    onTap: moreCallBack,
                                                    child: new Icon(Icons.ellipsis, size: 20, color: CColors.BrownGrey)
                                                )
                                            }
                                        ),
                                        new Container(
                                            margin: EdgeInsets.only(top: 3, bottom: 5),
                                            child: _content
                                        ),
                                        new Row(
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            children: new List<Widget> {
                                                new Text($"{DateConvert.DateStringFromNonce(message.nonce)}",
                                                    style: CTextStyle.PSmallBody4),
                                                new Container(
                                                    child: new Row(
                                                        children: new List<Widget> {
                                                            new GestureDetector(
                                                                onTap: praiseCallBack,
                                                                child: new Text(
                                                                    $"点赞 {message.reactions.Count}",
                                                                    style: isPraised
                                                                        ? CTextStyle.PRegularBlue
                                                                        : CTextStyle.PRegularBody4
                                                                )
                                                            ),
                                                            new Container(width: 10),
                                                            new GestureDetector(
                                                                onTap: replyCallBack,
                                                                child: new Text(
                                                                    $"回复 {message.replyMessageIds.Count}",
                                                                    style: CTextStyle.PRegularBody4
                                                                )
                                                            )
                                                        }
                                                    )
                                                )
                                            }
                                        ),
                                        new Container(
                                            margin: EdgeInsets.only(top: 12),
                                            height: 1,
                                            color: CColors.Separator2
                                        )
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