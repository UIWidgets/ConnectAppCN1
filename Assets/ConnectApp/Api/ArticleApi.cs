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
        public static Promise<FetchArticlesResponse> FetchArticles(int offset) {
            var promise = new Promise<FetchArticlesResponse>();
            var para = new Dictionary<string, object> {
                {"language", "zh_CN"},
                {"hottestHasMore", "true"},
                {"feedHasMore", "false"},
                {"isApp", "true"},
                {"hottestOffset", offset}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/getFeedList", parameter: para);
            HttpManager.resume(request).Then(responseText => {
                var articlesResponse = JsonConvert.DeserializeObject<FetchArticlesResponse>(responseText);
                promise.Resolve(articlesResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<FetchFollowArticlesResponse> FetchFollowArticles(int pageNumber) {
            var promise = new Promise<FetchFollowArticlesResponse>();
            var para = new Dictionary<string, object> {
                {"page", pageNumber}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/followingUsersArticles", parameter: para);
            HttpManager.resume(request).Then(responseText => {
                var followArticlesResponse = JsonConvert.DeserializeObject<FetchFollowArticlesResponse>(responseText);
                promise.Resolve(followArticlesResponse);
            }).Catch(exception => { promise.Reject(exception); });
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

            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/p/{articleId}", parameter: para);
            HttpManager.resume(request).Then(responseText => {
                var articleDetailResponse = JsonConvert.DeserializeObject<FetchArticleDetailResponse>(responseText);
                promise.Resolve(articleDetailResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<FetchCommentsResponse> FetchArticleComments(string channelId, string currOldestMessageId) {
            var promise = new Promise<FetchCommentsResponse>();
            var para = new Dictionary<string, object> {
                {"limit", 5}
            };
            if (currOldestMessageId != null && currOldestMessageId.isNotEmpty()) {
                para.Add("before", value: currOldestMessageId);
            }

            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/channels/{channelId}/messages", parameter: para);
            HttpManager.resume(request).Then(responseText => {
                var responseComments = JsonConvert.DeserializeObject<FetchCommentsResponse>(responseText);
                promise.Resolve(responseComments);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise LikeArticle(string articleId) {
            var promise = new Promise();
            var para = new LikeArticleParameter {
                type = "project",
                itemId = articleId
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/like", parameter: para);
            HttpManager.resume(request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<Message> LikeComment(string commentId) {
            var promise = new Promise<Message>();
            var para = new ReactionParameter {
                reactionType = "like"
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/messages/{commentId}/addReaction", para);
            HttpManager.resume(request).Then(responseText => {
                var message = JsonConvert.DeserializeObject<Message>(responseText);
                promise.Resolve(message);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<Message> RemoveLikeComment(string commentId) {
            var promise = new Promise<Message>();
            var para = new ReactionParameter {
                reactionType = "like"
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/messages/{commentId}/removeReaction", para);
            HttpManager.resume(request).Then(responseText => {
                var message = JsonConvert.DeserializeObject<Message>(responseText);
                promise.Resolve(message);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }


        public static Promise<Message> SendComment(string channelId, string content, string nonce,
            string parentMessageId = "") {
            var promise = new Promise<Message>();
            var para = new SendCommentParameter {
                content = content,
                parentMessageId = parentMessageId,
                nonce = nonce,
                app = true
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/channels/{channelId}/messages", para);
            HttpManager.resume(request).Then(responseText => {
                var message = JsonConvert.DeserializeObject<Message>(responseText);
                promise.Resolve(message);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}