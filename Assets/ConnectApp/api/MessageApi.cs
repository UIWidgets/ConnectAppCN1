using System;
using System.Collections;
using System.Text;
using ConnectApp.constants;
using ConnectApp.models;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public static class MessageApi {
        public static Promise FetchMessages(string channelId, string currOldestMessageId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_FetchMessages(promise, channelId, currOldestMessageId));
            return promise;
        }

        private static IEnumerator
            _FetchMessages(Promise promise, string channelId, string currOldestMessageId) {
            var url = IApi.apiAddress + "/api/channels/" + channelId + "/messages?limit=5";
            if (currOldestMessageId.Length > 0) url += "&before=" + currOldestMessageId;
            var request = UnityWebRequest.Get(url);
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
                var responseText = request.downloadHandler.text;
                Debug.Log(responseText);
                if (responseText != null)
                    promise.Resolve();
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise SendMessage(string channelId, string content, string nonce, string parentMessageId = "") {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_SendMessage(promise, channelId, content, nonce, parentMessageId));
            return promise;
        }

        private static IEnumerator _SendMessage(Promise promise, string channelId, string content, string nonce,
            string parentMessageId = "") {
            var para = new SendCommentParameter {
                content = content,
                parentMessageId = parentMessageId,
                nonce = nonce
            };
            var body = JsonConvert.SerializeObject(para);
            var request = new UnityWebRequest(IApi.apiAddress + "/api/channels/" + channelId + "/messages", "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("X-Requested-With", "XmlHttpRequest");
            request.SetRequestHeader("Content-Type", "application/json");
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
                Debug.Log(json);
                if (json != null)
                    promise.Resolve();
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}