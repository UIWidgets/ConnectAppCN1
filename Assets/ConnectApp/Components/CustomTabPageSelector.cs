using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    static class TabsUtils {
        public const float _kTabHeight = 46.0f;
        public const float _kTextAndIconTabHeight = 72.0f;

        public static float _indexChangeProgress(TabController controller) {
            float controllerValue = controller.animation.value;
            float previousIndex = controller.previousIndex;
            float currentIndex = controller.index;

            if (!controller.indexIsChanging) {
                return (currentIndex - controllerValue).abs().clamp(0.0f, 1.0f);
            }

            return (controllerValue - currentIndex).abs() / (currentIndex - previousIndex).abs();
        }

        public static readonly PageScrollPhysics _kTabBarViewPhysics =
            (PageScrollPhysics) new PageScrollPhysics().applyTo(new ClampingScrollPhysics());
    }

    public class CustomTabPageSelector : StatelessWidget {
        public CustomTabPageSelector(
            Key key = null,
            TabController controller = null,
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

        public readonly TabController controller;

        public readonly float indicatorSize;

        public readonly Color color;

        public readonly Color selectedColor;

        Widget _buildTabIndicator(
            int tabIndex,
            TabController tabController,
            ColorTween selectedColorTween,
            ColorTween previousColorTween) {
            Color background = null;
            if (tabController.indexIsChanging) {
                float t = 1.0f - TabsUtils._indexChangeProgress(tabController);
                if (tabController.index == tabIndex) {
                    background = selectedColorTween.lerp(t);
                }
                else if (tabController.previousIndex == tabIndex) {
                    background = previousColorTween.lerp(t);
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
                    background = selectedColorTween.lerp(offset);
                }
                else if (tabController.index == tabIndex + 1 && offset < 0.0) {
                    background = selectedColorTween.lerp(-offset);
                }
                else {
                    background = selectedColorTween.begin;
                }
            }

            return new TabPageSelectorIndicator(
                backgroundColor: background,
                borderColor: background,
                size: this.indicatorSize
            );
        }

        public override Widget build(BuildContext context) {
            Color fixColor = this.color ?? Colors.transparent;
            Color fixSelectedColor = this.selectedColor ?? Theme.of(context).accentColor;
            ColorTween selectedColorTween = new ColorTween(begin: fixColor, end: fixSelectedColor);
            ColorTween previousColorTween = new ColorTween(begin: fixSelectedColor, end: fixColor);
            TabController tabController = this.controller ?? DefaultTabController.of(context);
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
                builder: (BuildContext subContext, Widget child) => {
                    List<Widget> children = new List<Widget>();

                    for (int tabIndex = 0; tabIndex < tabController.length; tabIndex++) {
                        children.Add(this._buildTabIndicator(
                            tabIndex,
                            tabController,
                            selectedColorTween,
                            previousColorTween)
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
}