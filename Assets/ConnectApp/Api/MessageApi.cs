using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Api {
    public static class MessageApi {
        public static Promise<FetchCommentsResponse> FetchMessages(string channelId, string currOldestMessageId) {
            var promise = new Promise<FetchCommentsResponse>();
            var url = $"/api/channels/{channelId}/messages";
            if (currOldestMessageId.isNotEmpty()) {
                url += "?before=" + currOldestMessageId;
            }

            var request = HttpManager.GET(url);
            HttpManager.resume(request).Then(responseText => {
                var messagesResponse = JsonConvert.DeserializeObject<FetchCommentsResponse>(responseText);
                promise.Resolve(messagesResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<FetchSendMessageResponse> SendMessage(string channelId, string content, string nonce,
            string parentMessageId = "") {
            var promise = new Promise<FetchSendMessageResponse>();
            var para = new SendCommentParameter {
                content = content,
                parentMessageId = parentMessageId,
                nonce = nonce
            };
            var request = HttpManager.POST($"/api/channels/{channelId}/messages", para);
            HttpManager.resume(request).Then(responseText => {
                var sendMessageResponse = new FetchSendMessageResponse {
                    channelId = channelId,
                    content = content,
                    nonce = nonce
                };
                promise.Resolve(sendMessageResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}