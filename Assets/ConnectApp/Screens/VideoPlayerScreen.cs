using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class VideoPlayerScreen : StatelessWidget {
        public VideoPlayerScreen(
            string url = null,
            Key key = null
        ) : base(key) {
            D.assert(url != null);
            this.url = url;
        }

        private readonly string url;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.Black,
                child: new CustomSafeArea(
                    child: new Container(
                        color: CColors.Black,
                        child: new Center(
                            child: new AspectRatio(
                                aspectRatio: 16.0f / 9.0f,
                                child: new CustomVideoPlayer(
                                    "https://www.quirksmode.org/html5/videos/big_buck_bunny.mp4"
                                )
                            )
                        )
                    )
                )
            );
        }
    }
}