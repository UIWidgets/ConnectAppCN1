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
    public class MessageScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MessageScreenViewModel>(
                converter: state => new MessageScreenViewModel {
                    notificationLoading = state.notificationState.loading,
                    page = state.notificationState.page,
                    pageTotal = state.notificationState.pageTotal,
                    notifications = state.notificationState.notifications,
                    mentions = state.notificationState.mentions,
                    userDict = state.userState.userDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MessageScreenActionModel {
                    };
                    return new MessageScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class MessageScreen : StatefulWidget {
        public MessageScreen(
            MessageScreenViewModel viewModel = null,
            MessageScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly MessageScreenViewModel viewModel;
        public readonly MessageScreenActionModel actionModel;

        public override State createState() {
            return new _MessageScreenState();
        }
    }

    public class _MessageScreenState : AutomaticKeepAliveClientMixin<MessageScreen>, RouteAware {
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
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
        }

        public override void dispose() {
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            return new Container();
        }

        public void didPopNext() {
        }

        public void didPush() {
        }

        public void didPop() {
        }

        public void didPushNext() {
        }
    }
}
