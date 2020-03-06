using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Api {
    public static class FavoriteApi {
        public static Promise<FetchFavoriteTagsResponse> FetchMyFavoriteTags(string userId, int offset) {
            var promise = new Promise<FetchFavoriteTagsResponse>();
            var para = new Dictionary<string, object> {
                {"offset", offset},
                {"list", "my"}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/favorite-tag/{userId}/list",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var favoritesResponse = JsonConvert.DeserializeObject<FetchFavoriteTagsResponse>(value: responseText);
                promise.Resolve(value: favoritesResponse);
            }).Catch(exception => { promise.Reject(ex: exception); });
            return promise;
        }

        public static Promise<FetchFavoriteTagsResponse> FetchFollowFavoriteTags(string userId, int offset) {
            var promise = new Promise<FetchFavoriteTagsResponse>();
            var para = new Dictionary<string, object> {
                {"offset", offset},
                {"list", "other"}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/favorite-tag/{userId}/list",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var favoritesResponse = JsonConvert.DeserializeObject<FetchFavoriteTagsResponse>(value: responseText);
                promise.Resolve(value: favoritesResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchFavoriteDetailResponse>
            FetchFavoriteDetail(string userId, string tagId, int offset) {
            var promise = new Promise<FetchFavoriteDetailResponse>();
            var para = new Dictionary<string, object> {
                {"tagId", tagId},
                {"offset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/favorite/{userId}/list",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var favoriteDetailResponse =
                    JsonConvert.DeserializeObject<FetchFavoriteDetailResponse>(value: responseText);
                promise.Resolve(value: favoriteDetailResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FavoriteTag>
            CreateFavoriteTag(IconStyle iconStyle, string name, string description = "") {
            var promise = new Promise<FavoriteTag>();
            var para = new Dictionary<string, object> {
                {"name", name},
                {"description", description},
                {"iconStyle", iconStyle}
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/favorite-tag", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var createFavoriteTagResponse = JsonConvert.DeserializeObject<FavoriteTag>(value: responseText);
                promise.Resolve(value: createFavoriteTagResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FavoriteTag> EditFavoriteTag(string tagId, IconStyle iconStyle, string name,
            string description = "") {
            var promise = new Promise<FavoriteTag>();
            var para = new Dictionary<string, object> {
                {"id", tagId},
                {"name", name},
                {"description", description},
                {"iconStyle", iconStyle}
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/favorite-tag/edit",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var editFavoriteTagResponse = JsonConvert.DeserializeObject<FavoriteTag>(value: responseText);
                promise.Resolve(value: editFavoriteTagResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FavoriteTag> DeleteFavoriteTag(string tagId = "", string quoteTagId = "") {
            var promise = new Promise<FavoriteTag>();
            var para = new Dictionary<string, object>();
            if (quoteTagId.isNotEmpty()) {
                para.Add("quoteTagId", quoteTagId);
            }
            else {
                para.Add("id", tagId);
            }

            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/favorite-tag/delete",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var deleteFavoriteTagResponse = JsonConvert.DeserializeObject<FavoriteTag>(value: responseText);
                promise.Resolve(value: deleteFavoriteTagResponse);
            }).Catch(exception => { promise.Reject(ex: exception); });
            return promise;
        }

        public static Promise<CollectFavoriteTagResponse> CollectFavoriteTag(string tagId) {
            var promise = new Promise<CollectFavoriteTagResponse>();
            var para = new Dictionary<string, object> {
                {"tagId", tagId}
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/favorite-tag/collect",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var collectFavoriteTagResponse =
                    JsonConvert.DeserializeObject<CollectFavoriteTagResponse>(value: responseText);
                promise.Resolve(value: collectFavoriteTagResponse);
            }).Catch(exception => { promise.Reject(ex: exception); });
            return promise;
        }
    }
}