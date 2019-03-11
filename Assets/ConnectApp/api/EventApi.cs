using System;
using System.Collections;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public class EventApi {
        public static IPromise<List<IEvent>> FetchEvents(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<List<IEvent>>();
            Window.instance.startCoroutine(_FetchEvents(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchEvents(Promise<List<IEvent>> promise, int pageNumber) {
            var request = UnityWebRequest.Get(IApi.apiAddress + "/api/live/events");
#pragma warning disable 618
            yield return request.Send();
#pragma warning restore 618

            if (request.isNetworkError) // something went wrong
            {
                promise.Reject(new Exception(request.error));
            }
            else if (request.responseCode != 200) // or the response is not OK 
            {
                promise.Reject(new Exception(request.downloadHandler.text));
            }
            else {
                // Format output and resolve promise
                var json = request.downloadHandler.text;
                var events = JsonConvert.DeserializeObject<List<IEvent>>(json);
                if (events != null)
                    promise.Resolve(events);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static IPromise<IEvent> FetchLiveDetail(string eventId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<IEvent>();
            Window.instance.startCoroutine(_FetchLiveDetail(promise, eventId));
            return promise;
        }

        private static IEnumerator _FetchLiveDetail(Promise<IEvent> promise, string eventId) {
            var request = UnityWebRequest.Get(IApi.apiAddress + "/api/live/events/" + eventId);
//            request.SetRequestHeader("Cookie", cookie);
#pragma warning disable 618
            yield return request.Send();
#pragma warning restore 618
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
                var json = request.downloadHandler.text;
                var liveDetail = JsonConvert.DeserializeObject<IEvent>(json);
                if (liveDetail != null)
                    promise.Resolve(liveDetail);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}