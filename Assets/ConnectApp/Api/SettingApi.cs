using System;
using System.Collections;
using System.Collections.Generic;
using ConnectApp.constants;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public static class SettingApi {
        public static IPromise<string> FetchReviewUrl(string platform, string store) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<string>();
            Window.instance.startCoroutine(_FetchReviewUrl(promise, platform, store));
            return promise;
        }

        private static IEnumerator _FetchReviewUrl(Promise<string> promise, string platform, string store) {
            var request =
                UnityWebRequest.Get(Config.apiAddress + $"/api/live/reviewUrl?platform={platform}&store={store}");
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
                var urlDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                if (urlDictionary != null)
                    promise.Resolve(urlDictionary["url"]);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}