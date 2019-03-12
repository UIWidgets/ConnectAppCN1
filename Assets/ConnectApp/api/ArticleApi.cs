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
    public class ArticleApi {
        // Article Api
        public static Promise<FetchArticlesResponse> FetchArticles(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<FetchArticlesResponse>();
            Window.instance.startCoroutine(_FetchArticles(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchArticles(Promise<FetchArticlesResponse> promise, int pageNumber) {
            var request =
                UnityWebRequest.Get(IApi.apiAddress + "/api/p?projectType=article&t=projects&page=" + pageNumber);
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
                var articlesResponse = JsonConvert.DeserializeObject<FetchArticlesResponse>(responseText);

                if (responseText != null)
                    promise.Resolve(articlesResponse);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise<FetchArticleDetailResponse> FetchArticleDetail(string articleId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<FetchArticleDetailResponse>();
            Window.instance.startCoroutine(_FetchArticleDetail(promise, articleId));
            return promise;
        }

        private static IEnumerator _FetchArticleDetail(Promise<FetchArticleDetailResponse> promise, string articleId) {
            var request = UnityWebRequest.Get(IApi.apiAddress + "/api/p/" + articleId);
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
                var articleDetailResponse = JsonConvert.DeserializeObject<FetchArticleDetailResponse>(responseText);
                if (responseText != null)
                    promise.Resolve(articleDetailResponse);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise FetchArticleComments(string channelId, string currOldestMessageId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_FetchArticleComments(promise, channelId, currOldestMessageId));
            return promise;
        }

        private static IEnumerator
            _FetchArticleComments(Promise promise, string channelId, string currOldestMessageId) {
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

        public static Promise LikeArticle(string articleId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_LikeArticle(promise, articleId));
            return promise;
        }

        private static IEnumerator _LikeArticle(Promise promise, string articleId) {
            var para = new LikeArticleParameter {
                type = "project",
                itemId = articleId
            };
            var body = JsonConvert.SerializeObject(para);
            var request = new UnityWebRequest(IApi.apiAddress + "/api/like", "POST");
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

        public static Promise LikeComment(string commentId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_LikeComment(promise, commentId));
            return promise;
        }

        private static IEnumerator _LikeComment(Promise promise, string commentId) {
            var para = new ReactionParameter {
                reactionType = "like"
            };
            var body = JsonConvert.SerializeObject(para);
            var request = new UnityWebRequest(IApi.apiAddress + "/api/messages/" + commentId + "/addReaction", "POST");
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

        public static Promise RemoveLikeComment(string commentId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_RemoveLikeComment(promise, commentId));
            return promise;
        }

        private static IEnumerator _RemoveLikeComment(Promise promise, string commentId) {
            var para = new ReactionParameter {
                reactionType = "like"
            };
            var body = JsonConvert.SerializeObject(para);
            var request =
                new UnityWebRequest(IApi.apiAddress + "/api/messages/" + commentId + "/removeReaction", "POST");
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

        public static Promise SendComment(string channelId, string content, string nonce, string parentMessageId = "") {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_SendComment(promise, channelId, content, nonce, parentMessageId));
            return promise;
        }

        private static IEnumerator _SendComment(Promise promise, string channelId, string content, string nonce,
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