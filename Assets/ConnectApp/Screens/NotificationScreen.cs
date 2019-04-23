using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class NotificationScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, NotificationScreenViewModel>(
                converter: state => new NotificationScreenViewModel {
                    notificationLoading = state.notificationState.loading,
                    total = state.notificationState.total,
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
        private const int firstPageNumber = 1;
        private int _pageNumber = firstPageNumber;
        private RefreshController _refreshController;
        private TextStyle titleStyle;
        private const float maxNavBarHeight = 96;
        private const float minNavBarHeight = 44;
        private float navBarHeight;
        private string _loginSubId;

        protected override bool wantKeepAlive => true;

        public override void initState() {
            base.initState();
            _refreshController = new RefreshController();
            navBarHeight = maxNavBarHeight;
            titleStyle = CTextStyle.H2;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                widget.actionModel.startFetchNotifications();
                widget.actionModel.fetchNotifications(firstPageNumber);
            });
            _loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
                navBarHeight = maxNavBarHeight;
                titleStyle = CTextStyle.H2;
                widget.actionModel.startFetchNotifications();
                widget.actionModel.fetchNotifications(firstPageNumber);
            });
        }

        public override void dispose() {
            EventBus.unSubscribe(EventBusConstant.login_success, _loginSubId);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            base.build(context);
            Widget content = new Container();
            if (widget.viewModel.notificationLoading && widget.viewModel.notifications.Count == 0) {
                content = new GlobalLoading();
            }
            else {
                if (widget.viewModel.notifications.Count <= 0)
                    content = new Container(
                        child: new BlankView("暂无通知消息")
                    );
                else
                    content = new Container(
                        color: CColors.background3,
                        child: new SmartRefresher(
                            controller: _refreshController,
                            enablePullDown: true,
                            enablePullUp: widget.viewModel.notifications.Count < widget.viewModel.total,
                            onRefresh: _onRefresh,
                            child: ListView.builder(
                                physics: new AlwaysScrollableScrollPhysics(),
                                itemCount: widget.viewModel.notifications.Count,
                                itemBuilder: (cxt, index) => {
                                    var notification = widget.viewModel.notifications[index];
                                    var user = widget.viewModel.userDict[notification.data.userId];
                                    return new NotificationCard(
                                        notification,
                                        user,
                                        () => widget.actionModel.pushToArticleDetail(notification.data.projectId),
                                        new ObjectKey(notification.id)
                                    );
                                }
                            )
                        ));
            }

            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new AnimatedContainer(
                            height: navBarHeight,
                            duration: new TimeSpan(0, 0, 0, 0, 0),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.start,
                                crossAxisAlignment: CrossAxisAlignment.end,
                                children: new List<Widget> {
                                    new Container(
                                        padding: EdgeInsets.only(16, bottom: 8),
                                        child: new AnimatedDefaultTextStyle(
                                            child: new Text("通知"),
                                            style: titleStyle,
                                            duration: new TimeSpan(0, 0, 0, 0, 100)
                                        )
                                    )
                                })
                        ),
                        new CustomDivider(
                            color: CColors.Separator2,
                            height: 1
                        ),
                        new Flexible(
                            child: new NotificationListener<ScrollNotification>(
                                onNotification: _onNotification,
                                child: content
                            )
                        )
                    }
                )
            );
        }

        private bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (pixels > 0 && pixels <= 52) {
                    titleStyle = CTextStyle.H5;
                    navBarHeight = maxNavBarHeight - pixels;
                    setState(() => { });
                }
                else if (pixels <= 0) {
                    if (navBarHeight <= maxNavBarHeight) {
                        titleStyle = CTextStyle.H2;
                        navBarHeight = maxNavBarHeight;
                        setState(() => { });
                    }
                }
                else if (pixels > 52) {
                    if (!(navBarHeight <= minNavBarHeight)) {
                        titleStyle = CTextStyle.H5;
                        navBarHeight = minNavBarHeight;
                        setState(() => { });
                    }
                }
            });
            return true;
        }

        private void _onRefresh(bool up) {
            if (up)
                _pageNumber = firstPageNumber;
            else
                _pageNumber++;
            widget.actionModel.fetchNotifications(_pageNumber)
                .Then(() => _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => _refreshController.sendBack(up, RefreshStatus.failed));
        }
    }
}