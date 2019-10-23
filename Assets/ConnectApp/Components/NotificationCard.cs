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
            User user = null,
            Team team = null,
            List<User> mentions = null,
            Action onTap = null,
            Action<string> pushToUserDetail = null,
            Action<string> pushToTeamDetail = null,
            bool isLast = false,
            Key key = null
        ) : base(key: key) {
            this.notification = notification;
            this.user = user;
            this.team = team;
            this.mentions = mentions;
            this.onTap = onTap;
            this.pushToUserDetail = pushToUserDetail;
            this.pushToTeamDetail = pushToTeamDetail;
            this.isLast = isLast;
        }

        readonly Notification notification;
        readonly User user;
        readonly Team team;
        readonly List<User> mentions;
        readonly Action onTap;
        readonly Action<string> pushToTeamDetail;
        readonly Action<string> pushToUserDetail;
        readonly bool isLast;
        static readonly List<string> types = new List<string> {
            "project_liked",
            "project_commented",
            "project_message_commented",
            "project_participate_comment",
            "project_message_liked",
            "project_message_participate_liked",
            "project_article_publish",
            "followed",
            "team_followed"
        };

        public override Widget build(BuildContext context) {
            if (this.notification == null) {
                return new Container();
            }

            var type = this.notification.type;
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
                            this._buildNotificationAvatar(),
                            new Expanded(
                                child: new Container(
                                    padding: EdgeInsets.only(0, 16, 16, 16),
                                    decoration: new BoxDecoration(
                                        border: this.isLast
                                            ? null
                                            : new Border(bottom: new BorderSide(color: CColors.Separator2))
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

        Widget _buildNotificationAvatar() {
            Widget avatar;
            GestureTapCallback onTap;
            if (this.user == null) {
                avatar = Avatar.Team(team: this.team, 48);
                onTap = () => this.pushToTeamDetail(obj: this.team.id);
            }
            else {
                avatar = Avatar.User(user: this.user, 48);
                onTap = () => this.pushToUserDetail(obj: this.user.id);
            }

            return new Container(
                padding: EdgeInsets.only(16, 16, 16),
                child: new GestureDetector(
                    onTap: onTap,
                    child: avatar
                )
            );
        }

        Widget _buildNotificationTitle() {
            var type = this.notification.type;
            var data = this.notification.data;

            List<TextSpan> textSpans = new List<TextSpan>();
            Widget subTitleWidget = new Container();
            switch (type) {
                case "project_liked": {
                    textSpans.Add(new TextSpan(
                        " 赞了你的文章",
                        style: CTextStyle.PLargeBody2
                    ));
                    textSpans.Add(new TextSpan(
                        $"《{data.projectTitle}》",
                        style: CTextStyle.PLargeMedium
                    ));
                    break;
                }

                case "project_commented": {
                    textSpans.Add(new TextSpan(
                        " 评价了你的文章",
                        style: CTextStyle.PLargeBody2
                    ));
                    textSpans.Add(new TextSpan(
                        $"《{data.projectTitle}》",
                        style: CTextStyle.PLargeMedium
                    ));

                    subTitleWidget = new Text(
                        this._analyzeComment(comment: data.comment),
                        style: CTextStyle.PLargeBody2,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis
                    );
                    break;
                }

                case "project_message_commented": {
                    if (data.upperMessageId.isNotEmpty()) {
                        textSpans.Add(new TextSpan(
                            " 回复了你" + this._analyzeComment(comment: data.upperComment),
                            style: CTextStyle.PLargeBody2
                        ));
                    }
                    else if (data.parentMessageId.isNotEmpty()) {
                        textSpans.Add(new TextSpan(
                            " 回复了你" + this._analyzeComment(comment: data.parentComment),
                            style: CTextStyle.PLargeBody2
                        ));
                    }

                    subTitleWidget = new Text(
                        this._analyzeComment(comment: data.comment),
                        style: CTextStyle.PLargeBody2,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis
                    );
                    break;
                }

                case "project_participate_comment": {
                    textSpans.Add(new TextSpan(
                        " 评价了你关注的文章",
                        style: CTextStyle.PLargeBody2
                    ));
                    textSpans.Add(new TextSpan(
                        $"《{data.projectTitle}》",
                        style: CTextStyle.PLargeMedium
                    ));

                    subTitleWidget = new Text(
                        this._analyzeComment(comment: data.comment),
                        style: CTextStyle.PLargeBody2,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis
                    );
                    break;
                }

                case "project_message_liked": {
                    textSpans.Add(new TextSpan(
                        " 赞了你的评论" + this._analyzeComment(comment: data.comment),
                        style: CTextStyle.PLargeBody2
                    ));
                    break;
                }

                case "project_message_participate_liked": {
                    textSpans.Add(new TextSpan(
                        " 赞了你关注的评论" + this._analyzeComment(comment: data.comment),
                        style: CTextStyle.PLargeBody2
                    ));
                    break;
                }

                case "followed": {
                    textSpans.Add(new TextSpan(" 关注了你", style: CTextStyle.PLargeBody2));
                    break;
                }

                case "team_followed": {
                    textSpans.Add(new TextSpan(" 关注了 ", style: CTextStyle.PLargeBody2));
                    textSpans.Add(new TextSpan(
                        text: data.teamName,
                        recognizer: new TapGestureRecognizer {
                            onTap = () => this.pushToTeamDetail(obj: data.teamId)
                        },
                        style: CTextStyle.PLargeBlue
                    ));
                    break;
                }

                case "project_article_publish": {
                    string name;
                    GestureTapCallback onTap;
                    if (this.notification.data.role == "team") {
                        name = data.teamName;
                        onTap = () => this.pushToTeamDetail(obj: data.teamId);
                    }
                    else {
                        name = data.fullname;
                        onTap = () => this.pushToUserDetail(obj: data.userId);
                    }

                    textSpans.Add(new TextSpan(
                        text: name,
                        recognizer: new TapGestureRecognizer {
                            onTap = onTap
                        },
                        style: CTextStyle.PLargeMedium
                    ));
                    textSpans.Add(new TextSpan(" 发布了新文章", style: CTextStyle.PLargeBody2));
                    textSpans.Add(new TextSpan(
                        $"《{data.projectTitle}》",
                        style: CTextStyle.PLargeMedium
                    ));
                    break;
                }
            }

            if (type != "project_article_publish") {
                textSpans.Insert(0, new TextSpan(
                    text: data.fullname,
                    style: CTextStyle.PLargeMedium,
                    recognizer: new TapGestureRecognizer {
                        onTap = () => this.pushToUserDetail(obj: data.userId)
                    }
                ));
            }

            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget> {
                    new RichText(
                        maxLines: 2,
                        text: new TextSpan(
                            children: textSpans
                        ),
                        overflow: TextOverflow.ellipsis
                    ),
                    subTitleWidget
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

        string _analyzeComment(string comment) {
            return comment.isNotEmpty() 
                ? $" “{MessageUtils.AnalyzeMessage(content: comment, mentions: this.mentions, false)}”" 
                : "";
        }
    }
}