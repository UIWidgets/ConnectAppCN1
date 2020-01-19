using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class CustomSegmentedControl : StatefulWidget {
        public readonly List<Widget> children;
        public readonly List<object> items;
        public readonly ValueChanged<int> onValueChanged;
        public readonly int currentIndex;
        public readonly float headerHeight;
        public readonly Widget trailing;
        public readonly Decoration headerDecoration;
        public readonly Decoration indicator;
        public readonly EdgeInsets headerPadding;
        public readonly EdgeInsets labelPadding;
        public readonly float? indicatorWidth;
        public readonly Color selectedColor;
        public readonly Color unselectedColor;
        public readonly TextStyle unselectedTextStyle;
        public readonly TextStyle selectedTextStyle;
        public readonly CustomTabController controller;
        public readonly ScrollPhysics physics;
        public readonly ValueChanged<int> onTap;

        public CustomSegmentedControl(
            List<object> items,
            List<Widget> children,
            ValueChanged<int> onValueChanged = null,
            int currentIndex = 0,
            float headerHeight = 44,
            Widget trailing = null,
            Decoration headerDecoration = null,
            Decoration indicator = null,
            EdgeInsets headerPadding = null,
            EdgeInsets labelPadding = null,
            float? indicatorWidth = null,
            Color unselectedColor = null,
            Color selectedColor = null,
            TextStyle unselectedTextStyle = null,
            TextStyle selectedTextStyle = null,
            CustomTabController controller = null,
            ScrollPhysics physics = null,
            ValueChanged<int> onTap = null,
            Key key = null
        ) : base(key: key) {
            D.assert(items != null);
            D.assert(items.Count >= 2);
            D.assert(children != null);
            D.assert(children.Count >= 2);
            D.assert(children.Count == items.Count);
            D.assert(currentIndex < children.Count);
            this.items = items;
            this.children = children;
            this.onValueChanged = onValueChanged;
            this.unselectedColor = unselectedColor ?? CColors.TextBody4;
            this.selectedColor = selectedColor ?? CColors.TextTitle;
            this.currentIndex = currentIndex;
            this.headerHeight = headerHeight;
            this.trailing = trailing;
            this.headerDecoration = headerDecoration ?? new BoxDecoration(
                                        color: CColors.White,
                                        border: new Border(bottom: new BorderSide(color: CColors.Separator2))
                                    );
            this.indicator = indicator ?? new CustomGradientsTabIndicator(
                                 insets: EdgeInsets.zero,
                                 height: 8,
                                 gradient: new LinearGradient(
                                     begin: Alignment.centerLeft,
                                     end: Alignment.centerRight,
                                     new List<Color> {
                                         new Color(0xFFB1E0FF),
                                         new Color(0xFF6EC6FF)
                                     }
                                 )
                             );
            this.headerPadding = headerPadding ?? EdgeInsets.only(bottom: 10);
            this.labelPadding = labelPadding ?? EdgeInsets.symmetric(horizontal: 16);
            this.indicatorWidth = indicatorWidth;
            this.unselectedTextStyle = unselectedTextStyle ?? new TextStyle(
                                           fontSize: 16,
                                           fontFamily: "Roboto-Regular",
                                           color: this.unselectedColor
                                       );
            this.selectedTextStyle = selectedTextStyle ?? new TextStyle(
                                         fontSize: 16,
                                         fontFamily: "Roboto-Medium",
                                         color: this.unselectedColor
                                     );
            this.controller = controller;
            this.physics = physics;
            this.onTap = onTap;
        }

        public override State createState() {
            return new _CustomSegmentedControlState();
        }
    }

    class _CustomSegmentedControlState : TickerProviderStateMixin<CustomSegmentedControl> {
        CustomTabController _controller;
        int _currentIndex;

        public override void initState() {
            base.initState();
            this._currentIndex = this.widget.currentIndex;
            this._controller = this.widget.controller
                               ?? new CustomTabController(length: this.widget.children.Count, this);
            this._controller.addListener(() => {
                if (this._controller.index != this._currentIndex) {
                    this._currentIndex = this._controller.index;
                    this.widget.onValueChanged?.Invoke(value: this._currentIndex);
                }
            });
            this._controller.index = this.widget.currentIndex;
        }

        public override void dispose() {
            this._controller.dispose();
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            return new Container(
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        this._buildSelectView(),
                        this._buildContentView()
                    }
                )
            );
        }

        Widget _buildSelectView() {
            var children = new List<Widget>();
            this.widget.items.ForEach(item => {
                if (item is string itemString) {
                    children.Add(new Text(data: itemString));
                }

                if (item is int itemInt) {
                    children.Add(new Text($"{itemInt}"));
                }

                if (item is Widget widget) {
                    children.Add(item: widget);
                }
            });
            return new Container(
                decoration: this.widget.headerDecoration,
                height: this.widget.headerHeight,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.end,
                    children: new List<Widget> {
                        new Container(
                            padding: this.widget.headerPadding,
                            child: new CustomTabBarHeader(
                                tabs: children,
                                controller: this._controller,
                                indicator: this.widget.indicator,
                                indicatorSize: CustomTabBarIndicatorSize.fixedOrLabel,
                                indicatorFixedSize: this.widget.indicatorWidth,
                                indicatorPadding: EdgeInsets.zero,
                                indicatorChangeStyle: CustomTabBarIndicatorChangeStyle.enlarge,
                                unselectedLabelStyle: this.widget.unselectedTextStyle,
                                unselectedLabelColor: this.widget.unselectedColor,
                                labelStyle: this.widget.selectedTextStyle,
                                labelColor: this.widget.selectedColor,
                                isScrollable: true,
                                labelPadding: this.widget.labelPadding,
                                onTap: this.widget.onTap
                            )
                        ),
                        this.widget.trailing ?? new Container()
                    }
                )
            );
        }

        Widget _buildContentView() {
            return new Flexible(
                child: new CustomTabBarView(
                    physics: this.widget.physics,
                    children: this.widget.children,
                    controller: this._controller
                )
            );
        }
    }
}