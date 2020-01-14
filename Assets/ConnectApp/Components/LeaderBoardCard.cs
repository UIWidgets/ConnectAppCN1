using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
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
            RankData collection,
            Dictionary<string, FavoriteTag> favoriteTags,
            Dictionary<string, FavoriteTagArticle> favoriteTagArticles,
            int index = 0,
            GestureTapCallback onPress = null,
            Key key = null
        ) : base(key: key) {
            this.collection = collection;
            this.favoriteTags = favoriteTags;
            this.favoriteTagArticles = favoriteTagArticles;
            this.index = index;
            this.onPress = onPress;
        }

        readonly RankData collection;
        readonly Dictionary<string, FavoriteTag> favoriteTags;
        readonly Dictionary<string, FavoriteTagArticle> favoriteTagArticles;
        readonly int index;
        readonly GestureTapCallback onPress;

        public override Widget build(BuildContext context) {
            var favoriteTag = this.favoriteTags[key: this.collection.itemId];
            var favoriteTagArticle = this.favoriteTagArticles[key: this.collection.itemId];

            var images = new List<string>();
            favoriteTagArticle.list.ForEach(article => {
                images.Add(item: article.thumbnail.url);
            });

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
                                            this.collection.resetTitle.isNotEmpty() ? this.collection.resetTitle : favoriteTag.name,
                                            maxLines: 2,
                                            overflow: TextOverflow.ellipsis,
                                            style: CTextStyle.PXLargeMediumBody
                                        ),
                                        new Text(
                                            $"作者 {favoriteTagArticle.authorCount} • 文章 {favoriteTag.stasitics.count}",
                                            style: CTextStyle.PSmallBody4
                                        )
                                    }
                                )
                            ),
                            new SizedBox(width: 16),
                            new CoverImages(
                                images: images,
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
            User blogger,
            int index = 0,
            GestureTapCallback onPress = null,
            Key key = null
        ) : base(key: key) {
            this.blogger = blogger;
            this.index = index;
            this.onPress = onPress;
        }

        readonly User blogger;
        readonly int index;
        readonly GestureTapCallback onPress;

        public override Widget build(BuildContext context) {
            Widget titleWidget;
            if (this.blogger.title.isNotEmpty()) {
                titleWidget = new Text(
                    data: this.blogger.title,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    style: CTextStyle.PRegularBody4
                );
            }
            else {
                titleWidget = new Container();
            }

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
                                child: Avatar.User(user: this.blogger, 56)
                            ),
                            new Expanded(
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Text(
                                            data: this.blogger.fullName,
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis,
                                            style: new TextStyle(
                                                height: 1.33f,
                                                fontSize: 16,
                                                fontFamily: "Roboto-Medium",
                                                color: CColors.TextBody
                                            )
                                        ),
                                        titleWidget
                                    }
                                )
                            ),
                            new Padding(
                                padding: EdgeInsets.only(16),
                                child: new Text(
                                    $"文章 {this.blogger.articleCount ?? 0} • 赞 {this.blogger.likeCount ?? 0}",
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
            List<string> bloggerIds,
            Dictionary<string, User> userDict,
            Key key = null
        ) : base(key: key) {
            this.bloggerIds = bloggerIds;
            this.userDict = userDict;
        }

        readonly List<string> bloggerIds;
        readonly Dictionary<string, User> userDict;

        public override Widget build(BuildContext context) {
            if (this.bloggerIds == null) {
                return new Container();
            }

            Widget firstAvatar;
            if (this.bloggerIds.Count > 0) {
                var user = this.userDict[this.bloggerIds[0]];
                firstAvatar = _buildAvatar(user: user);
            }
            else {
                firstAvatar = new Container();
            }

            Widget secondAvatar;
            if (this.bloggerIds.Count > 1) {
                var user = this.userDict[this.bloggerIds[1]];
                secondAvatar = _buildAvatar(user: user);
            }
            else {
                secondAvatar = new Container();
            }

            Widget thirdAvatar;
            if (this.bloggerIds.Count > 2) {
                var user = this.userDict[this.bloggerIds[2]];
                thirdAvatar = _buildAvatar(user: user);
            }
            else {
                thirdAvatar = new Container();
            }
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
                            left: 8,
                            bottom: 116,
                            width: 96,
                            child: secondAvatar
                        ),
                        new Positioned(
                            left: (MediaQuery.of(context: context).size.width - 104) / 2 - 6,
                            bottom: 152,
                            width: 104,
                            child: firstAvatar
                        ),
                        new Positioned(
                            right: 8,
                            bottom: 96,
                            width: 96,
                            child: thirdAvatar
                        )
                    }
                )
            );
        }

        static Widget _buildAvatar(User user) {
            return new Column(
                children: new List<Widget> {
                    Avatar.User(user: user, 64, true),
                    new Padding(
                        padding: EdgeInsets.only(top: 8),
                        child: new Text(
                            data: user.fullName,
                            maxLines: 1,
                            overflow: TextOverflow.ellipsis,
                            style: CTextStyle.PMediumWhite
                        )
                    )
                }
            );
        }
    }
}