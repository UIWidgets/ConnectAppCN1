namespace ConnectApp.redux.actions {
    public class FetchMyFutureEventsAction : RequestAction {
        public int pageNumber = 0;
    }

    public class FetchMyFutureEventsSuccessAction : BaseAction {
    }

    public class FetchMyPastEventsAction : RequestAction {
        public int pageNumber = 0;
    }

    public class FetchMyPastEventsSuccessAction : BaseAction {
    }
}