using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components
{
    public class CustomAppBar : StatelessWidget
    {
        public CustomAppBar(
            Key key = null,
            Widget leading = null,
            Widget title = null,
            List<Widget> actions = null,
            Color backgroundColor = null
        ) : base(key)
        {
            this.leading = leading;
            this.title = title;
            this.actions = actions;
            this.backgroundColor = backgroundColor;
        }

        public readonly Widget leading;
        public readonly Widget title;
        public readonly List<Widget> actions;
        public readonly Color backgroundColor;

        public override Widget build(BuildContext context)
        {
            const double height = 56.0;
            return new Container(
                height: height,
                decoration: new BoxDecoration(backgroundColor ?? CColors.background1
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget>
                    {
                        leading ?? new CustomButton(
                            onPressed: () => Navigator.pop(context),
                            child: new Icon(
                                Icons.arrow_back,
                                size: 28,
                                color: CColors.icon1
                            )
                        ),
                        title,
                        actions != null && actions.Count > 0
                            ? (Widget) new Row(children: actions)
                            : new Container()
                    }
                )
            );
        }
    }
}