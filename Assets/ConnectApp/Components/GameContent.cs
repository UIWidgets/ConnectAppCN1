using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class GameBrief : StatelessWidget {
        public GameBrief(
            RankData game,
            GestureTapCallback onPlay = null,
            Key key = null
        ) : base(key: key) {
            this.game = game;
            this.onPlay = onPlay;
        }

        readonly RankData game;
        readonly GestureTapCallback onPlay;

        public override Widget build(BuildContext context) {
            if (this.game == null) {
                return new Container();
            }

            return new Container(
                padding: EdgeInsets.only(16, 10, 16, 24),
                child: new Row(
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(right: 12),
                            child: new PlaceholderImage(
                                imageUrl: this.game.image,
                                104,
                                104,
                                12,
                                fit: BoxFit.cover,
                                true
                            )
                        ),
                        new Expanded(
                            child: new Container(
                                height: 104,
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new SizedBox(height: 6),
                                        new Text(
                                            data: this.game.resetTitle,
                                            style: CTextStyle.H5,
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis
                                        ),
                                        new SizedBox(height: 2),
                                        new Text(
                                            data: this.game.resetDesc,
                                            style: CTextStyle.PRegularBody4,
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis
                                        ),
                                        new Flexible(child: new Container()),
                                        new CustomButton(
                                            padding: EdgeInsets.zero,
                                            child: new Container(
                                                width: 60,
                                                height: 28,
                                                decoration: new BoxDecoration(
                                                    color: CColors.PrimaryBlue,
                                                    borderRadius: BorderRadius.all(14)
                                                ),
                                                alignment: Alignment.center,
                                                child: new Text(
                                                    "开始",
                                                    style: new TextStyle(
                                                        fontSize: 14,
                                                        fontFamily: "Roboto-Medium",
                                                        color: CColors.White
                                                    )
                                                )
                                            ),
                                            onPressed: this.onPlay
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

    public class GameImageGallery : StatelessWidget {
        public GameImageGallery(
            RankData game,
            Key key = null
        ) : base(key: key) {
            this.game = game;
        }

        readonly RankData game;

        public override Widget build(BuildContext context) {
            if (this.game == null) {
                return new Container();
            }

            var listCount = 5;
            
            return new Container(
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            height: 1,
                            margin: EdgeInsets.symmetric(horizontal: 16),
                            color: CColors.Separator
                        ),
                        new Padding(
                            padding: EdgeInsets.all(16),
                            child: new Text(
                                "预览",
                                style: CTextStyle.H5
                            )
                        ),
                        new Container(
                            height: MediaQuery.of(context: context).size.width * 0.4f,
                            child: ListView.builder(
                                scrollDirection: Axis.horizontal,
                                itemCount: listCount,
                                itemBuilder: (cxt, index) => new Container(
                                    margin: EdgeInsets.only(index == 0 ? 16 : 8, right: index == listCount - 1 ? 16 : 0),
                                    child: new AspectRatio(
                                        aspectRatio: 2,
                                        child: new PlaceholderImage(
                                            "http://via.placeholder.com/350x150",
                                            borderRadius: 8,
                                            fit: BoxFit.fill,
                                            useCachedNetworkImage: true
                                        )
                                    )
                                )
                            )
                        )
                    }
                )
            );
        }
    }

    public class GameDescription : StatelessWidget {
        public GameDescription(
            RankData game,
            Key key = null
        ) : base(key: key) {
            this.game = game;
        }

        readonly RankData game;

        public override Widget build(BuildContext context) {
            if (this.game == null) {
                return new Container();
            }

            return new Container(
                padding: EdgeInsets.only(16, 40, 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text(
                            "功能介绍",
                            style: CTextStyle.H5
                        ),
                        new Padding(
                            padding: EdgeInsets.only(top: 16),
                            child: new Text(
                                "“Tiny Racing” browser game made in Unity3d Project Tiny. 使用tiny开发的小游戏。使用tiny开发的小游戏。使用tiny开发的小游戏。使用tiny开发的小游戏。使用tiny开发的小游戏。使用tiny开发的小游戏。使用tiny开发的小游戏。使用tiny开发的小游戏。",
                                style: CTextStyle.PRegularBody
                            )
                        )
                    }
                )
            );
        }
    }
}