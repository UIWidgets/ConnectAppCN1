using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.foundation;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.Api {
    public static class ChannelApi {
        public static Promise<FetchChannelsResponse> FetchChannels(int page, bool joined = true,
            bool discoverAll = false) {
            var promise = new Promise<FetchChannelsResponse>();
            var para = new Dictionary<string, object> {
                {"discoverPage", page},
                {"joined", joined ? "true" : "false"},
                {discoverAll ? "discoverAll" : "discover", "true"}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/channels", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var publicChannelsResponse = JsonConvert.DeserializeObject<FetchChannelsResponse>(value: responseText);
                promise.Resolve(value: publicChannelsResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchStickChannelResponse> FetchStickChannel(string channelId) {
            var promise = new Promise<FetchStickChannelResponse>();
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/stick");
            HttpManager.resume(request: request).Then(responseText => {
                var stickChannel = JsonConvert.DeserializeObject<FetchStickChannelResponse>(value: responseText);
                promise.Resolve(value: stickChannel);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchUnStickChannelResponse> FetchUnStickChannel(string channelId) {
            var promise = new Promise<FetchUnStickChannelResponse>();
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/unStick");
            HttpManager.resume(request: request).Then(responseText => {
                var unStickChannel = JsonConvert.DeserializeObject<FetchUnStickChannelResponse>(value: responseText);
                promise.Resolve(value: unStickChannel);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchMuteChannelResponse> FetchMuteChannel(string channelId) {
            var promise = new Promise<FetchMuteChannelResponse>();
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/mute");
            HttpManager.resume(request: request).Then(responseText => {
                var muteChannel = JsonConvert.DeserializeObject<FetchMuteChannelResponse>(value: responseText);
                promise.Resolve(value: muteChannel);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchUnMuteChannelResponse> FetchUnMuteChannel(string channelId) {
            var promise = new Promise<FetchUnMuteChannelResponse>();
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/unMute");
            HttpManager.resume(request: request).Then(responseText => {
                var unMuteChannel = JsonConvert.DeserializeObject<FetchUnMuteChannelResponse>(value: responseText);
                promise.Resolve(value: unMuteChannel);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchChannelMessagesResponse> FetchChannelMessages(string channelId, string before = null,
            string after = null) {
            D.assert(before == null || after == null);
            var promise = new Promise<FetchChannelMessagesResponse>();
            var para = new Dictionary<string, object> {
                {"needDeleted", "true"}
            };
            if (before != null) {
                para.Add("before", value: before);
            } else if (after != null) {
                para.Add("after", value: after);
            }
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/messages",
                parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<FetchChannelMessagesResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<DeleteChannelMessageResponse> DeleteChannelMessage(string messageId = null) {
            var promise = new Promise<DeleteChannelMessageResponse>();
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/messages/{messageId}/delete");
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<DeleteChannelMessageResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<AckChannelMessagesResponse> AckChannelMessage(string messageId) {
            var promise = new Promise<AckChannelMessagesResponse>();
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/messages/{messageId}/ack");
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<AckChannelMessagesResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchChannelInfoResponse> FetchChannelInfo(string channelId) {
            var promise = new Promise<FetchChannelInfoResponse>();
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}");
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<FetchChannelInfoResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchChannelMembersResponse> FetchChannelMembers(string channelId, int offset = 0) {
            var promise = new Promise<FetchChannelMembersResponse>();
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/members",
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
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/members/{userId}");
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<FetchChannelMemberResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<JoinChannelResponse> JoinChannel(string channelId, string groupId = null) {
            var promise = new Promise<JoinChannelResponse>();
            UnityWebRequest request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}" +
                (groupId.isEmpty() ? $"/channels/{channelId}/join" : $"/groups/{groupId}/join"));

            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<JoinChannelResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<LeaveChannelResponse> LeaveChannel(string channelId, string memberId = null,
            string groupId = null) {
            var promise = new Promise<LeaveChannelResponse>();
            UnityWebRequest request;
            if (groupId.isEmpty() || memberId.isEmpty()) {
                request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/leave");
            }
            else {
                request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/groups/{groupId}/leave",
                    parameter: new Dictionary<string, string> {
                        {"memberId", memberId}
                    });
            }

            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<LeaveChannelResponse>(value: responseText));
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<FetchSendMessageResponse> SendAttachment(string channelId, string content, string nonce,
            byte[] attachmentData, string filename, string fileType, string parentMessageId = "") {
            var promise = new Promise<FetchSendMessageResponse>();
            var request = HttpManager.POST(
                $"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/messages/attachments",
                new List<List<object>> {
                    new List<object> {"channel", channelId},
                    new List<object> {"content", content},
                    new List<object> {"parentMessageId", parentMessageId},
                    new List<object> {"nonce", nonce},
                    new List<object> {"size", $"{attachmentData.Length}"},
                    new List<object> {"file", attachmentData}
                },
                true,
                filename: filename,
                fileType: fileType);
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
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/members",
                new Dictionary<string, object> {
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
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/channels/{channelId}/searchMembers",
                parameter: new Dictionary<string, object> {
                    {"q", query}
                });
            HttpManager.resume(request: request).Then(responseText => {
                var members = JsonConvert.DeserializeObject<FetchChannelMemberQueryResponse>(value: responseText);
                promise.Resolve(value: members);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<UpdateChannelMessagesReactionResponse> UpdateReaction(
            string messageId, string likeEmoji, bool isRemove = false) {
            var promise = new Promise<UpdateChannelMessagesReactionResponse>();
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/messages/{messageId}/{(isRemove ? "removeReaction" : "addReaction")}",
                likeEmoji != null ? new Dictionary<string, object> {
                    {"reactionType" , "like"},
                    {"likeEmoji", likeEmoji}
                } : new Dictionary<string, object> {
                    {"reactionType" , "like"}
                });
            HttpManager.resume(request: request).Then(responseText => {
                promise.Resolve(JsonConvert.DeserializeObject<UpdateChannelMessagesReactionResponse>(value: responseText));
            }).Catch(exception => {
                promise.Reject(ex: exception);
            });
            return promise;
        }
    }
}