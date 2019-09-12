using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.Api {
    public static class ChannelApi {
        public static Promise<FetchPublicChannelsResponse> FetchPublicChannels() {
            var promise = new Promise<FetchPublicChannelsResponse>();
            var para = new Dictionary<string, object> {
                {"k", "[%22q:%22]"},
                {"lt", "public"},
                {"workspaceId", "058d9079fac00000"}, // TEST
                // {"workspaceId", "05a748aedac0c000"}, // PROD
            };
            var request = HttpManager.GET($"{Config.apiAddress}/api/c", parameter: para);
            HttpManager.resume(request).Then(responseText => {
                var articlesResponse = JsonConvert.DeserializeObject<FetchPublicChannelsResponse>(responseText);
                promise.Resolve(articlesResponse);
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
                var articlesResponse = JsonConvert.DeserializeObject<FetchChannelMessagesResponse>(responseText);
                promise.Resolve(articlesResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}