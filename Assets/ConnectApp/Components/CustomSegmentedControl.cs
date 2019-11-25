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
        public readonly int currentIndex;

        public readonly List<string> items;
        public readonly ValueChanged<int> onValueChanged;
        public readonly Color selectedColor;
        public readonly Color unselectedColor;

        public CustomSegmentedControl(
            List<string> items,
            List<Widget> children,
            ValueChanged<int> onValueChanged = null,
            int currentIndex = 0,
            Color unselectedColor = null,
            Color selectedColor = null,
            Key key = null
        ) : base(key: key) {
            D.assert(items != null);
            D.assert(items.Count >= 2);
            D.assert(children != null);
            D.assert(children.Count >= 2);
            D.assert(children.Count == items.Count);
            D.assert(currentIndex < items.Count);
            this.items = items;
            this.children = children;
            this.onValueChanged = onValueChanged;
            this.currentIndex = currentIndex;
            this.unselectedColor = unselectedColor ?? CColors.TextTitle;
            this.selectedColor = selectedColor ?? CColors.PrimaryBlue;
        }

        public override State createState() {
            return new _CustomSegmentedControlState();
        }
    }

    class _CustomSegmentedControlState : TickerProviderStateMixin<CustomSegmentedControl> {
        static readonly TextStyle _labelStyle = new TextStyle(
            fontSize: 16,
            fontFamily: "Roboto-Regular"
        );

        CustomTabController _controller;

        public override void initState() {
            base.initState();
            this._controller = new CustomTabController(2, this);
            this._controller.addListener(() => { this.widget.onValueChanged(value: this._controller.index); });
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
                var itemIndex = this.widget.items.IndexOf(item: item);
                var itemWidget = this._buildSelectItem(title: item, index: itemIndex);
                children.Add(item: itemWidget);
            });
            return new CustomTabBarHeader(
                tabs: children,
                controller: this._controller,
                indicator: new BoxDecoration(
                    border: new Border(bottom: new BorderSide(color: this.widget.selectedColor, 2))),
                indicatorSize: CustomTabBarIndicatorSize.label,
                indicatorChangeStyle: CustomTabBarIndicatorChangeStyle.enlarge,
                unselectedLabelStyle: _labelStyle,
                unselectedLabelColor: this.widget.unselectedColor,
                labelStyle: _labelStyle,
                labelColor: this.widget.unselectedColor,
                isScrollable: true
            );
        }

        Widget _buildContentView() {
            return new Flexible(
                child: new CustomTabBarView(
                    children: this.widget.children,
                    controller: this._controller
                )
            );
        }

        Widget _buildSelectItem(string title, int index) {
            return new CustomTab(
                child: new Container(
                    height: 44,
                    padding: EdgeInsets.symmetric(10),
                    child: new Text(
                        data: title,
                        style: _labelStyle
                    )
                )
            );
        }
    }
}