using UnityEngine;

namespace ConnectApp.components
{
    
    public class WebViewManager
    {
        private WebViewManager() {
        }

        public static readonly WebViewManager instance = new WebViewManager();

        public WebViewObject webViewObject { get; set; }
        
        public void initWebView(GameObject gameObject)
        {
            if (gameObject.GetComponent<WebViewObject>())
            {
                instance.webViewObject = gameObject.GetComponent<WebViewObject>();
            }
            else
            {
                instance.webViewObject = gameObject.AddComponent<WebViewObject>();
            }
        }
    }
}