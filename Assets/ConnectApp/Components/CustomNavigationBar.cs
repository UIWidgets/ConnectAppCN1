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

        private readonly Widget leftWidget;
        private readonly List<Widget> rightWidgets;
        private readonly Color backgroundColor;
        private readonly float offset;
        private readonly float height = 140;


        public override Widget build(BuildContext context) {
            return new Container(
                color: backgroundColor,
                height: height - offset,
                child: new Container(
                    height: 52,
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: new List<Widget> {
                            new Padding(
                                padding: EdgeInsets.only(bottom: 12, left: 16, right: 16),
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    children: new List<Widget> {
                                        new Container(
                                            child: leftWidget
                                        ),
                                        new Container(
                                            height: 40,
                                            child: new Row(
                                                mainAxisAlignment: MainAxisAlignment.end,
                                                crossAxisAlignment: CrossAxisAlignment.center,
                                                children: rightWidgets
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