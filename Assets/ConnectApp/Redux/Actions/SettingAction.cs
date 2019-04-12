using ConnectApp.api;
using ConnectApp.constants;
using ConnectApp.models;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class FetchReviewUrlSuccessAction : BaseAction {
        public string url;
    }

    public class FetchReviewUrlFailureAction : BaseAction {
    }

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