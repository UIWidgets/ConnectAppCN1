using System.Collections.Generic;
using ConnectApp.Components.LikeButton.Utils;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class FollowArticleCard : StatelessWidget {
        FollowArticleCard(
            Article article,
            User user = null,
            string license = null,
            Team team = null,
            bool isLike = false,
            UserType userType = UserType.me,
            OwnerType type = OwnerType.user,
            GestureTapCallback onTap = null,
            GestureTapCallback avatarCallBack = null,
            GestureTapCallback followCallBack = null,
            GestureTapCallback likeCallBack = null,
            GestureTapCallback commentCallBack = null,
            GestureTapCallback moreCallBack = null,
            Key key = null
        ) : base(key: key) {
            this.article = article;
            this.user = user ?? new User();
            this.license = license;
            this.team = team ?? new Team();
            this.isLike = isLike;
            this.userType = userType;
            this.type = type;
            this.onTap = onTap;
            this.avatarCallBack = avatarCallBack;
            this.followCallBack = followCallBack;
            this.likeCallBack = likeCallBack;
            this.commentCallBack = commentCallBack;
            this.moreCallBack = moreCallBack;
        }

        public static FollowArticleCard User(
            Article article,
            User user = null,
            string license = null,
            bool isLike = false,
            UserType userType = UserType.me,
            GestureTapCallback onTap = null,
            GestureTapCallback avatarCallBack = null,
            GestureTapCallback followCallBack = null,
            GestureTapCallback likeCallBack = null,
            GestureTapCallback commentCallBack = null,
            GestureTapCallback moreCallBack = null,
            Key key = null
        ) {
            return new FollowArticleCard(
                article: article,
                user: user,
                license: license,
                isLike: isLike,
                userType: userType,
                type: OwnerType.user,
                onTap: onTap,
                avatarCallBack: avatarCallBack,
                followCallBack: followCallBack,
                likeCallBack: likeCallBack,
                commentCallBack: commentCallBack,
                moreCallBack: moreCallBack,
                key: key
            );
        }

        public static FollowArticleCard Team(
            Article article,
            Team team = null,
            bool isLike = false,
            UserType userType = UserType.me,
            GestureTapCallback onTap = null,
            GestureTapCallback avatarCallBack = null,
            GestureTapCallback followCallBack = null,
            GestureTapCallback likeCallBack = null,
            GestureTapCallback commentCallBack = null,
            GestureTapCallback moreCallBack = null,
            Key key = null
        ) {
            return new FollowArticleCard(
                article: article,
                team: team,
                isLike: isLike,
                userType: userType,
                type: OwnerType.team,
                onTap: onTap,
                avatarCallBack: avatarCallBack,
                followCallBack: followCallBack,
                likeCallBack: likeCallBack,
                commentCallBack: commentCallBack,
                moreCallBack: moreCallBack,
                key: key
            );
        }

        readonly Article article;
        readonly User user;
        readonly string license;
        readonly Team team;
        readonly bool isLike;
        readonly UserType userType;
        readonly OwnerType type;
        readonly GestureTapCallback onTap;
        readonly GestureTapCallback avatarCallBack;
        readonly GestureTapCallback followCallBack;
        readonly GestureTapCallback likeCallBack;
        readonly GestureTapCallback commentCallBack;
        readonly GestureTapCallback moreCallBack;

        public override Widget build(BuildContext context) {
            if (this.article == null) {
                return new Container();
            }

            return new Column(
                children: new List<Widget> {
                    new GestureDetector(
                        onTap: this.onTap,
                        child: new Container(
                            color: CColors.White,
                            child: new Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    this._buildAvatar(),
                                    this._buildArticleInfo(),
                                    this._buildBottom()
                                }
                            )
                        )
                    ),
                    new CustomDivider(
                        color: CColors.Background,
                        height: 8
                    )
                }
            );
        }

        Widget _buildAvatar() {
            Widget rightWidget;
            if (this.userType == UserType.me || this.userType == UserType.follow) {
                var time = this.article.publishedTime;
                rightWidget = new Text(
                    $"{DateConvert.DateStringFromNow(dt: time)}",
                    style: CTextStyle.PSmallBody5
                );
            }
            else {
                rightWidget = new FollowButton(
                    userType: this.userType,
                    onFollow: this.followCallBack
                );
            }

            var avatar = this.type == OwnerType.user
                ? Avatar.User(
                    user: this.user,
                    38
                )
                : Avatar.Team(
                    team: this.team,
                    38
                );

            var badge = this.type == OwnerType.user
                ? CImageUtils.GenBadgeImage(
                    badges: this.user.badges,
                    license: this.license,
                    EdgeInsets.only(4)
                )
                : CImageUtils.GenBadgeImage(
                    badges: this.team.badges,
                    null,
                    EdgeInsets.only(4)
                );

            Widget titleWidget = new Container();
            if (this.user.title != null && this.user.title.isNotEmpty() && this.type == OwnerType.user) {
                titleWidget = new Text(
                    data: this.user.title,
                    style: CTextStyle.PSmallBody4,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis
                );
            }

            return new Container(
                padding: EdgeInsets.only(16, 16, 16),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new GestureDetector(
                            child: avatar,
                            onTap: this.avatarCallBack
                        ),
                        new Expanded(
                            child: new GestureDetector(
                                onTap: this.avatarCallBack,
                                child: new Container(
                                    color: CColors.Transparent,
                                    margin: EdgeInsets.only(8, right: 16),
                                    child: new Column(
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Row(
                                                children: new List<Widget> {
                                                    new Flexible(
                                                        child: new Text(
                                                            this.type == OwnerType.user
                                                                ? this.user.fullName ?? this.user.name ?? "佚名"
                                                                : this.team.name,
                                                            style: CTextStyle.PLargeBody.merge(
                                                                new TextStyle(height: 1)),
                                                            maxLines: 1,
                                                            overflow: TextOverflow.ellipsis
                                                        )
                                                    ),
                                                    badge
                                                }
                                            ),
                                            titleWidget
                                        }
                                    )
                                )
                            )
                        ),
                        rightWidget
                    }
                )
            );
        }

        Widget _buildArticleInfo() {
            const float imageWidth = 100;
            const float imageHeight = 66;
            const float borderRadius = 4;
            var imageUrl = this.article.thumbnail.url.EndsWith(".gif")
                ? this.article.thumbnail.url
                : CImageUtils.SuitableSizeImageUrl(imageWidth: imageWidth, imageUrl: this.article.thumbnail.url);
            return new Container(
                padding: EdgeInsets.only(16, 10, 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text(
                            data: this.article.title,
                            style: CTextStyle.PXLargeMedium.copyWith(height:1.44f),
                            maxLines: 2,
                            overflow: TextOverflow.ellipsis
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 10),
                            child: new Row(
                                children: new List<Widget> {
                                    new Expanded(
                                        child: new Container(
                                            height: imageHeight,
                                            child: new Column(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: new List<Widget> {
                                                    new Text(
                                                        data: this.article.subTitle,
                                                        style: CTextStyle.PRegularBody3,
                                                        maxLines: 3,
                                                        overflow: TextOverflow.ellipsis
                                                    )
                                                }
                                            )
                                        )
                                    ),
                                    new Container(
                                        margin: EdgeInsets.only(8),
                                        child: new PlaceholderImage(
                                            imageUrl: imageUrl,
                                            width: imageWidth,
                                            height: imageHeight,
                                            borderRadius: borderRadius,
                                            fit: BoxFit.cover,
                                            color: CColorUtils.GetSpecificDarkColorFromId(id: this.article.id)
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }

        Widget _buildBottom() {
            int articleLikeCount = this.article.appLikeCount > 0 ? this.article.appLikeCount : 0;

            return new Container(
                height: 56,
                child: new Stack(
                    alignment: Alignment.center,
                    children: new List<Widget> {
                        new Positioned(
                            left: 0,
                            child: new GestureDetector(
                                onTap: () => this.likeCallBack(),
                                child: new Container(
                                    color: CColors.Transparent,
                                    height: 56,
                                    padding: EdgeInsets.only(16, right: 20),
                                    child: new IgnorePointer(
                                        child: new Row(
                                            children: new List<Widget> {
                                                new Container(
                                                    padding: EdgeInsets.only(bottom: 4),
                                                    child: new Icon(
                                                        this.isLike ? Icons.thumb_bold : Icons.thumb_line, 
                                                        size: 24, 
                                                        color: this.isLike ? CColors.Thumb : CColors.Icon
                                                    )
                                                ),
                                                new Container(
                                                    margin: EdgeInsets.only(6),
                                                    child: new Text(
                                                        CStringUtils.CountToString(count: articleLikeCount,
                                                            "点赞"),
                                                        style: CTextStyle.PRegularBody5.merge(new TextStyle(height: 1))
                                                    )
                                                )
                                            }
                                        )
                                    )
                                )
                            )
                        ),
                        new Positioned(
                            left: 96,
                            child: new Container(
                                child: new GestureDetector(
                                    onTap: this.commentCallBack,
                                    child: new Container(
                                        color: CColors.Transparent,
                                        height: 56,
                                        padding: EdgeInsets.only(right: 20),
                                        child: new Row(
                                            children: new List<Widget> {
                                                new Icon(icon: Icons.comment, size: 24, color: CColors.Icon),
                                                new Container(
                                                    margin: EdgeInsets.only(6),
                                                    child: new Text(
                                                        CStringUtils.CountToString(count: this.article.commentCount,
                                                            "评论"),
                                                        style: CTextStyle.PRegularBody5.merge(new TextStyle(height: 1))
                                                    )
                                                )
                                            }
                                        )
                                    )
                                )
                            )
                        ),
                        new Align(
                            alignment: Alignment.centerRight,
                            child: new GestureDetector(
                                child: new Container(
                                    color: CColors.Transparent,
                                    height: 56,
                                    padding: EdgeInsets.symmetric(horizontal: 16),
                                    child: new Icon(
                                        icon: Icons.ellipsis,
                                        size: 20,
                                        color: CColors.BrownGrey
                                    )
                                ),
                                onTap: this.moreCallBack
                            )
                        )
                    }
                )
            );
        }
    }
}