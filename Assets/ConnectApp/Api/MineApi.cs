using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class MineApi {
        public static Promise<FetchEventsResponse> FetchMyFutureEvents(int pageNumber, string mode = "") {
            var promise = new Promise<FetchEventsResponse>();
            var para = new Dictionary<string, object> {
                {"tab", "my"},
                {"status", "ongoing"},
                {"page", pageNumber},
                {"mode", mode}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/events", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var eventsResponse = JsonConvert.DeserializeObject<FetchEventsResponse>(value: responseText);
                promise.Resolve(value: eventsResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchEventsResponse> FetchMyPastEvents(int pageNumber, string mode = "") {
            var promise = new Promise<FetchEventsResponse>();
            var para = new Dictionary<string, object> {
                {"tab", "my"},
                {"status", "completed"},
                {"page", pageNumber},
                {"mode", mode}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/events", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var eventsResponse = JsonConvert.DeserializeObject<FetchEventsResponse>(value: responseText);
                promise.Resolve(value: eventsResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}