using System.Collections;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Newtonsoft.Json;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;
using Event = ConnectApp.models.Event;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.screens {
    public class EventsScreen : StatefulWidget {
        public EventsScreen(Key key = null) : base(key) {
        }

        public override State createState() {
            return new _EventsScreen();
        }
    }

    internal class _EventsScreen : State<EventsScreen> {
        private const double headerHeight = 80.0;
        private double _offsetY = 0.0;
        private List<Event> events;

        private Widget _buildHeader(BuildContext context) {
            return new Container(
                padding: EdgeInsets.only(16.0, right: 8.0),
                height: headerHeight - _offsetY,
                child: new Row(
                    children: new List<Widget> {
                        new Flexible(
                            flex: 1,
                            fit: FlexFit.tight,
                            child: new Text(
                                "Today",
                                style: new TextStyle(
                                    fontSize: (34.0 / headerHeight) * (headerHeight - _offsetY),
                                    color: CColors.white
                                )
                            )),
                        new CustomButton(
                            padding: EdgeInsets.symmetric(horizontal: 8.0),
                            onPressed: () => { Navigator.pushName(context, "/mine"); },
                            child: new Icon(
                                Icons.notifications,
                                size: 28.0,
                                color: CColors.icon2
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.only(8.0, right: 16.0),
                            onPressed: () => { Navigator.pushName(context, "/login"); },
                            child: new Icon(
                                Icons.account_circle,
                                size: 28.0,
                                color: CColors.icon2
                            )
                        )
                    }
                )
            );
        }

        private IEnumerator requestData() {
            UnityWebRequest request =
                UnityWebRequestTexture.GetTexture("https://connect-dev.unity.com/api/live/events");
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log(request.error);
            }
            else {
                if (request.responseCode != 200) yield break;
                var dataList = JsonConvert.DeserializeObject<List<Event>>(request.downloadHandler.text);
                setState(() => { events = dataList; });
            }
        }

        public override void initState() {
            base.initState();
            Window.instance.startCoroutine(requestData());
        }


        private bool _onNotification(ScrollNotification notification, BuildContext context) {
            double pixels = notification.metrics.pixels;
            if (pixels >= 0.0) {
                if (pixels <= headerHeight) setState(() => { _offsetY = pixels / 2.0; });
            }
            else {
                if (_offsetY != 0.0) setState(() => { _offsetY = 0.0; });
            }

            return true;
        }

        private Widget _buildContentList(BuildContext context) {
            var cardList = new List<Widget>();
            if (events != null) {
                events.ForEach(action: model => { cardList.Add(new EventCard(Key.key(model.id), model: model)); });
            }
            else {
                cardList.Add(new Container());
            }

            return new NotificationListener<ScrollNotification>(
                onNotification: (ScrollNotification notification) => {
                    _onNotification(notification, context);
                    return true;
                },
                child: new Flexible(
                    child: new Container(
                        child: new ListView(
                            physics: new AlwaysScrollableScrollPhysics(),
                            children: cardList
                        )
                    )
                )
            );
        }

        public override Widget build(BuildContext context) {
            var container = new Container(
                child: new Container(
                    color: CColors.background1,
                    child: new Column(
                        children: new List<Widget> {
                            _buildHeader(context), _buildContentList(context)
                        }
                    )
                )
            );
            return container;
        }
    }
}