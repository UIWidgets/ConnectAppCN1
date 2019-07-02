using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class SearchApi {
        public static Promise<List<PopularSearch>> PopularSearch() {
            var promise = new Promise<List<PopularSearch>>();
            var para = new Dictionary<string, object> {
                {"searchType", "project"}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/search/popularSearch", para);
            HttpManager.resume(request).Then(responseText => {
                var popularSearch = JsonConvert.DeserializeObject<List<PopularSearch>>(responseText);
                promise.Resolve(popularSearch);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<FetchSearchResponse> SearchArticle(string keyword, int pageNumber) {
            var promise = new Promise<FetchSearchResponse>();
            var para = new Dictionary<string, object> {
                {"t", "project"},
                {"projectType", "article"},
                {"k", $"[\"q:{keyword}\"]"},
                {"searchAllLoadMore", "false"},
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/search", para);
            HttpManager.resume(request).Then(responseText => {
                var searchResponse = JsonConvert.DeserializeObject<FetchSearchResponse>(responseText);
                promise.Resolve(searchResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}