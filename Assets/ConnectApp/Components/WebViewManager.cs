using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.Components {
    public class WebViewManager {
        internal WebViewManager() {
        }

        public static readonly WebViewManager instance = new WebViewManager();

        public static readonly List<string> localFiles = new List<string> {
            "TinyRacing.asm.js",
            "TinyRacing.html",
            "TinyRacing.js",
            "TinyRacing.mem",
            "TinyRacing.symbols",
            "Data/0d5e50f7eabe6a6502c1030d4ee4718f",
            "Data/10aeba28d4ab63c3305682fc472875c5",
            "Data/280810710d6f05853ec42a19b495f691",
            "Data/2854a0d276ea18dafb86129dceebd5fd",
            "Data/2cd9007690621096b89af05f593f8663",
            "Data/2d64951bcbe1207bbc4428bd48372e6e",
            "Data/2de1e48a0df415974f1e4ab70eee6a39",
            "Data/3e34a5381fe416054b1ef0781db8fa23",
            "Data/449946fd57371d5c42920bb00a50bf55",
            "Data/46b433b264c69cbd39f04ad2e5d12be8",
            "Data/59358898b48609a23461e9c8b859ed2f",
            "Data/59d6638e4b7e8aaccc4fc9f5f426d4a2",
            "Data/5b670003415d5cee62e802c8835f9b6a",
            "Data/65d5fbc38183100ec77bafab84861788",
            "Data/6f8a0afafeb011d229556ecb72b64f2b",
            "Data/7cc46b9899b87b873e6ea34289e77298",
            "Data/7e32ce129f8c57e7e00f7c1fa375250b",
            "Data/8df76df07a7373d6c0ad5c99f80b90e8",
            "Data/909c715064827523f10db9020a99b895",
            "Data/97162039e7f37ef4ca7440c5715f50be",
            "Data/9c1f59a8469e55a0c310d07852af55cb",
            "Data/a5b5006c295bac1813f192b14f6259f6",
            "Data/ab0761da2a51c32e2615a68509584819",
            "Data/ad9f50e074e96e6dcb06e0d003872bba",
            "Data/af72d1c4fd8e9b256dff946a8bcfbdfb",
            "Data/b428c1199fcb250b43ecc6508f5c7933",
            "Data/bc3c5d55c3bacd14b54a3db8cf949399",
            "Data/c3c4c8e879a47e4d55b552c78dcb37b4",
            "Data/c5ec4e826e38ad1393950d34d5dc9bde",
            "Data/c8d2de3e6bc4aa1b85594883919778f3",
            "Data/d85957014e39ac2b6633b84bfe644da6",
            "Data/e202c59369789e0284f7a1b2b6aef378",
            "Data/e8e9a0d851982419005b605602bf0e69",
            "Data/ead95ae8a6288294c98220eeaff82fe6",
            "Data/f24cae80b439bd77d3d4a0431ad79207",
            "Data/f9da9dfd0a3128a4ce5eb41feeec2936",
            "Data/fb938b7923ddc981c8055f0083baa709",
            "Data/ffaa5d7921d081cd42b1cc4d188f317a",
        };

        WebViewObject webView { get; set; }

        GameObject gameObject { get; set; }

        public WebViewObject getWebView() {
            if (this.gameObject.GetComponent<WebViewObject>()) {
                instance.webView = this.gameObject.GetComponent<WebViewObject>();
            }
            else {
                instance.webView = this.gameObject.AddComponent<WebViewObject>();
            }

            return instance.webView;
        }

        public static void destroyWebView() {
            Object.Destroy(instance.webView);
        }

        public void initWebView(GameObject gameObject) {
            this.gameObject = gameObject;
        }

        public static bool finishedCopyFiles = false;

        public string getFileURL(string filename) {
            return "file://" + Path.Combine(Application.persistentDataPath, filename);
        }

        public IEnumerator copyFilesToPersistentDataPath() {
            // Create directory persistentDataPath/Data/ ahead of the time
            var dataPath = Path.Combine(Application.persistentDataPath, "Data");
            if (!Directory.Exists(dataPath)) {
                Directory.CreateDirectory(dataPath);
            }
            
            foreach (var filename in localFiles) {
                var src = Path.Combine(Application.streamingAssetsPath, filename);
                var dst = Path.Combine(Application.persistentDataPath, filename);
                byte[] result = null;
                if (src.Contains("://")) {  // for Android
                    var www = UnityWebRequest.Get(src);
                    yield return www.SendWebRequest();
                    if (www.isDone) {
                        result = www.downloadHandler.data;
                    }
                } else {
                    result = File.ReadAllBytes(src);
                }
                File.WriteAllBytes(dst, result);
            }

            finishedCopyFiles = true;
        }
    }
}