using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class StartReportItemAction : RequestAction {
        public string itemId;
        public string itemType;
        public string reportContext;
    }

    public class ReportItemSuccessAction : BaseAction {
    }

    public class ReportItemFailureAction : BaseAction {
    }

    public class ChangeFeedbackTypeAction : BaseAction {
        public FeedbackType type;
    }

    public class StartFeedbackAction : RequestAction {
    }

    public class FeedbackSuccessAction : BaseAction {
    }

    public class FeedbackFailureAction : BaseAction {
    }

    public static partial class Actions {
        public static object reportItem(string itemId, string itemType, string reportContext) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ReportApi.ReportItem(itemId, itemType, reportContext)
                    .Then(() => {
                        dispatcher.dispatch(new MainNavigatorPopAction());
                        CustomDialogUtils.showToast("举报成功", Icons.sentiment_satisfied);
                        dispatcher.dispatch(new ReportItemSuccessAction());
                    })
                    .Catch(error => {
                        CustomDialogUtils.showToast("举报失败", Icons.sentiment_dissatisfied);
                        dispatcher.dispatch(new ReportItemFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object feedback(FeedbackType type, string content, string name = "", string contact = "") {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ReportApi.Feedback(type, content, name, contact)
                    .Then(() => {
                        dispatcher.dispatch(new MainNavigatorPopAction());
                        CustomDialogUtils.showToast("反馈成功", Icons.sentiment_satisfied);
                        dispatcher.dispatch(new FeedbackSuccessAction());
                    })
                    .Catch(error => {
                        CustomDialogUtils.showToast("发送失败", Icons.sentiment_dissatisfied);
                        dispatcher.dispatch(new FeedbackFailureAction());
                    });
            });
        }
    }
}