using System;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.screens;
using ConnectApp.utils;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.components {
    public class EventHeader : StatefulWidget {
        public EventHeader(
            IEvent eventObj,
            EventType eventType,
            EventStatus eventStatus,
            bool isLoggedIn,
            Key key = null
        ) : base(key) {
            this.eventObj = eventObj;
            this.eventType = eventType;
            this.eventStatus = eventStatus;
            this.isLoggedIn = isLoggedIn;
        }

        public readonly IEvent eventObj;
        public readonly EventType eventType;
        public readonly EventStatus eventStatus;
        public readonly bool isLoggedIn;

        public override State createState() {
            return new _EventHeaderState();
        }
    }

    internal class _EventHeaderState : State<EventHeader>, TickerProvider {
        private AnimationController _animationController;

        public override void initState() {
            base.initState();
            _animationController = new AnimationController(
                vsync: this,
                duration: new TimeSpan(0, 0, 655)
            );
        }

        public override void dispose() {
            _animationController.dispose();
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick, $"created by {this}");
        }

        public override Widget build(BuildContext context) {
            if (widget.eventObj == null) return new Container();

            var isLoggedIn = widget.isLoggedIn;
            if (widget.eventType == EventType.offline) return _buildOfflineHeaderView();
            if (!isLoggedIn) return _buildOnLineNotLoginHeaderView();
            if (widget.eventStatus == EventStatus.future) return _buildFutureView();
            if (widget.eventStatus == EventStatus.countDown) return _buildCountDownView();
            if (widget.eventStatus == EventStatus.live) return _buildLiveView();
            if (widget.eventStatus == EventStatus.past) return _buildPastView();
            return new Container();
        }

        private Widget _buildHeadImage(Widget child) {
            var eventObj = widget.eventObj;
            var imageUrl = eventObj.background != null ? eventObj.background : "";
            return new Container(
                color: new Color(0xFFD8D8D8),
                child: new AspectRatio(
                    aspectRatio: 16.0f / 9.0f,
                    child: new Stack(
                        fit: StackFit.expand,
                        children: new List<Widget> {
                            Image.network(
                                imageUrl,
                                fit: BoxFit.cover
                            ),
                            child
                        }
                    )
                )
            );
        }

        private static Widget _buildHeadBottom(Widget child) {
            return new Container(
                height: 44,
                padding: EdgeInsets.symmetric(horizontal: 16),
                decoration: new BoxDecoration(
                    gradient: new LinearGradient(
                        colors: new List<Color> {
                            new Color(0x0),
                            new Color(0x80000000)
                        },
                        begin: Alignment.topCenter,
                        end: Alignment.bottomCenter
                    )
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        child
                    }
                )
            );
        }

        private static Widget _buildFutureCard() {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 8, vertical: 2),
                decoration: new BoxDecoration(
                    CColors.Black,
                    borderRadius: BorderRadius.all(4)
                ),
                child: new Text(
                    "未开始",
                    style: CTextStyle.CaptionWhite
                )
            );
        }

        private static Widget _buildVideoTimeCard() {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 8, vertical: 2),
                decoration: new BoxDecoration(
                    CColors.Black,
                    borderRadius: BorderRadius.circular(4)
                ),
                child: new Row(
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(right: 6.7f),
                            child: new Icon(
                                Icons.replay,
                                color: CColors.White,
                                size: 12
                            )
                        ),
                        new Text(
                            "59:28",
                            style: CTextStyle.CaptionWhite
                        )
                    }
                )
            );
        }

        private Widget _buildOfflineHeaderView() {
            return _buildHeadImage(
                Positioned.fill(
                    new Container()
                )
            );
        }

        private Widget _buildOnLineNotLoginHeaderView() {
            if (widget.eventStatus == EventStatus.future) return _buildFutureView();
            if (widget.eventStatus == EventStatus.countDown) return _buildCountDownView();
            Widget child = new Container();
            if (widget.eventStatus == EventStatus.live)
                child = new Container(
                    padding: EdgeInsets.symmetric(horizontal: 8, vertical: 2),
                    decoration: new BoxDecoration(
                        CColors.SecondaryPink,
                        borderRadius: BorderRadius.circular(4)
                    ),
                    child: new Row(
                        children: new List<Widget> {
                            new Container(
                                width: 6,
                                height: 6,
                                margin: EdgeInsets.only(right: 4),
                                decoration: new BoxDecoration(
                                    CColors.White,
                                    borderRadius: BorderRadius.circular(3)
                                )
                            ),
                            new Text(
                                "直播",
                                style: CTextStyle.CaptionWhite
                            )
                        }
                    )
                );
            if (widget.eventStatus == EventStatus.past) child = _buildVideoTimeCard();

            return _buildHeadImage(
                new Positioned(
                    left: 16,
                    bottom: 16,
                    child: child
                )
            );
        }

        private Widget _buildFutureView() {
            return _buildHeadImage(
                new Positioned(
                    left: 16,
                    bottom: 16,
                    child: _buildFutureCard()
                )
            );
        }

        private Widget _buildCountDownView() {
            return _buildHeadImage(
                new EventCountDown(
                    new StepTween(
                        655,
                        0
                    ).animate(_animationController)
                )
            );
        }

        private Widget _buildLiveView() {
            return _buildHeadImage(
                new Container()
            );
        }

        private Widget _buildPastView()
        {
            return new VideoPlayerScreen();
        }
    }
}