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
    public class GameCard : StatelessWidget {
        public GameCard(
            RankData game,
            GestureTapCallback onTap = null,
            GestureTapCallback onPlay = null,
            Key key = null
        ) : base(key: key) {
            this.game = game;
            this.onTap = onTap;
            this.onPlay = onPlay;
        }

        readonly RankData game;
        readonly GestureTapCallback onTap;
        readonly GestureTapCallback onPlay;

        public override Widget build(BuildContext context) {
            if (this.game == null) {
                return new Container();
            }

            return new GestureDetector(
                onTap: () => this.onTap?.Invoke(),
                child: new Container(
                    padding: EdgeInsets.symmetric(12, 16),
                    color: CColors.White,
                    child: new Row(
                        children: new List<Widget> {
                            new Container(
                                margin: EdgeInsets.only(right: 16),
                                child: new PlaceholderImage(
                                    imageUrl: this.game.image,
                                    48,
                                    48,
                                    8,
                                    fit: BoxFit.cover,
                                    true
                                )
                            ),
                            new Expanded(
                                child: new Container(
                                    height: 48,
                                    child: new Column(
                                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Text(
                                                data: this.game.resetTitle,
                                                style: CTextStyle.PLargeBody,
                                                maxLines: 1,
                                                overflow: TextOverflow.ellipsis
                                            ),
                                            new Text(
                                                this.game.resetSubLabel ?? "Unity Tiny官方示例项目",
                                                style: CTextStyle.PSmallBody4,
                                                maxLines: 1,
                                                overflow: TextOverflow.ellipsis
                                            )
                                        }
                                    )
                                )
                            ),
                            new Container(
                                margin: EdgeInsets.only(16),
                                child: new CustomButton(
                                    padding: EdgeInsets.zero,
                                    child: new Container(
                                        width: 60,
                                        height: 28,
                                        decoration: new BoxDecoration(
                                            borderRadius: BorderRadius.all(14),
                                            border: Border.all(color: CColors.PrimaryBlue)
                                        ),
                                        alignment: Alignment.center,
                                        child: new Text(
                                            "开始",
                                            style: new TextStyle(
                                                fontSize: 14,
                                                fontFamily: "Roboto-Medium",
                                                color: CColors.PrimaryBlue
                                            )
                                        )
                                    ),
                                    onPressed: this.onPlay
                                )
                            )
                        }
                    )
                )
            );
        }
    }
}