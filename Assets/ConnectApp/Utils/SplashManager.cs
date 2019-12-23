using System.IO;
using ConnectApp.Api;
using ConnectApp.Models.Model;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.Utils {
    public static class SplashManager {
        const string SplashInfoKey = "SPlashInfo";

        static readonly string PATH = Application.persistentDataPath + "/";

        static byte[] image_bytes;

        public static byte[] readImage() {
            if (image_bytes == null) {
                var splash = JsonConvert.DeserializeObject<Splash>(PlayerPrefs.GetString(SplashInfoKey));
                image_bytes = CImageUtils.readImage(PATH + splash.image.GetHashCode());
            }

            return image_bytes;
        }

        public static void fetchSplash() {
            SplashApi.FetchSplash().Then(splash => {
                if (splash == null) {
                    deleteSplashFile();
                    return;
                }

                if (!isExistSplash()) {
                    fetchImage(splash);
                }
                else {
                    var oldInfo = JsonConvert.DeserializeObject<Splash>(PlayerPrefs.GetString(SplashInfoKey));
                    if (oldInfo.id == splash.id && oldInfo.image == splash.image) {
                        return;
                    }

                    deleteSplashFile();
                    fetchImage(splash);
                }
            });
        }

        static void deleteSplashFile() {
            if (isExistSplash()) {
                var oldInfo = JsonConvert.DeserializeObject<Splash>(PlayerPrefs.GetString(SplashInfoKey));
                if (File.Exists(PATH + oldInfo.image.GetHashCode())) {
                    File.Delete(PATH + oldInfo.image.GetHashCode());
                }
            }
        }

        static void fetchImage(Splash splash) {
            SplashApi.FetchSplashImage(CImageUtils.SplashImageUrl(splash.image)).Then(imageBytes => {
                File.WriteAllBytes(PATH + splash.image.GetHashCode(), imageBytes);
                var splashInfo = JsonConvert.SerializeObject(splash);
                PlayerPrefs.SetString(SplashInfoKey, splashInfo);
            });
        }

        public static bool isExistSplash() {
            if (PlayerPrefs.GetString(SplashInfoKey).isEmpty()) {
                return false;
            }

            var splash = JsonConvert.DeserializeObject<Splash>(PlayerPrefs.GetString(SplashInfoKey));
            if (File.Exists(PATH + splash.image.GetHashCode())) {
                return true;
            }

            return false;
        }

        public static Splash getSplash() {
            if (isExistSplash()) {
                return JsonConvert.DeserializeObject<Splash>(PlayerPrefs.GetString(SplashInfoKey));
            }

            return null;
        }

        public static void hiddenAndroidSpalsh() {
#if UNITY_ANDROID
            hiddenSplash();
#endif
        }

#if UNITY_ANDROID
        static AndroidJavaObject _plugin;

        static AndroidJavaObject Plugin() {
            if (_plugin == null) {
                _plugin = new AndroidJavaClass("com.unity3d.unityconnect.plugins.CommonPlugin");
            }

            return _plugin;
        }

        static void hiddenSplash() {
            Plugin().CallStatic("hiddenSplash");
        }
#endif
    }
}