using System.Collections.Generic;
using System.Linq;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    public class DiscoverChannelsScreenConnector : StatelessWidget {
        public DiscoverChannelsScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, DiscoverChannelsScreenViewModel>(
                converter: state => {
                    return new DiscoverChannelsScreenViewModel {
                        publicChannels = state.channelState.publicChannels.Select(
                            channelId => state.channelState.channelDict[channelId]
                        ).ToList(),
                        joinedChannels = state.channelState.joinedChannels.Select(
                            channelId => state.channelState.channelDict[channelId]).ToList(),
                        page = state.channelState.discoverPage
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new DiscoverChannelsScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        joinChannel = (channelId, groupId) => {
                            dispatcher.dispatch<IPromise>(Actions.joinChannel(channelId, groupId));
                        },
                        fetchChannels = () => dispatcher.dispatch<IPromise>(Actions.fetchChannels(viewModel.page+1))
                    };
                    return new DiscoverChannelsScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class DiscoverChannelsScreen : StatefulWidget {
        public DiscoverChannelsScreen(
            DiscoverChannelsScreenViewModel viewModel = null,
            DiscoverChannelsScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly DiscoverChannelsScreenViewModel viewModel;
        public readonly DiscoverChannelsScreenActionModel actionModel;

        public override State createState() {
            return new _DiscoverChannelsScreenState();
        }
    }

    class _DiscoverChannelsScreenState : State<DiscoverChannelsScreen> {
        TextEditingController _fullNameController;
        TextEditingController _titleController;
        RefreshController _refreshController;

        Dictionary<string, string> _jobRole;

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Flexible(
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
                onBack: () => this.widget.actionModel.mainRouterPop(),
                new Text(
                    "发现群聊",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent() {
            return new Container(
                color: CColors.White,
                child: new SmartRefresher(
                    controller: this._refreshController,
                    enablePullUp: true,
                    enablePullDown: false,
                    onRefresh: this._onRefresh,
                    child: new ListView(
                        padding: EdgeInsets.symmetric(16, 0),
                        children: this.widget.viewModel.publicChannels
                            .Select((channel) => MessengerBuildUtils.buildDiscoverChannelItem(
                                    channel, this.widget.actionModel.joinChannel)
                            ).ToList()
                    )
                )
            );
        }

        void _onRefresh(bool up) {
            if (!up) {
                this.widget.actionModel.fetchChannels().Then(
                    () => {
                        this._refreshController.sendBack(false, RefreshStatus.idle);
                        Debug.Log("Completed");
                    }).Catch((e) => {
                        this._refreshController.sendBack(false, RefreshStatus.idle);
                        Debug.Log("Failed");
                    });
            }
        }
    }
}