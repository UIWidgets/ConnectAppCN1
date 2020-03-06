using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using UnityEngine;

namespace ConnectApp.Api {
    public static class ReportApi {
        public static Promise ReportItem(string itemId, string itemType, string reportContext) {
            var promise = new Promise();
            var para = new ReportParameter {
                itemType = itemType,
                itemId = itemId,
                reasons = new List<string> {"other:" + reportContext}
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/report", parameter: para);
            HttpManager.resume(request: request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise Feedback(FeedbackType type, string content, string name = "", string contact = "") {
            var userId = UserInfoManager.isLogin() ? UserInfoManager.getUserInfo().userId : "";
            var device = AnalyticsManager.deviceId() + (SystemInfo.deviceModel ?? "");
            var dict = new Dictionary<string, string> {
                {"userId", userId}, {"device", device}
            };
            var data = JsonConvert.SerializeObject(value: dict);
            var promise = new Promise();
            var para = new FeedbackParameter {
                type = type.value,
                contact = contact,
                name = name,
                content = content,
                data = data
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/feedback", parameter: para);
            HttpManager.resume(request: request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}