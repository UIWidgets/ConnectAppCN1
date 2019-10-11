using System;
using ConnectApp.Utils;
using RSG;
using UnityEngine;

namespace ConnectApp.Api {
    public static class ShareApi {
        public static Promise<byte[]> FetchImageBytes(string url) {
            var promise = new Promise<byte[]>();
            HttpManager.DownloadImage(url: url).Then(responseText => {
                if (url.EndsWith(".jpg") || url.EndsWith(".png")) {
                    var quality = 75;
                    var data = responseText.EncodeToJPG(quality: quality);
                    while (data.Length > 32 * 1024) {
                        quality -= 1;
                        data = responseText.EncodeToJPG(quality: quality);
                    }

                    if (data != null) {
                        promise.Resolve(value: data);
                    }
                    else {
                        promise.Reject(new Exception("No user under this username found!"));
                    }
                }
                else {
                    promise.Reject(new Exception("no picture"));
                }
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}