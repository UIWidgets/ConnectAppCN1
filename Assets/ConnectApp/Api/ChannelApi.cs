using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.Api {
    public static class ChannelApi {
        public static Promise<FetchChannelsResponse> FetchChannels(int page) {
            var promise = new Promise<FetchChannelsResponse>();
            var para = new Dictionary<string, object> {
                {"discover", "true"},
                {"discoverPage", page},
                {"joined", "true"},
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/channels", parameter: para);
            HttpManager.resume(request).Then(responseText => {
                var publicChannelsResponse = JsonConvert.DeserializeObject<FetchChannelsResponse>(responseText);
                promise.Resolve(publicChannelsResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
        
        public static Promise<FetchChannelMessagesResponse> FetchChannelMessages(
            string channelId, string before = null, string after = null) {
            D.assert(before == null || after == null);
            var promise = new Promise<FetchChannelMessagesResponse>();
            var request = HttpManager.GET($"{Config.apiAddress}/api/channels/{channelId}/messages",
                parameter: before != null
                ? new Dictionary<string, object> {{"before", before}}
                : after != null
                    ? new Dictionary<string, object> {{"after", after}}
                    : null);
            HttpManager.resume(request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<FetchChannelMessagesResponse>(responseText));
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
        
        public static Promise<AckChannelMessagesResponse> AckChannelMessage(string messageId) {
            var promise = new Promise<AckChannelMessagesResponse>();
            var request = HttpManager.POST($"{Config.apiAddress}/api/messages/{messageId}/ack");
            HttpManager.resume(request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<AckChannelMessagesResponse>(responseText));
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<FetchChannelMembersResponse> FetchChannelMembers(string channelId, int offset = 0) {
            var promise = new Promise<FetchChannelMembersResponse>();
            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/channels/{channelId}/members",
                parameter: new Dictionary<string, object> {
                    {"offset", offset}
                });
            HttpManager.resume(request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<FetchChannelMembersResponse>(responseText));
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
        
        public static Promise<List<JoinChannelResponse>> JoinChannel(string channelId, string groupId = null) {
            var promise = new Promise<List<JoinChannelResponse>>();
            var request = HttpManager.POST($"{Config.apiAddress}/api/channels/{channelId}/join",
                parameter: new Dictionary<string, string> {
                    {"channelId", channelId}
            });
            HttpManager.resume(request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<List<JoinChannelResponse>>(responseText));
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
        
        public static Promise<LeaveChannelResponse> LeaveChannel(string channelId, string groupId = null) {
            var promise = new Promise<LeaveChannelResponse>();
            var request = HttpManager.POST($"{Config.apiAddress}/api/channels/{channelId}/leave",
                parameter: new Dictionary<string, string> {
                    {"channelId", channelId}
            });
            HttpManager.resume(request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<LeaveChannelResponse>(responseText));
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
        
        public static Promise<FetchSendMessageResponse> SendImage(string channelId, string content, string nonce,
            string imageData, string parentMessageId = "") {
            var data = Convert.FromBase64String(imageData);
            var promise = new Promise<FetchSendMessageResponse>();
            var request = HttpManager.POST(
                $"{Config.apiAddress}/api/channels/{channelId}/messages/attachments",
                parameter: new List<List<object>> {
                    new List<object>{"channel", channelId},
                    new List<object>{"content", content},
                    new List<object>{"parentMessageId", parentMessageId},
                    new List<object>{"nonce", nonce},
                    new List<object>{"size", $"{data.Length}"},
                    new List<object>{"file", data}
                },
                multipart: true, 
                filename: "image.png", 
                fileType: "image/png");
            HttpManager.resume(request).Then(responseText => {
                promise.Resolve(new FetchSendMessageResponse {
                    channelId = channelId,
                    content = content,
                    nonce = nonce
                });
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}