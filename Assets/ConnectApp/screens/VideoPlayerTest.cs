using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Texture = Unity.UIWidgets.widgets.Texture;

namespace ConnectApp.screens
{
    public class VideoPlayerTest : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            var texture = Resources.Load<RenderTexture>("ConnectAppRT");
            var player = VideoPlayerManager.instance.player;
            player.url = "https://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";
            player.targetTexture = texture;
            player.isLooping = true;
            player.sendFrameReadyEvents = true;
            player.frameReady += (_, __) => Texture.textureFrameAvailable();
            player.Play();
            
            return new Center(
                child: new Container(
                    width:MediaQuery.of(context).size.width,
                    height:MediaQuery.of(context).size.width*2/3,
                    child: new Stack(children:new List<Widget>
                    {
                        new Texture(texture: texture),
                        new Positioned(
                            left:16,
                            bottom:16,
                            child:new Icon(Icons.eye,null,24))
                    }) 
                )
            );
        }
    }
}