using System;

namespace ConnectApp.models {
    [Serializable]
    public class SettingState {
        public bool hasReviewUrl { get; set; }
        public bool fetchReviewUrlLoading { get; set; }
        public string reviewUrl { get; set; }
    }
}