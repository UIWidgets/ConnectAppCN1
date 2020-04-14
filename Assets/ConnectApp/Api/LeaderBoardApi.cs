using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.screens;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class LeaderBoardApi {
        public static Promise<FetchLeaderBoardCollectionResponse> FetchLeaderBoardCollection(int page) {
            var promise = new Promise<FetchLeaderBoardCollectionResponse>();
            var para = new Dictionary<string, object> {
                {"page", page}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/rankList/collection", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var collectionResponse =
                    JsonConvert.DeserializeObject<FetchLeaderBoardCollectionResponse>(value: responseText);
                promise.Resolve(value: collectionResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchLeaderBoardColumnResponse> FetchLeaderBoardColumn(int page) {
            var promise = new Promise<FetchLeaderBoardColumnResponse>();
            var para = new Dictionary<string, object> {
                {"page", page}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/rankList/column", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var columnResponse = JsonConvert.DeserializeObject<FetchLeaderBoardColumnResponse>(value: responseText);
                promise.Resolve(value: columnResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchBloggerResponse> FetchLeaderBoardBlogger(int page) {
            var promise = new Promise<FetchBloggerResponse>();
            var para = new Dictionary<string, object> {
                {"page", page}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/rankList/blogger", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var bloggerResponse = JsonConvert.DeserializeObject<FetchBloggerResponse>(value: responseText);
                promise.Resolve(value: bloggerResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchBloggerResponse> FetchHomeBlogger(int page) {
            var promise = new Promise<FetchBloggerResponse>();
            var para = new Dictionary<string, object> {
                {"page", page}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/rankList/homeBlogger", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var homeBloggerResponse = JsonConvert.DeserializeObject<FetchBloggerResponse>(value: responseText);
                promise.Resolve(value: homeBloggerResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static IPromise<FetchHomeEventsResponse> FetchHomeEvents(int page) {
            var promise = new Promise<FetchHomeEventsResponse>();
            var para = new Dictionary<string, object> {
                {"page", page}
            };
            var request = HttpManager.GET($"{Config.apiAddress_com}{Config.apiPath}/rankList/homeEvent", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var eventsResponse = JsonConvert.DeserializeObject<FetchHomeEventsResponse>(value: responseText);
                promise.Resolve(value: eventsResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchLeaderBoardDetailResponse> FetchLeaderBoardDetail(
            string tagId, int page, LeaderBoardType leaderBoardType) {
            var promise = new Promise<FetchLeaderBoardDetailResponse>();
            var para = new Dictionary<string, object> {
                {"page", page}
            };
            var request = HttpManager.GET(
                $"{Config.apiAddress_cn}{Config.apiPath}/rankList/{leaderBoardType.ToString()}/{tagId}",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var detailResponse = JsonConvert.DeserializeObject<FetchLeaderBoardDetailResponse>(value: responseText);
                promise.Resolve(value: detailResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}