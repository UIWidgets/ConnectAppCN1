using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class PlaceState {
        public Dictionary<string, Place> placeDict { get; set; }
    }
}