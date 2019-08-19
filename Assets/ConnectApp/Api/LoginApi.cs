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
            var request = HttpManager.POST($"{Config.apiAddress}/api/connectapp/auth/live/login", para);
            HttpManager.resume(request).Then(responseText => {
                var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(responseText);
                promise.Resolve(loginInfo);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static IPromise<LoginInfo> LoginByWechat(string code) {
            var promise = new Promise<LoginInfo>();
            var para = new WechatLoginParameter {
                code = code
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/connectapp/auth/live/wechat", para);
            HttpManager.resume(request).Then(responseText => {
                var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(responseText);
                promise.Resolve(loginInfo);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static IPromise<bool> LoginByQr(string token) {
            var promise = new Promise<bool>();
            var para = new QRLoginParameter {
                token = token
            };
            var request = HttpManager.POST($"{Config.apiAddress}/api/auth/qrlogin", para);
            HttpManager.resume(request).Then(responseText => {
                var successDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseText);
                var success = successDictionary.ContainsKey("success") ? successDictionary["success"] : false;
                promise.Resolve((bool)success);
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }

        public static IPromise<string> FetchCreateUnityIdUrl() {
            var promise = new Promise<string>();
            var para = new Dictionary<string, object> {
                {"redirect_to", "%2F"},
                {"locale", "zh_CN"},
                {"is_reg", "true"}
            };
            var request =
                HttpManager.GET($"{Config.apiAddress}/api/authUrl", para);
            HttpManager.resume(request).Then(responseText => {
                var urlDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                promise.Resolve(urlDictionary["url"]);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static IPromise<string> InitData() {
            var promise = new Promise<string>();
            var request =
                HttpManager.GET($"{Config.apiAddress}/api/connectapp/initData");
            HttpManager.resume(request).Then(responseText => {
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                promise.Resolve(dictionary["VS"]);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}