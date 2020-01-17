using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class MineState {
        public List<string> futureEventIds { get; set; }
        public List<string> pastEventIds { get; set; }
        public bool futureListLoading { get; set; }
        public bool pastListLoading { get; set; }
        public int futureEventTotal { get; set; }
        public int pastEventTotal { get; set; }
    }
}