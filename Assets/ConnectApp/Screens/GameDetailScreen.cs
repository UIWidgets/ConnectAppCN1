using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.Plugins;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class GameDetailScreenConnector : StatelessWidget {
        public GameDetailScreenConnector(
            string gameId,
            Key key = null
        ) : base(key: key) {
            this.gameId = gameId;
        }

        readonly string gameId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, GameDetailScreenViewModel>(
                converter: state => {
                    var game = state.leaderBoardState.rankDict.ContainsKey(key: this.gameId)
                        ? state.leaderBoardState.rankDict[key: this.gameId]
                        : null;
                    return new GameDetailScreenViewModel {
                        gameDetailLoading = state.gameState.gameDetailLoading,
                        game = game
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new GameDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        startFetchGameDetail = () => dispatcher.dispatch(new StartFetchGameDetailAction()),
                        fetchGameDetail = () => dispatcher.dispatch<IPromise>(Actions.fetchGameDetail(gameId: this.gameId))
                    };
                    return new GameDetailScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class GameDetailScreen : StatefulWidget {
        public GameDetailScreen(
            GameDetailScreenViewModel viewModel,
            GameDetailScreenActionModel actionModel,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly GameDetailScreenViewModel viewModel;
        public readonly GameDetailScreenActionModel actionModel;

        public override State createState() {
            return new _GameDetailScreenState();
        }
    }

    class _GameDetailScreenState : State<GameDetailScreen> {
        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            // SchedulerBinding.instance.addPostFrameCallback(_ => {
            //     this.widget.actionModel.startFetchGameDetail();
            //     this.widget.actionModel.fetchGameDetail();
            // });
        }

        public override Widget build(BuildContext context) {
            var game = this.widget.viewModel.game;
            Widget content;
            if (this.widget.viewModel.gameDetailLoading && game == null) {
                content = new GlobalLoading();
            }
            else if (game == null) {
                content = new BlankView(
                    "暂无该游戏",
                    "image/default-article"
                );
            }
            else {
                content = ListView.builder(
                    itemCount: 3,
                    itemBuilder: this._buildGameItem
                );
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Expanded(
                                    child: content
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
                bottomSeparatorColor: CColors.Transparent
            );
        }

        Widget _buildGameItem(BuildContext context, int index) {
            var game = this.widget.viewModel.game;
            if (index == 0) {
                return new GameBrief(
                    game: game,
                    () => TinyWasmPlugin.PushToTinyWasmScreen(url: game.redirectURL, name: game.resetLabel)
                );
            }
            if (index == 1) {
                return new GameImageGallery(
                    game: game
                );
            }
            if (index == 2) {
                return new GameDescription(
                    game: game
                );
            }
            return new Container();
        }
    }
}