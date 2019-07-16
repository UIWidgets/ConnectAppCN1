using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public class AnalyticsApi {
        public static Promise OpenApp(string userId, string device, string eventType, DateTime appTime,
            Dictionary<string, string> data) {
            var promise = new Promise();
            var dataStr = JsonConvert.SerializeObject(value: data);
            var para = new OpenAppParameter {
                userId = userId,
                device = device,
                eventType = eventType,
                appTime = appTime,
                data = dataStr
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/connectapp/statistic", para);
            HttpManager.resume(request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}