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
            var para = new Dictionary<string, object> { };
            if (before != null) {
                para["before"] = before;
            } else if (after != null) {
                para["after"] = after;
            }
            var request = HttpManager.GET($"{Config.apiAddress}/api/channels/{channelId}/messages", parameter: para);
            HttpManager.resume(request).Then(responseText => {
                var channelMessagesResponse = JsonConvert.DeserializeObject<FetchChannelMessagesResponse>(responseText);
                promise.Resolve(channelMessagesResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
        
        public static Promise<List<ChannelMember>> FetchChannelMembers(string channelId) {
            var promise = new Promise<List<ChannelMember>>();
            var request = HttpManager.GET($"{Config.apiAddress}/api/channels/{channelId}/members");
            HttpManager.resume(request).Then(responseText => {
                var channelMemberResponse = JsonConvert.DeserializeObject<List<ChannelMember>>(responseText);
                promise.Resolve(channelMemberResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
        
        public static Promise<List<JoinChannelResponse>> JoinChannel(string channelId, string groupId = null) {
            var promise = new Promise<List<JoinChannelResponse>>();
            var request = HttpManager.POST($"{Config.apiAddress}/api/channels/{channelId}/join",
                parameter: new Dictionary<string, string> {
                    {"channelId", channelId}
            });
            if (string.IsNullOrEmpty(groupId)) {
                request = HttpManager.POST($"{Config.apiAddress}/api/group/{groupId}/requestJoin",
                parameter: new Dictionary<string, string> {
                    {"groupId", groupId}
                });
            }
            Debug.Log(request.uri);
            HttpManager.resume(request).Then(responseText => {
                var joinChannelResponse = JsonConvert.DeserializeObject<List<JoinChannelResponse>>(responseText);
                promise.Resolve(joinChannelResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
        
        public static Promise<LeaveChannelResponse> LeaveChannel(string channelId, string groupId = null) {
            var promise = new Promise<LeaveChannelResponse>();
            var request = HttpManager.POST($"{Config.apiAddress}/api/channels/{channelId}/leave",
                parameter: new Dictionary<string, string> {
                    {"channelId", channelId}
            });
            if (string.IsNullOrEmpty(groupId)) {
                request = HttpManager.POST($"{Config.apiAddress}/api/group/{groupId}/deleteMember",
                parameter: new Dictionary<string, string> {
                    {"groupId", groupId}
                });
            }
            HttpManager.resume(request).Then(responseText => {
                var leaveChannelResponse = JsonConvert.DeserializeObject<LeaveChannelResponse>(responseText);
                promise.Resolve(leaveChannelResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}