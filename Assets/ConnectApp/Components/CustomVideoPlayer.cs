using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Video;
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

            _texture = new RenderTexture(Screen.width,Screen.height*9/16,24);
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
                            child:new Row(
                                children:new List<Widget>
                                {
                                    new Text($"{DateConvert.formatTime(_player.frameCount / _player.frameRate)}")
                                })
                            )),
                    Positioned.fill(
                        child: isPaused
                            ? new GestureDetector(
                                onTap: () => {
                                    _player.Play();
                                    setState(() => { isPaused = false; });
                                },
                                child: new Icon(Icons.play_arrow, null, 64, CColors.White)
                            )
                            : new GestureDetector(
                                onTap: () => {
                                    _player.Pause();
                                    setState(() => { isPaused = true; });
                                },
                                child: new Container(
                                    color: CColors.Transparent)
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
            player.aspectRatio = VideoAspectRatio.FitOutside;
            player.prepareCompleted += Prepared;
            player.sendFrameReadyEvents = true;
            player.Prepare();
            return player;
        }

        void Prepared(VideoPlayer player)
        {
            
            player.Play();
        }

//            ｛
//        //把图像赋给RawImage
//        image.texture = p.texture;
//        //帧数/帧速率=总时长    如果是本地直接赋值的视频，我们可以通过VideoClip.length获取总时长
//        sliderVideo.maxValue = vPlayer.frameCount / vPlayer.frameRate;
//        time = sliderVideo.maxValue;
//        hour = (int)time / 60;
//        mint = (int)time % 60;
//        text_Count.text = string.Format("/  ｛0:D2｝:｛1:D2｝", hour, mint);
//        sliderVideo.onValueChanged.AddListener(delegate ｛ ChangeVideo(sliderVideo.value); ｝);
//        player.Play();
//        ｝
    }
}