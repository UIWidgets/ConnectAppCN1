using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class PlaceState {
        public Dictionary<string, Place> placeDict { get; set; }
    }
}