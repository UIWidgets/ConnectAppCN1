using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class PlaceMapAction : BaseAction {
        public Dictionary<string, Place> placeMap;
    }
}