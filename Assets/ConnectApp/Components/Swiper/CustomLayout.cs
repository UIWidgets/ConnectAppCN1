using System;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Transform = Unity.UIWidgets.widgets.Transform;

namespace ConnectApp.Components.Swiper {
    static class CustomLayoutUtils {
        public static float _getValue(List<float> values, float animationValue, int index) {
            float s = values[index: index];
            if (animationValue >= 0.5f) {
                if (index < values.Count - 1) {
                    s = s + (values[index + 1] - s) * (animationValue - 0.5f) * 2.0f;
                }
            }
            else {
                if (index != 0) {
                    s = s - (s - values[index - 1]) * (0.5f - animationValue) * 2.0f;
                }
            }

            return s;
        }

        public static Offset _getOffsetValue(List<Offset> values, float animationValue, int index) {
            Offset s = values[index: index];
            float dx = s.dx;
            float dy = s.dy;
            if (animationValue >= 0.5f) {
                if (index < values.Count - 1) {
                    dx = dx + (values[index + 1].dx - dx) * (animationValue - 0.5f) * 2.0f;
                    dy = dy + (values[index + 1].dy - dy) * (animationValue - 0.5f) * 2.0f;
                }
            }
            else {
                if (index != 0) {
                    dx = dx - (dx - values[index - 1].dx) * (0.5f - animationValue) * 2.0f;
                    dy = dy - (dy - values[index - 1].dy) * (0.5f - animationValue) * 2.0f;
                }
            }

            return new Offset(dx: dx, dy: dy);
        }
    }

    abstract class _CustomLayoutStateBase<T> : SingleTickerProviderStateMixin<T> where T : _SubSwiper {
        protected float _swiperWidth;
        protected float _swiperHeight;
        Animation<float> _animation;
        AnimationController _animationController;
        internal int _startIndex;
        internal int _animationCount;

        public override void initState() {
            if (this.widget.itemWidth == null) {
                throw new Exception(
                    "==============\n\nwidget.itemWith must not be null when use stack layout.\n========\n");
            }

            this._createAnimationController();
            this.widget.controller.addListener(listener: this._onController);
            base.initState();
        }

        void _createAnimationController() {
            this._animationController = new AnimationController(vsync: this, value: 0.5f);
            FloatTween tween = new FloatTween(0.0f, 1.0f);
            this._animation = tween.animate(parent: this._animationController);
        }

        public override void didChangeDependencies() {
            WidgetsBinding.instance.addPostFrameCallback(callback: this._getSize);
            base.didChangeDependencies();
        }

        void _getSize(TimeSpan duration) {
            this.afterRender();
        }

        public virtual void afterRender() {
            RenderObject renderObject = this.context.findRenderObject();
            Size size = renderObject.paintBounds.size;
            this._swiperWidth = size.width;
            this._swiperHeight = size.height;
            this.setState(() => { });
        }

        public override void didUpdateWidget(StatefulWidget _oldWidget) {
            T oldWidget = _oldWidget as T;
            if (this.widget.controller != oldWidget.controller) {
                oldWidget.controller.removeListener(listener: this._onController);
                this.widget.controller.addListener(listener: this._onController);
            }

            if (this.widget.loop != oldWidget.loop) {
                if (!this.widget.loop == true) {
                    this._currentIndex = this._ensureIndex(index: this._currentIndex);
                }
            }

            base.didUpdateWidget(oldWidget: oldWidget);
        }

        int _ensureIndex(int index) {
            index = index % this.widget.itemCount.Value;
            if (index < 0) {
                index += this.widget.itemCount.Value;
            }

            return index;
        }

        public override void dispose() {
            this.widget.controller.removeListener(listener: this._onController);
            this._animationController?.dispose();
            base.dispose();
        }

        protected abstract Widget _buildItem(int i, int realIndex, float animationValue);

        Widget _buildContainer(List<Widget> list) {
            return new Stack(
                children: list
            );
        }

        Widget _buildAnimation(BuildContext context, Widget w) {
            List<Widget> list = new List<Widget>();

            float animationValue = this._animation.value;

            for (int i = 0; i < this._animationCount; ++i) {
                int realIndex = this._currentIndex + i + this._startIndex;
                realIndex = realIndex % this.widget.itemCount.Value;
                if (realIndex < 0) {
                    realIndex += this.widget.itemCount.Value;
                }

                list.Add(this._buildItem(i: i, realIndex: realIndex, animationValue: animationValue));
            }

            return new GestureDetector(
                behavior: HitTestBehavior.opaque,
                onPanStart: this._onPanStart,
                onPanEnd: this._onPanEnd,
                onPanUpdate: this._onPanUpdate,
                child: new ClipRect(
                    child: new Center(
                        child: this._buildContainer(list: list)
                    )
                )
            );
        }

