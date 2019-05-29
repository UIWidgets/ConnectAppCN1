using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.Models.Model;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class CommentCard : StatelessWidget {
        public CommentCard(
            Message message,
            bool isPraised,
            string parentName = null,
            GestureTapCallback moreCallBack = null,
            GestureTapCallback praiseCallBack = null,
            GestureTapCallback replyCallBack = null,
            Key key = null
        ) : base(key) {
            this.message = message;
            this.isPraised = isPraised;
            this.parentName = parentName;
            this.moreCallBack = moreCallBack;
            this.praiseCallBack = praiseCallBack;
            this.replyCallBack = replyCallBack;
        }

        readonly Message message;
        readonly bool isPraised;
        readonly string parentName;
        readonly GestureTapCallback moreCallBack;
        readonly GestureTapCallback praiseCallBack;
        readonly GestureTapCallback replyCallBack;


        public override Widget build(BuildContext context) {
            if (this.message == null) {
                return new Container();
            }

            var content = MessageUtils.AnalyzeMessage(this.message.content, this.message.mentions,
                this.message.mentionEveryone);
            Widget _content = this.parentName.isEmpty()
                ? new Container(
                    child: new Text(
                        content,
                        style: CTextStyle.PLargeBody
                    ),
                    alignment: Alignment.centerLeft
                )
                : new Container(alignment: Alignment.centerLeft, child: new RichText(text: new TextSpan(
                            "回复",
                            children: new List<TextSpan> {
                                new TextSpan(
                                    $"@{this.parentName}",
                                    children: new List<TextSpan> {
                                        new TextSpan(
                                            $" ：{content}",
                                            CTextStyle.PLargeBody
                                        )
                                    },
                                    style: CTextStyle.PLargeBlue)
                            },
                            style: CTextStyle.PLargeBody4
                        )
                    )
                );
            var reply = this.message.parentMessageId.isEmpty()
                ? new GestureDetector(
                    onTap: this.replyCallBack,
                    child: new Container(
                        margin: EdgeInsets.only(left: 10),
                        child: new Text(
                            $"回复 {this.message.replyMessageIds.Count}",
                            style: CTextStyle.PRegularBody4
                        ))
                )
                : new GestureDetector(
                    child: new Container()
                );
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, 16, 16),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            height: 24,
                            margin: EdgeInsets.only(right: 8),
                            child: Avatar.User(this.message.author.id, this.message.author, 24)
                        ),
                        new Expanded(
                            child: new Container(
                                child: new Column(
                                    children: new List<Widget> {
                                        new Container(
                                            height: 24,
                                            child: new Row(
                                                children: new List<Widget> {
                                                    new Expanded(
                                                        child: new Container(
                                                            alignment: Alignment.centerLeft,
                                                            child: new Text(this.message.author.fullName,
                                                                style: CTextStyle.PMediumBody3.apply(heightFactor: 0,
                                                                    heightDelta: 1)))),

                                                    new CustomButton(
                                                        padding: EdgeInsets.only(8, 0, 0, 8),
                                                        onPressed: this.moreCallBack,
                                                        child: new Icon(Icons.ellipsis, size: 20,
                                                            color: CColors.BrownGrey)
                                                    )
                                                }
                                            )
                                        ),

                                        new Container(
                                            margin: EdgeInsets.only(top: 3, bottom: 5),
                                            child: _content
                                        ),

                                        new Row(
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            children: new List<Widget> {
                                                new Text($"{DateConvert.DateStringFromNonce(this.message.nonce)}",
                                                    style: CTextStyle.PSmallBody4),
                                                new Container(
                                                    child: new Row(
                                                        children: new List<Widget> {
                                                            new GestureDetector(
                                                                onTap: this.praiseCallBack,
                                                                child: new Container(
                                                                    color: CColors.White,
                                                                    child: new Text(
                                                                        $"点赞 {this.message.reactions.Count}",
                                                                        style: this.isPraised
                                                                            ? CTextStyle.PRegularBlue
                                                                            : CTextStyle.PRegularBody4
                                                                    )
                                                                )),
                                                            reply
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