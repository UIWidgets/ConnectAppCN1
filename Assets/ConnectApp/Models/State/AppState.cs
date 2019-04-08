using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.models {
    [Serializable]
    public class AppState : IEquatable<AppState> {
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

        public static AppState initialState() {

            var searchHistory = PlayerPrefs.GetString("searchHistoryKey");
            var searchHistoryList = new List<string>();
            if (searchHistory.isNotEmpty())
                searchHistoryList = JsonConvert.DeserializeObject<List<string>>(searchHistory);
            
            return new AppState {
                Count = PlayerPrefs.GetInt("count", 0),
                loginState = new LoginState {
                    email = "",
                    password = "",
                    loginInfo = new LoginInfo(),
                    isLoggedIn = false,
                    loading = false
                },
                articleState = new ArticleState {
                    articleList = new List<string>(),
                    articleDict = new Dictionary<string, Article>(),
                    articlesLoading = false,
                    articleDetailLoading = false,
                    articleTotal = 0,
                    pageNumber = 1,
                    articleHistory = new List<Article>()
                },
                eventState = new EventState {
                    ongoingEvents = new List<string>(),
                    eventsDict = new Dictionary<string, IEvent>(),
                    ongoingEventTotal = 0,
                    completedEvents = new List<string>(),
                    completedEventTotal = 0,
                    pageNumber = 1,
                    completedPageNumber = 1,
                    eventsLoading = false,
                    eventHistory = new List<IEvent>(),
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
                    searchHistoryList = searchHistoryList,
                },
                notificationState = new NotificationState {
                    loading = false,
                    notifications = new List<Notification>()
                },
                userState = new UserState {
                    userDict = new Dictionary<string, User>()
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
                    fetchReviewUrlLoading = false,
                    reviewUrl = ""
                }
            };
        }

        public bool Equals(AppState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Count == other.Count && Equals(loginState, other.loginState) && Equals(articleState, other.articleState) && Equals(eventState, other.eventState) && Equals(popularSearchState, other.popularSearchState) && Equals(searchState, other.searchState) && Equals(notificationState, other.notificationState) && Equals(userState, other.userState) && Equals(teamState, other.teamState) && Equals(placeState, other.placeState) && Equals(mineState, other.mineState) && Equals(messageState, other.messageState) && Equals(settingState, other.settingState);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AppState) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = Count;
                hashCode = (hashCode * 397) ^ (loginState != null ? loginState.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (articleState != null ? articleState.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (eventState != null ? eventState.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (popularSearchState != null ? popularSearchState.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (searchState != null ? searchState.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (notificationState != null ? notificationState.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (userState != null ? userState.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (teamState != null ? teamState.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (placeState != null ? placeState.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (mineState != null ? mineState.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (messageState != null ? messageState.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (settingState != null ? settingState.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}