        public override Widget build(BuildContext context) {
            return new AnimatedBuilder(
                animation: this._animationController, builder: this._buildAnimation);
        }

        float _currentValue;
        float _currentPos;

        bool _lockScroll = false;

        void _move(float position, int? nextIndex = null) {
            if (this._lockScroll) {
                return;
            }

            try {
                this._lockScroll = true;
                this._animationController.animateTo(
                    target: position,
                    duration: TimeSpan.FromMilliseconds(this.widget.duration ?? 0),
                    curve: this.widget.curve
                );
                if (nextIndex != null) {
                    this.widget.onIndexChanged(this.widget.getCorrectIndex(indexNeedsFix: nextIndex.Value));
                }
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
            finally {
                if (nextIndex != null) {
                    try {
                        this._animationController.setValue(0.5f);
                    }
                    catch (Exception e) {
                        Debug.LogError(e);
                    }

                    this._currentIndex = nextIndex.Value;
                }

                this._lockScroll = false;
            }
        }

        int _nextIndex() {
            int index = this._currentIndex + 1;
            if (this.widget.loop != true && index >= this.widget.itemCount - 1) {
                return this.widget.itemCount.Value - 1;
            }

            return index;
        }

        int _prevIndex() {
            int index = this._currentIndex - 1;
            if (this.widget.loop != true && index < 0) {
                return 0;
            }

            return index;
        }

        void _onController() {
            switch (this.widget.controller.evt) {
                case IndexController.PREVIOUS:
                    int prevIndex = this._prevIndex();
                    if (prevIndex == this._currentIndex) {
                        return;
                    }

                    this._move(1.0f, nextIndex: prevIndex);
                    break;
                case IndexController.NEXT:
                    int nextIndex = this._nextIndex();
                    if (nextIndex == this._currentIndex) {
                        return;
                    }

                    this._move(0.0f, nextIndex: nextIndex);
                    break;
                case IndexController.MOVE:
                    throw new Exception(
                        "Custom layout does not support SwiperControllerEvent.MOVE_INDEX yet!");
                case SwiperController.STOP_AUTOPLAY:
                case SwiperController.START_AUTOPLAY:
                    break;
            }
        }

        void _onPanEnd(DragEndDetails details) {
            if (this._lockScroll) {
                return;
            }

            float velocity = this.widget.scrollDirection == Axis.horizontal
                ? details.velocity.pixelsPerSecond.dx
                : details.velocity.pixelsPerSecond.dy;

            if (this._animationController.value >= 0.75f || velocity > 500.0f) {
                if (this._currentIndex <= 0 && this.widget.loop != true) {
                    return;
                }

                this._move(1.0f, nextIndex: this._currentIndex - 1);
            }
            else if (this._animationController.value < 0.25f || velocity < -500.0f) {
                if (this._currentIndex >= this.widget.itemCount - 1 && this.widget.loop != true) {
                    return;
                }

                this._move(0.0f, nextIndex: this._currentIndex + 1);
            }
            else {
                this._move(0.5f);
            }
        }

        void _onPanStart(DragStartDetails details) {
            if (this._lockScroll) {
                return;
            }

            this._currentValue = this._animationController.value;
            this._currentPos = this.widget.scrollDirection == Axis.horizontal
                ? details.globalPosition.dx
                : details.globalPosition.dy;
        }

        void _onPanUpdate(DragUpdateDetails details) {
            if (this._lockScroll) {
                return;
            }

            float value = this._currentValue +
                          ((this.widget.scrollDirection == Axis.horizontal
                               ? details.globalPosition.dx
                               : details.globalPosition.dy) - this._currentPos) / this._swiperWidth /
                          2;
            if (this.widget.loop != true) {
                if (this._currentIndex >= this.widget.itemCount - 1) {
                    if (value < 0.5f) {
                        value = 0.5f;
                    }
                }
                else if (this._currentIndex <= 0) {
                    if (value > 0.5f) {
                        value = 0.5f;
                    }
                }
            }

            this._animationController.setValue(newValue: value);
        }

        protected int _currentIndex = 0;
    }

    public abstract class TransformBuilder {
        public abstract Widget build(int i, float animationValue, Widget widget);
    }

    public abstract class TransformBuilder<T> : TransformBuilder {
        public readonly List<T> values;

        protected TransformBuilder(List<T> values) {
            this.values = values;
        }
    }

    public class ScaleTransformBuilder : TransformBuilder<float> {
        public readonly Alignment alignment;

        public ScaleTransformBuilder(List<float> values = null, Alignment alignment = null)
            : base(values: values) {
            this.alignment = alignment ?? Alignment.center;
        }

