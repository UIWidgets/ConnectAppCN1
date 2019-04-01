using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class TeamMapAction : BaseAction {
        public Dictionary<string, Team> teamMap;
    }
}