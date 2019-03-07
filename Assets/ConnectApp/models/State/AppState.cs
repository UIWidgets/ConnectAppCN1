using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectApp.models {
    [Serializable]
    public class AppState {
        public int Count { get; set; }
        public LoginState LoginState { get; set; }
        public ArticleState ArticleState { get; set; }
        public List<IEvent> Events { get; set; }
        public LiveState LiveState { get; set; }
        public bool EventsLoading { get; set; }

        public static AppState initialState() {
//            var xx = PlayerPrefs.GetString();
//            var xxxx = new LoginState {
//                email = "empty",
//                isLoggedIn = false,
//                loading = false
//            };
            return new AppState {
                Count = PlayerPrefs.GetInt("count", 0),
                LoginState = new LoginState {
                    email = "ods@ods.com",
                    password = "Welcome123",
                    loginInfo = new LoginInfo(),
                    isLoggedIn = false,
                    loading = false
                },
                ArticleState = new ArticleState {
                    ArticleList = new List<string>(),
                    ArticleDict = new Dictionary<string, Article>(),
                    ArticlesLoading = false,
                    ArticleDetailLoading = false
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