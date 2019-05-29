using System;
using System.Collections;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;

namespace ConnectApp.api {
    public static class SocketApi {
        public static Promise<string> FetchSocketUrl() {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<string>();
            Window.instance.startCoroutine(_FetchSocketUrl(promise));
            return promise;
        }

        static IEnumerator
            _FetchSocketUrl(Promise<string> promise) {
            var request = HttpManager.GET(Config.apiAddress + "/api/socketgw");
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
                var response = JsonConvert.DeserializeObject<FetchSocketUrlResponse>(responseText);
                if (!string.IsNullOrEmpty(response.url)) {
                    promise.Resolve(response.url);
                }
                else {
                    promise.Reject(new Exception("No user under this username found!"));
                }
            }
        }
    }
}