using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using ConnectApp.redux;
using ConnectApp.redux.actions;
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

        public readonly Notification notification;

        public override Widget build(BuildContext context) {
            if (notification == null) {
                return new Container();
            }

            var data = notification.data;
            return new GestureDetector(
                onTap: () => {
                    StoreProvider.store.Dispatch(new NavigatorToEventDetailAction {eventId = data.projectId});
                    Navigator.pushNamed(context, "/detail");
                },
                child: new Container(
                    color: CColors.Transparent,
                    child: new Row(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget> {
                            new Container(
                                padding: EdgeInsets.only(16, 16, 16),
                                child: new ClipRRect(
                                    borderRadius: BorderRadius.circular(24),
                                    child: Image.asset("pikachu", width: 48, height: 48)
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
            if (type == "project_liked") {
                subTitle = new TextSpan(
                    $"点赞了你的{data.projectTitle}文章", 
                    new TextStyle(
                        fontSize: 16,
                        fontFamily: "PingFang-Regular",
                        color: CColors.TextBody2
                    )
                );
            }
            
            return new Container(
                child: new RichText(
                    text: new TextSpan(
                        children: new List<TextSpan> {
                            new TextSpan(
                                data.fullname,
                                new TextStyle(
                                    fontSize: 16,
                                    fontFamily: "PingFang-Medium",
                                    color: CColors.TextTitle
                                )
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
                    style: CTextStyle.TextBody4
                )
            );
        }
    }
}