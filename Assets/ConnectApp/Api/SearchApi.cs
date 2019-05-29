using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.api {
    public static class SearchApi {
        public static Promise<List<PopularSearch>> PopularSearch() {
            var promise = new Promise<List<PopularSearch>>();
            var request = HttpManager.GET(Config.apiAddress + "/api/search/popularSearch?searchType=project");
            HttpManager.resume(request).Then(responseText => {
                var popularSearch = JsonConvert.DeserializeObject<List<PopularSearch>>(responseText);
                promise.Resolve(popularSearch);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<FetchSearchResponse> SearchArticle(string keyword, int pageNumber) {
            var promise = new Promise<FetchSearchResponse>();
            var request = HttpManager.GET(Config.apiAddress +
                                          $"/api/search?t=project&projectType=article&k=[\"q:{keyword}\"]&searchAllLoadMore=false&page={pageNumber}");
            HttpManager.resume(request).Then(responseText => {
                var searchResponse = JsonConvert.DeserializeObject<FetchSearchResponse>(responseText);
                promise.Resolve(searchResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}