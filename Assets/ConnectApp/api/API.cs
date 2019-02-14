using System;
using System.Collections;
using System.Collections.Generic;
using ConnectApp.models;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public class API {
        private const string apiAddress = "https://connect-dev.unity.com";

        public IPromise<List<IEvent>> FetchEvents(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<List<IEvent>>();
            Window.instance.startCoroutine(_FetchEvents(promise, pageNumber));
            return promise;
        }

        private IEnumerator _FetchEvents(Promise<List<IEvent>> promise, int pageNumber) {
            var request = UnityWebRequest.Get(apiAddress + "/api/live/events");
            yield return request.Send();

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
    }
}