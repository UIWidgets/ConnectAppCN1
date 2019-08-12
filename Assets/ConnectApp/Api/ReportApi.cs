using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
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
            var request = HttpManager.POST($"{Config.apiAddress}/api/report", para);
            HttpManager.resume(request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise Feedback(FeedbackType type, string contact, string name, string content) {
            var promise = new Promise();
            var para = new FeedbackParameter {
                type = type.Value,
                contact = contact,
                name = name,
                content = content,
                data = null
            };
            Debug.Log($"para: {para}");
            var request = HttpManager.POST($"{Config.apiAddress}/api/connectapp/feedback", para);
            HttpManager.resume(request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}