using System.Collections;
using ConnectApp.api;
using ConnectApp.constants;
using ConnectApp.models;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;

namespace ConnectApp.redux.actions {
    public class FetchReviewUrlAction : RequestAction {
        public string platform;
        public string store;
    }

    public class FetchReviewUrlSuccessAction : BaseAction {
        public string url;
    }

    public class FetchReviewUrlFailureAction : BaseAction {}

    public class SettingClearCacheAction : BaseAction {
    }
    
    public static partial class Actions {
        public static object fetchReviewUrl() {
            return new ThunkAction<AppState>((dispatcher, getState) => {                
                return SettingApi.FetchReviewUrl(Config.platform, Config.store)
                    .Then(url => dispatcher.dispatch(new FetchReviewUrlSuccessAction {url = url}))
                    .Catch(error => { dispatcher.dispatch(new FetchReviewUrlFailureAction()); });
            });
        }
    }
}