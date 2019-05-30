using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.redux.actions {
    public class TeamMapAction : BaseAction {
        public Dictionary<string, Team> teamMap;
    }
}