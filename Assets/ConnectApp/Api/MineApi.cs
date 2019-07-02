using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class MineApi {
        public static Promise<FetchEventsResponse> FetchMyFutureEvents(int pageNumber) {
            var promise = new Promise<FetchEventsResponse>();
            var para = new Dictionary<string, object> {
                {"tab", "my"},
                {"status", "ongoing"},
                {"mode", "offline"},
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/events", para);
            HttpManager.resume(request).Then(responseText => {
                var eventsResponse = JsonConvert.DeserializeObject<FetchEventsResponse>(responseText);
                promise.Resolve(eventsResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<FetchEventsResponse> FetchMyPastEvents(int pageNumber) {
            var promise = new Promise<FetchEventsResponse>();
            var para = new Dictionary<string, object> {
                {"tab", "my"},
                {"status", "completed"},
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/events", para);
            HttpManager.resume(request).Then(responseText => {
                var eventsResponse = JsonConvert.DeserializeObject<FetchEventsResponse>(responseText);
                promise.Resolve(eventsResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}