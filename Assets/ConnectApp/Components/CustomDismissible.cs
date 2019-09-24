using System;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
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
            if (ctx.state.overallMoveAnimation.value > ctx.state.totalActionsExtent) {
                return this.buildActionsWhileDismissing(context, ctx);
            }

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

        readonly SlideActionBuilder builder;
        new int actionCount;

        public override Widget build(BuildContext context, int index, Animation<float> animation,
            CustomDismissibleMode mode) {
            return this.builder(context, index, animation, mode);
        }
    }

    public class SlideActionListDelegate : SlideActionDelegate {
        public SlideActionListDelegate(
            List<Widget> actions
        ) {
            this.actions = actions;
        }

        readonly List<Widget> actions;

        public override int actionCount {
            get { return this.actions?.Count ?? 0; }
        }

        public override Widget build(BuildContext context, int index, Animation<float> animation,
            CustomDismissibleMode mode) {
            return this.actions[index];
        }
    }

    public class CustomDismissibleDelegateContext {
        public CustomDismissibleDelegateContext(
            CustomDismissibleStateView state
        ) {
            this.state = state;
        }

        public readonly CustomDismissibleStateView state;

        public bool showActions {
            get { return this.state.actionType == CustomDismissibleActionType.primary; }
        }

        public List<Widget> buildActions(BuildContext context) {
            var widgets = new List<Widget>();
            for (var index = 0; index < this.state.actionCount; index++) {
                var widget = this.state.actionDelegate.build(context, index, this.state.actionsMoveAnimation,
                    CustomDismissibleMode.slide);
                widgets.Add(widget);
            }

            return widgets;
        }

        public Offset createOffset(float value) {
            return this.state.directionIsXAxis
                ? new Offset(value, 0)
                : new Offset(0, value);
        }

        public float getMaxExtent(BoxConstraints constraints) {
            return this.state.directionIsXAxis
                ? constraints.maxWidth
                : constraints.maxHeight;
        }

        public Positioned createPositioned(Widget child, float extent, float position) {
            float? left = 0;
            float? right = 0;
            float? top = null;
            float? bottom = position;
            if (this.showActions) {
                top = position;
                bottom = null;
            }

            float? width = null;
            float? height = extent;
            if (this.state.directionIsXAxis) {
                left = null;
                right = position;
                if (this.showActions) {
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
            D.assert(fastThreshold == null || fastThreshold >= 0.0, () => "fastThreshold must be positive");
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

            if (ctx.state.actionsMoveAnimation.value != 0.0f) {
                return new Container(
                    child: new Stack(
                        children: new List<Widget> {
                            this.buildStackActions(context, ctx),
                            new SlideTransition(
                                position: animation,
                                child: ctx.state.widget.child
                            )
                        }
                    )
                );
            }

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

        readonly ValueChanged<Animation<float>> onSlideAnimationChanged;
        readonly ValueChanged<bool> onSlideIsOpenChanged;
        bool _isSlideOpen;

        Animation<float> _slideAnimation;

        CustomDismissibleState _activeState;

        public void _setActiveState(CustomDismissibleState newValue) {
            this._activeState = newValue;
        }

        public CustomDismissibleState activeState {
            get { return this._activeState; }
        }

        public void setActiveState(CustomDismissibleState newValue) {
            this._activeState?._flingAnimationControllers();
            this._activeState = newValue;
            if (this.onSlideAnimationChanged != null) {
                this._slideAnimation?.removeListener(this._handleSlideIsOpenChanged);
                if (this.onSlideIsOpenChanged != null) {
                    this._slideAnimation = newValue?.overallMoveAnimation;
                    this._slideAnimation?.addListener(this._handleSlideIsOpenChanged);
                    if (this._slideAnimation == null) {
                        this._isSlideOpen = false;
                        this.onSlideIsOpenChanged(this._isSlideOpen);
                    }
                }

                this.onSlideAnimationChanged(newValue?.overallMoveAnimation);
            }
        }

        void _handleSlideIsOpenChanged() {
            if (this.onSlideIsOpenChanged != null && this._slideAnimation != null) {
                var isOpen = this._slideAnimation.value != 0.0;
                if (isOpen != this._isSlideOpen) {
                    this._isSlideOpen = isOpen;
                    this.onSlideIsOpenChanged(this._isSlideOpen);
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
        CustomDismissible(
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

        public static CustomDismissibleState of(BuildContext context) {
            return (CustomDismissibleState) context.ancestorStateOfType(new TypeMatcher<CustomDismissibleState>());
        }

        public override State createState() {
            return new CustomDismissibleState();
        }
    }

    public class CustomDismissibleState : AutomaticKeepAliveClientWithTickerProviderStateMixin<CustomDismissible>,
        CustomDismissibleStateView {
        AnimationController _overallMoveController;

        public Animation<float> overallMoveAnimation {
            get { return this._overallMoveController.view; }
        }

        AnimationController _actionsMoveController;

        public Animation<float> actionsMoveAnimation {
            get { return this._actionsMoveController.view; }
        }

        AnimationController _resizeController;
        Animation<float> _resizeAnimation;

        float _dragExtent;

        public float dragSign {
            get { return this._dragExtent.sign() == 0 ? 1.0f : this._dragExtent.sign(); }
        }

        CustomDismissibleMode _renderingMode = CustomDismissibleMode.none;

        public CustomDismissibleMode renderingMode {
            get { return this._renderingMode; }
        }

        ScrollPosition _scrollPosition;
        bool _dragUnderway = false;
        Size _sizePriorToCollapse;
        bool _dismissing = false;

        public CustomDismissibleActionType actionType {
            get {
                return this.dragSign > 0 ? CustomDismissibleActionType.primary : CustomDismissibleActionType.secondary;
            }
        }

        public int actionCount {
            get { return this.actionDelegate?.actionCount ?? 0; }
        }

        public float totalActionsExtent {
            get { return (float) this.widget.actionExtentRatio * this.actionCount; }
        }

        public float dismissThreshold {
            get {
                return this.widget.slideToDismissDelegate.dismissThresholds[this.actionType] ??
                       CustomDismissibleUtil.DismissThreshold;
            }
        }

        public bool dismissible {
            get { return this.widget.slideToDismissDelegate != null && this.dismissThreshold < 1.0; }
        }

        protected override bool wantKeepAlive {
            get {
                return !this.widget.closeOnScroll &&
                       (this._overallMoveController?.isAnimating == true ||
                        this._actionsMoveController?.isAnimating == true ||
                        this._resizeController?.isAnimating == true);
            }
        }

        public SlideActionDelegate actionDelegate {
            get {
                return this.actionType == CustomDismissibleActionType.primary
                    ? this.widget.actionDelegate
                    : this.widget.secondaryActionDelegate;
            }
        }

        public bool directionIsXAxis {
            get { return this.widget.direction == Axis.horizontal; }
        }

        float _overallDragAxisExtent {
            get {
                var size = this.context.size;
                return this.directionIsXAxis ? size.width : size.height;
            }
        }

        float _actionsDragAxisExtent {
            get { return this._overallDragAxisExtent * this.totalActionsExtent; }
        }

        public override void initState() {
            base.initState();
            this._overallMoveController = new AnimationController(duration: this.widget.movementDuration, vsync: this);
            this._overallMoveController.addStatusListener(this._handleDismissStatusChanged);
            this._actionsMoveController = new AnimationController(duration: this.widget.movementDuration, vsync: this);
            this._actionsMoveController.addStatusListener(this._handleShowAllActionsStatusChanged);
            this._dragExtent = 0;
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            this._removeScrollingNotifierListener();
            this._addScrollingNotifierListener();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            if (oldWidget is CustomDismissible customDismissible) {
                if (this.widget.closeOnScroll != customDismissible.closeOnScroll) {
                    this._removeScrollingNotifierListener();
                    this._addScrollingNotifierListener();
                }
            }
        }

        public override void dispose() {
            this._overallMoveController.dispose();
            this._actionsMoveController.dispose();
            this._resizeController?.dispose();
            this._removeScrollingNotifierListener();
            this.widget.controller?._setActiveState(null);
            base.dispose();
        }

        void _addScrollingNotifierListener() {
            if (this.widget.closeOnScroll) {
                this._scrollPosition = Scrollable.of(this.context)?.position;
                if (this._scrollPosition != null) {
                    this._scrollPosition.isScrollingNotifier.addListener(this._isScrollingListener);
                }
            }
        }

        void _removeScrollingNotifierListener() {
            if (this._scrollPosition != null) {
                this._scrollPosition.isScrollingNotifier.removeListener(this._isScrollingListener);
            }
        }

        public void open() {
            this._actionsMoveController.fling();
            this._overallMoveController.animateTo(this.totalActionsExtent,
                curve: Curves.easeIn,
                duration: this.widget.movementDuration
            );
        }

        public void close() {
            this._flingAnimationControllers();
            this.widget.controller?.setActiveState(null);
        }

        public void _flingAnimationControllers() {
            if (!this._dismissing) {
                this._actionsMoveController.fling(-1);
                this._overallMoveController.fling(-1);
            }
        }

        public void dismiss(CustomDismissibleActionType? actionType) {
            if (this.dismissible) {
                this._dismissing = true;
                if (actionType == null) {
                    actionType = this.actionType;
                }

                if (actionType != this.actionType) {
                    this.setState(() => {
                        this._dragExtent = actionType == CustomDismissibleActionType.primary ? 1 : -1;
                    });
                }

                this._overallMoveController.fling();
            }
        }

        void _isScrollingListener() {
            if (!this.widget.closeOnScroll || this._scrollPosition == null) {
                return;
            }

            if (this._scrollPosition.isScrollingNotifier.value) {
                this.close();
            }
        }

        void _handleDragStart(DragStartDetails details) {
            this._dragUnderway = true;
            this.widget.controller?.setActiveState(this);
            this._dragExtent = this._actionsMoveController.value * this._actionsDragAxisExtent *
                               this._dragExtent.sign();
            if (this._overallMoveController.isAnimating) {
                this._overallMoveController.stop();
            }

            if (this._actionsMoveController.isAnimating) {
                this._actionsMoveController.stop();
            }
        }

        void _handleDragUpdate(DragUpdateDetails details) {
            if (this.widget.controller != null && this.widget.controller.activeState != this) {
                return;
            }

            var delta = details.primaryDelta;
            if (delta != null) {
                this._dragExtent += (float) delta;
            }

            this.setState(() => {
                this._overallMoveController.setValue(this._dragExtent.abs() / this._overallDragAxisExtent);
                this._actionsMoveController.setValue(this._dragExtent.abs() / this._actionsDragAxisExtent);
                this._renderingMode = this._overallMoveController.value > this.totalActionsExtent
                    ? CustomDismissibleMode.dismiss
                    : CustomDismissibleMode.slide;
            });
        }

        void _handleDragEnd(DragEndDetails details) {
            if (this.widget.controller != null && this.widget.controller.activeState != this) {
                return;
            }

            this._dragUnderway = false;
            var velocity = (float) details.primaryVelocity;
            var shouldOpen = velocity.sign() == this._dragExtent.sign();
            var fast = velocity.abs() > this.widget.dismissibleDelegate.fastThreshold;

            if (this.dismissible && this.overallMoveAnimation.value > this.totalActionsExtent) {
                // We are in a dismiss state.
                if (this.overallMoveAnimation.value >= this.dismissThreshold) {
                    this.dismiss(null);
                }
                else {
                    this.open();
                }
            }
            else if (this.actionsMoveAnimation.value >= this.widget.showAllActionsThreshold || (shouldOpen && fast)) {
                this.open();
            }
            else {
                this.close();
            }
        }

        void _handleShowAllActionsStatusChanged(AnimationStatus status) {
            if (status == AnimationStatus.completed || status == AnimationStatus.dismissed) {
                this.setState(() => { });
            }

            this.updateKeepAlive();
        }

        void _handleDismissStatusChanged(AnimationStatus status) {
            if (this.dismissible) {
                if (status == AnimationStatus.completed &&
                    this._overallMoveController.value == this._overallMoveController.upperBound &&
                    !this._dragUnderway) {
                    if (this.widget.slideToDismissDelegate.onWillDismiss == null ||
                        this.widget.slideToDismissDelegate.onWillDismiss(this.actionType)) {
                        this._startResizeAnimation();
                    }
                    else {
                        this._dismissing = false;
                        if (this.widget.slideToDismissDelegate?.closeOnCanceled == true) {
                            this.close();
                        }
                        else {
                            this.open();
                        }
                    }
                }

                this.updateKeepAlive();
            }
        }

        void _handleDismiss() {
            this.widget.controller?.setActiveState(null);
            var slideToDismissDelegate = this.widget.slideToDismissDelegate;
            if (slideToDismissDelegate.onDismissed != null) {
                slideToDismissDelegate.onDismissed(this.actionType);
            }
        }

        void _startResizeAnimation() {
            D.assert(this._overallMoveController != null);
            D.assert(this._overallMoveController.isCompleted);
            D.assert(this._resizeController == null);
            D.assert(this._sizePriorToCollapse == null);
            var slideToDismissDelegate = this.widget.slideToDismissDelegate;
            if (slideToDismissDelegate.resizeDuration == null) {
                this._handleDismiss();
            }
            else {
                this._resizeController =
                    new AnimationController(duration: slideToDismissDelegate.resizeDuration, vsync: this);
                this._resizeController.addListener(this._handleResizeProgressChanged);
                this._resizeController.addStatusListener(status => this.updateKeepAlive());
                this._resizeController.forward();
                this.setState(() => {
                    this._renderingMode = CustomDismissibleMode.resize;
                    this._sizePriorToCollapse = this.context.size;
                    this._resizeAnimation = new FloatTween(1, 0).animate(
                        new CurvedAnimation(this._resizeController, CustomDismissibleUtil.ResizeTimeCurve));
                });
            }
        }

        void _handleResizeProgressChanged() {
            var slideToDismissDelegate = this.widget.slideToDismissDelegate;
            if (this._resizeController.isCompleted) {
                this._handleDismiss();
            }
            else {
                if (slideToDismissDelegate.onResize != null) {
                    slideToDismissDelegate.onResize();
                }
            }
        }

        public override Widget build(BuildContext context) {
            base.build(context); // See AutomaticKeepAliveClientMixin.
            if (!this.widget.enabled ||
                (this.widget.actionDelegate == null || this.widget.actionDelegate.actionCount == 0) &&
                (this.widget.secondaryActionDelegate == null || this.widget.secondaryActionDelegate.actionCount == 0)) {
                return this.widget.child;
            }

            var content = this.widget.child;

            if (this.actionType == CustomDismissibleActionType.primary && this.widget.actionDelegate != null &&
                this.widget.actionDelegate.actionCount > 0 ||
                this.actionType == CustomDismissibleActionType.secondary &&
                this.widget.secondaryActionDelegate != null && this.widget.secondaryActionDelegate.actionCount > 0) {
                if (this.dismissible) {
                    content = this.widget.slideToDismissDelegate.buildActions(
                        context,
                        new CustomDismissibleDelegateContext(this), this.widget.dismissibleDelegate
                    );

                    if (this._resizeAnimation != null) {
                        D.assert(() => {
                            if (this._resizeAnimation.status != AnimationStatus.forward) {
                                D.assert(this._resizeAnimation.status == AnimationStatus.completed);
                            }

                            return true;
                        });
                        return new SizeTransition(
                            sizeFactor: this._resizeAnimation,
                            axis: this.directionIsXAxis ? Axis.vertical : Axis.horizontal,
                            child: new SizedBox(
                                width: this._sizePriorToCollapse.width,
                                height: this._sizePriorToCollapse.height,
                                child: content
                            )
                        );
                    }
                }
                else {
                    content = this.widget.dismissibleDelegate.buildActions(
                        context,
                        new CustomDismissibleDelegateContext(this)
                    );
                }
            }

            GestureDragStartCallback onHorizontalDragStart = null;
            GestureDragUpdateCallback onHorizontalDragUpdate = null;
            GestureDragEndCallback onHorizontalDragEnd = null;
            GestureDragStartCallback onVerticalDragStart = this._handleDragStart;
            GestureDragUpdateCallback onVerticalDragUpdate = this._handleDragUpdate;
            GestureDragEndCallback onVerticalDragEnd = this._handleDragEnd;
            if (this.directionIsXAxis) {
                onHorizontalDragStart = this._handleDragStart;
                onHorizontalDragUpdate = this._handleDragUpdate;
                onHorizontalDragEnd = this._handleDragEnd;
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