namespace ConnectApp.Components.Swiper {
    public class SwiperController : IndexController {
        public const int START_AUTOPLAY = 2;

        public const int STOP_AUTOPLAY = 3;

        public const int SWIPE = 4;

        public const int BUILD = 5;

        public SwiperPluginConfig config;

        public float pos;

        public int index;
        public bool animation;
        public bool? autoplay;

        public SwiperController() { }

        public void startAutoplay() {
            this.evt = START_AUTOPLAY;
            this.autoplay = true;
            this.notifyListeners();
        }

        public void stopAutoplay() {
            this.evt = STOP_AUTOPLAY;
            this.autoplay = false;
            this.notifyListeners();
        }
    }
}