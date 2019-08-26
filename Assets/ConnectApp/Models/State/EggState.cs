using System;

namespace ConnectApp.Models.State {
    [Serializable]
    public class EggState {
        public bool showFirst { get; set; }
        public bool scanEnabled { get; set; }
    }
}