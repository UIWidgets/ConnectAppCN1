using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class PopularChannelCard : StatelessWidget {
        public PopularChannelCard(
            ChannelView channel,
            GestureTapCallback onTap = null,
            Key key = null
        ) : base(key: key) {
            this.channel = channel;
            this.onTap = onTap;
        }

        readonly ChannelView channel;
        readonly GestureTapCallback onTap;
        public override Widget build(BuildContext context) {
            if (this.channel == null) {
                return new Container();
            }

            Widget image = Positioned.fill(
                new CachedNetworkImage(
                    this.channel?.thumbnail ?? "",
                    fit: BoxFit.cover
                )
            );
            Widget gradient = Positioned.fill(
                new Container(
                    decoration: new BoxDecoration(
                        gradient: new LinearGradient(
                            begin: Alignment.topCenter,
                            end: Alignment.bottomCenter,
                            new List<Color> {
                                Color.fromARGB(20, 0, 0, 0),
                                Color.fromARGB(80, 0, 0, 0)
                            }
                        )
                    )
                )
            );

            Widget title = new Container(
                height: 72,
                padding: EdgeInsets.symmetric(0, 8),
                child: new Align(
                    alignment: Alignment.bottomLeft,
                    child: new Text(
                        data: this.channel.name,
                        style: CTextStyle.PLargeMediumWhite
                    )
                )
            );

            Widget count = new Container(
                padding: EdgeInsets.only(top: 4, left: 8),
                child: new Row(
                    children: new List<Widget> {
                        new Container(
                            width: 8,
                            height: 8,
                            decoration: new BoxDecoration(
                                color: CColors.AquaMarine,
                                borderRadius: BorderRadius.all(4)
                            )
                        ),
                        new Container(width: 4),
                        new Text($"{this.channel.memberCount}人",
                            style: CTextStyle.PSmallWhite)
                    }
                )
            );

            Widget content = Positioned.fill(
                new Column(
                    children: new List<Widget> {
                        title, count
                    }
                )
            );

            return new GestureDetector(
                onTap: this.onTap,
                child: new Container(
                    width: 120,
                    height: 120,
                    margin: EdgeInsets.only(right: 16),
                    child: new ClipRRect(
                        borderRadius: BorderRadius.all(8),
                        child: new Container(
                            child: new Stack(
                                children: new List<Widget> {
                                    image, gradient, content
                                }
                            )
                        )
                    )
                )
            );
        }
    }
}