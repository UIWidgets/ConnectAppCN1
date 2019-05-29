using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class EventsScreen : StatefulWidget {
        public override State createState() {
            return new _EventsScreenState();
        }
    }

    class _EventsScreenState : AutomaticKeepAliveClientMixin<EventsScreen> {
        PageController _pageController;
        int _selectedIndex;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._pageController = new PageController();
            this._selectedIndex = 0;
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
                        this._buildSelectView(),
                        this._buildContentView()
                    }
                )
            );
        }

        Widget _buildSelectView() {
            return new CustomSegmentedControl(
                new List<string> {"即将开始", "往期活动"},
                newValue => {
                    this.setState(() => this._selectedIndex = newValue);
                    this._pageController.animateToPage(
                        newValue,
                        new TimeSpan(0, 0, 0, 0, 250),
                        Curves.ease
                    );
                }, this._selectedIndex
            );
        }


        Widget _buildContentView() {
            return new Flexible(
                child: new Container(
                    child: new PageView(
                        physics: new BouncingScrollPhysics(),
                        controller: this._pageController,
                        onPageChanged: index => { this.setState(() => { this._selectedIndex = index; }); },
                        children: new List<Widget> {
                            new EventOngoingScreenConnector(),
                            new EventCompletedScreenConnector()
                        }
                    )
                )
            );
        }

        public override void dispose() {
            this._pageController.dispose();
            base.dispose();
        }
    }
}