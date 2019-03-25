using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Video;
using Texture = Unity.UIWidgets.widgets.Texture;

namespace ConnectApp.components
{
    public class CustomVideoPlayer : StatefulWidget
    {
        public CustomVideoPlayer(
            string url,
            Key key = null
            ) : base(key)
        {
            this.url = url;
        }

        public readonly string url;

        public override State createState()
        {
            return new _CustomVideoPlayerState();
        }
    }

    public class _CustomVideoPlayerState : State<CustomVideoPlayer>
    {
        private VideoPlayer _player = null;
        private RenderTexture _texture = null;
        private bool isPaused = false;
        public override void initState()
        {
            base.initState();
            
            _texture = Resources.Load<RenderTexture>("ConnectAppRT");

            _player = _videoPlayer(widget.url);

            isPaused = false;
        }

        public override void dispose()
        {
            base.dispose();
            _player.Stop();
        }

        public override Widget build(BuildContext context)
        {
            return new Container(
                child: new Stack(children: new List<Widget>
                {
                    new Texture(texture: _texture),
                    new Positioned(
                        top:0,
                        right:0,
                        left:0,
                        bottom:0,
                        child: isPaused? new GestureDetector(
                            onTap: () =>
                            {
                                _player.Play();
                                setState(() => { isPaused = false; });
                            },
                            child:new Icon(Icons.play_arrow, null, 64, CColors.White)

                        ):new GestureDetector(
                            onTap: () =>
                            {
                                _player.Pause();
                                setState(() => { isPaused = true; });
                            },
                            child: new Container(
                                color:CColors.Transparent)
                        )
                    )
                })
            );
        }

        private VideoPlayer _videoPlayer(string url)
        {
            var player = VideoPlayerManager.instance.player;
            player.url = url;
            player.targetTexture = _texture;
            player.isLooping = false;
            player.aspectRatio = VideoAspectRatio.FitOutside;
            player.sendFrameReadyEvents = true;
            player.frameReady += (_, __) => Texture.textureFrameAvailable();
            
            player.Prepare();
            return player;
        }

    }
}