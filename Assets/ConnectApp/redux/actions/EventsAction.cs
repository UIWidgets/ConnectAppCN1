using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class EventsAction : BaseAction {
    }

    public class FetchEventsAction : RequestAction {
        public int pageNumber;
    }

    public class FetchEventsSuccessAction : BaseAction {
        public List<IEvent> events;
    }
}