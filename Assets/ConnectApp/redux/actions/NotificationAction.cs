using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class FetchNotificationsAction : RequestAction {
        public int pageNumber;
    }

    public class FetchNotificationsSuccessAction : BaseAction {
        public int pageNumber;
        public Notification notificationObj;
    }
}