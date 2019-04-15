using System;
using System.Collections.Generic;
using ConnectApp.utils;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.models {
    [Serializable]
    public class AppState {
        public int Count { get; set; }
        public LoginState loginState { get; set; }
        public ArticleState articleState { get; set; }
        public EventState eventState { get; set; }
        public PopularSearchState popularSearchState { get; set; }
        public SearchState searchState { get; set; }
        public NotificationState notificationState { get; set; }
        public UserState userState { get; set; }
        public TeamState teamState { get; set; }
        public PlaceState placeState { get; set; }
        public MineState mineState { get; set; }
        public MessageState messageState { get; set; }
        public SettingState settingState { get; set; }
        public ReportState reportState { get; set; }

        public static AppState initialState() {
            
            return new AppState {
                Count = PlayerPrefs.GetInt("count", 0),
                loginState = new LoginState {
                    email = "",
                    password = "",
                    loginInfo = UserInfoManager.initUserInfo(),
                    isLoggedIn = UserInfoManager.isLogin(),
                    loading = false
                },
                articleState = new ArticleState {
                    articleList = new List<string>(),
                    articleDict = new Dictionary<string, Article>(),
                    articlesLoading = false,
                    articleDetailLoading = false,
                    articleTotal = 0,
                    pageNumber = 0,
                    articleHistory = HistoryManager.articleHistoryList()
                },
                eventState = new EventState {
                    ongoingEvents = new List<string>(),
                    eventsDict = new Dictionary<string, IEvent>(),
                    ongoingEventTotal = 0,
                    completedEvents = new List<string>(),
                    completedEventTotal = 0,
                    pageNumber = 1,
                    completedPageNumber = 1,
                    eventsOngoingLoading = false,
                    eventsCompletedLoading = false,
                    eventHistory = HistoryManager.eventHistoryList(),
                    channelId = ""
                },
                popularSearchState = new PopularSearchState {
                    popularSearchs = new List<PopularSearch>()
                },
                searchState = new SearchState {
                    loading = false,
                    keyword = "",
                    searchArticles = new List<Article>(),
                    currentPage = 0,
                    pages = new List<int>(),
                    searchHistoryList = HistoryManager.searchHistoryList(),
                },
                notificationState = new NotificationState {
                    loading = false,
                    notifications = new List<Notification>()
                },
                userState = new UserState {
                    userDict = UserInfoManager.initUserDict()
                },
                teamState = new TeamState {
                    teamDict = new Dictionary<string, Team>()
                },
                placeState = new PlaceState {
                    placeDict = new Dictionary<string, Place>()
                },
                mineState = new MineState {
                    futureEventsList = new List<IEvent>(),
                    pastEventsList = new List<IEvent>(),
                    futureListLoading = false,
                    pastListLoading = false,
                    futureEventTotal = 0,
                    pastEventTotal = 0
                },
                messageState = new MessageState {
                    channelMessageDict = new Dictionary<string, Dictionary<string, Message>>(),
                    channelMessageList = new Dictionary<string, List<string>>()
                },
                settingState = new SettingState {
                    hasReviewUrl = false,
                    reviewUrl = ""
                },
                reportState = new ReportState {
                    loading = false
                }
            };
        }
    }
}