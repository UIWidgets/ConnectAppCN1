using System;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.State;
using ConnectApp.Plugins;
using ConnectApp.redux;
using ConnectApp.Utils;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Main {
    public sealed class ConnectAppPanel : UIWidgetsPanel {
        protected override void OnEnable() {
            base.OnEnable();
            Debuger.EnableLog = Config.enableDebug;
            Screen.fullScreen = false;
            Screen.orientation = ScreenOrientation.Portrait;
            Window.onFrameRateCoolDown = CustomFrameRateCoolDown;
            LoadFonts();
            VideoPlayerManager.instance.initPlayer(this.gameObject);
            WebViewManager.instance.initWebView(this.gameObject);
            InitBugly();
            AnalyticsManager.EnterApp();
            if (CCommonUtils.isIPhone) {
                if (LocalDataManager.getFPSLabelStatus()) { 
                    FPSLabelPlugin.SwitchFPSLabelShowStatus(true);
                }
            }
        }

        static void InitBugly() {
            if (Application.isEditor) {
                return;
            }

            // Enable the debug log print
            BuglyAgent.ConfigDebugMode(enable: Config.enableDebug);
            // Config default channel, version, user 
            BuglyAgent.ConfigDefault(channel: Config.store, $"{Config.versionName}-{Config.versionCode}",
                user: UserInfoManager.getUserInfo().userId ?? "anonymous", 0);
            // Config auto report log level, default is LogSeverity.LogError, so the LogError, LogException log will auto report
            BuglyAgent.ConfigAutoReportLogLevel(level: LogSeverity.LogWarning);
            // Config auto quit the application make sure only the first one c# exception log will be report, please don't set TRUE if you do not known what are you doing.
            BuglyAgent.ConfigAutoQuitApplication(false);
            // If you need register Application.RegisterLogCallback(LogCallback), you can replace it with this method to make sure your function is ok.
            BuglyAgent.RegisterLogCallback(null);

            // Init the bugly sdk and enable the c# exception handler.
            BuglyAgent.InitWithAppId(appId: Config.buglyId);

            // please call this method to enable c# exception handler only.
            BuglyAgent.EnableExceptionHandler();
        }

        static void CustomFrameRateCoolDown() {
            Application.targetFrameRate = 60;
        }

        static void LoadFonts() {
            FontManager.instance.addFont(Resources.Load<Font>("font/Material Icons"), "Material Icons");
            FontManager.instance.addFont(Resources.Load<Font>("font/Outline Material Icons"), "Outline Material Icons");
            FontManager.instance.addFont(Resources.Load<Font>("font/Roboto-Regular"), "Roboto-Regular");
            FontManager.instance.addFont(Resources.Load<Font>("font/Roboto-Medium"), "Roboto-Medium");
            FontManager.instance.addFont(Resources.Load<Font>("font/Roboto-Bold"), "Roboto-Bold");
            FontManager.instance.addFont(Resources.Load<Font>("font/PingFangSC-Regular"), "PingFangSC-Regular");
            FontManager.instance.addFont(Resources.Load<Font>("font/PingFangSC-Medium"), "PingFangSC-Medium");
            FontManager.instance.addFont(Resources.Load<Font>("font/PingFangSC-Semibold"), "PingFangSC-Semibold");
            FontManager.instance.addFont(Resources.Load<Font>("font/Menlo-Regular"), "Menlo");
            FontManager.instance.addFont(Resources.Load<Font>("font/iconFont"), "iconfont");
        }

        protected override void Update() {
            base.Update();
            JPushPlugin.addListener();
        }

        protected override Widget createWidget() {
            return new StoreProvider<AppState>(
                store: StoreProvider.store,
                new WidgetsApp(
                    home: new Router(),
                    pageRouteBuilder: pageRouteBuilder
                )
            );
        }

        static PageRouteFactory pageRouteBuilder {
            get {
                return (settings, builder) =>
                    new PageRouteBuilder(
                        settings: settings,
                        (context, animation, secondaryAnimation) => builder(context)
                    );
            }
        }

        void OnApplicationFocus(bool hasFocus) {
            if (Application.isEditor) {
                return;
            }

            if (hasFocus) {
                using (WindowProvider.of(GlobalContext.context).getScope()) {
                    AnalyticsManager.foucsTime = DateTime.UtcNow.ToString();
                }
            }
            else {
                using (WindowProvider.of(GlobalContext.context).getScope()) {
                    JPushPlugin.clearIconBadge();
                    if (AnalyticsManager.foucsTime.isNotEmpty()) {
                        AnalyticsManager.AnalyticsActiveTime(
                            (DateTime.UtcNow - DateTime.Parse(AnalyticsManager.foucsTime)).Milliseconds);
                        AnalyticsManager.foucsTime = null;
                    }
                }
            }
        }

        void OnApplicationQuit() {
            if (Application.isEditor) {
                return;
            }

            using (WindowProvider.of(GlobalContext.context).getScope()) {
                if (AnalyticsManager.foucsTime.isNotEmpty()) {
                    AnalyticsManager.AnalyticsActiveTime((DateTime.UtcNow - DateTime.Parse(AnalyticsManager.foucsTime))
                        .Milliseconds);
                }
            }
        }
    }
}