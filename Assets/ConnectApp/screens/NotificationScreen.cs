using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class NotificationScreen : StatefulWidget {
        public NotificationScreen(
            Key key = null
        ) : base(key) {
        }

        public override State createState() {
            return new _NotificationScreenState();
        }
    }

    public class _NotificationScreenState : State<NotificationScreen> {
        private const float headerHeight = 140;
        private float _offsetY = 0;

        private bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            if (pixels >= 0) {
                if (pixels <= headerHeight) setState(() => { _offsetY = pixels / 2.0f; });
            }
            else {
                if (_offsetY != 0) setState(() => { _offsetY = 0; });
            }

            return true;
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new CustomNavigationBar(
                            new Text(
                                "通知",
                                style: new TextStyle(
                                    height: 1.25f,
                                    fontSize: 32 / headerHeight * (headerHeight - _offsetY),
                                    fontFamily: "PingFang-Semibold",
                                    color: CColors.TextTitle
                                )
                            ),
                            null,
                            CColors.White,
                            _offsetY
                        ),
                        new CustomDivider(
                            color: CColors.Separator2,
                            height: 1
                        ),
                        new Flexible(
                            child: new NotificationListener<ScrollNotification>(
                                onNotification: _onNotification,
                                child: new Container(
                                    padding: EdgeInsets.only(bottom: 49),
                                    child: ListView.builder(
                                        itemCount: 10,
                                        itemBuilder: (BuildContext cxt, int index) => { return new NotificationCard(); }
                                    )
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}