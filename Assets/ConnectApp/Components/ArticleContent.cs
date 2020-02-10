using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using RSG;
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
    public class KingKongView : StatefulWidget {
        public KingKongView(
            DateTime? leaderBoardUpdatedTime,
            TypeCallback onPress,
            Key key = null
        ) : base(key: key) {
            this.leaderBoardUpdatedTime = leaderBoardUpdatedTime;
            this.onPress = onPress;
        }

        public readonly DateTime? leaderBoardUpdatedTime;
        public readonly TypeCallback onPress;

        public override State createState() {
            return new _KingKongViewState();
        }
    }

    public class _KingKongViewState : State<KingKongView> {
        public override void initState() {
            base.initState();
            Promise.Delayed(TimeSpan.FromMilliseconds(1)).Then(() => {
                var rect = this.context.getContextRect();
                var kingKongTypes = PreferencesManager.initKingKongType();
                if (!kingKongTypes.Contains(item: KingKongType.dailyCollection)) {
                    this._showDailyCollectionDialog(rect: rect, kingKongTypes: kingKongTypes);
                }
                else if (!kingKongTypes.Contains(item: KingKongType.leaderBoard)) {
                    this._showLeaderBoardDialog(rect: rect, kingKongTypes: kingKongTypes);
                }
                else if (!kingKongTypes.Contains(item: KingKongType.blogger)) {
                    this._showBloggerDialog(rect: rect);
                }
            });
        }

        void _showDailyCollectionDialog(Rect rect, List<KingKongType> kingKongTypes) {
            PreferencesManager.updateKingKongType(type: KingKongType.dailyCollection);
            CustomDialogUtils.showCustomDialog(
                child: new Bubble(
                    "每日精选1篇文章，记得查看哦",
                    rect.width / 8.0f,
                    rect.bottom - 16 - CCommonUtils.getSafeAreaTopPadding(context: this.context),
                    16
                ),
                barrierDismissible: true,
                onPop: () => {
                    if (!kingKongTypes.Contains(item: KingKongType.leaderBoard)) {
                        this._showLeaderBoardDialog(rect: rect, kingKongTypes: kingKongTypes);
                    }
                }
            );
        }

        void _showLeaderBoardDialog(Rect rect, List<KingKongType> kingKongTypes) {
            PreferencesManager.updateKingKongType(type: KingKongType.leaderBoard);
            CustomDialogUtils.showCustomDialog(
                child: new Bubble(
                    "榜单上线，合辑、专栏、博主在这里哦",
                    rect.width / 8.0f + rect.width / 4.0f,
                    rect.bottom - 16 - CCommonUtils.getSafeAreaTopPadding(context: this.context),
                    contentRight: 16
                ),
                barrierDismissible: true,
                onPop: () => {
                    if (!kingKongTypes.Contains(item: KingKongType.blogger)) {
                        this._showBloggerDialog(rect: rect);
                    }
                }
            );
        }

        void _showBloggerDialog(Rect rect) {
            PreferencesManager.updateKingKongType(type: KingKongType.blogger);
            CustomDialogUtils.showCustomDialog(
                child: new Bubble(
                    "这里可以发现很多大牛博主哦",
                    rect.width - rect.width / 8.0f,
                    rect.bottom - 16 - CCommonUtils.getSafeAreaTopPadding(context: this.context),
                    contentRight: 16
                ),
                barrierDismissible: true
            );
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new Row(
                            children: new List<Widget> {
                                this._buildKingKongItem("每日精选", "daily-collection",
                                    () => this.widget.onPress(type: KingKongType.dailyCollection)),
                                this._buildKingKongItem("榜单", "leader-board",
                                    () => this.widget.onPress(type: KingKongType.leaderBoard)),
                                this._buildKingKongItem("活动", "activity",
                                    () => this.widget.onPress(type: KingKongType.activity)),
                                this._buildKingKongItem("博主", "blogger",
                                    () => this.widget.onPress(type: KingKongType.blogger))
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

        Widget _buildKingKongItem(string title, string imageName, GestureTapCallback onPressItem) {
            Widget newDot;
            if (title == "榜单" && this.widget.leaderBoardUpdatedTime.HasValue &&
                LocalDataManager.needNoticeNewLeaderBoard(dateTime: this.widget.leaderBoardUpdatedTime.Value)) {
                newDot = new Positioned(
                    top: 0,
                    right: 0,
                    child: new Container(
                        width: 18,
                        height: 18,
                        decoration: new BoxDecoration(
                            color: CColors.Error,
                            borderRadius: BorderRadius.only(9, 9, 9)
                        ),
                        alignment: Alignment.center,
                        child: new Text(
                            "新",
                            style: new TextStyle(
                                fontSize: 10,
                                fontFamily: "Roboto-Bold",
                                color: CColors.White
                            )
                        )
                    )
                );
            }
            else {
                newDot = Positioned.fill(new Container());
            }

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
                                            new Padding(
                                                padding: EdgeInsets.symmetric(horizontal: 9),
                                                child: Image.asset(
                                                    "image/kingkong-bg",
                                                    width: 48,
                                                    height: 48
                                                )
                                            ),
                                            newDot,
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
            List<string> data,
            Dictionary<string, RankData> rankDict,
            Dictionary<string, FavoriteTag> favoriteTagDict,
            GestureTapCallback onPressMore = null,
            StringCallback onPressItem = null,
            Key key = null
        ) : base(key: key) {
            this.data = data;
            this.rankDict = rankDict;
            this.favoriteTagDict = favoriteTagDict;
            this.onPressMore = onPressMore;
            this.onPressItem = onPressItem;
        }

        readonly List<string> data;
        readonly Dictionary<string, RankData> rankDict;
        readonly Dictionary<string, FavoriteTag> favoriteTagDict;
        readonly GestureTapCallback onPressMore;
        readonly StringCallback onPressItem;

        public override Widget build(BuildContext context) {
            if (this.data.isNullOrEmpty()) {
                return new Container();
            }

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
                                itemCount: this.data.Count,
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
            var collectionId = this.data[index: index];
            var rankData = this.rankDict.ContainsKey(key: collectionId)
                ? this.rankDict[key: collectionId]
                : new RankData();
            var favoriteTag = this.favoriteTagDict.ContainsKey(key: rankData.itemId)
                ? this.favoriteTagDict[key: rankData.itemId]
                : new FavoriteTag();

            return new GestureDetector(
                onTap: () => this.onPressItem?.Invoke(text: rankData.id),
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
                                    new Container(color: CColorUtils.GetCardColorFromId(id: collectionId))
                                ),
                                Image.asset(
                                    CImageUtils.GetSpecificPatternImageNameFromId(id: collectionId),
                                    width: 160,
                                    height: 80,
                                    fit: BoxFit.fill
                                ),
                                Positioned.fill(
                                    new Padding(
                                        padding: EdgeInsets.all(16),
                                        child: new Text(
                                            rankData.resetTitle.isNotEmpty() ? rankData.resetTitle : favoriteTag.name,
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
            List<string> bloggerIds,
            Dictionary<string, RankData> rankDict,
            Dictionary<string, User> userDict,
            Dictionary<string, bool> followMap,
            bool isLoggedIn,
            StringCallback followUser = null,
            StringCallback unFollowUser = null,
            VoidCallback pushToLogin = null,
            GestureTapCallback onPressBloggerMore = null,
            GestureTapCallback onPressMore = null,
            StringCallback onPressItem = null,
            Key key = null
        ) : base(key: key) {
            this.bloggerIds = bloggerIds;
            this.rankDict = rankDict;
            this.userDict = userDict;
            this.followMap = followMap;
            this.isLoggedIn = isLoggedIn;
            this.followUser = followUser;
            this.unFollowUser = unFollowUser;
            this.pushToLogin = pushToLogin;
            this.onPressBloggerMore = onPressBloggerMore;
            this.onPressMore = onPressMore;
            this.onPressItem = onPressItem;
        }

        readonly List<string> bloggerIds;
        readonly Dictionary<string, RankData> rankDict;
        readonly Dictionary<string, User> userDict;
        readonly Dictionary<string, bool> followMap;
        readonly bool isLoggedIn;
        readonly StringCallback followUser;
        readonly StringCallback unFollowUser;
        readonly VoidCallback pushToLogin;
        readonly GestureTapCallback onPressBloggerMore;
        readonly GestureTapCallback onPressMore;
        readonly StringCallback onPressItem;

        void _onFollow(UserType userType, string userId) {
            if (this.isLoggedIn) {
                if (userType == UserType.follow) {
                    ActionSheetUtils.showModalActionSheet(
                        new ActionSheet(
                            title: "确定不再关注？",
                            items: new List<ActionSheetItem> {
                                new ActionSheetItem("确定", type: ActionType.normal,
                                    () => this.unFollowUser?.Invoke(text: userId)),
                                new ActionSheetItem("取消", type: ActionType.cancel)
                            }
                        )
                    );
                }

                if (userType == UserType.unFollow) {
                    this.followUser?.Invoke(text: userId);
                }
            }
            else {
                this.pushToLogin?.Invoke();
            }
        }

        public override Widget build(BuildContext context) {
            if (this.bloggerIds.isNullOrEmpty()) {
                return new Container();
            }

            var children = new List<Widget> {
                new SizedBox(width: 16)
            };
            if (this.bloggerIds.Count <= 3) {
                this.bloggerIds.ForEach(bloggerId => {
                    var rankData = this.rankDict.ContainsKey(key: bloggerId)
                        ? this.rankDict[key: bloggerId]
                        : new RankData();
                    if (this.userDict.ContainsKey(key: rankData.itemId)) {
                        var user = this.userDict[key: rankData.itemId];
                        children.Add(this._buildBlogger(user: user, resetTitle: rankData.resetTitle));
                    }
                });
            }
            else {
                var _showBloggerCount = 3;
                for (int index = 0; index < _showBloggerCount; index++) {
                    var bloggerId = this.bloggerIds[index: index];
                    var rankData = this.rankDict.ContainsKey(key: bloggerId)
                        ? this.rankDict[key: bloggerId]
                        : new RankData();
                    if (this.userDict.ContainsKey(key: rankData.itemId)) {
                        var user = this.userDict[key: rankData.itemId];
                        children.Add(this._buildBlogger(user: user, resetTitle: rankData.resetTitle));
                    }
                }

                children.Add(this._buildMoreBlogger(this.bloggerIds.GetRange(_showBloggerCount,
                    this.bloggerIds.Count - _showBloggerCount > 3 ? 3 : this.bloggerIds.Count - _showBloggerCount)));
            }

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
                                    onPress: this.onPressBloggerMore
                                ),
                                new Container(
                                    height: 230,
                                    child: new ListView(
                                        scrollDirection: Axis.horizontal,
                                        children: children
                                    )
                                )
                            }
                        )
                    }
                )
            );
        }

        Widget _buildBlogger(User user, string resetTitle) {
            UserType userType = UserType.unFollow;
            if (!this.isLoggedIn) {
                userType = UserType.unFollow;
            }
            else {
                if (UserInfoManager.getUserInfo().userId == user.id) {
                    userType = UserType.me;
                }
                else if (user.followUserLoading ?? false) {
                    userType = UserType.loading;
                }
                else if (this.followMap.ContainsKey(key: user.id)) {
                    userType = UserType.follow;
                }
            }

            return new GestureDetector(
                onTap: () => this.onPressItem?.Invoke(text: user.id),
                child: new Container(
                    width: 160,
                    margin: EdgeInsets.only(right: 16),
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
                                    color: CColorUtils.GetSpecificDarkColorFromId(id: user.id),
                                    child: new Stack(
                                        fit: StackFit.expand,
                                        children: new List<Widget> {
                                            Image.asset(
                                                "image/blogger-avatar-pattern",
                                                fit: BoxFit.fill
                                            ),
                                            Positioned.fill(
                                                new Center(
                                                    child: Avatar.User(user: user, 64, true)
                                                )
                                            )
                                        }
                                    )
                                ),
                                new Padding(
                                    padding: EdgeInsets.only(16, 16, 16, 4),
                                    child: new Text(data: user.fullName, style: CTextStyle.PXLargeMedium, maxLines: 1)
                                ),
                                new Text(
                                    $"粉丝{user.followCount ?? 0} • 文章{user.articleCount ?? 0}",
                                    style: CTextStyle.PSmallBody4
                                ),
                                new Text(resetTitle ?? "", style: CTextStyle.PSmallBody4),
                                new Padding(
                                    padding: EdgeInsets.only(top: 12),
                                    child: new FollowButton(
                                        userType: userType,
                                        () => this._onFollow(userType: userType, userId: user.id)
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildMoreBlogger(List<string> bloggerIds) {
            return new GestureDetector(
                onTap: () => this.onPressMore?.Invoke(),
                child: new Container(
                    width: 160,
                    margin: EdgeInsets.only(right: 16),
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
                                        children: this._buildAvatars(bloggerIds)
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }

        List<Widget> _buildAvatars(List<string> bloggerIds) {
            var list = new List<Widget>();
            var items = new List<Widget>();
            bloggerIds.ForEach(bloggerId => {
                var index = bloggerIds.IndexOf(bloggerId);
                var rankData = this.rankDict.ContainsKey(key: bloggerId)
                    ? this.rankDict[key: bloggerId]
                    : new RankData();
                if (this.userDict.ContainsKey(key: rankData.itemId)) {
                    var user = this.userDict[key: rankData.itemId];
                    var left = 0;
                    if (index == 1) {
                        left = 36;
                    } else if (index == 2) {
                        left = 72;
                    }
                    items.Add(new Positioned(
                            Avatar.User(user: user, 44, true),
                            left: left
                        )
                    );
                }
            });
            items.Reverse();
            var width = 0;
            if (bloggerIds.Count == 1) {
                width = 44;
            }
            else if (bloggerIds.Count == 2) {
                width = 84;
            }
            else if (bloggerIds.Count == 3) {
                width = 124;
            }
            var avatar = new Container(
                height: 44,
                width: width,
                child: new Stack(children: items)
            );
            list.Add(avatar);
            list.Add(new Padding(
                    padding: EdgeInsets.only(top: 16),
                    child: new Text("查看更多", style: CTextStyle.PRegularBody2)
                )
            );

            return list;
        }
    }


    /// <summary>
    /// 推荐榜单
    /// </summary>
    public class RecommendLeaderBoard : StatelessWidget {
        public RecommendLeaderBoard(
            List<string> data,
            Dictionary<string, RankData> rankDict,
            Dictionary<string, FavoriteTag> favoriteTagDict,
            Dictionary<string, FavoriteTagArticle> favoriteTagArticleDict,
            GestureTapCallback onPressMore = null,
            StringCallback onPressItem = null,
            Key key = null
        ) : base(key: key) {
            this.data = data;
            this.rankDict = rankDict;
            this.favoriteTagDict = favoriteTagDict;
            this.favoriteTagArticleDict = favoriteTagArticleDict;
            this.onPressMore = onPressMore;
            this.onPressItem = onPressItem;
        }

        readonly List<string> data;
        readonly Dictionary<string, RankData> rankDict;
        readonly Dictionary<string, FavoriteTag> favoriteTagDict;
        readonly Dictionary<string, FavoriteTagArticle> favoriteTagArticleDict;
        readonly GestureTapCallback onPressMore;
        readonly StringCallback onPressItem;

        public override Widget build(BuildContext context) {
            if (this.data.isNullOrEmpty() || this.favoriteTagDict.isNullOrEmpty()) {
                return new Container();
            }

            var collectionId = this.data[0];
            var rankData = this.rankDict.ContainsKey(key: collectionId)
                ? this.rankDict[key: collectionId]
                : new RankData();
            var favoriteTagArticle = this.favoriteTagArticleDict.ContainsKey(key: rankData.itemId)
                ? this.favoriteTagArticleDict[key: rankData.itemId]
                : new FavoriteTagArticle();
            var favoriteTag = this.favoriteTagDict.ContainsKey(key: rankData.itemId)
                ? this.favoriteTagDict[key: rankData.itemId]
                : new FavoriteTag();
            var title = rankData.resetTitle.isNotEmpty() ? rankData.resetTitle : favoriteTag.name;
            var images = new List<string>();
            favoriteTagArticle.list.ForEach(article => { images.Add(item: article.thumbnail.url); });
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
                                new GestureDetector(
                                    onTap: () => this.onPressItem?.Invoke(text: rankData.id),
                                    child: new Container(
                                        color: CColors.Transparent,
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
                                                                    images: images,
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
                                                                            data: title,
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
                                                                            $"文章 {favoriteTag.stasitics.count}",
                                                                            style: new TextStyle(
                                                                                height: 1.53f,
                                                                                fontSize: 12,
                                                                                fontFamily: "Roboto-Regular",
                                                                                color: new Color(0xFFCCCCCC)
                                                                            )
                                                                        )
                                                                    }
                                                                )
                                                            ),
                                                            new SizedBox(width: 16)
                                                        }
                                                    )
                                                )
                                            }
                                        )
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