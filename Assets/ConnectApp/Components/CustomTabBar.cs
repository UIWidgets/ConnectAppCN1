using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.State;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.Components {
    public delegate bool SelectTabCallBack(int fromIndex, int toIndex);

    public class CustomTabBarConnector : StatelessWidget {
        public CustomTabBarConnector(
            List<Widget> controllers,
            List<CustomTabBarItem> items,
            Color backgroundColor,
            SelectTabCallBack tapCallBack = null,
            int initialTabIndex = 0,
            Key key = null
        ) : base(key: key) {
            this.tapCallBack = tapCallBack;
            this.backgroundColor = backgroundColor;
            this.controllers = controllers;
            this.items = items;
            this.initialTabIndex = initialTabIndex;
        }

        readonly SelectTabCallBack tapCallBack;
        readonly Color backgroundColor;
        readonly List<Widget> controllers;
        readonly List<CustomTabBarItem> items;
        readonly int initialTabIndex;


        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, List<string>>(
                converter: state => new List<string> {
                    null,
                    null,
                    state.channelState.totalNotification(),
                    null
                },
                builder: (context1, notifications, dispatcher) => new CustomTabBar(
                    controllers: this.controllers,
                    items: this.items,
                    backgroundColor: this.backgroundColor,
                    notifications: notifications,
                    tapCallBack: this.tapCallBack,
                    initialTabIndex: this.initialTabIndex
                )
            );
        }
    }

    public class CustomTabBar : StatefulWidget {
        public CustomTabBar(
            List<Widget> controllers,
            List<CustomTabBarItem> items,
            List<string> notifications,
            Color backgroundColor,
            SelectTabCallBack tapCallBack = null,
            int initialTabIndex = 0,
            Key key = null
        ) : base(key: key) {
            D.assert(controllers != null && controllers.Count > 1);
            D.assert(items != null && items.Count > 1);
            D.assert(controllers.Count == items.Count);
            this.controllers = controllers;
            this.items = items;
            this.backgroundColor = backgroundColor;
            this.tapCallBack = tapCallBack;
            this.notifications = notifications;
            this.initialTabIndex = initialTabIndex;
        }

        public readonly SelectTabCallBack tapCallBack;
        public readonly Color backgroundColor;
        public readonly List<Widget> controllers;
        public readonly List<CustomTabBarItem> items;
        public readonly List<string> notifications;
        public readonly int initialTabIndex;

        public override State createState() {
            return new CustomTabBarState();
        }
    }

    public class CustomTabBarState : State<CustomTabBar> {
        PageController _pageController;
        int _selectedIndex;
        float _bottomPadding;

        public override void initState() {
            base.initState();
            this._selectedIndex = this.widget.initialTabIndex;
            this._bottomPadding = 0;
            this._pageController = new PageController(initialPage: this._selectedIndex);
        }

        public override Widget build(BuildContext context) {
            if (this._bottomPadding != MediaQuery.of(context).padding.bottom &&
                Application.platform != RuntimePlatform.Android) {
                this._bottomPadding = MediaQuery.of(context).padding.bottom;
            }

            return new Container(
                child: new Stack(
                    children: new List<Widget> {
                        this._buildContentView(),
                        this._buildBottomTabBar()
                    }
                )
            );
        }

        Widget _buildContentView() {
            return new Container(
                color: CColors.Background,
                child: new PageView(
                    physics: new NeverScrollableScrollPhysics(),
                    children: this.widget.controllers,
                    controller: this._pageController,
                    onPageChanged: this._onPageChanged
                )
            );
        }

        Widget _buildBottomTabBar() {
            return new Positioned(
                left: 0,
                right: 0,
                bottom: 0,
                height: CConstant.TabBarHeight + this._bottomPadding,
                child: new BackdropFilter(
                    filter: ImageFilter.blur(25, 25),
                    child: new Container(
                        decoration: new BoxDecoration(
                            border: new Border(new BorderSide(CColors.Separator)),
                            color: this.widget.backgroundColor
                        ),
                        child: new Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            children: new List<Widget> {
                                new Flexible(
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.spaceAround,
                                        children: this._buildItems()
                                    )
                                ),
                                new Container(height: this._bottomPadding)
                            }
                        )
                    )
                )
            );
        }

        List<Widget> _buildItems() {
            var children = new List<Widget>();
            var screenWidth = MediaQuery.of(context: this.context).size.width;
            this.widget.items.ForEach(item => {
                Widget buildItem = new Flexible(
                    child: new Stack(
                        fit: StackFit.expand,
                        children: new List<Widget> {
                            new GestureDetector(
                                onTap: () => {
                                    if (this._selectedIndex != item.index) {
                                        if (this.widget.tapCallBack != null) {
                                            if (this.widget.tapCallBack(this._selectedIndex, item.index)) {
                                                this.setState(() => {
                                                    this._selectedIndex = item.index;
                                                    this._pageController.animateToPage(item.index,
                                                        new TimeSpan(0, 0, 0, 0, 1),
                                                        Curves.ease);
                                                });
                                            }
                                        }
                                    }
                                },
                                child: new Container(
                                    decoration: new BoxDecoration(
                                        color: CColors.Transparent
                                    ),
                                    child: new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        children: new List<Widget> {
                                            new Padding(
                                                padding: EdgeInsets.only(top: 5),
                                                child: new Stack(
                                                    children: new List<Widget> {
                                                        new Icon(this._selectedIndex == item.index
                                                                ? item.selectedIcon
                                                                : item.normalIcon, size: item.size,
                                                            color: this._selectedIndex == item.index
                                                                ? item.activeColor
                                                                : item.inActiveColor),
                                                    }
                                                )
                                            ),
                                            new Padding(
                                                padding: EdgeInsets.only(top: 2.5f),
                                                child: new Text(item.title,
                                                    style: new TextStyle(fontSize: 10,
                                                        color: this._selectedIndex == item.index
                                                            ? item.activeColor
                                                            : item.inActiveColor))
                                            )
                                        }
                                    )
                                )
                            ),
                            new Positioned(
                                left: (float) Math.Ceiling(screenWidth / 8) + 2,
                                top: 4,
                                child: new IgnorePointer(
                                    child: new NotificationDot(
                                        this.widget.notifications[index: item.index],
                                        new BorderSide(color: CColors.White, 2)
                                    )
                                )
                            )
                        }
                    )
                );
                children.Add(buildItem);
            });

            return children;
        }

        void _onPageChanged(int page) {
            this._pageController.jumpToPage(page);
        }

        public override void dispose() {
            this._pageController.dispose();
            base.dispose();
        }
    }
}