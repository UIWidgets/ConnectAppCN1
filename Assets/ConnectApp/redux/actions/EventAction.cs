using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class FetchEventsAction : RequestAction {
        public int pageNumber = 1;
    }

    public class FetchEventsSuccessAction : BaseAction {
        public List<IEvent> events;
    }

    public class FetchEventDetailAction : RequestAction {
        public string eventId;
    }

    public class FetchEventDetailSuccessAction : BaseAction {
        public IEvent eventObj;
    }

    public class JoinEventAction : RequestAction {
        public string eventId;
    }

    public class JoinEventSuccessAction : BaseAction {
    }

    public class ClearEventDetailAction : BaseAction {
    }
}