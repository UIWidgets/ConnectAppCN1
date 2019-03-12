namespace ConnectApp.redux.actions {
    public class ReportItemAction : RequestAction {
        public string itemId;
        public string itemType;
        public string reportContext;
    }

    public class ReportItemSuccessAction : BaseAction {
    }
}