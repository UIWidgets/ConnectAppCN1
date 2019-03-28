using System;
using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
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
        private readonly TextEditingController _textController = new TextEditingController("");
        private readonly FocusNode _focusNode = new FocusNode();

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
            var ratio = 1.0f - 64.0f / (screenHeight - screenWidth * 9.0f / 16.0f);

            _position = new OffsetTween(
                new Offset(0, ratio),
                new Offset(0, 0)
            ).animate(new CurvedAnimation(
                _controller,
                Curves.easeInOut
            ));
        }
        
        private static Widget _buildHeadTop(bool isShowShare) {
            Widget shareWidget = new Container();
            if (isShowShare) {
                shareWidget = new CustomButton(
                    child: new Icon(
                        Icons.share,
                        size: 28,
                        color: CColors.White
                    ),
                    onPressed: () => ShareUtils.showShareView(new ShareView())
                );
            }
            return new Container(
                height: 44,
                padding: EdgeInsets.symmetric(horizontal: 8),
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
                        shareWidget
                    }
                )
            );
        }
        
        private static Widget _buildEventHeader(IEvent eventObj, EventType eventType, EventStatus eventStatus, bool isLoggedIn) {
            return new Stack(
                children: new List<Widget> {
                    new EventHeader(eventObj, eventType, eventStatus, isLoggedIn),
                    new Positioned(
                        left: 0,
                        top: 0,
                        right: 0,
                        child: _buildHeadTop(eventType == EventType.onLine)
                    )
                }
            );
        }
        
        private Widget _buildEventDetail(IEvent eventObj, EventType eventType, EventStatus eventStatus, bool isLoggedIn) {
            if (eventStatus != EventStatus.future && eventType == EventType.onLine && isLoggedIn) {
                return new Expanded(
                    child: new Stack(
                        fit: StackFit.expand,
                        children: new List<Widget>{
                            new Container(
                                margin: EdgeInsets.only(bottom: 64),
                                color: CColors.White,
                                child: new EventDetail(eventObj)
                            ),
                            Positioned.fill(
                                new Container(
                                    child: new SlideTransition(
                                        position: _position,
                                        child: _buildChatWindow(eventType, isLoggedIn)
                                    )
                                )
                            )
                        }
                    )
                );
            }
            return new Expanded(
                child: new EventDetail(eventObj)
            );
        }
        
        private static Widget _buildEventBottom(IEvent eventObj, EventType eventType, EventStatus eventStatus, bool isLoggedIn) {
            if (eventType == EventType.offline) {
                return _buildOfflineRegisterNow(eventObj, isLoggedIn);
            }
            if (eventStatus != EventStatus.future && eventType == EventType.onLine && isLoggedIn) {
                return new Container();
            }
            
            var onlineCount = eventObj.onlineMemberCount;
            var recordWatchCount = eventObj.recordWatchCount;
            var title = "";
            var subTitle = "";
            if (eventStatus == EventStatus.live) {
                title = "正在直播";
                subTitle = $"{onlineCount}人正在观看";
            }
            if (eventStatus == EventStatus.past) {
                title = "回放";
                subTitle = $"{recordWatchCount}次观看";
            }
            if (eventStatus == EventStatus.future || eventStatus == EventStatus.countDown) {
                title = "距离开始还有";
                subTitle = "10天10小时";
            }
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
                                    title,
                                    style: CTextStyle.PSmallBody4
                                ),
                                new Container(height: 2),
                                new Text(
                                    subTitle,
                                    style: CTextStyle.H5Body
                                )
                            }
                        ),
                        new CustomButton(
                            onPressed: () => StoreProvider.store.Dispatch(new ChatWindowShowAction {show = true}),
                            child: new Container(
                                width: 96,
                                height: 40,
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

        private Widget _buildChatWindow(EventType eventType, bool isLoggedIn) {
            return new StoreConnector<AppState, bool>(
                converter: (state, dispatcher) => state.eventState.showChatWindow,
                builder: (context, showChatWindow) => {
                    return new Container(
                        child: new Column(
                            children: new List<Widget> {
                                _buildChatBar(showChatWindow),
                                _buildChatList(),
                                new CustomDivider(
                                    height: 1,
                                    color: CColors.Separator
                                ),
                                _buildTextField()
                            }
                        )
                    );
                }
            );
        }

        private Widget _buildChatBar(bool showChatWindow) {
            IconData iconData;
            Widget bottomWidget;
            if (showChatWindow) {
                iconData = Icons.expand_more;
                bottomWidget = new Container();
            } else {
                iconData = Icons.expand_less;
                bottomWidget = new Text(
                    "轻点展开聊天",
                    style: CTextStyle.PSmallBody4
                );
            }
            return new GestureDetector(
                onTap: () => {
                    _focusNode.unfocus();
                    if (!showChatWindow) {
                        _controller.forward();
                    } else {
                        _controller.reverse();
                    }
                    StoreProvider.store.Dispatch(new ChatWindowShowAction {show = !showChatWindow});
                },
                child: new Container(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    color: CColors.White,
                    height: showChatWindow ? 44 : 64,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: new List<Widget>{
                            new Column(
                                mainAxisAlignment: MainAxisAlignment.center,
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Row(
                                        children: new List<Widget> {
                                            new Container(
                                                margin: EdgeInsets.only(right: 6),
                                                width: 6,
                                                height: 6,
                                                decoration: new BoxDecoration(
                                                    CColors.SecondaryPink,
                                                    borderRadius: BorderRadius.circular(3)
                                                )
                                            ),
                                            new Text(
                                                "直播聊天",
                                                style: new TextStyle(
                                                    height: 1.09f,
                                                    fontSize: 16,
                                                    fontFamily: "Roboto-Medium",
                                                    color: CColors.TextBody
                                                )
                                            )
                                        }
                                    ),
                                    bottomWidget
                                }
                            ),
                            new Icon(
                                iconData,
                                color: CColors.text3,
                                size: 28
                            )
                        }
                    )
                )
            );
        }

        private Widget _buildChatList() {
            return new Flexible(
                child: new GestureDetector(
                    onTap: () => _focusNode.unfocus(),
                    child: new Container(
                        color: CColors.White,
                        child: new StoreConnector<AppState, MessageState>(
                            converter: (state, dispatcher) => state.messageState,
                            builder: (_context, model) => {
                                return ListView.builder(
                                    padding: EdgeInsets.only(16, right: 16, bottom: 10),
                                    physics: new AlwaysScrollableScrollPhysics(),
                                    itemBuilder: (cxt, index) => new Container(
                                        height: 100,
                                        color: index / 2 == 0 ? CColors.Red : CColors.Black
                                    ), 
                                    itemCount: 10
                                );
                            }
                        )
                    )
                )
            );
        }

        private Widget _buildTextField() {
            var sendMessageLoading = false;
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.symmetric(horizontal: 16),
                height: 40,
                child: new Row(
                    children: new List<Widget>{
                        new Expanded(
                            child: new InputField(
                                // key: _textFieldKey,
                                controller: _textController,
                                focusNode: _focusNode,
                                height: 40,
                                style: new TextStyle(
                                    color: sendMessageLoading
                                        ? CColors.TextBody3
                                        : CColors.TextBody,
                                    fontFamily: "Roboto-Regular",
                                    fontSize: 16
                                ),
                                hintText: "说点想法…",
                                hintStyle: CTextStyle.PLargeBody4,
                                keyboardType: TextInputType.multiline,
                                maxLines: 1,
                                cursorColor: CColors.PrimaryBlue,
                                textInputAction: TextInputAction.send,
                                onChanged: text => { },
                                onSubmitted: text => { }
                            )
                        ),
                        sendMessageLoading
                            ? new Container(
                                width: 32,
                                height: 32,
                                child: new CustomActivityIndicator()
                            )
                            : new Container()
                    }
                )
            );
        }

        private static Widget _buildOfflineRegisterNow(IEvent eventObj, bool isLoggedIn) {
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
                    {"ongoingEventDict", state.eventState.ongoingEventDict}
                },
                builder: (_context, viewModel) => {
                    _setAnimationPosition(context);
                    var isLoggedIn = (bool) viewModel["isLoggedIn"];
                    var eventType = widget.eventType;
                    var eventId = widget.eventId;
                    var ongoingEventDict = (Dictionary<string, IEvent>) viewModel["ongoingEventDict"];
                    var eventObj = new IEvent();
                    if (ongoingEventDict.ContainsKey(eventId)) eventObj = ongoingEventDict[eventId];
                    var loading = (bool) viewModel["loading"];
                    if (loading || eventObj == null)
                        return new EventDetailLoading();
                    var eventStatus = DateConvert.GetEventStatus(eventObj.begin);

                    return new Container(
                        color: CColors.White,
                        child: new SafeArea(
                            child: new Container(
                                color: CColors.White,
                                child: new Column(
                                    children: new List<Widget> {
                                        _buildEventHeader(eventObj, eventType, eventStatus, isLoggedIn),
                                        _buildEventDetail(eventObj, eventType, eventStatus, isLoggedIn),
                                        _buildEventBottom(eventObj, eventType, eventStatus, isLoggedIn)
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