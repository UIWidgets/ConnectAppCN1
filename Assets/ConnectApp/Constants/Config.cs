namespace ConnectApp.Constants {
    public static class Config {
        public const bool enableDebug = false;

        public const string apiAddress_cn = "https://connect.unity.cn";
        
        public const string apiAddress_com = "https://connect.unity.com";
        
        public const string unity_com_url = "https://connect.unity.com";
        
        public const string unity_cn_url = "https://unity.cn";

        public const string apiPath = "/api/connectapp/v4";
        
        public const string domain = ".connect.unity.cn";

        public const string idBaseUrl = "https://id.unity.cn";

        public const string termsOfService = "https://unity.cn/legal/terms-of-service";

        public const string privacyPolicy = "https://unity.cn/legal/privacy-policy";

        public const string wechatAppId = "wx0ab79f0c7db7ca52";

        public const string jgAppKey = "a50eff2d99416a0495f02766";

        public const string miniId = "gh_f731aec0bdd0";

        public const string versionName = "2.0.5";

        public const int versionCode = 113;

        public const string messengerTag = "messenger";

        public const string originCodeUrl = "https://github.com/UnityTech/ConnectAppCN";

        public const string widgetOriginCodeUrl = "https://github.com/UnityTech/UIWidgets";

        public const string unityStoreUrl = "https://store.unity.com";

        public const string unityLearnPremiumUrl = "https://unity.com/learn-premium";
        
        public const int miniProgramType = 0; // 0 -> 正式版  1 -> 开发版  2 -> 体验版
        
        public const string store = "dev";
        
#if UNITY_IOS
        public const string platform = "ios";
//        public const string store = "appstore";
        public const string buglyId = "f3e3717b9f";
#elif UNITY_ANDROID
        public const string platform = "android";
//        public const string store = "official";
//        public const string store = "xiaomi";
//        public const string store = "huawei";
//        public const string store = "ali";
//        public const string store = "yingyongbao";
        public const string buglyId = "f56bb28093";
#else
        public const string platform = "";
//        public const string store = "";
        public const string buglyId = "";
#endif
    }
}