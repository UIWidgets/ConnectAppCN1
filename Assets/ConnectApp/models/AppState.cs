using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class AppState {
        public int Count { get; set; }
        public Login Login { get; set; }
        public List<IEvent> Events { get; set; }
        
        public bool EventsLoading { get; set; }
        
        public static AppState initialState() {
            return new AppState {
                Count = 0,
                Login = new Login {
                    email = "empty",
                    isLoggedIn = false,
                    loading = false
                },
                Events = new List<IEvent>(),
                EventsLoading = false
            };
        }
    }
}