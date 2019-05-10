using ConnectApp.components;
using ConnectApp.models;
using ConnectApp.plugins;
using ConnectApp.redux;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.canvas {
    public sealed class ConnectAppCanvas : UIWidgetsPanel {
        protected override void OnEnable() {
            base.OnEnable();
            Screen.fullScreen = false;
            Application.targetFrameRate = 60;
            FontManager.instance.addFont(Resources.Load<Font>("Material Icons"), "Material Icons");
            FontManager.instance.addFont(Resources.Load<Font>("Roboto-Regular"), "Roboto-Regular");
            FontManager.instance.addFont(Resources.Load<Font>("Roboto-Medium"), "Roboto-Medium");
            FontManager.instance.addFont(Resources.Load<Font>("Roboto-Bold"), "Roboto-Bold");
            FontManager.instance.addFont(Resources.Load<Font>("PingFangSC-Regular"), "PingFangSC-Regular");
            FontManager.instance.addFont(Resources.Load<Font>("PingFangSC-Medium"), "PingFangSC-Medium");
            FontManager.instance.addFont(Resources.Load<Font>("PingFangSC-Semibold"), "PingFangSC-Semibold");
            FontManager.instance.addFont(Resources.Load<Font>("Menlo-Regular"), "Menlo");
            FontManager.instance.addFont(Resources.Load<Font>("iconFont"), "iconfont");
            VideoPlayerManager.instance.initPlayer(gameObject);
            WebViewManager.instance.initWebView(gameObject);
        }

        protected override void Update() {
            base.Update();
            if (!Application.isEditor) JPushPlugin.addListener();
        }

        protected override Widget createWidget() {
            return new StoreProvider<AppState>(
                StoreProvider.store,
                new WidgetsApp(
                    home: new VersionUpdater(
                        new Router()
                    ),
                    pageRouteBuilder: pageRouteBuilder
                )
            );
        }

        private static PageRouteFactory pageRouteBuilder {
            get {
                return (settings, builder) =>
                    new PageRouteBuilder(
                        settings,
                        (context, animation, secondaryAnimation) => builder(context)
                    );
            }
        }
    }
}