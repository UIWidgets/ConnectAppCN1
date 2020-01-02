using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.Swiper {
    public abstract class SwiperPlugin {
        public abstract Widget build(BuildContext context, SwiperPluginConfig config);
    }

    public class SwiperPluginConfig {
        public readonly int? activeIndex;
        public readonly int itemCount;
        public readonly PageIndicatorLayout? indicatorLayout;
        public readonly Axis scrollDirection;
        public readonly bool? loop;
        public readonly bool? outer;
        public readonly PageController pageController;
        public readonly SwiperController controller;
        public readonly SwiperLayout layout;

        public SwiperPluginConfig(
            int? activeIndex = null,
            int? itemCount = null,
            PageIndicatorLayout? indicatorLayout = null,
            bool? outer = null,
            Axis scrollDirection = Axis.horizontal,
            SwiperController controller = null,
            PageController pageController = null,
            SwiperLayout layout = SwiperLayout.normal,
            bool? loop = null
        ) {
            D.assert(controller != null);
            D.assert(itemCount != null);
            this.activeIndex = activeIndex;
            this.itemCount = itemCount.Value;
            this.indicatorLayout = indicatorLayout;
            this.outer = outer;
            this.scrollDirection = scrollDirection;
            this.controller = controller;
            this.pageController = pageController;
            this.layout = layout;
            this.loop = loop;
        }
    }

    public class SwiperPluginView : StatelessWidget {
        readonly SwiperPlugin plugin;
        readonly SwiperPluginConfig config;

        public SwiperPluginView(
            SwiperPlugin plugin,
            SwiperPluginConfig config,
            Key key = null
        ) : base(key: key) {
            this.plugin = plugin;
            this.config = config;
        }

        public override Widget build(BuildContext context) {
            return this.plugin.build(context: context, config: this.config);
        }
    }
}