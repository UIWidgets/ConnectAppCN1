using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Main;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public static class CustomAppBarUtil {
        public const float appBarHeight = 44;
    }

    public class CustomAppBar : StatelessWidget {
        public CustomAppBar(
            GestureTapCallback onBack = null,
            Widget title = null,
            Widget rightWidget = null,
            Color bottomColor = null,
            float height = CustomAppBarUtil.appBarHeight,
            Key key = null
        ) : base(key: key) {
            this.onBack = onBack;
            this.title = title;
            this.bottomColor = bottomColor ?? CColors.Separator2;
            this.height = height;
            this.rightWidget = rightWidget ?? new Container(width: 56);
        }

        readonly GestureTapCallback onBack;
        readonly Widget title;
        readonly Widget rightWidget;
        readonly Color bottomColor;
        readonly float height;

        public override Widget build(BuildContext context) {
            return new Container(
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: new Border(
                        bottom: new BorderSide(
                            color: this.bottomColor
                        )
                    )
                ),
                height: this.height,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        this._buildLeftWidget(context: context),
                        this._buildTitle(),
                        this.rightWidget
                    }
                )
            );
        }

        Widget _buildLeftWidget(BuildContext context) {
            ModalRoute parentRoute = ModalRoute.of(context: context);
            bool canPop = parentRoute?.canPop ?? false;
            if (canPop) {
                return new CustomButton(
                    padding: EdgeInsets.only(16, 10, 0, 10),
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
            return new Container(width: 56);
        }

        Widget _buildTitle() {
            if (this.title == null) {
                return new Container();
            }
            return this.title;
        }
    }
}