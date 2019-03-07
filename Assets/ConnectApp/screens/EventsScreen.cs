using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
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
        private const float headerHeight = 80;
        private PageController _pageController;
        private int _selectedIndex;

        private float _offsetY = 0;

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
                                    color: CColors.White
                                )
                            )),
                        new CustomButton(
                            padding: EdgeInsets.symmetric(horizontal: 8),
                            onPressed: () => {
//                                StoreProvider.store.Dispatch(new ChangeEmailAction {email = "ods@ods.com"});
                                Navigator.pushNamed(context, "/mine");
                            },
                            child: new Icon(
                                Icons.notifications,
                                size: 28,
                                color: CColors.icon2
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.only(8, right: 16),
                            onPressed: () => { Navigator.pushNamed(context, "/login"); },
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
            if (StoreProvider.store.state.Events.Count == 0)
                StoreProvider.store.Dispatch(new EventsRequestAction {pageNumber = 1});
            _pageController = new PageController();
            _selectedIndex = 0;
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
                color: CColors.White,
                child: new Stack(
                    children: new List<Widget> {
                        new Container(
                            padding: EdgeInsets.only(0, 140, 0, 49),
                            child: new Column(
                                children: new List<Widget> {
                                    buildSelectView(),
                                    contentView()
                                }
                            )
                        ),
                        new Positioned(
                            top: 0,
                            left: 0,
                            right: 0,
                            child: new CustomNavigationBar(new Text("活动", style: CTextStyle.H2), new List<Widget> {
                                new Container(child: new Icon(Icons.search, null, 28,
                                    Color.fromRGBO(255, 255, 255, 0.8f)))
                            }, CColors.White, _offsetY)
                        )
                    }
                )
            );
        }

        private Widget buildSelectItem(BuildContext context, string title, int index) {
            var textColor = CColors.TextTitle;
            Widget lineView = new Positioned(new Container());
            if (index == _selectedIndex) {
                textColor = CColors.PrimaryBlue;
                lineView = new Positioned(
                    bottom: 0,
                    left: 0,
                    right: 0,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: new List<Widget> {
                            new Container(
                                width: 80,
                                height: 2,
                                decoration: new BoxDecoration(
                                    CColors.PrimaryBlue
                                )
                            )
                        }
                    )
                );
            }

            return new Container(
                child: new Stack(
                    children: new List<Widget> {
                        new CustomButton(
                            onPressed: () => {
                                if (_selectedIndex != index) setState(() => _selectedIndex = index);
                                _pageController.animateToPage(
                                    index,
                                    new TimeSpan(0, 0,
                                        0, 0, 250),
                                    Curves.ease
                                );
                            },
                            child: new Container(
                                height: 44,
                                width: 96,
                                alignment: Alignment.center,
                                child: new Text(
                                    title,
                                    style: new TextStyle(
                                        fontSize: 16,
                                        fontWeight: FontWeight.w400,
                                        color: textColor
                                    )
                                )
                            )
                        ),
                        lineView
                    }
                )
            );
        }

        private Widget buildSelectView() {
            return new Container(
                child: new Container(
                    height: 44,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: new List<Widget> {
                            buildSelectItem(context, "即将开始", 0), buildSelectItem(context, "往期活动", 1)
                        }
                    )
                )
            );
        }

        private Widget mineList() {
            return new Container(
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
                            events.ForEach(model => { cardList.Add(new ActiveCard(model)); });
                        else
                            cardList.Add(new Container());

                        return new ListView(
                            physics: new AlwaysScrollableScrollPhysics(),
                            children: cardList
                        );
                    }
                )
            );
        }

        private Widget contentView() {
            return new Flexible(
                child: new Container(
                    child: new PageView(
                        physics: new BouncingScrollPhysics(),
                        controller: _pageController,
                        onPageChanged: (int index) => { setState(() => { _selectedIndex = index; }); },
                        children: new List<Widget> {
                            mineList(), mineList()
                        }
                    )
                )
            );
        }
    }
}