using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components
{
    public class LiveDetail : StatefulWidget
    {
        public LiveDetail(
            Key key = null,
            LiveInfo liveInfo = null
        ) : base(key){
            this.liveInfo = liveInfo;
        }

        public LiveInfo liveInfo;
        
        public override State createState() {
            return new _LiveDetailState();
        }
    }

    internal class _LiveDetailState : State<LiveDetail>
    {
        
        public override Widget build(BuildContext context)
        {
            return new Container(child:_content());
        }
        
        private Widget _content() {
            return new Flexible(
                child: new ListView(
                    physics: new AlwaysScrollableScrollPhysics(),
                    children: new List<Widget> {
                        _contentHead(),
                        new Container(height: 40),
                        _contentDetail()
                    }
                )
            );
        }
        private Widget _contentHead()
        {
            LiveInfo liveInfo = widget.liveInfo;
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 16),
                        new Text(
                            data:liveInfo.title,
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
                                        src:liveInfo.user.avatar,
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
                                            data:liveInfo.user.fullName,
                                            style: new TextStyle(
                                                fontSize: 13,
                                                color: CColors.text1
                                            )
                                        ),
                                        new Container(height: 5),
                                        new Text(
                                            data:liveInfo.createdTime,
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
            LiveInfo liveInfo = widget.liveInfo;
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 40),
                        new Text(
                            "内容介绍",
                            style: new TextStyle(
                                fontSize: 17,
                                color: CColors.text1
                            )
                        ),
                        new Container(height: 16),
                        new Text(
                            data:liveInfo.shortDescription,
                            style: new TextStyle(
                                fontSize: 14,
                                color: CColors.text1
                            )
                        ),
                    }
                )
            );
        }
    }
    
    
    
}