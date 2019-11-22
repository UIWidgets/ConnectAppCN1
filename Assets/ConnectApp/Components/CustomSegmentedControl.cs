using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using Transform = Unity.UIWidgets.widgets.Transform;

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

    class _CustomSegmentedControlState : State<CustomSegmentedControl> {
        PageController _pageController;
        int _selectedIndex;
        float _selectorWidth;
        float _selectorOffset;

        float _selectorOriginalWidth {
            get { return 1.0f / this.widget.items.Count; }
        }

        public override void initState() {
            base.initState();
            this._selectedIndex = this.widget.currentIndex;
            this._pageController = new PageController(initialPage: this._selectedIndex);
            this._selectorWidth = this._selectorOriginalWidth;
            this._selectorOffset = 0;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this._pageController.position.addListener(() => {
                    this.setState(() => {
                        this._selectorWidth = this._selectorOriginalWidth *
                                              ((this._pageController.page - this._pageController.page.round())
                                               .abs() * 2 + 1);
                        this._selectorOffset = this._pageController.page.round() == this._pageController.page.floor()
                            ? this._selectorOriginalWidth * this._pageController.page.floor()
                            : this._selectorOriginalWidth * (this._pageController.page.ceil() + 1) - this._selectorWidth;
                    });
                });
            });
        }

        public override void dispose() {
            this._pageController.dispose();
            base.dispose();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget: oldWidget);
            if (oldWidget is CustomSegmentedControl customSegmentedControl) {
                if (this.widget.currentIndex != customSegmentedControl.currentIndex) {
                    this.setState(() => this._selectedIndex = this.widget.currentIndex);
                }
            }
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
            return new Stack(
                children: new List<Widget> {
                    new Container(
                        decoration: new BoxDecoration(
                            color: CColors.White,
                            border: new Border(bottom: new BorderSide(color: CColors.Separator2))
                        ),
                        height: 44,
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.start,
                            mainAxisSize: MainAxisSize.min,
                            children: children
                        )
                    ),
                    new Positioned(
                        left: 0,
                        right: 0,
                        bottom: 0,
                        child: new Container(
                            height: 2,
                            child: new FractionalTranslation(
                                translation: new Offset(this._selectorOffset, 0),
                                child: new FractionallySizedBox(
                                    alignment: Alignment.centerLeft,
                                    widthFactor: this._selectorWidth,
                                    child: new Container(color: this.widget.selectedColor)
                                )
                            )
                        )
                    )
                }
            );
        }

        Widget _buildContentView() {
            return new Flexible(
                child: new Container(
                    child: new PageView(
                        physics: new BouncingScrollPhysics(),
                        controller: this._pageController,
                        onPageChanged: index => {
                            if (this._selectedIndex != index) {
                                this.setState(() => this._selectedIndex = index);
                                if (this.widget.onValueChanged != null) {
                                    this.widget.onValueChanged(value: index);
                                }
                            }
                        },
                        itemBuilder: (cxt, index) => this.widget.children[index: index],
                        itemCount: this.widget.children.Count
                    )
                )
            );
        }

        Widget _buildSelectItem(string title, int index) {
            var textColor = this.widget.unselectedColor;
            var fontFamily = "Roboto-Regular";
//            Widget lineView = new Positioned(
//                bottom: 0,
//                left: 0,
//                right: 0,
//                child: new Container(height: 2)
//            );
//            if (index == this._selectedIndex) {
//                textColor = this.widget.selectedColor;
//                fontFamily = "Roboto-Medium";
//                lineView = new Positioned(
//                    bottom: 0,
//                    left: 0,
//                    right: 0,
//                    child: new Container(
//                        height: 2,
//                        color: this.widget.selectedColor
//                    )
//                );
//            }

            return new CustomButton(
                onPressed: () => {
                    if (this._selectedIndex != index) {
                        this.setState(() => {
                            this._pageController.animateToPage(
                                page: index,
                                TimeSpan.FromMilliseconds(this.widget.items.Count > 2 ? 1 : 250),
                                curve: Curves.ease
                            );
                            this._selectedIndex = index;
                        });
                        if (this.widget.onValueChanged != null) {
                            this.widget.onValueChanged(value: index);
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
                                    data: title,
                                    style: new TextStyle(
                                        fontSize: 16,
                                        fontFamily: fontFamily,
                                        color: textColor
                                    )
                                )
                            )
                            // lineView
                        }
                    )
                )
            );
        }
    }
}