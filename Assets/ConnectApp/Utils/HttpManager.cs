using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ConnectApp.Api;
using ConnectApp.Constants;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.Utils {
    public static class Method {
        public const string GET = "GET";
        public const string POST = "POST";
    }

    public class HttpResponseContent {
        public string text;
        public Dictionary<string, string> headers;
    }

    public static class HttpManager {
        public const string COOKIE = "Cookie";

        static UnityWebRequest initRequest(
            string url,
            string method) {
            var request = new UnityWebRequest {
                url = url,
                method = method,
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("X-Requested-With", "XmlHttpRequest");
            UnityWebRequest.ClearCookieCache();
            request.SetRequestHeader(COOKIE, _cookieHeader());
            request.SetRequestHeader("ConnectAppVersion", Config.versionName);
            return request;
        }

        public static UnityWebRequest GET(string uri, object parameter = null) {
            var newUri = uri;
            if (parameter != null) {
                string parameterString = "";
                var par = JsonHelper.ToDictionary(json: parameter);
                foreach (var keyValuePair in par) {
                    parameterString += $"{keyValuePair.Key}={keyValuePair.Value}&";
                }

                if (parameterString.Length > 0) {
                    var newParameterString = parameterString.Remove(parameterString.Length - 1);
                    newUri += $"?{newParameterString}";
                }
            }

            return initRequest(url: newUri, method: Method.GET);
        }

        public static UnityWebRequest POST(string uri, object parameter = null, bool multipart = false,
            string filename = "", string fileType = "") {
            var request = initRequest(url: uri, method: Method.POST);
            if (parameter != null) {
                var boundary = $"----WebKitFormBoundary{Snowflake.CreateNonce()}";
                if (multipart) {
                    List<byte[]> results = new List<byte[]>();
                    int size = 0;
                    if (parameter is List<List<object>> list) {
                        foreach (List<object> item in list) {
                            D.assert(item.Count == 2);
                            D.assert(item[0] is string);
                            if (item[1] == null) {
                                continue;
                            }

                            if (item[1] is byte[]) {
                                var itemStr =
                                    $"--{boundary}\r\nContent-Disposition: form-data; name=\"{item[0]}\"; filename=\"{filename}\"\r\n" +
                                    $"Content-Type: {fileType}\r\n\r\n";
                                results.Add(Encoding.UTF8.GetBytes(itemStr));
                                size += results.last().Length;
                                results.Add(item[1] as byte[]);
                                size += results.last().Length;
                                results.Add(Encoding.UTF8.GetBytes("\r\n"));
                                size += results.last().Length;
                            }
                            else {
                                string s = $"{item[1]}";
                                var itemStr =
                                    $"--{boundary}\r\nContent-Disposition: form-data; name=\"{item[0]}\"\r\n\r\n{s}\r\n";
                                results.Add(Encoding.UTF8.GetBytes(itemStr));
                                size += results.last().Length;
                            }
                        }
                    }
                    else {
                        D.assert(false, () => "Parameter must be list of lists");
                    }

                    results.Add(Encoding.UTF8.GetBytes($"--{boundary}--"));
                    size += results.last().Length;
                    byte[] bodyRaw = new byte[size];
                    int offset = 0;
                    foreach (byte[] bytes in results) {
                        Buffer.BlockCopy(bytes, 0, bodyRaw, offset, bytes.Length);
                        offset += bytes.Length;
                    }

                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.SetRequestHeader("Content-Type", $"multipart/form-data; boundary={boundary}");
                }
                else {
                    var body = JsonConvert.SerializeObject(value: parameter);
                    var bodyRaw = Encoding.UTF8.GetBytes(s: body);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.SetRequestHeader("Content-Type", "application/json");
                }
            }

            return request;
        }

        public static Promise<Texture2D> DownloadImage(string url) {
            var promise = new Promise<Texture2D>();
            Window.instance.startCoroutine(fetchImageBytes(promise: promise, url: url));
            return promise;
        }

        public static Promise<string> resume(UnityWebRequest request) {
            var promise = new Promise<string>();
            Window.instance.startCoroutine(sendRequest(promise, request));
            return promise;
        }

        public static Promise<HttpResponseContent> resumeAll(UnityWebRequest request) {
            var promise = new Promise<HttpResponseContent>();
            Window.instance.startCoroutine(sendRequestAll(promise, request));
            return promise;
        }

        static IEnumerator sendRequestAll(Promise<HttpResponseContent> promise, UnityWebRequest request) {
            yield return request.SendWebRequest();
            if (request.isNetworkError) {
                promise.Reject(new Exception("NetworkError"));
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

                var content = new HttpResponseContent {
                    text = request.downloadHandler.text,
                    headers = request.GetResponseHeaders()
                };

                promise.Resolve(content);
            }
        }

        static IEnumerator sendRequest(Promise<string> promise, UnityWebRequest request) {
            yield return request.SendWebRequest();
            if (request.isNetworkError) {
                promise.Reject(new Exception("NetworkError"));
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

        static IEnumerator fetchImageBytes(Promise<Texture2D> promise, string url) {
            var request = UnityWebRequestTexture.GetTexture(url);
            request.SetRequestHeader("X-Requested-With", "XmlHttpRequest");
            yield return request.SendWebRequest();
            if (request.isNetworkError) {
                promise.Reject(new Exception(request.error));
            }
            else if (request.responseCode != 200) {
                promise.Reject(new Exception(request.downloadHandler.text));
            }
            else {
                var texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                if (texture) {
                    promise.Resolve(texture);
                }
                else {
                    promise.Reject(new Exception("no picture"));
                }
            }
        }

        static string _cookieHeader() {
            if (PlayerPrefs.GetString(COOKIE).isNotEmpty()) {
                return PlayerPrefs.GetString(COOKIE);
            }

            return "";
        }

        public static void clearCookie() {
            PlayerPrefs.SetString(COOKIE, "");
            PlayerPrefs.Save();

            SocketApi.OnCookieChanged();
        }

        public static string getCookie() {
            return _cookieHeader();
        }

        public static string getCookie(string key) {
            var cookie = getCookie();
            if (cookie.isNotEmpty()) {
                var cookieArr = cookie.Split(';');
                foreach (var c in cookieArr) {
                    var carr = c.Split('=');

                    if (carr.Length != 2) {
                        continue;
                    }

                    var name = carr[0].Trim();
                    var value = carr[1].Trim();
                    if (name == key) {
                        return value;
                    }
                }
            }

            return "";
        }

        public static void updateCookie(string newCookie) {
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
                    if (cookieDict.ContainsKey(name)) {
                        cookieDict[name] = item;
                    }
                    else {
                        cookieDict.Add(name, item);
                    }
                }

                var updateCookieArr = cookieDict.Values;
                updateCookie = string.Join(";", updateCookieArr);
            }

            if (updateCookie.isNotEmpty()) {
                PlayerPrefs.SetString(COOKIE, updateCookie);
                PlayerPrefs.Save();

                SocketApi.OnCookieChanged();
            }
        }

        public static bool isNetWorkError() {
            return Application.internetReachability == NetworkReachability.NotReachable;
        }
    }
}