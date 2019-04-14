using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components.pull_to_refresh {
    public class RefreshStatus {
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
            IndicatorBuilder headerBuilder = null,
            IndicatorBuilder footerBuilder = null,
            Config headerConfig = null,
            Config footerConfig = null,
            bool enablePullUp = DefaultConstants.default_enablePullUp,
            bool enablePullDown = DefaultConstants.default_enablePullDown,
            bool enableOverScroll = DefaultConstants.default_enableOverScroll,
            OnRefresh onRefresh = null,
            OnOffsetChange onOffsetChange = null,
            RefreshController controller = null,
            Key key = null
        ) : base(key) {
            this.child = child;
            this.headerBuilder = headerBuilder ?? ((context, mode) => new ClassicIndicator(mode,idleIcon:new CustomActivityIndicator()));
            this.footerBuilder = footerBuilder ?? ((context, mode) => new ClassicIndicator(mode));
            this.headerConfig = headerConfig ?? new RefreshConfig();
            this.footerConfig = footerConfig ?? new LoadConfig();
            this.enablePullUp = enablePullUp;
            this.enablePullDown = enablePullDown;
            this.enableOverScroll = enableOverScroll;
            this.onRefresh = onRefresh;
            this.onOffsetChange = onOffsetChange;
            this.controller = controller ?? new RefreshController();
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

        //controll inner state
        public readonly RefreshController controller;


        public override State createState() {
            return new _SmartRefresherState();
        }
    }

    public class _SmartRefresherState : State<SmartRefresher> {
        private ScrollController _scrollController;
        private readonly GlobalKey _headerKey = GlobalKey.key();
        private readonly GlobalKey _footerKey = GlobalKey.key();
        private float _headerHeight = 0.0f;
        private float _footerHeight = 0.0f;
        private readonly ValueNotifier<float> offsetLis = new ValueNotifier<float>(0.0f);
        private readonly ValueNotifier<int> topModeLis = new ValueNotifier<int>(0);
        private readonly ValueNotifier<int> bottomModeLis = new ValueNotifier<int>(0);

        private bool _handleScrollStart(ScrollStartNotification notification) {
            // This is used to interupt useless callback when the pull up load rolls back.
            if ((notification.metrics.outOfRange())) return false;
            GestureProcessor topWrap = _headerKey.currentState as GestureProcessor;
            GestureProcessor bottomWrap = _footerKey.currentState as GestureProcessor;
            if (widget.enablePullUp) bottomWrap.onDragStart(notification);
            if (widget.enablePullDown) topWrap.onDragStart(notification);
            return false;
        }

        private bool _handleScrollMoving(ScrollUpdateNotification notification) {
            if (_measure(notification) != -1.0)
                offsetLis.value = _measure(notification);
            GestureProcessor topWrap = _headerKey.currentState as GestureProcessor;
            GestureProcessor bottomWrap = _footerKey.currentState as GestureProcessor;
            if (widget.enablePullUp) bottomWrap.onDragMove(notification);
            if (widget.enablePullDown) topWrap.onDragMove(notification);
            return false;
        }

        private bool _handleScrollEnd(ScrollNotification notification) {
            GestureProcessor topWrap = _headerKey.currentState as GestureProcessor;
            GestureProcessor bottomWrap = _footerKey.currentState as GestureProcessor;
            if (widget.enablePullUp) bottomWrap.onDragEnd(notification);
            if (widget.enablePullDown) topWrap.onDragEnd(notification);
            return false;
        }

        private bool _dispatchScrollEvent(ScrollNotification notification) {
            // when is scroll in the ScrollInside,nothing to do
            if ((!_isPullUp(notification) && !_isPullDown(notification))) return false;
            if (notification is ScrollStartNotification) {
                var startNotification = (ScrollStartNotification) notification;
                return _handleScrollStart(startNotification);
            }

            if (notification is ScrollUpdateNotification) {
                var startNotification = (ScrollUpdateNotification) notification;
                //if dragDetails is null,This represents the user's finger out of the screen
                if (startNotification.dragDetails == null)
                    return _handleScrollEnd(notification);
                else if (startNotification.dragDetails != null) return _handleScrollMoving(startNotification);
            }

            if (notification is ScrollEndNotification) _handleScrollEnd(notification);

            return false;
        }

        private static bool _isPullUp(ScrollNotification notification) {
            return notification.metrics.pixels < 0;
        }

        //check user is pulling down
        private static bool _isPullDown(ScrollNotification notification) {
            return notification.metrics.pixels > 0;
        }

        private float _measure(ScrollNotification notification) {
            if (notification.metrics.minScrollExtent - notification.metrics.pixels >
                0)
                return (notification.metrics.minScrollExtent -
                        notification.metrics.pixels) /
                       widget.headerConfig.triggerDistance;
            if (notification.metrics.pixels -
                notification.metrics.maxScrollExtent >
                0)
                return (notification.metrics.pixels -
                        notification.metrics.maxScrollExtent) /
                       widget.footerConfig.triggerDistance;
            return -1.0f;
        }

        private void _init() {
            _scrollController = new ScrollController();
            widget.controller.scrollController = _scrollController;
            SchedulerBinding.instance.addPostFrameCallback(duration => { _onAfterBuild(); });
            _scrollController.addListener(_handleOffsetCallback);
            widget.controller._headerMode = topModeLis;
            widget.controller._footerMode = bottomModeLis;
        }

        private void _handleOffsetCallback() {
            double overscrollPastStart = Math.Max(
                _scrollController.position.minScrollExtent -
                _scrollController.position.pixels + (widget.headerConfig is RefreshConfig &&
                                                     (topModeLis.value == RefreshStatus.refreshing ||
                                                      topModeLis.value == RefreshStatus.completed ||
                                                      topModeLis.value == RefreshStatus.failed)
                    ? (widget.headerConfig as RefreshConfig).visibleRange
                    : 0.0),
                0.0);
            double overscrollPastEnd = Math.Max(
                _scrollController.position.pixels -
                _scrollController.position.maxScrollExtent + (widget.footerConfig is RefreshConfig &&
                                                              (bottomModeLis.value == RefreshStatus.refreshing ||
                                                               bottomModeLis.value == RefreshStatus.completed ||
                                                               bottomModeLis.value == RefreshStatus.failed)
                    ? (widget.footerConfig as RefreshConfig).visibleRange
                    : 0.0),
                0.0);
            if (overscrollPastStart > overscrollPastEnd) {
                if (widget.headerConfig is RefreshConfig) {
                    if (widget.onOffsetChange != null) widget.onOffsetChange(true, overscrollPastStart);
                }
                else {
                    if (widget.onOffsetChange != null) widget.onOffsetChange(true, overscrollPastStart);
                }
            }
            else if (overscrollPastEnd > 0) {
                if (widget.footerConfig is RefreshConfig) {
                    if (widget.onOffsetChange != null) widget.onOffsetChange(false, overscrollPastEnd);
                }
                else {
                    if (widget.onOffsetChange != null) widget.onOffsetChange(false, overscrollPastEnd);
                }
            }
        }

        private void _didChangeMode(bool up, ValueNotifier<int> mode) {
            switch (mode.value) {
                case RefreshStatus.refreshing:
                    if (widget.onRefresh != null) widget.onRefresh(up);
                    if (up && widget.headerConfig is RefreshConfig) {
                        RefreshConfig config = widget.headerConfig as RefreshConfig;
                        _scrollController
                            .jumpTo(_scrollController.offset + config.visibleRange);
                    }

                    break;
            }
        }

        private void _onAfterBuild() {
            if (widget.headerConfig is LoadConfig)
                if ((widget.headerConfig as LoadConfig).bottomWhenBuild)
                    _scrollController.jumpTo(-(_scrollController.position.pixels -
                                               _scrollController.position.maxScrollExtent));

            topModeLis.addListener(() => { _didChangeMode(true, topModeLis); });
            bottomModeLis.addListener(() => { _didChangeMode(false, bottomModeLis); });
            setState(() => {
                if (widget.enablePullDown)
                    _headerHeight = _headerKey.currentContext.size.height;
                if (widget.enablePullUp) _footerHeight = _footerKey.currentContext.size.height;
            });
        }

        private Widget _buildWrapperByConfig(Config config, bool up) {
            if (config is LoadConfig) {
                var loadConfig = (LoadConfig) config;
                return new LoadWrapper(
                    key: up ? _headerKey : _footerKey,
                    modeListener: up ? topModeLis : bottomModeLis,
                    up: up,
                    autoLoad: loadConfig.autoLoad,
                    triggerDistance: loadConfig.triggerDistance,
                    builder: up ? widget.headerBuilder : widget.footerBuilder
                );
            }

            if (config is RefreshConfig) {
                var refreshConfig = (RefreshConfig) config;
                return new RefreshWrapper(
                    key: up ? _headerKey : _footerKey,
                    modeListener: up ? topModeLis : bottomModeLis,
                    up: up,
                    onOffsetChange: (offsetUp, offset) => {
                        if (widget.onOffsetChange != null)
                            widget.onOffsetChange(
                                offsetUp,
                                offsetUp
                                    ? -_scrollController.offset + offset
                                    : _scrollController.position.pixels -
                                      _scrollController.position.maxScrollExtent +
                                      offset);
                    },
                    completeDuration: refreshConfig.completeDuration,
                    triggerDistance: refreshConfig.triggerDistance,
                    visibleRange: refreshConfig.visibleRange,
                    builder: up ? widget.headerBuilder : widget.footerBuilder
                );
            }

            return new Container();
        }

        public override void dispose() {
            _scrollController.removeListener(_handleOffsetCallback);
            _scrollController.dispose();
            base.dispose();
        }

        public override void initState() {
            base.initState();
            _init();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            widget.controller._headerMode = topModeLis;
            widget.controller._footerMode = bottomModeLis;
            widget.controller.scrollController = _scrollController;
            base.didUpdateWidget(oldWidget);
        }

        public override Widget build(BuildContext context) {
            var type = typeof(ScrollView);
            var method = type.GetMethod("buildSlivers", BindingFlags.NonPublic | BindingFlags.Instance);
            var objs = new object[1];
            objs[0] = context;
            var slivers = (List<Widget>) method.Invoke(widget.child, objs);
            slivers.Add(new SliverToBoxAdapter(
                child: widget.footerBuilder != null && widget.enablePullUp
                    ? _buildWrapperByConfig(widget.footerConfig, false)
                    : new Container()
            ));
            slivers.Insert(
                0,
                new SliverToBoxAdapter(
                    child: widget.headerBuilder != null && widget.enablePullDown
                        ? _buildWrapperByConfig(widget.headerConfig, true)
                        : new Container()));
            return new LayoutBuilder(builder: (cxt, cons) => {
                return new Stack(
                    children: new List<Widget> {
                        new Positioned(
                            top: !widget.enablePullDown || widget.headerConfig is LoadConfig
                                ? 0.0f
                                : -_headerHeight,
                            bottom: !widget.enablePullUp || widget.footerConfig is LoadConfig
                                ? 0.0f
                                : -_footerHeight,
                            left: 0.0f,
                            right: 0.0f,
                            child: new NotificationListener<ScrollNotification>(
                                child: new CustomScrollView(
                                    physics: new RefreshScrollPhysics(enableOverScroll: widget.enableOverScroll),
                                    controller: _scrollController,
                                    slivers: slivers
                                ),
                                onNotification: _dispatchScrollEvent
                            )
                        )
                    }
                );
            });
        }
    }


    public abstract class Indicator : StatefulWidget {
        public readonly int mode;

        public Indicator(
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
                if (_headerMode.value == RefreshStatus.idle)
                    _headerMode.value = RefreshStatus.refreshing;
            }
            else {
                if (_footerMode.value == RefreshStatus.idle) _footerMode.value = RefreshStatus.refreshing;
            }
        }

        public void scrollTo(float offset) {
            scrollController.jumpTo(offset);
        }

        public void sendBack(bool up, int mode) {
            if (up)
                _headerMode.value = mode;
            else
                _footerMode.value = mode;
        }

        public bool isRefresh(bool up) {
            if (up)
                return _headerMode.value == RefreshStatus.refreshing;
            else
                return _footerMode.value == RefreshStatus.refreshing;
        }

        public int headerMode => _headerMode.value;

        public int footerMode => _footerMode.value;
    }
}