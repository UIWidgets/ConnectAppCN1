using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.components {
    public class CustomTabBar : StatefulWidget {
        public CustomTabBar(
            List<Widget> controllers,
            List<CustomTabBarItem> items,
            Color tabbarBackgroudColor,
            Key key = null
        ) : base(key) {
            this.controllers = controllers;
            this.items = items;
            this.tabbarBackgroudColor = tabbarBackgroudColor;
        }

        public readonly Color tabbarBackgroudColor;
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
        private Widget _body;

        public override Widget build(BuildContext context) {
            return new Container(
                child: new Stack(
                    children: new List<Widget> {
                        _contentView(context),
                        new Positioned(
                            bottom: 0,
                            left: 0,
                            right: 0,
                            child: _bottomTabBar()
                        )
                    }
                )
            );
        }

        private Widget _contentView(BuildContext context) {
            return new Container(
                child: new Container(
                    height: Screen.safeArea.height,
                    color: CColors.background1,
                    child: widget.controllers[_selectedIndex]
                )
            );
        }

        private Widget _bottomTabBar() {
            return new Container(
                decoration: new BoxDecoration(
                    border: Border.all(CColors.Separator),
                    color: widget.tabbarBackgroudColor
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
                                    if (_selectedIndex != item.index) setState(() => _selectedIndex = item.index);
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
                                                child: new Icon(item.icon, null, item.size,
                                                    _selectedIndex == item.index
                                                        ? item.activeColor
                                                        : item.inActiveColor)
                                            ),
                                            new Padding(
                                                padding: EdgeInsets.only(top: 5),
                                                child: new Text(item.title,
                                                    style: new TextStyle(fontSize: 9,
                                                        color: _selectedIndex == item.index
                                                            ? item.activeColor
                                                            : item.inActiveColor))
                                            ),
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
    }
}