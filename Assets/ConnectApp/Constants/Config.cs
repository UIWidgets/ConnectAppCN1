namespace ConnectApp.Constants {
    public static class Config {
        public const string apiAddress = "https://connect.unity.com";

        public const string domain = ".connect.unity.com";

        public const string idBaseUrl = "https://id.unity.com";

        public const string termsOfService = "https://unity3d.com/legal/terms-of-service";

        public const string privacyPolicy = "https://unity3d.com/legal/privacy-policy";

        public const string wechatAppId = "wx0ab79f0c7db7ca52";

        public const string jgAppKey = "a50eff2d99416a0495f02766";

        public const string versionNumber = "0.9.8";

        public const int versionCode = 31;

#if UNITY_IOS
        public const string platform = "ios";
        public const string store = "appstore";
#elif UNITY_ANDROID
        public const string platform = "android";
//        public const string store = "test";
        public const string store = "official";
//        public const string store = "xiaomi";
//        public const string store = "huawei";
//        public const string store = "ali";
//        public const string store = "yingyongbao";
#else
        public const string platform = "";
        public const string store = "";
#endif
    }
}