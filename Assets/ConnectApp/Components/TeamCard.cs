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
    public class TeamCard : StatelessWidget {
        public TeamCard(
            Team team,
            GestureTapCallback onTap = null,
            UserType userType = UserType.unFollow,
            GestureTapCallback onFollow = null,
            bool isSearchCard = false,
            Key key = null
        ) : base(key: key) {
            this.team = team;
            this.onTap = onTap;
            this.userType = userType;
            this.onFollow = onFollow;
            this.isSearchCard = isSearchCard;
        }

        readonly Team team;
        readonly GestureTapCallback onTap;
        readonly UserType userType;
        readonly GestureTapCallback onFollow;
        readonly bool isSearchCard;

        public override Widget build(BuildContext context) {
            if (this.team == null) {
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
                                        Avatar.Team(team: this.team, 48),
                                        new Expanded(
                                            child: new Container(
                                                margin: EdgeInsets.only(12, right: 16),
                                                child: new Column(
                                                    mainAxisAlignment: MainAxisAlignment.center,
                                                    crossAxisAlignment: CrossAxisAlignment.start,
                                                    children: new List<Widget> {
                                                        new Row(
                                                            children: new List<Widget> {
                                                                new Flexible(
                                                                    child: new Text(
                                                                        data: this.team.name,
                                                                        style: new TextStyle(
                                                                            height: 1,
                                                                            fontSize: this.isSearchCard ? 16 : 14,
                                                                            fontFamily: "Roboto-Medium",
                                                                            color: CColors.TextBody
                                                                        ),
                                                                        maxLines: 1,
                                                                        overflow: TextOverflow.ellipsis
                                                                    )
                                                                ),
                                                                CImageUtils.GenBadgeImage(
                                                                    badges: this.team.badges,
                                                                    null,
                                                                    EdgeInsets.only(4)
                                                                )
                                                            }
                                                        )
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
    }
}