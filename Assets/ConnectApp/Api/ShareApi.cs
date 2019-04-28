using System;
using System.Collections;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public static class ShareApi {
        public static Promise<byte[]> FetchImageBytes(string url) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<byte[]>();
            Window.instance.startCoroutine(_FetchImageBytes(promise, url));
            return promise;
        }

        private static IEnumerator
            _FetchImageBytes(Promise<byte[]> promise, string url) {
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
            else
            {
                if (url.EndsWith(".jpg"))
                {
                    var data = ((DownloadHandlerTexture) request.downloadHandler).texture.EncodeToJPG();
                    if (data != null)
                        promise.Resolve(data);
                    else
                        promise.Reject(new Exception("No user under this username found!"));
                }
                else if(url.EndsWith(".png"))
                {
                    var data = ((DownloadHandlerTexture) request.downloadHandler).texture.EncodeToPNG();
                    if (data != null)
                        promise.Resolve(data);
                    else
                        promise.Reject(new Exception("No user under this username found!"));
                }
                else
                {
                    promise.Reject(new Exception("no picture"));
                }

            }
        }
    }
}