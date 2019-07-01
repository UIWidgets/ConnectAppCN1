using System;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using UnityEngine;

namespace ConnectApp.Api {
    public static class SplashApi {
        public static Promise<Splash> FetchSplash() {
            var promise = new Promise<Splash>();
            var request = HttpManager.GET("/api/connectapp/ads");
            HttpManager.resume(request).Then(responseText => {
                var splashResponse = JsonConvert.DeserializeObject<Splash>(responseText);
                promise.Resolve(splashResponse);
            }).Catch(exception => { promise.Reject(exception); });
            return promise;
        }

        public static Promise<byte[]> FetchSplashImage(string url) {
            var promise = new Promise<byte[]>();
            HttpManager.DownloadImage(url).Then(responseText => {
                var pngData = responseText.EncodeToPNG();
                if (pngData != null) {
                    promise.Resolve(pngData);
                }
                else {
                    promise.Reject(new Exception("No user under this username found!"));
                }
            }).Catch(exception => promise.Reject(exception));
            return promise;
        }
    }
}