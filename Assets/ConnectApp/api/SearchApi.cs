using System;
using System.Collections;
using ConnectApp.constants;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public class SearchApi {
        public static Promise SearchArticle(string keyword, int pageNumber) {
            // We return a promise instantly and start the coroutine to do the real work
            var promise = new Promise();
            Window.instance.startCoroutine(_SearchArticle(promise, keyword, pageNumber));
            return promise;
        }

        private static IEnumerator _SearchArticle(Promise promise, string keyword, int pageNumber) {
            var request = UnityWebRequest.Get(IApi.apiAddress +
                                              $"/api/search?t=project&projectType=article&k=[%22q:{keyword}%22]&searchAllLoadMore=false&page={pageNumber}");
            request.SetRequestHeader("X-Requested-With", "XmlHttpRequest");
#pragma warning disable 618
            yield return request.Send();
#pragma warning restore 618

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
                Debug.Log(json);
//                var events = JsonConvert.DeserializeObject<List<IEvent>>(json);
                if (json != null)
                    promise.Resolve();
                else
                    promise.Reject(new Exception("No user under this username found!"));
            }
        }
    }
}