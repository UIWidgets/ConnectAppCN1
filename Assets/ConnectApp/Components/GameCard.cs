using System.Collections.Generic;
using System.Linq;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
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

            var imageUrl = this.game.attachmentURLs.FirstOrDefault();
            return new GestureDetector(
                onTap: () => this.onTap?.Invoke(),
                child: new Container(
                    padding: EdgeInsets.only(16, 16, 16, 0),
                    child: new AspectRatio(
                        aspectRatio: 4f / 3,
                        child: new Container(
                            decoration: new BoxDecoration(
                                CColorUtils.GetSpecificDarkColorFromId(id: this.game.id),
                                new DecorationImage(
                                     new CachedNetworkImageProvider(url: imageUrl),
                                     fit: BoxFit.cover
                                ),
                                borderRadius: BorderRadius.circular(8)
                            ),
                            child: new Container(
                                margin: EdgeInsets.only(top: 87),
                                decoration: new BoxDecoration(
                                    gradient: new LinearGradient(
                                        colors: new List<Color> {
                                            Color.fromRGBO(0, 0, 0, 0),
                                            Color.fromRGBO( 0, 0, 0, 0.6f)
                                        },
                                        begin: Alignment.topCenter,
                                        end: Alignment.bottomCenter
                                    ),
                                    borderRadius: BorderRadius.circular(8)
                                ),
                                child: new Container(
                                    padding: EdgeInsets.only(16, 0, 16, 16),
                                    child: new Column(
                                        mainAxisAlignment: MainAxisAlignment.end,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Text(
                                                data: this.game.resetTitle,
                                                maxLines: 1,
                                                style: CTextStyle.H2White.defaultHeight()
                                            ),
                                            new SizedBox(height: 8),
                                            new Text(
                                                data: this.game.resetSubLabel,
                                                maxLines: 1,
                                                style: CTextStyle.PLargeWhite.defaultHeight()
                                            )
                                        }
                                    )
                                )
                            )
                        )
                    )
                )
            );
        }
    }
}