using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
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
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/reviewUrl", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var urlDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(value: responseText);
                promise.Resolve(urlDictionary["url"]);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static IPromise<CheckNewVersionResponse> CheckNewVersion(string platform, string store, string version) {
            var promise = new Promise<CheckNewVersionResponse>();
            var para = new Dictionary<string, object> {
                {"platform", platform},
                {"store", store},
                {"version", version}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/version", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var versionDictionary = JsonConvert.DeserializeObject<CheckNewVersionResponse>(value: responseText);
                promise.Resolve(value: versionDictionary);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}