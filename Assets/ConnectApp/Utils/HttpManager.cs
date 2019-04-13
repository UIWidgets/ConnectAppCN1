using System;
using System.Collections;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.utils {
    public static class Method {
        public const string GET = "GET";
        public const string POST = "POST";
    }

    public static class HttpManager {
        private const string COOKIE = "Cookie";

        internal static UnityWebRequest initRequest(
            string url,
            string method) {
            var request = new UnityWebRequest(url, method);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("X-Requested-With", "XmlHttpRequest");
            request.SetRequestHeader(COOKIE, _cookieHeader());
            return request;
        }

        public static UnityWebRequest GET(string uri) {
            return initRequest(uri, Method.GET);
        }


        public static Promise<string> resume(UnityWebRequest request)
        {
            
            var promise = new Promise<string>();
            Window.instance.startCoroutine(sendRequest(promise,request));
            return promise;
        }

        private static IEnumerator sendRequest(Promise<string> promise,UnityWebRequest request)
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError) {
                promise.Reject(new Exception(request.error));
            }
            else if (request.responseCode == 403) {
                
                promise.Reject(new Exception(request.downloadHandler.text));
            }
            else if (request.responseCode != 200) {
                promise.Reject(new Exception(request.downloadHandler.text));
            }
            else
            {
                if (request.GetResponseHeaders().ContainsKey("SET-COOKIE")) {
                    var cookie = request.GetResponseHeaders()["SET-COOKIE"];
                    updateCookie(cookie);
                }
                promise.Resolve(request.downloadHandler.text);
            }
            
        }


        private static string _cookieHeader() {
            if (PlayerPrefs.GetString(COOKIE).isNotEmpty()) return PlayerPrefs.GetString(COOKIE);

            return "";
        }

        public static void clearCookie() {
            PlayerPrefs.DeleteKey(COOKIE);
        }

        public static void updateCookie(string newCookie) {
            var oldCookie = PlayerPrefs.GetString(COOKIE);
            if (oldCookie.isEmpty() || !oldCookie.Equals(newCookie)) {
                PlayerPrefs.SetString(COOKIE, newCookie);
                PlayerPrefs.Save();
            }
        }
    }
}