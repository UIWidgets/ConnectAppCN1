using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class EventApi {
        public static IPromise<FetchEventsResponse> FetchEvents(int pageNumber, string tab, string mode = "") {
            var promise = new Promise<FetchEventsResponse>();
            var para = new Dictionary<string, object> {
                {"tab", tab},
                {"page", pageNumber},
                {"status", tab},
                {"language", "zh_CN"},
                {"mode", mode}
            };
            var request = HttpManager.GET($"{Config.apiAddress_com}{Config.apiPath}/events", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var eventsResponse = JsonConvert.DeserializeObject<FetchEventsResponse>(value: responseText);
                promise.Resolve(value: eventsResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static IPromise<IEvent> FetchEventDetail(string eventId) {
            var promise = new Promise<IEvent>();
            var request = HttpManager.GET($"{Config.apiAddress_com}{Config.apiPath}/events/{eventId}");
            HttpManager.resume(request: request).Then(responseText => {
                var eventDetail = JsonConvert.DeserializeObject<IEvent>(value: responseText);
                promise.Resolve(value: eventDetail);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<string> JoinEvent(string eventId) {
            var promise = new Promise<string>();
            var request = HttpManager.POST($"{Config.apiAddress_com}{Config.apiPath}/events/{eventId}/join");
            HttpManager.resume(request: request).Then(responseText => promise.Resolve(value: eventId))
                .Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}