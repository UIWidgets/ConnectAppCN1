using System;

namespace ConnectApp.models {
    [Serializable]
    public class SettingState {
        public bool reviewLoading { get; set; }
        public string reviewUrl { get; set; }
    }
}