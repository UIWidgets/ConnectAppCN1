using System;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.utils;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Video;
using Color = Unity.UIWidgets.ui.Color;
using Icons = ConnectApp.constants.Icons;
using Texture = Unity.UIWidgets.widgets.Texture;
using Transform = Unity.UIWidgets.widgets.Transform;

namespace ConnectApp.components {

    public delegate void FullScreenCallback(bool isFullScreen);
    public delegate void FailureCallback();
    
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
            FullScreenCallback fullScreenCallback,
            float recordDuration = 0,
            Key key = null
        ) : base(key) {
            D.assert(url != null);
            this.url = url;
            this.recordDuration = recordDuration;
            this.context = context;
            this.topWidget = topWidget;
            this.fullScreenCallback = fullScreenCallback;
        }
        public readonly string url;
        public readonly float recordDuration;
        public readonly Widget topWidget;
        public readonly BuildContext context;
        public readonly FullScreenCallback fullScreenCallback;


        public override State createState() {
            return new _CustomVideoPlayerState();
        }
    }

    public class _CustomVideoPlayerState : SingleTickerProviderStateMixin<CustomVideoPlayer>
    {
        private VideoPlayer _player = null;
        private RenderTexture _texture = null;
        private PlayState _playState = PlayState.pause;
        private float _relative; //播放进度比例
        private bool _isFullScreen; //是否全屏
        private bool _isHiddenBar; //是否隐藏工具栏
        private bool _isFailure; //是否隐藏工具栏

        public override void initState()
        {
            base.initState();
            _texture = Resources.Load<RenderTexture>("ConnectAppRT");
            _player = _videoPlayer(widget.url);
        }

        public override void dispose()
        {
            _player.targetTexture.Release();
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

            var content = new AnimatedContainer(
                duration: TimeSpan.FromSeconds(0),
                curve: Curves.easeInOut,
                child: new Stack(children: new List<Widget>
                {
                    new GestureDetector(
                        onTap: () =>
                        {
                            if (_isFailure&&_isHiddenBar==false)
                            {
                                return;
                            }
                            _isHiddenBar = !_isHiddenBar;
                            if (_playState == PlayState.play & !_isHiddenBar)
                            {
                                _hiddenBar();
                            }

                            setState();
                        },
                        child: new Container(color:Colors.black,child:new Texture(texture: _texture)) 
                    ),
                    _isHiddenBar
                        ? new Positioned(child: new Container())
                        : new Positioned(top: 0, left: 0, right: 0, child: _isFullScreen
                            ? new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: new List<Widget>
                                {
                                    new GestureDetector(
                                        onTap: _setScreenOrientation,
                                        child: new Container(
                                            margin: EdgeInsets.only(left: 8, top: 8),
                                            child: new Icon(
                                                Icons.arrow_back,
                                                size: 28,
                                                color: CColors.White
                                            )
                                        )
                                    )
                                }
                            )
                            : widget.topWidget),
                    _isHiddenBar
                        ? new Positioned(child: new Container())
                        : new Positioned(
                            bottom: 0,
                            left: 0,
                            right: 0,
                            child: _isFailure?new Container(
                                height: 44,
                                padding:EdgeInsets.only(top:15,left:8),
                                color:Color.fromRGBO(0,0,0,0.2f),
                                child:new Text("视频播放失败",style:new TextStyle(
                                    fontSize: 15,
                                    fontFamily: "Roboto-Bold",
                                    color: CColors.White
                                ))) : new Container(
                                height: 44,
                                decoration: new BoxDecoration(gradient: new LinearGradient(
                                    colors: new List<Color>
                                    {
                                        Color.fromRGBO(0, 0, 0, 0),
                                        Color.fromRGBO(0, 0, 0, 0.5f)
                                    },
                                    begin: Alignment.topCenter,
                                    end: Alignment.bottomCenter
                                )),
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    crossAxisAlignment: CrossAxisAlignment.center,
                                    children: new List<Widget>
                                    {
                                        new GestureDetector(
                                            child: new Container(
                                                height: 44,
                                                width: 44,
                                                color: CColors.Transparent,
                                                child: new Icon(iconData, size: 24, color: CColors.White)
                                            ),
                                            onTap: _playOrPause
                                        ),
                                        new Container(margin: EdgeInsets.only(right: 8), child:
                                            new Text($"{DateConvert.formatTime((float) _player.time)}",
                                                style: CTextStyle.CaptionWhite)),
                                        new Expanded(
                                            child: new ProgressBar(_relative,
                                                changeCallback: relative =>
                                                {
                                                    _relative = relative;
                                                    _playState = PlayState.play;
                                                    _player.Play();
                                                    setState(() => { });
                                                }, onDragStart: () =>
                                                {
                                                    _playState = PlayState.pause;
                                                    _player.Pause();
                                                })),
                                        new Container(margin: EdgeInsets.only(left: 8, right: 8), child:
                                            new Text($"{DateConvert.formatTime(widget.recordDuration>0?widget.recordDuration:_player.frameCount/_player.frameRate)}",
                                                style: CTextStyle.CaptionWhite)),
                                        new GestureDetector(
                                            child: new Container(
                                                height: 44,
                                                width: 44,
                                                color: CColors.Transparent,
                                                child: new Icon(
                                                    _isFullScreen ? Icons.fullscreen_exit : Icons.fullscreen, size: 24,
                                                    color: CColors.White)
                                            ),
                                            onTap: _setScreenOrientation
                                        )
                                    })
                            )
                        )
                })
            );

            if (Application.platform == RuntimePlatform.Android)
            {
                return new AnimatedContainer(
                    duration: TimeSpan.FromMilliseconds(150),
                    width: _isFullScreen
                        ? MediaQuery.of(context).size.height * 16 / 9
                        : MediaQuery.of(context).size.width,
                    height: _isFullScreen
                        ? MediaQuery.of(context).size.height
                        : MediaQuery.of(context).size.width * 9 / 16,
                    child: content
                );
            }

            return new Container(
                width: _isFullScreen ? MediaQuery.of(context).size.height * 16 / 9 : MediaQuery.of(context).size.width,
                height: _isFullScreen ? MediaQuery.of(context).size.height : MediaQuery.of(context).size.width * 9 / 16,
                child: content
            );
        }

        private VideoPlayer _videoPlayer(string url)
        {
            var player = VideoPlayerManager.instance.getPlayer();
            var audioSource = VideoPlayerManager.instance.getAudioSource();
            player.url = url;
            player.renderMode = VideoRenderMode.RenderTexture;
            player.source = VideoSource.Url;
            player.audioOutputMode = VideoAudioOutputMode.AudioSource;
            player.SetTargetAudioSource(0, audioSource);
            player.playOnAwake = false;
            player.IsAudioTrackEnabled(0);
            player.targetTexture = _texture;
            player.isLooping = false;
            player.sendFrameReadyEvents = true;
            player.aspectRatio = VideoAspectRatio.Stretch;
            player.frameReady += (source, frameIndex) =>
            {
                using (WindowProvider.of(widget.context).getScope())
                {
                    Texture.textureFrameAvailable();
                    _relative = (float) frameIndex / source.frameCount;
                    Promise.Delayed(TimeSpan.FromMilliseconds(200)).Then(() =>
                    {
                        setState(() => { });
                    });
                }
            };    
            player.loopPointReached += _player =>
            {
                using (WindowProvider.of(widget.context).getScope())
                {
                    _relative = 0.0f;
                    _playState = PlayState.stop;
                    _player.Stop();
                    _player.frame = 0;
                    setState(() => { });
                }

            };
            player.errorReceived += _errorReceived;
            player.Prepare();
            player.Pause();
            return player;
        }

        private void _playOrPause()
        {
            if (_playState == PlayState.play)
            {
                _player.Pause();
                _playState = PlayState.pause;
            }
            else
            {
                _player.Play();
                _hiddenBar();
                _playState = PlayState.play;
            }
            setState(() => {}); 
        }
        private void _errorReceived(VideoPlayer player,string message)
        {
            using (WindowProvider.of(widget.context).getScope())
            {
                setState(() => { _isFailure = true;});
            }
        }

        private void _hiddenBar()
        {
            Promise.Delayed(TimeSpan.FromSeconds(5)).Then(() =>
            {
                setState(() => { _isHiddenBar = true; });   
            });
        }

        private void _setScreenOrientation()
        {
            _isFullScreen = !_isFullScreen;
            
            if (_isFullScreen)
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            }
            else
            {
                Screen.orientation = ScreenOrientation.Portrait;
            }
            
            if (widget.fullScreenCallback != null)
            {
                widget.fullScreenCallback(_isFullScreen);
            }
        }

    }
}