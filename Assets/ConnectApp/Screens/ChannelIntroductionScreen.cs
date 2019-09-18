using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class ChannelIntroductionScreenConnector : StatelessWidget {
        public ChannelIntroductionScreenConnector(string channelId, Key key = null) : base(key : key) {
            this.channelId = channelId;
        }

        public readonly string channelId;
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelIntroductionScreenViewModel>(
                converter: state => new ChannelIntroductionScreenViewModel {
                    channel = state.channelState.channelDict[this.channelId]
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelIntroductionScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction())
                    };
                    return new ChannelIntroductionScreen(actionModel, viewModel);
                }
            );
        }
    }

    public class ChannelIntroductionScreen : StatelessWidget {
        public ChannelIntroductionScreen(
            ChannelIntroductionScreenActionModel actionModel = null,
            ChannelIntroductionScreenViewModel viewModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        readonly ChannelIntroductionScreenActionModel actionModel;
        readonly ChannelIntroductionScreenViewModel viewModel;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        color: new Color(0xFFFAFAFA),
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Expanded(
                                    child: this._buildContent()
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomAppBar(
                () => this.actionModel.mainRouterPop(),
                new Text(
                    "群简介",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent() {
            return new Container(
                color: CColors.Background,
                child: new ListView(
                    children: new List<Widget> {
                        new Container(
                            color: CColors.White,
                            padding: EdgeInsets.all(16),
                            height: 80,
                            child: new Row(
                                children: new List<Widget> {
                                    new ClipRRect(
                                        borderRadius: BorderRadius.all(4),
                                        child: new Container(
                                            width: 48,
                                            height: 48,
                                            child: Image.network(this.viewModel.channel?.thumbnail ?? "", fit: BoxFit.cover)
                                        )
                                    ),
                                    new Expanded(
                                        child: new Container(
                                            padding: EdgeInsets.only(16),
                                            child: new Column(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: new List<Widget> {
                                                    new Text(this.viewModel.channel.name,
                                                        style: CTextStyle.PLargeMedium,
                                                        maxLines: 1, overflow: TextOverflow.ellipsis),
                                                    new Expanded(
                                                        child: new Text(
                                                            $"{this.viewModel.channel.memberCount}名群成员",
                                                            style: CTextStyle.PRegularBody4,
                                                            maxLines: 1)
                                                    )
                                                }
                                            )
                                        )
                                    ),
                                }
                            )
                        ),
                        new Container(
                            color: CColors.White,
                            padding: EdgeInsets.only(16, 0, 16, 16),
                            child: new Text(this.viewModel.channel.topic)
                        ),
                    }
                )
            );
        }
    }
}