using System;
using System.Collections;
using System.Collections.Generic;
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
        public static Promise<ArticlesResponse> FetchArticles(int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<ArticlesResponse>();
            Window.instance.startCoroutine(_FetchArticles(promise, pageNumber));
            return promise;
        }

        private static IEnumerator _FetchArticles(Promise<ArticlesResponse> promise, int pageNumber) {
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
                var articlesResponse = JsonConvert.DeserializeObject<ArticlesResponse>(responseText);
      
                if (responseText != null)
                    promise.Resolve(articlesResponse);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise FetchArticleDetail(string articleId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_FetchArticleDetail(promise, articleId));
            return promise;
        }

        private static IEnumerator _FetchArticleDetail(Promise promise, string articleId) {
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
                if (responseText != null)
                    promise.Resolve();
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }

        public static Promise FetchArticleCommentFirst(string channelId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_FetchArticleCommentFirst(promise, channelId));
            return promise;
        }

        private static IEnumerator _FetchArticleCommentFirst(Promise promise, string channelId) {
            var request = UnityWebRequest.Get(IApi.apiAddress + "/api/channels/" + channelId + "/messages?limit=5");
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

        public static Promise FetchArticleCommentMore(string channelId, string currOldestMessageId) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_FetchArticleCommentMore(promise, channelId, currOldestMessageId));
            return promise;
        }

        private static IEnumerator _FetchArticleCommentMore(Promise promise, string channelId,
            string currOldestMessageId) {
            var request = UnityWebRequest.Get(IApi.apiAddress + "/api/channels/" + channelId +
                                              "/messages?limit=5&before=" + currOldestMessageId);
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

        public static Promise ReportArticle(string articleId, string reportContext) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_ReportArticle(promise, articleId, reportContext));
            return promise;
        }

        private static IEnumerator _ReportArticle(Promise promise, string articleId, string reportContext) {
            var para = new ReportArticleParameter {
                itemType = "project",
                itemId = articleId,
                reasons = new List<string> {"other:" + reportContext}
            };
            var body = JsonConvert.SerializeObject(para);
            var request = new UnityWebRequest(IApi.apiAddress + "/api/report", "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
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