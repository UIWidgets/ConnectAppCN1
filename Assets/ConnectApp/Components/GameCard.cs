using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class GameCard : StatelessWidget {
        public GameCard(
            RankData game,
            GestureTapCallback onTap = null,
            Key key = null
        ) : base(key: key) {
            this.game = game;
            this.onTap = onTap;
        }

        readonly RankData game;
        readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context) {
            if (this.game == null) {
                return new Container();
            }

            return new Container(
                padding: EdgeInsets.all(16),
                color: CColors.White,
                child: new Row(
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(right: 8),
                            child: new PlaceholderImage(
                                imageUrl: this.game.image,
                                60,
                                60,
                                4,
                                fit: BoxFit.cover,
                                true
                            )
                        ),
                        new Expanded(
                            child: new Column(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                crossAxisAlignment:  CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Text(data: this.game.resetTitle, style: CTextStyle.PLargeMedium),
                                    new Text(data: this.game.resetDesc, style: CTextStyle.PRegularBody)
                                }
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(8),
                            child: new CustomButton(
                                child: new Text("Play"),
                                onPressed: this.onTap
                            )
                        )
                    }
                )
            );
        }
    }
}