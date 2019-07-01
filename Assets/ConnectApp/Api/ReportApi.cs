using System.Collections.Generic;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using RSG;

namespace ConnectApp.Api {
    public static class ReportApi {
        public static Promise ReportItem(string itemId, string itemType, string reportContext) {
            var promise = new Promise();
            var para = new ReportParameter {
                itemType = itemType,
                itemId = itemId,
                reasons = new List<string> {"other:" + reportContext}
            };
            var request = HttpManager.POST("/api/report", para);
            HttpManager.resume(request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}