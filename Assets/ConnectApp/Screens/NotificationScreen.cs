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
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class NotificationScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, NotificationScreenViewModel>(
                converter: state => new NotificationScreenViewModel {
                    notificationLoading = state.notificationState.loading,
                    page = state.notificationState.page,
                    pageTotal = state.notificationState.pageTotal,
                    notifications = state.notificationState.notifications,
                    mentions = state.notificationState.mentions,
                    userDict = state.userState.userDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new NotificationScreenActionModel {
                        startFetchNotifications = () => dispatcher.dispatch(new StartFetchNotificationsAction()),
                        fetchNotifications = pageNumber =>
                            dispatcher.dispatch<IPromise>(Actions.fetchNotifications(pageNumber)),
                        fetchMakeAllSeen = () => dispatcher.dispatch<IPromise>(Actions.fetchMakeAllSeen()),
                        pushToArticleDetail = id => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {
                                articleId = id
                            }
                        ),
                        pushToUserDetail = userId => dispatcher.dispatch(
                            new MainNavigatorPushToUserDetailAction {
                                userId = userId
                            }
                        ),
                        pushToTeamDetail = teamId => dispatcher.dispatch(
                            new MainNavigatorPushToTeamDetailAction {
                                teamId = teamId
                            }
                        )
                    };
                    return new NotificationScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class NotificationScreen : StatefulWidget {
        public NotificationScreen(
            NotificationScreenViewModel viewModel = null,
            NotificationScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly NotificationScreenViewModel viewModel;
        public readonly NotificationScreenActionModel actionModel;

        public override State createState() {
            return new _NotificationScreenState();
        }
    }

    public class _NotificationScreenState : AutomaticKeepAliveClientMixin<NotificationScreen>, RouteAware {
        const int firstPageNumber = 1;
        int _pageNumber = firstPageNumber;
        RefreshController _refreshController;
        TextStyle titleStyle;
        const float maxNavBarHeight = 96;
        const float minNavBarHeight = 44;
        float navBarHeight;
        string _loginSubId;
        string _refreshSubId;
        bool _hasBeenLoaded;


        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._refreshController = new RefreshController();
            this.navBarHeight = maxNavBarHeight;
            this.titleStyle = CTextStyle.H2;
            this._hasBeenLoaded = false;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchNotifications();
                this.widget.actionModel.fetchNotifications(arg: firstPageNumber);
            });
            this._loginSubId = EventBus.subscribe(sName: EventBusConstant.login_success, args => {
                this.navBarHeight = maxNavBarHeight;
                this.titleStyle = CTextStyle.H2;
                this.widget.actionModel.startFetchNotifications();
                this.widget.actionModel.fetchNotifications(arg: firstPageNumber);
            });
            this._refreshSubId = EventBus.subscribe(sName: EventBusConstant.refreshNotifications, args => {
                this.navBarHeight = maxNavBarHeight;
                this.titleStyle = CTextStyle.H2;
                this.widget.actionModel.startFetchNotifications();
                this.widget.actionModel.fetchNotifications(arg: firstPageNumber);
            });
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(this.context));
        }

        public override void dispose() {
            EventBus.unSubscribe(sName: EventBusConstant.login_success, id: this._loginSubId);
            EventBus.unSubscribe(sName: EventBusConstant.refreshNotifications, id: this._refreshSubId);
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            Widget content;
            var notifications = this.widget.viewModel.notifications;
            if (this.widget.viewModel.notificationLoading && 0 == notifications.Count) {
                content = new Container(
                    padding: EdgeInsets.only(bottom: CConstant.TabBarHeight +
                                                     CCommonUtils.getSafeAreaBottomPadding(context: context)),
                    child: new GlobalLoading()
                );
                ;
                this._hasBeenLoaded = true;
            }
            else if (this._hasBeenLoaded && 0 == notifications.Count) {
                content = new Container(
                    padding: EdgeInsets.only(bottom: CConstant.TabBarHeight +
                                                     CCommonUtils.getSafeAreaBottomPadding(context: context)),
                    child: new BlankView(
                        "好冷清，多和小伙伴们互动呀",
                        "image/default-notification",
                        true,
                        () => {
                            this.widget.actionModel.startFetchNotifications();
                            this.widget.actionModel.fetchNotifications(arg: firstPageNumber);
                        }
                    )
                );
            }
            else {
                var enablePullUp = this.widget.viewModel.page < this.widget.viewModel.pageTotal;
                var itemCount = enablePullUp ? notifications.Count : notifications.Count + 1;
                content = new Container(
                    color: CColors.Background,
                    child: new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: true,
                        enablePullUp: enablePullUp,
                        onRefresh: this._onRefresh,
                        hasBottomMargin: true,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: itemCount,
                            itemBuilder: this._buildNotificationCard
                        )
                    )
                );
            }

            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        this._buildNavigationBar(),
                        new CustomDivider(
                            color: CColors.Separator2,
                            height: 1
                        ),
                        new Flexible(
                            child: new NotificationListener<ScrollNotification>(
                                onNotification: this._onNotification,
                                child: new CustomScrollbar(child: content)
                            )
                        )
                    }
                )
            );
        }

        Widget _buildNavigationBar() {
            return new AnimatedContainer(
                height: this.navBarHeight,
                duration: TimeSpan.Zero,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.end,
                    children: new List<Widget> {
                        new Container(
                            padding: EdgeInsets.only(16, bottom: 8),
                            child: new AnimatedDefaultTextStyle(
                                child: new Text("通知"),
                                style: this.titleStyle,
                                duration: new TimeSpan(0, 0, 0, 0, 100)
                            )
                        )
                    }
                )
            );
        }

        Widget _buildNotificationCard(BuildContext context, int index) {
            var notifications = this.widget.viewModel.notifications;
            if (index == notifications.Count) {
                return new EndView(hasBottomMargin: true);
            }

            var notification = notifications[index: index];
            var user = this.widget.viewModel.userDict[key: notification.data.userId];
            return new NotificationCard(
                notification: notification,
                user: user,
                mentions: this.widget.viewModel.mentions,
                () => {
                    if (notification.type == "followed" || notification.type == "team_followed") {
                        this.widget.actionModel.pushToUserDetail(obj: notification.data.userId);
                    }
                    else {
                        this.widget.actionModel.pushToArticleDetail(obj: notification.data.projectId);
                        AnalyticsManager.ClickEnterArticleDetail(
                            "Notification_Article",
                            articleId: notification.data.projectId,
                            articleTitle: notification.data.projectTitle
                        );
                    }
                },
                pushToUserDetail: this.widget.actionModel.pushToUserDetail,
                this.widget.actionModel.pushToTeamDetail,
                index == notifications.Count - 1,
                new ObjectKey(value: notification.id)
            );
        }

        bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (pixels > 0 && pixels <= 52) {
                    this.titleStyle = CTextStyle.H5;
                    this.navBarHeight = maxNavBarHeight - pixels;
                    this.setState(() => { });
                }
                else if (pixels <= 0) {
                    if (this.navBarHeight <= maxNavBarHeight) {
                        this.titleStyle = CTextStyle.H2;
                        this.navBarHeight = maxNavBarHeight;
                        this.setState(() => { });
                    }
                }
                else if (pixels > 52) {
                    if (!(this.navBarHeight <= minNavBarHeight)) {
                        this.titleStyle = CTextStyle.H5;
                        this.navBarHeight = minNavBarHeight;
                        this.setState(() => { });
                    }
                }
            });
            return true;
        }

        void _onRefresh(bool up) {
            this._pageNumber = up ? firstPageNumber : this.widget.viewModel.page + 1;

            this.widget.actionModel.fetchNotifications(arg: this._pageNumber)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
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