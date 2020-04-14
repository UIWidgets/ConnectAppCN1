using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.Plugins;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
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
                        copyText = text => dispatcher.dispatch(new CopyTextAction {text = text}),
                        shareToWechat = (type, title, description, linkUrl, imageUrl, path) =>
                            dispatcher.dispatch<IPromise>(
                                Actions.shareToWechat(type, title, description, linkUrl, imageUrl)),
                        startFetchGameDetail = () => dispatcher.dispatch(new StartFetchGameDetailAction()),
                        fetchGameDetail = () =>
                            dispatcher.dispatch<IPromise>(Actions.fetchGameDetail(gameId: this.gameId))
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

    class _GameDetailScreenState : State<GameDetailScreen>, TickerProvider {
        bool _showNavBarShadow;
        bool _isHaveTitle;
        Animation<RelativeRect> _animation;
        AnimationController _controller;
        float _titlePosition;
        float _playButtonPosition;
        float _aspectRatio;
        static readonly GlobalKey gameTitleKey = GlobalKey.key("game-title");
        static readonly GlobalKey gameBriefKey = GlobalKey.key("game-brief");

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(true);
            this._showNavBarShadow = true;
            this._isHaveTitle = false;
            this._titlePosition = 0.0f;
            this._playButtonPosition = 0.0f;
            this._aspectRatio = 16.0f / 9;
            if (CCommonUtils.isAndroid) {
                this._aspectRatio = 3f / 2;
            }

            this._controller = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            var rectTween = new RelativeRectTween(
                RelativeRect.fromLTRB(0, 44, 0, 0),
                RelativeRect.fromLTRB(0, 0, 0, 0)
            );
            this._animation = rectTween.animate(this._controller);
            // SchedulerBinding.instance.addPostFrameCallback(_ => {
            //     this.widget.actionModel.startFetchGameDetail();
            //     this.widget.actionModel.fetchGameDetail();
            // });
        }

        Widget _buildHeadTop(RankData game, BuildContext context) {
            Widget titleWidget = new Container();
            Widget playButton = new Container();
            if (this._isHaveTitle) {
                titleWidget = new Text(
                    game.resetTitle,
                    style: CTextStyle.PXLargeMedium,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    textAlign: TextAlign.left
                );
                
                playButton = _buildPlayButton(game: game);
            }

            return new AnimatedContainer(
                height: 44 + CCommonUtils.getSafeAreaTopPadding(context: context),
                duration: TimeSpan.Zero,
                padding: EdgeInsets.only(8, right: 8, top: CCommonUtils.getSafeAreaTopPadding(context: context)),
                decoration: new BoxDecoration(
                    CColors.White,
                    border: new Border(
                        bottom: new BorderSide(this._isHaveTitle ? CColors.Separator2 : CColors.Transparent)),
                    gradient: this._showNavBarShadow
                        ? new LinearGradient(
                            colors: new List<Color> {
                                new Color(0x80000000),
                                new Color(0x0)
                            },
                            begin: Alignment.topCenter,
                            end: Alignment.bottomCenter
                        )
                        : null
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new CustomButton(
                            onPressed: () => this.widget.actionModel.mainRouterPop(),
                            child: new Icon(
                                Icons.arrow_back,
                                size: 24,
                                color: this._showNavBarShadow ? CColors.White : CColors.Icon
                            )
                        ),
                        new Expanded(
                            child: new Stack(
                                fit: StackFit.expand,
                                children: new List<Widget> {
                                    new PositionedTransition(
                                        rect: this._animation,
                                        child: new Row(
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            children: new List<Widget> {
                                                titleWidget,
                                                playButton
                                            }
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }

        static Widget _buildPlayButton(RankData game) {
            return new CustomButton(
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
                onPressed: () => TinyWasmPlugin.PushToTinyWasmScreen(url: game.redirectURL, name: game.resetLabel)
            );
        }

        Widget _buildShareWidget(RankData game) {
            return new CustomButton(
                onPressed: () => ActionSheetUtils.showModalActionSheet(
                    new ShareView(
                        projectType: ProjectType.iEvent,
                        onPressed: type => {
                            // AnalyticsManager.ClickShare(type, "Event", "Event_" + eventObj.id, eventObj.title);
                            var linkUrl = CStringUtils.JointTinyGameShareLink(gameId: game.id);;
                            if (type == ShareType.clipBoard) {
                                this.widget.actionModel.copyText(obj: linkUrl);
                                CustomDialogUtils.showToast("复制链接成功", iconData: Icons.check_circle_outline);
                            }
                            else {
                                var imageUrl = CImageUtils.SizeTo200ImageUrl(imageUrl: game.image);
                                CustomDialogUtils.showCustomDialog(
                                    child: new CustomLoadingDialog()
                                );
                                this.widget.actionModel.shareToWechat(
                                        arg1: type, 
                                        arg2: game.resetTitle,
                                        arg3: game.resetSubLabel,
                                        arg4: linkUrl,
                                        arg5: imageUrl
                                        , null)
                                    .Then(onResolved: CustomDialogUtils.hiddenCustomDialog)
                                    .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                            }
                        }
                    )
                ),
                child: new Container(
                    color: CColors.Transparent,
                    child: new Icon(
                        icon: Icons.outline_share,
                        size: 24,
                        color: CColors.Icon
                    )
                )
            );
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
                    top: false,
                    bottom: false,
                    child: new Container(
                        color: CColors.White,
                        child: new NotificationListener<ScrollNotification>(
                            onNotification: notification => this._onNotification(context, notification),
                            child: new Column(
                                children: new List<Widget> {
                                    new Expanded(
                                        child: new Stack(
                                            children: new List<Widget> {
                                                content,
                                                new Positioned(
                                                    left: 0,
                                                    top: 0,
                                                    right: 0,
                                                    child: this._buildHeadTop(game: this.widget.viewModel.game,
                                                        context: context)
                                                )
                                            }
                                        )
                                    )
                                }
                            )
                        )
                    )
                )
            );
        }

        bool _onNotification(BuildContext context, ScrollNotification notification) {
            var axisDirection = notification.metrics.axisDirection;
            if (axisDirection == AxisDirection.left || axisDirection == AxisDirection.right) {
                return true;
            }

            var pixels = notification.metrics.pixels;
            var topPadding = 44 + CCommonUtils.getSafeAreaTopPadding(context: context);

            if (this._playButtonPosition == 0.0f) {
                var width = MediaQuery.of(context).size.width;
                var imageHeight = width / this._aspectRatio;
                this._playButtonPosition = imageHeight + gameBriefKey.currentContext.size.height - topPadding - 24 - 6; 
                // topPadding 是顶部的高度, 24 是底部的 padding, 6 是按钮到左边图片到底部的距离
            }

            if (pixels >= 44 + topPadding) {
                if (this._showNavBarShadow) {
                    this.setState(() => { this._showNavBarShadow = false; });
                    StatusBarManager.statusBarStyle(false);
                }
            }
            else {
                if (!this._showNavBarShadow) {
                    this.setState(() => { this._showNavBarShadow = true; });
                    StatusBarManager.statusBarStyle(true);
                }
            }

            if (pixels > this._playButtonPosition) {
                if (!this._isHaveTitle) {
                    this._controller.forward();
                    this.setState(() => { this._isHaveTitle = true; });
                }
            }
            else {
                if (this._isHaveTitle) {
                    this._controller.reverse();
                    this.setState(() => { this._isHaveTitle = false; });
                }
            }

            return true;
        }

        Widget _buildGameItem(BuildContext context, int index) {
            var game = this.widget.viewModel.game;
            if (index == 0) {
                return new GameImageGalleryHeader(
                    game: game
                );
            }

            if (index == 1) {
                return new GameBrief(
                    game: game,
                    titleKey: gameTitleKey,
                    briefKey: gameBriefKey,
                    _buildPlayButton(game: game),
                    this._buildShareWidget(game: game)
                );
            }

            if (index == 2) {
                return new GameDescription(
                    game: game
                );
            }

            return new Container();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick, () => $"created by {this}");
        }
    }
}