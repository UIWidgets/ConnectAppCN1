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
using Notification = ConnectApp.Models.Model.Notification;

namespace ConnectApp.Components {
    public class NotificationCard : StatelessWidget {
        public NotificationCard(
            Notification notification,
            User user,
            List<User> mentions,
            Action onTap = null,
            Action<string> pushToUserDetail = null,
            Action<string> pushToTeamDetail = null,
            bool isLast = false,
            Key key = null
        ) : base(key: key) {
            this.notification = notification;
            this.user = user;
            this.mentions = mentions;
            this.onTap = onTap;
            this.pushToUserDetail = pushToUserDetail;
            this.pushToTeamDetail = pushToTeamDetail;
            this.isLast = isLast;
        }

        readonly Notification notification;
        readonly User user;
        readonly List<User> mentions;
        readonly Action onTap;
        readonly Action<string> pushToTeamDetail;
        readonly Action<string> pushToUserDetail;
        readonly bool isLast;

        public override Widget build(BuildContext context) {
            if (this.notification == null) {
                return new Container();
            }

            var type = this.notification.type;
            var types = new List<string> {
                "project_liked",
                "project_message_commented",
                "project_participate_comment",
                "project_message_liked",
                "project_message_participate_liked",
                "followed",
                "team_followed"
            };
            if (!types.Contains(item: type)) {
                return new Container();
            }

            return new GestureDetector(
                onTap: () => this.onTap(),
                child: new Container(
                    color: CColors.White,
                    child: new Row(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget> {
                            new Container(
                                padding: EdgeInsets.only(16, 16, 16),
                                child: new GestureDetector(
                                    onTap: () => this.pushToUserDetail(obj: this.user.id),
                                    child: Avatar.User(user: this.user, 48)
                                )
                            ),
                            new Expanded(
                                child: new Container(
                                    padding: EdgeInsets.only(0, 16, 16, 16),
                                    decoration: new BoxDecoration(
                                        border: this.isLast
                                            ? null
                                            : new Border(bottom: new BorderSide(CColors.Separator2))
                                    ),
                                    child: new Column(
                                        mainAxisAlignment: MainAxisAlignment.start,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            this._buildNotificationTitle(), this._buildNotificationTime()
                                        }
                                    )
                                )
                            )
                        }
                    )
                )
            );
        }

        Widget _buildNotificationTitle() {
            var type = this.notification.type;
            var data = this.notification.data;
            var subTitle = new TextSpan();
            var content = "";
            if (type == "project_liked") {
                subTitle = new TextSpan(
                    " 赞了你的文章",
                    style: CTextStyle.PLargeBody2
                );
            }

            if (type == "project_message_commented") {
                if (data.parentComment.isNotEmpty()) {
                    content = $" “{MessageUtils.AnalyzeMessage(data.parentComment, this.mentions, false)}”";
                    subTitle = new TextSpan(
                        " 回复了你的评论" + content,
                        style: CTextStyle.PLargeBody2
                    );
                }
                else {
                    subTitle = new TextSpan(
                        " 评论了你的文章",
                        style: CTextStyle.PLargeBody2
                    );
                }
            }

            if (type == "project_participate_comment") {
                if (data.parentComment.isNotEmpty()) {
                    content = $" “{MessageUtils.AnalyzeMessage(data.parentComment, this.mentions, false)}”";
                }

                subTitle = new TextSpan(
                    " 回复了你的评论" + content,
                    style: CTextStyle.PLargeBody2
                );
            }

            if (type == "project_message_liked") {
                if (data.comment.isNotEmpty()) {
                    content = $" “{MessageUtils.AnalyzeMessage(data.comment, this.mentions, false)}”";
                }

                subTitle = new TextSpan(
                    " 赞了你的评论" + content,
                    style: CTextStyle.PLargeBody2
                );
            }

            if (type == "project_message_participate_liked") {
                if (data.comment.isNotEmpty()) {
                    content = $" “{MessageUtils.AnalyzeMessage(data.comment, this.mentions, false)}”";
                }

                subTitle = new TextSpan(
                    " 赞了你的评论" + content,
                    style: CTextStyle.PLargeBody2
                );
            }

            if (type == "followed") {
                subTitle = new TextSpan(
                    " 关注了你",
                    style: CTextStyle.PLargeBody2
                );
            }

            if (type == "team_followed") {
                subTitle = new TextSpan(
                    children: new List<TextSpan> {
                        new TextSpan("关注了"),
                        new TextSpan(data.teamName, recognizer: new TapGestureRecognizer {
                            onTap = () => { this.pushToTeamDetail(data.teamId); }
                        }, style: CTextStyle.PLargeBlue)
                    },
                    style: CTextStyle.PLargeBody2
                );
            }

            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget> {
                    new RichText(
                        maxLines: 2,
                        text: new TextSpan(
                            children: new List<TextSpan> {
                                new TextSpan(
                                    text: data.fullname,
                                    style: CTextStyle.PLargeMedium,
                                    recognizer: new TapGestureRecognizer {
                                        onTap = () => this.pushToUserDetail(obj: data.userId)
                                    }
                                ),
                                subTitle
                            }
                        ),
                        overflow: TextOverflow.ellipsis
                    ),
                    new Text(
                        data: data.projectTitle,
                        maxLines: 1,
                        style: CTextStyle.PLargeMedium,
                        overflow: TextOverflow.ellipsis
                    )
                }
            );
        }

        Widget _buildNotificationTime() {
            var createdTime = this.notification.createdTime;
            return new Container(
                child: new Text(
                    DateConvert.DateStringFromNow(dt: createdTime),
                    style: CTextStyle.PSmallBody4
                )
            );
        }
    }
}