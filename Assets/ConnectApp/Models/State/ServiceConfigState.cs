using System;

namespace ConnectApp.Models.State {
    [Serializable]
    public class ServiceConfigState {
        public bool showFirstEgg { get; set; }
        public bool scanEnabled { get; set; }
        public bool nationalDayEnabled { get; set; }
    }
}