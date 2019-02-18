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
                    state.LoginState.email = action.email;
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
                case LiveRequestAction action: {
                    state.LiveState.loading = true;

                    break;
                }
                case LiveResponseAction action: {
                    state.LiveState.loading = false;
                    state.LiveState.liveInfo = action.liveInfo;
                    break;
                }
                case ChatWindowShowAction action: {
                    state.LiveState.showChatWindow = action.show;
                    break;
                }
                case ChatWindowStatusAction action: {
                    state.LiveState.openChatWindow = action.status;
                    break;
                }
                case NavigatorToLiveAction action: {
                    state.LiveState.detailId = action.eventId;
                    break;
                }
                case ClearLiveInfoAction action: {
                    state.LiveState.liveInfo = null;
                    break;
                }
            }

            return state;
        }
    }
}