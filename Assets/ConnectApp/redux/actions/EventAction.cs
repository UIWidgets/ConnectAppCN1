using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class FetchEventsAction : RequestAction {
        public int pageNumber;
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

    public class ClearEventDetailAction : BaseAction {
    }
}