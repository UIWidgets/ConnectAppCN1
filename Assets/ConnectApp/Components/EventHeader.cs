using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
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

    class _EventHeaderState : State<EventHeader>, TickerProvider {
        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick, () => $"created by {this}");
        }

        public override Widget build(BuildContext context) {
            if (this.widget.eventObj == null) {
                return new Container();
            }

            var isLoggedIn = this.widget.isLoggedIn;
            if (this.widget.eventType == EventType.offline) {
                return this._buildOfflineHeaderView();
            }

            if (!isLoggedIn) {
                return this._buildOnLineNotLoginHeaderView();
            }

            if (this.widget.eventStatus == EventStatus.future) {
                return this._buildFutureView();
            }

            if (this.widget.eventStatus == EventStatus.countDown) {
                return this._buildCountDownView();
            }

            if (this.widget.eventStatus == EventStatus.live) {
                return this._buildLiveView();
            }

            if (this.widget.eventStatus == EventStatus.past) {
                return this._buildPastView();
            }

            return new Container();
        }

        Widget _buildHeadImage(Widget child) {
            var eventObj = this.widget.eventObj;
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

        static Widget _buildFutureCard() {
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

        static Widget _buildVideoTimeCard(float recordDuration) {
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
                            DateConvert.formatTime(recordDuration),
                            style: CTextStyle.CaptionWhite
                        )
                    }
                )
            );
        }

        Widget _buildOfflineHeaderView() {
            return this._buildHeadImage(
                Positioned.fill(
                    new Container()
                )
            );
        }

        Widget _buildOnLineNotLoginHeaderView() {
            if (this.widget.eventStatus == EventStatus.future) {
                return this._buildFutureView();
            }

            if (this.widget.eventStatus == EventStatus.countDown) {
                return this._buildCountDownView();
            }

            Widget child = new Container();
            if (this.widget.eventStatus == EventStatus.live) {
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
            }

            if (this.widget.eventStatus == EventStatus.past && this.widget.eventObj.recordDuration > 0) {
                child = _buildVideoTimeCard(this.widget.eventObj.recordDuration);
            }

            return this._buildHeadImage(
                new Positioned(
                    left: 16,
                    bottom: 16,
                    child: child
                )
            );
        }

        Widget _buildFutureView() {
            return this._buildHeadImage(
                new Positioned(
                    left: 16,
                    bottom: 16,
                    child: _buildFutureCard()
                )
            );
        }

        Widget _buildCountDownView() {
            return this._buildHeadImage(
                new Container()
            );
        }

        Widget _buildLiveView() {
            return this._buildHeadImage(
                new Container()
            );
        }

        Widget _buildPastView() {
            return this._buildHeadImage(
                new Container()
            );
        }
    }
}