        public override Widget build(int i, float animationValue, Widget widget) {
            float s = CustomLayoutUtils._getValue(values: this.values, animationValue: animationValue, index: i);
            return Transform.scale(scale: s, child: widget);
        }
    }

    public class OpacityTransformBuilder : TransformBuilder<float> {
        public OpacityTransformBuilder(List<float> values) : base(values: values) { }

        public override Widget build(int i, float animationValue, Widget widget) {
            float v = CustomLayoutUtils._getValue(values: this.values, animationValue: animationValue, index: i);
            return new Opacity(
                opacity: v,
                child: widget
            );
        }
    }

    public class RotateTransformBuilder : TransformBuilder<float> {
        public RotateTransformBuilder(List<float> values) : base(values: values) { }

        public override Widget build(int i, float animationValue, Widget widget) {
            float v = CustomLayoutUtils._getValue(values: this.values, animationValue: animationValue, index: i);
            return Transform.rotate(
                degree: v,
                child: widget
            );
        }
    }

    public class TranslateTransformBuilder : TransformBuilder<Offset> {
        public TranslateTransformBuilder(List<Offset> values) : base(values: values) { }

        public override Widget build(int i, float animationValue, Widget widget) {
            Offset s = CustomLayoutUtils._getOffsetValue(values: this.values, animationValue: animationValue, index: i);
            return Transform.translate(
                offset: s,
                child: widget
            );
        }
    }

    public class CustomLayoutOption {
        public readonly List<TransformBuilder> builders = new List<TransformBuilder> ();
        public readonly int startIndex;
        public readonly int stateCount;

        CustomLayoutOption(int stateCount, int startIndex) {
            this.startIndex = startIndex;
            this.stateCount = stateCount;
        }

        CustomLayoutOption addOpacity(List<float> values) {
            this.builders.Add(new OpacityTransformBuilder(values: values));
            return this;
        }

        CustomLayoutOption addTranslate(List<Offset> values) {
            this.builders.Add(new TranslateTransformBuilder(values: values));
            return this;
        }

        CustomLayoutOption addScale(List<float> values, Alignment alignment) {
            this.builders
                .Add(new ScaleTransformBuilder(values: values, alignment: alignment));
            return this;
        }

        CustomLayoutOption addRotate(List<float> values) {
            this.builders.Add(new RotateTransformBuilder(values: values));
            return this;
        }
    }

    class _CustomLayoutSwiper : _SubSwiper {
        public readonly CustomLayoutOption option;

        public _CustomLayoutSwiper(
            CustomLayoutOption option,
            float? itemWidth = null,
            bool? loop = null,
            float? itemHeight = null,
            ValueChanged<int> onIndexChanged = null,
            Key key = null,
            IndexedWidgetBuilder itemBuilder = null,
            Curve curve = null,
            int? duration = null,
            int? index = null,
            int? itemCount = null,
            Axis scrollDirection = Axis.horizontal,
            SwiperController controller = null
        ) : base(
            loop: loop,
            onIndexChanged: onIndexChanged,
            itemWidth: itemWidth,
            itemHeight: itemHeight,
            key: key,
            itemBuilder: itemBuilder,
            curve: curve,
            duration: duration,
            index: index,
            itemCount: itemCount,
            controller: controller,
            scrollDirection: scrollDirection) {
            D.assert(option != null);
            this.option = option;
        }

        public override State createState() {
            return new _CustomLayoutState();
        }
    }

    class _CustomLayoutState : _CustomLayoutStateBase<_CustomLayoutSwiper> {
        public override void didChangeDependencies() {
            base.didChangeDependencies();
            this._startIndex = this.widget.option.startIndex;
            this._animationCount = this.widget.option.stateCount;
        }

        public override void didUpdateWidget(StatefulWidget _oldWidget) {
            _CustomLayoutSwiper oldWidget = _oldWidget as _CustomLayoutSwiper;
            this._startIndex = this.widget.option.startIndex;
            this._animationCount = this.widget.option.stateCount;
            base.didUpdateWidget(_oldWidget: oldWidget);
        }

        protected override Widget _buildItem(int index, int realIndex, float animationValue) {
            List<TransformBuilder> builders = this.widget.option.builders;

            Widget child = new SizedBox(
                width: this.widget.itemWidth ?? float.PositiveInfinity,
                height: this.widget.itemHeight ?? float.PositiveInfinity,
                child: this.widget.itemBuilder(context: this.context, index: realIndex));

            for (int i = builders.Count - 1; i >= 0; --i) {
                TransformBuilder builder = builders[i];
                child = builder.build(i: index, animationValue: animationValue, widget: child);
            }

            return child;
        }
    }
}