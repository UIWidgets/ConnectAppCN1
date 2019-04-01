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
    public static class MineApi {
        public static Promise<List<IEvent>> FetchMyFutureEvents(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<List<IEvent>>();
            Window.instance.startCoroutine(_FetchMyFutureEvents(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchMyFutureEvents(Promise<List<IEvent>> promise, int pageNumber) {
            var request = UnityWebRequest.Get(Config.apiAddress + $"/api/live/my/events/future?page={pageNumber}");
            request.SetRequestHeader("X-Requested-With", "XmlHttpRequest");
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
                var responseText = request.downloadHandler.text;
                var events = JsonConvert.DeserializeObject<List<IEvent>>(responseText);
                if (events != null)
                    promise.Resolve(events);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise<List<IEvent>> FetchMyPastEvents(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<List<IEvent>>();
            Window.instance.startCoroutine(_FetchMyPastEvents(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchMyPastEvents(Promise<List<IEvent>> promise, int pageNumber) {
            var request = UnityWebRequest.Get(Config.apiAddress + $"/api/live/my/events/past?page={pageNumber}");
            request.SetRequestHeader("X-Requested-With", "XmlHttpRequest");
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
                var responseText = request.downloadHandler.text;
                var events = JsonConvert.DeserializeObject<List<IEvent>>(responseText);
                if (events != null)
                    promise.Resolve(events);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}