using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.api {
    public static class EventApi {
        public static IPromise<FetchEventsResponse> FetchEvents(int pageNumber, string tab, string mode) {
            if (tab == "completed") {
                mode = "";
            }

            var promise = new Promise<FetchEventsResponse>();
            var request = HttpManager.GET(Config.apiAddress +
                                          $"/api/events?tab={tab}&page={pageNumber}&mode={mode}&isPublic=true&pageSize=10");
            HttpManager.resume(request).Then(responseText => {
                var eventsResponse = JsonConvert.DeserializeObject<FetchEventsResponse>(responseText);
                promise.Resolve(eventsResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static IPromise<IEvent> FetchEventDetail(string eventId) {
            var promise = new Promise<IEvent>();
            var request = HttpManager.GET(Config.apiAddress + "/api/live/events/" + eventId);
            HttpManager.resume(request).Then(responseText => {
                var liveDetail = JsonConvert.DeserializeObject<IEvent>(responseText);
                promise.Resolve(liveDetail);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<string> JoinEvent(string eventId) {
            var promise = new Promise<string>();
            var request = HttpManager.initRequest(Config.apiAddress + $"/api/live/events/{eventId}/join", Method.POST);
            HttpManager.resume(request).Then(responseText => { promise.Resolve(eventId); })
                .Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}