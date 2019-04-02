using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class MineState {
        public List<IEvent> futureEventsList { get; set; }
        public List<IEvent> pastEventsList { get; set; }
        public bool futureListLoading { get; set; }
        public bool pastListLoading { get; set; }
        public int futureEventTotal { get; set; }
        public int pastEventTotal { get; set; }
    }
}