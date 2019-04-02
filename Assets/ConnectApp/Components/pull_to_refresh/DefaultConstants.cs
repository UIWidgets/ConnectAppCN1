using Unity.UIWidgets.widgets;

namespace ConnectApp.components.pull_to_refresh {
    public delegate void OnRefresh(bool up);

    public delegate void OnOffsetChange(bool up, double offset);

    public delegate Widget IndicatorBuilder(BuildContext context, int mode);

    public class DefaultConstants {
        public const int default_completeDuration = 800;

        public const float default_refresh_triggerDistance = 100.0f;

        public const float default_load_triggerDistance = 5.0f;

        public const float default_VisibleRange = 50.0f;

        public const bool default_AutoLoad = true;

        public const bool default_enablePullDown = true;

        public const bool default_enablePullUp = false;

        public const bool default_BottomWhenBuild = true;

        public const bool default_enableOverScroll = true;

        public const int spaceAnimateMill = 300;

        public const float minSpace = 0.000001f;
    }
}