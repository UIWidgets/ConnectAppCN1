using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using RSG;

namespace ConnectApp.Api {
    public static class AnalyticsApi {
        public static Promise AnalyticsApp(string userId, string device, string eventType, DateTime appTime,
            List<Dictionary<string, string>> data) {
            var promise = new Promise();
            var para = new OpenAppParameter {
                userId = userId,
                device = device,
                eventType = eventType,
                appTime = appTime,
                extraData = data
            };
            var request = HttpManager.POST($"{Config.apiAddress}{Config.apiPath}/statistic", parameter: para);
            HttpManager.resume(request: request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}