using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Video;
using Texture = Unity.UIWidgets.widgets.Texture;

namespace ConnectApp.components
{
    public class CustomVideoPlayer : StatelessWidget
    {
        public CustomVideoPlayer(
            string url,
            Key key = null
            ) : base(key)
        {
            this.url = url;
        }

        public readonly string url;


        public override Widget build(BuildContext context)
        {
            var texture = Resources.Load<RenderTexture>("ConnectAppRT");
            var player = VideoPlayerManager.instance.player;
            player.url = url;
            player.targetTexture = texture;
            player.isLooping = true;
            player.aspectRatio = VideoAspectRatio.FitOutside;
            player.sendFrameReadyEvents = true;
            player.frameReady += (_, __) => Texture.textureFrameAvailable();
            player.Prepare();

            return new Container(
                child: new Stack(children: new List<Widget>
                {
                    new Texture(texture: texture),
                    new Positioned(
                        left: 16,
                        bottom: 16,
                        child: new GestureDetector(
                            onTap: () =>
                            {
                                if (player.isPlaying)
                                {
                                    player.Pause();
                                }
                                else
                                {
                                    player.Prepare();
                                }
                            },
                            child:new Icon(Icons.eye, null, 24,CColors.Red))) 
                })
            );
        }
    }
}