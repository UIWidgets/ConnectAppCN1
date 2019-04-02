using ConnectApp.components;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class VideoPlayerTest : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Center(
                child: new AspectRatio(
                    aspectRatio: 16.0f / 9.0f,
                    child: new CustomVideoPlayer(
                        "https://www.quirksmode.org/html5/videos/big_buck_bunny.mp4"
                    )
                )
            );
        }
    }
}