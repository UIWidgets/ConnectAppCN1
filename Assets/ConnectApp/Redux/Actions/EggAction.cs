namespace ConnectApp.redux.actions {
    public class InitEggsAction : BaseAction {
        public bool firstEgg;
    }

    public class ScanEnabledAction : BaseAction {
        public bool scanEnabled;
    }

    public class NationalDayEnabledAction : BaseAction {
        public bool nationalDayEnabled;
    }
}