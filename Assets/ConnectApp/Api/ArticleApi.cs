using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

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

        public static Promise<FetchArticleDetailResponse> FetchArticleDetail(string articleId) {
            var promise = new Promise<FetchArticleDetailResponse>();
            var para = new Dictionary<string, object> {
                {"view", "true"}
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/p/{articleId}", parameter: para);
            HttpManager.resume(request).Then(responseText => {
                var articleDetailResponse = JsonConvert.DeserializeObject<FetchArticleDetailResponse>(responseText);
                promise.Resolve(articleDetailResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<FetchCommentsResponse>
            FetchArticleComments(string channelId, string currOldestMessageId) {
            var promise = new Promise<FetchCommentsResponse>();
            var para = new Dictionary<string, object> {
                {"limit", 5}
            };
            if (currOldestMessageId.Length > 0) {
                para.Add("before", currOldestMessageId);
            }

            var request = HttpManager.GET($"{Config.apiAddress}/api/channels/{channelId}/messages", parameter: para);
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
            var request = HttpManager.POST( $"{Config.apiAddress}/api/like", parameter: para);
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
                nonce = nonce
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