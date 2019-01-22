using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Samples.ConnectApp.widgets;
using Unity.UIWidgets.widgets;

namespace Unity.UIWidgets.Samples.ConnectApp {
    public class DetailScreen : StatefulWidget {
        public DetailScreen(Key key = null) : base(key) {
        }

        public override State createState() {
            return new _DetailScreen();
        }
    }

    internal class _DetailScreen : State<DetailScreen> {
        Widget _headerView(BuildContext context) {
            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            decoration: new BoxDecoration(
                                CLColors.black
                            ),
                            height: 210,
                            child: new Stack(
                                fit: StackFit.expand,
                                children: new List<Widget> {
                                    Image.network(
                                        "https://blogs.unity3d.com/wp-content/uploads/2018/12/2018.3-BLOG_cover-1280x720.jpg",
                                        fit: BoxFit.cover
                                    ),
                                    new Flex(
                                        Axis.vertical,
                                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Padding(
                                                padding: EdgeInsets.symmetric(horizontal: 4),
                                                child: new Row(
                                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                                    children: new List<Widget> {
                                                        new CustomButton(
                                                            onPressed: () => Navigator.pop(context),
                                                            child: new Icon(
                                                                Icons.arrow_back,
                                                                size: 28.0,
                                                                color: CLColors.icon1
                                                            )
                                                        ),
                                                        new CustomButton(
                                                            child: new Icon(
                                                                Icons.share,
                                                                size: 28.0,
                                                                color: CLColors.icon1
                                                            )
                                                        )
                                                    }
                                                )
                                            ),
                                            new Container(
                                                height: 40,
                                                padding: EdgeInsets.symmetric(horizontal: 16),
                                                child: new Row(
                                                    mainAxisAlignment: MainAxisAlignment.start,
                                                    children: new List<Widget> {
                                                        new Container(
                                                            height: 20,
                                                            width: 48,
                                                            decoration: new BoxDecoration(
                                                                CLColors.text4
                                                            ),
                                                            alignment: Alignment.center,
                                                            child: new Text(
                                                                "未开始",
                                                                style: new TextStyle(
                                                                    fontSize: 12,
                                                                    color: CLColors.text1
                                                                )
                                                            )
                                                        )
                                                    }
                                                )
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

        Widget _contentHead() {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 16),
                        new Text(
                            "如何在Unity中创建ARCore的应用",
                            style: new TextStyle(
                                fontSize: 20,
                                color: CLColors.text1
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
                                    child: Image.asset(
                                        "mario",
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
                                            "杨栋",
                                            style: new TextStyle(
                                                fontSize: 13,
                                                color: CLColors.text1
                                            )
                                        ),
                                        new Container(height: 5),
                                        new Text(
                                            "5天前发布",
                                            style: new TextStyle(
                                                fontSize: 13,
                                                color: CLColors.text2
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

        Widget _contentDetail() {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height: 40),
                        new Text(
                            "直播介绍",
                            style: new TextStyle(
                                fontSize: 17,
                                color: CLColors.text1
                            )
                        ),
                        new Container(height: 16),
                        new Text(
                            "最近谷歌发布了ARCore预览版，它将为整个Android生态系统启用AR（增强现实）开发，为开发人员提供了为数百万用户构建迷人AR体验的能力，而无需专门的硬件。Unity的XR团队一直与Google的团队紧密合作，以克服AR开发呈现的一些最困难的挑战。而随着Unity2017.2的发布，开发者已经可以很方便的在Unity中使用ARCore创建应用。",
                            style: new TextStyle(
                                fontSize: 14,
                                color: CLColors.text1
                            )
                        ),
                        new Container(height: 16),
                        new Text(
                            "内容：" +
                            "这次直播主要介绍如何在Unity中使用ARCore。内容包括ARCore基本介绍，和其他AR解决方案的区别，以及如何创建一个ARCore的项目。在项目实例中，我们将会展示如何在真实场景中添加一个动画角色，以及如何在真实场景中创建通向另一个虚拟世界的入口。",
                            style: new TextStyle(
                                fontSize: 14,
                                color: CLColors.text1
                            )
                        )
                    }
                )
            );
        }

        Widget _content() {
            return new Flexible(
                child: new ListView(
                    physics: new AlwaysScrollableScrollPhysics(),
                    children: new List<Widget> {
                        this._contentHead(),
                        new Container(height: 40),
                        this._contentDetail()
                    }
                )
            );
        }

        Widget _joinBar() {
            return new Container(
                color: CLColors.background1,
                height: 64,
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                new Container(height: 14),
                                new Text(
                                    "正在直播",
                                    style: new TextStyle(
                                        fontSize: 12,
                                        color: CLColors.text2
                                    )
                                ),
                                new Container(height: 2),
                                new Text(
                                    "34位观众",
                                    style: new TextStyle(
                                        fontSize: 16,
                                        color: CLColors.text1
                                    )
                                )
                            }
                        ),
                        new CustomButton(
                            child: new Container(
                                width: 100,
                                height: 44,
                                decoration: new BoxDecoration(
                                    CLColors.primary
                                ),
                                alignment: Alignment.center,
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: new List<Widget> {
                                        new Text(
                                            "立即加入",
                                            maxLines: 1,
                                            style: new TextStyle(
                                                fontSize: 16,
                                                color: CLColors.text1
                                            )
                                        )
                                    })
                            )
                        )
                    }
                )
            );
        }

        Widget _chatWindow() {
            return new Container(
                height: 420,
                decoration: new BoxDecoration(
                    CLColors.red
                ),
                child: new Text(
                    "chatWindow",
                    style: new TextStyle(
                        fontSize: 16,
                        color: CLColors.text1
                    )
                )
            );
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CLColors.background2,
                child: new Stack(
                    children: new List<Widget> {
                        new Column(
                            children: new List<Widget> {
                                this._headerView(context),
                                this._content()
                            }
                        ),
                        this._joinBar()
                    }
                )
            );
        }
    }
}