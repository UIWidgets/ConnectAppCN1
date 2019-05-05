using System;
using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens
{
    
    public class EventOfflineDetailScreenConnector : StatelessWidget {
        public EventOfflineDetailScreenConnector(string eventId) {
            this.eventId = eventId;
        }

        private readonly string eventId;
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, EventDetailScreenViewModel>(
                converter: state => {
                    var channelId = state.eventState.channelId;
                    var channelMessageList = state.messageState.channelMessageList;
                    var messageList = new List<string>();
                    if (channelMessageList.ContainsKey(channelId))
                        messageList = channelMessageList[channelId];
                    return new EventDetailScreenViewModel {
                        eventId = eventId,
                        isLoggedIn = state.loginState.isLoggedIn,
                        eventDetailLoading = state.eventState.eventDetailLoading,
                        joinEventLoading = state.eventState.joinEventLoading,
                        channelId = state.eventState.channelId,
                        eventsDict = state.eventState.eventsDict
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new EventDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        openUrl = url => dispatcher.dispatch(new OpenUrlAction {url = url}),
                        copyText = text => dispatcher.dispatch(new CopyTextAction {text = text}),
                        startFetchEventDetail = () => dispatcher.dispatch(new StartFetchEventDetailAction()),
                        fetchEventDetail = (id, eventType) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchEventDetail(id, eventType)),
                        startJoinEvent = () => dispatcher.dispatch(new StartJoinEventAction()),
                        joinEvent = id => dispatcher.dispatch<IPromise>(Actions.joinEvent(id)),
                        shareToWechat = (type, title, description, linkUrl, imageUrl) => dispatcher.dispatch<IPromise>(
                            Actions.shareToWechat(type, title, description, linkUrl, imageUrl))
                    };
                    return new EventOfflineDetailScreen(viewModel, actionModel);
                }
            );
        }
    }
    
    
    public class EventOfflineDetailScreen : StatefulWidget
    {
        public EventOfflineDetailScreen(
            EventDetailScreenViewModel viewModel = null,
            EventDetailScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly EventDetailScreenViewModel viewModel;
        public readonly EventDetailScreenActionModel actionModel;

        public override State createState() {
            return new _EventOfflineDetailScreenState();
        }
    }

    internal class _EventOfflineDetailScreenState : State<EventOfflineDetailScreen>, TickerProvider
    {
        private AnimationController _controller;
        private Animation<Offset> _position;
        private string _loginSubId;
        private bool _showNavBarShadow;

        public override void initState() {
            base.initState();
            _showNavBarShadow = true;
            _controller = new AnimationController(
                duration: new TimeSpan(0, 0, 0, 0, 300),
                vsync: this
            );
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                widget.actionModel.startFetchEventDetail();
                widget.actionModel.fetchEventDetail(widget.viewModel.eventId, EventType.offline);
            });
            _loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
                widget.actionModel.startFetchEventDetail();
                widget.actionModel.fetchEventDetail(widget.viewModel.eventId, EventType.offline);
            });
        }

        public override Widget build(BuildContext context) {
            _setAnimationPosition(context);
            var eventObj = new IEvent();
            if (widget.viewModel.eventsDict.ContainsKey(widget.viewModel.eventId))
                eventObj = widget.viewModel.eventsDict[widget.viewModel.eventId];
            if ((widget.viewModel.eventDetailLoading || eventObj?.user == null)&&!eventObj.isNotFirst)
                return new EventDetailLoading(mainRouterPop: widget.actionModel.mainRouterPop);
            var eventStatus = DateConvert.GetEventStatus(eventObj.begin);
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        color: CColors.White,
                        child: new NotificationListener<ScrollNotification>(
                            onNotification: _onNotification,
                            child: new Column(
                                children: new List<Widget> {
                                    _buildEventDetail(context,eventObj),
                                    _buildOfflineRegisterNow(eventObj, widget.viewModel.isLoggedIn, eventStatus)
                                }
                            )
                        )
                    )
                )
            );
        }
        
        private Widget _buildEventDetail(BuildContext context, IEvent eventObj) {
            return new Expanded(
                child: new Stack(
                    children: new List<Widget> {
                        new EventDetail(true,eventObj, widget.actionModel.openUrl),
                        new Positioned(
                            left: 0,
                            top: 0,
                            right: 0,
                            child: _buildHeadTop(true, eventObj)
                        )
                    }
                )
            );
        }
        private bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            _showNavBarShadow = !(pixels >= 44);
            setState(() => { });
            return true;
        }
      
        public override void dispose() {
            EventBus.unSubscribe(EventBusConstant.login_success, _loginSubId);
            _controller.dispose();
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick);
        }

        private void _setAnimationPosition(BuildContext context) {
            if (_position != null) return;
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

        private Widget _buildHeadTop(bool isShowShare, IEvent eventObj) {
            Widget shareWidget = new Container();
            if (isShowShare)
                shareWidget = new CustomButton(
                    onPressed: () => ShareUtils.showShareView(new ShareView(
                        projectType: ProjectType.iEvent,
                        onPressed: type => {
                            var linkUrl =
                                $"{Config.apiAddress}/events/{eventObj.id}";
                            if (type == ShareType.clipBoard) {
                                widget.actionModel.copyText(linkUrl);
                                CustomDialogUtils.showToast("复制链接成功", Icons.check_circle_outline);
                            }
                            else {
                                var imageUrl = $"{eventObj.avatar}.200x0x1.jpg";
                                CustomDialogUtils.showCustomDialog(
                                    child: new CustomDialog()
                                );
                                widget.actionModel.shareToWechat(type, eventObj.title, eventObj.shortDescription,
                                        linkUrl,
                                        imageUrl).Then(CustomDialogUtils.hiddenCustomDialog)
                                    .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                            }
                        })),
                    child: new Container(
                        alignment: Alignment.topRight,
                        width: 64,
                        height: 64,
                        color: CColors.Transparent,
                        child: new Icon(Icons.share, size: 28, color: _showNavBarShadow ? CColors.White : CColors.icon3))
                );
            return new AnimatedContainer(
                height: 44,
                duration: new TimeSpan(0, 0, 0, 0, 0),
                padding: EdgeInsets.symmetric(horizontal: 8),
                decoration: new BoxDecoration(
                    CColors.White,
                    gradient: _showNavBarShadow ? new LinearGradient(
                        colors: new List<Color> {
                            new Color(0x80000000),
                            new Color(0x0)
                        },
                        begin: Alignment.topCenter,
                        end: Alignment.bottomCenter
                    ) : null 
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new CustomButton(
                            onPressed: () => widget.actionModel.mainRouterPop(),
                            child: new Icon(
                                Icons.arrow_back,
                                size: 28,
                                color: _showNavBarShadow ? CColors.White : CColors.icon3
                            )
                        ),
                        shareWidget
                    }
                )
            );
        }
        
        private Widget _buildOfflineRegisterNow(IEvent eventObj, bool isLoggedIn, EventStatus eventStatus) {

            if (eventObj.type.isNotEmpty() && !(eventObj.type == "bagevent" || eventObj.type == "customize")) {
                return new Container();
            }
            
            var buttonText = "立即报名";
            var backgroundColor = CColors.PrimaryBlue;
            var isEnabled = true;

            if (eventStatus == EventStatus.past) {
                buttonText = "已结束";
                backgroundColor = CColors.Disable;
                isEnabled = false;
            }

            return new Container(
                height: 64,
                padding: EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                decoration: new BoxDecoration(
                    CColors.White,
                    border: new Border(new BorderSide(CColors.Separator))
                ),
                child: new CustomButton(
                    onPressed: () => {
                        if (!isEnabled) return;
                        if (isLoggedIn && eventObj.type.isNotEmpty()) {
                            if (eventObj.type == "bagevent") {
                                widget.actionModel.openUrl(
                                    $"{Config.apiAddress}/events/{eventObj.id}/purchase");
                            }
                            else if (eventObj.type == "customize" && eventObj.typeParam.isNotEmpty()) {
                                widget.actionModel.openUrl(eventObj.typeParam);
                            }
                        }
                        else {
                            widget.actionModel.pushToLogin();
                        }
                    },
                    padding: EdgeInsets.zero,
                    child: new Container(
                        decoration: new BoxDecoration(
                            backgroundColor,
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
    }
}