using System;

namespace ConnectApp.Models.State {
    [Serializable]
    public class SettingState {
        public bool hasReviewUrl { get; set; }
        public string reviewUrl { get; set; }
        public bool vibrate { get; set; }
    }
}