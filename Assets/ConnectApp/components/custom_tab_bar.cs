using System.Collections.Generic;
using ConnectApp.constants;
using NUnit.Framework;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

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

        private Widget _contentView() {
            return new Flexible(
                child: new Container(
                    color: CColors.blue,
                    decoration: new BoxDecoration(CColors.background1),
                    child: new PageView(
                        physics: new BouncingScrollPhysics(),
                        controller: _pageController,
                        onPageChanged: (int index) => { setState(() => { _selectedIndex = index; }); },
                        children: widget.controllers
                    )
                )
            );
        }

        private Widget _bottomTabBar()
        {
            return new Flexible(
                child: new Container(
                    height:kTabBarHeight,
                    color: CColors.red,
                    child:new Row(
                        mainAxisAlignment:MainAxisAlignment.spaceBetween,
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
                _widgets.Add(
                    new Flexible(
                        child:new Column(
                            children:new List<Widget>
                            {
                                Image.asset(item.activeImge),
                                new Text(item.title,style:new TextStyle(fontSize:14))
                            }
                        )
                    )
                );
            });
            
            return _widgets;
        }


    }
    
    
  
   

}