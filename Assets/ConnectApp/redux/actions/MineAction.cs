namespace ConnectApp.redux.actions {
    public class FetchMyFutureEventsAction : RequestAction {
        public int pageNumber = 1;
    }

    public class FetchMyFutureEventsSuccessAction : BaseAction {
    }

    public class FetchMyPastEventsAction : RequestAction {
        public int pageNumber = 1;
    }

    public class FetchMyPastEventsSuccessAction : BaseAction {
    }
}