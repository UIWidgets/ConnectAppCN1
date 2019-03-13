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

namespace ConnectApp.components {
    public class NotificationCard : StatelessWidget {
        public NotificationCard(
            Key key = null,
            NotificationResult notificationResult = null
        ) : base(key) {
            this.notificationResult = notificationResult;
        }

        public readonly NotificationResult notificationResult;

        public override Widget build(BuildContext context) {
            if (notificationResult == null) {
                return new Container();
            }

            var data = notificationResult.data;
            return new GestureDetector(
                onTap: () => {
                    StoreProvider.store.Dispatch(new NavigatorToLiveAction {eventId = data.projectId});
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
            var type = notificationResult.type;
            var data = notificationResult.data;
            var subTitle = new TextSpan();
            if (type == "project_liked") {
                subTitle = new TextSpan(
                    $"点赞了你的{data.projectTitle}文章", 
                    new TextStyle(
                        fontSize: 16,
                        fontFamily: "PingFang-Regular",
                        color: CColors.TextSecondary
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
            var createdTime = notificationResult.createdTime;
            return new Container(
                child: new Text(
                    DateConvert.DateStringFromNow(createdTime),
                    style: CTextStyle.TextBody4
                )
            );
        }
    }
}