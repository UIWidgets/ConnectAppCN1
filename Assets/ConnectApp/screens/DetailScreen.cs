using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class DetailScreen : StatefulWidget {
        public DetailScreen(
            Key key = null
        ) : base(key) {
        }


        public override State createState() {
            return new _DetailScreen();
        }
    }

    internal class _DetailScreen : State<DetailScreen> {
        private string _eventId;

        public override void initState() {
            base.initState();
            StoreProvider.store.Dispatch(new FetchEventDetailAction()
                {eventId = StoreProvider.store.state.LiveState.detailId});
        }

        private Widget _headerView(BuildContext context, LiveInfo liveInfo) {
            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            color: CColors.Black,
                            height: 210,
                            child: new Stack(
                                fit: StackFit.expand,
                                children: new List<Widget> {
                                    Image.network(
                                        liveInfo.background,
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
                                                            onPressed: () => {
                                                                Navigator.pop(context);
                                                                StoreProvider.store.Dispatch(new ClearLiveInfoAction());
                                                            },
                                                            child: new Icon(
                                                                Icons.arrow_back,
                                                                size: 28,
                                                                color: CColors.icon1
                                                            )
                                                        ),
                                                        new CustomButton(
                                                            child: new Icon(
                                                                Icons.share,
                                                                size: 28,
                                                                color: CColors.icon1
                                                            ),
                                                            onPressed: () => {
                                                                ShareUtils.showShareView(context, new ShareView());
                                                            }
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
                                                                CColors.text4
                                                            ),
                                                            alignment: Alignment.center,
                                                            child: new Text(
                                                                "未开始",
                                                                style: new TextStyle(
                                                                    fontSize: 12,
                                                                    color: CColors.text1
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

        private Widget _contentHead(LiveInfo liveInfo) {
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

        private Widget _contentDetail(LiveInfo liveInfo) {
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
                            liveInfo.shortDescription,
                            style: new TextStyle(
                                fontSize: 14,
                                color: CColors.text1
                            )
                        ),
                    }
                )
            );
        }

        private Widget _content(LiveInfo liveInfo) {
            return new Flexible(
                child: new ListView(
                    physics: new AlwaysScrollableScrollPhysics(),
                    children: new List<Widget> {
                        _contentHead(liveInfo),
                        new Container(height: 40),
                        //_contentDetail(liveInfo)
                    }
                )
            );
        }

        private Widget _joinBar(LiveInfo liveinfo) {
            return new Container(
                color: CColors.background1,
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
                                        color: CColors.text2
                                    )
                                ),
                                new Container(height: 2),
                                new Text(
                                    $"{liveinfo.participantsCount}位观众",
                                    style: new TextStyle(
                                        fontSize: 16,
                                        color: CColors.text1
                                    )
                                )
                            }
                        ),
                        new CustomButton(
                            onPressed: () => StoreProvider.store.Dispatch(new ChatWindowShowAction {show = true}),
                            child: new Container(
                                width: 100,
                                height: 44,
                                decoration: new BoxDecoration(
                                    CColors.primary
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
                                                color: CColors.text1
                                            )
                                        )
                                    })
                            )
                        )
                    }
                )
            );
        }

        private Widget _chatWindow() {
            return new StoreConnector<AppState, bool>(
                converter: (state, dispatcher) => state.LiveState.openChatWindow,
                builder: (context, openChatWindow) => {
                    return new GestureDetector(
                        onTap: () =>
                            StoreProvider.store.Dispatch(new ChatWindowStatusAction {status = !openChatWindow}),
                        child: new Container(
                            height: openChatWindow ? 457 : 64,
                            width: 375,
                            decoration: new BoxDecoration(
                                CColors.Red
                            ),
                            child: new Text(
                                "chatWindow",
                                style: new TextStyle(
                                    fontSize: 16,
                                    color: CColors.text1
                                )
                            )
                        )
                    );
                }
            );
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, Dictionary<string, object>>(
                converter: (state, dispatcher) => new Dictionary<string, object> {
                    {"liveInfo", state.LiveState.liveInfo},
                    {"showChatWindow", state.LiveState.showChatWindow},
                    {"openChatWindow", state.LiveState.openChatWindow}
                },
                builder: (context1, viewModel) => {
                    var liveInfo = viewModel["liveInfo"] as LiveInfo;
                    var showChatWindow = (bool) viewModel["showChatWindow"];
                    var openChatWindow = (bool) viewModel["openChatWindow"];
                    if (StoreProvider.store.state.LiveState.loading)
                        return new Container(
                            color: CColors.background2,
                            child: new Container(child: new CustomActivityIndicator(radius: 16))
                        );
                    else if (liveInfo == null) return new Container();
                    return new Container(
                        color: CColors.background2,
                        child: new Stack(
                            children: new List<Widget> {
                                new Column(
                                    children: new List<Widget> {
                                        _headerView(context1, liveInfo),
                                        new LiveDetail(liveInfo: liveInfo),
                                    }
                                ),
                                showChatWindow ? _chatWindow() : _joinBar(liveInfo)
                            }
                        )
                    );
                }
            );
        }
    }
}