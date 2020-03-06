using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.Api {
    public static class MessageApi {
        
        public static Promise<FetchCommentsResponse> FetchMessages(string channelId, string currOldestMessageId) {
            var promise = new Promise<FetchCommentsResponse>();
            var para = new Dictionary<string, object> ();
            if (currOldestMessageId.isNotEmpty()) {
                para.Add("before", value: currOldestMessageId);
            }

            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/messages",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var messagesResponse = JsonConvert.DeserializeObject<FetchCommentsResponse>(value: responseText);
                promise.Resolve(value: messagesResponse);
            }).Catch(exception => promise.Reject(ex: exception));
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
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/messages",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var sendMessageResponse = new FetchSendMessageResponse {
                    channelId = channelId,
                    content = content,
                    nonce = nonce
                };
                promise.Resolve(value: sendMessageResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}