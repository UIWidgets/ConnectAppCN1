using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public enum UserType {
        follow,
        unFollow,
        me,
        loading
    }

    public class UserCard : StatelessWidget {
        public UserCard(
            User user,
            string license,
            GestureTapCallback onTap = null,
            UserType userType = UserType.unFollow,
            GestureTapCallback onFollow = null,
            bool isSearchCard = false,
            Key key = null
        ) : base(key: key) {
            this.user = user;
            this.license = license;
            this.onTap = onTap;
            this.userType = userType;
            this.onFollow = onFollow;
            this.isSearchCard = isSearchCard;
        }

        readonly User user;
        readonly string license;
        readonly GestureTapCallback onTap;
        readonly UserType userType;
        readonly GestureTapCallback onFollow;
        readonly bool isSearchCard;

        public override Widget build(BuildContext context) {
            if (this.user == null) {
                return new Container();
            }

            return new GestureDetector(
                onTap: this.onTap,
                child: new Container(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    color: CColors.White,
                    height: 72,
                    child: new Row(
                        children: new List<Widget> {
                            new Expanded(
                                child: new Row(
                                    children: new List<Widget> {
                                        Avatar.User(user: this.user, 48),
                                        new Expanded(
                                            child: new Container(
                                                padding: EdgeInsets.only(12, right: 16),
                                                child: new Column(
                                                    mainAxisAlignment: MainAxisAlignment.center,
                                                    crossAxisAlignment: CrossAxisAlignment.start,
                                                    children: new List<Widget> {
                                                        new Row(
                                                            children: new List<Widget> {
                                                                new Flexible(
                                                                    child: new Text(
                                                                        data: this.user.fullName,
                                                                        style: new TextStyle(
                                                                            fontSize: this.isSearchCard ? 16 : 14,
                                                                            height: 1,
                                                                            fontFamily: "Roboto-Medium",
                                                                            color: CColors.TextBody
                                                                        ),
                                                                        maxLines: 1,
                                                                        overflow: TextOverflow.ellipsis
                                                                    )
                                                                ),
                                                                CImageUtils.GenBadgeImage(
                                                                    badges: this.user.badges,
                                                                    license: this.license,
                                                                    EdgeInsets.only(4)
                                                                )
                                                            }
                                                        ),
                                                        this._buildUserTitle()
                                                    }
                                                )
                                            )
                                        )
                                    }
                                )
                            ),
                            new FollowButton(
                                userType: this.userType,
                                onFollow: this.onFollow
                            )
                        }
                    )
                )
            );
        }

        Widget _buildUserTitle() {
            if (this.user.title.isNotEmpty()) {
                return new Text(
                    data: this.user.title,
                    style: CTextStyle.PRegularBody4,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis
                );
            }

            return new Container();
        }
    }

    public class MemberCard : StatelessWidget {
        public MemberCard(
            ChannelMember member,
            GestureTapCallback onTap = null,
            UserType userType = UserType.unFollow,
            GestureTapCallback onFollow = null,
            Key key = null
        ) : base(key: key) {
            this.member = member;
            this.onTap = onTap;
            this.userType = userType;
            this.onFollow = onFollow;
        }

        readonly ChannelMember member;
        readonly GestureTapCallback onTap;
        readonly UserType userType;
        readonly GestureTapCallback onFollow;

        public override Widget build(BuildContext context) {
            if (this.member == null) {
                return new Container();
            }

            return new GestureDetector(
                onTap: this.onTap,
                child: new Container(
                    color: CColors.White,
                    height: 72,
                    padding: EdgeInsets.symmetric(12, 16),
                    child: new Row(
                        children: new List<Widget> {
                            Avatar.User(user: this.member.user, 48),
                            new Expanded(
                                child: new Container(
                                    padding: EdgeInsets.symmetric(0, 16),
                                    child: new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            this._buildMemberName(),
                                            this._buildMemberTitle()
                                        }
                                    )
                                )
                            ),
                            new FollowButton(
                                userType: this.userType,
                                onFollow: this.onFollow
                            )
                        }
                    )
                )
            );
        }

        Widget _buildMemberName() {
            Widget memberNameWidget = new Text(
                data: this.member.user.fullName,
                style: CTextStyle.PMediumBody.copyWith(height: 1),
                maxLines: 1,
                overflow: TextOverflow.ellipsis
            );
            if (this.member.role != "member") {
                return new Row(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new Flexible(child: memberNameWidget),
                        new Container(
                            height: 16,
                            alignment: Alignment.center,
                            decoration: new BoxDecoration(
                                this.member.role != "admin" ? CColors.Orange : CColors.JordyBlue,
                                borderRadius: BorderRadius.all(2)
                            ),
                            padding: EdgeInsets.symmetric(0, 4),
                            margin: EdgeInsets.only(4),
                            child: new Text(
                                this.member.role == "admin" ? "管理员" : "群主",
                                style: CTextStyle.PSmallWhite.copyWith(height: 1.0f)
                            )
                        )
                    }
                );
            }

            return memberNameWidget;
        }

        Widget _buildMemberTitle() {
            if (this.member.user.title.isNotEmpty()) {
                return new Text(
                    data: this.member.user.title,
                    style: CTextStyle.PRegularBody4,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis
                );
            }

            return new Container();
        }
    }
}