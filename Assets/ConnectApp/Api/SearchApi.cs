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
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/search/popularSearch",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var popularSearch = JsonConvert.DeserializeObject<List<PopularSearch>>(value: responseText);
                promise.Resolve(value: popularSearch);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<List<PopularSearch>> PopularSearchUser() {
            var promise = new Promise<List<PopularSearch>>();
            var para = new Dictionary<string, object> {
                {"searchType", "user"}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/search/popularSearch",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var popularSearch = JsonConvert.DeserializeObject<List<PopularSearch>>(value: responseText);
                promise.Resolve(value: popularSearch);
            }).Catch(exception => promise.Reject(ex: exception));
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
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/search", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var searchResponse = JsonConvert.DeserializeObject<FetchSearchArticleResponse>(value: responseText);
                promise.Resolve(value: searchResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
        
        public static Promise<FetchSearchUserResponse> SearchUser(string keyword, int pageNumber) {
            var promise = new Promise<FetchSearchUserResponse>();
            var para = new Dictionary<string, object> {
                {"q", keyword},
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/search/users", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var searchUserResponse = JsonConvert.DeserializeObject<FetchSearchUserResponse>(value: responseText);
                promise.Resolve(value: searchUserResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchSearchTeamResponse> SearchTeam(string keyword, int pageNumber) {
            var promise = new Promise<FetchSearchTeamResponse>();
            var para = new Dictionary<string, object> {
                {"q", keyword},
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/search/teams", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var searchTeamResponse = JsonConvert.DeserializeObject<FetchSearchTeamResponse>(value: responseText);
                promise.Resolve(value: searchTeamResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}