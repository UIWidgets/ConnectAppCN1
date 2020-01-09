using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
    /// <summary>
    /// 合辑卡片
    /// </summary>
    public class LeaderBoardCollectionCard : StatelessWidget {
        public LeaderBoardCollectionCard(
            object data = null,
            int index = 0,
            GestureTapCallback onPress = null,
            Key key = null
        ) : base(key: key) {
            this.data = data;
            this.index = index;
            this.onPress = onPress;
        }

        readonly object data;
        readonly int index;
        readonly GestureTapCallback onPress;

        public override Widget build(BuildContext context) {
            Color numColor = this.index + 1 <= 3 ? CColors.Error : CColors.TextBody5;
            return new GestureDetector(
                onTap: this.onPress,
                child: new Container(
                    padding: EdgeInsets.all(16),
                    color: CColors.White,
                    height: 112,
                    child: new Row(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget> {
                            new Text(
                                $"{this.index + 1}",
                                style: new TextStyle(
                                    height: 1.27f,
                                    fontSize: 20,
                                    fontFamily: "Roboto-Bold",
                                    color: numColor
                                )
                            ),
                            new SizedBox(width: 16),
                            new Expanded(
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Text(
                                            "Unity官方博主预备营准备启动",
                                            maxLines: 2,
                                            overflow: TextOverflow.ellipsis,
                                            style: CTextStyle.PXLargeMediumBody
                                        ),
                                        new Text(
                                            "作者 10 • 文章 10",
                                            style: CTextStyle.PSmallBody4
                                        )
                                    }
                                )
                            ),
                            new SizedBox(width: 16),
                            new CoverImages(
                                verticalGap: 0
                            )
                        }
                    )
                )
            );
        }
    }

    /// <summary>
    /// 专栏卡片
    /// </summary>
    public class LeaderBoardColumnCard : StatelessWidget {
        public LeaderBoardColumnCard(
            object data = null,
            int index = 0,
            GestureTapCallback onPress = null,
            Key key = null
        ) : base(key: key) {
            this.data = data;
            this.index = index;
            this.onPress = onPress;
        }

        readonly object data;
        readonly int index;
        readonly GestureTapCallback onPress;

        public override Widget build(BuildContext context) {
            Color numColor = this.index + 1 <= 3 ? CColors.Error : CColors.TextBody5;
            return new GestureDetector(
                onTap: this.onPress,
                child: new Container(
                    padding: EdgeInsets.all(16),
                    color: CColors.White,
                    height: 112,
                    child: new Row(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget> {
                            new Text(
                                $"{this.index + 1}",
                                style: new TextStyle(
                                    height: 1.27f,
                                    fontSize: 20,
                                    fontFamily: "Roboto-Bold",
                                    color: numColor
                                )
                            ),
                            new SizedBox(width: 16),
                            new Expanded(
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Text(
                                            "商汤SenseAR丨Setup演示及基本功能讲解",
                                            maxLines: 2,
                                            overflow: TextOverflow.ellipsis,
                                            style: CTextStyle.PXLargeMediumBody
                                        ),
                                        new Text(
                                            "作者 10 • 文章 10",
                                            style: CTextStyle.PSmallBody4
                                        )
                                    }
                                )
                            ),
                            new SizedBox(width: 16),
                            new CoverImages(
                                verticalGap: 0
                            )
                        }
                    )
                )
            );
        }
    }

    /// <summary>
    /// 博主卡片
    /// </summary>
    public class LeaderBoardBloggerCard : StatelessWidget {
        public LeaderBoardBloggerCard(
            int index = 0,
            GestureTapCallback onPress = null,
            Key key = null
        ) : base(key: key) {
            this.index = index;
            this.onPress = onPress;
        }

        readonly int index;
        readonly GestureTapCallback onPress;

        public override Widget build(BuildContext context) {
            return new GestureDetector(
                onTap: this.onPress,
                child: new Container(
                    padding: EdgeInsets.all(16),
                    color: CColors.White,
                    height: 88,
                    child: new Row(
                        children: new List<Widget> {
                            new Text(
                                $"{this.index + 1}",
                                style: new TextStyle(
                                    height: 1.27f,
                                    fontSize: 20,
                                    fontFamily: "Roboto-Bold",
                                    color: CColors.TextBody5
                                )
                            ),
                            new Padding(
                                padding: EdgeInsets.symmetric(horizontal: 16),
                                child: new Container(
                                    width: 56,
                                    height: 56,
                                    decoration: new BoxDecoration(
                                        color: CColors.Red,
                                        borderRadius: BorderRadius.all(28)
                                    )
                                )
                            ),
                            new Expanded(
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Text(
                                            "十月",
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis,
                                            style: new TextStyle(
                                                height: 1.33f,
                                                fontSize: 16,
                                                fontFamily: "Roboto-Medium",
                                                color: CColors.TextBody
                                            )
                                        ),
                                        new Text(
                                            "社会热心人士社会热心人士社会热心人士社会热心人士",
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis,
                                            style: CTextStyle.PRegularBody4
                                        )
                                    }
                                )
                            ),
                            new Padding(
                                padding: EdgeInsets.only(16),
                                child: new Text(
                                    "文章 10 • 赞 98",
                                    style: CTextStyle.PSmallBody4
                                )
                            )
                        }
                    )
                )
            );
        }
    }

    /// <summary>
    /// 每周三更新
    /// </summary>
    public class LeaderBoardUpdateTip : StatelessWidget {
        public LeaderBoardUpdateTip(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new Container(
                height: 54,
                alignment: Alignment.center,
                decoration: new BoxDecoration(
                    color: CColors.White,
                    borderRadius: BorderRadius.only(12, 12)
                ),
                child: new Text("每周三更新", style: CTextStyle.PRegularBody4)
            );
        }
    }

    /// <summary>
    /// 博主头视图
    /// </summary>
    public class LeaderBoardBloggerHeader : StatelessWidget {
        public LeaderBoardBloggerHeader(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new Container(
                height: 286,
                child: new Stack(
                    children: new List<Widget> {
                        Image.asset(
                            "image/leaderboard-podium",
                            width: MediaQuery.of(context: context).size.width - 32,
                            height: 128,
                            fit: BoxFit.fill
                        ),
                        new Positioned(
                            left: 24,
                            bottom: 116,
                            width: 96,
                            child: new Column(
                                children: new List<Widget> {
                                    new Container(
                                        width: 64,
                                        height: 64,
                                        decoration: new BoxDecoration(
                                            color: CColors.Red,
                                            borderRadius: BorderRadius.all(32)
                                        )
                                    ),
                                    new Padding(
                                        padding: EdgeInsets.only(top: 8),
                                        child: new Text(
                                            "Michael Wang",
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis,
                                            style: CTextStyle.PMediumWhite
                                        )
                                    )
                                }
                            )
                        ),
                        new Positioned(
                            left: (MediaQuery.of(context: context).size.width - 104) / 2 - 6,
                            bottom: 152,
                            width: 104,
                            child: new Column(
                                children: new List<Widget> {
                                    new Container(
                                        width: 64,
                                        height: 64,
                                        decoration: new BoxDecoration(
                                            color: CColors.Red,
                                            borderRadius: BorderRadius.all(32)
                                        )
                                    ),
                                    new Padding(
                                        padding: EdgeInsets.only(top: 8),
                                        child: new Text(
                                            "Wu Xiaomu",
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis,
                                            style: CTextStyle.PMediumWhite
                                        )
                                    )
                                }
                            )
                        ),
                        new Positioned(
                            right: 24,
                            bottom: 96,
                            width: 96,
                            child: new Column(
                                children: new List<Widget> {
                                    new Container(
                                        width: 64,
                                        height: 64,
                                        decoration: new BoxDecoration(
                                            color: CColors.Red,
                                            borderRadius: BorderRadius.all(32)
                                        )
                                    ),
                                    new Padding(
                                        padding: EdgeInsets.only(top: 8),
                                        child: new Text(
                                            "樱花兔",
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis,
                                            style: CTextStyle.PMediumWhite
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