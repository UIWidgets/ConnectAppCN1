using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    public class NotificationScreen : StatefulWidget {
        public NotificationScreen(
            Key key = null
        ) : base(key) {
        }

        public override State createState() {
            return new _NotificationScreenState();
        }
    }

    public class _NotificationScreenState : State<NotificationScreen> {
        private const float headerHeight = 140;
        private float _offsetY;
        private int _pageNumber = 1;
        private RefreshController _refreshController;

        public override void initState() {
            base.initState();
            _offsetY = 0;
            _refreshController = new RefreshController();
            var results = StoreProvider.store.state.notificationState.notifications;
            if (results == null || results.Count == 0)
                StoreProvider.store.Dispatch(new FetchNotificationsAction {pageNumber = 1});
        }

        private bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            if (pixels >= 0) {
                if (pixels <= headerHeight) setState(() => { _offsetY = pixels / 2.0f; });
            }
            else {
                if (_offsetY != 0) setState(() => { _offsetY = 0; });
            }

            return true;
        }

        private void _onRefresh(bool up) {
            if (up)
                _pageNumber = 1;
            else
                _pageNumber++;
            NotificationApi.FetchNotifications(_pageNumber)
                .Then(notificationResponse => {
                    StoreProvider.store.Dispatch(new FetchNotificationsSuccessAction {
                        notificationResponse = notificationResponse,
                        pageNumber = _pageNumber
                    });
                    _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle);
                })
                .Catch(error => {
                    _refreshController.sendBack(up, RefreshStatus.failed);
                    Debug.Log($"{error}");
                });
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new CustomNavigationBar(
                            new Text(
                                "通知",
                                style: new TextStyle(
                                    height: 1.25f,
                                    fontSize: 32 / headerHeight * (headerHeight - _offsetY),
                                    fontFamily: "Roboto-Bold",
                                    color: CColors.TextTitle
                                )
                            ),
                            null,
                            CColors.White,
                            _offsetY
                        ),
                        new CustomDivider(
                            color: CColors.Separator2,
                            height: 1
                        ),
                        new Flexible(
                            child: new NotificationListener<ScrollNotification>(
                                onNotification: _onNotification,
                                child: new Container(
                                    padding: EdgeInsets.only(bottom: 49),
                                    child: new StoreConnector<AppState, NotificationState>(
                                        converter: (state, dispatch) => state.notificationState,
                                        builder: (_context, viewModel) => {
                                            if (viewModel.loading) return new GlobalLoading();
                                            var notifications = viewModel.notifications;
                                            if (notifications.Count <= 0) return new BlankView("暂无通知消息");
                                            var isLoadMore = notifications.Count == viewModel.total;

                                            return new SmartRefresher(
                                                controller: _refreshController,
                                                enablePullDown: true,
                                                enablePullUp: !isLoadMore,
                                                headerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                                                footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                                                onRefresh: _onRefresh,
                                                child: ListView.builder(
                                                    physics: new AlwaysScrollableScrollPhysics(),
                                                    itemCount: notifications.Count,
                                                    itemBuilder: (cxt, index) => {
                                                        var notification = notifications[index];
                                                        return new NotificationCard(
                                                            notification: notification
                                                        );
                                                    }
                                                )
                                            );
                                        }
                                    )
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}