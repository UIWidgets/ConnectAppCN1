using System;
using System.Collections;
using ConnectApp.constants;
using ConnectApp.models;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace ConnectApp.api {
    public static class SearchApi {
        public static Promise<FetchSearchResponse> SearchArticle(string keyword, int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise<FetchSearchResponse>();
            Window.instance.startCoroutine(_SearchArticle(promise, keyword, pageNumber));
            return promise;
        }

        private static IEnumerator _SearchArticle(Promise<FetchSearchResponse> promise, string keyword, int pageNumber) {
            var request = UnityWebRequest.Get(IApi.apiAddress +
                                              $"/api/search?t=project&projectType=article&k=[%22q:{keyword}%22]&searchAllLoadMore=false&page={pageNumber}");
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
                var searchResponse = JsonConvert.DeserializeObject<FetchSearchResponse>(responseText);
                if (searchResponse != null)
                    promise.Resolve(searchResponse);
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}