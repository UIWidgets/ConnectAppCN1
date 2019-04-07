using ConnectApp.api;
using ConnectApp.models;
using Unity.UIWidgets.Redux;

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
        public static object fetchReviewUrl(string platform, string store) {
            return new ThunkAction<AppState>((dispatcher, getState) => {                
                return SettingApi.FetchReviewUrl(platform, store)
                    .Then(url => dispatcher.dispatch(new FetchReviewUrlSuccessAction {url = url}))
                    .Catch(error => { dispatcher.dispatch(new FetchReviewUrlFailureAction()); });
            });
        }
    }
}