using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.constants;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using Notification = ConnectApp.models.Notification;

namespace ConnectApp.components {
    public class NotificationCard : StatelessWidget {
        public NotificationCard(
            Key key = null,
            Notification notification = null
        ) : base(key) {
            this.notification = notification;
        }

        private readonly Notification notification;

        public override Widget build(BuildContext context) {
            if (notification == null) return new Container();
            var type = notification.type;
            if (type != "project_liked" && type != "project_message_commented") return new Container();

            var data = notification.data;
            return new GestureDetector(
                onTap: () => {
                    StoreProvider.store.Dispatch(new MainNavigatorPushToArticleDetailAction {ArticleId = data.projectId});
                },
                child: new Container(
                    color: CColors.Transparent,
                    child: new Row(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget> {
                            new Container(
                                padding: EdgeInsets.only(16, 16, 16),
                                child: new Avatar(
                                    data.userId,
                                    48
                                )
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
                                            _buildNotificationTitle(),
                                            _buildNotificationTime()
                                        }
                                    )
                                )
                            )
                        }
                    )
                )
            );
        }

        private Widget _buildNotificationTitle() {
            var type = notification.type;
            var data = notification.data;
            var subTitle = new TextSpan();
            if (type == "project_liked")
                subTitle = new TextSpan(
                    $" 点赞了你的{data.projectTitle}文章",
                    CTextStyle.PLargeBody2
                );
            if (type == "project_message_commented")
                subTitle = new TextSpan(
                    $" 评价了你的{data.projectTitle}文章",
                    CTextStyle.PLargeBody2
                );

            return new Container(
                child: new RichText(
                    text: new TextSpan(
                        children: new List<TextSpan> {
                            new TextSpan(
                                data.fullname,
                                CTextStyle.PLargeMedium
                            ),
                            subTitle
                        }
                    )
                )
            );
        }

        private Widget _buildNotificationTime() {
            var createdTime = notification.createdTime;
            return new Container(
                child: new Text(
                    DateConvert.DateStringFromNow(createdTime),
                    style: CTextStyle.PSmallBody4
                )
            );
        }
    }
}