using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.redux.actions {
    public class UserMapAction : BaseAction {
        public Dictionary<string, User> userMap;
    }
}