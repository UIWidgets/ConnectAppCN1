using System;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.Swiper {
    public class WarmPainter : BasePainter {
        public WarmPainter(PageIndicator widget, float page, int index, Paint paint)
            : base(widget, page, index, paint) { }

        public override void draw(Canvas canvas, float space, float size, float radius) {
            float progress = this.page - this.index;
            float distance = size + space;
            float start = this.index * (size + space);

            if (progress > 0.5f) {
                float right = start + size + distance;

                float left = this.index * distance + distance * (progress - 0.5f) * 2;
                canvas.drawRRect(
                    RRect.fromLTRBR(left, 0.0f, right, size, Radius.circular(radius)), this._paint);
            }
            else {
                float right = start + size + distance * progress * 2;

                canvas.drawRRect(
                    RRect.fromLTRBR(start, 0.0f, right, size, Radius.circular(radius)), this._paint);
            }
        }
    }

    public class DropPainter : BasePainter {
        public DropPainter(PageIndicator widget, float page, int index, Paint paint)
            : base(widget, page, index, paint) { }

        public override void draw(Canvas canvas, float space, float size, float radius) {
            float progress = this.page - this.index;
            float dropHeight = this.widget.dropHeight;
            float rate = (0.5f - progress).abs() * 2;
            float scale = this.widget.scale;


            canvas.drawCircle(
                new Offset(radius + ((this.page) * (size + space)),
                    radius - dropHeight * (1 - rate)),
                radius * (scale + rate * (1.0f - scale)), this._paint);
        }
    }

    public class NonePainter : BasePainter {
        public NonePainter(PageIndicator widget, float page, int index, Paint paint)
            : base(widget, page, index, paint) { }

        public override void draw(Canvas canvas, float space, float size, float radius) {
            float progress = this.page - this.index;
            float secondOffset = this.index == this.widget.count - 1
                ? radius
                : radius + ((this.index + 1) * (size + space));

            if (progress > 0.5f) {
                canvas.drawCircle(
                    new Offset(secondOffset, radius),
                    radius, this._paint);
            }
            else {
                canvas.drawCircle(new Offset(radius + (this.index * (size + space)), radius),
                    radius, this._paint);
            }
        }
    }

    public class SlidePainter : BasePainter {
        public SlidePainter(PageIndicator widget, float page, int index, Paint paint)
            : base(widget, page, index, paint) { }

        public override void draw(Canvas canvas, float space, float size, float radius) {
            canvas.drawCircle(
                new Offset(radius + (this.page * (size + space)), radius), radius, this._paint);
        }
    }

    public class ScalePainter : BasePainter {
        public ScalePainter(
            PageIndicator widget, float page, int index, Paint paint)
            : base(widget, page, index, paint) { }


        protected override bool _shouldSkip(int i) {
            if (this.index == this.widget.count - 1) {
                return i == 0 || i == this.index;
            }

            return (i == this.index || i == this.index + 1);
        }

        public override void paint(Canvas canvas, Size _size) {
            this._paint.color = this.widget.color;
            float space = this.widget.space;
            float size = this.widget.size;
            float radius = size / 2;
            for (int i = 0, c = this.widget.count; i < c; ++i) {
                if (this._shouldSkip(i)) {
                    continue;
                }

                canvas.drawCircle(new Offset(i * (size + space) + radius, radius),
                    radius * this.widget.scale, this._paint);
            }


            this._paint.color = this.widget.activeColor;
            this.draw(canvas, space, size, radius);
        }

        public override void draw(Canvas canvas, float space, float size, float radius) {
            float secondOffset = this.index == this.widget.count - 1
                ? radius
                : radius + ((this.index + 1) * (size + space));

            float progress = this.page - this.index;
            this._paint.color = Color.lerp(this.widget.activeColor, this.widget.color, progress);
            canvas.drawCircle(new Offset(radius + (this.index * (size + space)), radius), this.lerp(radius, radius * this.widget.scale, progress), this._paint);
            this._paint.color = Color.lerp(this.widget.color, this.widget.activeColor, progress);
            canvas.drawCircle(
                new Offset(secondOffset, radius), this.lerp(radius * this.widget.scale, radius, progress), this._paint);
        }
    }

    public class ColorPainter : BasePainter {
        public ColorPainter(PageIndicator widget, float page, int index, Paint paint)
            : base(widget, page, index, paint) { }

        protected override bool _shouldSkip(int i) {
            if (this.index == this.widget.count - 1) {
                return i == 0 || i == this.index;
            }

            return (i == this.index || i == this.index + 1);
        }

        public override void draw(Canvas canvas, float space, float size, float radius) {
            float progress = this.page - this.index;
            float secondOffset = this.index == this.widget.count - 1
                ? radius
                : radius + ((this.index + 1) * (size + space));

            this._paint.color = Color.lerp(this.widget.activeColor, this.widget.color, progress);
            canvas.drawCircle(
                new Offset(radius + (this.index * (size + space)), radius), radius, this._paint);
            this._paint.color = Color.lerp(this.widget.color, this.widget.activeColor, progress);
            canvas.drawCircle(
                new Offset(secondOffset, radius),
                radius, this._paint);
        }
    }

    public abstract class BasePainter : AbstractCustomPainter {
        public readonly PageIndicator widget;
        public readonly float page;
        public readonly int index;
        protected readonly Paint _paint;

        public float lerp(float begin, float end, float progress) {
            return begin + (end - begin) * progress;
        }


        public BasePainter(PageIndicator widget, float page, int index, Paint _paint) {
            this.widget = widget;
            this.page = page;
            this.index = index;
            this._paint = _paint;
        }

        public abstract void draw(Canvas canvas, float space, float size, float radius);

        protected virtual bool _shouldSkip(int index) {
            return false;
        }

        public override void paint(Canvas canvas, Size _size) {
            this._paint.color = this.widget.color;
            float space = this.widget.space;
            float size = this.widget.size;
            float radius = size / 2;
            for (int i = 0, c = this.widget.count; i < c; ++i) {
                if (this._shouldSkip(i)) {
                    continue;
                }

                canvas.drawCircle(
                    new Offset(i * (size + space) + radius, radius), radius, this._paint);
            }

            float page = this.page;
            if (page < this.index) {
                page = 0.0f;
            }

            this._paint.color = this.widget.activeColor;
            this.draw(canvas, space, size, radius);
        }

        public override bool shouldRepaint(CustomPainter _oldDelegate) {
            BasePainter oldDelegate = _oldDelegate as BasePainter;
            return oldDelegate.page != this.page;
        }
    }

    class _PageIndicatorState : State<PageIndicator> {
        int index = 0;
        Paint _paint = new Paint();

        BasePainter _createPainer() {
            switch (this.widget.layout) {
                case PageIndicatorLayout.NONE:
                    return new NonePainter(this.widget, this.widget.controller.page, this.index, this._paint);
                case PageIndicatorLayout.SLIDE:
                    return new SlidePainter(this.widget, this.widget.controller.page, this.index, this._paint);
                case PageIndicatorLayout.WARM:
                    return new WarmPainter(this.widget, this.widget.controller.page, this.index, this._paint);
                case PageIndicatorLayout.COLOR:
                    return new ColorPainter(this.widget, this.widget.controller.page, this.index, this._paint);
                case PageIndicatorLayout.SCALE:
                    return new ScalePainter(this.widget, this.widget.controller.page, this.index, this._paint);
                case PageIndicatorLayout.DROP:
                    return new DropPainter(this.widget, this.widget.controller.page, this.index, this._paint);
                default:
                    throw new Exception("Not a valid layout");
            }
        }

        public override Widget build(BuildContext context) {
            Widget child = new SizedBox(
                width: this.widget.count * this.widget.size + (this.widget.count - 1) * this.widget.space,
                height: this.widget.size,
                child: new CustomPaint(
                    painter: this._createPainer()
                )
            );

            if (this.widget.layout == PageIndicatorLayout.SCALE || this.widget.layout == PageIndicatorLayout.COLOR) {
                child = new ClipRect(
                    child: child
                );
            }

            return new IgnorePointer(
                child: child
            );
        }

        void _onController() {
            float page = this.widget.controller.page;
            this.index = page.floor();

            this.setState(() => { });
        }

        public override void initState() {
            this.widget.controller.addListener(this._onController);
            base.initState();
        }

        public override void didUpdateWidget(StatefulWidget _oldWidget) {
            PageIndicator oldWidget = _oldWidget as PageIndicator;
            if (this.widget.controller != oldWidget.controller) {
                oldWidget.controller.removeListener(this._onController);
                this.widget.controller.addListener(this._onController);
            }

            base.didUpdateWidget(oldWidget);
        }

        public override void dispose() {
            this.widget.controller.removeListener(this._onController);
            base.dispose();
        }
    }

    public enum PageIndicatorLayout {
        NONE,
        SLIDE,
        WARM,
        COLOR,
        SCALE,
        DROP
    }

    public class PageIndicator : StatefulWidget {
        public readonly float size;

        public readonly float space;

        public readonly int count;

        public readonly Color activeColor;

        public readonly Color color;

        public readonly PageIndicatorLayout layout;

        public readonly float scale;

        public readonly float dropHeight;

        public readonly PageController controller;

        public readonly float activeSize;

        public PageIndicator(
            Key key = null,
            float size = 20.0f,
            float space = 5.0f,
            int? count = null,
            float activeSize = 20.0f,
            PageController controller = null,
            Color color = null,
            PageIndicatorLayout layout = PageIndicatorLayout.SLIDE,
            Color activeColor = null,
            float scale = 0.6f,
            float dropHeight = 20.0f
            ) : base(key: key) {
            D.assert(count != null);
            D.assert(controller != null);
            this.size = size;
            this.space = space;
            this.count = count.Value;
            this.activeSize = activeSize;
            this.controller = controller;
            this.color = color ?? Colors.white30;
            this.layout = layout;
            this.activeColor = activeColor ?? Colors.white;
            this.scale = scale;
            this.dropHeight = dropHeight;
        }

        public override State createState() {
            return new _PageIndicatorState();
        }
    }
}
