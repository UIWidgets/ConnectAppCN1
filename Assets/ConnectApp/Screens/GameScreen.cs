using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.Plugins;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class GameScreenConnector : StatelessWidget {
        public GameScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, GameScreenViewModel>(
                converter: state => new GameScreenViewModel {
                    gameLoading = state.gameState.gameLoading,
                    gameIds = state.gameState.gameIds,
                    gamePage = state.gameState.gamePage,
                    gameHasMore = state.gameState.gameHasMore,
                    rankDict = state.leaderBoardState.rankDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new GameScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToGameDetail = gameId => dispatcher.dispatch(new MainNavigatorPushToGameDetailAction {
                            gameId = gameId
                        }),
                        startFetchGame = () => dispatcher.dispatch(new StartFetchGameAction()),
                        fetchGame = page => dispatcher.dispatch<IPromise>(Actions.fetchGames(page: page))
                    };
                    return new GameScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class GameScreen : StatefulWidget {
        public GameScreen(
            GameScreenViewModel viewModel,
            GameScreenActionModel actionModel,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly GameScreenViewModel viewModel;
        public readonly GameScreenActionModel actionModel;

        public override State createState() {
            return new _GameScreenState();
        }
    }

    class _GameScreenState : State<GameScreen>, RouteAware {
        const int firstPageNumber = 1;
        int gamePageNumber = firstPageNumber;
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchGame();
                this.widget.actionModel.fetchGame(arg: firstPageNumber);
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
        
        void _onRefresh(bool up) {
            this.gamePageNumber = up ? firstPageNumber : this.gamePageNumber + 1;
            this.widget.actionModel.fetchGame(arg: this.gamePageNumber)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            var gameIds = this.widget.viewModel.gameIds;
            Widget content;
            if (this.widget.viewModel.gameLoading && gameIds.isEmpty()) {
                content = new GlobalLoading();
            }
            else if (gameIds.Count <= 0) {
                content = new BlankView(
                    "暂无游戏",
                    "image/default-article",
                    true,
                    () => {
                        this.widget.actionModel.startFetchGame();
                        this.widget.actionModel.fetchGame(arg: firstPageNumber);
                    }
                );
            }
            else {
                var enablePullUp = this.widget.viewModel.gameHasMore;
                content = new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: enablePullUp,
                    onRefresh: this._onRefresh,
                    itemCount: gameIds.Count,
                    itemBuilder: this._buildGameCard,
                    footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                );
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
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
            return new CustomNavigationBar(
                new Text(
                    "Unity Tiny 小游戏",
                    style: CTextStyle.H2
                ),
                padding: EdgeInsets.only(16, bottom: 8),
                onBack: () => this.widget.actionModel.mainRouterPop()
            );
        }

        Widget _buildGameCard(BuildContext context, int index) {
            var gameId = this.widget.viewModel.gameIds[index: index];
            var rankDict = this.widget.viewModel.rankDict;
            if (!rankDict.ContainsKey(key: gameId)) {
                return new Container();
            }

            var game = rankDict[key: gameId];
            return new GameCard(
                game: game,
                () => this.widget.actionModel.pushToGameDetail(obj: game.id),
                () => TinyWasmPlugin.PushToTinyWasmScreen(url: game.redirectURL, name: game.resetLabel));
        }
        
        public void didPopNext() { 
            StatusBarManager.statusBarStyle(false);
        }

        public void didPush() {
        }

        public void didPop() {
        }

        public void didPushNext() {
        }
    }
}