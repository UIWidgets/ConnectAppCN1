using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {

    public class EventsScreen : StatefulWidget {

        public override State createState() {
            return new _EventsScreenState();
        }
    }

    internal class _EventsScreenState : State<EventsScreen> {
        private const float headerHeight = 80;
        private PageController _pageController;
        private int _selectedIndex;
        private float _offsetY;

//        protected override bool wantKeepAlive {
//            get => true;
//        }

        public override void initState() {
            base.initState();
            _pageController = new PageController();
            _selectedIndex = 0;
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new CustomNavigationBar(
                            new Text("活动", style: CTextStyle.H2),
                            null,
                            CColors.White,
                            _offsetY
                        ),
                        _buildSelectView(),
                        _buildContentView()
                    }
                )
            );
        }

        private Widget _buildSelectView() {
            return new Container(
                child: new Container(
                    decoration: new BoxDecoration(
                        border: new Border(bottom: new BorderSide(CColors.Separator2))
                    ),
                    height: 44,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: new List<Widget> {
                            _buildSelectItem("即将开始", 0),
                            _buildSelectItem("往期活动", 1)
                        }
                    )
                )
            );
        }


        private Widget _buildContentView() {
            return new Flexible(
                child: new Container(
                    padding: EdgeInsets.only(bottom: 49),
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

        private Widget _buildSelectItem(string title, int index) {
            Widget lineView = new Positioned(new Container());
            if (index == _selectedIndex)
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
                                    style: index == _selectedIndex
                                        ? CTextStyle.PLargeMediumBlue
                                        : CTextStyle.PLargeTitle
                                )
                            )
                        ),
                        lineView
                    }
                )
            );
        }
        public override void dispose() {
            base.dispose();
            _pageController.dispose();
        }
    }
}