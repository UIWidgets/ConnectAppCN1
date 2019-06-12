using RSG;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.LikeButton.Utils {
    public delegate void LikeButtonTapCallback();

    public delegate Widget LikeWidgetBuilder(bool isLiked);

    public delegate Widget LikeCountWidgetBuilder(int likeCount, bool isLiked, string text);

    public enum LikeCountAnimationType {
        none,
        part,
        all
    }
}