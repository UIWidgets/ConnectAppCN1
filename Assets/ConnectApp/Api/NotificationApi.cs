using System;
using System.Collections;
using ConnectApp.constants;
using ConnectApp.models;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public static class NotificationApi {
        public static Promise<FetchNotificationResponse> FetchNotifications(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<FetchNotificationResponse>();
            Window.instance.startCoroutine(_FetchNotifications(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchNotifications(Promise<FetchNotificationResponse> promise, int pageNumber) {
            var request = UnityWebRequest.Get(Config.apiAddress + "/api/notifications?page=" + pageNumber);
            request.SetRequestHeader("X-Requested-With", "XmlHttpRequest");
            yield return request.SendWebRequest();
            if (request.isNetworkError) {
                // something went wrong
                promise.Reject(new Exception(request.error));
            }
            else if (request.responseCode != 200) {
                // or the response is not OK
                promise.Reject(new Exception(request.downloadHandler.text));
            }
            else {
                // Format output and resolve promise
                var responseText = request.downloadHandler.text;
                var notificationResponse = JsonConvert.DeserializeObject<FetchNotificationResponse>(responseText);
                if (notificationResponse != null)
                    promise.Resolve(notificationResponse);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}