using System;
using System.Collections;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public static class EventApi {
        public static IPromise<FetchEventsResponse> FetchEvents(int pageNumber, string tab) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<FetchEventsResponse>();
            Window.instance.startCoroutine(_FetchEvents(promise, pageNumber, tab));
            return promise;
        }

        private static IEnumerator _FetchEvents(Promise<FetchEventsResponse> promise, int pageNumber, string tab) {
            var request = UnityWebRequest.Get(IApi.apiAddress + $"/api/events?tab={tab}&page={pageNumber}");
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
                var json = request.downloadHandler.text;
                var events = JsonConvert.DeserializeObject<FetchEventsResponse>(json);
                if (events != null)
                    promise.Resolve(events);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static IPromise<IEvent> FetchEventDetail(string eventId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<IEvent>();
            Window.instance.startCoroutine(_FetchEventDetail(promise, eventId));
            return promise;
        }

        private static IEnumerator _FetchEventDetail(Promise<IEvent> promise, string eventId) {
            var request = UnityWebRequest.Get(IApi.apiAddress + "/api/live/events/" + eventId);
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
                var json = request.downloadHandler.text;
                var liveDetail = JsonConvert.DeserializeObject<IEvent>(json);
                if (liveDetail != null)
                    promise.Resolve(liveDetail);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise<string> JoinEvent(string eventId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<string>();
            Window.instance.startCoroutine(_JoinEvent(promise, eventId));
            return promise;
        }

        private static IEnumerator _JoinEvent(Promise<string> promise, string eventId) {
            var request = new UnityWebRequest(IApi.apiAddress + $"/api/live/events/{eventId}/join", "POST");
            request.downloadHandler = new DownloadHandlerBuffer();
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
                var json = request.downloadHandler.text;
                if (json != null)
                    promise.Resolve(eventId);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}