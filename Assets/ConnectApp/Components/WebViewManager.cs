using UnityEngine;

namespace ConnectApp.Components {
    public class WebViewManager {
        internal WebViewManager() {
        }

        public static readonly WebViewManager instance = new WebViewManager();

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
    }
}