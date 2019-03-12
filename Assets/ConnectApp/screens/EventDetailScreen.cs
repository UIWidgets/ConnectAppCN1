using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class EventDetailScreen : StatefulWidget {
        public EventDetailScreen(
            Key key = null
        ) : base(key) {
        }


        public override State createState() {
            return new _EventDetailScreenState();
        }
    }

    internal class _EventDetailScreenState : State<EventDetailScreen> {
        private string _eventId;

        public override void initState() {
            base.initState();
            StoreProvider.store.Dispatch(new FetchEventDetailAction()
                {eventId = StoreProvider.store.state.eventState.detailId});
        }

        private Widget _headerView(BuildContext context, IEvent eventObj) {
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
                                        eventObj.background,
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
                                                                StoreProvider.store.Dispatch(
                                                                    new ClearEventDetailAction());
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

        private Widget _joinBar(IEvent eventObj) {
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
                                    $"{eventObj.participantsCount}位观众",
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
                converter: (state, dispatcher) => state.eventState.openChatWindow,
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
                    {"detailId", state.eventState.detailId},
                    {"eventDict", state.eventState.eventDict},
                    {"showChatWindow", state.eventState.showChatWindow},
                    {"openChatWindow", state.eventState.openChatWindow}
                },
                builder: (context1, viewModel) => {
                    var detailId = (string) viewModel["detailId"];
                    // TODO: get eventObj from eventState.eventDict
                    var eventDict = (Dictionary<string, IEvent>) viewModel["eventDict"];
                    IEvent eventObj = null;
                    if (eventDict.ContainsKey(detailId)) eventObj = eventDict[detailId];
                    var showChatWindow = (bool) viewModel["showChatWindow"];
                    var openChatWindow = (bool) viewModel["openChatWindow"];
                    if (StoreProvider.store.state.eventState.eventDetailLoading)
                        return new Container(
                            color: CColors.White,
                            child: new Container(child: new CustomActivityIndicator(radius: 16))
                        );
                    else if (eventObj == null) return new Container();
                    return new Container(
                        color: CColors.White,
                        child: new Stack(
                            children: new List<Widget> {
                                new Column(
                                    children: new List<Widget> {
                                        _headerView(context1, eventObj),
                                        new EventDetail(eventObj: eventObj),
                                    }
                                ),
                                !showChatWindow ? _chatWindow() : _joinBar(eventObj)
                            }
                        )
                    );
                }
            );
        }
    }
}