using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.api {
    public static class SettingApi {
        public static IPromise<string> FetchReviewUrl(string platform, string store) {
            var promise = new Promise<string>();
            var request =
                HttpManager.GET(Config.apiAddress + $"/api/live/reviewUrl?platform={platform}&store={store}");
            HttpManager.resume(request).Then(responseText => {
                var urlDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                promise.Resolve(urlDictionary["url"]);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static IPromise<Dictionary<string, string>> FetchVersion(string platform, string store, string version) {
            var promise = new Promise<Dictionary<string, string>>();
            var request =
                HttpManager.GET(Config.apiAddress +
                                $"/api/live/version?platform={platform}&store={store}&version={version}");
            HttpManager.resume(request).Then(responseText => {
                var versionDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                promise.Resolve(versionDictionary);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}