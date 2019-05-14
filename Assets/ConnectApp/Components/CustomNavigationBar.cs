using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class CustomNavigationBar : StatelessWidget {
        public CustomNavigationBar(
            Widget leftWidget,
            List<Widget> rightWidgets,
            Color backgroundColor,
            float offset,
            Key key = null
        ) : base(key) {
            this.leftWidget = leftWidget;
            this.rightWidgets = rightWidgets;
            this.backgroundColor = backgroundColor;
            this.offset = offset;
        }

        readonly Widget leftWidget;
        readonly List<Widget> rightWidgets;
        readonly Color backgroundColor;
        readonly float offset;
        public static readonly float height = 96;

        public override Widget build(BuildContext context) {
            return new Container(
                color: this.backgroundColor,
                height: height - this.offset,
                child: new Container(
                    height: 52,
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: new List<Widget> {
                            new Padding(
                                padding: EdgeInsets.only(bottom: 8, left: 16, right: 16),
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    children: new List<Widget> {
                                        new Container(
                                            child: this.leftWidget
                                        ),
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
                )
            );
        }
    }
}