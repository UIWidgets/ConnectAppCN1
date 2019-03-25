using ConnectApp.components;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Texture = Unity.UIWidgets.widgets.Texture;

namespace ConnectApp.screens {
    public class VideoPlayerTest : StatelessWidget {
        public override Widget build(BuildContext context) {
//            var texture = Resources.Load<RenderTexture>("ConnectAppRT");
            var texture = Resources.Load<RenderTexture>("VideoSampleRT");
            var player = VideoPlayerManager.instance.player;
//            player.clip = 
//            player.url = "https://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";
            player.targetTexture = texture;
            player.isLooping = true;
            player.sendFrameReadyEvents = true;
            player.frameReady += (_, __) => Texture.textureFrameAvailable();
            player.Play();

            return new Center(
                child: new Container(
                    width: MediaQuery.of(context).size.width,
                    height: MediaQuery.of(context).size.width * 2 / 3,
                    child: new Texture(texture: texture)
                )
            );
        }
    }
}