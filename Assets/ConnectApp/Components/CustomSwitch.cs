using System;
using ConnectApp.Constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    static class CustomSwitchUtils {
        public const float _kTrackWidth = 50.0f;
        public const float _kTrackHeight = 28.0f;
        public const float _kTrackRadius = _kTrackHeight / 2.0f;
        public const float _kThumbRadius = 12.0f;
        public const float _kTrackInnerStart = _kTrackHeight / 2.0f;
        public const float _kTrackInnerEnd = _kTrackWidth - _kTrackInnerStart;
        public const float _kTrackInnerLength = _kTrackInnerEnd - _kTrackInnerStart;
        public const float _kSwitchWidth = 51.0f;
        public const float _kSwitchHeight = 31.0f;
        public const float _kCustomSwitchDisabledOpacity = 0.5f;
        public static readonly Color _kTrackColor = CColors.Separator;
        public static readonly TimeSpan _kReactionDuration = new TimeSpan(0, 0, 0, 0, 300);
        public static readonly TimeSpan _kToggleDuration = new TimeSpan(0, 0, 0, 0, 200);
    }

    public class CustomSwitch : StatefulWidget {
        public CustomSwitch(
            bool value,
            ValueChanged<bool> onChanged,
            Key key = null,
            Color activeColor = null,
            DragStartBehavior dragStartBehavior = DragStartBehavior.start
        ) : base(key: key) {
            this.value = value;
            this.onChanged = onChanged;
            this.activeColor = activeColor;
            this.dragStartBehavior = dragStartBehavior;
        }

        public readonly bool value;

        public readonly ValueChanged<bool> onChanged;

        public readonly Color activeColor;

        public readonly DragStartBehavior dragStartBehavior;

        public override State createState() {
            return new _CustomSwitchState();
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new FlagProperty("value", value: this.value, ifTrue: "on", ifFalse: "off", showName: true));
            properties.add(new ObjectFlagProperty<ValueChanged<bool>>("onChanged", this.onChanged, ifNull: "disabled"));
        }
    }

    class _CustomSwitchState : TickerProviderStateMixin<CustomSwitch> {
        public override Widget build(BuildContext context) {
            return new Opacity(
                opacity: this.widget.onChanged == null ? CustomSwitchUtils._kCustomSwitchDisabledOpacity : 1.0f,
                child: new _CustomSwitchRenderObjectWidget(
                    value: this.widget.value,
                    activeColor: this.widget.activeColor ?? CColors.PrimaryBlue,
                    onChanged: this.widget.onChanged,
                    vsync: this,
                    dragStartBehavior: this.widget.dragStartBehavior
                )
            );
        }
    }

    class _CustomSwitchRenderObjectWidget : LeafRenderObjectWidget {
        public _CustomSwitchRenderObjectWidget(
            Key key = null,
            bool value = false,
            Color activeColor = null,
            ValueChanged<bool> onChanged = null,
            TickerProvider vsync = null,
            DragStartBehavior dragStartBehavior = DragStartBehavior.start
        ) : base(key: key) {
            this.value = value;
            this.activeColor = activeColor;
            this.onChanged = onChanged;
            this.vsync = vsync;
            this.dragStartBehavior = dragStartBehavior;
        }

        readonly bool value;
        readonly Color activeColor;
        readonly ValueChanged<bool> onChanged;
        readonly TickerProvider vsync;
        readonly DragStartBehavior dragStartBehavior;
        
        public override RenderObject createRenderObject(BuildContext context) {
            return new _RenderCustomSwitch(
                value: this.value,
                activeColor: this.activeColor,
                onChanged: this.onChanged,
                textDirection: Directionality.of(context),
                vsync: this.vsync,
                dragStartBehavior: this.dragStartBehavior
            );
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObject) {
            var _renderObject = renderObject as _RenderCustomSwitch;
            _renderObject.value = this.value;
            _renderObject.activeColor = this.activeColor;
            _renderObject.onChanged = this.onChanged;
            _renderObject.textDirection = Directionality.of(context);
            _renderObject.vsync = this.vsync;
            _renderObject.dragStartBehavior = this.dragStartBehavior;
        }
    }

    class _RenderCustomSwitch : RenderConstrainedBox {
        public _RenderCustomSwitch(
            bool value,
            Color activeColor,
            TextDirection textDirection,
            TickerProvider vsync,
            ValueChanged<bool> onChanged = null,
            DragStartBehavior dragStartBehavior = DragStartBehavior.start
        ) : base(additionalConstraints: BoxConstraints.tightFor(
            width: CustomSwitchUtils._kSwitchWidth,
            height: CustomSwitchUtils._kSwitchHeight)
        ) {
            D.assert(activeColor != null);
            D.assert(vsync != null);
            this._value = value;
            this._activeColor = activeColor;
            this._onChanged = onChanged;
            this._textDirection = textDirection;
            this._vsync = vsync;

            this._tap = new TapGestureRecognizer() {
                onTapDown = this._handleTapDown,
                onTap = this._handleTap,
                onTapUp = this._handleTapUp,
                onTapCancel = this._handleTapCancel,
            };

            this._drag = new HorizontalDragGestureRecognizer() {
                onStart = this._handleDragStart,
                onUpdate = this._handleDragUpdate,
                onEnd = this._handleDragEnd,
                dragStartBehavior = dragStartBehavior
            };

            this._positionController = new AnimationController(
                duration: CustomSwitchUtils._kToggleDuration,
                value: value ? 1.0f : 0.0f,
                vsync: vsync
            );
            this._position = new CurvedAnimation(
                parent: this._positionController,
                curve: Curves.linear
            );
            this._position.addListener(this.markNeedsPaint);
            this._position.addStatusListener(this._handlePositionStateChanged);

            this._reactionController = new AnimationController(
                duration: CustomSwitchUtils._kReactionDuration,
                vsync: vsync
            );
            this._reaction = new CurvedAnimation(
                parent: this._reactionController,
                curve: Curves.ease
            );
            this._reaction.addListener(this.markNeedsPaint);
        }

        readonly AnimationController _positionController;
        readonly CurvedAnimation _position;
        readonly AnimationController _reactionController;
        Animation<float> _reaction;

        public bool value {
            get { return this._value; }
            set {
                if (value == this._value) {
                    return;
                }

                this._value = value;
                // this.markNeedsSemanticsUpdate();
                this._position.curve = Curves.ease;
                this._position.reverseCurve = Curves.ease.flipped;
                if (value) {
                    this._positionController.forward();
                }
                else {
                    this._positionController.reverse();
                }
            }
        }

        bool _value;

        public TickerProvider vsync {
            get { return this._vsync; }
            set {
                D.assert(value != null);
                if (value == this._vsync) {
                    return;
                }

                this._vsync = value;
                this._positionController.resync(this.vsync);
                this._reactionController.resync(this.vsync);
            }
        }

        TickerProvider _vsync;

        public Color activeColor {
            get { return this._activeColor; }
            set {
                D.assert(value != null);
                if (value == this._activeColor) {
                    return;
                }

                this._activeColor = value;
                this.markNeedsPaint();
            }
        }

        Color _activeColor;

        public ValueChanged<bool> onChanged {
            get { return this._onChanged; }
            set {
                if (value == this._onChanged) {
                    return;
                }

                bool wasInteractive = this.isInteractive;
                this._onChanged = value;
                if (wasInteractive != this.isInteractive) {
                    this.markNeedsPaint();
                }
            }
        }

        ValueChanged<bool> _onChanged;

        public TextDirection textDirection {
            get { return this._textDirection; }
            set {
                if (this._textDirection == value) {
                    return;
                }

                this._textDirection = value;
                this.markNeedsPaint();
            }
        }

        TextDirection _textDirection;

        public DragStartBehavior dragStartBehavior {
            get { return this._drag.dragStartBehavior; }
            set {
                if (this._drag.dragStartBehavior == value) {
                    return;
                }

                this._drag.dragStartBehavior = value;
            }
        }

        public bool isInteractive {
            get { return this.onChanged != null; }
        }

        readonly TapGestureRecognizer _tap;
        readonly HorizontalDragGestureRecognizer _drag;

        public override void attach(object _owner) {
            base.attach(_owner);
            if (this.value) {
                this._positionController.forward();
            }
            else {
                this._positionController.reverse();
            }

            if (this.isInteractive) {
                switch (this._reactionController.status) {
                    case AnimationStatus.forward:
                        this._reactionController.forward();
                        break;
                    case AnimationStatus.reverse:
                        this._reactionController.reverse();
                        break;
                    case AnimationStatus.dismissed:
                    case AnimationStatus.completed:
                        break;
                }
            }
        }

        public override void detach() {
            this._positionController.stop();
            this._reactionController.stop();
            base.detach();
        }

        void _handlePositionStateChanged(AnimationStatus status) {
            if (this.isInteractive) {
                if (status == AnimationStatus.completed && !this._value) {
                    this.onChanged(true);
                }
                else if (status == AnimationStatus.dismissed && this._value) {
                    this.onChanged(false);
                }
            }
        }

        void _handleTapDown(TapDownDetails details) {
            if (this.isInteractive) {
                this._reactionController.forward();
            }
        }

        void _handleTap() {
            if (this.isInteractive) {
                this.onChanged(!this._value);
                this._emitVibration();
            }
        }

        void _handleTapUp(TapUpDetails details) {
            if (this.isInteractive) {
                this._reactionController.reverse();
            }
        }

        void _handleTapCancel() {
            if (this.isInteractive) {
                this._reactionController.reverse();
            }
        }

        void _handleDragStart(DragStartDetails details) {
            if (this.isInteractive) {
                this._reactionController.forward();
                this._emitVibration();
            }
        }

        void _handleDragUpdate(DragUpdateDetails details) {
            if (this.isInteractive) {
                this._position.curve = null;
                this._position.reverseCurve = null;
                float delta = details.primaryDelta / CustomSwitchUtils._kTrackInnerLength ?? 0f;

                this._positionController.setValue(this._positionController.value + delta);

                // switch (this.textDirection) {
                //     case TextDirection.rtl:
                //         this._positionController.setValue(this._positionController.value - delta);
                //         break;
                //     case TextDirection.ltr:
                //         this._positionController.setValue(this._positionController.value + delta);
                //         break;
                // }
            }
        }

        void _handleDragEnd(DragEndDetails details) {
            if (this._position.value >= 0.5) {
                this._positionController.forward();
            }
            else {
                this._positionController.reverse();
            }

            this._reactionController.reverse();
        }

        void _emitVibration() {
            // switch (Platform defaultTargetPlatform) {
            //     case TargetPlatform.iOS:
            //         HapticFeedback.lightImpact();
            //         break;
            //     case TargetPlatform.fuchsia:
            //     case TargetPlatform.android:
            //         break;
            // }
            return;
        }

        protected override bool hitTestSelf(Offset position) {
            return true;
        }

        public override void handleEvent(PointerEvent evt, HitTestEntry entry) {
            D.assert(this.debugHandleEvent(evt, entry));
            if (evt is PointerDownEvent && this.isInteractive) {
                this._drag.addPointer(evt as PointerDownEvent);
                this._tap.addPointer(evt as PointerDownEvent);
            }
        }
        
        readonly CustomThumbPainter _thumbPainter = new CustomThumbPainter(shadowColor: CColors.Transparent);

        public override void paint(PaintingContext context, Offset offset) {
            Canvas canvas = context.canvas;

            float currentValue = this._position.value;

            float visualPosition = 0f;
            switch (this.textDirection) {
                case TextDirection.rtl:
                    visualPosition = 1.0f - currentValue;
                    break;
                case TextDirection.ltr:
                    visualPosition = currentValue;
                    break;
            }

            Color trackColor = this._value ? this.activeColor : CustomSwitchUtils._kTrackColor;

            Paint paint = new Paint {
                color = trackColor
            };

            Rect trackRect = Rect.fromLTWH(
                offset.dx + (this.size.width - CustomSwitchUtils._kTrackWidth) / 2.0f,
                offset.dy + (this.size.height - CustomSwitchUtils._kTrackHeight) / 2.0f,
                CustomSwitchUtils._kTrackWidth,
                CustomSwitchUtils._kTrackHeight
            );
            RRect outerRRect = RRect.fromRectAndRadius(trackRect, Radius.circular(CustomSwitchUtils
                ._kTrackRadius));
            canvas.drawRRect(outerRRect, paint);

            float thumbLeft = MathUtils.lerpFloat(
                trackRect.left + CustomSwitchUtils._kTrackInnerStart - CustomSwitchUtils._kThumbRadius,
                trackRect.left + CustomSwitchUtils._kTrackInnerEnd - CustomSwitchUtils._kThumbRadius,
                visualPosition
            );
            float thumbRight = MathUtils.lerpFloat(
                trackRect.left + CustomSwitchUtils._kTrackInnerStart + CustomSwitchUtils._kThumbRadius,
                trackRect.left + CustomSwitchUtils._kTrackInnerEnd + CustomSwitchUtils._kThumbRadius,
                visualPosition
            );
            float thumbCenterY = offset.dy + this.size.height / 2.0f;

            this._thumbPainter.paint(canvas, Rect.fromLTRB(
                thumbLeft,
                thumbCenterY - CustomSwitchUtils._kThumbRadius,
                thumbRight,
                thumbCenterY + CustomSwitchUtils._kThumbRadius
            ));
        }


        public override void debugFillProperties(DiagnosticPropertiesBuilder description) {
            base.debugFillProperties(description);
            description.add(
                new FlagProperty("value", value: this.value, ifTrue: "checked", ifFalse: "unchecked", showName: true));
            description.add(new FlagProperty("isInteractive", value: this.isInteractive, ifTrue: "enabled",
                ifFalse: "disabled",
                showName: true, defaultValue: true));
        }
    }

    public class CustomThumbPainter {
        public CustomThumbPainter(
            Color color = null,
            Color shadowColor = null
        ) {
            this._shadowPaint = new BoxShadow(
                color: shadowColor,
                blurRadius: 1.0f
            ).toPaint();

            this.color = color ?? CColors.White;
            this.shadowColor = shadowColor ?? new Color(0x2C000000);
        }

        readonly Color color;
        readonly Color shadowColor;
        readonly Paint _shadowPaint;
        public const float radius = 14.0f;
        public const float extension = 7.0f;

        public void paint(Canvas canvas, Rect rect) {
            RRect rRect = RRect.fromRectAndRadius(
                rect: rect,
                Radius.circular(rect.shortestSide / 2.0f)
            );

            canvas.drawRRect(rect: rRect, paint: this._shadowPaint);
            canvas.drawRRect(rRect.shift(new Offset(0.0f, 3.0f)), paint: this._shadowPaint);
            var _paint = new Paint {
                color = this.color
            };
            canvas.drawRRect(rect: rRect, paint: _paint);
        }
    }
}