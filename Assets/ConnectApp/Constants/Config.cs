namespace ConnectApp.constants {
    public static class Config {
        public const string apiAddress = "https://connect.unity.com";
        
        public const string domain = ".connect.unity.com";

        public const string idBaseUrl = "https://id.unity.com";

        public const string wechatAppId = "wx0ab79f0c7db7ca52";

        public const string versionNumber = "0.8.1";
        
        public const int versionCode = 17;

#if UNITY_IOS
        public const string platform = "ios";
        public const string store = "appstore";
#elif UNITY_ANDROID
        public const string platform = "android";
//        public const string store = "test";
        public const string store = "unity";
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