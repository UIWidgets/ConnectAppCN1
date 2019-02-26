using System;
using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components
{
    public class CustomTabBar : StatefulWidget
    {
        public CustomTabBar(
            List<Widget> controllers,
            List<CustomTabBarItem> items,
            Key key = null
        ) : base(key)
        {
            this.controllers = controllers;
            this.items = items;
            
        }

        public readonly List<Widget> controllers;
        public readonly List<CustomTabBarItem> items;
        
        public override State createState()
        {
            return new _CustomTabBarState();
        }
    }

    public class _CustomTabBarState : State<CustomTabBar>
    {
        private PageController _pageController;
        private int _selectedIndex;
        
        private const int kTabBarHeight = 50;
        private Widget _body;
        
        public override Widget build(BuildContext context)
        {
            return new Column(
                mainAxisAlignment:MainAxisAlignment.start,
                children:new List<Widget>
                {
                    _contentView(),
                    _bottomTabBar()
                }
            );
        }

        private Widget _contentView()
        {
           return new Container(
                    child: new Container(
                        height:MediaQuery.of(context).size.height-kTabBarHeight,
                        color: CColors.blue,
                        decoration: new BoxDecoration(CColors.background1),
                        child: widget.controllers[_selectedIndex]
                    )
                );

        }

        private Widget _bottomTabBar()
        {
            return new Flexible(
                child: new Container(
                    height:kTabBarHeight,
                    color: Color.black,
                    child:new Row(
                        mainAxisAlignment:MainAxisAlignment.spaceAround,
                        children: _buildItems()
                    )
                )
            );
        }

        List<Widget> _buildItems()
        {         
            List<Widget> _widgets = new List<Widget>();
            widget.items.ForEach(item =>
            {
                Widget _bulidItem = new Flexible(
                    child: new Stack(
                        fit: StackFit.expand,
                        children: new List<Widget>
                        {
                            new GestureDetector(
                                onTap: () =>
                                {
                                    if (_selectedIndex != item.index) setState(() => _selectedIndex = item.index);

                                },
                                child: new Container(
                                    decoration: new BoxDecoration(
                                        color: Color.clear),
                                    child: new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        children: new List<Widget>
                                        {
                                            new Padding(
                                                padding: EdgeInsets.only(top: 5),
                                                child: Image.asset(
                                                    _selectedIndex == item.index ? item.activeImge : item.inactiveImge,
                                                    height: 24, width: 24)
                                            ),
                                            new Padding(
                                                padding: EdgeInsets.only(top: 5),
                                                child: new Text(item.title,
                                                    style: new TextStyle(fontSize: 9,
                                                        color: _selectedIndex == item.index
                                                            ? Colors.blue
                                                            : Color.white))
                                            ),

                                        }
                                    )
                                )
                            )
                        }
                    )

                );
                _widgets.Add(_bulidItem);
            });
            
            return _widgets;
        }


    }
    
    
  
   

}