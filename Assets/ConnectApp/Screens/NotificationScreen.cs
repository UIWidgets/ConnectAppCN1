using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.utils;
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
                    pageTotal = state.notificationState.pageTotal,
                    notifications = state.notificationState.notifications,
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
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly NotificationScreenViewModel viewModel;
        public readonly NotificationScreenActionModel actionModel;

        public override State createState() {
            return new _NotificationScreenState();
        }
    }

    public class _NotificationScreenState : AutomaticKeepAliveClientMixin<NotificationScreen> {
        const int firstPageNumber = 1;
        int _pageNumber = firstPageNumber;
        RefreshController _refreshController;
        TextStyle titleStyle;
        const float maxNavBarHeight = 96;
        const float minNavBarHeight = 44;
        float navBarHeight;
        string _loginSubId;
        string _refreshSubId;


        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
            this.navBarHeight = maxNavBarHeight;
            this.titleStyle = CTextStyle.H2;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchNotifications();
                this.widget.actionModel.fetchNotifications(firstPageNumber);
            });
            this._loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
                this.navBarHeight = maxNavBarHeight;
                this.titleStyle = CTextStyle.H2;
                this.widget.actionModel.startFetchNotifications();
                this.widget.actionModel.fetchNotifications(firstPageNumber);
            });
            this._refreshSubId = EventBus.subscribe(EventBusConstant.refreshNotifications, args => {
                this.navBarHeight = maxNavBarHeight;
                this.titleStyle = CTextStyle.H2;
                this.widget.actionModel.startFetchNotifications();
                this.widget.actionModel.fetchNotifications(firstPageNumber);
            });
        }

        public override void dispose() {
            EventBus.unSubscribe(EventBusConstant.login_success, this._loginSubId);
            EventBus.unSubscribe(EventBusConstant.refreshNotifications, this._refreshSubId);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            base.build(context);
            Widget content = new Container();
            if (this.widget.viewModel.notificationLoading && this.widget.viewModel.notifications.Count == 0) {
                content = new GlobalLoading();
            }
            else {
                if (this.widget.viewModel.notifications.Count <= 0) {
                    content = new Container(
                        child: new BlankView("暂无通知消息", true, () => {
                            this.widget.actionModel.startFetchNotifications();
                            this.widget.actionModel.fetchNotifications(firstPageNumber);
                        })
                    );
                }
                else {
                    content = new Container(
                        color: CColors.background3,
                        child: new SmartRefresher(
                            controller: this._refreshController,
                            enablePullDown: true,
                            enablePullUp: this._pageNumber < this.widget.viewModel.pageTotal,
                            onRefresh: this._onRefresh,
                            child: ListView.builder(
                                physics: new AlwaysScrollableScrollPhysics(),
                                itemCount: this.widget.viewModel.notifications.Count,
                                itemBuilder: (cxt, index) => {
                                    var notification = this.widget.viewModel.notifications[index];
                                    var user = this.widget.viewModel.userDict[notification.data.userId];
                                    return new NotificationCard(
                                        notification,
                                        user,
                                        () => this.widget.actionModel.pushToArticleDetail(notification.data.projectId),
                                        new ObjectKey(notification.id)
                                    );
                                }
                            )
                        )
                    );
                }
            }

            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new AnimatedContainer(
                            height: this.navBarHeight,
                            duration: new TimeSpan(0, 0, 0, 0, 0),
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
                        ),
                        new CustomDivider(
                            color: CColors.Separator2,
                            height: 1
                        ),
                        new Flexible(
                            child: new NotificationListener<ScrollNotification>(
                                onNotification: this._onNotification,
                                child: content
                            )
                        )
                    }
                )
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
            if (up) {
                this._pageNumber = firstPageNumber;
            }
            else {
                this._pageNumber++;
            }

            this.widget.actionModel.fetchNotifications(this._pageNumber)
                .Then(() => this._refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up, RefreshStatus.failed));
        }
    }
}