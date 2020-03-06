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
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/teams/{teamId}");
            HttpManager.resume(request: request).Then(responseText => {
                var teamResponse = JsonConvert.DeserializeObject<FetchTeamResponse>(value: responseText);
                promise.Resolve(value: teamResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchTeamArticleResponse> FetchTeamArticle(string teamId, int pageNumber) {
            var promise = new Promise<FetchTeamArticleResponse>();
            var para = new Dictionary<string, object> {
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/teams/{teamId}/projects",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var teamArticleResponse = JsonConvert.DeserializeObject<FetchTeamArticleResponse>(value: responseText);
                promise.Resolve(value: teamArticleResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchFollowerResponse> FetchTeamFollower(string teamId, int offset) {
            var promise = new Promise<FetchFollowerResponse>();
            var para = new Dictionary<string, object> {
                {"teamId", teamId},
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/teams/{teamId}/followers",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var followerResponse = JsonConvert.DeserializeObject<FetchFollowerResponse>(value: responseText);
                promise.Resolve(value: followerResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchTeamMemberResponse> FetchTeamMember(string teamId, int pageNumber) {
            var promise = new Promise<FetchTeamMemberResponse>();
            var para = new Dictionary<string, object> {
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/teams/{teamId}/members",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var teamMemberResponse = JsonConvert.DeserializeObject<FetchTeamMemberResponse>(value: responseText);
                promise.Resolve(value: teamMemberResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<bool> FetchFollowTeam(string teamId) {
            var promise = new Promise<bool>();
            var para = new FollowParameter {
                type = "team",
                followeeId = teamId
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/follow", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var followResponse = JsonConvert.DeserializeObject<Dictionary<string, bool>>(value: responseText);
                promise.Resolve(followResponse["success"]);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<bool> FetchUnFollowTeam(string teamId) {
            var promise = new Promise<bool>();
            var para = new FollowParameter {
                followeeId = teamId
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/unfollow", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var unFollowResponse = JsonConvert.DeserializeObject<Dictionary<string, bool>>(value: responseText);
                promise.Resolve(unFollowResponse["success"]);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}