using ConnectApp.models;
using ConnectApp.redux.actions;

namespace ConnectApp.redux.reducers {
    public static class AppReducer {
        public static AppState Reduce(AppState state, object bAction) {
            switch (bAction) {
                case AddCountAction action: {
                    state.Count += action.number;
                    break;
                }
                case ChangeEmailAction action: {
                    state.Login.email = action.email;
                    break;
                }
                case EventsRequestAction action: {
                    state.EventsLoading = true;
                    break;
                }
                case EventsResponseAction action: {
                    state.EventsLoading = false;
                    state.Events = action.events;
                    break;
                }
            }

            return state;
        }
    }
}