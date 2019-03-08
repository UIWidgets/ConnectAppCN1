using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectApp.models {
    [Serializable]
    public class AppState {
        public int Count { get; set; }
        public LoginState loginState { get; set; }
        public ArticleState articleState { get; set; }
        public EventState eventState { get; set; }
        public UserState userState { get; set; }

        public static AppState initialState() {
//            var xx = PlayerPrefs.GetString();
//            var xxxx = new LoginState {
//                email = "empty",
//                isLoggedIn = false,
//                loading = false
//            };
            return new AppState {
                Count = PlayerPrefs.GetInt("count", 0),
                loginState = new LoginState {
                    email = "ods@ods.com",
                    password = "Welcome123",
                    loginInfo = new LoginInfo(),
                    isLoggedIn = false,
                    loading = false
                },
                articleState = new ArticleState {
                    Articles = new List<Article>(),
                    ArticleDetail = null,
                    ArticlesLoading = false,
                    ArticleDetailLoading = false
                },
                eventState = new EventState {
                    events = new List<string>(),
                    eventDict = new Dictionary<string, IEvent>(),
                    eventsLoading = false
                },
                userState = new UserState {
                    UserDict = new Dictionary<string, User>()
                }
            };
        }
    }
}