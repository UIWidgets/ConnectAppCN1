using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class CommentCard : StatelessWidget {
        public CommentCard(
            Message message,
            string userLicense,
            bool isPraised,
            string parentName = null,
            string parentAuthorId = null,
            GestureTapCallback moreCallBack = null,
            GestureTapCallback praiseCallBack = null,
            GestureTapCallback replyCallBack = null,
            Action<string> pushToUserDetail = null,
            Key key = null
        ) : base(key: key) {
            this.message = message;
            this.userLicense = userLicense;
            this.isPraised = isPraised;
            this.parentName = parentName;
            this.parentAuthorId = parentAuthorId;
            this.moreCallBack = moreCallBack;
            this.praiseCallBack = praiseCallBack;
            this.replyCallBack = replyCallBack;
            this.pushToUserDetail = pushToUserDetail;
        }

        readonly Message message;
        readonly string userLicense;
        readonly bool isPraised;
        readonly string parentName;
        readonly string parentAuthorId;
        readonly GestureTapCallback moreCallBack;
        readonly GestureTapCallback praiseCallBack;
        readonly GestureTapCallback replyCallBack;
        readonly Action<string> pushToUserDetail;


        public override Widget build(BuildContext context) {
            if (this.message == null) {
                return new Container();
            }

            var replyCount = this.message.parentMessageId.isNotEmpty()
                ? (this.message.lowerMessageIds ?? new List<string>()).Count
                : (this.message.replyMessageIds ?? new List<string>()).Count;
            
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, 16, 16),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new GestureDetector(
                            onTap: () => this.pushToUserDetail(obj: this.message.author.id),
                            child: new Container(
                                height: 24,
                                margin: EdgeInsets.only(right: 8),
                                child: Avatar.User(user: this.message.author, 24)
                            )
                        ),
                        new Expanded(
                            child: new Container(
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        this._buildCommentAvatarName(UserInfoManager.isMe(userId: this.message.author.id)),
                                        this._buildCommentContent(),
                                        new Row(
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            children: new List<Widget> {
                                                new Text(
                                                    $"{DateConvert.DateStringFromNonce(nonce: this.message.nonce)}",
                                                    style: CTextStyle.PSmallBody4
                                                ),
                                                new Container(
                                                    child: new Row(
                                                        children: new List<Widget> {
                                                            new GestureDetector(
                                                                onTap: this.praiseCallBack,
                                                                child: new Container(
                                                                    color: CColors.White,
                                                                    child: new Text(
                                                                        $"点赞 {CStringUtils.CountToString(count: this.message.reactions.Count)}",
                                                                        style: this.isPraised
                                                                            ? CTextStyle.PRegularBlue
                                                                            : CTextStyle.PRegularBody4
                                                                    )
                                                                )
                                                            ),
                                                            new GestureDetector(
                                                                onTap: this.replyCallBack,
                                                                child: new Container(
                                                                    margin: EdgeInsets.only(15),
                                                                    child: new Text(
                                                                        $"回复 {CStringUtils.CountToString(count: replyCount)}",
                                                                        style: CTextStyle.PRegularBody4
                                                                    )
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

        Widget _buildCommentAvatarName(bool isMe) {
            var textStyle = CTextStyle.PMediumBody3.apply(heightFactor: 0, heightDelta: 1);
            return new Container(
                height: 24,
                child: new Row(
                    children: new List<Widget> {
                        new Expanded(
                            child: new GestureDetector(
                                onTap: () => this.pushToUserDetail(obj: this.message.author.id),
                                child: new Row(
                                    children: new List<Widget> {
                                        new Flexible(
                                            child: new Text(
                                                this.message.author.fullName,
                                                style: textStyle,
                                                maxLines: 1,
                                                overflow: TextOverflow.ellipsis
                                            )
                                        ),
                                        CImageUtils.GenBadgeImage(
                                            badges: this.message.author.badges,
                                            license: this.userLicense,
                                            EdgeInsets.only(4)
                                        )
                                    }
                                )
                            )
                        ),
                        !isMe ? new CustomButton(
                            padding: EdgeInsets.only(16, 0, 0, 8),
                            onPressed: this.moreCallBack,
                            child: new Icon(
                                icon: Icons.ellipsis,
                                size: 20,
                                color: CColors.BrownGrey
                            )
                        ) : (Widget)new Container()
                    }
                )
            );
        }

        Widget _buildCommentContent() {
            var content = MessageUtils.messageToTextSpans(
                content: this.message.content,
                mentions: this.message.mentions,
                mentionEveryone: this.message.mentionEveryone,
                userId => this.pushToUserDetail(obj: userId)
            );
            List<TextSpan> textSpans = new List<TextSpan>();
            if (this.parentName.isNotEmpty()) {
                textSpans.AddRange(new List<TextSpan> {
                    new TextSpan(
                        "回复",
                        style: CTextStyle.PLargeBody4
                    ),
                    new TextSpan(
                        $"@{this.parentName}",
                        style: CTextStyle.PLargeBlue,
                        recognizer: new TapGestureRecognizer {
                            onTap = () => this.pushToUserDetail(obj: this.parentAuthorId)
                        }
                    ),
                    new TextSpan(
                        ": ",
                        style: CTextStyle.PLargeBody
                    )
                });
            }

            textSpans.AddRange(collection: content);
            return new Container(
                margin: EdgeInsets.only(top: 3, bottom: 5),
                child: new RichText(
                    text: new TextSpan(
                        children: textSpans
                    )
                )
            );
        }
    }
}