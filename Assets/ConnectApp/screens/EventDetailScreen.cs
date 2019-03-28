using System;
using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.screens {
    public class EventDetailScreen : StatefulWidget {
        public EventDetailScreen(
            Key key = null,
            string eventId = null,
            EventType eventType = EventType.offline
        ) : base(key) {
            D.assert(eventId != null);
            this.eventId = eventId;
            this.eventType = eventType;
        }

        public readonly string eventId;
        public readonly EventType eventType;
        
        public override State createState() {
            return new _EventDetailScreenState();
        }
    }

    internal class _EventDetailScreenState : State<EventDetailScreen>, TickerProvider {
        
        private AnimationController _controller;
        private Animation<Offset> _position;

        public override void initState() {
            base.initState();
            StoreProvider.store.Dispatch(new FetchEventDetailAction
                {eventId = widget.eventId});
            // new CurvedAnimation();
            _controller = new AnimationController(
                duration: new TimeSpan(0,0,0,0,300),
                vsync: this
            );
        }

        public override void dispose() {
            _controller.dispose();
            base.dispose();
        }
        
        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick, $"created by {this}");
        }
        
        private void _setAnimationPosition(BuildContext context) {
            if (_position != null) {
                return;
            }
            var screenHeight = MediaQuery.of(context).size.height;
            var screenWidth = MediaQuery.of(context).size.width;
            var ratio = 1.0f - 44.0f / (screenHeight - (screenWidth * 9 / 16 + 20.0f));

            _position = new OffsetTween(
                new Offset(0, ratio),
                new Offset(0, 0)
            ).animate(new CurvedAnimation(
                _controller,
                Curves.easeInOut
            ));
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
                                height: 24,
                                width: 54,
                                decoration: new BoxDecoration(
                                    CColors.Black,
                                    borderRadius: BorderRadius.all(4)
                                ),
                                alignment: Alignment.center,
                                child: new Text(
                                    "未开始",
                                    style: CTextStyle.CaptionWhite
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
                            Positioned.fill(child:new Container(color:Color.fromRGBO(0,0,0,0.3f))),
                            new Flex(
                                Axis.vertical,
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Container(
                                        padding: EdgeInsets.symmetric(horizontal: 8),
                                        height: 44,
                                        decoration: new BoxDecoration(
                                            gradient: new LinearGradient(
                                                colors: new List<Color> {
                                                    new Color(0x80000000),
                                                    new Color(0x0)
                                                },
                                                begin: Alignment.topCenter,
                                                end: Alignment.bottomCenter
                                            )
                                        ),
                                        child: new Row(
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            children: new List<Widget> {
                                                new CustomButton(
                                                    onPressed: () => StoreProvider.store.Dispatch(new MainNavigatorPopAction()),
                                                    child: new Icon(
                                                        Icons.arrow_back,
                                                        size: 28,
                                                        color: CColors.White
                                                    )
                                                ),
                                                new CustomButton(
                                                    child: new Icon(
                                                        Icons.share,
                                                        size: 28,
                                                        color: CColors.White
                                                    ),
                                                    onPressed: () => {
                                                        ShareUtils.showShareView(new ShareView());
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
                height: 64,
                padding: EdgeInsets.symmetric(horizontal: 16),
                decoration: new BoxDecoration(
                    CColors.White,
                    border: new Border(new BorderSide(CColors.Separator))
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new Column(
                            mainAxisAlignment: MainAxisAlignment.center,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                new Text(
                                    "正在直播",
                                    style: CTextStyle.CaptionBody
                                ),
                                new Container(height: 2),
                                new Text(
                                    $"{eventObj.participantsCount}位观众",
                                    style: CTextStyle.PLargeMedium
                                )
                            }
                        ),
                        new CustomButton(
                            onPressed: () => StoreProvider.store.Dispatch(new ChatWindowShowAction {show = true}),
                            child: new Container(
                                width: 96,
                                height: 44,
                                decoration: new BoxDecoration(
                                    CColors.PrimaryBlue,
                                    borderRadius: BorderRadius.all(4)
                                ),
                                alignment: Alignment.center,
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: new List<Widget> {
                                        new Text(
                                            "立即加入",
                                            style: CTextStyle.PLargeMediumWhite
                                        )
                                    }
                                )
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
                            StoreProvider.store.Dispatch(new MainNavigatorPushToAction {RouteName = MainNavigatorRoutes.Login});
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
                                    style: CTextStyle.PLargeMediumWhite
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
                    {"loading", state.eventState.eventDetailLoading},
                    {"ongoingEventDict", state.eventState.ongoingEventDict},
                    {"showChatWindow", state.eventState.showChatWindow},
                    {"openChatWindow", state.eventState.openChatWindow}
                },
                builder: (_context, viewModel) => {
                    _setAnimationPosition(context);
                    var isLoggedIn = (bool) viewModel["isLoggedIn"];
                    var eventType = widget.eventType;
                    var eventId = widget.eventId;
                    var ongoingEventDict = (Dictionary<string, IEvent>) viewModel["ongoingEventDict"];
                    IEvent eventObj = null;
                    if (ongoingEventDict.ContainsKey(eventId)) eventObj = ongoingEventDict[eventId];
                    var showChatWindow = (bool) viewModel["showChatWindow"];
                    var openChatWindow = (bool) viewModel["openChatWindow"];
                    var loading = (bool) viewModel["loading"];
                    if (loading || eventObj == null)
                        return new Container(
                            color: CColors.White,
                            child: new CustomActivityIndicator(size:24)
                        );
                    var bottomWidget = eventType == EventType.offline
                        ? _buildOfflineRegisterNow(context, eventObj, isLoggedIn)
                        : showChatWindow
                            ? _buildChatWindow()
                            : _buildJoinBar(eventObj);

                    return new Container(
                        color: CColors.White,
                        child: new SafeArea(
                            child: new Container(
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
                            )
                        )
                    );
                }
            );
        }
    }
}