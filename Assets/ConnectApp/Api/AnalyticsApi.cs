using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using RSG;
using UnityEngine;

namespace ConnectApp.Api {
    public static class AnalyticsApi {
        public static Promise AnalyticsApp(string userId, string eventType, List<Dictionary<string, string>> data) {
            var promise = new Promise();
            var device = AnalyticsManager.deviceId() + (SystemInfo.deviceModel ?? "");
            var para = new OpenAppParameter {
                userId = userId,
                device = device,
                store = Config.store,
                eventType = eventType,
                appTime = DateTime.UtcNow,
                extraData = data
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/statistic", parameter: para);
            HttpManager.resume(request: request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}