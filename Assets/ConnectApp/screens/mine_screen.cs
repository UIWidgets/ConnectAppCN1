using System;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Samples.ConnectApp.widgets;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.UIWidgets.Samples.ConnectApp {
    public class MineScreen : StatefulWidget {
        public MineScreen(Key key = null) : base(key) {
        }

        public override State createState() {
            return new _MineScreen();
        }
    }

    internal class _MineScreen : State<MineScreen> {
        PageController _pageController;
        int _selectedIndex;

        public override void initState() {
            base.initState();
            this._pageController = new PageController();
            this._selectedIndex = 0;
        }

        public override Widget build(BuildContext context) {
            return new Column(
                children: new List<Widget> {
                    new CustomAppBar(
                        title: new Text(
                            "Mine",
                            style: new TextStyle(
                                fontSize: 17.0,
                                color: CLColors.text1
                            )
                        ),
                        actions: new List<Widget> {
                            new CustomButton(
                                child: new Icon(
                                    Icons.settings,
                                    size: 28.0,
                                    color: CLColors.icon2
                                ),
                                onPressed: () => Navigator.pushName(context, "/setting")
                            )
                        }
                    ),
                    this.buildSelectView(),
                    this.contentView()
                }
            );
        }

        Widget buildSelectItem(string title, int index) {
            var textColor = CLColors.text2;
            Widget lineView = new Positioned(new Container());
            if (index == this._selectedIndex) {
                textColor = CLColors.text11;
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
                                    CLColors.text11
                                )
                            )
                        }
                    )
                );
            }

            return new Flexible(
                child: new Stack(
                    fit: StackFit.expand,
                    children: new List<Widget> {
                        new CustomButton(
                            onPressed: () => {
                                if (this._selectedIndex != index) {
                                    this.setState(() => this._selectedIndex = index);
                                }

                                this._pageController.animateToPage(
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

        Widget buildSelectView() {
            return new Column(
                children: new List<Widget> {
                    new Container(
                        height: 44,
                        decoration: new BoxDecoration(
                            CLColors.background1
                        ),
                        child: new Row(
                            children: new List<Widget> {
                                this.buildSelectItem("即将参与", 0), this.buildSelectItem("浏览记录", 1)
                            }
                        )
                    ),
                    new Container(decoration: new BoxDecoration(CLColors.black), height: 1)
                }
            );
        }

        Widget mineList() {
            return new Container(
                decoration: new BoxDecoration(
                    CLColors.background1
                ),
                child: new ListView(
                    physics: new AlwaysScrollableScrollPhysics(),
                    children: new List<Widget> {
                        this.cellView(),
                        this.cellView(),
                        this.cellView(),
                        this.cellView(),
                        this.cellView(),
                        this.cellView(),
                        this.cellView()
                    }
                )
            );
        }

        Widget contentView() {
            return new Flexible(
                child: new Container(
                    decoration: new BoxDecoration(CLColors.background1),
                    child: new PageView(
                        physics: new BouncingScrollPhysics(),
                        controller: this._pageController,
                        onPageChanged: (int index) => { this.setState(() => { this._selectedIndex = index; }); },
                        children: new List<Widget> {
                            this.mineList(), this.mineList()
                        }
                    )
                )
            );
        }

        Widget cellView() {
            return new Container(
                height: 100,
                padding: EdgeInsets.fromLTRB(16, 16, 16, 0),
                decoration: new BoxDecoration(
                    CLColors.background2
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        Image.asset(
                            "yoshi",
                            height: 84,
                            width: 150,
                            fit: BoxFit.fill
                        ),
                        new Container(width: 16),
                        new Flexible(
                            child: new Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Text(
                                        "迪士尼电视动画与Unity联袂合作，开创实时动画新纪元",
                                        maxLines: 3,
                                        style: new TextStyle(
                                            fontSize: 15,
                                            fontWeight: FontWeight.w700,
                                            color: CLColors.text1
                                        )
                                    ),
                                    new Container(height: 8),
                                    new Text(
                                        "1920次观看",
                                        maxLines: 1,
                                        style: new TextStyle(
                                            fontSize: 12,
                                            color: CLColors.text2
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }
    }
}