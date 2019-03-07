using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ConnectApp.models;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public class API {
        private const string apiAddress = "https://connect-dev.unity.com";
        private static string cookie = "";

        public static IPromise<List<IEvent>> FetchEvents(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<List<IEvent>>();
            Window.instance.startCoroutine(_FetchEvents(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchEvents(Promise<List<IEvent>> promise, int pageNumber) {
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

        public static IPromise<LiveInfo> FetchLiveDetail(string eventId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<LiveInfo>();
            Window.instance.startCoroutine(_FetchLiveDetail(promise, eventId));
            return promise;
        }

        private static IEnumerator _FetchLiveDetail(Promise<LiveInfo> promise, string eventId) {
            var request = UnityWebRequest.Get(apiAddress + "/api/live/events/" + eventId);
            request.SetRequestHeader("Cookie", cookie);

            yield return request.Send();
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
                var json = request.downloadHandler.text;
                var liveDetail = JsonConvert.DeserializeObject<LiveInfo>(json);
                if (liveDetail != null)
                    promise.Resolve(liveDetail);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise FetchNotifications(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_FetchNotifications(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchNotifications(Promise promise, int pageNumber) {
            var request = UnityWebRequest.Get(apiAddress + "/api/notifications?page=" + pageNumber);
            request.SetRequestHeader("X-Requested-With", "XmlHttpRequest");

            yield return request.Send();
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
                Debug.Log(responseText);
                if (responseText != null)
                    promise.Resolve();
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static IPromise<LoginInfo> LoginByEmail(string email, string password) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<LoginInfo>();
            Window.instance.startCoroutine(_LoginByEmail(promise, email, password));
            return promise;
        }

        private static IEnumerator _LoginByEmail(IPendingPromise<LoginInfo> promise, string email, string password) {
            var para = new LoginParameter {
                email = email,
                password = password
            };
            var body = JsonConvert.SerializeObject(para);
            var request = new UnityWebRequest(apiAddress + "/auth/live/login", "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.Send();

            if (request.isNetworkError) {
                // something went wrong
                promise.Reject(new Exception(request.error));
            }
            else if (request.responseCode != 200) {
                // or the response is not OK
                promise.Reject(new Exception(request.downloadHandler.text));
            }
            else {
                if (request.GetResponseHeaders().ContainsKey("SET-COOKIE"))
                    cookie = request.GetResponseHeaders()["SET-COOKIE"];
                // Format output and resolve promise
                var json = request.downloadHandler.text;
                var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(json);
                Debug.Log(loginInfo);
                if (loginInfo != null)
                    promise.Resolve(loginInfo);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}