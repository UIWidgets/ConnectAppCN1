using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class EventDetail : StatelessWidget {
        public EventDetail(
            IEvent eventObj = null,
            Key key = null
        ) : base(key) {
            this.eventObj = eventObj;
        }

        private readonly IEvent eventObj;

        public override Widget build(BuildContext context) {
            return new Container(child: _buildContent(context));
        }

        private Widget _buildContent(BuildContext context) {
            var items = new List<Widget>();
            items.Add(_buildContentHead());
            items.AddRange(ArticleDescription.map(context,eventObj.content, eventObj.contentMap));
            items.Add(_buildContentLecturerList());
            return new Container(
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: items.Count,
                    itemBuilder: (cxt, index) => items[index]
                )
            );
        }

        private Widget _buildContentHead() {
            var user = eventObj.user ?? new User();
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                margin: EdgeInsets.only(top: 16, bottom: 20),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text(
                            eventObj.title ?? "",
                            style: CTextStyle.H4
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 20),
                            child: new Row(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 8),
                                        child: new Avatar(
                                            user.id ?? "",
                                            32
                                        )
                                    ),
                                    new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Text(
                                                user.fullName ?? "",
                                                style: CTextStyle.PMediumBody
                                            ),
                                            new Text(
                                                $"{DateConvert.DateStringFromNow(eventObj.createdTime)}发布",
                                                style: CTextStyle.PSmallBody3
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }

        private Widget _buildContentDetail() {
            return new Container(
                margin: EdgeInsets.only(top: 44),
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new EventDescription(content: eventObj.content, contentMap: eventObj.contentMap)
                    }
                )
            );
        }

        private Widget _buildContentLecturerList() {
            var hosts = eventObj.hosts;
            if (hosts == null || hosts.Count == 0) return new Container();
            var hostItems = new List<Widget>();
            hostItems.Add(new Container(
                margin: EdgeInsets.symmetric(horizontal: 16, vertical: 40),
                height: 1,
                color: CColors.Separator2
            ));
            hostItems.Add(new Padding(
                padding: EdgeInsets.fromLTRB(16, 0, 16, 24),
                child: new Text(
                    "讲师",
                    style: CTextStyle.H4
                )
            ));
            hosts.ForEach(host => { hostItems.Add(_buildLecture(host)); });
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: hostItems
            );
        }

        private static Widget _buildLecture(User host) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                margin: EdgeInsets.only(bottom: 24),
                color: CColors.White,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Row(
                            children: new List<Widget> {
                                new Container(
                                    margin: EdgeInsets.only(right: 8),
                                    child: new Avatar(
                                        host.id,
                                        48
                                    )
                                ),
                                new Column(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Container(
                                            child: new Text(
                                                host.fullName, 
                                                style: new TextStyle(
                                                    color: CColors.TextBody,
                                                    fontFamily: "Roboto-Medium",
                                                    fontSize: 16
                                                )
                                            )
                                        ),
                                        new Container(
                                            child: new Text(
                                                host.title ?? "title",
                                                maxLines: 1, 
                                                overflow: TextOverflow.ellipsis,
                                                style: CTextStyle.PRegularBody3
                                            )
                                        )
                                    }
                                )
                            }
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 8),
                            child: new Text(
                                host.description,
                                style: new TextStyle(
                                    color: CColors.TextBody3,
                                    fontFamily: "Roboto-Regular",
                                    fontSize: 16
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}