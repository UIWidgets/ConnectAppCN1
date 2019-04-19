using System;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public enum CustomDismissibleMode {
        none,
        slide,
        dismiss,
        resize
    }

    public enum CustomDismissibleActionType {
        primary,
        secondary
    }

    public delegate void DismissSlideActionCallback(CustomDismissibleActionType actionType);

    public delegate bool SlideActionWillBeDismissed(CustomDismissibleActionType actionType);

    public delegate Widget SlideActionBuilder(BuildContext context, int index, Animation<float> animation,
        CustomDismissibleMode mode);

    public static class CustomDismissibleUtil {
        public static readonly float ActionsExtentRatio = 0.25f;
        public static readonly float FastThreshold = 2500;
        public static readonly float DismissThreshold = 0.75f;
        public static readonly Curve ResizeTimeCurve = new Interval(0.4f, 1.0f, Curves.ease);
        public static readonly TimeSpan ResizeDuration = new TimeSpan(0, 0, 0, 0, 300);
        public static readonly TimeSpan MovementDuration = new TimeSpan(0, 0, 0, 0, 200);
    }

    public abstract class SlideToDismissDelegate {
        protected SlideToDismissDelegate(
            Dictionary<CustomDismissibleActionType, float?> dismissThresholds = null,
            VoidCallback onResize = null,
            DismissSlideActionCallback onDismissed = null,
            TimeSpan? resizeDuration = null,
            SlideActionWillBeDismissed onWillDismiss = null,
            bool closeOnCanceled = false
        ) {
            this.dismissThresholds = dismissThresholds ?? new Dictionary<CustomDismissibleActionType, float?>();
            this.onResize = onResize;
            this.onDismissed = onDismissed;
            this.resizeDuration = resizeDuration ?? CustomDismissibleUtil.ResizeDuration;
            this.onWillDismiss = onWillDismiss;
            this.closeOnCanceled = closeOnCanceled;
        }

        public readonly Dictionary<CustomDismissibleActionType, float?> dismissThresholds;
        public readonly VoidCallback onResize;
        public readonly DismissSlideActionCallback onDismissed;
        public readonly TimeSpan? resizeDuration;
        public readonly SlideActionWillBeDismissed onWillDismiss;
        public readonly bool closeOnCanceled;

        public Widget buildActions(BuildContext context, CustomDismissibleDelegateContext ctx,
            CustomDismissibleDelegate dismissibleDelegate) {
            if (ctx.state.overallMoveAnimation.value > ctx.state.totalActionsExtent)
                return buildActionsWhileDismissing(context, ctx);
            return dismissibleDelegate.buildActions(context, ctx);
        }

        protected abstract Widget buildActionsWhileDismissing(BuildContext context,
            CustomDismissibleDelegateContext ctx);
    }

    public class SlideToDismissDrawerDelegate : SlideToDismissDelegate {
        public SlideToDismissDrawerDelegate(
            Dictionary<CustomDismissibleActionType, float?> dismissThresholds = null,
            VoidCallback onResize = null,
            DismissSlideActionCallback onDismissed = null,
            TimeSpan? resizeDuration = null,
            SlideActionWillBeDismissed onWillDismiss = null,
            bool closeOnCanceled = false
        ) : base(dismissThresholds,
            onResize,
            onDismissed,
            resizeDuration,
            onWillDismiss,
            closeOnCanceled
        ) {
        }

        protected override Widget buildActionsWhileDismissing(BuildContext context,
            CustomDismissibleDelegateContext ctx) {
            var animation = new OffsetTween(
                Offset.zero,
                ctx.createOffset(ctx.state.dragSign)
            ).animate(ctx.state.overallMoveAnimation);
            return new Container(
                child: new Stack(
                    children: new List<Widget> {
                        Positioned.fill(
                            new LayoutBuilder(builder: (_context, constraints) => {
                                    var count = ctx.state.actionCount;
                                    var actionExtent =
                                        ctx.getMaxExtent(constraints) * ctx.state.widget.actionExtentRatio;
                                    var totalExtent = ctx.getMaxExtent(constraints);

                                    var extentAnimations = new List<Animation<float>>();
                                    for (var index = 0; index < count; index++) {
                                        var extentAnimation = new FloatTween(
                                            (float) actionExtent,
                                            totalExtent - (float) actionExtent * (ctx.state.actionCount - index - 1)
                                        ).animate(
                                            new CurvedAnimation(
                                                ctx.state.overallMoveAnimation,
                                                new Interval(ctx.state.totalActionsExtent, 1)
                                            )
                                        );
                                        extentAnimations.Add(extentAnimation);
                                    }

                                    return new AnimatedBuilder(
                                        animation: ctx.state.overallMoveAnimation,
                                        builder: (cxt, child) => {
                                            var widgets = new List<Widget>();
                                            for (var index = 0; index < ctx.state.actionCount; index++) {
                                                var displayIndex = ctx.showActions
                                                    ? ctx.state.actionCount - index - 1
                                                    : index;
                                                var widget = ctx.createPositioned(
                                                    position: (float) actionExtent *
                                                              (ctx.state.actionCount - index - 1),
                                                    extent: extentAnimations[index].value,
                                                    child: ctx.state.actionDelegate.build(
                                                        context,
                                                        displayIndex,
                                                        ctx.state.overallMoveAnimation,
                                                        ctx.state.renderingMode
                                                    )
                                                );
                                                widgets.Add(widget);
                                            }

                                            return new Stack(
                                                children: widgets
                                            );
                                        }
                                    );
                                }
                            )
                        ),
                        new SlideTransition(
                            position: animation,
                            child: ctx.state.widget.child
                        )
                    }
                ));
        }
    }

    public abstract class SlideActionDelegate {
        protected SlideActionDelegate() {
        }

        public abstract Widget build(BuildContext context, int index, Animation<float> animation,
            CustomDismissibleMode mode);

        public virtual int actionCount { get; }
    }

    public class SlideActionBuilderDelegate : SlideActionDelegate {
        public SlideActionBuilderDelegate(
            SlideActionBuilder builder,
            int actionCount
        ) {
            this.builder = builder;
            this.actionCount = actionCount;
        }

        private readonly SlideActionBuilder builder;
        private new int actionCount;

        public override Widget build(BuildContext context, int index, Animation<float> animation,
            CustomDismissibleMode mode) {
            return builder(context, index, animation, mode);
        }
    }

    public class SlideActionListDelegate : SlideActionDelegate {
        public SlideActionListDelegate(
            List<Widget> actions
        ) {
            this.actions = actions;
        }

        private readonly List<Widget> actions;

        public override int actionCount => actions?.Count ?? 0;

        public override Widget build(BuildContext context, int index, Animation<float> animation,
            CustomDismissibleMode mode) {
            return actions[index];
        }
    }

    public class CustomDismissibleDelegateContext {
        public CustomDismissibleDelegateContext(
            CustomDismissibleStateView state
        ) {
            this.state = state;
        }

        public readonly CustomDismissibleStateView state;

        public bool showActions => state.actionType == CustomDismissibleActionType.primary;

        public List<Widget> buildActions(BuildContext context) {
            var widgets = new List<Widget>();
            for (var index = 0; index < state.actionCount; index++) {
                var widget = state.actionDelegate.build(context, index,
                    state.actionsMoveAnimation, CustomDismissibleMode.slide);
                widgets.Add(widget);
            }

            return widgets;
        }

        public Offset createOffset(float value) {
            return state.directionIsXAxis
                ? new Offset(value, 0)
                : new Offset(0, value);
        }

        public float getMaxExtent(BoxConstraints constraints) {
            return state.directionIsXAxis
                ? constraints.maxWidth
                : constraints.maxHeight;
        }

        public Positioned createPositioned(Widget child, float extent, float position) {
            float? left = 0;
            float? right = 0;
            float? top = null;
            float? bottom = position;
            if (showActions) {
                top = position;
                bottom = null;
            }

            float? width = null;
            float? height = extent;
            if (state.directionIsXAxis) {
                left = null;
                right = position;
                if (showActions) {
                    left = position;
                    right = null;
                }

                top = 0;
                bottom = 0;
                width = extent;
                height = null;
            }

            return new Positioned(
                left: left,
                right: right,
                top: top,
                bottom: bottom,
                width: width,
                height: height,
                child: child
            );
        }
    }

    public abstract class CustomDismissibleDelegate {
        protected CustomDismissibleDelegate(
            float? fastThreshold = null
        ) {
            D.assert(fastThreshold == null || fastThreshold >= 0.0, "fastThreshold must be positive");
            this.fastThreshold = fastThreshold ?? CustomDismissibleUtil.FastThreshold;
        }

        public readonly float fastThreshold;

        public abstract Widget buildActions(BuildContext context, CustomDismissibleDelegateContext ctx);
    }

    public abstract class CustomDismissibleStackDelegate : CustomDismissibleDelegate {
        protected CustomDismissibleStackDelegate(
            float? fastThreshold = null
        ) : base(fastThreshold) {
        }

        public override Widget buildActions(BuildContext context, CustomDismissibleDelegateContext ctx) {
            var animation = new OffsetTween(
                Offset.zero,
                ctx.createOffset(ctx.state.totalActionsExtent * ctx.state.dragSign)
            ).animate(ctx.state.actionsMoveAnimation);

            if (ctx.state.actionsMoveAnimation.value != 0.0f)
                return new Container(
                    child: new Stack(
                        children: new List<Widget> {
                            buildStackActions(context, ctx),
                            new SlideTransition(
                                position: animation,
                                child: ctx.state.widget.child
                            )
                        }
                    )
                );
            return ctx.state.widget.child;
        }

        protected abstract Widget buildStackActions(BuildContext context, CustomDismissibleDelegateContext ctx);
    }

    public class CustomDismissibleStretchDelegate : CustomDismissibleStackDelegate {
        public CustomDismissibleStretchDelegate(
            float? fastThreshold = null
        ) : base(fastThreshold) {
        }

        protected override Widget buildStackActions(BuildContext context, CustomDismissibleDelegateContext ctx) {
            var animation = new FloatTween(0, ctx.state.totalActionsExtent * ctx.state.dragSign
            ).animate(ctx.state.actionsMoveAnimation);

            return Positioned.fill(
                new LayoutBuilder(builder: (_context, constraints) => {
                    return new AnimatedBuilder(
                        animation: ctx.state.actionsMoveAnimation,
                        builder: (cxt, child) => {
                            var widgets = new List<Widget>();
                            ctx.buildActions(cxt).ForEach(item => {
                                var widget = new Expanded(child: item);
                                widgets.Add(widget);
                            });
                            return new Stack(
                                children: new List<Widget> {
                                    ctx.createPositioned(
                                        position: 0,
                                        extent:
                                        ctx.getMaxExtent(constraints) * animation.value.abs(),
                                        child: new Flex(
                                            ctx.state.widget.direction,
                                            children: widgets
                                        )
                                    )
                                }
                            );
                        }
                    );
                })
            );
        }
    }

    public class CustomDismissibleBehindDelegate : CustomDismissibleStackDelegate {
        public CustomDismissibleBehindDelegate(
            float? fastThreshold = null
        ) : base(fastThreshold) {
        }

        protected override Widget buildStackActions(BuildContext context, CustomDismissibleDelegateContext ctx) {
            return Positioned.fill(
                new LayoutBuilder(builder: (_context, constraints) => {
                    var widgets = new List<Widget>();
                    ctx.buildActions(_context).ForEach(item => {
                        var widget = new Expanded(child: item);
                        widgets.Add(widget);
                    });
                    return new Stack(
                        children: new List<Widget> {
                            ctx.createPositioned(
                                position: 0,
                                extent:
                                ctx.getMaxExtent(constraints) * ctx.state.totalActionsExtent,
                                child: new Flex(
                                    ctx.state.widget.direction,
                                    children: widgets
                                )
                            )
                        }
                    );
                })
            );
        }
    }

    public class CustomDismissibleScrollDelegate : CustomDismissibleStackDelegate {
        public CustomDismissibleScrollDelegate(
            float? fastThreshold = null
        ) : base(fastThreshold) {
        }

        protected override Widget buildStackActions(BuildContext context, CustomDismissibleDelegateContext ctx) {
            return Positioned.fill(
                new LayoutBuilder(builder: (_context, constraints) => {
                    var totalExtent =
                        ctx.getMaxExtent(constraints) * ctx.state.totalActionsExtent;
                    var animation = new FloatTween(-totalExtent, 0).animate(ctx.state.actionsMoveAnimation);
                    return new AnimatedBuilder(
                        animation: ctx.state.actionsMoveAnimation,
                        builder: (cxt, child) => {
                            var widgets = new List<Widget>();
                            ctx.buildActions(cxt).ForEach(item => {
                                var widget = new Expanded(child: item);
                                widgets.Add(widget);
                            });
                            return new Stack(
                                children: new List<Widget> {
                                    ctx.createPositioned(
                                        position: animation.value,
                                        extent: totalExtent,
                                        child: new Flex(
                                            ctx.state.widget.direction,
                                            children: widgets
                                        )
                                    )
                                }
                            );
                        }
                    );
                })
            );
        }
    }

    public class CustomDismissibleDrawerDelegate : CustomDismissibleStackDelegate {
        public CustomDismissibleDrawerDelegate(
            float? fastThreshold = null
        ) : base(fastThreshold) {
        }

        protected override Widget buildStackActions(BuildContext context, CustomDismissibleDelegateContext ctx) {
            return Positioned.fill(
                new LayoutBuilder(builder: (_context, constraints) => {
                    var state = ctx.state;
                    var count = state.actionCount;
                    var showActions = ctx.showActions;
                    var actionsMoveAnimation = state.actionsMoveAnimation;
                    var actionExtent = ctx.getMaxExtent(constraints) * state.widget.actionExtentRatio;
                    var actionDelegate = state.actionDelegate;

                    var animations = new List<Animation<float>>();
                    for (var index = 0; index < count; index++) {
                        var animation = new FloatTween(
                            -(float) actionExtent,
                            (count - index - 1) * (float) actionExtent
                        ).animate(actionsMoveAnimation);
                        animations.Add(animation);
                    }

                    return new AnimatedBuilder(
                        animation: actionsMoveAnimation,
                        builder: (cxt, child) => {
                            var widgets = new List<Widget>();
                            for (var index = 0; index < count; index++) {
                                var displayIndex = showActions ? count - index - 1 : index;
                                var widget = ctx.createPositioned(
                                    position: animations[index].value,
                                    extent: (float) actionExtent,
                                    child: actionDelegate.build(
                                        context,
                                        displayIndex,
                                        actionsMoveAnimation,
                                        CustomDismissibleMode.slide
                                    )
                                );
                                widgets.Add(widget);
                            }

                            return new Stack(
                                children: widgets
                            );
                        }
                    );
                })
            );
        }
    }

    public class CustomDismissibleController {
        public CustomDismissibleController(
            ValueChanged<Animation<float>> onSlideAnimationChanged = null,
            ValueChanged<bool> onSlideIsOpenChanged = null
        ) {
            this.onSlideAnimationChanged = onSlideAnimationChanged;
            this.onSlideIsOpenChanged = onSlideIsOpenChanged;
        }

        private readonly ValueChanged<Animation<float>> onSlideAnimationChanged;
        private readonly ValueChanged<bool> onSlideIsOpenChanged;
        private bool _isSlideOpen;

        private Animation<float> _slideAnimation;

        private CustomDismissibleState _activeState;

        public void _setActiveState(CustomDismissibleState newValue) {
            _activeState = newValue;
        }

        public CustomDismissibleState activeState => _activeState;

        public void setActiveState(CustomDismissibleState newValue) {
            _activeState?._flingAnimationControllers();
            _activeState = newValue;
            if (onSlideAnimationChanged != null) {
                _slideAnimation?.removeListener(_handleSlideIsOpenChanged);
                if (onSlideIsOpenChanged != null) {
                    _slideAnimation = newValue?.overallMoveAnimation;
                    _slideAnimation?.addListener(_handleSlideIsOpenChanged);
                    if (_slideAnimation == null) {
                        _isSlideOpen = false;
                        onSlideIsOpenChanged(_isSlideOpen);
                    }
                }

                onSlideAnimationChanged(newValue?.overallMoveAnimation);
            }
        }

        private void _handleSlideIsOpenChanged() {
            if (onSlideIsOpenChanged != null && _slideAnimation != null) {
                var isOpen = _slideAnimation.value != 0.0;
                if (isOpen != _isSlideOpen) {
                    _isSlideOpen = isOpen;
                    onSlideIsOpenChanged(_isSlideOpen);
                }
            }
        }
    }

    public interface CustomDismissibleStateView {
        Animation<float> overallMoveAnimation { get; }
        Animation<float> actionsMoveAnimation { get; }
        float dragSign { get; }
        CustomDismissibleActionType actionType { get; }
        int actionCount { get; }
        float totalActionsExtent { get; }
        SlideActionDelegate actionDelegate { get; }
        bool directionIsXAxis { get; }
        CustomDismissible widget { get; }
        CustomDismissibleMode renderingMode { get; }
    }

    public class CustomDismissible : StatefulWidget {
        private CustomDismissible(
            Key key,
            Widget child,
            CustomDismissibleDelegate dismissibleDelegate,
            SlideActionDelegate actionDelegate = null,
            SlideActionDelegate secondaryActionDelegate = null,
            float showAllActionsThreshold = 0.5f,
            float? actionExtentRatio = null,
            TimeSpan? movementDuration = null,
            Axis direction = Axis.horizontal,
            bool closeOnScroll = true,
            bool enabled = true,
            SlideToDismissDelegate slideToDismissDelegate = null,
            CustomDismissibleController controller = null
        ) : base(key) {
            D.assert(dismissibleDelegate != null);
            this.child = child;
            this.dismissibleDelegate = dismissibleDelegate;
            this.actionDelegate = actionDelegate;
            this.secondaryActionDelegate = secondaryActionDelegate;
            this.showAllActionsThreshold = showAllActionsThreshold;
            this.actionExtentRatio = actionExtentRatio ?? CustomDismissibleUtil.ActionsExtentRatio;
            this.movementDuration = movementDuration ?? CustomDismissibleUtil.MovementDuration;
            this.direction = direction;
            this.closeOnScroll = closeOnScroll;
            this.enabled = enabled;
            this.slideToDismissDelegate = slideToDismissDelegate;
            this.controller = controller;
        }

        public static CustomDismissible builder(
            Key key,
            Widget child,
            CustomDismissibleDelegate dismissibleDelegate,
            List<Widget> actions = null,
            List<Widget> secondaryActions = null,
            float showAllActionsThreshold = 0.5f,
            float? actionExtentRatio = null,
            TimeSpan? movementDuration = null,
            Axis direction = Axis.horizontal,
            bool closeOnScroll = true,
            bool enabled = true,
            SlideToDismissDelegate slideToDismissDelegate = null,
            CustomDismissibleController controller = null
        ) {
            return new CustomDismissible(
                key,
                child,
                dismissibleDelegate,
                new SlideActionListDelegate(actions),
                new SlideActionListDelegate(secondaryActions),
                showAllActionsThreshold,
                actionExtentRatio ?? CustomDismissibleUtil.ActionsExtentRatio,
                movementDuration,
                direction,
                closeOnScroll,
                enabled,
                slideToDismissDelegate,
                controller
            );
        }

        public readonly Widget child;
        public readonly CustomDismissibleDelegate dismissibleDelegate;
        public readonly SlideActionDelegate actionDelegate;
        public readonly SlideActionDelegate secondaryActionDelegate;
        public readonly float showAllActionsThreshold;
        public readonly float? actionExtentRatio;
        public readonly TimeSpan? movementDuration;
        public readonly Axis direction;
        public readonly bool closeOnScroll;
        public readonly bool enabled;
        public readonly SlideToDismissDelegate slideToDismissDelegate;
        public readonly CustomDismissibleController controller;

        private static CustomDismissibleState of(BuildContext context) {
            return (CustomDismissibleState) context.ancestorStateOfType(new TypeMatcher<CustomDismissibleState>());
        }

        public override State createState() {
            return new CustomDismissibleState();
        }
    }

    public class CustomDismissibleState : AutomaticKeepAliveClientWithTickerProviderStateMixin<CustomDismissible>,
        CustomDismissibleStateView {
        private AnimationController _overallMoveController;
        public Animation<float> overallMoveAnimation => _overallMoveController.view;

        private AnimationController _actionsMoveController;
        public Animation<float> actionsMoveAnimation => _actionsMoveController.view;

        private AnimationController _resizeController;
        private Animation<float> _resizeAnimation;

        private float _dragExtent;
        public float dragSign => _dragExtent.sign() == 0 ? 1.0f : _dragExtent.sign();

        private CustomDismissibleMode _renderingMode = CustomDismissibleMode.none;
        public CustomDismissibleMode renderingMode => _renderingMode;

        private ScrollPosition _scrollPosition;
        private bool _dragUnderway = false;
        private Size _sizePriorToCollapse;
        private bool _dismissing = false;

        public CustomDismissibleActionType actionType =>
            dragSign > 0 ? CustomDismissibleActionType.primary : CustomDismissibleActionType.secondary;

        public int actionCount => actionDelegate?.actionCount ?? 0;

        public float totalActionsExtent => (float) widget.actionExtentRatio * actionCount;

        public float dismissThreshold => widget.slideToDismissDelegate.dismissThresholds[actionType] ??
                                         CustomDismissibleUtil.DismissThreshold;

        public bool dismissible => widget.slideToDismissDelegate != null && dismissThreshold < 1.0;

        protected override bool wantKeepAlive =>
            !widget.closeOnScroll &&
            (_overallMoveController?.isAnimating == true ||
             _actionsMoveController?.isAnimating == true ||
             _resizeController?.isAnimating == true);

        public SlideActionDelegate actionDelegate =>
            actionType == CustomDismissibleActionType.primary
                ? widget.actionDelegate
                : widget.secondaryActionDelegate;

        public bool directionIsXAxis => widget.direction == Axis.horizontal;

        private float _overallDragAxisExtent {
            get {
                var size = context.size;
                return directionIsXAxis ? size.width : size.height;
            }
        }

        private float _actionsDragAxisExtent => _overallDragAxisExtent * totalActionsExtent;

        public override void initState() {
            base.initState();
            _overallMoveController = new AnimationController(duration: widget.movementDuration, vsync: this);
            _overallMoveController.addStatusListener(_handleDismissStatusChanged);
            _actionsMoveController = new AnimationController(duration: widget.movementDuration, vsync: this);
            _actionsMoveController.addStatusListener(_handleShowAllActionsStatusChanged);
            _dragExtent = 0;
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            _removeScrollingNotifierListener();
            _addScrollingNotifierListener();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            if (oldWidget is CustomDismissible customDismissible)
                if (widget.closeOnScroll != customDismissible.closeOnScroll) {
                    _removeScrollingNotifierListener();
                    _addScrollingNotifierListener();
                }
        }

        public override void dispose() {
            _overallMoveController.dispose();
            _actionsMoveController.dispose();
            _resizeController?.dispose();
            _removeScrollingNotifierListener();
            widget.controller?._setActiveState(null);
            base.dispose();
        }

        private void _addScrollingNotifierListener() {
            if (widget.closeOnScroll) {
                _scrollPosition = Scrollable.of(context)?.position;
                if (_scrollPosition != null)
                    _scrollPosition.isScrollingNotifier.addListener(_isScrollingListener);
            }
        }

        private void _removeScrollingNotifierListener() {
            if (_scrollPosition != null) _scrollPosition.isScrollingNotifier.removeListener(_isScrollingListener);
        }

        public void open() {
            _actionsMoveController.fling();
            _overallMoveController.animateTo(
                totalActionsExtent,
                curve: Curves.easeIn,
                duration: widget.movementDuration
            );
        }

        public void close() {
            _flingAnimationControllers();
            widget.controller?.setActiveState(null);
        }

        public void _flingAnimationControllers() {
            if (!_dismissing) {
                _actionsMoveController.fling(-1);
                _overallMoveController.fling(-1);
            }
        }

        public void dismiss(CustomDismissibleActionType? actionType) {
            if (dismissible) {
                _dismissing = true;
                if (actionType == null)
                    actionType = this.actionType;
                if (actionType != this.actionType)
                    setState(() => { _dragExtent = actionType == CustomDismissibleActionType.primary ? 1 : -1; });
                _overallMoveController.fling();
            }
        }

        private void _isScrollingListener() {
            if (!widget.closeOnScroll || _scrollPosition == null) return;
            if (_scrollPosition.isScrollingNotifier.value) close();
        }

        private void _handleDragStart(DragStartDetails details) {
            _dragUnderway = true;
            widget.controller?.setActiveState(this);
            _dragExtent = _actionsMoveController.value *
                          _actionsDragAxisExtent *
                          _dragExtent.sign();
            if (_overallMoveController.isAnimating) _overallMoveController.stop();

            if (_actionsMoveController.isAnimating) _actionsMoveController.stop();
        }

        private void _handleDragUpdate(DragUpdateDetails details) {
            if (widget.controller != null && widget.controller.activeState != this) return;

            var delta = details.primaryDelta;
            if (delta != null)
                _dragExtent += (float) delta;
            setState(() => {
                _overallMoveController.setValue(_dragExtent.abs() / _overallDragAxisExtent);
                _actionsMoveController.setValue(_dragExtent.abs() / _actionsDragAxisExtent);
                _renderingMode = _overallMoveController.value > totalActionsExtent
                    ? CustomDismissibleMode.dismiss
                    : CustomDismissibleMode.slide;
            });
        }

        private void _handleDragEnd(DragEndDetails details) {
            if (widget.controller != null && widget.controller.activeState != this) return;

            _dragUnderway = false;
            var velocity = (float) details.primaryVelocity;
            var shouldOpen = velocity.sign() == _dragExtent.sign();
            var fast = velocity.abs() > widget.dismissibleDelegate.fastThreshold;

            if (dismissible && overallMoveAnimation.value > totalActionsExtent) {
                // We are in a dismiss state.
                if (overallMoveAnimation.value >= dismissThreshold)
                    dismiss(null);
                else
                    open();
            }
            else if (actionsMoveAnimation.value >= widget.showAllActionsThreshold || (shouldOpen && fast)) {
                open();
            }
            else {
                close();
            }
        }

        private void _handleShowAllActionsStatusChanged(AnimationStatus status) {
            if (status == AnimationStatus.completed || status == AnimationStatus.dismissed) setState(() => { });
            updateKeepAlive();
        }

        private void _handleDismissStatusChanged(AnimationStatus status) {
            if (dismissible) {
                if (status == AnimationStatus.completed &&
                    _overallMoveController.value == _overallMoveController.upperBound &&
                    !_dragUnderway) {
                    if (widget.slideToDismissDelegate.onWillDismiss == null ||
                        widget.slideToDismissDelegate.onWillDismiss(actionType)) {
                        _startResizeAnimation();
                    }
                    else {
                        _dismissing = false;
                        if (widget.slideToDismissDelegate?.closeOnCanceled == true)
                            close();
                        else
                            open();
                    }
                }

                updateKeepAlive();
            }
        }

        private void _handleDismiss() {
            widget.controller?.setActiveState(null);
            var slideToDismissDelegate = widget.slideToDismissDelegate;
            if (slideToDismissDelegate.onDismissed != null) slideToDismissDelegate.onDismissed(actionType);
        }

        private void _startResizeAnimation() {
            D.assert(_overallMoveController != null);
            D.assert(_overallMoveController.isCompleted);
            D.assert(_resizeController == null);
            D.assert(_sizePriorToCollapse == null);
            var slideToDismissDelegate = widget.slideToDismissDelegate;
            if (slideToDismissDelegate.resizeDuration == null) {
                _handleDismiss();
            }
            else {
                _resizeController =
                    new AnimationController(duration: slideToDismissDelegate.resizeDuration, vsync: this);
                _resizeController.addListener(_handleResizeProgressChanged);
                _resizeController.addStatusListener(status => updateKeepAlive());
                _resizeController.forward();
                setState(() => {
                    _renderingMode = CustomDismissibleMode.resize;
                    _sizePriorToCollapse = context.size;
                    _resizeAnimation = new FloatTween(1, 0).animate(
                        new CurvedAnimation(_resizeController, CustomDismissibleUtil.ResizeTimeCurve));
                });
            }
        }

        private void _handleResizeProgressChanged() {
            var slideToDismissDelegate = widget.slideToDismissDelegate;
            if (_resizeController.isCompleted) {
                _handleDismiss();
            }
            else {
                if (slideToDismissDelegate.onResize != null)
                    slideToDismissDelegate.onResize();
            }
        }

        public override Widget build(BuildContext context) {
            base.build(context); // See AutomaticKeepAliveClientMixin.
            if (!widget.enabled ||
                (widget.actionDelegate == null || widget.actionDelegate.actionCount == 0) &&
                (widget.secondaryActionDelegate == null || widget.secondaryActionDelegate.actionCount == 0))
                return widget.child;

            var content = widget.child;

            if (actionType == CustomDismissibleActionType.primary &&
                widget.actionDelegate != null &&
                widget.actionDelegate.actionCount > 0 ||
                actionType == CustomDismissibleActionType.secondary &&
                widget.secondaryActionDelegate != null &&
                widget.secondaryActionDelegate.actionCount > 0) {
                if (dismissible) {
                    content = widget.slideToDismissDelegate.buildActions(
                        context,
                        new CustomDismissibleDelegateContext(this),
                        widget.dismissibleDelegate
                    );

                    if (_resizeAnimation != null) {
                        D.assert(() => {
                            if (_resizeAnimation.status != AnimationStatus.forward)
                                D.assert(_resizeAnimation.status == AnimationStatus.completed);
                            return true;
                        });
                        return new SizeTransition(
                            sizeFactor: _resizeAnimation,
                            axis: directionIsXAxis ? Axis.vertical : Axis.horizontal,
                            child: new SizedBox(
                                width: _sizePriorToCollapse.width,
                                height: _sizePriorToCollapse.height,
                                child: content
                            )
                        );
                    }
                }
                else {
                    content = widget.dismissibleDelegate.buildActions(
                        context,
                        new CustomDismissibleDelegateContext(this)
                    );
                }
            }

            GestureDragStartCallback onHorizontalDragStart = null;
            GestureDragUpdateCallback onHorizontalDragUpdate = null;
            GestureDragEndCallback onHorizontalDragEnd = null;
            GestureDragStartCallback onVerticalDragStart = _handleDragStart;
            GestureDragUpdateCallback onVerticalDragUpdate = _handleDragUpdate;
            GestureDragEndCallback onVerticalDragEnd = _handleDragEnd;
            if (directionIsXAxis) {
                onHorizontalDragStart = _handleDragStart;
                onHorizontalDragUpdate = _handleDragUpdate;
                onHorizontalDragEnd = _handleDragEnd;
                onVerticalDragStart = null;
                onVerticalDragUpdate = null;
                onVerticalDragEnd = null;
            }

            return new GestureDetector(
                onHorizontalDragStart: onHorizontalDragStart,
                onHorizontalDragUpdate: onHorizontalDragUpdate,
                onHorizontalDragEnd: onHorizontalDragEnd,
                onVerticalDragStart: onVerticalDragStart,
                onVerticalDragUpdate: onVerticalDragUpdate,
                onVerticalDragEnd: onVerticalDragEnd,
                behavior: HitTestBehavior.opaque,
                child: content
            );
        }
    }
}