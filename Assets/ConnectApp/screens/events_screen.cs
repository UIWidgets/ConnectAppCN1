using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.screens {
    public class EventsScreen : StatefulWidget {
        public EventsScreen(Key key = null) : base(key) {
        }

        public override State createState() {
            return new _EventsScreen();
        }
    }

    internal class countActionModel {
        public Action<int> onAdd;
    }

    internal class _EventsScreen : State<EventsScreen> {
        private const double headerHeight = 80.0;

        private double _offsetY = 0.0;
//        private List<IEvent> events;

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
                            onPressed: () => {
                                StoreProvider.store.Dispatch(new ChangeEmailAction() {email = "ods@ods.com"});
//                                Navigator.pushName(context, "/mine");
                            },
                            child: new Icon(
                                Icons.notifications,
                                size: 28.0,
                                color: CColors.icon2
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.only(8.0, right: 16.0),
                            onPressed: () => {
                                StoreProvider.store.Dispatch(new AddCountAction() {number = 3});
//                                Navigator.pushName(context, "/login");
                            },
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

        public override void initState() {
            base.initState();
            StoreProvider.store.Dispatch(new EventsRequestAction {pageNumber = 1});
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
            return new NotificationListener<ScrollNotification>(
                onNotification: (ScrollNotification notification) => {
                    _onNotification(notification, context);
                    return true;
                },
                child: new Flexible(
                    child: new Container(
                        child: new StoreConnector<AppState, Dictionary<string, object>>(
                            converter: (state, dispatch) => new Dictionary<string, object> {
                                {"loading", (bool) state.Get("event.loading")},
                                {"events", state.Get("event.events")}
                            },
                            builder: (context1, viewModel) => {
                                var loading = (bool) viewModel["loading"];
                                var events = viewModel["events"] as List<IEvent>;
                                var cardList = new List<Widget>();
                                Debug.Log($"loading: + {loading}");
                                Debug.Log($"events: + {events}");
                                if (!loading) {
                                    events.ForEach(action: model => {
                                        cardList.Add(new EventCard(Key.key(model.id), model));
                                    });
                                }
                                else {
                                    cardList.Add(new Container());
                                }

                                return new ListView(
                                    physics: new AlwaysScrollableScrollPhysics(),
                                    children: cardList
                                );
                            }
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
                            new StoreConnector<AppState, string>(
                                converter: (state, dispatch) => $"Count:{state.Get("count", 0)}",
                                builder: (context1, countText) => new Text(countText, style: new TextStyle(
                                    fontSize: 20, fontWeight: FontWeight.w700
                                ))
                            ),
                            new StoreConnector<AppState, string>(
                                converter: (state, dispatch) => $"Email:{state.Get("login.email", "")}",
                                builder: (context1, countText) => new Text(countText, style: new TextStyle(
                                    fontSize: 20, fontWeight: FontWeight.w700
                                ))
                            ),
                            _buildHeader(context), _buildContentList(context),
                        }
                    )
                )
            );
            return new StoreProvider<AppState>(StoreProvider.store, container);
        }
    }
}