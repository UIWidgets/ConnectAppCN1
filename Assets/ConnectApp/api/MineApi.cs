using System;
using System.Collections;
using ConnectApp.constants;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public class MineApi {
        public static Promise FetchMyFutureEvents(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_FetchMyFutureEvents(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchMyFutureEvents(Promise promise, int pageNumber) {
            var request = UnityWebRequest.Get(IApi.apiAddress + $"/api/live/my/events/future?page={pageNumber}");
            request.SetRequestHeader("X-Requested-With", "XmlHttpRequest");
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
                Debug.Log(json);
//                var events = JsonConvert.DeserializeObject<List<IEvent>>(json);
                if (json != null)
                    promise.Resolve();
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise FetchMyPastEvents(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_FetchMyPastEvents(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchMyPastEvents(Promise promise, int pageNumber) {
            var request = UnityWebRequest.Get(IApi.apiAddress + $"/api/live/my/events/past?page={pageNumber}");
            request.SetRequestHeader("X-Requested-With", "XmlHttpRequest");
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
//                var events = JsonConvert.DeserializeObject<List<IEvent>>(json);
                Debug.Log(json);
                if (json != null)
                    promise.Resolve();
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}