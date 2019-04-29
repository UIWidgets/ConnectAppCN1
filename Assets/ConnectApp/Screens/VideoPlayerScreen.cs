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
                child: new Container(
                    color: CColors.Black,
                    child: new AspectRatio(
                        aspectRatio: 16.0f / 9.0f,
                        child: new CustomVideoPlayer(
                            "https://connect.unity.com/project-attachments/5cc266e2edbc2a47c60f38b4/5cc2671cedbc2a46f87c7d0f"
//                            "https://www.quirksmode.org/html5/videos/big_buck_bunny.mp4"
                        )
                    )
                )
            );
        }
    }
}