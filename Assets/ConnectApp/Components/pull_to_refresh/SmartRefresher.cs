using System;
using System.Collections.Generic;
using System.Reflection;
using ConnectApp.Constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Components.pull_to_refresh {
    public static class RefreshStatus {
        public const int idle = 0;
        public const int canRefresh = 1;
        public const int refreshing = 2;
        public const int completed = 3;
        public const int failed = 4;
        public const int noMore = 5;
    }

    public class SmartRefresher : StatefulWidget {
        public SmartRefresher(
            ScrollView child,
            float initialOffset = 0f,
            IndicatorBuilder headerBuilder = null,
            IndicatorBuilder footerBuilder = null,
            Config headerConfig = null,
            Config footerConfig = null,
            bool enablePullUp = DefaultConstants.default_enablePullUp,
            bool enablePullDown = DefaultConstants.default_enablePullDown,
            bool enableOverScroll = DefaultConstants.default_enableOverScroll,
            bool reverse = false,
            OnRefresh onRefresh = null,
            OnOffsetChange onOffsetChange = null,
            RefreshController controller = null,
            NotificationListenerCallback<ScrollNotification> onNotification = null,
            bool hasBottomMargin = false,
            Key key = null
        ) : base(key: key) {
            this.child = child;
            this.initialOffset = initialOffset;
            this.headerBuilder =
                headerBuilder ?? ((context, mode) => new SmartRefreshHeader(mode, RefreshHeaderType.other));
            this.footerBuilder = footerBuilder ?? ((context, mode) => new SmartRefreshFooter(
                                     mode: mode,
                                     hasBottomMargin
                                         ? EdgeInsets.only(0, 16, 0,
                                             16 + CConstant.TabBarHeight +
                                             MediaQuery.of(context: context).padding.bottom)
                                         : null
                                 ));
            this.headerConfig = headerConfig ?? new RefreshConfig();
            this.footerConfig = footerConfig ?? new LoadConfig(triggerDistance: 0);
            this.enablePullUp = enablePullUp;
            this.enablePullDown = enablePullDown;
            this.enableOverScroll = enableOverScroll;
            this.onRefresh = onRefresh;
            this.onOffsetChange = onOffsetChange;
            this.controller = controller ?? new RefreshController();
            this.onNotification = onNotification;
            this.reverse = reverse;
        }

        public readonly ScrollView child;

        public readonly IndicatorBuilder headerBuilder;

        public readonly IndicatorBuilder footerBuilder;

        // configure your header and footer
        public readonly Config headerConfig, footerConfig;

        // This bool will affect whether or not to have the function of drop-up load.
        public readonly bool enablePullUp;

        //This bool will affect whether or not to have the function of drop-down refresh.
        public readonly bool enablePullDown;

        // if open OverScroll if you use RefreshIndicator and LoadFooter
        public readonly bool enableOverScroll;

        // upper and downer callback when you drag out of the distance
        public readonly OnRefresh onRefresh;

        // This method will callback when the indicator changes from edge to edge.
        public readonly OnOffsetChange onOffsetChange;

        //controller inner state
        public readonly RefreshController controller;

        public readonly float initialOffset;

        public readonly NotificationListenerCallback<ScrollNotification> onNotification;

        public readonly bool reverse;

        public override State createState() {
            return new _SmartRefresherState();
        }
    }

    public class _SmartRefresherState : State<SmartRefresher> {
        ScrollController _scrollController;
        readonly GlobalKey _headerKey = GlobalKey.key();
        readonly GlobalKey _footerKey = GlobalKey.key();
        float _headerHeight = DefaultConstants.default_VisibleRange;
        float _footerHeight = DefaultConstants.default_VisibleRange;
        readonly ValueNotifier<float> offsetLis = new ValueNotifier<float>(0.0f);
        readonly ValueNotifier<int> topModeLis = new ValueNotifier<int>(0);
        readonly ValueNotifier<int> bottomModeLis = new ValueNotifier<int>(0);

        bool _handleScrollStart(ScrollStartNotification notification) {
            // This is used to interrupt useless callback when the pull up load rolls back.
            if (notification.metrics.outOfRange()) {
                return false;
            }

            GestureProcessor topWrap = this._headerKey.currentState as GestureProcessor;
            GestureProcessor bottomWrap = this._footerKey.currentState as GestureProcessor;
            if (this.widget.enablePullUp) {
                bottomWrap.onDragStart(notification);
            }

            if (this.widget.enablePullDown) {
                topWrap.onDragStart(notification);
            }

            return false;
        }

        bool _handleScrollMoving(ScrollUpdateNotification notification) {
            if (this._measure(notification) != -1.0) {
                this.offsetLis.value = this._measure(notification);
            }

            GestureProcessor topWrap = this._headerKey.currentState as GestureProcessor;
            GestureProcessor bottomWrap = this._footerKey.currentState as GestureProcessor;
            if (this.widget.enablePullUp) {
                bottomWrap.onDragMove(notification);
            }

            if (this.widget.enablePullDown) {
                topWrap.onDragMove(notification);
            }

            return false;
        }

        bool _handleScrollEnd(ScrollNotification notification) {
            GestureProcessor topWrap = this._headerKey.currentState as GestureProcessor;
            GestureProcessor bottomWrap = this._footerKey.currentState as GestureProcessor;
            if (this.widget.enablePullUp) {
                bottomWrap.onDragEnd(notification);
            }

            if (this.widget.enablePullDown) {
                topWrap.onDragEnd(notification);
            }

            return false;
        }

        bool _dispatchScrollEvent(ScrollNotification notification) {
            this.widget.onNotification?.Invoke(notification);

            var axisDirection = notification.metrics.axisDirection;
            var scrollDirection = this.widget.child.scrollDirection;
            if ((axisDirection == AxisDirection.left || axisDirection == AxisDirection.right) && scrollDirection == Axis.vertical) {
                return false;
            }

            if ((axisDirection == AxisDirection.up || axisDirection == AxisDirection.down) && scrollDirection == Axis.horizontal) {
                return false;
            }

            // when is scroll in the ScrollInside,nothing to do
            if (!_isPullUp(notification) && !_isPullDown(notification)) {
                return false;
            }

            if (notification is ScrollStartNotification startNotification) {
                return this._handleScrollStart(startNotification);
            }

            if (notification is ScrollUpdateNotification updateNotification) {
                //if dragDetails is null,This represents the user's finger out of the screen
                if (updateNotification.dragDetails == null) {
                    return this._handleScrollEnd(notification);
                }

                if (updateNotification.dragDetails != null) {
                    return this._handleScrollMoving(updateNotification);
                }
            }

            if (notification is ScrollEndNotification) {
                this._handleScrollEnd(notification);
            }

            return false;
        }

        static bool _isPullUp(ScrollNotification notification) {
            return notification.metrics.pixels < 0;
        }

        static bool _isPullDown(ScrollNotification notification) {
            return notification.metrics.pixels > 0;
        }

        float _measure(ScrollNotification notification) {
            if (notification.metrics.minScrollExtent - notification.metrics.pixels >
                0) {
                return (notification.metrics.minScrollExtent -
                        notification.metrics.pixels) / this.widget.headerConfig.triggerDistance;
            }

            if (notification.metrics.pixels -
                notification.metrics.maxScrollExtent >
                0) {
                return (notification.metrics.pixels -
                        notification.metrics.maxScrollExtent) / this.widget.footerConfig.triggerDistance;
            }

            return -1.0f;
        }

        void _init() {
            this._scrollController = new ScrollController(initialScrollOffset: this.widget.initialOffset);
            this.widget.controller.scrollController = this._scrollController;
            SchedulerBinding.instance.addPostFrameCallback(duration => { this._onAfterBuild(); });
            this._scrollController.addListener(this._handleOffsetCallback);
            this.widget.controller._headerMode = this.topModeLis;
            this.widget.controller._footerMode = this.bottomModeLis;
        }

        void _handleOffsetCallback() {
            var overScrollPastStart = Mathf.Max(this._scrollController.position.minScrollExtent -
                                                this._scrollController.position.pixels +
                                                (this.widget.headerConfig is RefreshConfig &&
                                                 (this.topModeLis.value == RefreshStatus.refreshing ||
                                                  this.topModeLis.value == RefreshStatus.completed ||
                                                  this.topModeLis.value == RefreshStatus.failed)
                                                    ? (this.widget.headerConfig as RefreshConfig).visibleRange
                                                    : 0.0f),
                0.0f);
            var overScrollPastEnd = Mathf.Max(this._scrollController.position.pixels -
                                              this._scrollController.position.maxScrollExtent +
                                              (this.widget.footerConfig is RefreshConfig &&
                                               (this.bottomModeLis.value == RefreshStatus.refreshing ||
                                                this.bottomModeLis.value == RefreshStatus.completed ||
                                                this.bottomModeLis.value == RefreshStatus.failed)
                                                  ? (this.widget.footerConfig as RefreshConfig).visibleRange
                                                  : 0.0f),
                0.0f);
            if (overScrollPastStart > overScrollPastEnd) {
                if (this.widget.headerConfig is RefreshConfig) {
                    if (this.widget.onOffsetChange != null) {
                        this.widget.onOffsetChange(true, overScrollPastStart);
                    }
                }
                else {
                    if (this.widget.onOffsetChange != null) {
                        this.widget.onOffsetChange(true, overScrollPastStart);
                    }
                }
            }
            else if (overScrollPastEnd > 0) {
                if (this.widget.footerConfig is RefreshConfig) {
                    if (this.widget.onOffsetChange != null) {
                        this.widget.onOffsetChange(false, overScrollPastEnd);
                    }
                }
                else {
                    if (this.widget.onOffsetChange != null) {
                        this.widget.onOffsetChange(false, overScrollPastEnd);
                    }
                }
            }
        }

        void _didChangeMode(bool up, ValueNotifier<int> mode) {
            switch (mode.value) {
                case RefreshStatus.refreshing:
                    if (this.widget.onRefresh != null) {
                        this.widget.onRefresh(up);
                    }

                    if (up && this.widget.headerConfig is RefreshConfig config) {
                        this._scrollController
                            .jumpTo(this._scrollController.offset + config.visibleRange);
                    }

                    break;
            }
        }

        void _onAfterBuild() {
            if (this.widget.headerConfig is LoadConfig loadConfig) {
                if (loadConfig.bottomWhenBuild) {
                    this._scrollController.jumpTo(-(this._scrollController.position.pixels -
                                                    this._scrollController.position.maxScrollExtent));
                }
            }

            this.topModeLis.addListener(() => { this._didChangeMode(true, this.topModeLis); });
            this.bottomModeLis.addListener(() => { this._didChangeMode(false, this.bottomModeLis); });
            this.setState(() => {
                if (this.widget.enablePullDown) {
                    this._headerHeight = this._headerKey.currentContext.size.height;
                }

                if (this.widget.enablePullUp) {
                    this._footerHeight = this._footerKey.currentContext.size.height;
                }
            });
        }

        Widget _buildWrapperByConfig(Config config, bool up) {
            if (config is LoadConfig loadConfig) {
                return new LoadWrapper(
                    key: up ? this._headerKey : this._footerKey,
                    modeListener: up ? this.topModeLis : this.bottomModeLis,
                    up: up,
                    autoLoad: loadConfig.autoLoad,
                    triggerDistance: loadConfig.triggerDistance,
                    builder: up ? this.widget.headerBuilder : this.widget.footerBuilder
                );
            }

            if (config is RefreshConfig refreshConfig) {
                return new RefreshWrapper(
                    key: up ? this._headerKey : this._footerKey,
                    modeListener: up ? this.topModeLis : this.bottomModeLis,
                    up: up,
                    onOffsetChange: (offsetUp, offset) => {
                        if (this.widget.onOffsetChange != null) {
                            this.widget.onOffsetChange(
                                offsetUp,
                                offsetUp
                                    ? -this._scrollController.offset + offset
                                    : this._scrollController.position.pixels -
                                      this._scrollController.position.maxScrollExtent +
                                      offset);
                        }
                    },
                    completeDuration: refreshConfig.completeDuration,
                    triggerDistance: refreshConfig.triggerDistance,
                    visibleRange: refreshConfig.visibleRange,
                    builder: up ? this.widget.headerBuilder : this.widget.footerBuilder
                );
            }

            return new Container();
        }

        public override void dispose() {
            this._scrollController.removeListener(this._handleOffsetCallback);
            this._scrollController.dispose();
            base.dispose();
        }

        public override void initState() {
            base.initState();
            this._init();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            this.widget.controller._headerMode = this.topModeLis;
            this.widget.controller._footerMode = this.bottomModeLis;
            this.widget.controller.scrollController = this._scrollController;
            base.didUpdateWidget(oldWidget);
        }

        public override Widget build(BuildContext context) {
            var type = typeof(ScrollView);
            var method = type.GetMethod("buildSlivers", BindingFlags.NonPublic | BindingFlags.Instance);
            var objs = new object[1];
            objs[0] = context;
            var slivers = (List<Widget>) method.Invoke(this.widget.child, objs);
            slivers.Add(new SliverToBoxAdapter(
                child: this.widget.footerBuilder != null && this.widget.enablePullUp
                    ? this._buildWrapperByConfig(this.widget.footerConfig, false)
                    : new Container()
            ));
            slivers.Insert(
                0,
                new SliverToBoxAdapter(
                    child: this.widget.headerBuilder != null && this.widget.enablePullDown
                        ? this._buildWrapperByConfig(this.widget.headerConfig, true)
                        : new Container()));
            return new LayoutBuilder(builder: (cxt, cons) => {
                return new Stack(
                    children: new List<Widget> {
                        new Positioned(
                            top: !this.widget.enablePullDown || this.widget.headerConfig is LoadConfig
                                ? 0.0f
                                : -this._headerHeight,
                            bottom: !this.widget.enablePullUp || this.widget.footerConfig is LoadConfig
                                ? 0.0f
                                : -this._footerHeight,
                            left: 0.0f,
                            right: 0.0f,
                            child: new NotificationListener<ScrollNotification>(
                                child: new CustomScrollView(
                                    reverse: this.widget.reverse,
                                    physics: new RefreshScrollPhysics(enableOverScroll: this.widget.enableOverScroll),
                                    controller: this._scrollController,
                                    slivers: slivers
                                ),
                                onNotification: this._dispatchScrollEvent
                            )
                        )
                    }
                );
            });
        }
    }


    public abstract class Indicator : StatefulWidget {
        public readonly int mode;

        protected Indicator(
            int mode,
            Key key = null
        ) : base(key) {
            this.mode = mode;
        }
    }


    public class RefreshController {
        public ValueNotifier<int> _headerMode = new ValueNotifier<int>(0);
        public ValueNotifier<int> _footerMode = new ValueNotifier<int>(0);
        public ScrollController scrollController;

        public void requestRefresh(bool up) {
            if (up) {
                if (this._headerMode.value == RefreshStatus.idle) {
                    this._headerMode.value = RefreshStatus.refreshing;
                }
            }
            else {
                if (this._footerMode.value == RefreshStatus.idle) {
                    this._footerMode.value = RefreshStatus.refreshing;
                }
            }
        }

        public void scrollTo(float offset) {
            this.scrollController?.jumpTo(offset);
        }

        public void animateTo(float to, TimeSpan duration, Curve curve) {
            this.scrollController?.animateTo(to, duration, curve);
        }

        public float offset {
            get { return this.scrollController.offset; }
        }

        public void sendBack(bool up, int mode) {
            if (up) {
                this._headerMode.value = mode;
            }
            else {
                this._footerMode.value = mode;
            }
        }

        public bool isRefresh(bool up) {
            if (up) {
                return this._headerMode.value == RefreshStatus.refreshing;
            }
            else {
                return this._footerMode.value == RefreshStatus.refreshing;
            }
        }

        public int headerMode {
            get { return this._headerMode.value; }
        }

        public int footerMode {
            get { return this._footerMode.value; }
        }
    }
}