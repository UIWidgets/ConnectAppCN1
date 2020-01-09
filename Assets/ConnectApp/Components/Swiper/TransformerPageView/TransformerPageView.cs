using System;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.Swiper {
    static class TransformerPageViewUtils {
        public const int kMaxValue = 20000;
        public const int kMiddleValue = 10000;
        public const int kDefaultTransactionDuration = 300;
    }

    public class TransformInfo {
        public readonly float? width;

        public readonly float? height;

        public readonly float? position;

        public readonly int? index;

        public readonly int? activeIndex;

        public readonly int? fromIndex;

        public readonly bool? forward;

        public readonly bool? done;

        public readonly float? viewportFraction;

        public readonly Axis? scrollDirection;

        public TransformInfo(
            int? index = null,
            float? position = null,
            float? width = null,
            float? height = null,
            int? activeIndex = null,
            int? fromIndex = null,
            bool? forward = null,
            bool? done = null,
            float? viewportFraction = null,
            Axis? scrollDirection = null
        ) {
            this.index = index;
            this.position = position;
            this.width = width;
            this.height = height;
            this.activeIndex = activeIndex;
            this.fromIndex = fromIndex;
            this.forward = forward;
            this.done = done;
            this.viewportFraction = viewportFraction;
            this.scrollDirection = scrollDirection;
        }
    }

    public abstract class PageTransformer {
        public readonly bool reverse;

        protected PageTransformer(bool reverse = false) {
            this.reverse = reverse;
        }

        public abstract Widget transform(Widget child, TransformInfo info);
    }

    public delegate Widget PageTransformerBuilderCallback(Widget child, TransformInfo info);

    public class PageTransformerBuilder : PageTransformer {
        public readonly PageTransformerBuilderCallback builder;

        public PageTransformerBuilder(bool reverse = false, PageTransformerBuilderCallback builder = null)
            : base(reverse: reverse) {
            D.assert(builder != null);
            this.builder = builder;
        }

        public override Widget transform(Widget child, TransformInfo info) {
            return this.builder(child: child, info: info);
        }
    }

    public class TransformerPageController : PageController {
        public readonly bool loop;
        public readonly int itemCount;
        public readonly bool reverse;

        public TransformerPageController(
            int? initialPage = 0,
            bool keepPage = true,
            float viewportFraction = 1.0f,
            bool loop = false,
            int? itemCount = null,
            bool reverse = false
        ) : base(
            _getRealIndexFromRenderIndex(initialPage ?? 0, loop: loop, itemCount: itemCount, reverse: reverse),
            keepPage: keepPage,
            viewportFraction: viewportFraction
        ) {
            D.assert(itemCount != null);
            this.loop = loop;
            this.itemCount = itemCount.Value;
            this.reverse = reverse;
        }

        public int getRenderIndexFromRealIndex(int index) {
            return _getRenderIndexFromRealIndex(index, this.loop, this.itemCount, this.reverse);
        }

        public int getRealItemCount() {
            if (this.itemCount == 0) {
                return 0;
            }

            return this.loop ? this.itemCount + TransformerPageViewUtils.kMaxValue : this.itemCount;
        }

        static int _getRenderIndexFromRealIndex(int index, bool loop, int itemCount, bool reverse) {
            if (itemCount == 0) {
                return 0;
            }

            int renderIndex;
            if (loop) {
                renderIndex = index - TransformerPageViewUtils.kMiddleValue;
                renderIndex = renderIndex % itemCount;
                if (renderIndex < 0) {
                    renderIndex += itemCount;
                }
            }
            else {
                renderIndex = index;
            }

            if (reverse) {
                renderIndex = itemCount - renderIndex - 1;
            }

            return renderIndex;
        }

        public float realPage {
            get {
                float page;
                if (!this.position.hasMaxScrollExtent || !this.position.hasMinScrollExtent) {
                    page = 0.0f;
                }
                else {
                    page = base.page;
                }

                return page;
            }
        }

        static float _getRenderPageFromRealPage(float page, bool loop, int itemCount, bool reverse) {
            float renderPage;
            if (loop) {
                renderPage = page - TransformerPageViewUtils.kMiddleValue;
                renderPage = renderPage % itemCount;
                if (renderPage < 0) {
                    renderPage += itemCount;
                }
            }
            else {
                renderPage = page;
            }

            if (reverse) {
                renderPage = itemCount - renderPage - 1;
            }

            return renderPage;
        }

        public override float page {
            get {
                return this.loop
                    ? _getRenderPageFromRealPage(this.realPage, this.loop, this.itemCount, this.reverse)
                    : this.realPage;
            }
        }

        public int getRealIndexFromRenderIndex(int index) {
            return _getRealIndexFromRenderIndex(index, this.loop, this.itemCount, this.reverse);
        }

        static int _getRealIndexFromRenderIndex(
            int index, bool loop, int? itemCount, bool reverse) {
            int result = reverse ? (itemCount.Value - index - 1) : index;
            if (loop) {
                result += TransformerPageViewUtils.kMiddleValue;
            }

            return result;
        }
    }

    public class TransformerPageView : StatefulWidget {
        public readonly PageTransformer transformer;

        public readonly Axis scrollDirection;

        public readonly ScrollPhysics physics;

        public readonly bool pageSnapping;

        public readonly ValueChanged<int> onPageChanged;

        public readonly IndexedWidgetBuilder itemBuilder;

        public readonly IndexController controller;

        public readonly TimeSpan? duration;

        public readonly Curve curve;

        public readonly TransformerPageController pageController;

        public readonly bool loop;

        public readonly int itemCount;

        public readonly float viewportFraction;

        public readonly int? index;

        public TransformerPageView(
            Key key = null,
            int? index = null,
            TimeSpan? duration = null,
            Curve curve = null,
            float viewportFraction = 1.0f,
            bool loop = false,
            Axis scrollDirection = Axis.horizontal,
            ScrollPhysics physics = null,
            bool pageSnapping = true,
            ValueChanged<int> onPageChanged = null,
            IndexController controller = null,
            PageTransformer transformer = null,
            IndexedWidgetBuilder itemBuilder = null,
            TransformerPageController pageController = null,
            int? itemCount = null
        ) : base(key: key) {
            D.assert(itemCount != null);
            D.assert(itemCount == 0 || itemBuilder != null || transformer != null);
            this.duration =
                duration ?? TimeSpan.FromMilliseconds(TransformerPageViewUtils.kDefaultTransactionDuration);
            this.index = index;
            this.duration = duration;
            this.curve = curve ?? Curves.ease;
            this.viewportFraction = viewportFraction;
            this.loop = loop;
            this.scrollDirection = scrollDirection;
            this.physics = physics;
            this.pageSnapping = pageSnapping;
            this.onPageChanged = onPageChanged;
            this.controller = controller;
            this.transformer = transformer;
            this.itemBuilder = itemBuilder;
            this.pageController = pageController;
            this.itemCount = itemCount.Value;
        }

        public static TransformerPageView children(
            Key key = null,
            int? index = null,
            TimeSpan? duration = null,
            Curve curve = null,
            float viewportFraction = 1.0f,
            bool loop = false,
            Axis scrollDirection = Axis.horizontal,
            ScrollPhysics physics = null,
            bool pageSnapping = true,
            ValueChanged<int> onPageChanged = null,
            IndexController controller = null,
            PageTransformer transformer = null,
            List<Widget> children = null,
            TransformerPageController pageController = null
        ) {
            D.assert(children != null);
            return new TransformerPageView(
                itemCount: children.Count,
                itemBuilder: (context, _index) => children[index: _index],
                pageController: pageController,
                transformer: transformer,
                pageSnapping: pageSnapping,
                key: key,
                index: index,
                duration: duration,
                curve: curve,
                viewportFraction: viewportFraction,
                loop: loop,
                scrollDirection: scrollDirection,
                physics: physics,
                onPageChanged: onPageChanged,
                controller: controller
            );
        }

        public override State createState() {
            return new _TransformerPageViewState();
        }

        public static int getRealIndexFromRenderIndex(bool reverse, int index, int itemCount, bool loop) {
            int initPage = reverse ? (itemCount - index - 1) : index;
            if (loop) {
                initPage += TransformerPageViewUtils.kMiddleValue;
            }

            return initPage;
        }

        public static PageController createPageController(
            bool reverse,
            int index,
            int itemCount,
            bool loop,
            float viewportFraction
        ) {
            return new PageController(
                initialPage: getRealIndexFromRenderIndex(
                    reverse: reverse, index: index, itemCount: itemCount, loop: loop),
                viewportFraction: viewportFraction);
        }
    }

    class _TransformerPageViewState : State<TransformerPageView> {
        Size _size;
        int _activeIndex;
        float _currentPixels;
        bool _done = false;

        int _fromIndex;

        PageTransformer _transformer;

        TransformerPageController _pageController;

        Widget _buildItemNormal(BuildContext context, int index) {
            int renderIndex = this._pageController.getRenderIndexFromRealIndex(index);
            Widget child = this.widget.itemBuilder(context, renderIndex);
            return child;
        }

        Widget _buildItem(BuildContext context, int index) {
            return new AnimatedBuilder(
                animation: this._pageController,
                builder: (BuildContext c, Widget w) => {
                    int renderIndex = this._pageController.getRenderIndexFromRealIndex(index);
                    Widget child = null;
                    if (this.widget.itemBuilder != null) {
                        child = this.widget.itemBuilder(context, renderIndex);
                    }

                    if (child == null) {
                        child = new Container();
                    }

                    if (this._size == null) {
                        return child ?? new Container();
                    }

                    float position;

                    float page = this._pageController.realPage;

                    if (this._transformer.reverse) {
                        position = page - index;
                    }
                    else {
                        position = index - page;
                    }

                    position *= this.widget.viewportFraction;

                    TransformInfo info = new TransformInfo(
                        index: renderIndex,
                        width: this._size.width,
                        height: this._size.height,
                        position: position.clamp(-1.0f, 1.0f),
                        activeIndex: this._pageController.getRenderIndexFromRealIndex(this._activeIndex),
                        fromIndex: this._fromIndex,
                        forward: this._pageController.position.pixels - this._currentPixels >= 0,
                        done: this._done,
                        scrollDirection: this.widget.scrollDirection,
                        viewportFraction: this.widget.viewportFraction);
                    return this._transformer.transform(child, info);
                });
        }

        float _calcCurrentPixels() {
            this._currentPixels = this._pageController.getRenderIndexFromRealIndex(this._activeIndex) *
                                  this._pageController.position.viewportDimension * this.widget.viewportFraction;


            return this._currentPixels;
        }

        public override Widget build(BuildContext context) {
            IndexedWidgetBuilder builder = this._transformer == null
                ? (IndexedWidgetBuilder) this._buildItemNormal
                : this._buildItem;
            Widget child = new PageView(
                itemBuilder: builder,
                itemCount: this._pageController.getRealItemCount(),
                onPageChanged: this._onIndexChanged,
                controller: this._pageController,
                scrollDirection: this.widget.scrollDirection,
                physics: this.widget.physics,
                pageSnapping: this.widget.pageSnapping,
                reverse: this._pageController.reverse
            );
            if (this._transformer == null) {
                return child;
            }

            return new NotificationListener<ScrollNotification>(
                onNotification: notification => {
                    if (notification is ScrollStartNotification) {
                        this._calcCurrentPixels();
                        this._done = false;
                        this._fromIndex = this._activeIndex;
                    }
                    else if (notification is ScrollEndNotification) {
                        this._calcCurrentPixels();
                        this._fromIndex = this._activeIndex;
                        this._done = true;
                    }

                    return false;
                },
                child: child
            );
        }

        void _onIndexChanged(int index) {
            this._activeIndex = index;
            this.widget.onPageChanged?.Invoke(this._pageController.getRenderIndexFromRealIndex(index));
        }

        void _onGetSize(TimeSpan _) {
            Size size = null;
            if (this.context == null) {
                this.onGetSize(size);
                return;
            }

            RenderObject renderObject = this.context.findRenderObject();
            if (renderObject != null) {
                Rect bounds = renderObject.paintBounds;
                if (bounds != null) {
                    size = bounds.size;
                }
            }

            this._calcCurrentPixels();
            this.onGetSize(size);
        }

        void onGetSize(Size size) {
            if (this.mounted) {
                this.setState(() => { this._size = size; });
            }
        }

        public override void initState() {
            this._transformer = this.widget.transformer;
            this._pageController = this.widget.pageController;
            if (this._pageController == null) {
                this._pageController = new TransformerPageController(
                    initialPage: this.widget.index,
                    itemCount: this.widget.itemCount,
                    loop: this.widget.loop,
                    reverse: this.widget.transformer?.reverse ?? false);
            }

            this._fromIndex = this._activeIndex = this._pageController.initialPage;

            this._controller = this.getNotifier();
            this._controller?.addListener(this.onChangeNotifier);

            base.initState();
        }

        public override void didUpdateWidget(StatefulWidget _oldWidget) {
            TransformerPageView oldWidget = _oldWidget as TransformerPageView;
            this._transformer = this.widget.transformer;
            int index = this.widget.index ?? 0;
            bool created = false;
            if (this._pageController != this.widget.pageController) {
                if (this.widget.pageController != null) {
                    this._pageController = this.widget.pageController;
                }
                else {
                    created = true;
                    this._pageController = new TransformerPageController(
                        initialPage: this.widget.index,
                        itemCount: this.widget.itemCount,
                        loop: this.widget.loop,
                        reverse: this.widget.transformer?.reverse ?? false);
                }
            }

            if (this._pageController.getRenderIndexFromRealIndex(this._activeIndex) != index) {
                this._fromIndex = this._activeIndex = this._pageController.initialPage;
                if (!created) {
                    int initPage = this._pageController.getRealIndexFromRenderIndex(index);
                    this._pageController.animateToPage(initPage,
                        duration: this.widget.duration.Value, curve: this.widget.curve);
                }
            }

            if (this._transformer != null) {
                WidgetsBinding.instance.addPostFrameCallback(this._onGetSize);
            }

            if (this._controller != this.getNotifier()) {
                this._controller?.removeListener(this.onChangeNotifier);

                this._controller = this.getNotifier();
                this._controller?.addListener(this.onChangeNotifier);
            }

            base.didUpdateWidget(oldWidget);
        }

        public override void didChangeDependencies() {
            if (this._transformer != null) {
                WidgetsBinding.instance.addPostFrameCallback(this._onGetSize);
            }

            base.didChangeDependencies();
        }

        ChangeNotifier getNotifier() {
            return this.widget.controller;
        }

        int _calcNextIndex(bool next) {
            int currentIndex = this._activeIndex;
            if (this._pageController.reverse) {
                if (next) {
                    currentIndex--;
                }
                else {
                    currentIndex++;
                }
            }
            else {
                if (next) {
                    currentIndex++;
                }
                else {
                    currentIndex--;
                }
            }

            if (!this._pageController.loop) {
                if (currentIndex >= this._pageController.itemCount) {
                    currentIndex = 0;
                }
                else if (currentIndex < 0) {
                    currentIndex = this._pageController.itemCount - 1;
                }
            }

            return currentIndex;
        }

        void onChangeNotifier() {
            int evt = this.widget.controller.evt;
            int index;
            switch (evt) {
                case IndexController.MOVE: {
                    index = this._pageController
                        .getRealIndexFromRenderIndex(this.widget.controller.index);
                }
                    break;
                case IndexController.PREVIOUS:
                case IndexController.NEXT: {
                    index = this._calcNextIndex(evt == IndexController.NEXT);
                }
                    break;
                default:
                    return;
            }

            if (this.widget.controller.animation == true) {
                this._pageController
                    .animateToPage(index,
                        duration: this.widget.duration.Value, curve: this.widget.curve ?? Curves.ease)
                    .Finally(this.widget.controller.complete);
            }
            else {
                this._pageController.jumpToPage(index);
                this.widget.controller.complete();
            }
        }

        ChangeNotifier _controller;

        public override void dispose() {
            base.dispose();
            this._controller?.removeListener(this.onChangeNotifier);
        }
    }
}