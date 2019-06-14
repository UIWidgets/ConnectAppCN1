using System;
using System.Collections;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.Api {
    public static class SplashApi {
        public static Promise<Splash> FetchSplash() {
            var promise = new Promise<Splash>();
            var request = HttpManager.GET(Config.apiAddress + "/api/live/ads");
            HttpManager.resume(request).Then(responseText => {
                var splashResponse = JsonConvert.DeserializeObject<Splash>(responseText);
                promise.Resolve(splashResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<byte[]> FetchSplashImage(string url) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<byte[]>();
            Window.instance.startCoroutine(_FetchSplashImage(promise, url));
            return promise;
        }

        static IEnumerator
            _FetchSplashImage(Promise<byte[]> promise, string url) {
            var request = UnityWebRequestTexture.GetTexture(url);
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
                var texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                var pngData = texture.EncodeToPNG();
                if (pngData != null) {
                    promise.Resolve(pngData);
                }
                else {
                    promise.Reject(new Exception("No user under this username found!"));
                }
            }
        }
    }
}