using System;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using Notification = ConnectApp.models.Notification;

namespace ConnectApp.components {
    public class NotificationCard : StatelessWidget {
        public NotificationCard(
            Notification notification,
            User user,
            Action onTap = null,
            Key key = null
        ) : base(key) {
            this.notification = notification;
            this.user = user;
            this.onTap = onTap;
        }

        readonly Notification notification;
        readonly User user;
        readonly Action onTap;

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
                "project_message_participate_liked"

            };
            if (!types.Contains(type)) {
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
                                child: Avatar.User(this.user.id, this.user, 48)
                            ),
                            new Expanded(
                                child: new Container(
                                    padding: EdgeInsets.only(0, 16, 16, 16),
                                    decoration: new BoxDecoration(
                                        border: new Border(bottom: new BorderSide(CColors.Separator2)
                                        )
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
                    CTextStyle.PLargeBody2
                );
            }

            if (type == "project_message_commented") {
                if (data.parentComment.isNotEmpty()) {
                    content = $" “{data.parentComment}”";
                    subTitle = new TextSpan(
                        " 回复了你的评论" + content,
                        CTextStyle.PLargeBody2
                    );
                }
                else {
                    subTitle = new TextSpan(
                        " 评论了你的文章",
                        CTextStyle.PLargeBody2
                    );  
                }
            }

            if (type == "project_participate_comment") {
                if (data.parentComment.isNotEmpty()) {
                    content = $" “{data.parentComment}”";
                }
                subTitle = new TextSpan(
                    " 回复了你的评论" + content,
                    CTextStyle.PLargeBody2
                );
            }
            
            if (type == "project_message_liked") {
                if (data.comment.isNotEmpty()) {
                    content = $" “{data.comment}”";
                }
                subTitle = new TextSpan(
                    " 赞了你的评论" + content,
                    CTextStyle.PLargeBody2
                );
            }
            
            if (type == "project_message_participate_liked") {
                if (data.comment.isNotEmpty()) {
                    content = $" “{data.comment}”";
                }
                subTitle = new TextSpan(
                    " 赞了你的评论" + content,
                    CTextStyle.PLargeBody2
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
                                    data.fullname,
                                    CTextStyle.PLargeMedium
                                ),
                                subTitle
                            }
                        ),
                        overflow: TextOverflow.ellipsis
                    ),
                    new Text(
                        data.projectTitle,
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
                    DateConvert.DateStringFromNow(createdTime),
                    style: CTextStyle.PSmallBody4
                )
            );
        }
    }
}