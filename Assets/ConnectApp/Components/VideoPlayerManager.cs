using UnityEngine.Video;

namespace ConnectApp.components {
    public class VideoPlayerManager {
        private VideoPlayerManager() {
        }

        public static readonly VideoPlayerManager instance = new VideoPlayerManager();

        public VideoPlayer player { get; set; }
    }
}