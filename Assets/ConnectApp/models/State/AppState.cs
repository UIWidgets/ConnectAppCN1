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
        public NotificationState notificationState { get; set; }
        public UserState userState { get; set; }
        public MineState mineState { get; set; }
        public MessageState messageState { get; set; }

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
                    articleList = new List<string>(),
                    articleDict = new Dictionary<string, Article>(),
                    articlesLoading = false,
                    articleDetailLoading = false,
                    detailId = null,
                    articleDetail = null
                },
                eventState = new EventState {
                    events = new List<string>(),
                    eventDict = new Dictionary<string, IEvent>(),
                    eventsLoading = false,
                    detailId = null
                },
                notificationState = new NotificationState {
                    loading = false,
                    notifications = new List<Notification>()
                },
                userState = new UserState {
                    UserDict = new Dictionary<string, User>()
                },
                mineState = new MineState {
                    futureEventsList = new List<string>(),
                    pastEventsList = new List<string>(),
                    futureListLoading = false,
                    pastListLoading = false
                },
                messageState = new MessageState
                {
                    channelMessageDict = new Dictionary<string, Dictionary<string, Message>>(),
                    channelMessageList = new Dictionary<string, List<string>>()
                }
            };
        }
    }
}