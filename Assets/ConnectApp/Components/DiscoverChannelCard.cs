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
    public class DiscoverChannelCard : StatelessWidget {
        public DiscoverChannelCard(
            ChannelView channel,
            GestureTapCallback onTap = null,
            GestureTapCallback joinChannel = null,
            Key key = null
        ) : base(key: key) {
            this.channel = channel;
            this.onTap = onTap;
            this.joinChannel = joinChannel;
        }

        readonly ChannelView channel;
        readonly GestureTapCallback onTap;
        readonly GestureTapCallback joinChannel;

        public override Widget build(BuildContext context) {
            Widget title = new Text(
                this.channel.name ?? "",
                style: CTextStyle.PLargeMedium,
                maxLines: 1,
                overflow: TextOverflow.ellipsis
            );

            Widget body = new Container(
                padding: EdgeInsets.symmetric(0, 16),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        this.channel.live
                            ? new Row(
                                children: new List<Widget> {
                                    new Icon(icon: Icons.whatshot, color: CColors.Error, size: 18),
                                    new Container(width: 7),
                                    new Expanded(child: title)
                                }
                            )
                            : title,
                        new Text(
                            $"{this.channel.memberCount}成员",
                            style: CTextStyle.PRegularBody4
                        )
                    }
                )
            );

            return new GestureDetector(
                onTap: this.onTap,
                child: new Container(
                    color: CColors.White,
                    height: 72,
                    padding: EdgeInsets.symmetric(12, 16),
                    child: new Row(
                        children: new List<Widget> {
                            new PlaceholderImage(
                                this.channel?.thumbnail ?? "",
                                48,
                                48,
                                4,
                                fit: BoxFit.cover,
                                true,
                                CColorUtils.GetSpecificDarkColorFromId(id: this.channel?.id)
                            ),
                            new Expanded(child: body),
                            this._buildJoinButton()
                        }
                    )
                )
            );
        }

        Widget _buildJoinButton() {
            Widget child;
            if (this.channel.joinLoading) {
                child = new CustomActivityIndicator(
                    size: LoadingSize.xSmall
                );
            }
            else {
                child = this.channel.joined
                    ? new Text(
                        "已加入",
                        style: CTextStyle.PRegularBody5.copyWith(height: 1)
                    )
                    : new Text(
                        "加入",
                        style: CTextStyle.PRegularBlue.copyWith(height: 1)
                    );
            }

            return new CustomButton(
                padding: EdgeInsets.zero,
                onPressed: this.channel.joined || this.channel.joinLoading
                    ? null
                    : (GestureTapCallback) (() => this.joinChannel()),
                child: new Container(
                    width: 60,
                    height: 28,
                    decoration: new BoxDecoration(
                        border: Border.all(this.channel.joined ? CColors.Disable2 : CColors.PrimaryBlue),
                        borderRadius: BorderRadius.all(14)
                    ),
                    child: new Center(
                        child: child
                    )
                )
            );
        }
    }
}