using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.pull_to_refresh {
    public delegate void OnRefresh(bool up);

    public delegate void OnOffsetChange(bool up, float offset);

    public delegate Widget IndicatorBuilder(BuildContext context, int mode);

    public static class DefaultConstants {
        public const int default_completeDuration = 600;

        public const float default_refresh_triggerDistance = 56.0f;

        public const float default_load_triggerDistance = 5.0f;

        public const float default_VisibleRange = 56.0f;

        public const bool default_AutoLoad = true;

        public const bool default_enablePullDown = true;

        public const bool default_enablePullUp = false;

        public const bool default_BottomWhenBuild = true;

        public const bool default_enableOverScroll = true;

        public const int spaceAnimateMill = 300;

        public const float minSpace = 0.000001f;
    }
}