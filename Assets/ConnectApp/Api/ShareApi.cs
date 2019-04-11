using System;
using System.Collections;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public class ShareApi {
        public static Promise<byte[]> FetchImageBytes(string url) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<byte[]>();
            Window.instance.startCoroutine(_FetchImageBytes(promise, url));
            return promise;
        }

        private static IEnumerator
            _FetchImageBytes(Promise<byte[]> promise, string url) {
            var request = UnityWebRequestTexture.GetTexture(url);
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
                var data = ((DownloadHandlerTexture) request.downloadHandler).texture.GetRawTextureData();
                if (data != null)
                    promise.Resolve(data);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}