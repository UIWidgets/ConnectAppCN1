using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class EventCountDown : AnimatedWidget {
        public EventCountDown(
            Animation<int> animation,
            Key key = null
        ) : base(key, animation) {
            this.animation = animation;
        }

        readonly Animation<int> animation;

        protected override Widget build(BuildContext context) {
            int seconds = this.animation.value;
            int minutes = seconds / 60;

            int minutesOne = 0;
            if (minutes >= 10) {
                minutesOne = minutes / 10;
            }

            int minutesTwo = minutes - minutesOne * 10;

            int lastSeconds = seconds - minutes * 60;
            int lastSecondsOne = 0;
            if (lastSeconds >= 10) {
                lastSecondsOne = lastSeconds / 10;
            }

            int lastSecondsTwo = lastSeconds - lastSecondsOne * 10;

            return Positioned.fill(
                new Container(
                    alignment: Alignment.center,
                    color: Color.fromRGBO(0, 0, 0, 0.4f),
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        mainAxisSize: MainAxisSize.min,
                        children: new List<Widget> {
                            new Column(
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: new List<Widget> {
                                    new Row(
                                        children: new List<Widget> {
                                            new Container(
                                                margin: EdgeInsets.only(right: 4),
                                                child: _buildCountView($"{minutesOne}")
                                            ),
                                            _buildCountView($"{minutesTwo}")
                                        }
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(top: 8),
                                        child: new Text(
                                            "分钟",
                                            style: CTextStyle.CaptionWhite
                                        )
                                    )
                                }
                            ),
                            new Container(
                                margin: EdgeInsets.symmetric(horizontal: 4),
                                height: 50,
                                child: new Text(
                                    ":",
                                    style: new TextStyle(
                                        color: CColors.White,
                                        fontFamily: "Roboto-Bold",
                                        fontSize: 20
                                    )
                                )
                            ),
                            new Column(
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: new List<Widget> {
                                    new Row(
                                        children: new List<Widget> {
                                            new Container(
                                                margin: EdgeInsets.only(right: 4),
                                                child: _buildCountView($"{lastSecondsOne}")
                                            ),
                                            _buildCountView($"{lastSecondsTwo}")
                                        }
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(top: 8),
                                        child: new Text(
                                            "秒",
                                            style: CTextStyle.CaptionWhite
                                        )
                                    )
                                }
                            )
                        }
                    )
                )
            );
        }

        static Widget _buildCountView(string num) {
            return new Container(
                width: 40,
                height: 60,
                alignment: Alignment.center,
                decoration: new BoxDecoration(
                    CColors.PrimaryBlue,
                    borderRadius: BorderRadius.all(4)
                ),
                child: new Stack(
                    fit: StackFit.expand,
                    children: new List<Widget> {
                        new Container(
                            width: 40,
                            height: 60,
                            alignment: Alignment.center,
                            child: new Text(
                                num,
                                style: new TextStyle(
                                    fontSize: 32,
                                    fontFamily: "Roboto-Bold",
                                    color: CColors.White
                                ),
                                textAlign: TextAlign.center
                            )
                        ),
                        new Positioned(
                            left: 0,
                            right: 0,
                            bottom: 0,
                            height: 30,
                            child: new Container(
                                color: Color.fromRGBO(255, 255, 255, 0.2f)
                            )
                        )
                    }
                )
            );
        }
    }
}