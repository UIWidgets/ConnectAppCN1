using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Gradient = Unity.UIWidgets.painting.Gradient;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.Components {
    static class CustomTabsUtils {
        public const float _kTabHeight = 46.0f;
        public const float _kTextAndIconTabHeight = 72.0f;

        public static float _indexChangeProgress(CustomTabController controller) {
            float controllerValue = controller.animation.value;
            float previousIndex = controller.previousIndex;
            float currentIndex = controller.index;

            if (!controller.indexIsChanging) {
                return (currentIndex - controllerValue).abs().clamp(0.0f, 1.0f);
            }

            return (controllerValue - currentIndex).abs() / (currentIndex - previousIndex).abs();
        }

        public static Rect _lerpRect(Rect a, Rect b, float t, CustomTabBarIndicatorChangeStyle style) {
            if (style == CustomTabBarIndicatorChangeStyle.slide) {
                return Rect.lerp(a, b, t);
            }

            if (style == CustomTabBarIndicatorChangeStyle.enlarge) {
                return t < 0.5f
                    ? Rect.lerp(a, b, t * 2).expandToInclude(a)
                    : Rect.lerp(a, b, t * 2 - 1).expandToInclude(b);
            }
            return t < 0.5f ? a : b;
        }

        public static readonly PageScrollPhysics _kTabBarViewPhysics =
            (PageScrollPhysics) new PageScrollPhysics().applyTo(new ClampingScrollPhysics());

        public static TimeSpan kTabScrollDuration = TimeSpan.FromMilliseconds(300);
    }

    public enum CustomTabBarIndicatorChangeStyle {
        none,
        slide,
        enlarge
    }

    public enum CustomTabBarIndicatorSize {
        tab,
        label,
        fixedOrTab, // if indicatorFixedWidth == null, default to tab
        fixedOrLabel // if indicatorFixedWidth == null, default to label
    }

    public class CustomTab : StatelessWidget {
        public CustomTab(
            Key key = null,
            string text = null,
            Widget icon = null,
            Widget child = null
        ) : base(key: key) {
            D.assert(text != null || child != null || icon != null);
            D.assert(!(text != null && child != null));
            this.text = text;
            this.icon = icon;
            this.child = child;
        }

        public readonly string text;
        readonly Widget child;
        public readonly Widget icon;

        Widget _buildLabelText() {
            return this.child ?? new Text(data: this.text, softWrap: false, overflow: TextOverflow.fade);
        }

        public override Widget build(BuildContext context) {
            float height;
            Widget label;
            if (this.icon == null) {
                height = CustomTabsUtils._kTabHeight;
                label = this._buildLabelText();
            }
            else if (this.text == null && this.child == null) {
                height = CustomTabsUtils._kTabHeight;
                label = this.icon;
            }
            else {
                height = CustomTabsUtils._kTextAndIconTabHeight;
                label = new Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new Container(
                            child: this.icon,
                            margin: EdgeInsets.only(bottom: 10.0f)
                        ),
                        this._buildLabelText()
                    }
                );
            }

            return new SizedBox(
                height: height,
                child: new Center(
                    child: label,
                    widthFactor: 1.0f
                )
            );
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties: properties);
            properties.add(new StringProperty("text", value: this.text,
                defaultValue: Diagnostics.kNullDefaultValue));
            properties.add(new DiagnosticsProperty<Widget>("icon", value: this.icon,
                defaultValue: Diagnostics.kNullDefaultValue));
        }
    }

    class _CustomTabStyle : AnimatedWidget {
        public _CustomTabStyle(
            Key key = null,
            Animation<float> animation = null,
            bool? selected = null,
            Color labelColor = null,
            Color unselectedLabelColor = null,
            TextStyle labelStyle = null,
            TextStyle unselectedLabelStyle = null,
            Widget child = null
        ) : base(key: key, listenable: animation) {
            D.assert(child != null);
            D.assert(selected != null);
            this.selected = selected.Value;
            this.labelColor = labelColor;
            this.unselectedLabelColor = unselectedLabelColor;
            this.labelStyle = labelStyle;
            this.unselectedLabelStyle = unselectedLabelStyle;
            this.child = child;
        }

        readonly TextStyle labelStyle;
        readonly TextStyle unselectedLabelStyle;
        readonly bool selected;
        readonly Color labelColor;
        readonly Color unselectedLabelColor;
        readonly Widget child;

        protected override Widget build(BuildContext context) {
            Animation<float> animation = (Animation<float>) this.listenable;
            
            TextStyle defaultStyle = (this.labelStyle
                                      ?? new TextStyle(
                                          color: CColors.White,
                                          fontSize: 14,
                                          fontFamily: "Roboto-Regular"
                                      )).copyWith(true);
            TextStyle defaultUnselectedStyle = (this.unselectedLabelStyle
                                               ?? this.labelStyle
                                               ?? new TextStyle(
                                                   color: CColors.White,
                                                   fontSize: 16,
                                                   fontFamily: "Roboto-Regular"
                                               )).copyWith(true);
            TextStyle textStyle = this.selected
                ? TextStyle.lerp(a: defaultStyle, b: defaultUnselectedStyle, t: animation.value)
                : TextStyle.lerp(a: defaultUnselectedStyle, b: defaultStyle, t: animation.value);

            Color selectedColor = this.labelColor ?? CColors.PrimaryBlue;
            Color unselectedColor = this.unselectedLabelColor ?? selectedColor.withAlpha(0xB2);
            Color color = this.selected
                ? Color.lerp(a: selectedColor, b: unselectedColor, t: animation.value)
                : Color.lerp(a: unselectedColor, b: selectedColor, t: animation.value);

            return new DefaultTextStyle(
                style: textStyle.copyWith(color: color),
                child: IconTheme.merge(
                    data: new IconThemeData(
                        size: 24.0f,
                        color: color
                    ),
                    child: this.child
                )
            );
        }
    }

    delegate void _CustomLayoutCallback(List<float> xOffsets, float width);

    class _CustomTabLabelBarRenderer : RenderFlex {
        public _CustomTabLabelBarRenderer(
            List<RenderBox> children = null,
            Axis? direction = null,
            MainAxisSize? mainAxisSize = null,
            MainAxisAlignment? mainAxisAlignment = null,
            CrossAxisAlignment? crossAxisAlignment = null,
            VerticalDirection? verticalDirection = null,
            _CustomLayoutCallback onPerformLayout = null
        ) : base(
            children: children,
            direction: direction.Value,
            mainAxisSize: mainAxisSize.Value,
            mainAxisAlignment: mainAxisAlignment.Value,
            crossAxisAlignment: crossAxisAlignment.Value,
            verticalDirection: verticalDirection.Value
        ) {
            D.assert(direction != null);
            D.assert(mainAxisSize != null);
            D.assert(mainAxisAlignment != null);
            D.assert(crossAxisAlignment != null);
            D.assert(verticalDirection != null);

            D.assert(onPerformLayout != null);
            this.onPerformLayout = onPerformLayout;
        }

        public _CustomLayoutCallback onPerformLayout;

        protected override void performLayout() {
            base.performLayout();

            RenderBox child = this.firstChild;
            List<float> xOffsets = new List<float>();

            while (child != null) {
                FlexParentData childParentData = (FlexParentData) child.parentData;
                xOffsets.Add(item: childParentData.offset.dx);
                D.assert(child.parentData == childParentData);
                child = childParentData.nextSibling;
            }

            xOffsets.Add(item: this.size.width);
            this.onPerformLayout(xOffsets: xOffsets, width: this.size.width);
        }
    }

    class _CustomTabLabelBar : Flex {
        public _CustomTabLabelBar(
            Key key = null,
            List<Widget> children = null,
            _CustomLayoutCallback onPerformLayout = null
        ) : base(
            key: key,
            children: children ?? new List<Widget>(),
            direction: Axis.horizontal,
            mainAxisSize: MainAxisSize.max,
            mainAxisAlignment: MainAxisAlignment.start,
            crossAxisAlignment: CrossAxisAlignment.center,
            verticalDirection: VerticalDirection.down
        ) {
            this.onPerformLayout = onPerformLayout;
        }

        readonly _CustomLayoutCallback onPerformLayout;

        public override RenderObject createRenderObject(BuildContext context) {
            return new _CustomTabLabelBarRenderer(
                direction: this.direction,
                mainAxisAlignment: this.mainAxisAlignment,
                mainAxisSize: this.mainAxisSize,
                crossAxisAlignment: this.crossAxisAlignment,
                verticalDirection: this.verticalDirection,
                onPerformLayout: this.onPerformLayout
            );
        }

        public override void updateRenderObject(BuildContext context, RenderObject renderObject) {
            base.updateRenderObject(context: context, renderObject: renderObject);
            _CustomTabLabelBarRenderer _renderObject = (_CustomTabLabelBarRenderer) renderObject;
            _renderObject.onPerformLayout = this.onPerformLayout;
        }
    }

    class _CustomIndicatorPainter : AbstractCustomPainter {
        public _CustomIndicatorPainter(
            CustomTabController controller,
            Decoration indicator,
            CustomTabBarIndicatorSize indicatorSize,
            List<GlobalKey> tabKeys,
            _CustomIndicatorPainter old = null,
            CustomTabBarIndicatorChangeStyle changeStyle = CustomTabBarIndicatorChangeStyle.slide,
            float? indicatorFixedSize = null
        ) : base(repaint: controller.animation) {
            this.controller = controller;
            this.indicator = indicator;
            this.indicatorSize = indicatorSize;
            this.tabKeys = tabKeys;
            if (old != null) {
                this.saveTabOffsets(tabOffsets: old._currentTabOffsets);
            }

            this.changeStyle = changeStyle;
            this.indicatorFixedSize = indicatorFixedSize;
        }

        readonly CustomTabController controller;
        readonly Decoration indicator;
        readonly CustomTabBarIndicatorSize indicatorSize;
        readonly List<GlobalKey> tabKeys;
        readonly CustomTabBarIndicatorChangeStyle changeStyle;
        readonly float? indicatorFixedSize;

        List<float> _currentTabOffsets;
        Rect _currentRect;
        BoxPainter _painter;
        bool _needsPaint = false;

        void markNeedsPaint() {
            this._needsPaint = true;
        }

        public void dispose() {
            this._painter?.Dispose();
        }

        public void saveTabOffsets(List<float> tabOffsets) {
            this._currentTabOffsets = tabOffsets;
        }

        public int maxTabIndex {
            get { return this._currentTabOffsets.Count - 2; }
        }

        public float centerOf(int tabIndex) {
            D.assert(this._currentTabOffsets != null);
            D.assert(this._currentTabOffsets.isNotEmpty());
            D.assert(tabIndex >= 0);
            D.assert(tabIndex <= this.maxTabIndex);
            return (this._currentTabOffsets[index: tabIndex] + this._currentTabOffsets[tabIndex + 1]) / 2.0f;
        }

        public Rect indicatorRect(Size tabBarSize, int tabIndex) {
            D.assert(this._currentTabOffsets != null);
            D.assert(this._currentTabOffsets.isNotEmpty());
            D.assert(tabIndex >= 0);
            D.assert(tabIndex <= this.maxTabIndex);
            float tabLeft = this._currentTabOffsets[tabIndex];
            float tabRight = this._currentTabOffsets[tabIndex + 1];

            if (this.indicatorFixedSize == null) {
                if (this.indicatorSize == CustomTabBarIndicatorSize.label ||
                    this.indicatorSize == CustomTabBarIndicatorSize.fixedOrLabel) {
                    float tabWidth = this.tabKeys[tabIndex].currentContext.size.width;
                    float delta = ((tabRight - tabLeft) - tabWidth) / 2.0f;
                    tabLeft += delta;
                    tabRight -= delta;
                }
            }
            else {
                if (this.indicatorSize == CustomTabBarIndicatorSize.fixedOrTab ||
                    this.indicatorSize == CustomTabBarIndicatorSize.fixedOrLabel) {
                    float delta = ((tabRight - tabLeft) - this.indicatorFixedSize.Value) / 2.0f;
                    tabLeft += delta;
                    tabRight -= delta;
                }
            }


            return Rect.fromLTWH(tabLeft, 0.0f, tabRight - tabLeft, tabBarSize.height);
        }

        public override void paint(Canvas canvas, Size size) {
            this._needsPaint = false;
            this._painter = this._painter ?? this.indicator.createBoxPainter(onChanged: this.markNeedsPaint);

            if (this.controller.indexIsChanging) {
                Rect targetRect = this.indicatorRect(tabBarSize: size, tabIndex: this.controller.index);
                
                this._currentRect = CustomTabsUtils._lerpRect(a: targetRect, this._currentRect ?? targetRect,
                    CustomTabsUtils._indexChangeProgress(controller: this.controller), this.changeStyle);
            }
            else {
                int currentIndex = this.controller.index;
                Rect previous = currentIndex > 0 ? this.indicatorRect(size, currentIndex - 1) : null;
                Rect middle = this.indicatorRect(size, currentIndex);
                Rect next = currentIndex < this.maxTabIndex ? this.indicatorRect(size, currentIndex + 1) : null;
                float index = this.controller.index;
                float value = this.controller.animation.value;
                if (value == index - 1.0f) {
                    this._currentRect = previous ?? middle;
                }
                else if (value == index + 1.0f) {
                    this._currentRect = next ?? middle;
                }
                else if (value == index) {
                    this._currentRect = middle;
                }
                else if (value < index) {
                    this._currentRect = previous == null
                        ? middle
                        : CustomTabsUtils._lerpRect(middle, previous, index - value, this.changeStyle);
                }
                else {
                    this._currentRect = next == null
                        ? middle
                        : CustomTabsUtils._lerpRect(middle, next, value - index, this.changeStyle);
                }
            }

            D.assert(this._currentRect != null);

            ImageConfiguration configuration = new ImageConfiguration(
                size: this._currentRect.size
            );

            this._painter.paint(canvas, this._currentRect.topLeft, configuration);
        }

        static bool _tabOffsetsEqual(List<float> a, List<float> b) {
            if (a?.Count != b?.Count) {
                return false;
            }

            for (int i = 0; i < a.Count; i++) {
                if (a[i] != b[i]) {
                    return false;
                }
            }

            return true;
        }

        public override bool shouldRepaint(CustomPainter old) {
            _CustomIndicatorPainter _old = (_CustomIndicatorPainter) old;
            return this._needsPaint
                   || this.controller != _old.controller
                   || this.indicator != _old.indicator
                   || this.tabKeys.Count != _old.tabKeys.Count
                   || !_tabOffsetsEqual(this._currentTabOffsets, _old._currentTabOffsets);
        }
    }

    class _CustomChangeAnimation : AnimationWithParentMixin<float, float> {
        public _CustomChangeAnimation(
            CustomTabController controller
        ) {
            this.controller = controller;
        }

        public readonly CustomTabController controller;

        public override Animation<float> parent {
            get { return this.controller.animation; }
        }

        public override float value {
            get { return CustomTabsUtils._indexChangeProgress(this.controller); }
        }
    }

    class _CustomDragAnimation : AnimationWithParentMixin<float, float> {
        public _CustomDragAnimation(
            CustomTabController controller,
            int index) {
            this.controller = controller;
            this.index = index;
        }

        public readonly CustomTabController controller;
        public readonly int index;

        public override Animation<float> parent {
            get { return this.controller.animation; }
        }

        public override float value {
            get {
                D.assert(!this.controller.indexIsChanging);
                return (this.controller.animation.value - this.index).abs().clamp(0.0f, 1.0f);
            }
        }
    }

    class _CustomTabBarScrollPosition : ScrollPositionWithSingleContext {
        public _CustomTabBarScrollPosition(
            ScrollPhysics physics = null,
            ScrollContext context = null,
            ScrollPosition oldPosition = null,
            _CustomTabBarHeaderState tabBar = null
        ) : base(
            physics: physics,
            context: context,
            initialPixels: null,
            oldPosition: oldPosition) {
            this.tabBar = tabBar;
        }

        public readonly _CustomTabBarHeaderState tabBar;

        bool _initialViewportDimensionWasZero;

        public override bool applyContentDimensions(float minScrollExtent, float maxScrollExtent) {
            bool result = true;
            if (this._initialViewportDimensionWasZero != true) {
                this._initialViewportDimensionWasZero = this.viewportDimension != 0.0;
                this.correctPixels(this.tabBar._initialScrollOffset(this.viewportDimension, minScrollExtent,
                    maxScrollExtent));
                result = false;
            }

            return base.applyContentDimensions(minScrollExtent, maxScrollExtent) && result;
        }
    }

    class _CustomTabBarScrollController : ScrollController {
        public _CustomTabBarScrollController(_CustomTabBarHeaderState tabBar) {
            this.tabBar = tabBar;
        }

        public readonly _CustomTabBarHeaderState tabBar;

        public override ScrollPosition createScrollPosition(ScrollPhysics physics, ScrollContext context,
            ScrollPosition oldPosition) {
            return new _CustomTabBarScrollPosition(
                physics: physics,
                context: context,
                oldPosition: oldPosition,
                tabBar: this.tabBar
            );
        }
    }

    public class CustomTabBarHeader : PreferredSizeWidget {
        public CustomTabBarHeader(
            Key key = null,
            List<Widget> tabs = null,
            CustomTabController controller = null,
            bool isScrollable = false,
            Color indicatorColor = null,
            float indicatorWeight = 2.0f,
            EdgeInsets indicatorPadding = null,
            Decoration indicator = null,
            CustomTabBarIndicatorSize? indicatorSize = null,
            float? indicatorFixedSize = null,
            CustomTabBarIndicatorChangeStyle indicatorChangeStyle = CustomTabBarIndicatorChangeStyle.slide,
            Color labelColor = null,
            TextStyle labelStyle = null,
            EdgeInsets labelPadding = null,
            Color unselectedLabelColor = null,
            TextStyle unselectedLabelStyle = null,
            DragStartBehavior dragStartBehavior = DragStartBehavior.start,
            ValueChanged<int> onTap = null
        ) : base(key: key) {
            D.assert(tabs != null);
            D.assert(indicator != null || indicatorWeight > 0.0f);
            this.tabs = tabs;
            this.controller = controller;
            this.isScrollable = isScrollable;
            this.indicatorColor = indicatorColor;
            this.indicatorWeight = indicatorWeight;
            this.indicatorPadding = indicatorPadding ?? EdgeInsets.zero;
            this.indicator = indicator;
            this.indicatorSize = indicatorSize;
            this.indicatorFixedSize = indicatorFixedSize;
            this.indicatorChangeStyle = indicatorChangeStyle;
            this.labelColor = labelColor;
            this.labelStyle = labelStyle;
            this.labelPadding = labelPadding;
            this.unselectedLabelColor = unselectedLabelColor;
            this.unselectedLabelStyle = unselectedLabelStyle;
            this.dragStartBehavior = dragStartBehavior;
            this.onTap = onTap;
        }

        public readonly List<Widget> tabs;

        public readonly CustomTabController controller;

        public readonly bool isScrollable;

        public readonly Color indicatorColor;

        public readonly float indicatorWeight;

        public readonly EdgeInsets indicatorPadding;

        public readonly Decoration indicator;

        public readonly CustomTabBarIndicatorSize? indicatorSize;

        public readonly float? indicatorFixedSize;

        public readonly Color labelColor;

        public readonly Color unselectedLabelColor;

        public readonly TextStyle labelStyle;

        public readonly EdgeInsets labelPadding;

        public readonly TextStyle unselectedLabelStyle;

        public readonly DragStartBehavior dragStartBehavior;

        public readonly ValueChanged<int> onTap;

        public readonly CustomTabBarIndicatorChangeStyle indicatorChangeStyle;

        public override Size preferredSize {
            get {
                foreach (Widget item in this.tabs) {
                    if (item is CustomTab tab) {
                        if (tab.text != null && tab.icon != null) {
                            return Size.fromHeight(CustomTabsUtils._kTextAndIconTabHeight + this.indicatorWeight);
                        }
                    }
                }

                return Size.fromHeight(CustomTabsUtils._kTabHeight + this.indicatorWeight);
            }
        }

        public override State createState() {
            return new _CustomTabBarHeaderState();
        }
    }

    class _CustomTabBarHeaderState : State<CustomTabBarHeader> {
        ScrollController _scrollController;
        CustomTabController _controller;
        _CustomIndicatorPainter _indicatorPainter;
        int _currentIndex;
        List<GlobalKey> _tabKeys;

        public override void initState() {
            base.initState();
            this._tabKeys = new List<GlobalKey>();
            foreach (Widget tab in this.widget.tabs) {
                this._tabKeys.Add(GlobalKey.key());
            }
        }

        Decoration _indicator {
            get {
                if (this.widget.indicator != null) {
                    return this.widget.indicator;
                }

                Color color = this.widget.indicatorColor ?? CColors.White;
                return new CustomUnderlineTabIndicator(
                    insets: this.widget.indicatorPadding,
                    borderSide: new BorderSide(
                        width: this.widget.indicatorWeight,
                        color: color));
            }
        }

        void _updateTabController() {
            CustomTabController newController = this.widget.controller;
            D.assert(() => {
                if (newController == null) {
                    throw new UIWidgetsError(
                        "No TabController for " + this.widget.GetType() + ".\n" +
                        "When creating a " + this.widget.GetType() + ", you must either provide an explicit " +
                        "TabController using the \"controller\" property, or you must ensure that there " +
                        "is a DefaultTabController above the " + this.widget.GetType() + ".\n" +
                        "In this case, there was neither an explicit controller nor a default controller."
                    );
                }

                return true;
            });
            D.assert(() => {
                if (newController.length != this.widget.tabs.Count) {
                    throw new UIWidgetsError(
                        $"Controller's length property {newController.length} does not match the\n" +
                        $"number of tab elements {this.widget.tabs.Count} present in TabBar's tabs property."
                    );
                }

                return true;
            });
            if (newController == this._controller) {
                return;
            }

            if (this._controller != null) {
                this._controller.animation.removeListener(listener: this._handleTabControllerAnimationTick);
                this._controller.removeListener(listener: this._handleTabControllerTick);
            }

            this._controller = newController;
            if (this._controller != null) {
                this._controller.animation.addListener(listener: this._handleTabControllerAnimationTick);
                this._controller.addListener(listener: this._handleTabControllerTick);
                this._currentIndex = this._controller.index;
            }
        }

        void _initIndicatorPainter() {
            this._indicatorPainter = this._controller == null
                ? null
                : new _CustomIndicatorPainter(
                    controller: this._controller,
                    indicator: this._indicator,
                    this.widget.indicatorSize ?? CustomTabBarIndicatorSize.fixedOrLabel,
                    tabKeys: this._tabKeys,
                    old: this._indicatorPainter,
                    this.widget.indicatorChangeStyle,
                    indicatorFixedSize: this.widget.indicatorFixedSize
                );
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            this._updateTabController();
            this._initIndicatorPainter();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            CustomTabBarHeader _oldWidget = (CustomTabBarHeader) oldWidget;
            if (this.widget.controller != _oldWidget.controller) {
                this._updateTabController();
                this._initIndicatorPainter();
            }
            else if (this.widget.indicatorColor != _oldWidget.indicatorColor ||
                     this.widget.indicatorWeight != _oldWidget.indicatorWeight ||
                     this.widget.indicatorSize != _oldWidget.indicatorSize ||
                     this.widget.indicator != _oldWidget.indicator) {
                this._initIndicatorPainter();
            }

            if (this.widget.tabs.Count > _oldWidget.tabs.Count) {
                int delta = this.widget.tabs.Count - _oldWidget.tabs.Count;
                for (int i = 0; i < delta; i++) {
                    this._tabKeys.Add(GlobalKey.key());
                }
            }
            else if (this.widget.tabs.Count < _oldWidget.tabs.Count) {
                int delta = _oldWidget.tabs.Count - this.widget.tabs.Count;
                this._tabKeys.RemoveRange(this.widget.tabs.Count, delta);
            }
        }

        public override void dispose() {
            this._indicatorPainter.dispose();
            if (this._controller != null) {
                this._controller.animation.removeListener(this._handleTabControllerAnimationTick);
                this._controller.removeListener(this._handleTabControllerTick);
            }

            base.dispose();
        }

        public int maxTabIndex {
            get { return this._indicatorPainter.maxTabIndex; }
        }

        float _tabScrollOffset(int index, float viewportWidth, float minExtent, float maxExtent) {
            if (!this.widget.isScrollable) {
                return 0.0f;
            }

            float tabCenter = this._indicatorPainter.centerOf(index);
            return (tabCenter - viewportWidth / 2.0f).clamp(minExtent, maxExtent);
        }

        float _tabCenteredScrollOffset(int index) {
            ScrollPosition position = this._scrollController.position;
            return this._tabScrollOffset(index, position.viewportDimension, position.minScrollExtent,
                position.maxScrollExtent);
        }

        internal float _initialScrollOffset(float viewportWidth, float minExtent, float maxExtent) {
            return this._tabScrollOffset(index: this._currentIndex, viewportWidth: viewportWidth, minExtent, maxExtent);
        }

        void _scrollToCurrentIndex() {
            float offset = this._tabCenteredScrollOffset(index: this._currentIndex);
            this._scrollController.animateTo(to: offset, duration: CustomTabsUtils.kTabScrollDuration, curve: Curves.ease);
        }

        void _scrollToControllerValue() {
            float? leadingPosition = this._currentIndex > 0
                ? (float?) this._tabCenteredScrollOffset(this._currentIndex - 1)
                : null;
            float middlePosition = this._tabCenteredScrollOffset(this._currentIndex);
            float? trailingPosition = this._currentIndex < this.maxTabIndex
                ? (float?) this._tabCenteredScrollOffset(this._currentIndex + 1)
                : null;

            float index = this._controller.index;
            float value = this._controller.animation.value;
            float offset = 0.0f;
            if (value == index - 1.0f) {
                offset = leadingPosition ?? middlePosition;
            }
            else if (value == index + 1.0f) {
                offset = trailingPosition ?? middlePosition;
            }
            else if (value == index) {
                offset = middlePosition;
            }
            else if (value < index) {
                offset = leadingPosition == null
                    ? middlePosition
                    : MathUtils.lerpNullableFloat(middlePosition, leadingPosition, index - value).Value;
            }
            else {
                offset = trailingPosition == null
                    ? middlePosition
                    : MathUtils.lerpNullableFloat(middlePosition, trailingPosition, value - index).Value;
            }

            this._scrollController.jumpTo(offset);
        }


        void _handleTabControllerAnimationTick() {
            D.assert(this.mounted);
            if (!this._controller.indexIsChanging && this.widget.isScrollable) {
                this._currentIndex = this._controller.index;
                this._scrollToControllerValue();
            }
        }

        void _handleTabControllerTick() {
            if (this._controller.index != this._currentIndex) {
                this._currentIndex = this._controller.index;
                if (this.widget.isScrollable) {
                    this._scrollToCurrentIndex();
                }
            }

            this.setState(() => { });
        }

        void _saveTabOffsets(List<float> tabOffsets, float width) {
            this._indicatorPainter?.saveTabOffsets(tabOffsets: tabOffsets);
        }

        void _handleTap(int index) {
            D.assert(index >= 0 && index < this.widget.tabs.Count);
            if (this.widget.onTap == null) {
                this._controller.animateTo(value: index);
            }
            else {
                this.widget.onTap?.Invoke(value: index);
            }
        }

        Widget _buildStyledTab(Widget child, bool selected, Animation<float> animation) {
            return new _CustomTabStyle(
                animation: animation,
                selected: selected,
                labelColor: this.widget.labelColor,
                unselectedLabelColor: this.widget.unselectedLabelColor,
                labelStyle: this.widget.labelStyle,
                unselectedLabelStyle: this.widget.unselectedLabelStyle,
                child: child
            );
        }

        public override Widget build(BuildContext context) {
            if (this._controller.length == 0) {
                return new Container(
                    height: CustomTabsUtils._kTabHeight + this.widget.indicatorWeight
                );
            }

            List<Widget> wrappedTabs = new List<Widget>();
            for (int i = 0; i < this.widget.tabs.Count; i++) {
                wrappedTabs.Add(new Container(
                        alignment: Alignment.bottomCenter,
                        child: new Padding(
                            padding: this.widget.labelPadding ?? EdgeInsets.symmetric(horizontal: 16),
                            child: new KeyedSubtree(
                                this._tabKeys[index: i],
                                this.widget.tabs[index: i]
                            )
                        )
                    )
                );
            }

            if (this._controller != null) {
                int previousIndex = this._controller.previousIndex;

                if (this._controller.indexIsChanging) {
                    D.assert(this._currentIndex != previousIndex);
                    Animation<float> animation = new _CustomChangeAnimation(controller: this._controller);
                    wrappedTabs[index: this._currentIndex] =
                        this._buildStyledTab(wrappedTabs[this._currentIndex], true, animation: animation);
                    wrappedTabs[previousIndex] = this._buildStyledTab(wrappedTabs[previousIndex], false, animation);
                }
                else {
                    int tabIndex = this._currentIndex;
                    Animation<float> centerAnimation = new _CustomDragAnimation(this._controller, tabIndex);
                    wrappedTabs[tabIndex] = this._buildStyledTab(wrappedTabs[tabIndex], true, centerAnimation);
                    if (this._currentIndex > 0) {
                        int previousTabIndex = this._currentIndex - 1;
                        Animation<float> previousAnimation =
                            new ReverseAnimation(new _CustomDragAnimation(this._controller, previousTabIndex));
                        wrappedTabs[previousTabIndex] =
                            this._buildStyledTab(wrappedTabs[previousTabIndex], false, previousAnimation);
                    }

                    if (this._currentIndex < this.widget.tabs.Count - 1) {
                        int nextTabIndex = this._currentIndex + 1;
                        Animation<float> nextAnimation =
                            new ReverseAnimation(new _CustomDragAnimation(this._controller, nextTabIndex));
                        wrappedTabs[nextTabIndex] =
                            this._buildStyledTab(wrappedTabs[nextTabIndex], false, nextAnimation);
                    }
                }
            }

            int tabCount = this.widget.tabs.Count;
            for (int index = 0; index < tabCount; index++) {
                int tabIndex = index;
                wrappedTabs[index: index] = new GestureDetector(
                    onTap: () => this._handleTap(index: tabIndex),
                    child: new Container(
                        color: CColors.Transparent,
                        child: new Padding(
                            padding: EdgeInsets.only(bottom: this.widget.indicatorWeight),
                            child: wrappedTabs[index: index]
                        )
                    )
                );
                if (!this.widget.isScrollable) {
                    wrappedTabs[index: index] = new Expanded(child: wrappedTabs[index: index]);
                }
            }

            Widget tabBar = new CustomPaint(
                painter: this._indicatorPainter,
                child: new _CustomTabStyle(
                    animation: Animations.kAlwaysDismissedAnimation,
                    selected: false,
                    labelColor: this.widget.labelColor,
                    unselectedLabelColor: this.widget.unselectedLabelColor,
                    labelStyle: this.widget.labelStyle,
                    unselectedLabelStyle: this.widget.unselectedLabelStyle,
                    child: new _CustomTabLabelBar(
                        onPerformLayout: this._saveTabOffsets,
                        children: wrappedTabs
                    )
                )
            );

            if (this.widget.isScrollable) {
                this._scrollController = this._scrollController ?? new _CustomTabBarScrollController(this);
                tabBar = new SingleChildScrollView(
                    dragStartBehavior: this.widget.dragStartBehavior,
                    scrollDirection: Axis.horizontal,
                    controller: this._scrollController,
                    child: tabBar);
            }

            return tabBar;
        }
    }

    public class CustomTabBarView : StatefulWidget {
        public CustomTabBarView(
            List<Widget> children,
            CustomTabController controller = null,
            ScrollPhysics physics = null,
            DragStartBehavior dragStartBehavior = DragStartBehavior.start,
            Key key = null
        ) : base(key: key) {
            this.children = children;
            this.controller = controller;
            this.physics = physics;
            this.dragStartBehavior = dragStartBehavior;
        }

        public readonly List<Widget> children;
        public readonly CustomTabController controller;
        public readonly ScrollPhysics physics;
        public readonly DragStartBehavior dragStartBehavior;

        public override State createState() {
            return new _CustomTabBarViewState();
        }
    }

    class _CustomTabBarViewState : State<CustomTabBarView> {
        CustomTabController _controller;
        PageController _pageController;
        List<Widget> _children;
        List<Widget> _childrenWithKey;
        int _currentIndex;
        int _warpUnderwayCount;

        bool _controllerIsValid {
            get { return this._controller?.animation != null; }
        }

        void _updateTabController() {
            CustomTabController newController = this.widget.controller;
            D.assert(() => {
                if (newController == null) {
                    throw new UIWidgetsError(
                        "No TabController for " + this.widget.GetType() + "\n" +
                        "When creating a " + this.widget.GetType() + ", you must either provide an explicit " +
                        "TabController using the \"controller\" property, or you must ensure that there " +
                        "is a DefaultTabController above the " + this.widget.GetType() + ".\n" +
                        "In this case, there was neither an explicit controller nor a default controller."
                    );
                }

                return true;
            });
            if (newController == this._controller) {
                return;
            }

            if (this._controllerIsValid) {
                this._controller.animation.removeListener(listener: this._handleTabControllerAnimationTick); 
            }

            this._controller = newController;
            this._controller?.animation.addListener(listener: this._handleTabControllerAnimationTick);
        }

        public override void initState() {
            base.initState();
            this._warpUnderwayCount = 0;
            this._updateChildren();
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            this._updateTabController();
            this._currentIndex = this._controller?.index ?? 0;
            this._pageController = new PageController(initialPage: this._currentIndex);
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget: oldWidget);
            CustomTabBarView _oldWidget = (CustomTabBarView) oldWidget;
            if (this.widget.controller != _oldWidget.controller) {
                this._updateTabController();
            }

            if (this.widget.children != _oldWidget.children && this._warpUnderwayCount == 0) {
                this._updateChildren();
            }
        }

        public override void dispose() {
            if (this._controllerIsValid) {
                this._controller.animation.removeListener(listener: this._handleTabControllerAnimationTick);
            }
            base.dispose();
        }

        void _updateChildren() {
            this._children = this.widget.children;
            this._childrenWithKey = KeyedSubtree.ensureUniqueKeysForList(items: this.widget.children);
        }

        void _handleTabControllerAnimationTick() {
            if (this._warpUnderwayCount > 0 || !this._controller.indexIsChanging) {
                return;
            }

            if (this._controller.index != this._currentIndex) {
                this._currentIndex = this._controller.index;
                this._warpToCurrentIndex();
            }
        }

        void _warpToCurrentIndex() {
            if (!this.mounted) {
                return;
            }

            if (this._pageController.page == this._currentIndex) {
                return;
            }

            int previousIndex = this._controller.previousIndex;
            if ((this._currentIndex - previousIndex).abs() == 1) {
                this._pageController.animateToPage(page: this._currentIndex, duration: CustomTabsUtils.kTabScrollDuration, 
                    curve: Curves.ease);
                return;
            }

            D.assert((this._currentIndex - previousIndex).abs() > 1);
            int initialPage = this._currentIndex > previousIndex
                ? this._currentIndex - 1
                : this._currentIndex + 1;
            List<Widget> originalChildren = this._childrenWithKey;
            this.setState(() => {
                this._warpUnderwayCount += 1;

                Widget temp = this._childrenWithKey[index: initialPage];
                this._childrenWithKey[index: initialPage] = this._childrenWithKey[index: previousIndex];
                this._childrenWithKey[index: previousIndex] = temp;
            });

            this._pageController.jumpToPage(page: initialPage);
            this._pageController.animateToPage(page: this._currentIndex, duration: CustomTabsUtils.kTabScrollDuration,
                curve: Curves.ease).Then(() => {
                if (!this.mounted) {
                    return new Promise();
                }

                this.setState(() => {
                    this._warpUnderwayCount -= 1;
                    if (this.widget.children != this._children) {
                        this._updateChildren();
                    } else {
                        this._childrenWithKey = originalChildren;
                    }
                });

                return new Promise();
            });
        }

        bool _handleScrollNotification(ScrollNotification notification) {
            if (this._warpUnderwayCount > 0) {
                return false;
            }

            if (notification.depth != 0) {
                return false;
            }

            this._warpUnderwayCount += 1;
            if (notification is ScrollUpdateNotification && !this._controller.indexIsChanging) {
                if ((this._pageController.page - this._controller.index).abs() > 1.0) {
                    this._controller.index = this._pageController.page.floor();
                    this._currentIndex = this._controller.index;
                }

                this._controller.offset = (this._pageController.page - this._controller.index).clamp(-1.0f, 1.0f);
            }
            else if (notification is ScrollEndNotification) {
                this._controller.index = this._pageController.page.round();
                this._currentIndex = this._controller.index;
            }

            this._warpUnderwayCount -= 1;

            return false;
        }

        public override Widget build(BuildContext context) {
            return new NotificationListener<ScrollNotification>(
                onNotification: this._handleScrollNotification,
                child: new PageView(
                    dragStartBehavior: this.widget.dragStartBehavior,
                    controller: this._pageController,
                    physics: this.widget.physics == null
                        ? CustomTabsUtils._kTabBarViewPhysics
                        : CustomTabsUtils._kTabBarViewPhysics.applyTo(ancestor: this.widget.physics),
                    children: this._childrenWithKey
                )
            );
        }
    }

    public class CustomTabController : ChangeNotifier {
        public CustomTabController(
            int length,
            TickerProvider vsync,
            int initialIndex = 0
        ) {
            D.assert(length >= 0);
            D.assert(initialIndex >= 0 && (length == 0 || initialIndex < length));
            D.assert(vsync != null);
            this.length = length;
            this._index = initialIndex;
            this._previousIndex = initialIndex;
            this._animationController = length < 2
                ? null
                : new AnimationController(
                    value: initialIndex,
                    upperBound: length - 1,
                    vsync: vsync
                 );
        }

        public Animation<float> animation {
            get { return this._animationController?.view ?? Animations.kAlwaysCompleteAnimation; }
        }

        readonly AnimationController _animationController;

        public readonly int length;

        void _changeIndex(int value, TimeSpan? duration = null, Curve curve = null) {
            D.assert(value >= 0 && (value < this.length || this.length == 0));
            D.assert(duration == null ? curve == null : true);
            D.assert(this._indexIsChangingCount >= 0);

            if (value == this._index || this.length < 2) {
                return;
            }

            this._previousIndex = this.index;
            this._index = value;
            if (duration != null) {
                this._indexIsChangingCount++;
                this.notifyListeners();
                this._animationController.animateTo(
                    this._index, duration: duration, curve: curve).whenCompleteOrCancel(() => {
                    this._indexIsChangingCount--;
                    this.notifyListeners();
                });
            }
            else {
                this._indexIsChangingCount++;
                this._animationController.setValue(this._index);
                this._indexIsChangingCount--;
                this.notifyListeners();
            }
        }

        public int index {
            get { return this._index; }
            set { this._changeIndex(value); }
        }

        int _index;

        public int previousIndex {
            get { return this._previousIndex; }
        }

        int _previousIndex;

        public bool indexIsChanging {
            get { return this._indexIsChangingCount != 0; }
        }

        int _indexIsChangingCount = 0;

        public void animateTo(int value, TimeSpan? duration = null, Curve curve = null) {
            this._changeIndex(value: value, duration ?? CustomTabsUtils.kTabScrollDuration, curve ?? Curves.ease);
        }

        public float offset {
            get { return this.length > 1 ? this._animationController.value - this._index : 0.0f; }
            set {
                D.assert(this.length > 1);
                D.assert(value >= -1.0f && value <= 1.0f);
                D.assert(!this.indexIsChanging);
                if (value == this.offset) {
                    return;
                }

                this._animationController.setValue(value + this._index);
            }
        }

        public override void dispose() {
            this._animationController?.dispose();
            base.dispose();
        }
    }

    public class CustomUnderlineTabIndicator : Decoration {
        public CustomUnderlineTabIndicator(
            BorderSide borderSide = null,
            EdgeInsets insets = null,
            float? width = null
        ) {
            this.borderSide = borderSide ?? new BorderSide(width: 2.0f, color: CColors.White);
            this.insets = insets ?? EdgeInsets.zero;
            this.width = width;
        }

        public readonly BorderSide borderSide;
        public readonly EdgeInsets insets;
        public readonly float? width;

        public override Decoration lerpFrom(Decoration a, float t) {
            if (a is CustomUnderlineTabIndicator _a) {
                return new CustomUnderlineTabIndicator(
                    BorderSide.lerp(a: _a.borderSide, b: this.borderSide, t: t),
                    EdgeInsets.lerp(a: _a.insets, b: this.insets, t: t)
                );
            }

            return base.lerpFrom(a: a, t: t);
        }

        public override Decoration lerpTo(Decoration b, float t) {
            if (b is CustomUnderlineTabIndicator _b) {
                return new CustomUnderlineTabIndicator(
                    BorderSide.lerp(a: this.borderSide, b: _b.borderSide, t: t),
                    EdgeInsets.lerp(a: this.insets, b: _b.insets, t: t)
                );
            }

            return base.lerpTo(b: b, t: t);
        }

        public override BoxPainter createBoxPainter(VoidCallback onChanged) {
            return new _CustomUnderlinePainter(this, onChanged: onChanged);
        }
    }

    class _CustomUnderlinePainter : BoxPainter {
        public _CustomUnderlinePainter(
            CustomUnderlineTabIndicator decoration = null,
            VoidCallback onChanged = null
        ) : base(onChanged: onChanged) {
            D.assert(decoration != null);
            this.decoration = decoration;
        }

        readonly CustomUnderlineTabIndicator decoration;

        public BorderSide borderSide {
            get { return this.decoration.borderSide; }
        }

        public EdgeInsets insets {
            get { return this.decoration.insets; }
        }

        public float? width {
            get { return this.decoration.width; }
        }

        Rect _indicatorRectFor(Rect rect) {
            D.assert(rect != null);
            Rect indicator = this.insets.deflateRect(rect: rect);
            if (this.width == null) {
                return Rect.fromLTWH(
                    left: indicator.left,
                    indicator.bottom - this.borderSide.width,
                    width: indicator.width,
                    height: this.borderSide.width
                );
            }

            float cw = (indicator.left + indicator.right) / 2;
            return Rect.fromLTWH(
                cw - (float)this.width / 2,
                indicator.bottom - this.borderSide.width,
                (float)this.width,
                height: this.borderSide.width
            );
        }

        public override void paint(Canvas canvas, Offset offset, ImageConfiguration configuration) {
            D.assert(configuration != null);
            D.assert(configuration.size != null);
            Rect rect = offset & configuration.size;
            Rect indicator = this._indicatorRectFor(rect: rect).deflate(this.borderSide.width / 2.0f);
            Paint paint = new Paint {
                color = this.borderSide.color,
                strokeWidth = 1,
                style = PaintingStyle.fill
            };
            canvas.drawRRect(RRect.fromLTRBR(indicator.bottomLeft.dx,
                indicator.bottomLeft.dy - this.borderSide.width / 2.0f,
                indicator.bottomRight.dx,
                indicator.bottomRight.dy + this.borderSide.width / 2.0f,
                this.borderSide.width / 2.0f), paint);
        }
    }

    public class CustomGradientsTabIndicator : Decoration {
        public CustomGradientsTabIndicator(
            Gradient gradient = null,
            EdgeInsets insets = null,
            float height = 2
        ) {
            this.gradient = gradient;
            this.insets = insets ?? EdgeInsets.zero;
            this.height = height;
        }

        public readonly Gradient gradient;
        public readonly EdgeInsets insets;
        public readonly float height;

        public override Decoration lerpFrom(Decoration a, float t) {
            if (a is CustomGradientsTabIndicator _a) {
                return new CustomGradientsTabIndicator(
                    Gradient.lerp(a: _a.gradient, b: this.gradient, t: t),
                    EdgeInsets.lerp(a: _a.insets, b: this.insets, t: t)
                );
            }

            return base.lerpFrom(a: a, t: t);
        }

        public override Decoration lerpTo(Decoration b, float t) {
            if (b is CustomGradientsTabIndicator _b) {
                return new CustomGradientsTabIndicator(
                    Gradient.lerp(a: this.gradient, b: _b.gradient, t: t),
                    EdgeInsets.lerp(a: this.insets, b: _b.insets, t: t)
                );
            }

            return base.lerpTo(b: b, t: t);
        }

        public override BoxPainter createBoxPainter(VoidCallback onChanged = null) {
            return new _CustomGradientsPainter(this, onChanged: onChanged);
        }
    }

    class _CustomGradientsPainter : BoxPainter {
        public _CustomGradientsPainter(
            CustomGradientsTabIndicator decoration = null,
            VoidCallback onChanged = null
        ) : base(onChanged: onChanged) {
            D.assert(decoration != null);
            this.decoration = decoration;
        }

        readonly CustomGradientsTabIndicator decoration;

        public Gradient gradient {
            get { return this.decoration.gradient; }
        }

        public EdgeInsets insets {
            get { return this.decoration.insets; }
        }

        public float height {
            get { return this.decoration.height; }
        }

        Rect _indicatorRectFor(Rect rect) {
            D.assert(rect != null);
            Rect indicator = this.insets.deflateRect(rect: rect);
            return Rect.fromLTWH(
                left: indicator.left,
                indicator.bottom - this.height,
                width: indicator.width,
                height: this.height
            );
        }

        public override void paint(Canvas canvas, Offset offset, ImageConfiguration configuration) {
            D.assert(configuration != null);
            D.assert(configuration.size != null);
            Rect rect = offset & configuration.size;
            Rect indicator = this._indicatorRectFor(rect: rect);
            Paint paint = new Paint {
                shader = this.gradient.createShader(rect: rect)
            };
            canvas.drawRRect(RRect.fromRectAndRadius(rect: indicator, 0), paint: paint);
        }
    }
}