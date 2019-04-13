using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.api {
    public static class LoginApi {
        public static IPromise<LoginInfo> LoginByEmail(string email, string password) {
            
            var promise = new Promise<LoginInfo>();
            var para = new LoginParameter {
                email = email,
                password = password
            };
            var body = JsonConvert.SerializeObject(para);
            var request = HttpManager.initRequest(Config.apiAddress + "/auth/live/login", Method.POST);
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
            HttpManager.resume(request).Then(responseText =>
            {  
                var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(responseText);
                promise.Resolve(loginInfo);
            }).Catch(exception =>
            {
                promise.Reject(exception);  
            });
            return promise;
        }

        public static IPromise<LoginInfo> LoginByWechat(string code) {
            
            var promise = new Promise<LoginInfo>();
            var para = new WechatLoginParameter {
                code = code
            };
            var body = JsonConvert.SerializeObject(para);
            var request = HttpManager.initRequest(Config.apiAddress + "/auth/live/wechat", Method.POST);
            var bodyRaw = Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
            HttpManager.resume(request).Then(responseText =>
            {  
                var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(responseText);
                promise.Resolve(loginInfo);
            }).Catch(exception =>
            {
                promise.Reject(exception);  
            });
            return promise;
        }
        

        public static IPromise<string> FetchCreateUnityIdUrl() {
            
            var promise = new Promise<string>();
            var request =
                HttpManager.GET(Config.apiAddress + "/api/authUrl?redirect_to=%2F&locale=zh_CN&is_reg=true");
            HttpManager.resume(request).Then(responseText =>
            {  
                var urlDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                promise.Resolve(urlDictionary["url"]);
            }).Catch(exception =>
            {
                promise.Reject(exception);  
            });
            return promise;
        }
       
    }
}