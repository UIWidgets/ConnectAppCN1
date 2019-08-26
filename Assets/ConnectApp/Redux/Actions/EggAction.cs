using System.Collections.Generic;

namespace ConnectApp.redux.actions {
    public class InitEggsAction : BaseAction {
        public List<bool> showEggs;
    }

    public class ScanEnabledAction : BaseAction {
        public bool scanEnabled;
    }
}