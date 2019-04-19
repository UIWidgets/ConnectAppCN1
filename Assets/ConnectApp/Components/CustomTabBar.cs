using System;
using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.components {
    public delegate bool SelectTabCallBack(int index);

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
        private PageController _pageController;
        private int _selectedIndex;

        private const int KTabBarHeight = 49;

        public override void initState() {
            base.initState();
            _selectedIndex = 0;
            _pageController = new PageController(_selectedIndex);
        }

        public override Widget build(BuildContext context) {
            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        new Flexible(
                            child: _buildContentView()
                        ),
                        _buildBottomTabBar()
                    }
                )
            );
        }

        private Widget _buildContentView() {
            return new Container(
                child: new Container(
                    height: Screen.safeArea.height,
                    color: CColors.background1,
                    child: new PageView(
                        physics: new NeverScrollableScrollPhysics(),
                        children: widget.controllers,
                        controller: _pageController,
                        onPageChanged: _onPageChanged
                    )
                )
            );
        }

        private Widget _buildBottomTabBar() {
            return new Container(
                decoration: new BoxDecoration(
                    border: new Border(new BorderSide(CColors.Separator)),
                    color: widget.backgroundColor
                ),
                height: KTabBarHeight,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceAround,
                    children: _buildItems()
                )
            );
        }

        private List<Widget> _buildItems() {
            var children = new List<Widget>();
            widget.items.ForEach(item => {
                Widget buildItem = new Flexible(
                    child: new Stack(
                        fit: StackFit.expand,
                        children: new List<Widget> {
                            new GestureDetector(
                                onTap: () => {
                                    if (_selectedIndex != item.index)
                                        if (widget.tapCallBack != null)
                                            if (widget.tapCallBack(item.index))
                                                setState(() => {
                                                    _selectedIndex = item.index;
                                                    _pageController.animateToPage(item.index,
                                                        new TimeSpan(0, 0, 0, 0,1),
                                                        Curves.ease);
                                                });
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
                                                    color: _selectedIndex == item.index
                                                        ? item.activeColor
                                                        : item.inActiveColor)
                                            ),
                                            new Padding(
                                                padding: EdgeInsets.only(top: 2.5f),
                                                child: new Text(item.title,
                                                    style: new TextStyle(fontSize: 10,
                                                        color: _selectedIndex == item.index
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

        private void _onPageChanged(int page) {
            _pageController.jumpToPage(page);
        }

        public override void dispose() {
            _pageController.dispose();
            base.dispose();
        }
    }
}