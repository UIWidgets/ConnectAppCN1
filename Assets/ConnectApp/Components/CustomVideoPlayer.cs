using System;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Video;
using Color = Unity.UIWidgets.ui.Color;
using Icons = ConnectApp.constants.Icons;
using RawImage = UnityEngine.UI.RawImage;
using Texture = Unity.UIWidgets.widgets.Texture;

namespace ConnectApp.components {
    public class CustomVideoPlayer : StatefulWidget {
        public CustomVideoPlayer(
            string url,
            Key key = null
        ) : base(key) {
            D.assert(url != null);
            this.url = url;
        }

        public readonly string url;

        public override State createState() {
            return new _CustomVideoPlayerState();
        }
    }

    public class _CustomVideoPlayerState : State<CustomVideoPlayer> {
        private VideoPlayer _player = null;
        private RenderTexture _texture = null;
        
        private RawImage image;
        private bool isPaused = false;

        public override void initState() {
            base.initState();

            _texture = Resources.Load<RenderTexture>("ConnectAppRT");
            _player = _videoPlayer(widget.url);

            isPaused = false;
        }

        public override void dispose() {
            _player.Stop();
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            return new Container(
                child: new Stack(children: new List<Widget> {
                    new Texture(texture: _texture),
                    new Positioned(
                        bottom:0,
                        left:0,
                        right:0,
                        child:new Container(
                            height:44,
                            color:Color.fromRGBO(0,0,0,0.1f),
                            child:new Row(
                                mainAxisAlignment:MainAxisAlignment.spaceAround,
                                crossAxisAlignment:CrossAxisAlignment.center,
                                children:new List<Widget>
                                {
                                    new GestureDetector(
                                        child:new Container(
                                            color:CColors.Transparent,
                                            child:new Icon(Icons.replay,size:20,color:CColors.White)
                                        )
                                    ),
                                    new Text($"{DateConvert.formatTime((float)_player.time)}",style:CTextStyle.CaptionWhite),
                                    
                                    new Text($"{DateConvert.formatTime(_player.frameCount / _player.frameRate)}",style:CTextStyle.CaptionWhite),
                                    new GestureDetector(
                                        child:new Container(
                                            color:CColors.Transparent,
                                            child:new Icon(Icons.favorite,size:20,color:CColors.White)
                                        )
                                    )
                                })
                            )
                        )
                })
            );
        }

        private VideoPlayer _videoPlayer(string url) {
            var player = VideoPlayerManager.instance.player;
            player.url = url;
            player.renderMode = VideoRenderMode.RenderTexture;
            player.source = VideoSource.Url;
            player.audioOutputMode = VideoAudioOutputMode.AudioSource;
            player.targetTexture = _texture;
            player.isLooping = false;
            player.playOnAwake = true;
            player.aspectRatio = VideoAspectRatio.FitOutside;
            player.sendFrameReadyEvents = true;
            player.prepareCompleted += Prepared;
            player.Prepare();
            return player;
        }

        void Prepared(VideoPlayer player)
        {
            player.Play();
        }

    }
}