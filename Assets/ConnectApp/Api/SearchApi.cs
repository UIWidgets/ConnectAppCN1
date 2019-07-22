using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class SearchApi {
        public static Promise<List<PopularSearch>> PopularSearchArticle() {
            var promise = new Promise<List<PopularSearch>>();
            var para = new Dictionary<string, object> {
                {"searchType", "project"}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/search/popularSearch", para);
            HttpManager.resume(request).Then(responseText => {
                var popularSearch = JsonConvert.DeserializeObject<List<PopularSearch>>(responseText);
                promise.Resolve(popularSearch);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<List<PopularSearch>> PopularSearchUser() {
            var promise = new Promise<List<PopularSearch>>();
            var para = new Dictionary<string, object> {
                {"searchType", "user"}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/search/popularSearch", para);
            HttpManager.resume(request).Then(responseText => {
                var popularSearch = JsonConvert.DeserializeObject<List<PopularSearch>>(responseText);
                promise.Resolve(popularSearch);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchSearchArticleResponse> SearchArticle(string keyword, int pageNumber) {
            var promise = new Promise<FetchSearchArticleResponse>();
            var para = new Dictionary<string, object> {
                {"t", "project"},
                {"projectType", "article"},
                {"k", $"[\"q:{keyword}\"]"},
                {"searchAllLoadMore", "false"},
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/search", para);
            HttpManager.resume(request).Then(responseText => {
                var searchResponse = JsonConvert.DeserializeObject<FetchSearchArticleResponse>(responseText);
                promise.Resolve(searchResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }
        
        public static Promise<FetchSearchUserResponse> SearchUser(string keyword, int pageNumber) {
            var promise = new Promise<FetchSearchUserResponse>();
            var para = new Dictionary<string, object> {
                {"q", keyword},
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/search/users", para);
            HttpManager.resume(request).Then(responseText => {
                var searchUserResponse = JsonConvert.DeserializeObject<FetchSearchUserResponse>(responseText);
                promise.Resolve(searchUserResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchSearchTeamResponse> SearchTeam(string keyword, int pageNumber) {
            var promise = new Promise<FetchSearchTeamResponse>();
            var para = new Dictionary<string, object> {
                {"q", keyword},
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/search/teams", para);
            HttpManager.resume(request).Then(responseText => {
                var searchTeamResponse = JsonConvert.DeserializeObject<FetchSearchTeamResponse>(responseText);
                promise.Resolve(searchTeamResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }
    }
}