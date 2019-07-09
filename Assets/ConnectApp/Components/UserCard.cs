using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using Unity.UIWidgets.ui;

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
            GestureTapCallback onTap = null,
            UserType userType = UserType.unFollow,
            GestureTapCallback onFollow = null,
            Key key = null
        ) : base(key: key) {
            this.user = user;
            this.onTap = onTap;
            this.userType = userType;
            this.onFollow = onFollow;
        }

        readonly User user;
        readonly GestureTapCallback onTap;
        readonly UserType userType;
        readonly GestureTapCallback onFollow;

        public override Widget build(BuildContext context) {
            if (this.user == null) {
                return new Container();
            }

            Widget titleWidget = new Container();
            if (this.user.title.isNotEmpty()) {
                titleWidget = new Text(
                    data: this.user.title,
                    style: CTextStyle.PRegularBody4,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis
                );
            }

            Widget followButton;
            if (this.userType == UserType.me) {
                followButton = new Container();
            }
            else {
                Widget buttonChild;
                Color followColor = CColors.PrimaryBlue;
                if (this.userType == UserType.loading) {
                    buttonChild = new CustomActivityIndicator();
                }
                else {
                    string followText = "关注";
                    Color textColor = CColors.PrimaryBlue;
                    if (this.userType == UserType.follow) {
                        followText = "已关注";
                        followColor = CColors.Disable2;
                        textColor = new Color(0xFF959595);
                    }
                    buttonChild = new Text(
                        data: followText,
                        style: new TextStyle(
                            fontSize: 14,
                            fontFamily: "Roboto-Medium",
                            color: textColor
                        )
                    );
                }
                followButton = new CustomButton(
                    onPressed: () => this.onFollow(),
                    padding: EdgeInsets.zero,
                    child: new Container(
                        width: 60,
                        height: 28,
                        alignment: Alignment.center,
                        decoration: new BoxDecoration(
                            color: CColors.White,
                            borderRadius: BorderRadius.circular(14),
                            border: Border.all(color: followColor)
                        ),
                        child: buttonChild
                    )
                );
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
                                        Avatar.User(id: this.user.id, user: this.user, 48),
                                        new Expanded(
                                            child: new Container(
                                                margin: EdgeInsets.only(12),
                                                child: new Column(
                                                    mainAxisAlignment: MainAxisAlignment.center,
                                                    crossAxisAlignment: CrossAxisAlignment.start,
                                                    children: new List<Widget> {
                                                        new Text(
                                                            data: this.user.fullName,
                                                            style: CTextStyle.PMediumBody,
                                                            maxLines: 1,
                                                            overflow: TextOverflow.ellipsis
                                                        ),
                                                        titleWidget
                                                    }
                                                )
                                            )
                                        )
                                    }
                                )
                            ),
                            new SizedBox(width: 8),
                            followButton
                        }
                    )
                )
            );
        }
    }
}