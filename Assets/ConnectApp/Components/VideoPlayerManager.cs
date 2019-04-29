using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ConnectApp.components {
    public class VideoPlayerManager {
        private VideoPlayerManager() {
        }

        public static readonly VideoPlayerManager instance = new VideoPlayerManager();

        public VideoPlayer player { get; set; }


        public void initPlayer(GameObject gameObject)
        {
            if (gameObject.GetComponent<VideoPlayer>())
            {
                instance.player = gameObject.GetComponent<VideoPlayer>();
            }
            else
            {
                instance.player = gameObject.AddComponent<VideoPlayer>();
            }
        }

    }
}