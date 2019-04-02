using ConnectApp.components;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class VideoPlayerTest : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Center(
                child: new Container(
                    width: MediaQuery.of(context).size.width,
                    height: MediaQuery.of(context).size.width * 9 / 16,
                    child: new CustomVideoPlayer(
                        url: "https://www.quirksmode.org/html5/videos/big_buck_bunny.mp4"
                    )
                )
            );
        }
    }
}