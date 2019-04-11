using System;
using System.Collections;
using System.Text;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public static class ArticleApi {
        // Article Api
        public static Promise<FetchArticlesResponse> FetchArticles(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<FetchArticlesResponse>();
            Window.instance.startCoroutine(_FetchArticles(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchArticles(Promise<FetchArticlesResponse> promise, int pageNumber) {
            var request = HttpManager.GET(Config.apiAddress + "/api/p?orderBy=latest&projectType=article&t=projects&page=" + pageNumber);
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
                if (request.GetResponseHeaders().ContainsKey("SET-COOKIE")) {
                    var cookie = request.GetResponseHeaders()["SET-COOKIE"];
                    HttpManager.updateCookie(cookie);
                }
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
            var request = HttpManager.GET(Config.apiAddress + "/api/p/" + articleId + "?view=true");
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
                if (request.GetResponseHeaders().ContainsKey("SET-COOKIE")) {
                    var cookie = request.GetResponseHeaders()["SET-COOKIE"];
                    HttpManager.updateCookie(cookie);
                }
                // Format output and resolve promise
                var responseText = request.downloadHandler.text;
                var articleDetailResponse = JsonConvert.DeserializeObject<FetchArticleDetailResponse>(responseText);
                if (responseText != null)
                    promise.Resolve(articleDetailResponse);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise<FetchCommentsResponse>
            FetchArticleComments(string channelId, string currOldestMessageId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<FetchCommentsResponse>();
            Window.instance.startCoroutine(_FetchArticleComments(promise, channelId, currOldestMessageId));
            return promise;
        }

        private static IEnumerator
            _FetchArticleComments(Promise<FetchCommentsResponse> promise, string channelId,
                string currOldestMessageId) {
            var url = Config.apiAddress + "/api/channels/" + channelId + "/messages?limit=5";
            if (currOldestMessageId.Length > 0) url += "&before=" + currOldestMessageId;
            var request = HttpManager.GET(url);
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
                if (request.GetResponseHeaders().ContainsKey("SET-COOKIE")) {
                    var cookie = request.GetResponseHeaders()["SET-COOKIE"];
                    HttpManager.updateCookie(cookie);
                }
                // Format output and resolve promise
                var responseText = request.downloadHandler.text;
                var responseComments = JsonConvert.DeserializeObject<FetchCommentsResponse>(responseText);
                if (responseText != null)
                    promise.Resolve(responseComments);
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
            var request = HttpManager.initRequest(Config.apiAddress + "/api/like", "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
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
                if (request.GetResponseHeaders().ContainsKey("SET-COOKIE")) {
                    var cookie = request.GetResponseHeaders()["SET-COOKIE"];
                    HttpManager.updateCookie(cookie);
                }
                var json = request.downloadHandler.text;
                Debug.Log(json);
                if (json != null)
                    promise.Resolve();
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise<Message> LikeComment(string commentId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<Message>();
            Window.instance.startCoroutine(_LikeComment(promise, commentId));
            return promise;
        }

        private static IEnumerator _LikeComment(Promise<Message> promise, string commentId) {
            var para = new ReactionParameter {
                reactionType = "like"
            };
            var body = JsonConvert.SerializeObject(para);
            var request =
                HttpManager.initRequest(Config.apiAddress + "/api/messages/" + commentId + "/addReaction", "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
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
                if (request.GetResponseHeaders().ContainsKey("SET-COOKIE")) {
                    var cookie = request.GetResponseHeaders()["SET-COOKIE"];
                    HttpManager.updateCookie(cookie);
                }
                var responseText = request.downloadHandler.text;
                var message = JsonConvert.DeserializeObject<Message>(responseText);
                if (responseText != null)
                    promise.Resolve(message);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise<Message> RemoveLikeComment(string commentId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<Message>();
            Window.instance.startCoroutine(_RemoveLikeComment(promise, commentId));
            return promise;
        }

        private static IEnumerator _RemoveLikeComment(Promise<Message> promise, string commentId) {
            var para = new ReactionParameter {
                reactionType = "like"
            };
            var body = JsonConvert.SerializeObject(para);
            var request =
                HttpManager.initRequest(Config.apiAddress + "/api/messages/" + commentId + "/removeReaction", "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
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
                if (request.GetResponseHeaders().ContainsKey("SET-COOKIE")) {
                    var cookie = request.GetResponseHeaders()["SET-COOKIE"];
                    HttpManager.updateCookie(cookie);
                }
                var responseText = request.downloadHandler.text;
                var message = JsonConvert.DeserializeObject<Message>(responseText);
                Debug.Log(responseText);
                if (responseText != null)
                    promise.Resolve(message);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise<Message> SendComment(string channelId, string content, string nonce,
            string parentMessageId = "") {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<Message>();
            Window.instance.startCoroutine(_SendComment(promise, channelId, content, nonce, parentMessageId));
            return promise;
        }

        private static IEnumerator _SendComment(Promise<Message> promise, string channelId, string content,
            string nonce,
            string parentMessageId = "") {
            var para = new SendCommentParameter {
                content = content,
                parentMessageId = parentMessageId,
                nonce = nonce
            };
            var body = JsonConvert.SerializeObject(para);
            var request = HttpManager.initRequest(Config.apiAddress + "/api/channels/" + channelId + "/messages", "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
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
                if (request.GetResponseHeaders().ContainsKey("SET-COOKIE")) {
                    var cookie = request.GetResponseHeaders()["SET-COOKIE"];
                    HttpManager.updateCookie(cookie);
                }
                var responseText = request.downloadHandler.text;
                var message = JsonConvert.DeserializeObject<Message>(responseText);
                if (responseText != null)
                    promise.Resolve(message);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}