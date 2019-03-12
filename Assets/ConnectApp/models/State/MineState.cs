using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class MineState {
        public List<string> futureEventsList { get; set; }
        public List<string> pastEventsList { get; set; }
        public bool futureListLoading { get; set; }
        public bool pastListLoading { get; set; }
    }
}