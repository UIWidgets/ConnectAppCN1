using System;
using System.Collections;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public static class MineApi {
        public static Promise<FetchEventsResponse> FetchMyFutureEvents(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<FetchEventsResponse>();
            Window.instance.startCoroutine(_FetchMyFutureEvents(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchMyFutureEvents(Promise<FetchEventsResponse> promise, int pageNumber) {
            var request = HttpManager.GET(Config.apiAddress + $"/api/events?tab=my&status=ongoing&page={pageNumber}");
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
                var eventsResponse = JsonConvert.DeserializeObject<FetchEventsResponse>(responseText);
                if (eventsResponse != null)
                    promise.Resolve(eventsResponse);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise<FetchEventsResponse> FetchMyPastEvents(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<FetchEventsResponse>();
            Window.instance.startCoroutine(_FetchMyPastEvents(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchMyPastEvents(Promise<FetchEventsResponse> promise, int pageNumber) {
            var request = HttpManager.GET(Config.apiAddress + $"/api/events?tab=my&status=completed&page={pageNumber}");
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
                var eventsResponse = JsonConvert.DeserializeObject<FetchEventsResponse>(responseText);
                if (eventsResponse != null)
                    promise.Resolve(eventsResponse);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}