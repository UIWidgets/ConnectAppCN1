using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.redux.actions {
    public class PlaceMapAction : BaseAction {
        public Dictionary<string, Place> placeMap;
    }
}