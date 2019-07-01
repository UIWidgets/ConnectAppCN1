using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class NotificationApi {
        public static Promise<FetchNotificationResponse> FetchNotifications(int pageNumber) {
            var promise = new Promise<FetchNotificationResponse>();
            var request = HttpManager.GET("/api/connectapp/notifications?page=" + pageNumber);
            HttpManager.resume(request).Then(responseText => {
                var notificationResponse = JsonConvert.DeserializeObject<FetchNotificationResponse>(responseText);
                promise.Resolve(notificationResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise FetchMakeAllSeen() {
            var promise = new Promise();
            var request = HttpManager.POST("/api/notifications/make-all-seen");
            HttpManager.resume(request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}