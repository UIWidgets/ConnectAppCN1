using System.Collections.Generic;
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
            var request = HttpManager.POST("/auth/live/login", para);
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
            var request = HttpManager.POST("/auth/live/wechat", para);
            HttpManager.resume(request).Then(responseText => {
                var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(responseText);
                promise.Resolve(loginInfo);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }


        public static IPromise<string> FetchCreateUnityIdUrl() {
            var promise = new Promise<string>();
            var request =
                HttpManager.GET("/api/authUrl?redirect_to=%2F&locale=zh_CN&is_reg=true");
            HttpManager.resume(request).Then(responseText => {
                var urlDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                promise.Resolve(urlDictionary["url"]);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }
    }
}