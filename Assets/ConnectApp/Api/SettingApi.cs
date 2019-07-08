using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class SettingApi {
        public static IPromise<string> FetchReviewUrl(string platform, string store) {
            var promise = new Promise<string>();
            var para = new Dictionary<string, object> {
                {"platform", platform},
                {"store", store}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/reviewUrl", para);
            HttpManager.resume(request).Then(responseText => {
                var urlDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                promise.Resolve(urlDictionary["url"]);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static IPromise<Dictionary<string, string>> FetchVersion(string platform, string store, string version) {
            var promise = new Promise<Dictionary<string, string>>();
            var para = new Dictionary<string, object> {
                {"platform", platform},
                {"store", store},
                {"version", version}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/version", para);
            HttpManager.resume(request).Then(responseText => {
                var versionDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                promise.Resolve(versionDictionary);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}