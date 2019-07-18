using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
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
            Key key = null
        ) : base(key: key) {
            this.team = team;
            this.onTap = onTap;
            this.userType = userType;
            this.onFollow = onFollow;
        }

        readonly Team team;
        readonly GestureTapCallback onTap;
        readonly UserType userType;
        readonly GestureTapCallback onFollow;

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
                                                margin: EdgeInsets.only(12),
                                                child: new Column(
                                                    mainAxisAlignment: MainAxisAlignment.center,
                                                    crossAxisAlignment: CrossAxisAlignment.start,
                                                    children: new List<Widget> {
                                                        new Text(
                                                            data: this.team.name,
                                                            style: CTextStyle.PMediumBody,
                                                            maxLines: 1,
                                                            overflow: TextOverflow.ellipsis
                                                        )
                                                    }
                                                )
                                            )
                                        )
                                    }
                                )
                            ),
                            new SizedBox(width: 8),
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