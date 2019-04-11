using ConnectApp.api;
using ConnectApp.models;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public class StartReportItemAction : RequestAction {
        public string itemId;
        public string itemType;
        public string reportContext;
    }

    public class ReportItemSuccessAction : BaseAction {
    }

    public static partial class Actions {
        public static object reportItem(string itemId, string itemType, string reportContext) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ReportApi.ReportItem(itemId, itemType, reportContext)
                    .Then(() => dispatcher.dispatch(new ReportItemSuccessAction()))
                    .Catch(Debug.Log);
            });
        }
    }
}