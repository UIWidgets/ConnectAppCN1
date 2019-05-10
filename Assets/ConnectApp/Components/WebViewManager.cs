using UnityEngine;

namespace ConnectApp.components
{
    
    public class WebViewManager
    {
        internal WebViewManager() {
        }

        public static readonly WebViewManager instance = new WebViewManager();

        private WebViewObject webView { get; set; }

        private GameObject gameObject { get; set; }

        public WebViewObject getWebView()
        {
            if (gameObject.GetComponent<WebViewObject>())
            {
                instance.webView = gameObject.GetComponent<WebViewObject>();
            }
            else
            {
                instance.webView = gameObject.AddComponent<WebViewObject>();
            }

            return instance.webView;
        }
        
        public void destroyWebView()
        {
            WebViewObject.Destroy(instance.webView);
        }
        public void initWebView(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}