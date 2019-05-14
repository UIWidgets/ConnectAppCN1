using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class CustomSegmentedControl : StatefulWidget {
        public CustomSegmentedControl(
            List<string> items,
            ValueChanged<int> onValueChanged,
            int currentIndex = 0,
            Color unselectedColor = null,
            Color selectedColor = null,
            Key key = null
        ) : base(key) {
            D.assert(items != null);
            D.assert(items.Count >= 2);
            D.assert(currentIndex < items.Count);
            this.items = items;
            this.onValueChanged = onValueChanged;
            this.currentIndex = currentIndex;
            this.unselectedColor = unselectedColor ?? CColors.TextTitle;
            this.selectedColor = selectedColor ?? CColors.PrimaryBlue;
        }

        public readonly List<string> items;
        public readonly ValueChanged<int> onValueChanged;
        public readonly int currentIndex;
        public readonly Color unselectedColor;
        public readonly Color selectedColor;

        public override State createState() {
            return new _CustomSegmentedControlState();
        }
    }

    class _CustomSegmentedControlState : State<CustomSegmentedControl> {
        int _selectedIndex;

        public override void initState() {
            base.initState();
            this._selectedIndex = this.widget.currentIndex;
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            if (oldWidget is CustomSegmentedControl) {
                var customSegmentedControl = (CustomSegmentedControl) oldWidget;
                if (this.widget.currentIndex != customSegmentedControl.currentIndex) {
                    this.setState(() => this._selectedIndex = this.widget.currentIndex);
                }
            }
        }

        public override Widget build(BuildContext context) {
            return new Container(
                child: new Container(
                    decoration: new BoxDecoration(
                        border: new Border(bottom: new BorderSide(CColors.Separator2))
                    ),
                    height: 44,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: this._buildChildren()
                    )
                )
            );
        }

        List<Widget> _buildChildren() {
            var widgets = new List<Widget>();
            this.widget.items.ForEach(item => {
                var itemIndex = this.widget.items.IndexOf(item);
                var itemWidget = this._buildSelectItem(item, itemIndex);
                widgets.Add(itemWidget);
            });
            return widgets;
        }

        Widget _buildSelectItem(string title, int index) {
            var textColor = this.widget.unselectedColor;
            var fontFamily = "Roboto-Regular";
            Widget lineView = new Positioned(
                bottom: 0,
                left: 0,
                right: 0,
                child: new Container(height: 2)
            );
            if (index == this._selectedIndex) {
                textColor = this.widget.selectedColor;
                fontFamily = "Roboto-Medium";
                lineView = new Positioned(
                    bottom: 0,
                    left: 0,
                    right: 0,
                    child: new Container(
                        height: 2,
                        color: this.widget.selectedColor
                    )
                );
            }

            return new CustomButton(
                onPressed: () => {
                    if (this._selectedIndex != index) {
                        if (this.widget.onValueChanged != null) {
                            this.widget.onValueChanged(index);
                        }
                        else {
                            this.setState(() => this._selectedIndex = index);
                        }
                    }
                },
                padding: EdgeInsets.zero,
                child: new Container(
                    height: 44,
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    child: new Stack(
                        children: new List<Widget> {
                            new Container(
                                padding: EdgeInsets.symmetric(10),
                                child: new Text(
                                    title,
                                    style: new TextStyle(
                                        fontSize: 16,
                                        fontFamily: fontFamily,
                                        color: textColor
                                    )
                                )
                            ),
                            lineView
                        }
                    )
                )
            );
        }
    }
}