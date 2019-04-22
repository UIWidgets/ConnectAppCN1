using System;
using System.Collections;
using System.Collections.Generic;
using ConnectApp.redux;
using ConnectApp.redux.actions;
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
            UnityWebRequest.ClearCookieCache();
            request.SetRequestHeader(COOKIE, _cookieHeader());
            return request;
        }

        public static UnityWebRequest GET(string uri) {
            return initRequest(uri, Method.GET);
        }

        public static Promise<string> resume(UnityWebRequest request) {
            var promise = new Promise<string>();
            Window.instance.startCoroutine(sendRequest(promise, request));
            return promise;
        }

        private static IEnumerator sendRequest(Promise<string> promise, UnityWebRequest request) {
            yield return request.SendWebRequest();
            if (request.isNetworkError) {
                promise.Reject(new Exception(request.error));
            }
            else if (request.responseCode == 401) {
                StoreProvider.store.dispatcher.dispatch(new LogoutAction());
                promise.Reject(new Exception(request.downloadHandler.text));
            }
            else if (request.responseCode != 200) {
                promise.Reject(new Exception(request.downloadHandler.text));
            }
            else {
                if (request.GetResponseHeaders().ContainsKey("Set-Cookie")) {
                    var cookie = request.GetResponseHeaders()["Set-Cookie"];
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

        private static void updateCookie(string newCookie) {
            var cookie = PlayerPrefs.GetString(COOKIE);
            var cookieDict = new Dictionary<string, string>();
            var updateCookie = "";
            if (cookie.isNotEmpty()) {
                var cookieArr = cookie.Split(';');
                foreach (var c in cookieArr) {
                    var name = c.Split('=').first();
                    cookieDict.Add(name, c);
                }
            }

            if (newCookie.isNotEmpty()) {
                var newCookieArr = newCookie.Split(',');
                foreach (var c in newCookieArr) {
                    var item = c.Split(';').first();
                    var name = item.Split('=').first();
                    if (cookieDict.ContainsKey(name))
                        cookieDict[name] = item;
                    else
                        cookieDict.Add(name, item);
                }

                var updateCookieArr = cookieDict.Values;
                updateCookie = string.Join(";", updateCookieArr);
            }

            if (updateCookie.isNotEmpty()) {
                PlayerPrefs.SetString(COOKIE, updateCookie);
                PlayerPrefs.Save();
            }
        }
    }
}