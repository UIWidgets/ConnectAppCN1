using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class NotificationApi {
        public static Promise<FetchNotificationResponse> FetchNotifications(int pageNumber) {
            var promise = new Promise<FetchNotificationResponse>();
            var para = new Dictionary<string, object> {
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/notifications", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var notificationResponse = JsonConvert.DeserializeObject<FetchNotificationResponse>(value: responseText);
                promise.Resolve(value: notificationResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise FetchMakeAllSeen() {
            var promise = new Promise();
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/notifications/make-all-seen");
            HttpManager.resume(request: request).Then(responseText => promise.Resolve())
                .Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}