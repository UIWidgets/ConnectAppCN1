using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ChannelShareScreenConnector : StatelessWidget {
        public ChannelShareScreenConnector(
            string channelId,
            Key key = null
        ) : base(key: key) {
            this.channelId = channelId;
        }

        readonly string channelId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ChannelShareScreenViewModel>(
                converter: state => {
                    var channel = !state.channelState.channelDict.ContainsKey(this.channelId)
                        ? ChannelView.fromChannel(new Channel())
                        : state.channelState.channelDict[this.channelId];
                    channel.isTop = state.channelState.channelTop.TryGetValue(key: this.channelId, out var isTop) &&
                                    isTop;
                    return new ChannelShareScreenViewModel {
                        channel = channel,
                        channelShareInfoLoading = state.channelState.channelShareInfoLoading
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelShareScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToChannel = () => dispatcher.dispatch(new MainNavigatorPushToChannelAction {
                            channelId = this.channelId,
                            pushReplace = true
                        }),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        fetchChannelInfo = () => dispatcher.dispatch<IPromise>(
                            Actions.fetchChannelInfo(channelId: this.channelId, true)),
                        joinChannel = () => dispatcher.dispatch<IPromise>(
                            Actions.joinChannel(channelId: this.channelId, groupId: viewModel.channel.groupId, true))
                    };
                    return new ChannelShareScreen(actionModel: actionModel, viewModel: viewModel);
                }
            );
        }
    }

    public class ChannelShareScreen : StatefulWidget {
        public ChannelShareScreen(
            ChannelShareScreenActionModel actionModel = null,
            ChannelShareScreenViewModel viewModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ChannelShareScreenActionModel actionModel;
        public readonly ChannelShareScreenViewModel viewModel;

        public override State createState() {
            return new _ChannelShareScreenState();
        }
    }

    class _ChannelShareScreenState : State<ChannelShareScreen>, RouteAware {
        public override void initState() {
            base.initState();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (this.widget.viewModel.channel.id == null) {
                    this.widget.actionModel.fetchChannelInfo().Then(() => {
                        if (UserInfoManager.isLogin() && this.widget.viewModel.channel.joined) {
                            this.widget.actionModel.pushToChannel();
                        }
                    });
                }
                else {
                    if (UserInfoManager.isLogin() && this.widget.viewModel.channel.joined) {
                        this.widget.actionModel.pushToChannel();
                    }
                }
            });
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Container(
                        color: CColors.Background,
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
                () => this.widget.actionModel.mainRouterPop(),
                new Text(
                    "群聊资料",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildChannelIntroduction() {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, right: 16, top: 16),
                height: 64,
                child: new Row(
                    children: new List<Widget> {
                        new PlaceholderImage(
                            this.widget.viewModel.channel?.thumbnail ?? "",
                            48,
                            48,
                            4,
                            fit: BoxFit.cover
                        ),
                        new Expanded(
                            child: new Container(
                                padding: EdgeInsets.only(16),
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Text(
                                            this.widget.viewModel.channel?.name ?? "",
                                            style: CTextStyle.PLargeMedium,
                                            maxLines: 1,
                                            overflow: TextOverflow.ellipsis
                                        ),
                                        new Container(height: 4),
                                        new Text(
                                            $"{this.widget.viewModel.channel?.memberCount ?? 0}名群成员",
                                            style: CTextStyle.PSmallBody4
                                        )
                                    }
                                )
                            )
                        )
                    }
                )
            );
        }

        Widget _buildChannelSetting() {
            return new Column(
                children: new List<Widget> {
                    new Container(height: 16), new GestureDetector(
                        onTap: () => {
                            if (UserInfoManager.isLogin()) {
                                if (this.widget.viewModel.channel.joined) {
                                    this.widget.actionModel.pushToChannel();
                                }
                                else {
                                    this.widget.actionModel.joinChannel();
                                }
                            }
                            else {
                                this.widget.actionModel.pushToLogin();
                            }
                        },
                        child: new Container(
                            color: CColors.White,
                            height: 60,
                            child: new Center(
                                child: new Text(
                                    this.widget.viewModel.channel.joined && UserInfoManager.isLogin() ? "进入群聊" : "加入群聊",
                                    style: CTextStyle.PLargeBlue)
                            )
                        )
                    )
                }
            );
        }

        Widget _buildContent() {
            if (this.widget.viewModel.channelShareInfoLoading && this.widget.viewModel.channel.id == null) {
                return new GlobalLoading();
            }

            if (this.widget.viewModel.channel.id == null ||
                this.widget.viewModel.channel.errorCode == "ResourceNotFound") {
                return new BlankView("频道不存在");
            }

            return new Container(
                color: CColors.Background,
                child: new ListView(
                    children: new List<Widget> {
                        this._buildChannelIntroduction(),
                        new GestureDetector(
                            onTap: () => { },
                            child: new Container(
                                color: CColors.White,
                                padding: EdgeInsets.only(16, 16, 12, 21),
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    children: new List<Widget> {
                                        new Expanded(
                                            child: new Text(
                                                this.widget.viewModel.channel?.topic ?? "",
                                                style: CTextStyle.PRegularBody2
                                            )
                                        )
                                    }
                                )
                            )
                        ),
                        this._buildChannelSetting()
                    }
                )
            );
        }

        public void didPop() {
        }

        public void didPopNext() {
            StatusBarManager.statusBarStyle(false);
        }

        public void didPush() {
        }

        public void didPushNext() {
        }
    }
}