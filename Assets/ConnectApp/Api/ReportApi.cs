using System.Collections.Generic;
using System.Text;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using UnityEngine.Networking;

namespace ConnectApp.Api {
    public static class ReportApi {
        public static Promise ReportItem(string itemId, string itemType, string reportContext) {
            var promise = new Promise();
            var para = new ReportParameter {
                itemType = itemType,
                itemId = itemId,
                reasons = new List<string> {"other:" + reportContext}
            };
            var body = JsonConvert.SerializeObject(para);
            var request = HttpManager.initRequest(Config.apiAddress + "/api/report", Method.POST);
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
            HttpManager.resume(request).Then(responseText => { promise.Resolve(); })
                .Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}