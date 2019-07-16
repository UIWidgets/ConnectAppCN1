using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class TeamApi {
        public static Promise<FetchTeamResponse> FetchTeam(string teamId) {
            var promise = new Promise<FetchTeamResponse>();
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/teams/{teamId}");
            HttpManager.resume(request: request).Then(responseText => {
                var teamResponse = JsonConvert.DeserializeObject<FetchTeamResponse>(responseText);
                promise.Resolve(teamResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchTeamArticleResponse> FetchTeamArticle(string teamId, int offset) {
            var promise = new Promise<FetchTeamArticleResponse>();
            var para = new Dictionary<string, object> {
                {"offset", offset},
                {"teamId", teamId}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/u/getProjects", para);
            HttpManager.resume(request: request).Then(responseText => {
                var teamArticleResponse = JsonConvert.DeserializeObject<FetchTeamArticleResponse>(responseText);
                promise.Resolve(teamArticleResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchFollowerResponse> FetchTeamFollower(string teamId, int offset) {
            var promise = new Promise<FetchFollowerResponse>();
            var para = new Dictionary<string, object> {
                {"teamId", teamId},
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/t/{teamId}/getFollowers", para);
            HttpManager.resume(request: request).Then(responseText => {
                var followerResponse = JsonConvert.DeserializeObject<FetchFollowerResponse>(responseText);
                promise.Resolve(followerResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<bool> FetchFollowTeam(string teamId) {
            var promise = new Promise<bool>();
            var para = new FollowParameter {
                type = "team",
                followeeId = teamId
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/follow", para);
            HttpManager.resume(request: request).Then(responseText => {
                var followResponse = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseText);
                promise.Resolve(followResponse["success"]);
                
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<bool> FetchUnFollowTeam(string teamId) {
            var promise = new Promise<bool>();
            var para = new FollowParameter {
                followeeId = teamId
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/unfollow", para);
            HttpManager.resume(request: request).Then(responseText => {
                var unFollowResponse = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseText);
                promise.Resolve(unFollowResponse["success"]);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }
    }
}