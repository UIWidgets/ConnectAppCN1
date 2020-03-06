using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Api {
    public static class ArticleApi {
        public static Promise<FetchArticlesResponse> FetchArticles(string userId, int offset) {
            var promise = new Promise<FetchArticlesResponse>();
            var para = new Dictionary<string, object> {
                {"language", "zh_CN"},
                {"hottestOffset", offset},
                {"needRank", true},
                {"afterTime", HistoryManager.homeAfterTime(userId: userId)}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/recommends", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var articlesResponse = JsonConvert.DeserializeObject<FetchArticlesResponse>(value: responseText);
                promise.Resolve(value: articlesResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchFollowArticlesResponse> FetchFollowArticles(int pageNumber, string beforeTime,
            string afterTime, bool isFirst, bool isHot) {
            var promise = new Promise<FetchFollowArticlesResponse>();
            Dictionary<string, object> para;
            if (isFirst) {
                para = null;
            }
            else {
                if (isHot) {
                    para = new Dictionary<string, object> {
                        {"hotPage", pageNumber}
                    };
                }
                else {
                    para = new Dictionary<string, object> {
                        {"beforeTime", beforeTime},
                        {"afterTime", afterTime}
                    };
                }
            }

            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/feeds", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var followArticlesResponse =
                    JsonConvert.DeserializeObject<FetchFollowArticlesResponse>(value: responseText);
                promise.Resolve(value: followArticlesResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchArticleDetailResponse> FetchArticleDetail(string articleId, bool isPush = false) {
            var promise = new Promise<FetchArticleDetailResponse>();
            var para = new Dictionary<string, object> {
                {"view", "true"}
            };
            if (isPush) {
                para.Add("isPush", "true");
            }

            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/p/{articleId}", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var articleDetailResponse =
                    JsonConvert.DeserializeObject<FetchArticleDetailResponse>(value: responseText);
                promise.Resolve(value: articleDetailResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchCommentsResponse>
            FetchArticleComments(string channelId, string currOldestMessageId) {
            var promise = new Promise<FetchCommentsResponse>();
            var para = new Dictionary<string, object> {
                {"limit", 5}
            };
            if (currOldestMessageId != null && currOldestMessageId.isNotEmpty()) {
                para.Add("before", value: currOldestMessageId);
            }

            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/messages",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var responseComments = JsonConvert.DeserializeObject<FetchCommentsResponse>(value: responseText);
                promise.Resolve(value: responseComments);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise LikeArticle(string articleId, int addCount) {
            var promise = new Promise();
            var para = new HandleArticleParameter {
                type = "project",
                itemId = articleId
            };
            var request =
                HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/like?isAppRepeatLike=true&addCount={addCount}",
                    parameter: para);
            HttpManager.resume(request: request).Then(responseText => promise.Resolve())
                .Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<List<Favorite>> FavoriteArticle(string articleId, List<string> tagIds) {
            var promise = new Promise<List<Favorite>>();
            var para = new HandleArticleParameter {
                type = "article",
                itemId = articleId,
                tagIds = tagIds
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/favorites", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var favoriteArticleResponse = JsonConvert.DeserializeObject<List<Favorite>>(value: responseText);
                promise.Resolve(value: favoriteArticleResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<Favorite> UnFavoriteArticle(string favoriteId) {
            var promise = new Promise<Favorite>();
            var para = new Dictionary<string, object> {
                {"id", favoriteId}
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/unfavorite", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var unFavoriteArticleResponse = JsonConvert.DeserializeObject<Favorite>(value: responseText);
                promise.Resolve(value: unFavoriteArticleResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<Message> LikeComment(string commentId) {
            var promise = new Promise<Message>();
            var para = new ReactionParameter {
                reactionType = "like"
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/messages/{commentId}/addReaction",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var message = JsonConvert.DeserializeObject<Message>(value: responseText);
                promise.Resolve(value: message);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<Message> RemoveLikeComment(string commentId) {
            var promise = new Promise<Message>();
            var para = new ReactionParameter {
                reactionType = "like"
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/messages/{commentId}/removeReaction",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var message = JsonConvert.DeserializeObject<Message>(value: responseText);
                promise.Resolve(value: message);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }


        public static Promise<Message> SendComment(string channelId, string content, string nonce,
            string parentMessageId = "", string upperMessageId = "") {
            var promise = new Promise<Message>();
            var para = new SendCommentParameter {
                content = content,
                parentMessageId = parentMessageId,
                upperMessageId = upperMessageId,
                nonce = nonce,
                app = true
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/messages",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var message = JsonConvert.DeserializeObject<Message>(value: responseText);
                promise.Resolve(value: message);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}