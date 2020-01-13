using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ArticleAlbumScreenConnector : StatelessWidget {
        public ArticleAlbumScreenConnector(
            string albumId = null,
            Key key = null
        ) : base(key: key) {
            this.albumId = albumId;
        }

        readonly string albumId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, UserDetailScreenViewModel>(
                converter: state => { return new UserDetailScreenViewModel(); },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new UserDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToArticleDetail = id => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {
                                articleId = id
                            }
                        ),
                        pushToReport = (reportId, reportType) => dispatcher.dispatch(
                            new MainNavigatorPushToReportAction {
                                reportId = reportId,
                                reportType = reportType
                            }
                        ),
                        shareToWechat = (type, title, description, linkUrl, imageUrl) => dispatcher.dispatch<IPromise>(
                            Actions.shareToWechat(type, title, description, linkUrl, imageUrl))
                    };
                    return new ArticleAlbumScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class ArticleAlbumScreen : StatefulWidget {
        public ArticleAlbumScreen(
            UserDetailScreenViewModel viewModel = null,
            UserDetailScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly UserDetailScreenViewModel viewModel;
        public readonly UserDetailScreenActionModel actionModel;

        public override State createState() {
            return new _ArticleAlbumScreenState();
        }
    }

    class _ArticleAlbumScreenState : State<ArticleAlbumScreen>, TickerProvider, RouteAware {
        int _pageNumber;
        bool _isHaveTitle;
        RefreshController _refreshController;
        Animation<RelativeRect> _animation;
        AnimationController _controller;
        readonly GlobalKey _albumInfoKey = GlobalKey.key("album-info");


        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._pageNumber = 1;
            this._refreshController = new RefreshController();
            this._controller = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            RelativeRectTween rectTween = new RelativeRectTween(
                RelativeRect.fromLTRB(0, 44, 0, 0),
                RelativeRect.fromLTRB(0, 13, 0, 0)
            );
            this._animation = rectTween.animate(parent: this._controller);
            SchedulerBinding.instance.addPostFrameCallback(_ => { });
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            this._controller.dispose();
            base.dispose();
        }

        void _onRefresh(bool up) {
            if (up) {
                this._pageNumber = 1;
            }
            else {
                this._pageNumber++;
            }

            this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle);
//            this.widget.actionModel.fetchUserArticle(arg1: this.widget.viewModel.user.id, arg2: this._articlePageNumber)
//                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
//                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: this._isHaveTitle ? CColors.White : CColors.Background,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Flexible(child: this._buildContent())
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            Widget titleWidget;
            if (this._isHaveTitle) {
                titleWidget = new Text(
                    data: "Unity官方博主预备营准备启动",
                    style: CTextStyle.PXLargeMedium,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    textAlign: TextAlign.center
                );
            }
            else {
                titleWidget = new Container();
            }

            Widget buttonChild;
            Color followColor = CColors.PrimaryBlue;
            if (false) {
                followColor = CColors.Disable2;
                buttonChild = new CustomActivityIndicator(
                    size: LoadingSize.xSmall
                );
            }
            else {
                string followText = "收藏";
                Color textColor = CColors.PrimaryBlue;
                if (false) {
                    followText = "已收藏";
                    followColor = CColors.Disable2;
                    textColor = new Color(0xFF959595);
                }

                buttonChild = new Text(
                    data: followText,
                    style: new TextStyle(
                        fontSize: 14,
                        fontFamily: "Roboto-Medium",
                        color: textColor
                    )
                );
            }

            Widget rightWidget = new Container();
            if (this._isHaveTitle) {
                rightWidget = new Padding(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    child: new CustomButton(
                        onPressed: () => { },
                        padding: EdgeInsets.zero,
                        child: new Container(
                            width: 60,
                            height: 28,
                            alignment: Alignment.center,
                            decoration: new BoxDecoration(
                                color: CColors.White,
                                borderRadius: BorderRadius.circular(14),
                                border: Border.all(color: followColor)
                            ),
                            child: buttonChild
                        )
                    ));
            }

            return new CustomAppBar(
                () => this.widget.actionModel.mainRouterPop(),
                new Expanded(
                    child: new Stack(
                        fit: StackFit.expand,
                        children: new List<Widget> {
                            new PositionedTransition(
                                rect: this._animation,
                                child: titleWidget
                            )
                        }
                    )
                ),
                rightWidget: rightWidget,
                backgroundColor: this._isHaveTitle ? CColors.White : CColors.Background,
                bottomSeparatorColor: this._isHaveTitle ? CColors.Separator2 : CColors.Transparent
            );
        }

        Widget _buildContent() {
            Widget content = new Container(
                color: CColors.Background,
                child: new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: true,
                    onRefresh: this._onRefresh,
                    itemCount: 10,
                    itemBuilder: this._buildAlbumCard,
                    footerWidget: false ? null : CustomListViewConstant.defaultFooterWidget
                )
            );

            return new NotificationListener<ScrollNotification>(child: content, onNotification: this._onNotification);
        }

        Widget _buildAlbumCard(BuildContext context, int index) {
            if (index == 0) {
                return new ArticleAlbumHeader();
            }

            return new ArticleCard(null);
        }

        bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            if (pixels > 44) {
                if (!this._isHaveTitle) {
                    this._controller.forward();
                    this.setState(() => this._isHaveTitle = true);
                }
            }
            else {
                if (this._isHaveTitle) {
                    this._controller.reverse();
                    this.setState(() => this._isHaveTitle = false);
                }
            }

            return true;
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick: onTick, () => $"created by {this}");
        }

        public void didPopNext() {
            StatusBarManager.statusBarStyle(true);
        }

        public void didPush() {
        }

        public void didPop() {
        }

        public void didPushNext() {
        }
    }
}