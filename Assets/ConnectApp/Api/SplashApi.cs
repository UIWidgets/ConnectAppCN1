using System;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using UnityEngine;

namespace ConnectApp.Api {
    public static class SplashApi {
        public static Promise<Splash> FetchSplash() {
            var promise = new Promise<Splash>();
            var request = HttpManager.GET($"{Config.apiAddress_cn}{Config.apiPath}/ads");
            HttpManager.resume(request: request).Then(responseText => {
                var splashResponse = JsonConvert.DeserializeObject<Splash>(value: responseText);
                promise.Resolve(value: splashResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }

        public static Promise<byte[]> FetchSplashImage(string url) {
            var promise = new Promise<byte[]>();
            HttpManager.DownloadImage(url: url).Then(responseText => {
                var pngData = responseText.EncodeToPNG();
                if (pngData != null) {
                    promise.Resolve(value: pngData);
                }
                else {
                    promise.Reject(new Exception("No user under this username found!"));
                }
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}