using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class MineApi {
        public static Promise<FetchEventsResponse> FetchMyFutureEvents(int pageNumber) {
            var promise = new Promise<FetchEventsResponse>();
            var request = HttpManager.GET($"/api/connectapp/events?tab=my&status=ongoing&mode=offline&page={pageNumber}");
            HttpManager.resume(request).Then(responseText => {
                var eventsResponse = JsonConvert.DeserializeObject<FetchEventsResponse>(responseText);
                promise.Resolve(eventsResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<FetchEventsResponse> FetchMyPastEvents(int pageNumber) {
            var promise = new Promise<FetchEventsResponse>();
            var request = HttpManager.GET($"/api/connectapp/events?tab=my&status=completed&page={pageNumber}");
            HttpManager.resume(request).Then(responseText => {
                var eventsResponse = JsonConvert.DeserializeObject<FetchEventsResponse>(responseText);
                promise.Resolve(eventsResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}