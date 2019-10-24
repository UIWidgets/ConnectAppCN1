using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.foundation;
using UnityEngine.Networking;

namespace ConnectApp.Api {
    public static class ChannelApi {
        public static Promise<FetchChannelsResponse> FetchChannels(int page) {
            var promise = new Promise<FetchChannelsResponse>();
            var para = new Dictionary<string, object> {
                {"discover", "true"},
                {"discoverPage", page},
                {"joined", "true"}
            };
            var request = HttpManager.GET($"{Config.apiAddress}{Config.apiPath}/channels", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var publicChannelsResponse = JsonConvert.DeserializeObject<FetchChannelsResponse>(value: responseText);
                promise.Resolve(value: publicChannelsResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchStickChannelResponse> FetchStickChannel(string channelId) {
            var promise = new Promise<FetchStickChannelResponse>();
            var request = HttpManager.POST($"{Config.apiAddress}{Config.apiPath}/channels/{channelId}/stick");
            HttpManager.resume(request: request).Then(responseText => {
                var stickChannel = JsonConvert.DeserializeObject<FetchStickChannelResponse>(value: responseText);
                promise.Resolve(value: stickChannel);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchUnStickChannelResponse> FetchUnStickChannel(string channelId) {
            var promise = new Promise<FetchUnStickChannelResponse>();
            var request = HttpManager.POST($"{Config.apiAddress}{Config.apiPath}/channels/{channelId}/unStick");
            HttpManager.resume(request: request).Then(responseText => {
                var unStickChannel = JsonConvert.DeserializeObject<FetchUnStickChannelResponse>(value: responseText);
                promise.Resolve(value: unStickChannel);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchChannelMessagesResponse> FetchChannelMessages(
            string channelId, string before = null, string after = null) {
            D.assert(before == null || after == null);
            var promise = new Promise<FetchChannelMessagesResponse>();
            var request = HttpManager.GET($"{Config.apiAddress}{Config.apiPath}/channels/{channelId}/messages",
                parameter: before != null
                ? new Dictionary<string, object> {{"before", before}}
                : after != null
                    ? new Dictionary<string, object> {{"after", after}}
                    : null);
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<FetchChannelMessagesResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<DeleteChannelMessageResponse> DeleteChannelMessage(string messageId = null) {
            var promise = new Promise<DeleteChannelMessageResponse>();
            var request = HttpManager.POST($"{Config.apiAddress}{Config.apiPath}/messages/{messageId}/delete");
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<DeleteChannelMessageResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<AckChannelMessagesResponse> AckChannelMessage(string messageId) {
            var promise = new Promise<AckChannelMessagesResponse>();
            var request = HttpManager.POST($"{Config.apiAddress}{Config.apiPath}/messages/{messageId}/ack");
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<AckChannelMessagesResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchChannelMembersResponse> FetchChannelMembers(string channelId, int offset = 0) {
            var promise = new Promise<FetchChannelMembersResponse>();
            var request = HttpManager.GET($"{Config.apiAddress}{Config.apiPath}/channels/{channelId}/members",
                parameter: new Dictionary<string, object> {
                    {"offset", offset}
                });
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<FetchChannelMembersResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchChannelMemberResponse> FetchChannelMember(string channelId, string userId) {
            var promise = new Promise<FetchChannelMemberResponse>();
            var request = HttpManager.GET($"{Config.apiAddress}{Config.apiPath}/channels/{channelId}/members/{userId}");
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<FetchChannelMemberResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<JoinChannelResponse> JoinChannel(string channelId, string groupId = null) {
            var promise = new Promise<JoinChannelResponse>();
            UnityWebRequest request = null;

            if (string.IsNullOrEmpty(groupId)) {
                request = HttpManager.POST($"{Config.apiAddress}{Config.apiPath}/channels/{channelId}/join");
            }
            else {
                request = HttpManager.POST($"{Config.apiAddress}{Config.apiPath}/groups/{groupId}/join");
            }
            
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<JoinChannelResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<LeaveChannelResponse> LeaveChannel(
            string channelId, string memberId = null, string groupId = null) {
            var promise = new Promise<LeaveChannelResponse>();
            UnityWebRequest request;
            if (string.IsNullOrEmpty(groupId) || string.IsNullOrEmpty(memberId)) {
                request = HttpManager.POST($"{Config.apiAddress}{Config.apiPath}/channels/{channelId}/leave");
            }
            else {
                request = HttpManager.POST($"{Config.apiAddress}{Config.apiPath}/groups/{groupId}/leave",
                    parameter: new Dictionary<string, string> {
                        {"memberId", memberId}
                    });
            }
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<LeaveChannelResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchSendMessageResponse> SendImage(string channelId, string content, string nonce,
            string imageData, string parentMessageId = "") {
            var data = Convert.FromBase64String(s: imageData);
            var promise = new Promise<FetchSendMessageResponse>();
            var request = HttpManager.POST(
                $"{Config.apiAddress}{Config.apiPath}/channels/{channelId}/messages/attachments",
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
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(new FetchSendMessageResponse {
                    channelId = channelId,
                    content = content,
                    nonce = nonce
                });
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchChannelMembersResponse> FetchChannelMemberSuggestions(string channelId) {
            var promise = new Promise<FetchChannelMembersResponse>();
            var request = HttpManager.GET($"{Config.apiAddress}{Config.apiPath}/channels/{channelId}/members",
                parameter: new Dictionary<string, object> {
                    {"get", "active"}
                });
            HttpManager.resume(request: request).Then(responseText => {
                var members = JsonConvert.DeserializeObject<FetchChannelMembersResponse>(value: responseText);
                promise.Resolve(value: members);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchChannelMemberQueryResponse> FetchChannelMemberQuery(string channelId,
            string query) {
            var promise = new Promise<FetchChannelMemberQueryResponse>();
            var request = HttpManager.GET($"{Config.apiAddress}{Config.apiPath}/channels/{channelId}/searchMembers", 
                parameter: new Dictionary<string, object> {
                    {"q", query}
                });
            HttpManager.resume(request: request).Then(responseText => {
                var members = JsonConvert.DeserializeObject<FetchChannelMemberQueryResponse>(value: responseText);
                promise.Resolve(value: members);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}