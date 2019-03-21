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
using Color = Unity.UIWidgets.ui.Color;

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
            StoreProvider.store.Dispatch(new FetchEventDetailAction
                {eventId = StoreProvider.store.state.eventState.detailId});
        }

        public override void dispose() {
            StoreProvider.store.Dispatch(new ClearEventDetailAction());
            base.dispose();
        }

        private static Widget _buildHeaderView(BuildContext context, IEvent eventObj, EventType eventType) {
            var bottomWidget = new Container();
            if (eventType == EventType.onLine)
                bottomWidget = new Container(
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
                );

            return new Container(
                color: new Color(0xFFD8D8D8),
                child: new AspectRatio(
                    aspectRatio: 16.0f / 9.0f,
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
                                                    onPressed: () => Navigator.pop(context),
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
                                    bottomWidget
                                }
                            )
                        }
                    )
                )
            );
        }

        private static Widget _buildJoinBar(IEvent eventObj) {
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

        private static Widget _buildChatWindow() {
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

        private static Widget _buildOfflineRegisterNow(BuildContext context, IEvent eventObj, bool isLoggedIn) {
            var buttonText = "立即报名";
            var isEnabled = false;
            if (eventObj.userIsCheckedIn && isLoggedIn) {
                buttonText = "已报名";
                isEnabled = true;
            }

            return new Container(
                color: CColors.White,
                height: 64,
                padding: EdgeInsets.symmetric(horizontal: 16),
                decoration: new BoxDecoration(
                    CColors.White,
                    border: new Border(new BorderSide(CColors.Separator))
                ),
                child: new CustomButton(
                    onPressed: () => {
                        if (isEnabled) return;

                        if (isLoggedIn)
                            StoreProvider.store.Dispatch(new JoinEventAction {eventId = eventObj.id});
                        else
                            Navigator.pushNamed(context, "/login");
                    },
                    child: new Container(
                        decoration: new BoxDecoration(
                            CColors.PrimaryBlue,
                            borderRadius: BorderRadius.all(4)
                        ),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: new List<Widget> {
                                new Text(
                                    buttonText,
                                    style: new TextStyle(
                                        color: CColors.White,
                                        fontSize: 16,
                                        fontFamily: "PingFang-Medium"
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }


        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, Dictionary<string, object>>(
                converter: (state, dispatcher) => new Dictionary<string, object> {
                    {"isLoggedIn", state.loginState.isLoggedIn},
                    {"eventType", state.eventState.eventType},
                    {"detailId", state.eventState.detailId},
                    {"loading", state.eventState.eventDetailLoading},
                    {"eventDict", state.eventState.eventDict},
                    {"showChatWindow", state.eventState.showChatWindow},
                    {"openChatWindow", state.eventState.openChatWindow}
                },
                builder: (_context, viewModel) => {
                    var isLoggedIn = (bool) viewModel["isLoggedIn"];
                    var eventType = (EventType) viewModel["eventType"];
                    var detailId = (string) viewModel["detailId"];
                    var eventDict = (Dictionary<string, IEvent>) viewModel["eventDict"];
                    IEvent eventObj = null;
                    if (eventDict.ContainsKey(detailId)) eventObj = eventDict[detailId];
                    var showChatWindow = (bool) viewModel["showChatWindow"];
                    var openChatWindow = (bool) viewModel["openChatWindow"];
                    var loading = (bool) viewModel["loading"];
                    if (loading && eventObj == null)
                        return new Container(
                            color: CColors.White,
                            child: new CustomActivityIndicator()
                        );
                    var bottomWidget = eventType == EventType.offline
                        ? _buildOfflineRegisterNow(context, eventObj, isLoggedIn)
                        : !showChatWindow
                            ? _buildChatWindow()
                            : _buildJoinBar(eventObj);

                    return new Container(
                        color: CColors.White,
                        child: new Stack(
                            children: new List<Widget> {
                                new Column(
                                    children: new List<Widget> {
                                        _buildHeaderView(context, eventObj, eventType),
                                        new EventDetail(eventObj: eventObj)
                                    }
                                ),
                                bottomWidget
                            }
                        )
                    );
                }
            );
        }
    }
}