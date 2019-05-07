using System;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Video;
using Color = Unity.UIWidgets.ui.Color;
using Icons = ConnectApp.constants.Icons;
using Texture = Unity.UIWidgets.widgets.Texture;
using Transform = Unity.UIWidgets.widgets.Transform;

namespace ConnectApp.components {
    
    public delegate void FullScreenCallback(bool isFullScreen);

    enum PlayState
    {
        play,
        pause,
        stop
    }
    
    public class CustomVideoPlayer : StatefulWidget {
        public CustomVideoPlayer(
            string url,
            BuildContext context,
            Widget topWidget,
            Key key = null
        ) : base(key) {
            D.assert(url != null);
            this.url = url;
            this.context = context;
            this.topWidget = topWidget;
        }
        public readonly string url;
        public readonly Widget topWidget;
        public readonly BuildContext context;

        public override State createState() {
            return new _CustomVideoPlayerState();
        }
    }

    public class _CustomVideoPlayerState : State<CustomVideoPlayer> {
        private VideoPlayer _player = null;
        private RenderTexture _texture = null;
        private PlayState _playState;
        private float _relative;
        private bool _isFullScreen;
        private bool _isHiddenBar;

        public override void initState() {
            base.initState();

            _texture = Resources.Load<RenderTexture>("ConnectAppRT");
            _player = _videoPlayer(widget.url);
        }

        public override void dispose()
        {
            _player.Stop();
            VideoPlayerManager.instance.destroyPlayer();
            base.dispose();
        }

        public override Widget build(BuildContext context)
        {
            var iconData = Icons.pause;
            switch (_playState)
            {
                case PlayState.stop:
                    iconData = Icons.replay;
                    break;
                case PlayState.pause:
                    iconData = Icons.play_arrow;
                    break;
            }
            var content = new Container(
                child: new Stack(children: new List<Widget> {
                    
                    new GestureDetector(
                        onTap: () =>
                        {
                            setState(() => { _isHiddenBar = !_isHiddenBar; });   
                        },
                        child:new Texture(texture: _texture)
                    ),
                    _isHiddenBar?new Positioned(child:new Container()):
                        new Positioned(top:0,left:0,right:0,child:_isFullScreen? new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget> {
                                new CustomButton(
                                    onPressed: fullScreen,
                                    child: new Icon(
                                        Icons.arrow_back,
                                        size: 28,
                                        color:  CColors.White
                                    )
                                )
                            }
                        ):widget.topWidget),
                    _isHiddenBar?new Positioned(child:new Container()):new Positioned(
                        bottom:0,
                        left:0,
                        right:0,
                        child:new Container(
                            height:44,
                            color:Color.fromRGBO(0,0,0,0.1f),
                            child:new Row(
                                mainAxisAlignment:MainAxisAlignment.spaceBetween,
                                crossAxisAlignment:CrossAxisAlignment.center,
                                children:new List<Widget>
                                {
                                    new GestureDetector(
                                        child:new Container(
                                            height:24,
                                            width:24,
                                            margin:EdgeInsets.only(left:8,right:8),
                                            color:CColors.Transparent,
                                            child:new Icon(iconData,size:20,color:CColors.White)
                                        ),
                                        onTap: playOrPause
                                    ),
                                    new Container(margin:EdgeInsets.only(right:8),child:
                                        new Text($"{DateConvert.formatTime((float)_player.time)}",style:CTextStyle.CaptionWhite)),
                                    new Expanded(
                                        child:new ProgressBar(_relative,
                                        changeCallback: relative =>
                                        {
                                            _relative = relative;
                                            _player.time = relative * (_player.frameCount / _player.frameRate);
                                            _player.Play();
                                        },onDragStart: () =>
                                        {
                                           _player.Pause();
                                        })),
                                    new Container(margin:EdgeInsets.only(left:8,right:8),child:
                                        new Text(_playState == PlayState.stop?"--:--":$"{DateConvert.formatTime(_player.frameCount / _player.frameRate)}",style:CTextStyle.CaptionWhite)),
                                    new GestureDetector(
                                        child:new Container(
                                            height:24,
                                            width:24,
                                            margin:EdgeInsets.only(right:8),
                                            color:CColors.Transparent,
                                            child:new Icon(_isFullScreen?Icons.fullscreen_exit:Icons.fullscreen,size:20,color:CColors.White)
                                        ),
                                        onTap: fullScreen
                                    )
                                })
                            )
                        )
                })
            );
            if (_isFullScreen)
            {
                return new Container(
                    child: new RotatedBox(
                        quarterTurns: 1,
                        child: new Container(
                            constraints: new BoxConstraints(MediaQuery.of(context).size.height),
                            width: MediaQuery.of(context).size.height,
                            height: MediaQuery.of(context).size.width,
                            child: content
                        )
                    )
                );  
            }
            return new Container(
                height:MediaQuery.of(context).size.width*9/16,
                child:content);
            
        }

        private VideoPlayer _videoPlayer(string url) {
            var player = VideoPlayerManager.instance.getPlayer();
            player.url = url;
            player.renderMode = VideoRenderMode.RenderTexture;
            player.source = VideoSource.Url;
            player.audioOutputMode = VideoAudioOutputMode.AudioSource;
            player.targetTexture = _texture;
            player.isLooping = false;
            player.sendFrameReadyEvents = true;
            player.aspectRatio = VideoAspectRatio.FitInside;
            player.prepareCompleted += OnPrepareFinished;
            player.frameReady += (source, frameIndex) =>
            {
                using (WindowProvider.of(widget.context).getScope())
                {
                    Texture.textureFrameAvailable();
                    _relative =(float)frameIndex/source.frameCount;
                    setState(() => { });
                }
            };
            player.loopPointReached += (_player)=>
            {
                using (WindowProvider.of(widget.context).getScope())
                {
                    _relative = 0.0f;
                    _playState = PlayState.stop;
                    _player.Stop();
                    setState(() => {});
                }
                
            };
            player.Prepare();
            return player;
        }
        void OnPrepareFinished(VideoPlayer player)
        {
            _playState = PlayState.play;
            player.Play();
        }

        void playOrPause()
        {
            if (_playState == PlayState.play)
            {
                _player.Pause();
                _playState = PlayState.pause;
            }
            else
            {
                _player.Play();
                _playState = PlayState.play;
            }
            setState(() => {}); 
        }

        void fullScreen()
        {
            _isFullScreen = !_isFullScreen;
            setState(() => { });
        }

    }
}