using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class UserApi {
        public static Promise<FetchUserProfileResponse> FetchUserProfile(string userId) {
            var promise = new Promise<FetchUserProfileResponse>();
            var request = HttpManager.GET($"{Config.apiAddress}/api/profile/{userId}");
            HttpManager.resume(request: request).Then(responseText => {
                var userProfileResponse = JsonConvert.DeserializeObject<FetchUserProfileResponse>(responseText);
                promise.Resolve(userProfileResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchUserArticleResponse> FetchUserArticle(string userId, int offset) {
            var promise = new Promise<FetchUserArticleResponse>();
            var para = new Dictionary<string, object> {
                {"offset", offset},
                {"type", "article"}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/profile/{userId}/getActivities", para);
            HttpManager.resume(request: request).Then(responseText => {
                var userArticleResponse = JsonConvert.DeserializeObject<FetchUserArticleResponse>(responseText);
                promise.Resolve(userArticleResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<bool> FetchFollowUser(string userId) {
            var promise = new Promise<bool>();
            var para = new FollowParameter {
                type = "user",
                followeeId = userId
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/follow", para);
            HttpManager.resume(request: request).Then(responseText => {
                var followResponse = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseText);
                promise.Resolve(followResponse["success"]);
                
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<bool> FetchUnFollowUser(string userId) {
            var promise = new Promise<bool>();
            var para = new FollowParameter {
                followeeId = userId
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/unfollow", para);
            HttpManager.resume(request: request).Then(responseText => {
                var unFollowResponse = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseText);
                promise.Resolve(unFollowResponse["success"]);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchFollowingUserResponse> FetchFollowingUser(string userId, int offset) {
            var promise = new Promise<FetchFollowingUserResponse>();
            var para = new Dictionary<string, object> {
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/u/{userId}/followingUsers", para);
            HttpManager.resume(request: request).Then(responseText => {
                var followingUserResponse = JsonConvert.DeserializeObject<FetchFollowingUserResponse>(responseText);
                promise.Resolve(followingUserResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchFollowerResponse> FetchFollower(string userId, int offset) {
            var promise = new Promise<FetchFollowerResponse>();
            var para = new Dictionary<string, object> {
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/u/{userId}/followers", para);
            HttpManager.resume(request: request).Then(responseText => {
                var followerResponse = JsonConvert.DeserializeObject<FetchFollowerResponse>(responseText);
                promise.Resolve(followerResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchFollowingTeamResponse> FetchFollowingTeam(string userId, int offset) {
            var promise = new Promise<FetchFollowingTeamResponse>();
            var para = new Dictionary<string, object> {
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/u/{userId}/followingTeams", para);
            HttpManager.resume(request: request).Then(responseText => {
                var followingTeamResponse = JsonConvert.DeserializeObject<FetchFollowingTeamResponse>(responseText);
                promise.Resolve(followingTeamResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchFollowingResponse> FetchFollowing(string userId, int offset) {
            var promise = new Promise<FetchFollowingResponse>();
            var para = new Dictionary<string, object> {
                {"needTeam", "true"},
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/u/{userId}/followings", para);
            HttpManager.resume(request: request).Then(responseText => {
                var followingResponse = JsonConvert.DeserializeObject<FetchFollowingResponse>(responseText);
                promise.Resolve(followingResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchEditPersonalInfoResponse> EditPersonalInfo(string fullName, string title, string jobRoleId, string placeId) {
            var promise = new Promise<FetchEditPersonalInfoResponse>();
            var para = new EditPersonalParameter {
                fullName = fullName,
                title = title,
                jobRoleId = jobRoleId,
                placeId = placeId
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/updateUserBasicInfo", para);
            HttpManager.resume(request: request).Then(responseText => {
                var editPersonalInfoResponse = JsonConvert.DeserializeObject<FetchEditPersonalInfoResponse>(responseText);
                promise.Resolve(editPersonalInfoResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }
    }
}