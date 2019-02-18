using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class LiveDetail : StatefulWidget {
        public LiveDetail(
            Key key = null,
            LiveInfo liveInfo = null
        ) : base(key) {
            this.liveInfo = liveInfo;
        }

        public LiveInfo liveInfo;

        public override State createState() {
            return new _LiveDetailState();
        }
    }

    internal class _LiveDetailState : State<LiveDetail> {
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
            var liveInfo = widget.liveInfo;
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 16),
                        new Text(
                            liveInfo.title,
                            style: new TextStyle(
                                fontSize: 20,
                                color: CColors.text1
                            )
                        ),
                        new Container(height: 16),
                        new Row(
                            mainAxisAlignment: MainAxisAlignment.start,
                            crossAxisAlignment: CrossAxisAlignment.center,
                            children: new List<Widget> {
                                new Container(
                                    margin: EdgeInsets.only(right: 10),
                                    decoration: new BoxDecoration(
                                        borderRadius: BorderRadius.all(18)
                                    ),
                                    child: Image.network(
                                        liveInfo.user.avatar,
                                        height: 36,
                                        width: 36,
                                        fit: BoxFit.fill
                                    )
                                ),
                                new Column(
                                    mainAxisAlignment: MainAxisAlignment.start,
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Container(height: 5),
                                        new Text(
                                            liveInfo.user.fullName,
                                            style: new TextStyle(
                                                fontSize: 13,
                                                color: CColors.text1
                                            )
                                        ),
                                        new Container(height: 5),
                                        new Text(
                                            liveInfo.createdTime,
                                            style: new TextStyle(
                                                fontSize: 13,
                                                color: CColors.text2
                                            )
                                        )
                                    }
                                )
                            }
                        )
                    }
                )
            );
        }

        private Widget _contentDetail() {
            var liveInfo = widget.liveInfo;
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 40),
                        new Padding(
                            padding: EdgeInsets.fromLTRB(0.0, 24.0, 0.0, 16.0),
                            child: new Container(
                                child: new Text("内容介绍",
                                    style: new TextStyle(
                                        fontSize: 17,
                                        color: CColors.text1
                                    )
                                )
                            )
                        ),
                        new Container(height: 16),
                        new EventDescription(content: liveInfo.content, contentMap: liveInfo.contentMap)
                    }
                )
            );
        }

        private Widget _contentLecturerList() {
            var liveInfo = widget.liveInfo;
            var hosts = liveInfo.hosts;
            var hostItems = new List<Widget>();
            hosts.ForEach(host => { hostItems.Add(_Lecture(host)); });
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget> {
                    new Padding(
                        padding: EdgeInsets.fromLTRB(16.0, 0.0, 0.0, 16.0),
                        child: new Text("讲师", style: new TextStyle(color: Color.white, fontSize: 17.0))),
                    new Container(
                        height: 238,
                        margin:EdgeInsets.only(bottom:64),
                        padding: EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 16.0),
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
                width: 212.0,
                padding: EdgeInsets.only(top: 24.0),
                margin: EdgeInsets.only(right: 16.0),
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
                            child: Image.network(host.avatar, width: 80, height: 80)
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