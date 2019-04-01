using System.Runtime.InteropServices;

namespace ConnectApp.plugins
{
    
    public class WechatPlugin
    {
        private WechatPlugin() {
        }

        public static readonly WechatPlugin instance = new WechatPlugin();
        
#if UNITY_IOS 

        public void init(string appId)
        {
            InitWechat(appId);
        }
        public void login(string stateId)
        {
            loginWechat(stateId);
        }
        public bool inInstalled()
        {
            return isInstallWechat();
        }

        [DllImport ("__Internal")]
        internal static extern void InitWechat(string appId);
         
        [DllImport ("__Internal")]
        internal static extern void loginWechat(string stateId);
        
        [DllImport ("__Internal")]
        internal static extern bool isInstallWechat();
#endif
    }
            

}