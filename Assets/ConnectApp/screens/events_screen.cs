using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    public class EventsScreen : StatefulWidget {
        public EventsScreen(Key key = null) : base(key) {
        }

        public override State createState() {
            return new _EventsScreen();
        }
    }

    internal class _EventsScreen : State<EventsScreen> {
        private const float headerHeight = 80;

        private float _offsetY = 0;
//        private List<IEvent> events;

        private Widget _buildHeader(BuildContext context) {
            return new Container(
                padding: EdgeInsets.only(16, right: 8),
                height: headerHeight - _offsetY,
                child: new Row(
                    children: new List<Widget> {
                        new Flexible(
                            flex: 1,
                            fit: FlexFit.tight,
                            child: new Text(
                                "Today",
                                style: new TextStyle(
                                    fontSize: 34 / headerHeight * (headerHeight - _offsetY),
                                    color: CColors.white
                                )
                            )),
                        new CustomButton(
                            padding: EdgeInsets.symmetric(horizontal: 8),
                            onPressed: () => {
//                                StoreProvider.store.Dispatch(new ChangeEmailAction {email = "ods@ods.com"});
                                Navigator.pushName(context, "/mine");
                            },
                            child: new Icon(
                                Icons.notifications,
                                size: 28,
                                color: CColors.icon2
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.only(8, right: 16),
                            onPressed: () => {
//                                StoreProvider.store.Dispatch(new AddCountAction() {number = 3});
                                Navigator.pushName(context, "/login");
                            },
                            child: new Icon(
                                Icons.account_circle,
                                size: 28,
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
            var pixels = notification.metrics.pixels;
            if (pixels >= 0) {
                if (pixels <= headerHeight) setState(() => { _offsetY = pixels / 2; });
            }
            else {
                if (_offsetY != 0) setState(() => { _offsetY = 0; });
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
                                {"loading", state.EventsLoading},
                                {"events", state.Events}
                            },
                            builder: (context1, viewModel) => {
                                var loading = (bool) viewModel["loading"];
                                var events = viewModel["events"] as List<IEvent>;
                                var cardList = new List<Widget>();
                                if (!loading)
                                    events.ForEach(model => { cardList.Add(new EventCard(Key.key(model.id), model)); });
                                else
                                    cardList.Add(new Container());

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
            return new Container(
                child: new CustomTabBar(
                    new List<Widget>
                    {
                        new LoginScreen(),
                        new MineScreen()
                    },
                    new List<CustomTabBarItem>
                    {
                        new CustomTabBarItem(
                            0,
                            "mario",
                            "mario",
                            "首页",
                            Color.white,
                            Color.white
                        ),
                        new CustomTabBarItem(
                            0,
                            "pikachu",
                            "pikachu",
                            "我的",
                            Color.white,
                            Color.white
                        ),
                    }
                ));
            
            
//            var container = new Container(
//                child: new Container(
//                    color: CColors.background1,
//                    child: new Column(
//                        children: new List<Widget> {
////                            new StoreConnector<AppState, string>(
////                                converter: (state, dispatch) => $"Count: {state.Count}",
////                                builder: (context1, countText) => new Text(countText, style: new TextStyle(
////                                    fontSize: 20, fontWeight: FontWeight.w700
////                                ))
////                            ),
////                            new StoreConnector<AppState, string>(
////                                converter: (state, dispatch) => $"Email: {state.LoginState.email}",
////                                builder: (context1, countText) => new Text(countText, style: new TextStyle(
////                                    fontSize: 20, fontWeight: FontWeight.w700
////                                ))
////                            ),
//                            _buildHeader(context), _buildContentList(context),
//                        }
//                    )
//                )
//            );
//            return container;
        }
    }
}