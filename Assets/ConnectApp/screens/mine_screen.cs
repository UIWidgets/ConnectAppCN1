using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.screens
{
    public class MineScreen : StatefulWidget
    {
        public MineScreen(Key key = null) : base(key)
        {
        }

        public override State createState()
        {
            return new _MineScreen();
        }
    }

    internal class _MineScreen : State<MineScreen>
    {
        private PageController _pageController;
        private int _selectedIndex;

        public override void initState()
        {
            base.initState();
            _pageController = new PageController();
            _selectedIndex = 0;
        }

        public override Widget build(BuildContext context)
        {
            return new Column(
                children: new List<Widget>
                {
                    new CustomAppBar(
                        title: new Text(
                            "Mine",
                            style: new TextStyle(
                                fontSize: 17.0,
                                color: CColors.text1
                            )
                        ),
                        actions: new List<Widget>
                        {
                            new CustomButton(
                                child: new Icon(
                                    Icons.settings,
                                    size: 28.0,
                                    color: CColors.icon2
                                ),
                                onPressed: () => Navigator.pushName(context, "/setting")
                            )
                        }
                    ),
                    buildSelectView(),
                    contentView()
                }
            );
        }

        private Widget buildSelectItem(string title, int index)
        {
            var textColor = CColors.text2;
            Widget lineView = new Positioned(new Container());
            if (index == _selectedIndex)
            {
                textColor = CColors.text11;
                lineView = new Positioned(
                    bottom: 0,
                    left: 0,
                    right: 0,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: new List<Widget>
                        {
                            new Container(
                                width: 80,
                                height: 2,
                                decoration: new BoxDecoration(
                                    CColors.text11
                                )
                            )
                        }
                    )
                );
            }

            return new Flexible(
                child: new Stack(
                    fit: StackFit.expand,
                    children: new List<Widget>
                    {
                        new CustomButton(
                            onPressed: () =>
                            {
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
                                alignment: Alignment.center,
                                child: new Text(
                                    title,
                                    style: new TextStyle(
                                        fontSize: 15,
                                        fontWeight: FontWeight.w700,
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

        private Widget buildSelectView()
        {
            return new Column(
                children: new List<Widget>
                {
                    new Container(
                        height: 44,
                        decoration: new BoxDecoration(
                            CColors.background1
                        ),
                        child: new Row(
                            children: new List<Widget>
                            {
                                buildSelectItem("即将参与", 0), buildSelectItem("浏览记录", 1)
                            }
                        )
                    ),
                    new Container(decoration: new BoxDecoration(CColors.black), height: 1)
                }
            );
        }

        private Widget mineList()
        {
            var historyCard = new HistoryCard();
            return new Container(
                decoration: new BoxDecoration(
                    CColors.background1
                ),
                child: new ListView(
                    physics: new AlwaysScrollableScrollPhysics(),
                    children: new List<Widget>
                    {
                        historyCard,
                        historyCard,
                        historyCard
                    }
                )
            );
        }

        private Widget contentView()
        {
            return new Flexible(
                child: new Container(
                    decoration: new BoxDecoration(CColors.background1),
                    child: new PageView(
                        physics: new BouncingScrollPhysics(),
                        controller: _pageController,
                        onPageChanged: (int index) => { setState(() => { _selectedIndex = index; }); },
                        children: new List<Widget>
                        {
                            mineList(), mineList()
                        }
                    )
                )
            );
        }
    }
}