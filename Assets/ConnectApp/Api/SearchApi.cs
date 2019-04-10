using System;
using System.Collections;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public static class SearchApi {
        public static Promise<List<PopularSearch>> PopularSearch() {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<List<PopularSearch>>();
            Window.instance.startCoroutine(_PopularSearch(promise));
            return promise;
        }

        private static IEnumerator _PopularSearch(Promise<List<PopularSearch>> promise) {
            var request = HttpManager.GET(Config.apiAddress + "/api/search/popularSearch");
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
                var popularSearch = JsonConvert.DeserializeObject<List<PopularSearch>>(responseText);
                if (popularSearch != null)
                    promise.Resolve(popularSearch);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
        
        public static Promise<FetchSearchResponse> SearchArticle(string keyword, int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<FetchSearchResponse>();
            Window.instance.startCoroutine(_SearchArticle(promise, keyword, pageNumber));
            return promise;
        }

        private static IEnumerator
            _SearchArticle(Promise<FetchSearchResponse> promise, string keyword, int pageNumber) {
            var request = HttpManager.GET(Config.apiAddress +
                                              $"/api/search?t=project&projectType=article&k=[\"q:{keyword}\"]&searchAllLoadMore=false&page={pageNumber}");
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
                var searchResponse = JsonConvert.DeserializeObject<FetchSearchResponse>(responseText);
                if (searchResponse != null)
                    promise.Resolve(searchResponse);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}