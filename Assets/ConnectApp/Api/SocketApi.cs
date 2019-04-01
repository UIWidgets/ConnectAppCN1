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
    public static class SocketApi {
        public static Promise<string> FetchSocketUrl() {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<string>();
            Window.instance.startCoroutine(_FetchSocketUrl(promise));
            return promise;
        }

        private static IEnumerator
            _FetchSocketUrl(Promise<string> promise) {
            var request = UnityWebRequest.Get(Config.apiAddress + "/api/socketgw");
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
                var response = JsonConvert.DeserializeObject<FetchSocketUrlResponse>(responseText);
                if (!string.IsNullOrEmpty(response.url))
                    promise.Resolve(response.url);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}