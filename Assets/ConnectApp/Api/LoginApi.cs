using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class LoginApi {
        public static IPromise<LoginInfo> LoginByEmail(string email, string password) {
            var promise = new Promise<LoginInfo>();
            var para = new LoginParameter {
                email = email,
                password = password
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/auth/live/login", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(value: responseText);
                promise.Resolve(value: loginInfo);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static IPromise<LoginInfo> LoginByWechat(string code) {
            var promise = new Promise<LoginInfo>();
            var para = new WechatLoginParameter {
                code = code
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/auth/live/wechat", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(value: responseText);
                promise.Resolve(value: loginInfo);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static IPromise<bool> LoginByQr(string token, string action) {
            var promise = new Promise<bool>();
            var para = new QRLoginParameter {
                token = token,
                action = action
            };
            var request = HttpManager.POST($"{Config.apiAddress_cn}{Config.apiPath}/auth/qrlogin", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var successDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(value: responseText);
                var success = successDictionary.ContainsKey("success") ? successDictionary["success"] : false;
                promise.Resolve((bool)success);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static IPromise<string> FetchCreateUnityIdUrl() {
            var promise = new Promise<string>();
            var para = new Dictionary<string, object> {
                {"redirect_to", "%2F"},
                {"locale", "zh_CN"},
                {"is_reg", "true"}
            };
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/authUrl", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var urlDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(value: responseText);
                promise.Resolve(urlDictionary["url"]);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static IPromise<FetchInitDataResponse> InitData() {
            var promise = new Promise<FetchInitDataResponse>();
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/initData");
            HttpManager.resume(request: request).Then(responseText => {
                var initDataResponse = JsonConvert.DeserializeObject<FetchInitDataResponse>(value: responseText);
                promise.Resolve(value: initDataResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}