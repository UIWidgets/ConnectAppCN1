using System;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.Swiper {
    static class SwiperUtils {
        public const int kDefaultAutoplayDelayMs = 3000;
        public const int kDefaultAutoplayTransactionDuration = 300;
        public const int kMaxValue = 2000000000;
        public const int kMiddleValue = 1000000000;
    }

    public delegate void SwiperOnTap(int index);

    public delegate Widget SwiperDataBuilder(BuildContext context, object data, int index);

    public enum SwiperLayout {
        normal,
        stack,
        tinder,
        custom
    }

    public class Swiper : StatefulWidget {
        public Swiper(
            IndexedWidgetBuilder itemBuilder = null,
            PageIndicatorLayout indicatorLayout = PageIndicatorLayout.none,
            PageTransformer transformer = null,
            int? itemCount = null,
            bool autoplay = false,
            SwiperLayout layout = SwiperLayout.normal,
            int autoplayDelay = SwiperUtils.kDefaultAutoplayDelayMs,
            bool autoplayDisableOnInteraction = true,
            int duration = SwiperUtils.kDefaultAutoplayTransactionDuration,
            ValueChanged<int> onIndexChanged = null,
            int? index = null,
            SwiperOnTap onTap = null,
            SwiperPlugin control = null,
            bool loop = true,
            Curve curve = null,
            Axis scrollDirection = Axis.horizontal,
            SwiperPlugin pagination = null,
            List<SwiperPlugin> plugins = null,
            ScrollPhysics physics = null,
            Key key = null,
            SwiperController controller = null,
            CustomLayoutOption customLayoutOption = null,
            float? containerHeight = null,
            float? containerWidth = null,
            float viewportFraction = 1.0f,
            float? itemHeight = null,
            float? itemWidth = null,
            bool? outer = false,
            float? scale = null,
            float? fade = null
        ) : base(key: key) {
            D.assert(itemBuilder != null || transformer != null,
                () => "itemBuilder and transformItemBuilder must not be both null");
            D.assert(!loop || ((loop && layout == SwiperLayout.normal &&
                                (indicatorLayout == PageIndicatorLayout.scale ||
                                 indicatorLayout == PageIndicatorLayout.color ||
                                 indicatorLayout == PageIndicatorLayout.none)) ||
                               (loop && layout != SwiperLayout.normal)),
                () => "Only support `PageIndicatorLayout.SCALE` and `PageIndicatorLayout.COLOR`when layout==SwiperLayout.DEFAULT in loop mode"
            );
            this.itemBuilder = itemBuilder;
            this.indicatorLayout = indicatorLayout;
            this.transformer = transformer;
            this.itemCount = itemCount;
            this.autoplay = autoplay;
            this.layout = layout;
            this.autoplayDelay = autoplayDelay;
            this.autoplayDisableOnInteraction = autoplayDisableOnInteraction;
            this.duration = duration;
            this.onIndexChanged = onIndexChanged;
            this.index = index;
            this.onTap = onTap;
            this.control = control;
            this.loop = loop;
            this.curve = curve ?? Curves.ease;
            this.scrollDirection = scrollDirection;
            this.pagination = pagination;
            this.plugins = plugins;
            this.physics = physics;
            this.controller = controller;
            this.customLayoutOption = customLayoutOption;
            this.containerHeight = containerHeight;
            this.containerWidth = containerWidth;
            this.viewportFraction = viewportFraction;
            this.itemHeight = itemHeight;
            this.itemWidth = itemWidth;
            this.outer = outer;
            this.scale = scale;
            this.fade = fade;
        }

        public readonly bool? outer;
        public readonly float? itemHeight;
        public readonly float? itemWidth;
        public readonly float? containerHeight;
        public readonly float? containerWidth;
        public readonly IndexedWidgetBuilder itemBuilder;
        public readonly PageTransformer transformer;
        public readonly int? itemCount;
        public readonly ValueChanged<int> onIndexChanged;
        public readonly bool autoplay;
        public readonly int autoplayDelay;
        public readonly bool autoplayDisableOnInteraction;
        public readonly int duration;
        public readonly Axis scrollDirection;
        public readonly Curve curve;
        public readonly bool loop;
        public readonly int? index;
        public readonly SwiperOnTap onTap;
        public readonly SwiperPlugin pagination;
        public readonly SwiperPlugin control;
        public readonly List<SwiperPlugin> plugins;
        public readonly SwiperController controller;
        public readonly ScrollPhysics physics;
        public readonly float viewportFraction;
        public readonly SwiperLayout layout;
        public readonly CustomLayoutOption customLayoutOption;
        public readonly float? scale;
        public readonly float? fade;
        public readonly PageIndicatorLayout indicatorLayout;


        public override State createState() {
            return new _SwiperState();
        }
    }

    abstract class _SwiperTimerMixin : State<Swiper> {
        internal Timer _timer;

        internal SwiperController _controller;

        public override void initState() {
            this._controller = this.widget.controller ?? new SwiperController();

            this._controller.addListener(listener: this._onController);
            this._handleAutoplay();
            base.initState();
        }

        void _onController() {
            switch (this._controller.evt) {
                case SwiperController.START_AUTOPLAY: {
                    if (this._timer == null) {
                        this._startAutoplay();
                    }
                }
                    break;
                case SwiperController.STOP_AUTOPLAY: {
                    if (this._timer != null) {
                        this._stopAutoplay();
                    }
                }
                    break;
            }
        }

        public override void didUpdateWidget(StatefulWidget _oldWidget) {
            Swiper oldWidget = _oldWidget as Swiper;
            if (this._controller != oldWidget.controller) {
                if (oldWidget.controller != null) {
                    oldWidget.controller.removeListener(this._onController);
                    this._controller = oldWidget.controller;
                    this._controller.addListener(this._onController);
                }
            }

            this._handleAutoplay();
            base.didUpdateWidget(oldWidget);
        }

        public override void dispose() {
            this._controller?.removeListener(this._onController);

            this._stopAutoplay();
            base.dispose();
        }

        bool _autoplayEnabled() {
            return this._controller.autoplay ?? this.widget.autoplay;
        }

        void _handleAutoplay() {
            if (this._autoplayEnabled() && this._timer != null) {
                return;
            }

            this._stopAutoplay();
            if (this._autoplayEnabled()) {
                this._startAutoplay();
            }
        }

        protected void _startAutoplay() {
            D.assert(this._timer == null, () => "Timer must be stopped before start!");
            this._timer =
                Window.instance.periodic(TimeSpan.FromMilliseconds(this.widget.autoplayDelay), this._onTimer);
        }

        void _onTimer() {
            this._controller.next();
        }

        protected void _stopAutoplay() {
            if (this._timer != null) {
                this._timer.cancel();
                this._timer = null;
            }
        }
    }

    class _SwiperState : _SwiperTimerMixin {
        int _activeIndex;

        TransformerPageController _pageController;

        Widget _wrapTap(BuildContext context, int index) {
            return new GestureDetector(
                behavior: HitTestBehavior.opaque,
                onTap: () => { this.widget.onTap(index); },
                child: this.widget.itemBuilder(context, index)
            );
        }

        public override void initState() {
            this._activeIndex = this.widget.index ?? 0;
            if (this._isPageViewLayout()) {
                this._pageController = new TransformerPageController(
                    initialPage: this.widget.index,
                    loop: this.widget.loop,
                    itemCount: this.widget.itemCount,
                    reverse: this.widget.transformer == null ? false : this.widget.transformer.reverse,
                    viewportFraction: this.widget.viewportFraction);
            }

            base.initState();
        }

        bool _isPageViewLayout() {
            return this.widget.layout == SwiperLayout.normal;
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
        }

        bool _getReverse(Swiper widget) {
            return widget.transformer == null ? false : widget.transformer.reverse;
        }

        public override void didUpdateWidget(StatefulWidget _oldWidget) {
            Swiper oldWidget = _oldWidget as Swiper;
            base.didUpdateWidget(oldWidget);
            if (this._isPageViewLayout()) {
                if (this._pageController == null ||
                    (this.widget.index != oldWidget.index || this.widget.loop != oldWidget.loop ||
                     this.widget.itemCount != oldWidget.itemCount ||
                     this.widget.viewportFraction != oldWidget.viewportFraction ||
                     this._getReverse(this.widget) != this._getReverse(oldWidget))) {
                    this._pageController = new TransformerPageController(
                        initialPage: this.widget.index,
                        loop: this.widget.loop,
                        itemCount: this.widget.itemCount,
                        reverse: this._getReverse(this.widget),
                        viewportFraction: this.widget.viewportFraction);
                }
            }
            else {
                Window.instance.scheduleMicrotask(() => {
                    if (this._pageController != null) {
                        this._pageController.dispose();
                        this._pageController = null;
                    }
                });
            }

            if (this.widget.index != null && this.widget.index != this._activeIndex) {
                this._activeIndex = this.widget.index.Value;
            }
        }

        void _onIndexChanged(int index) {
            this.setState(() => { this._activeIndex = index; });
            if (this.widget.onIndexChanged != null) {
                this.widget.onIndexChanged(index);
            }
        }

        Widget _buildSwiper() {
            IndexedWidgetBuilder itemBuilder = this.widget.onTap != null ? this._wrapTap : this.widget.itemBuilder;

            if (this.widget.layout == SwiperLayout.stack) {
                return new _StackSwiper(
                    loop: this.widget.loop,
                    itemWidth: this.widget.itemWidth,
                    itemHeight: this.widget.itemHeight,
                    itemCount: this.widget.itemCount,
                    itemBuilder: itemBuilder,
                    index: this._activeIndex,
                    curve: this.widget.curve,
                    duration: this.widget.duration,
                    onIndexChanged: this._onIndexChanged,
                    controller: this._controller,
                    scrollDirection: this.widget.scrollDirection
                );
            }
            else if (this._isPageViewLayout()) {
                PageTransformer transformer = this.widget.transformer;
                if (this.widget.scale != null || this.widget.fade != null) {
                    transformer = new ScaleAndFadeTransformer(scale: this.widget.scale, fade: this.widget.fade);
                }

                Widget child = new TransformerPageView(
                    pageController: this._pageController,
                    loop: this.widget.loop,
                    itemCount: this.widget.itemCount,
                    itemBuilder: itemBuilder,
                    transformer: transformer,
                    viewportFraction: this.widget.viewportFraction,
                    index: this._activeIndex,
                    duration: TimeSpan.FromMilliseconds(this.widget.duration),
                    scrollDirection: this.widget.scrollDirection,
                    onPageChanged: this._onIndexChanged,
                    curve: this.widget.curve,
                    physics: this.widget.physics,
                    controller: this._controller
                );
                if (this.widget.autoplayDisableOnInteraction && this.widget.autoplay) {
                    return new NotificationListener<ScrollNotification>(
                        child: child,
                        onNotification: notification => {
                            if (notification is ScrollStartNotification scrollStartNotification) {
                                if (scrollStartNotification.dragDetails != null) {
                                    if (this._timer != null) {
                                        this._stopAutoplay();
                                    }
                                }
                            }
                            else if (notification is ScrollEndNotification) {
                                if (this._timer == null) {
                                    this._startAutoplay();
                                }
                            }

                            return false;
                        }
                    );
                }

                return child;
            }
            else if (this.widget.layout == SwiperLayout.tinder) {
                return new _TinderSwiper(
                    loop: this.widget.loop,
                    itemWidth: this.widget.itemWidth,
                    itemHeight: this.widget.itemHeight,
                    itemCount: this.widget.itemCount,
                    itemBuilder: itemBuilder,
                    index: this._activeIndex,
                    curve: this.widget.curve,
                    duration: this.widget.duration,
                    onIndexChanged: this._onIndexChanged,
                    controller: this._controller,
                    scrollDirection: this.widget.scrollDirection
                );
            }
            else if (this.widget.layout == SwiperLayout.custom) {
                return new _CustomLayoutSwiper(
                    loop: this.widget.loop,
                    option: this.widget.customLayoutOption,
                    itemWidth: this.widget.itemWidth,
                    itemHeight: this.widget.itemHeight,
                    itemCount: this.widget.itemCount,
                    itemBuilder: itemBuilder,
                    index: this._activeIndex,
                    curve: this.widget.curve,
                    duration: this.widget.duration,
                    onIndexChanged: this._onIndexChanged,
                    controller: this._controller,
                    scrollDirection: this.widget.scrollDirection
                );
            }
            else {
                return new Container();
            }
        }

        SwiperPluginConfig _ensureConfig(SwiperPluginConfig config) {
            if (config == null) {
                config = new SwiperPluginConfig(
                    outer: this.widget.outer,
                    itemCount: this.widget.itemCount,
                    layout: this.widget.layout,
                    indicatorLayout: this.widget.indicatorLayout,
                    pageController: this._pageController,
                    activeIndex: this._activeIndex,
                    scrollDirection: this.widget.scrollDirection,
                    controller: this._controller,
                    loop: this.widget.loop);
            }

            return config;
        }

        List<Widget> _ensureListForStack(Widget swiper, List<Widget> listForStack, Widget widget) {
            if (listForStack == null) {
                listForStack = new List<Widget> {swiper, widget};
            }
            else {
                listForStack.Add(widget);
            }

            return listForStack;
        }

        public override Widget build(BuildContext context) {
            Widget swiper = this._buildSwiper();
            List<Widget> listForStack = null;
            SwiperPluginConfig config = null;
            if (this.widget.control != null) {
                config = this._ensureConfig(config);
                listForStack = this._ensureListForStack(
                    swiper, listForStack, this.widget.control.build(context, config));
            }

//            if (this.widget.plugins != null) {
//                config = this._ensureConfig(config);
//                foreach (SwiperPlugin plugin in this.widget.plugins) {
//                    listForStack = this._ensureListForStack(
//                        swiper, listForStack, plugin.build(context, config));
//                }
//            }

            if (this.widget.pagination != null) {
                config = this._ensureConfig(config);
                if (this.widget.outer == true) {
                    return this._buildOuterPagination((SwiperPagination) this.widget.pagination,
                        listForStack == null ? swiper : new Stack(children: listForStack),
                        config);
                }
                else {
                    listForStack = this._ensureListForStack(
                        swiper, listForStack, this.widget.pagination.build(context, config));
                }
            }

            if (listForStack != null) {
                return new Stack(
                    children: listForStack
                );
            }

            return swiper;
        }

        Widget _buildOuterPagination(
            SwiperPagination pagination, Widget swiper, SwiperPluginConfig config) {
            List<Widget> list = new List<Widget> { };
            if (this.widget.containerHeight != null || this.widget.containerWidth != null) {
                list.Add(swiper);
            }
            else {
                list.Add(new Expanded(child: swiper));
            }

            list.Add(new Align(
                alignment: Alignment.center,
                child: pagination.build(this.context, config)
            ));

            return new Column(
                children: list,
                crossAxisAlignment: CrossAxisAlignment.stretch,
                mainAxisSize: MainAxisSize.min
            );
        }
    }

    abstract class _SubSwiper : StatefulWidget {
        public readonly IndexedWidgetBuilder itemBuilder;
        public readonly int? itemCount;
        public readonly int? index;
        public readonly ValueChanged<int> onIndexChanged;
        public readonly SwiperController controller;
        public readonly int? duration;
        public readonly Curve curve;
        public readonly float? itemWidth;
        public readonly float? itemHeight;
        public readonly bool? loop;
        public readonly Axis scrollDirection;

        public _SubSwiper(
            Key key = null,
            bool? loop = null,
            float? itemHeight = null,
            float? itemWidth = null,
            int? duration = null,
            Curve curve = null,
            IndexedWidgetBuilder itemBuilder = null,
            SwiperController controller = null,
            int? index = null,
            int? itemCount = null,
            Axis scrollDirection = Axis.horizontal,
            ValueChanged<int> onIndexChanged = null
        ) : base(key: key) {
            this.loop = loop;
            this.itemHeight = itemHeight;
            this.itemWidth = itemWidth;
            this.duration = duration;
            this.curve = curve;
            this.itemBuilder = itemBuilder;
            this.controller = controller;
            this.index = index;
            this.itemCount = itemCount;
            this.scrollDirection = scrollDirection;
            this.onIndexChanged = onIndexChanged;
        }

        public abstract override State createState();

        public int getCorrectIndex(int indexNeedsFix) {
            if (this.itemCount == 0 || this.itemCount == null) {
                return 0;
            }

            int value = indexNeedsFix % this.itemCount.Value;
            if (value < 0) {
                value += this.itemCount.Value;
            }

            return value;
        }
    }

    class _TinderSwiper : _SubSwiper {
        public _TinderSwiper(
            Key key = null,
            Curve curve = null,
            int? duration = null,
            SwiperController controller = null,
            ValueChanged<int> onIndexChanged = null,
            float? itemHeight = null,
            float? itemWidth = null,
            IndexedWidgetBuilder itemBuilder = null,
            int? index = null,
            bool? loop = null,
            int? itemCount = null,
            Axis scrollDirection = Axis.horizontal
        ) : base(loop: loop,
            key: key,
            itemWidth: itemWidth,
            itemHeight: itemHeight,
            itemBuilder: itemBuilder,
            curve: curve,
            duration: duration,
            controller: controller,
            index: index,
            onIndexChanged: onIndexChanged,
            itemCount: itemCount,
            scrollDirection: scrollDirection) {
            D.assert(itemWidth != null && itemHeight != null);
        }

        public override State createState() {
            return new _TinderState();
        }
    }

    class _StackSwiper : _SubSwiper {
        public _StackSwiper(
            Key key = null,
            Curve curve = null,
            int? duration = null,
            SwiperController controller = null,
            ValueChanged<int> onIndexChanged = null,
            float? itemHeight = null,
            float? itemWidth = null,
            IndexedWidgetBuilder itemBuilder = null,
            int? index = null,
            bool? loop = null,
            int? itemCount = null,
            Axis scrollDirection = Axis.horizontal
        ) : base(
            loop: loop,
            key: key,
            itemWidth: itemWidth,
            itemHeight: itemHeight,
            itemBuilder: itemBuilder,
            curve: curve,
            duration: duration,
            controller: controller,
            index: index,
            onIndexChanged: onIndexChanged,
            itemCount: itemCount,
            scrollDirection: scrollDirection) { }

        public override State createState() {
            return new _StackViewState();
        }
    }

    class _TinderState : _CustomLayoutStateBase<_TinderSwiper> {
        List<float> scales;
        List<float> offsetsX;
        List<float> offsetsY;
        List<float> opacity;
        List<float> rotates;

        float? getOffsetY(float scale) {
            return this.widget.itemHeight - this.widget.itemHeight * scale;
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
        }

        public override void didUpdateWidget(StatefulWidget _oldWidget) {
            _TinderSwiper oldWidget = _oldWidget as _TinderSwiper;
            this._updateValues();
            base.didUpdateWidget(oldWidget);
        }

        public override void afterRender() {
            base.afterRender();

            this._startIndex = -3;
            this._animationCount = 5;
            this.opacity = new List<float> {0.0f, 0.9f, 0.9f, 1.0f, 0.0f, 0.0f};
            this.scales = new List<float> {0.80f, 0.80f, 0.85f, 0.90f, 1.0f, 1.0f, 1.0f};
            this.rotates = new List<float> {0.0f, 0.0f, 0.0f, 0.0f, 20.0f, 25.0f};
            this._updateValues();
        }

        void _updateValues() {
            if (this.widget.scrollDirection == Axis.horizontal) {
                this.offsetsX = new List<float> {0.0f, 0.0f, 0.0f, 0.0f, this._swiperWidth, this._swiperWidth};
                this.offsetsY = new List<float> {
                    0.0f,
                    0.0f,
                    -5.0f,
                    -10.0f,
                    -15.0f,
                    -20.0f
                };
            }
            else {
                this.offsetsX = new List<float> {
                    0.0f,
                    0.0f,
                    5.0f,
                    10.0f,
                    15.0f,
                    20.0f
                };

                this.offsetsY = new List<float> {0.0f, 0.0f, 0.0f, 0.0f, this._swiperHeight, this._swiperHeight};
            }
        }

        protected override Widget _buildItem(int i, int realIndex, float animationValue) {
            float s = CustomLayoutUtils._getValue(this.scales, animationValue, i);
            float f = CustomLayoutUtils._getValue(this.offsetsX, animationValue, i);
            float fy = CustomLayoutUtils._getValue(this.offsetsY, animationValue, i);
            float o = CustomLayoutUtils._getValue(this.opacity, animationValue, i);
            float a = CustomLayoutUtils._getValue(this.rotates, animationValue, i);

            Alignment alignment = this.widget.scrollDirection == Axis.horizontal
                ? Alignment.bottomCenter
                : Alignment.centerLeft;

            return new Opacity(
                opacity: o,
                child: Transform.rotate(
                    degree: a / 180.0f,
                    child: Transform.translate(
                        key: new ValueKey<int>(this._currentIndex + i),
                        offset: new Offset(f, fy),
                        child: Transform.scale(
                            scale: s,
                            alignment: alignment,
                            child: new SizedBox(
                                width: this.widget.itemWidth ?? float.PositiveInfinity,
                                height: this.widget.itemHeight ?? float.PositiveInfinity,
                                child: this.widget.itemBuilder(this.context, realIndex)
                            )
                        )
                    )
                )
            );
        }
    }

    class _StackViewState : _CustomLayoutStateBase<_StackSwiper> {
        public List<float> scales;
        public List<float> offsets;
        public List<float> opacity;

        public override void didChangeDependencies() {
            base.didChangeDependencies();
        }

        void _updateValues() {
            if (this.widget.scrollDirection == Axis.horizontal) {
                float space = ((this._swiperWidth - this.widget.itemWidth) / 2) ?? 0;
                this.offsets = new List<float> {-space, -space / 3 * 2, -space / 3, 0.0f, this._swiperWidth};
            }
            else {
                float space = ((this._swiperHeight - this.widget.itemHeight) / 2) ?? 0;
                this.offsets = new List<float> {-space, -space / 3 * 2, -space / 3, 0.0f, this._swiperHeight};
            }
        }

        public override void didUpdateWidget(StatefulWidget _oldWidget) {
            _StackSwiper oldWidget = _oldWidget as _StackSwiper;
            this._updateValues();
            base.didUpdateWidget(oldWidget);
        }

        public override void afterRender() {
            base.afterRender();

            this._animationCount = 5;

            this._startIndex = -3;
            this.scales = new List<float> {0.7f, 0.8f, 0.9f, 1.0f, 1.0f};
            this.opacity = new List<float> {0.0f, 0.5f, 1.0f, 1.0f, 1.0f};

            this._updateValues();
        }

        protected override Widget _buildItem(int i, int realIndex, float animationValue) {
            float s = CustomLayoutUtils._getValue(this.scales, animationValue, i);
            float f = CustomLayoutUtils._getValue(this.offsets, animationValue, i);
            float o = CustomLayoutUtils._getValue(this.opacity, animationValue, i);

            Offset offset = this.widget.scrollDirection == Axis.horizontal
                ? new Offset(f, 0.0f)
                : new Offset(0.0f, f);

            Alignment alignment = this.widget.scrollDirection == Axis.horizontal
                ? Alignment.centerLeft
                : Alignment.topCenter;

            return new Opacity(
                opacity: o,
                child: Transform.translate(
                    key: new ValueKey<int>(this._currentIndex + i),
                    offset: offset,
                    child: Transform.scale(
                        scale: s,
                        alignment: alignment,
                        child: new SizedBox(
                            width: this.widget.itemWidth ?? float.PositiveInfinity,
                            height: this.widget.itemHeight ?? float.PositiveInfinity,
                            child: this.widget.itemBuilder(this.context, realIndex)
                        )
                    )
                )
            );
        }
    }

    public class ScaleAndFadeTransformer : PageTransformer {
        public readonly float? _scale;
        public readonly float? _fade;

        public ScaleAndFadeTransformer(
            float? fade = 0.3f,
            float? scale = 0.8f
        ) {
            this._fade = fade;
            this._scale = scale;
        }

        public override Widget transform(Widget item, TransformInfo info) {
            float position = info.position.Value;
            Widget child = item;
            if (this._scale != null) {
                float scaleFactor = (1 - position.abs()) * (1 - this._scale.Value);
                float scale = this._scale.Value + scaleFactor;

                child = Transform.scale(
                    scale: scale,
                    child: item
                );
            }

            if (this._fade != null) {
                float fadeFactor = (1 - position.abs()) * (1 - this._fade.Value);
                float opacity = this._fade.Value + fadeFactor;
                child = new Opacity(
                    opacity: opacity,
                    child: child
                );
            }

            return child;
        }
    }
}