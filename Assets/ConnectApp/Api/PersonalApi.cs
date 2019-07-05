using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class PersonalApi {
        public static Promise<FetchPersonalResponse> FetchPersonal(string personalId) {
            var promise = new Promise<FetchPersonalResponse>();
            var request = HttpManager.GET($"{Config.apiAddress}/api/profile/{personalId}");
            HttpManager.resume(request: request).Then(responseText => {
                var personalResponse = JsonConvert.DeserializeObject<FetchPersonalResponse>(responseText);
                promise.Resolve(personalResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchPersonalArticleResponse> FetchPersonalArticle(string personalId, int offset) {
            var promise = new Promise<FetchPersonalArticleResponse>();
            var para = new Dictionary<string, object> {
                {"offset", offset},
                {"type", "article"}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/profile/{personalId}/getActivities", para);
            HttpManager.resume(request: request).Then(responseText => {
                var personalArticleResponse = JsonConvert.DeserializeObject<FetchPersonalArticleResponse>(responseText);
                promise.Resolve(personalArticleResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<bool> FetchFollowUser(string personalId) {
            var promise = new Promise<bool>();
            var para = new FollowParameter {
                type = "user",
                followeeId = personalId
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/follow", para);
            HttpManager.resume(request: request).Then(responseText => {
                var followResponse = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseText);
                promise.Resolve(followResponse["success"]);
                
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<bool> FetchUnFollowUser(string personalId) {
            var promise = new Promise<bool>();
            var para = new FollowParameter {
                followeeId = personalId
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/unfollow", para);
            request.SetRequestHeader("Content-Type", "application/json");
            HttpManager.resume(request: request).Then(responseText => {
                var unFollowResponse = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseText);
                promise.Resolve(unFollowResponse["success"]);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchFollowingResponse> FetchFollowing(string personalId, int offset) {
            var promise = new Promise<FetchFollowingResponse>();
            var para = new Dictionary<string, object> {
                {"userId", personalId},
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/u/getFollowings", para);
            HttpManager.resume(request: request).Then(responseText => {
                var followingResponse = JsonConvert.DeserializeObject<FetchFollowingResponse>(responseText);
                promise.Resolve(followingResponse);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static Promise<FetchFollowerResponse> FetchFollower(string personalId, int offset) {
            var promise = new Promise<FetchFollowerResponse>();
            var para = new Dictionary<string, object> {
                {"userId", personalId},
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/u/getFollowers", para);
            HttpManager.resume(request: request).Then(responseText => {
                var followerResponse = JsonConvert.DeserializeObject<FetchFollowerResponse>(responseText);
                promise.Resolve(followerResponse);
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