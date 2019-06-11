using RSG;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.LikeButton.Utils {
    public delegate IPromise<bool> LikeButtonTapCallback(bool isLiked);

    public delegate Widget LikeWidgetBuilder(bool isLiked);

    public delegate Widget LikeCountWidgetBuilder(int likeCount, bool isLiked, string text);

    public enum LikeCountAnimationType {
        //no animation
        none,

        //animation only on change part
        part,

        //animation on all
        all
    }
}