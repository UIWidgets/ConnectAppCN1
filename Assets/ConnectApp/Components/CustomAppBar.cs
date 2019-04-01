using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.constants;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class CustomAppBar : StatelessWidget {
        public CustomAppBar(
            Key key = null,
            Widget leading = null,
            Widget title = null,
            List<Widget> actions = null,
            Color backgroundColor = null
        ) : base(key) {
            this.leading = leading;
            this.title = title;
            this.actions = actions;
            this.backgroundColor = backgroundColor;
        }

        public readonly Widget leading;
        public readonly Widget title;
        public readonly List<Widget> actions;
        public readonly Color backgroundColor;

        public override Widget build(BuildContext context) {
            const float height = 56;
            return new Container(
                height: height,
                decoration: new BoxDecoration(backgroundColor ?? CColors.White),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        leading ?? new CustomButton(
                            onPressed: () => StoreProvider.store.Dispatch(new MainNavigatorPopAction()),
                            child: new Icon(
                                Icons.arrow_back,
                                size: 24,
                                color: CColors.icon3
                            )
                        ),
                        title,
                        actions != null && actions.Count > 0
                            ? (Widget) new Row(children: actions)
                            : new Container(width: 52)
                    }
                )
            );
        }
    }
}