using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class AppState {
        public int Count { get; set; }
        public LoginState LoginState { get; set; }
        public List<IEvent> Events { get; set; }

        public LiveState LiveState { get; set; }

        public bool EventsLoading { get; set; }

        public static AppState initialState() {
            return new AppState {
                Count = 0,
                LoginState = new LoginState {
                    email = "empty",
                    isLoggedIn = false,
                    loading = false
                },
                Events = new List<IEvent>(),
                EventsLoading = false,
                LiveState = new LiveState {
                    loading = false,
                    showChatWindow = false,
                    openChatWindow = false
                }
            };
        }
    }
}