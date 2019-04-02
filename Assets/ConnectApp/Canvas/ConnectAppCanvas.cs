using ConnectApp.models;
using ConnectApp.redux;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.canvas {
    public sealed class ConnectAppCanvas : UIWidgetsPanel {
        protected override void OnEnable() {
            base.OnEnable();
            Application.targetFrameRate = 60;
            FontManager.instance.addFont(Resources.Load<Font>("Material Icons"), "Material Icons");
            FontManager.instance.addFont(Resources.Load<Font>("Roboto-Regular"), "Roboto-Regular");
            FontManager.instance.addFont(Resources.Load<Font>("Roboto-Medium"), "Roboto-Medium");
            FontManager.instance.addFont(Resources.Load<Font>("Roboto-Bold"), "Roboto-Bold");
            FontManager.instance.addFont(Resources.Load<Font>("PingFangSC-Regular"), "PingFangSC-Regular");
            FontManager.instance.addFont(Resources.Load<Font>("PingFangSC-Medium"), "PingFangSC-Medium");
            FontManager.instance.addFont(Resources.Load<Font>("PingFangSC-Semibold"), "PingFangSC-Semibold");
//            var videoPlayer = GetComponent<VideoPlayer>();
//            VideoPlayerManager.instance.player = videoPlayer;
        }

        protected override Widget createWidget() {
            return new StoreProvider<AppState>(
                StoreProvider.store,
                new WidgetsApp(
                    home: new Router(),
                    pageRouteBuilder: pageRouteBuilder
                )
            );
        }

        private PageRouteFactory pageRouteBuilder {
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