using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using EventType = ConnectApp.Models.State.EventType;

namespace ConnectApp.Components {
    public class EventDetailLoading : StatelessWidget {
        public EventDetailLoading(
            EventType eventType = EventType.offline,
            Action mainRouterPop = null,
            Key key = null
        ) : base(key) {
            this.eventType = eventType;
            this.mainRouterPop = mainRouterPop;
        }

        readonly EventType eventType;
        readonly Action mainRouterPop;

        public override Widget build(BuildContext context) {
            var paddingTop = 0f;
            var aspectRatio = 16.0f / 9;
            var safeTop = true;
            if (this.eventType == EventType.offline && Application.platform != RuntimePlatform.Android) {
                paddingTop = MediaQuery.of(context).padding.top;
                aspectRatio = 3f / 2;
                safeTop = false;
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    top: safeTop,
                    child: new Container(
                        color: CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                new Stack(
                                    children: new List<Widget> {
                                        new AspectRatio(
                                            aspectRatio: aspectRatio,
                                            child: new Container(
                                                color: new Color(0xFFD8D8D8)
                                            )
                                        ),
                                        new Positioned(
                                            left: 0,
                                            top: 0,
                                            right: 0,
                                            child: new Container(
                                                height: 44 + paddingTop,
                                                padding: EdgeInsets.only(left: 8, top: paddingTop, right: 8),
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
                                                            onPressed: () => this.mainRouterPop(),
                                                            child: new Icon(
                                                                Icons.arrow_back,
                                                                size: 24,
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
                    )
                )
            );
        }
    }
}