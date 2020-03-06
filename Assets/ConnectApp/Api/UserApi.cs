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
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/u/{userId}");
            HttpManager.resume(request: request).Then(responseText => {
                var userProfileResponse = JsonConvert.DeserializeObject<FetchUserProfileResponse>(value: responseText);
                promise.Resolve(value: userProfileResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchUserArticleResponse> FetchUserArticle(string userId, int pageNumber) {
            var promise = new Promise<FetchUserArticleResponse>();
            var para = new Dictionary<string, object> {
                {"page", pageNumber},
                {"type", "article"}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/u/{userId}/activities",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var userArticleResponse = JsonConvert.DeserializeObject<FetchUserArticleResponse>(value: responseText);
                promise.Resolve(value: userArticleResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchUserLikeArticleResponse> FetchUserLikeArticle(string userId, int pageNumber) {
            var promise = new Promise<FetchUserLikeArticleResponse>();
            var para = new Dictionary<string, object> {
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/u/{userId}/likes", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var userArticleResponse = JsonConvert.DeserializeObject<FetchUserLikeArticleResponse>(value: responseText);
                promise.Resolve(value: userArticleResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<bool> FetchFollowUser(string userId) {
            var promise = new Promise<bool>();
            var para = new FollowParameter {
                type = "user",
                followeeId = userId
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/follow", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var followResponse = JsonConvert.DeserializeObject<Dictionary<string, bool>>(value: responseText);
                promise.Resolve(followResponse["success"]);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<bool> FetchUnFollowUser(string userId) {
            var promise = new Promise<bool>();
            var para = new FollowParameter {
                followeeId = userId
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/unfollow", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var unFollowResponse = JsonConvert.DeserializeObject<Dictionary<string, bool>>(value: responseText);
                promise.Resolve(unFollowResponse["success"]);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchFollowingUserResponse> FetchFollowingUser(string userId, int offset) {
            var promise = new Promise<FetchFollowingUserResponse>();
            var para = new Dictionary<string, object> {
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/u/{userId}/followingUsers",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var followingUserResponse =
                    JsonConvert.DeserializeObject<FetchFollowingUserResponse>(value: responseText);
                promise.Resolve(value: followingUserResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchFollowerResponse> FetchFollower(string userId, int offset) {
            var promise = new Promise<FetchFollowerResponse>();
            var para = new Dictionary<string, object> {
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/u/{userId}/followers",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var followerResponse = JsonConvert.DeserializeObject<FetchFollowerResponse>(value: responseText);
                promise.Resolve(value: followerResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchFollowingTeamResponse> FetchFollowingTeam(string userId, int offset) {
            var promise = new Promise<FetchFollowingTeamResponse>();
            var para = new Dictionary<string, object> {
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/u/{userId}/followingTeams",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var followingTeamResponse =
                    JsonConvert.DeserializeObject<FetchFollowingTeamResponse>(value: responseText);
                promise.Resolve(value: followingTeamResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchFollowingResponse> FetchFollowing(string userId, int offset) {
            var promise = new Promise<FetchFollowingResponse>();
            var para = new Dictionary<string, object> {
                {"needTeam", "true"},
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/u/{userId}/followings",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var followingResponse = JsonConvert.DeserializeObject<FetchFollowingResponse>(value: responseText);
                promise.Resolve(value: followingResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchEditPersonalInfoResponse> EditPersonalInfo(string userId, string fullName,
            string title,
            string jobRoleId, string placeId) {
            var promise = new Promise<FetchEditPersonalInfoResponse>();
            var para = new EditPersonalParameter {
                fullName = fullName,
                title = title,
                jobRoleId = jobRoleId,
                placeId = placeId
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/u/{userId}/updateUserBasicInfo",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var editPersonalInfoResponse =
                    JsonConvert.DeserializeObject<FetchEditPersonalInfoResponse>(value: responseText);
                promise.Resolve(value: editPersonalInfoResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<UpdateAvatarResponse> UpdateAvatar(string userId, string avatar) {
            var promise = new Promise<UpdateAvatarResponse>();
            var para = new UpdateAvatarParameter {
                avatar = $"data:image/jpeg;base64,{avatar}"
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/u/{userId}/updateUserAvatar",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var updateAvatarResponse =
                    JsonConvert.DeserializeObject<UpdateAvatarResponse>(value: responseText);
                promise.Resolve(value: updateAvatarResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise RegisterToken(string token, string userId = "") {
            var promise = new Promise();
            var para = new RegisterTokenParameter {
                token = token,
                userId = userId
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/registerToken",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}