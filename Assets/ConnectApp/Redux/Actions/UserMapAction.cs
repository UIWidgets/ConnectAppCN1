using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class UserMapAction : BaseAction {
        public Dictionary<string, User> userMap;
    }
}