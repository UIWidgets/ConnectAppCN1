using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Video;
using Color = Unity.UIWidgets.ui.Color;
using Texture = Unity.UIWidgets.widgets.Texture;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace ConnectApp.Components {
    public delegate void FullScreenCallback(bool isFullScreen);

    public enum PlayState {
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
            bool isAutoPlay = false,
            Key key = null
        ) : base(key: key) {
            D.assert(url != null);
            this.url = url;
            this.recordDuration = recordDuration;
            this.context = context;
            this.topWidget = topWidget;
            this.fullScreenCallback = fullScreenCallback;
            this.isAutoPlay = isAutoPlay;
        }

        public readonly string url;
        public readonly float recordDuration;
        public readonly Widget topWidget;
        public readonly BuildContext context;
        public readonly FullScreenCallback fullScreenCallback;
        public readonly bool isAutoPlay;


        public override State createState() {
            return new _CustomVideoPlayerState();
        }
    }

    public class _CustomVideoPlayerState : State<CustomVideoPlayer> {
        VideoPlayer _player;
        RenderTexture _texture;
        PlayState _playState = PlayState.pause;
        float _relative; //播放进度比例
        bool _isFullScreen; //是否全屏
        bool _isReadyHiddenBar; //在倒计时隐藏bar
        bool _isHiddenBar; //是否隐藏工具栏
        bool _isFailure; //加载失败
        bool _isLoaded; //加载完成，用来隐藏loading
        string _pauseVideoPlayerSubId; //收到通知暂停播放
        string _fullScreenSubId;
        string _changeOrientationSubId;
        Timer m_Timer;
        const int _toolBarHeight = 64;

        public override void initState() {
            base.initState();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            VideoPlayerManager.instance.isRotation = true;
            this._texture = Resources.Load<RenderTexture>("texture/ConnectAppRT");
            this._player = this._videoPlayer(this.widget.url);
            this._pauseVideoPlayerSubId = EventBus.subscribe(EventBusConstant.pauseVideoPlayer, args => {
                if (this._player) {
                    this._player.Pause();
                    this._isHiddenBar = false;
                    this._playState = PlayState.pause;
                    this.cancelTimer();
                    this.setState(() => { });
                }
            });
            this._fullScreenSubId = EventBus.subscribe(EventBusConstant.fullScreen, args => {
                this._isFullScreen = (bool) args[0];
                this._setScreenOrientation();
            });
            this._changeOrientationSubId = EventBus.subscribe(EventBusConstant.changeOrientation, args => {
                var orientation = (ScreenOrientation) args[0];
                this._changeOrientation(orientation);
            });
        }

        public override void dispose() {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            VideoPlayerManager.instance.isRotation = false;
            EventBus.unSubscribe(EventBusConstant.pauseVideoPlayer, this._pauseVideoPlayerSubId);
            EventBus.unSubscribe(EventBusConstant.fullScreen, this._fullScreenSubId);
            EventBus.unSubscribe(EventBusConstant.changeOrientation, this._changeOrientationSubId);
            this._player.targetTexture.Release();
            this._player.Stop();
            VideoPlayerManager.destroyPlayer();
            this.m_Timer?.cancel();
            this.m_Timer?.Dispose();
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            var iconData = Icons.pause;
            switch (this._playState) {
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
                        onTap: () => {
                            if (this._isFailure && this._isHiddenBar == false) {
                                return;
                            }

                            this._isHiddenBar = !this._isHiddenBar;
                            this._isReadyHiddenBar = false;
                            this.setState();
                            this.cancelTimer();
                        },
                        child: new Container(color: CColors.Black, child: new Texture(texture: this._texture))
                    ),
                    this._isLoaded
                        ? new Align()
                        : new Align(
                            alignment: Alignment.center,
                            child: new CustomActivityIndicator(loadingColor: LoadingColor.white)
                        ),
                    this._isHiddenBar
                        ? new Positioned(new Container())
                        : new Positioned(top: 0, left: 0, right: 0, child: this._isFullScreen
                            ? new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: new List<Widget> {
                                    new GestureDetector(
                                        onTap: this._setScreenOrientation,
                                        child: new Container(
                                            margin: EdgeInsets.only(8,
                                                Application.platform == RuntimePlatform.IPhonePlayer ? 20 : 8),
                                            child: new Icon(
                                                Icons.arrow_back,
                                                size: 28,
                                                color: CColors.White
                                            )
                                        )
                                    )
                                }
                            )
                            : this.widget.topWidget),
                    this._isHiddenBar
                        ? new Positioned(new Container())
                        : new Positioned(
                            bottom: 0,
                            left: 0,
                            right: 0,
                            child: this._isFailure
                                ? new Container(
                                    height: _toolBarHeight,
                                    padding: EdgeInsets.only(top: 0, left: 8),
                                    color: Color.fromRGBO(0, 0, 0, 0.2f),
                                    child: new Row(children: new List<Widget> {
                                        new GestureDetector(
                                            child: new Container(
                                                height: _toolBarHeight,
                                                width: _toolBarHeight,
                                                color: CColors.Transparent,
                                                child: new Icon(Icons.replay, size: 24, color: CColors.White)
                                            ),
                                            onTap: this._reloadVideo
                                        ),
                                        new Text("视频播放失败", style: new TextStyle(
                                            fontSize: 15,
                                            fontFamily: "Roboto-Bold",
                                            color: CColors.White
                                        ))
                                    }))
                                : new Container(
                                    height: _toolBarHeight,
                                    decoration: new BoxDecoration(gradient: new LinearGradient(
                                        colors: new List<Color> {
                                            Color.fromRGBO(0, 0, 0, 0),
                                            Color.fromRGBO(0, 0, 0, 0.5f)
                                        },
                                        begin: Alignment.topCenter,
                                        end: Alignment.bottomCenter
                                    )),
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                        crossAxisAlignment: CrossAxisAlignment.center,
                                        children: new List<Widget> {
                                            new GestureDetector(
                                                child: new Container(
                                                    height: _toolBarHeight,
                                                    width: _toolBarHeight,
                                                    color: CColors.Transparent,
                                                    child: new Icon(iconData, size: 24, color: CColors.White)
                                                ),
                                                onTap: this._playOrPause
                                            ),
                                            new Container(margin: EdgeInsets.only(right: 8), child:
                                                new Text($"{DateConvert.formatTime((float) this._player.time)}",
                                                    style: CTextStyle.CaptionWhite)),
                                            new Expanded(
                                                child: new ProgressBar(this._relative,
                                                    changeCallback: relative => {
                                                        this._relative = relative;
                                                        if (this.widget.recordDuration > 0) {
                                                            this._player.time = relative * this.widget.recordDuration;
                                                        }
                                                        else {
                                                            this._player.time =
                                                                relative * this._player.frameCount /
                                                                this._player.frameRate;
                                                        }

                                                        this.cancelTimer();
                                                        this._isLoaded = false;
                                                        this._player.Pause();
                                                        this.setState(() => { });
                                                    }, onDragStart: () => {
                                                        this.cancelTimer();
                                                        this._isLoaded = false;
                                                        this._player.Pause();
                                                        this.setState(() => { });
                                                    })),
                                            new Container(margin: EdgeInsets.only(left: 8, right: 8), child:
                                                new Text(
                                                    $"{DateConvert.formatTime(this.widget.recordDuration > 0 ? this.widget.recordDuration : this._player.frameCount / this._player.frameRate)}",
                                                    style: CTextStyle.CaptionWhite)),
                                            new GestureDetector(
                                                child: new Container(
                                                    height: _toolBarHeight,
                                                    width: _toolBarHeight,
                                                    color: CColors.Transparent,
                                                    child: new Icon(
                                                        this._isFullScreen ? Icons.fullscreen_exit : Icons.fullscreen,
                                                        size: 24,
                                                        color: CColors.White)
                                                ),
                                                onTap: this._setScreenOrientation
                                            )
                                        })
                                )
                        )
                })
            );
            return new Container(
                width: this._isFullScreen
                    ? MediaQuery.of(context).size.height * 16 / 9
                    : MediaQuery.of(context).size.width,
                height: this._isFullScreen
                    ? MediaQuery.of(context).size.height
                    : MediaQuery.of(context).size.width * 9 / 16,
                child: content
            );
        }

        VideoPlayer _videoPlayer(string url) {
            var player = VideoPlayerManager.instance.getPlayer();
            var audioSource = VideoPlayerManager.instance.getAudioSource();
            player.url = url;
            player.renderMode = VideoRenderMode.RenderTexture;
            player.source = VideoSource.Url;
            player.audioOutputMode = VideoAudioOutputMode.AudioSource;
            player.SetTargetAudioSource(0, audioSource);
            player.playOnAwake = false;
            player.IsAudioTrackEnabled(0);
            player.targetTexture = this._texture;
            player.isLooping = false;
            player.sendFrameReadyEvents = true;
            player.aspectRatio = VideoAspectRatio.Stretch;
            player.prepareCompleted += this.prepareCompleted;
            player.frameReady += (source, frameIndex) => {
                using (WindowProvider.of(this.widget.context).getScope()) {
                    _pauseAudioSession();
                    Texture.textureFrameAvailable();
                    if (this._relative * source.frameCount < frameIndex || frameIndex == 0) {
                        this._isLoaded = true;
                        if (!this._isHiddenBar && !this._isReadyHiddenBar) {
                            this._isReadyHiddenBar = true;
                            this._hiddenBar();
                        }
                    }

                    this._relative = (float) frameIndex / source.frameCount;
                    this._isFailure = false;
                    if (this._playState == PlayState.play) {
                        this._player.Play();
                    }

                    if (frameIndex == 0) {
                        Promise.Delayed(TimeSpan.FromMilliseconds(200)).Then(() => {
                            if (this.mounted) {
                                this.setState(() => {});
                            }
                        });
                    }
                    else {
                        this.setState(() => { });
                    }
                }
            };
            player.loopPointReached += _player => {
                using (WindowProvider.of(this.widget.context).getScope()) {
                    this._relative = 0.0f;
                    this._playState = PlayState.stop;
                    _player.Stop();
                    _player.frame = 0;
                    this.cancelTimer();
                    this._isHiddenBar = false;
                    this._isReadyHiddenBar = false;
                    this.setState(() => { });
                }
            };
            player.errorReceived += this._errorReceived;
            player.Prepare();
            player.Pause();
            return player;
        }

        void _playOrPause() {
            if (this._playState == PlayState.play) {
                this._player.Pause();
                this._playState = PlayState.pause;
            }
            else {
                this._player.Play();
                this._playState = PlayState.play;
            }

            this.cancelTimer();
            this.setState(() => { });
        }

        void prepareCompleted(VideoPlayer player) {
            if (this.widget.isAutoPlay) {
                this._playState = PlayState.play;
                player.Play();
            }
        }

        void _errorReceived(VideoPlayer player, string message) {
            using (WindowProvider.of(this.widget.context).getScope()) {
                this.cancelTimer();
                this.setState(() => {
                    this._isFailure = true;
                    this._isLoaded = true;
                });
            }
        }


        void _hiddenBar() {
            this.m_Timer = Window.instance.run(TimeSpan.FromSeconds(5), () => {
                if (this._playState == PlayState.play) {
                    this.setState(() => {
                        this._isHiddenBar = true;
                        this._isReadyHiddenBar = false;
                    });
                }
            });
        }

        void cancelTimer() {
            this.m_Timer?.cancel();
            this._isReadyHiddenBar = false;
        }

        void _reloadVideo() {
            this._player.Prepare();
            this.setState(() => {
                this._isLoaded = false;
                this._isFailure = false;
            });
        }

        void _setScreenOrientation() {
            this.cancelTimer();
            using (WindowProvider.of(this.widget.context).getScope()) {
                if (!this._isFullScreen) {
                    VideoPlayerManager.instance.lockPortrait = true;
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                    this._isFullScreen = true;
                }
                else {
                    VideoPlayerManager.instance.lockLandscape = true;
                    Screen.orientation = ScreenOrientation.Portrait;
                    this._isFullScreen = false;
                }

                if (this.widget.fullScreenCallback != null) {
                    this.widget.fullScreenCallback(this._isFullScreen);
                }
            }
        }

        void _changeOrientation(ScreenOrientation orientation) {
            if (!_isOpenSensor()) {
                return;
            }

            this.cancelTimer();
            using (WindowProvider.of(this.widget.context).getScope()) {
                if (orientation == ScreenOrientation.Portrait) {
                    Screen.orientation = ScreenOrientation.Portrait;
                    this._isFullScreen = false;
                }
                else {
                    Screen.orientation = orientation;
                    this._isFullScreen = true;
                }

                if (this.widget.fullScreenCallback != null) {
                    this.widget.fullScreenCallback(this._isFullScreen);
                }
            }
        }

        static void _pauseAudioSession() {
            if (!Application.isEditor) {
                pauseAudioSession();
            }
        }

        static bool _isOpenSensor() {
            if (Application.platform == RuntimePlatform.Android) {
                return isOpenSensor();
            }

            return true;
        }
#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void pauseAudioSession();

        [DllImport("__Internal")]
        static extern bool isOpenSensor();

#elif UNITY_ANDROID
        static AndroidJavaClass _plugin;

        static AndroidJavaClass Plugin() {
            if (_plugin == null) {
                _plugin = new AndroidJavaClass("com.unity3d.unityconnect.plugins.CommonPlugin");
            }

            return _plugin;
        }

        static void pauseAudioSession() {
            Plugin().CallStatic("pauseAudioSession");
        }

        static bool isOpenSensor() {
            return Plugin().CallStatic<bool>("isOpenSensor");
        }
#else
        static void pauseAudioSession() {
        }
        static bool isOpenSensor() {
            return false;
        }
#endif
    }
}