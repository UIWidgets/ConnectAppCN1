using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public delegate bool SelectTabCallBack(int fromIndex, int toIndex);

    public class CustomTabBar : StatefulWidget {
        public CustomTabBar(
            List<Widget> controllers,
            List<CustomTabBarItem> items,
            Color backgroundColor,
            SelectTabCallBack tapCallBack = null,
            Key key = null
        ) : base(key) {
            D.assert(controllers != null && controllers.Count > 1);
            D.assert(items != null && items.Count > 1);
            D.assert(controllers.Count == items.Count);
            this.controllers = controllers;
            this.items = items;
            this.backgroundColor = backgroundColor;
            this.tapCallBack = tapCallBack;
        }

        public readonly SelectTabCallBack tapCallBack;
        public readonly Color backgroundColor;
        public readonly List<Widget> controllers;
        public readonly List<CustomTabBarItem> items;

        public override State createState() {
            return new CustomTabBarState();
        }
    }

    public class CustomTabBarState : State<CustomTabBar> {
        PageController _pageController;
        int _selectedIndex;

        const int KTabBarHeight = 49;

        public override void initState() {
            base.initState();
            this._selectedIndex = 0;
            this._pageController = new PageController(this._selectedIndex);
        }

        public override Widget build(BuildContext context) {
            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        new Flexible(
                            child: this._buildContentView(context)
                        ),
                        this._buildBottomTabBar()
                    }
                )
            );
        }

        Widget _buildContentView(BuildContext context) {
            return new Container(
                child: new Container(
                    height: MediaQuery.of(context).size.height,
                    color: CColors.Background,
                    child: new PageView(
                        physics: new NeverScrollableScrollPhysics(),
                        children: this.widget.controllers,
                        controller: this._pageController,
                        onPageChanged: this._onPageChanged
                    )
                )
            );
        }

        Widget _buildBottomTabBar() {
            return new Container(
                decoration: new BoxDecoration(
                    border: new Border(new BorderSide(CColors.Separator)),
                    color: this.widget.backgroundColor
                ),
                height: KTabBarHeight,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceAround,
                    children: this._buildItems()
                )
            );
        }

        List<Widget> _buildItems() {
            var children = new List<Widget>();
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
                                        CColors.Transparent
                                    ),
                                    child: new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        children: new List<Widget> {
                                            new Padding(
                                                padding: EdgeInsets.only(top: 5),
                                                child: new Icon(item.icon, size: item.size,
                                                    color: this._selectedIndex == item.index
                                                        ? item.activeColor
                                                        : item.inActiveColor)
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