using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

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
}