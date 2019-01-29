using System;
using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class EventsAction: BaseAction {
        
    }
    
    public class EventsRequestAction: RequestAction {
        public int pageNumber;
    }
    
    [Serializable]
    public class EventsResponseAction: ResponseAction {
        public List<IEvent> events;
    }
}