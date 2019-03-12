using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class EventDetail : StatefulWidget {
        public EventDetail(
            Key key = null,
            IEvent eventObj = null
        ) : base(key) {
            this.eventObj = eventObj;
        }

        public readonly IEvent eventObj;

        public override State createState() {
            return new _EventDetailState();
        }
    }

    internal class _EventDetailState : State<EventDetail> {
        public override Widget build(BuildContext context) {
            return new Container(child: _content());
        }

        private Widget _content() {
            return new Flexible(
                child: new ListView(
                    physics: new AlwaysScrollableScrollPhysics(),
                    children: new List<Widget> {
                        _contentHead(),
                        _contentDetail(),
                        _contentLecturerList()
                    }
                )
            );
        }

        private Widget _contentHead() {
            var eventObj = widget.eventObj;
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text(
                            eventObj.title,
                            style: CTextStyle.H3
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 8),
                            child: new Text(
                                $"阅读 {eventObj.participantsCount} · {DateConvert.DateStringFromNow(eventObj.createdTime)}",
                                style: CTextStyle.PSmall)
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 24, bottom: 24),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.start,
                                crossAxisAlignment: CrossAxisAlignment.center,
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 8),
                                        child: new ClipRRect(
                                            borderRadius: BorderRadius.circular(16),
                                            child: new Container(
                                                height: 32,
                                                width: 32,
                                                child: Image.network(
                                                    eventObj.user.avatar,
                                                    fit: BoxFit.cover
                                                )
                                            )
                                        )
                                    ),


                                    new Column(
                                        mainAxisAlignment: MainAxisAlignment.start,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Container(height: 5),
                                            new Text(
                                                eventObj.user.fullName,
                                                style: CTextStyle.PRegular
                                            ),
                                            new Container(height: 5),
                                            new Text(
                                                DateConvert.DateStringFromNow(eventObj.createdTime),
                                                style: CTextStyle.PSmall
                                            )
                                        }
                                    )
                                }
                            )),
                    }
                )
            );
        }

        private Widget _contentDetail() {
            var eventObj = widget.eventObj;
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new EventDescription(content: eventObj.content, contentMap: eventObj.contentMap)
                    }
                )
            );
        }

        private Widget _contentLecturerList() {
            var eventObj = widget.eventObj;
            var hosts = eventObj.hosts;
            if (hosts.Count == 0) return new Container();
            var hostItems = new List<Widget>();
            hosts.ForEach(host => { hostItems.Add(_Lecture(host)); });
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget> {
                    new Padding(
                        padding: EdgeInsets.fromLTRB(16, 0, 0, 16),
                        child: new Text("讲师", style: new TextStyle(color: Color.white, fontSize: 17))),
                    new Container(
                        height: 238,
                        margin: EdgeInsets.only(bottom: 64),
                        padding: EdgeInsets.fromLTRB(16, 0, 16, 16),
                        child: new ListView(
                            physics: new AlwaysScrollableScrollPhysics(),
                            scrollDirection: Axis.horizontal,
                            children: hostItems)
                    ),
                }
            );
        }

        private Widget _Lecture(User host) {
            return new Container(
                width: 212,
                padding: EdgeInsets.only(top: 24),
                margin: EdgeInsets.only(right: 16),
                decoration: new BoxDecoration(
                    Color.fromARGB(255, 76, 76, 76)
                ),
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(right: 10),
                            decoration: new BoxDecoration(
                                borderRadius: BorderRadius.all(18)
                            ),
                            child: new ClipRRect(
                                borderRadius: BorderRadius.circular(40),
                                child: new Container(
                                    height: 80,
                                    width: 80,
                                    child: Image.network(
                                        host.avatar,
                                        fit: BoxFit.cover
                                    )
                                )
                            )
                            //child:  Image.network(host.avatar, width: 80, height: 80)
                        ),
                        new Container(margin: EdgeInsets.only(top: 12),
                            padding: EdgeInsets.symmetric(horizontal: 16),
                            child: new Text(host.fullName, style: new TextStyle(color: Color.white, fontSize: 16))
                        ),
                        new Container(margin: EdgeInsets.only(top: 4),
                            padding: EdgeInsets.symmetric(horizontal: 16),
                            child: new Text(host.title, maxLines: 1, overflow: TextOverflow.ellipsis,
                                style: new TextStyle(color: new Color(0xFF959595), fontSize: 16))
                        ),
                        new Container(margin: EdgeInsets.only(top: 12),
                            padding: EdgeInsets.symmetric(horizontal: 14),
                            child: new Text(host.description, textAlign: TextAlign.center, maxLines: 2,
                                overflow: TextOverflow.ellipsis,
                                style: new TextStyle(color: new Color(0xFFD8D8D8), fontSize: 16))
                        ),
                    }
                )
            );
        }
    }
}