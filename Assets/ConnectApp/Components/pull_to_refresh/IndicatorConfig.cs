namespace ConnectApp.Components.pull_to_refresh {
    public abstract class Config {
        public readonly float triggerDistance;

        protected Config(
            float triggerDistance
        ) {
            this.triggerDistance = triggerDistance;
        }
    }

    public class RefreshConfig : Config {
        public RefreshConfig(
            int completeDuration = DefaultConstants.default_completeDuration,
            float visibleRange = DefaultConstants.default_VisibleRange,
            float triggerDistance = DefaultConstants.default_refresh_triggerDistance
        ) : base(triggerDistance) {
            this.completeDuration = completeDuration;
            this.visibleRange = visibleRange;
        }

        public readonly int completeDuration;
        public readonly float visibleRange;
    }

    public class LoadConfig : Config {
        public LoadConfig(
            bool autoLoad = DefaultConstants.default_AutoLoad,
            bool bottomWhenBuild = DefaultConstants.default_BottomWhenBuild,
            bool enableOverScroll = DefaultConstants.default_enableOverScroll,
            float triggerDistance = DefaultConstants.default_load_triggerDistance
        ) : base(triggerDistance) {
            this.autoLoad = autoLoad;
            this.bottomWhenBuild = bottomWhenBuild;
            this.enableOverScroll = enableOverScroll;
        }

        public readonly bool autoLoad;
        public readonly bool bottomWhenBuild;
        public readonly bool enableOverScroll;
    }
}