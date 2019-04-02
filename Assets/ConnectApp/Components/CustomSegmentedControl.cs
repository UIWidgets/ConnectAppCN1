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

    internal class _CustomSegmentedControlState : State<CustomSegmentedControl> {
        private int _selectedIndex;

        public override void initState() {
            base.initState();
            _selectedIndex = widget.currentIndex;
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            if (oldWidget is CustomSegmentedControl) {
                var customSegmentedControl = (CustomSegmentedControl) oldWidget;
                if (widget.currentIndex != customSegmentedControl.currentIndex)
                    setState(() => _selectedIndex = widget.currentIndex);
            }
        }

        public override Widget build(BuildContext context) {
            return new Container(
                child: new Container(
                    height: 44,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: _buildChildren()
                    )
                )
            );
        }

        private List<Widget> _buildChildren() {
            var widgets = new List<Widget>();
            widget.items.ForEach(item => {
                var itemIndex = widget.items.IndexOf(item);
                var itemWidget = _buildSelectItem(item, itemIndex);
                widgets.Add(itemWidget);
            });
            return widgets;
        }

        private Widget _buildSelectItem(string title, int index) {
            var textColor = widget.unselectedColor;
            Widget lineView = new Positioned(new Container());
            if (index == _selectedIndex) {
                textColor = widget.selectedColor;
                lineView = new Positioned(
                    bottom: 0,
                    left: 0,
                    right: 0,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: new List<Widget> {
                            new Container(
                                width: 80,
                                height: 2,
                                color: widget.selectedColor
                            )
                        }
                    )
                );
            }

            return new Expanded(
                child: new Stack(
                    fit: StackFit.expand,
                    children: new List<Widget> {
                        new CustomButton(
                            onPressed: () => {
                                if (_selectedIndex != index) {
                                    if (widget.onValueChanged != null)
                                        widget.onValueChanged(index);
                                    else
                                        setState(() => _selectedIndex = index);
                                }
                            },
                            child: new Container(
                                alignment: Alignment.center,
                                child: new Text(
                                    title,
                                    style: new TextStyle(
                                        fontSize: 16,
                                        fontFamily: "Roboto-Regular",
                                        color: textColor
                                    )
                                )
                            )
                        ),
                        lineView
                    }
                )
            );
        }
    }
}