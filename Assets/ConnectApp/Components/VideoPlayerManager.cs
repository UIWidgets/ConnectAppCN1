using UnityEngine;
using UnityEngine.Video;

namespace ConnectApp.Components {
    public class VideoPlayerManager {
        VideoPlayerManager() {
        }

        public static readonly VideoPlayerManager instance = new VideoPlayerManager();

        public bool lockPortrait;

        public bool lockLandscape;

        public bool isRotation;


        VideoPlayer player { get; set; }

        AudioSource audioSource { get; set; }


        GameObject gameObject { get; set; }

        public VideoPlayer getPlayer() {
            if (this.gameObject.GetComponent<VideoPlayer>()) {
                instance.player = this.gameObject.GetComponent<VideoPlayer>();
            }
            else {
                instance.player = this.gameObject.AddComponent<VideoPlayer>();
            }

            return instance.player;
        }

        public AudioSource getAudioSource() {
            if (this.gameObject.GetComponent<AudioSource>()) {
                instance.audioSource = this.gameObject.GetComponent<AudioSource>();
            }
            else {
                instance.audioSource = this.gameObject.AddComponent<AudioSource>();
            }

            return instance.audioSource;
        }

        public static void destroyPlayer() {
            Object.Destroy(instance.player);
            Object.Destroy(instance.audioSource);
        }

        public void initPlayer(GameObject gameObject) {
            this.gameObject = gameObject;
        }
    }
}