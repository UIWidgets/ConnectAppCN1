using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
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
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class NotificationScreenConnector : StatelessWidget {
        public NotificationScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, NotificationScreenViewModel>(
                converter: state => new NotificationScreenViewModel {
                    notificationLoading = state.notificationState.loading,
                    page = state.notificationState.page,
                    pageTotal = state.notificationState.pageTotal,
                    notifications = state.notificationState.notifications,
                    mentions = state.notificationState.mentions,
                    userDict = state.userState.userDict,
                    teamDict = state.teamState.teamDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new NotificationScreenActionModel {
                        startFetchNotifications = () => dispatcher.dispatch(new StartFetchNotificationsAction()),
                        fetchNotifications = pageNumber =>
                            dispatcher.dispatch<IPromise>(Actions.fetchNotifications(pageNumber: pageNumber)),
                        fetchMakeAllSeen = () => dispatcher.dispatch<IPromise>(Actions.fetchMakeAllSeen()),
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
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
                    return new NotificationScreen(viewModel: viewModel, actionModel: actionModel);
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

    public class _NotificationScreenState : State<NotificationScreen>, RouteAware {
        const int firstPageNumber = 1;
        int notificationPageNumber = firstPageNumber;
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchNotifications();
                this.widget.actionModel.fetchNotifications(arg: firstPageNumber);
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
            Widget content;
            var notifications = this.widget.viewModel.notifications;
            if (this.widget.viewModel.notificationLoading && 0 == notifications.Count) {
                content = new GlobalLoading();
            }
            else if (0 == notifications.Count) {
                content = new BlankView(
                    "好冷清，多和小伙伴们互动呀",
                    "image/default-notification",
                    true,
                    () => {
                        this.widget.actionModel.startFetchNotifications();
                        this.widget.actionModel.fetchNotifications(arg: firstPageNumber);
                    }
                );
            }
            else {
                var enablePullUp = this.widget.viewModel.page < this.widget.viewModel.pageTotal;
                content = new Container(
                    color: CColors.Background,
                    child: new CustomListView(
                        controller: this._refreshController,
                        enablePullDown: true,
                        enablePullUp: enablePullUp,
                        onRefresh: this._onRefresh,
                        itemCount: notifications.Count,
                        itemBuilder: this._buildNotificationCard,
                        footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget,
                        hasScrollBar: false
                    )
                );
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Column(
                        children: new List<Widget> {
                            this._buildNavigationBar(),
                            new CustomDivider(
                                color: CColors.Separator2,
                                height: 1
                            ),
                            new Flexible(
                                child: new CustomScrollbar(child: content)
                            )
                        }
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomNavigationBar(
                new Text(
                    "通知",
                    style: CTextStyle.H2
                ),
                onBack: () => this.widget.actionModel.mainRouterPop()
            );
        }

        Widget _buildNotificationCard(BuildContext context, int index) {
            var notifications = this.widget.viewModel.notifications;

            var notification = notifications[index: index];
            if (notification.data.userId.isEmpty() && notification.data.role.Equals("user")) {
                return new Container();
            }

            User user;
            Team team;
            if (notification.type == "project_article_publish" && notification.data.role == "team") {
                user = null;
                team = this.widget.viewModel.teamDict[key: notification.data.teamId];
            }
            else {
                user = this.widget.viewModel.userDict[key: notification.data.userId];
                team = null;
            }

            return new NotificationCard(
                notification: notification,
                user: user,
                team: team,
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
                pushToTeamDetail: this.widget.actionModel.pushToTeamDetail,
                index == notifications.Count - 1,
                new ObjectKey(value: notification.id)
            );
        }

        void _onRefresh(bool up) {
            this.notificationPageNumber = up ? firstPageNumber : this.notificationPageNumber + 1;
            this.widget.actionModel.fetchNotifications(arg: this.notificationPageNumber)
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