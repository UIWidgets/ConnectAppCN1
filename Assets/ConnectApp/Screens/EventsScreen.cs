using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {

    public class EventsScreen : StatefulWidget {

        public override State createState() {
            return new _EventsScreenState();
        }
    }

    internal class _EventsScreenState : AutomaticKeepAliveClientMixin<EventsScreen> {
        private PageController _pageController;
        private int _selectedIndex;
        
        protected override bool wantKeepAlive { get=>true; }

        public override void initState() {
            base.initState();
            _pageController = new PageController();
            _selectedIndex = 0;
        }

        public override Widget build(BuildContext context) {
            base.build(context);
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new CustomNavigationBar(
                            new Text("活动", style: CTextStyle.H2),
                            null,
                            CColors.White,
                            0
                        ),
                        _buildSelectView(),
                        _buildContentView()
                    }
                )
            );
        }

        private Widget _buildSelectView() {
            return new CustomSegmentedControl(
                new List<string> {"即将开始", "往期活动"},
                newValue => {
                    setState(() => _selectedIndex = newValue);
                    _pageController.animateToPage(
                        newValue,
                        new TimeSpan(0, 0, 0, 0, 250),
                        Curves.ease
                    );
                },
                _selectedIndex
            );
        }


        private Widget _buildContentView() {
            return new Flexible(
                child: new Container(
                    child: new PageView(
                        physics: new BouncingScrollPhysics(),
                        controller: _pageController,
                        onPageChanged: index => { setState(() => { _selectedIndex = index; }); },
                        children: new List<Widget> {
                            new EventOngoingScreenConnector(),
                            new EventCompletedScreenConnector()
                        }
                    )
                )
            );
        }

        public override void dispose() {
            _pageController.dispose();
            base.dispose();
        }
    }
}