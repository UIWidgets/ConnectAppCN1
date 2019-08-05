using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Components.pull_to_refresh {
    
    public class CenteredRefresher : StatefulWidget {
        public CenteredRefresher(
            List<Widget> children,
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
            NotificationListenerCallback<ScrollNotification> onNotification = null,
            Key key = null,
            int centerIndex = 0
        ) : base(key) {
            this.children = children;
            this.headerBuilder =
                headerBuilder ?? ((context, mode) => new SmartRefreshHeader(mode, RefreshHeaderType.other));
            this.footerBuilder = footerBuilder ?? ((context, mode) => new SmartRefreshFooter(mode));
            this.headerConfig = headerConfig ?? new RefreshConfig();
            this.footerConfig = footerConfig ?? new LoadConfig(triggerDistance: 0);
            this.enablePullUp = enablePullUp;
            this.enablePullDown = enablePullDown;
            this.enableOverScroll = enableOverScroll;
            this.onRefresh = onRefresh;
            this.onOffsetChange = onOffsetChange;
            this.controller = controller ?? new RefreshController();
            this.onNotification = onNotification;
            this.centerIndex = centerIndex;
        }

        public readonly List<Widget> children;

        public readonly int centerIndex;

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

        public readonly NotificationListenerCallback<ScrollNotification> onNotification;


        public override State createState() {
            return new _CenteredRefresherState();
        }
    }

    public class _CenteredRefresherState : State<CenteredRefresher> {
        ScrollController _scrollController;
        readonly GlobalKey _headerKey = GlobalKey.key();
        readonly GlobalKey _footerKey = GlobalKey.key();
        float _headerHeight = DefaultConstants.default_VisibleRange;
        float _footerHeight = DefaultConstants.default_VisibleRange;
        readonly ValueNotifier<float> offsetLis = new ValueNotifier<float>(0.0f);
        readonly ValueNotifier<int> topModeLis = new ValueNotifier<int>(0);
        readonly ValueNotifier<int> bottomModeLis = new ValueNotifier<int>(0);
        
        readonly GlobalKey _selectedKey = GlobalKey.key("selectedKey");

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
            if (this.widget.onNotification != null) {
                this.widget.onNotification(notification);
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
            this._scrollController = new ScrollController();
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
            var children = this.widget.children;
            var slivers = new List<Widget>();

            int id = 0;
            foreach (var child in children) {
                Widget sliver = new SliverList(
                    del: new SliverChildBuilderDelegate((BuildContext Subcontext, int index1) => {
                            return child;
                        },
                        childCount: 1),
                    key: id == this.widget.centerIndex ? this._selectedKey : null);
                
                slivers.Add(sliver);
                id++;
            }

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
                                    physics: new RefreshScrollPhysics(enableOverScroll: false),
                                    controller: this._scrollController,
                                    slivers: slivers,
                                    center: this._selectedKey
                                ),
                                onNotification: this._dispatchScrollEvent
                            )
                        )
                    }
                );
            });
        }
    }
}