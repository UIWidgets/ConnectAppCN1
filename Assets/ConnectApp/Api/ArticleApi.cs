using System.Text;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.utils;
using Newtonsoft.Json;
using RSG;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public static class ArticleApi {
        public static Promise<FetchArticlesResponse> FetchArticles(int offset) {
            var promise = new Promise<FetchArticlesResponse>();
            var request = HttpManager.GET(Config.apiAddress +
                                          "/api/getFeedList?language=zh_CN&hottestHasMore=true&feedHasMore=false&isApp=true&hottestOffset=" +
                                          offset);
            HttpManager.resume(request).Then(responseText => {
                var articlesResponse = JsonConvert.DeserializeObject<FetchArticlesResponse>(responseText);
                promise.Resolve(articlesResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<FetchArticleDetailResponse> FetchArticleDetail(string articleId) {
            var promise = new Promise<FetchArticleDetailResponse>();
            var request = HttpManager.GET(Config.apiAddress + "/api/p/" + articleId + "?view=true");
            HttpManager.resume(request).Then(responseText => {
                var articleDetailResponse = JsonConvert.DeserializeObject<FetchArticleDetailResponse>(responseText);
                promise.Resolve(articleDetailResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<FetchCommentsResponse>
            FetchArticleComments(string channelId, string currOldestMessageId) {
            var promise = new Promise<FetchCommentsResponse>();
            var url = Config.apiAddress + "/api/channels/" + channelId + "/messages?limit=5";
            if (currOldestMessageId.Length > 0) {
                url += "&before=" + currOldestMessageId;
            }

            var request = HttpManager.GET(url);
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
            var body = JsonConvert.SerializeObject(para);
            var request = HttpManager.initRequest(Config.apiAddress + "/api/like", Method.POST);
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
            HttpManager.resume(request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<Message> LikeComment(string commentId) {
            var promise = new Promise<Message>();
            var para = new ReactionParameter {
                reactionType = "like"
            };
            var body = JsonConvert.SerializeObject(para);
            var request =
                HttpManager.initRequest(Config.apiAddress + "/api/messages/" + commentId + "/addReaction", Method.POST);
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
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
            var body = JsonConvert.SerializeObject(para);
            var request =
                HttpManager.initRequest(Config.apiAddress + "/api/messages/" + commentId + "/removeReaction",
                    Method.POST);
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
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
            var body = JsonConvert.SerializeObject(para);
            var request =
                HttpManager.initRequest(Config.apiAddress + "/api/channels/" + channelId + "/messages", Method.POST);
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
            HttpManager.resume(request).Then(responseText => {
                var message = JsonConvert.DeserializeObject<Message>(responseText);
                promise.Resolve(message);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}