using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
    public enum KingKongType {
        dailyCollection,
        leaderBoard,
        activity,
        blogger
    }

    public delegate void TypeCallback(KingKongType type);

    public delegate void StringCallback(string text);

    /// <summary>
    /// 金刚区
    /// </summary>
    public class KingKongView : StatelessWidget {
        public KingKongView(
            TypeCallback onPress,
            Key key = null
        ) : base(key: key) {
            this.onPress = onPress;
        }

        readonly TypeCallback onPress;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new Row(
                            children: new List<Widget> {
                                _buildKingKongItem("每日精选", "daily-collection",
                                    () => this.onPress(type: KingKongType.dailyCollection)),
                                _buildKingKongItem("榜单", "leader-board",
                                    () => this.onPress(type: KingKongType.leaderBoard)),
                                _buildKingKongItem("活动", "activity",
                                    () => this.onPress(type: KingKongType.activity)),
                                _buildKingKongItem("博主", "blogger",
                                    () => this.onPress(type: KingKongType.blogger))
                            }
                        ),
                        new CustomDivider(
                            height: 8,
                            color: CColors.Separator2
                        )
                    }
                )
            );
        }

        static Widget _buildKingKongItem(string title, string imageName, GestureTapCallback onPressItem) {
            return new Expanded(
                child: new GestureDetector(
                    onTap: onPressItem,
                    child: new Container(
                        color: CColors.Transparent,
                        padding: EdgeInsets.only(top: 16, bottom: 16),
                        child: new Column(
                            children: new List<Widget> {
                                new Container(
                                    margin: EdgeInsets.only(bottom: 8),
                                    child: new Stack(
                                        children: new List<Widget> {
                                            Image.asset(
                                                "image/kingkong-bg",
                                                width: 48,
                                                height: 48
                                            ),
                                            Positioned.fill(
                                                new Container(
                                                    padding: EdgeInsets.all(6),
                                                    child: Image.asset($"image/{imageName}")
                                                )
                                            )
                                        }
                                    )
                                ),
                                new Text(data: title, style: CTextStyle.PSmallBody4)
                            }
                        )
                    )
                )
            );
        }
    }

    /// <summary>
    /// 榜单
    /// </summary>
    public class LeaderBoard : StatelessWidget {
        public LeaderBoard(
            object data = null,
            GestureTapCallback onPressMore = null,
            StringCallback onPressItem = null,
            Key key = null
        ) : base(key: key) {
            this.data = data;
            this.onPressMore = onPressMore;
            this.onPressItem = onPressItem;
        }

        readonly object data;
        readonly GestureTapCallback onPressMore;
        readonly StringCallback onPressItem;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new MoreListTile(
                            "image/leader-board",
                            "当前无新内容，逛逛榜单吧",
                            EdgeInsets.only(16, 16),
                            onPress: this.onPressMore
                        ),
                        new Container(
                            height: 80,
                            margin: EdgeInsets.only(top: 16, bottom: 16),
                            child: ListView.builder(
                                scrollDirection: Axis.horizontal,
                                itemCount: 10,
                                itemBuilder: this._buildLeaderBoardItem
                            )
                        ),
                        new CustomDivider(
                            height: 8,
                            color: CColors.Separator2
                        )
                    }
                )
            );
        }

        Widget _buildLeaderBoardItem(BuildContext context, int index) {
            return new GestureDetector(
                onTap: () => this.onPressItem?.Invoke($"{index}"),
                child: new Container(
                    width: 160,
                    height: 80,
                    margin: EdgeInsets.only(index == 0 ? 16 : 0, right: 16),
                    decoration: new BoxDecoration(
                        borderRadius: BorderRadius.all(4)
                    ),
                    child: new ClipRRect(
                        borderRadius: BorderRadius.all(4),
                        child: new Stack(
                            children: new List<Widget> {
                                Positioned.fill(
                                    new Container(
                                        // Todo: v2.0.0 change index.ToString() to id 
                                        color: CColorUtils.GetCardColorFromId(index.ToString())
                                    )
                                ),
                                Image.asset(
                                    // Todo: v2.0.0 change index.ToString() to id
                                    CImageUtils.GetSpecificPatternImageNameFromId(index.ToString()),
                                    width: 160,
                                    height: 80,
                                    fit: BoxFit.fill
                                ),
                                Positioned.fill(
                                    new Padding(
                                        padding: EdgeInsets.all(16),
                                        child: new Text(
                                            "Editor GUI 编辑器入门",
                                            maxLines: 2,
                                            overflow: TextOverflow.ellipsis,
                                            style: CTextStyle.PLargeMediumWhite
                                        )
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }
    }

    /// <summary>
    /// 推荐博主
    /// </summary>
    public class RecommendBlogger : StatelessWidget {
        public RecommendBlogger(
            object data = null,
            GestureTapCallback onPressMore = null,
            StringCallback onPressItem = null,
            Key key = null
        ) : base(key: key) {
            this.data = data;
            this.onPressMore = onPressMore;
            this.onPressItem = onPressItem;
        }

        readonly object data;
        readonly GestureTapCallback onPressMore;
        readonly StringCallback onPressItem;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                height: 302,
                child: new Stack(
                    fit: StackFit.expand,
                    children: new List<Widget> {
                        Image.asset(
                            "image/recommend-blogger-bg",
                            fit: BoxFit.fill
                        ),
                        new Column(
                            children: new List<Widget> {
                                new MoreListTile(
                                    "image/blogger",
                                    "推荐博主",
                                    EdgeInsets.only(16, 16, bottom: 16),
                                    onPress: this.onPressMore
                                ),
                                new Container(
                                    height: 230,
                                    child: new ListView(
                                        scrollDirection: Axis.horizontal,
                                        children: new List<Widget> {
                                            this._buildBlogger("郭老师"),
                                            this._buildBlogger("PROS"),
                                            this._buildBlogger("PROS"),
                                            this._buildBlogger("PROS"),
                                            this._buildBlogger("PROS"),
                                            this._buildMoreBlogger(),
                                            new Container(width: 16)
                                        }
                                    )
                                )
                            }
                        )
                    }
                )
            );
        }

        Widget _buildBlogger(string bloggerName) {
            return new GestureDetector(
                onTap: () => this.onPressItem?.Invoke(""),
                child: new Container(
                    width: 160,
                    margin: EdgeInsets.only(16),
                    decoration: new BoxDecoration(
                        color: CColors.White,
                        borderRadius: BorderRadius.all(6)
                    ),
                    child: new ClipRRect(
                        borderRadius: BorderRadius.all(6),
                        child: new Column(
                            children: new List<Widget> {
                                new Container(
                                    height: 88,
                                    // Todo: v2.0.0 change bloggerName to id
                                    color: CColorUtils.GetSpecificDarkColorFromId(id: bloggerName),
                                    child: new Stack(
                                        fit: StackFit.expand,
                                        children: new List<Widget> {
                                            Image.asset(
                                                "image/blogger-avatar-pattern",
                                                fit: BoxFit.fill
                                            ),
                                            Positioned.fill(
                                                new Center(
                                                    child: new Container(
                                                        width: 64,
                                                        height: 64,
                                                        decoration: new BoxDecoration(
                                                            color: CColors.White,
                                                            borderRadius: BorderRadius.all(32)
                                                        )
                                                    )
                                                )
                                            )
                                        }
                                    )
                                ),
                                new Padding(
                                    padding: EdgeInsets.only(16, 16, 16, 4),
                                    child: new Text(data: bloggerName, style: CTextStyle.PXLargeMedium)
                                ),
                                new Text("粉丝234 • 文章2", style: CTextStyle.PSmallBody4),
                                new Text("C# • Unity Editor", style: CTextStyle.PSmallBody4),
                                new Padding(
                                    padding: EdgeInsets.only(top: 12),
                                    child: new FollowButton()
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildMoreBlogger() {
            return new GestureDetector(
                onTap: () => this.onPressMore?.Invoke(),
                child: new Container(
                    width: 160,
                    margin: EdgeInsets.only(16),
                    decoration: new BoxDecoration(
                        color: CColors.White,
                        borderRadius: BorderRadius.all(6)
                    ),
                    child: new ClipRRect(
                        borderRadius: BorderRadius.all(6),
                        child: new Stack(
                            fit: StackFit.expand,
                            children: new List<Widget> {
                                Image.asset(
                                    "image/blogger-more-pattern",
                                    fit: BoxFit.fill
                                ),
                                Positioned.fill(
                                    new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        children: new List<Widget> {
                                            new Container(
                                                width: 112,
                                                height: 40,
                                                child: new Stack(
                                                    children: new List<Widget> {
                                                        new Positioned(
                                                            right: 0,
                                                            bottom: 0,
                                                            child: new Container(
                                                                width: 40,
                                                                height: 40,
                                                                decoration: new BoxDecoration(
                                                                    color: CColors.Red,
                                                                    borderRadius: BorderRadius.all(20)
                                                                )
                                                            )
                                                        ),
                                                        new Positioned(
                                                            right: 36,
                                                            bottom: 0,
                                                            child: new Container(
                                                                width: 40,
                                                                height: 40,
                                                                decoration: new BoxDecoration(
                                                                    color: CColors.Green,
                                                                    borderRadius: BorderRadius.all(20)
                                                                )
                                                            )
                                                        ),
                                                        new Positioned(
                                                            right: 72,
                                                            bottom: 0,
                                                            child: new Container(
                                                                width: 40,
                                                                height: 40,
                                                                decoration: new BoxDecoration(
                                                                    color: CColors.PrimaryBlue,
                                                                    borderRadius: BorderRadius.all(20)
                                                                )
                                                            )
                                                        )
                                                    }
                                                )
                                            ),
                                            new Padding(
                                                padding: EdgeInsets.only(top: 16),
                                                child: new Text("查看更多", style: CTextStyle.PRegularBody2)
                                            )
                                        }
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }
    }

    /// <summary>
    /// 推荐榜单
    /// </summary>
    public class RecommendLeaderBoard : StatelessWidget {
        public RecommendLeaderBoard(
            object data = null,
            GestureTapCallback onPressMore = null,
            StringCallback onPressItem = null,
            Key key = null
        ) : base(key: key) {
            this.data = data;
            this.onPressMore = onPressMore;
            this.onPressItem = onPressItem;
        }

        readonly object data;
        readonly GestureTapCallback onPressMore;
        readonly StringCallback onPressItem;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                height: 184,
                child: new Stack(
                    fit: StackFit.expand,
                    children: new List<Widget> {
                        Image.asset(
                            "image/recommend-blogger-bg",
                            fit: BoxFit.fill
                        ),
                        new Column(
                            children: new List<Widget> {
                                new MoreListTile(
                                    "image/leader-board",
                                    "推荐榜单",
                                    EdgeInsets.only(16, 16),
                                    onPress: this.onPressMore
                                ),
                                new Container(
                                    padding: EdgeInsets.all(16),
                                    child: new Stack(
                                        children: new List<Widget> {
                                            new Column(
                                                children: new List<Widget> {
                                                    new Container(height: 8),
                                                    new Container(
                                                        height: 104,
                                                        decoration: new BoxDecoration(
                                                            new Color(0xFF3B516A),
                                                            borderRadius: BorderRadius.all(6)
                                                        )
                                                    )
                                                }
                                            ),
                                            new Positioned(
                                                right: 16,
                                                bottom: 8,
                                                child: new Text(
                                                    "UNITY",
                                                    style: new TextStyle(
                                                        fontSize: 40,
                                                        fontFamily: "Roboto-Bold",
                                                        color: new Color(0xFF4F6378)
                                                    )
                                                )
                                            ),
                                            Positioned.fill(
                                                new Row(
                                                    children: new List<Widget> {
                                                        new Padding(
                                                            padding: EdgeInsets.only(16, right: 16, bottom: 16),
                                                            child: new CoverImages(
                                                                80,
                                                                0,
                                                                8,
                                                                8
                                                            )
                                                        ),
                                                        new Expanded(
                                                            child: new Column(
                                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                                children: new List<Widget> {
                                                                    new Container(height: 20),
                                                                    new Text(
                                                                        "HDRP高清渲染管线-学习资料汇总",
                                                                        maxLines: 2,
                                                                        overflow: TextOverflow.ellipsis,
                                                                        style: new TextStyle(
                                                                            height: 1.27f,
                                                                            fontSize: 20,
                                                                            fontFamily: "Roboto-Medium",
                                                                            color: CColors.White
                                                                        )
                                                                    ),
                                                                    new Container(height: 4),
                                                                    new Text(
                                                                        "文章 30",
                                                                        style: new TextStyle(
                                                                            height: 1.53f,
                                                                            fontSize: 12,
                                                                            fontFamily: "Roboto-Regular",
                                                                            color: new Color(0xFFCCCCCC)
                                                                        )
                                                                    )
                                                                }
                                                            )
                                                        )
                                                    }
                                                )
                                            )
                                        }
                                    )
                                )
                            }
                        )
                    }
                )
            );
        }
    }

    /// <summary>
    /// 刷新分割线
    /// </summary>
    public class RefreshDivider : StatelessWidget {
        public RefreshDivider(
            GestureTapCallback onPress = null,
            Key key = null
        ) : base(key: key) {
            this.onPress = onPress;
        }

        readonly GestureTapCallback onPress;

        public override Widget build(BuildContext context) {
            return new Container(
                height: 54,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: new List<Widget> {
                        new Container(
                            width: 24,
                            height: 1,
                            color: CColors.LoadingGrey,
                            margin: EdgeInsets.only(right: 8)
                        ),
                        new RichText(
                            text: new TextSpan(
                                children: new List<TextSpan> {
                                    new TextSpan(
                                        "上次看到这里哟~",
                                        new TextStyle(
                                            fontSize: 14,
                                            fontFamily: "Roboto-Regular",
                                            color: CColors.TextBody4
                                        )
                                    ),
                                    new TextSpan(
                                        " 点击刷新",
                                        new TextStyle(
                                            fontSize: 14,
                                            fontFamily: "Roboto-Regular",
                                            color: CColors.PrimaryBlue
                                        ),
                                        recognizer: new TapGestureRecognizer {
                                            onTap = this.onPress
                                        }
                                    )
                                }
                            )
                        ),
                        new Container(
                            width: 24,
                            height: 1,
                            color: CColors.LoadingGrey,
                            margin: EdgeInsets.only(8)
                        )
                    }
                )
            );
        }
    }

    public class MoreListTile : StatelessWidget {
        public MoreListTile(
            string imageName,
            string title,
            EdgeInsets padding = null,
            GestureTapCallback onPress = null,
            Key key = null
        ) : base(key: key) {
            this.imageName = imageName;
            this.title = title;
            this.padding = padding;
            this.onPress = onPress;
        }

        readonly string imageName;
        readonly string title;
        readonly EdgeInsets padding;
        readonly GestureTapCallback onPress;

        public override Widget build(BuildContext context) {
            return new Padding(
                padding: this.padding,
                child: new Row(
                    children: new List<Widget> {
                        Image.asset(
                            name: this.imageName,
                            width: 24,
                            height: 24
                        ),
                        new Padding(
                            padding: EdgeInsets.only(4),
                            child: new Text(
                                data: this.title,
                                style: new TextStyle(
                                    fontSize: 16,
                                    fontFamily: "Roboto-Medium",
                                    color: CColors.TextBody
                                )
                            )
                        ),
                        new Flexible(child: new Container()),
                        new GestureDetector(
                            onTap: this.onPress,
                            child: new Container(
                                color: CColors.Transparent,
                                child: new Row(
                                    children: new List<Widget> {
                                        new Padding(
                                            padding: EdgeInsets.only(16),
                                            child: new Text(
                                                "查看更多",
                                                style: new TextStyle(
                                                    fontSize: 14,
                                                    fontFamily: "Roboto-Regular",
                                                    color: CColors.TextBody4
                                                )
                                            )
                                        ),
                                        new Padding(
                                            padding: EdgeInsets.only(right: 8),
                                            child: new Icon(
                                                icon: Icons.chevron_right,
                                                size: 20,
                                                color: Color.fromRGBO(199, 203, 207, 1)
                                            )
                                        )
                                    }
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}