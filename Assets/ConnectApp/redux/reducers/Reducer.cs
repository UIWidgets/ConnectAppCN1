using ConnectApp.models;
using ConnectApp.redux.actions;

namespace ConnectApp.redux.reducers {
    public static class AppReducer {
        public static AppState Reduce(AppState state, object bAction) {
            switch (bAction) {
                case AddCountAction action: {
                    var num = (int) state.Get("count", 0);
                    state.Set("count", num + action.number);
                    break;
                }
                case ChangeEmailAction action: {
                    state.Set("login.email", action.email);
                    break;
                }
                case EventsRequestAction action: {
                    state.Set("event.loading", true);
                    break;
                }
                case EventsResponseAction action: {
                    state.Set("event.loading", false);
                    state.Set("event.events", action.events);
                    break;
                }
            }
            return state;
        }
    }
}