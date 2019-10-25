using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Main;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class CustomNavigationBar : StatelessWidget {
        public CustomNavigationBar(
            Widget titleWidget = null,
            List<Widget> rightWidgets = null,
            Widget topRightWidget = null,
            Decoration decoration = null,
            float offset = 0,
            EdgeInsets padding = null,
            GestureTapCallback onBack = null,
            Key key = null
        ) : base(key: key) {
            this.titleWidget = titleWidget;
            this.rightWidgets = rightWidgets;
            this.topRightWidget = topRightWidget;
            this.decoration = decoration ?? new BoxDecoration(color: CColors.White);
            this.offset = offset;
            this.padding = padding ?? EdgeInsets.only(bottom: 8, left: 16, right: 16);
            this.onBack = onBack;
        }

        readonly Widget titleWidget;
        readonly List<Widget> rightWidgets;
        readonly Widget topRightWidget;
        readonly Decoration decoration;
        readonly float offset;
        readonly EdgeInsets padding;
        readonly GestureTapCallback onBack;
        public const float height = 96;

        public override Widget build(BuildContext context) {
            return new Container(
                decoration: this.decoration,
                height: height - this.offset,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget> {
                                this._buildBackWidget(context: context),
                                this.topRightWidget ?? new Container()
                            }
                        ),
                        new Padding(
                            padding: this.padding,
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: new List<Widget> {
                                    this.titleWidget ?? new Container(),
                                    new Container(
                                        height: 44,
                                        child: new Row(
                                            mainAxisAlignment: MainAxisAlignment.end,
                                            crossAxisAlignment: CrossAxisAlignment.center,
                                            children: this.rightWidgets
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }

        Widget _buildBackWidget(BuildContext context) {
            ModalRoute parentRoute = ModalRoute.of(context: context);
            bool canPop = parentRoute?.canPop ?? false;
            if (canPop) {
                return new CustomButton(
                    padding: EdgeInsets.symmetric(8, 16),
                    onPressed: () => {
                        if (this.onBack != null) {
                            this.onBack();
                        }
                        else {
                            if (Router.navigator.canPop()) {
                                Router.navigator.pop();
                            }
                        }
                    },
                    child: new Icon(
                        icon: Icons.arrow_back,
                        size: 24,
                        color: CColors.Icon
                    )
                );
            }

            return new Container();
        }
    }
}