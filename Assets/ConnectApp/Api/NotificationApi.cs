using System;
using System.Collections;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;

namespace ConnectApp.api {
    public static class NotificationApi {
        public static Promise<FetchNotificationResponse> FetchNotifications(int pageNumber) {
            var promise = new Promise<FetchNotificationResponse>();
            var request = HttpManager.GET(Config.apiAddress + "/api/notifications/app?page=" + pageNumber);
            HttpManager.resume(request).Then(responseText =>
            {  
                var notificationResponse = JsonConvert.DeserializeObject<FetchNotificationResponse>(responseText);
                promise.Resolve(notificationResponse);
            }).Catch(exception =>
            {
                promise.Reject(exception);  
            });
            return promise;
        }
        
        public static Promise FetchMakeAllSeen() {
            var promise = new Promise();
            var request = HttpManager.initRequest(Config.apiAddress + "/api/notifications/make-all-seen", Method.POST);
            HttpManager.resume(request).Then(responseText =>
            {  
                promise.Resolve();
            }).Catch(exception =>
            {
                promise.Reject(exception);  
            });
            return promise;
        }
    }
}