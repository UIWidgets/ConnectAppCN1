using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class CustomTabPageSelector : StatelessWidget {
        public CustomTabPageSelector(
            Key key = null,
            CustomTabController controller = null,
            float indicatorSize = 12.0f,
            Color color = null,
            Color selectedColor = null
        ) : base(key: key) {
            D.assert(indicatorSize > 0.0f);
            this.controller = controller;
            this.indicatorSize = indicatorSize;
            this.color = color;
            this.selectedColor = selectedColor;
        }

        readonly CustomTabController controller;
        readonly float indicatorSize;
        readonly Color color;
        readonly Color selectedColor;

        Widget _buildTabIndicator(
            int tabIndex,
            CustomTabController tabController,
            ColorTween selectedColorTween,
            ColorTween previousColorTween) {
            Color background;
            if (tabController.indexIsChanging) {
                float t = 1.0f - CustomTabsUtils._indexChangeProgress(controller: tabController);
                if (tabController.index == tabIndex) {
                    background = selectedColorTween.lerp(t: t);
                }
                else if (tabController.previousIndex == tabIndex) {
                    background = previousColorTween.lerp(t: t);
                }
                else {
                    background = selectedColorTween.begin;
                }
            }
            else {
                float offset = tabController.offset;
                if (tabController.index == tabIndex) {
                    background = selectedColorTween.lerp(1.0f - offset.abs());
                }
                else if (tabController.index == tabIndex - 1 && offset > 0.0) {
                    background = selectedColorTween.lerp(t: offset);
                }
                else if (tabController.index == tabIndex + 1 && offset < 0.0) {
                    background = selectedColorTween.lerp(t: -offset);
                }
                else {
                    background = selectedColorTween.begin;
                }
            }

            return new CustomTabPageSelectorIndicator(
                backgroundColor: background,
                borderColor: background,
                size: this.indicatorSize
            );
        }

        public override Widget build(BuildContext context) {
            Color fixColor = this.color ?? CColors.Transparent;
            Color fixSelectedColor = this.selectedColor ?? CColors.PrimaryBlue;
            ColorTween selectedColorTween = new ColorTween(begin: fixColor, end: fixSelectedColor);
            ColorTween previousColorTween = new ColorTween(begin: fixSelectedColor, end: fixColor);
            CustomTabController tabController = this.controller;
            D.assert(() => {
                if (tabController == null) {
                    throw new UIWidgetsError(
                        "No TabController for " + this.GetType() + ".\n" +
                        "When creating a " + this.GetType() + ", you must either provide an explicit TabController " +
                        "using the \"controller\" property, or you must ensure that there is a " +
                        "DefaultTabController above the " + this.GetType() + ".\n" +
                        "In this case, there was neither an explicit controller nor a default controller."
                    );
                }

                return true;
            });

            Animation<float> animation = new CurvedAnimation(
                parent: tabController.animation,
                curve: Curves.fastOutSlowIn
            );

            return new AnimatedBuilder(
                animation: animation,
                builder: (subContext, child) => {
                    List<Widget> children = new List<Widget>();

                    for (int tabIndex = 0; tabIndex < tabController.length; tabIndex++) {
                        children.Add(this._buildTabIndicator(
                            tabIndex: tabIndex,
                            tabController: tabController,
                            selectedColorTween: selectedColorTween,
                            previousColorTween: previousColorTween)
                        );
                    }

                    return new Row(
                        mainAxisSize: MainAxisSize.min,
                        children: children
                    );
                }
            );
        }
    }

    public class CustomTabPageSelectorIndicator : StatelessWidget {
        public CustomTabPageSelectorIndicator(
            Color backgroundColor,
            Color borderColor,
            float size,
            Key key = null
        ) : base(key: key) {
            D.assert(backgroundColor != null);
            D.assert(borderColor != null);
            this.backgroundColor = backgroundColor;
            this.borderColor = borderColor;
            this.size = size;
        }

        readonly Color backgroundColor;
        readonly Color borderColor;
        readonly float size;

        public override Widget build(BuildContext context) {
            return new Container(
                width: this.size,
                height: this.size,
                margin: EdgeInsets.all(4.0f),
                decoration: new BoxDecoration(
                    color: this.backgroundColor,
                    border: Border.all(color: this.borderColor),
                    shape: BoxShape.circle
                )
            );
        }
    }
}