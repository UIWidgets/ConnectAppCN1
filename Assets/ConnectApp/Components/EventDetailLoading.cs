using System;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class EventDetailLoading : StatelessWidget {
        public EventDetailLoading(
            EventType eventType = EventType.offline,
            Action mainRouterPop = null,
            Key key = null
        ) : base(key) {
            this.eventType = eventType;
            this.mainRouterPop = mainRouterPop;
        }

        private readonly EventType eventType;
        private readonly Action mainRouterPop;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new Stack(
                            children: new List<Widget> {
                                new AspectRatio(
                                    aspectRatio: 16.0f / 9.0f,
                                    child: new Container(
                                        color: new Color(0xFFD8D8D8)
                                    )
                                ),
                                new Positioned(
                                    left: 0,
                                    top: 0,
                                    right: 0,
                                    child: new Container(
                                        height: 44,
                                        padding: EdgeInsets.symmetric(horizontal: 8),
                                        decoration: new BoxDecoration(
                                            gradient: new LinearGradient(
                                                colors: new List<Color> {
                                                    new Color(0x80000000),
                                                    new Color(0x0)
                                                },
                                                begin: Alignment.topCenter,
                                                end: Alignment.bottomCenter
                                            )
                                        ),
                                        child: new Row(
                                            children: new List<Widget> {
                                                new CustomButton(
                                                    onPressed: () => mainRouterPop(),
                                                    child: new Icon(
                                                        Icons.arrow_back,
                                                        size: 28,
                                                        color: CColors.White
                                                    )
                                                )
                                            }
                                        )
                                    )
                                )
                            }
                        ),
                        new Container(
                            height: 12,
                            margin: EdgeInsets.only(16, 40, 16, 24),
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Container(
                            height: 12,
                            margin: EdgeInsets.only(16, 0, 132, 40),
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Container(
                            height: 6,
                            margin: EdgeInsets.only(16, 0, 16, 24),
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Container(
                            height: 6,
                            margin: EdgeInsets.only(16, 0, 16, 24),
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Container(
                            height: 6,
                            margin: EdgeInsets.only(16, 0, 16, 24),
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Container(
                            height: 6,
                            margin: EdgeInsets.only(16, 0, 132),
                            color: new Color(0xFFF8F8F8)
                        )
                    }
                )
            );
        }
    }
}