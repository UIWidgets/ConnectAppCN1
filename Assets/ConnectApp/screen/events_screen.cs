using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Samples.ConnectApp.widgets;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;

namespace Unity.UIWidgets.Samples.ConnectApp {
    public class EventsScreen : StatefulWidget {
        public EventsScreen(Key key = null) : base(key) {
        }

        public override State createState() {
            return new _EventsScreen();
        }
    }

    internal class _EventsScreen : State<EventsScreen> {
        const double headerHeight = 80.0;
        double _offsetY = 0.0;

        Widget _buildHeader(BuildContext context) {
            return new Container(
                padding: EdgeInsets.only(16.0, right: 8.0),
                height: headerHeight - this._offsetY,
                child: new Row(
                    children: new List<Widget> {
                        new Flexible(
                            flex: 1,
                            fit: FlexFit.tight,
                            child: new Text(
                                "Today",
                                style: new TextStyle(
                                    fontSize: (34.0 / headerHeight) * (headerHeight - this._offsetY),
                                    color: CLColors.white
                                )
                            )),
                        new CustomButton(
                            padding: EdgeInsets.symmetric(horizontal: 8.0),
                            onPressed: () => { Navigator.pushName(context, "/mine"); },
                            child: new Icon(
                                Icons.notifications,
                                size: 28.0,
                                color: CLColors.icon2
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.only(8.0, right: 16.0),
                            onPressed: () => { Navigator.pushName(context, "/login"); },
                            child: new Icon(
                                Icons.account_circle,
                                size: 28.0,
                                color: CLColors.icon2
                            )
                        )
                    }
                )
            );
        }

        IEnumerator requestData() {
            UnityWebRequest request =
                UnityWebRequestTexture.GetTexture("https://connect-dev.unity.com/api/live/events");
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log(request.error);
            }
            else {
                if (request.responseCode == 200) {
                    Debug.Log(request.downloadHandler.text);
                }
            }
        }

        public override void initState() {
            base.initState();
            this.requestData();
        }

        bool _onNotification(ScrollNotification notification, BuildContext context) {
            double pixels = notification.metrics.pixels;
            if (pixels >= 0.0) {
                if (pixels <= headerHeight) this.setState(() => { this._offsetY = pixels / 2.0; });
            }
            else {
                if (this._offsetY != 0.0) this.setState(() => { this._offsetY = 0.0; });
            }

            return true;
        }

        Widget _buildContentList(BuildContext context) {
            var testCard = new EventCard(
                "https://blogs.unity3d.com/wp-content/uploads/2018/12/2018.3-BLOG_cover-1280x720.jpg"
            );

            return new NotificationListener<ScrollNotification>(
                onNotification: (ScrollNotification notification) => {
                    this._onNotification(notification, context);
                    return true;
                },
                child: new Flexible(
                    child: new Container(
                        child: new ListView(
                            physics: new AlwaysScrollableScrollPhysics(),
                            children: new List<Widget> {
                                testCard,
                                testCard,
                                testCard,
                                testCard,
                                testCard,
                                testCard
                            }
                        )
                    )
                )
            );
        }

        public override Widget build(BuildContext context) {
            var container = new Container(
                child: new Container(
                    color: CLColors.background1,
                    child: new Column(
                        children: new List<Widget> {
                            this._buildHeader(context), this._buildContentList(context)
                        }
                    )
                )
            );
            return container;
        }
    }
}