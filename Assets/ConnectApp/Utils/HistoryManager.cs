using System.Collections.Generic;
using ConnectApp.models;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.utils {
    public static class HistoryManager {
        private const string _searchHistoryKey = "searchHistoryKey";
        private const string _articleHistoryKey = "articleHistoryKey";
        private const string _eventHistoryKey = "eventHistoryKey";
        private const string _visitorId = "visitor";
        
        public static List<Article> articleHistoryList(string userId= _visitorId) {
            var articleHistory = PlayerPrefs.GetString(_articleHistoryKey+userId);
            var articleHistoryList = new List<Article>();
            if (articleHistory.isNotEmpty())
                articleHistoryList = JsonConvert.DeserializeObject<List<Article>>(articleHistory);
            return articleHistoryList;
        }

        public static List<Article> saveArticleHistory(Article article,string userId= _visitorId) {
            var articleHistory = PlayerPrefs.GetString(_articleHistoryKey+userId);
            var articleHistoryList = new List<Article>();
            if (articleHistory.isNotEmpty())
                articleHistoryList = JsonConvert.DeserializeObject<List<Article>>(articleHistory);
            articleHistoryList.RemoveAll(item => item.id == article.id);
            articleHistoryList.Insert(0, article);
            if (articleHistoryList.Count > 50) articleHistoryList.RemoveRange(50, articleHistoryList.Count - 50);
            var newArticleHistory = JsonConvert.SerializeObject(articleHistoryList);
            PlayerPrefs.SetString(_articleHistoryKey+userId, newArticleHistory);
            PlayerPrefs.Save();
            return articleHistoryList;
        }
        
        public static List<Article> deleteArticleHistory(string articleId,string userId= _visitorId) {
            var articleHistory = PlayerPrefs.GetString(_articleHistoryKey+userId);
            var articleHistoryList = new List<Article>();
            if (articleHistory.isNotEmpty())
                articleHistoryList = JsonConvert.DeserializeObject<List<Article>>(articleHistory);
            articleHistoryList.RemoveAll(item => item.id == articleId);
            var newArticleHistory = JsonConvert.SerializeObject(articleHistoryList);
            PlayerPrefs.SetString(_articleHistoryKey+userId, newArticleHistory);
            PlayerPrefs.Save();
            return articleHistoryList;
        }

        public static void deleteAllArticleHistory(string userId= _visitorId) {
            if (PlayerPrefs.HasKey(_articleHistoryKey+userId))
                PlayerPrefs.DeleteKey(_articleHistoryKey+userId);
        }

        public static List<IEvent> eventHistoryList(string userId= _visitorId) {
            var eventHistory = PlayerPrefs.GetString(_eventHistoryKey+userId);
            var eventHistoryList = new List<IEvent>();
            if (eventHistory.isNotEmpty())
                eventHistoryList = JsonConvert.DeserializeObject<List<IEvent>>(eventHistory);
            return eventHistoryList;
        }
        
        public static List<IEvent> saveEventHistoryList(IEvent eventObj,string userId= _visitorId) {
            var eventHistory = PlayerPrefs.GetString(_eventHistoryKey+userId);
            var eventHistoryList = new List<IEvent>();
            if (eventHistory.isNotEmpty())
                eventHistoryList = JsonConvert.DeserializeObject<List<IEvent>>(eventHistory);
            eventHistoryList.RemoveAll(item => item.id == eventObj.id);
            eventHistoryList.Insert(0, eventObj);
            if (eventHistoryList.Count > 50) eventHistoryList.RemoveRange(50, eventHistoryList.Count - 50);
            var newEventHistory = JsonConvert.SerializeObject(eventHistoryList);
            PlayerPrefs.SetString(_eventHistoryKey+userId, newEventHistory);
            PlayerPrefs.Save();
            return eventHistoryList;
        }
        
        public static List<IEvent> deleteEventHistoryList(string eventId,string userId= _visitorId) {
            var eventHistory = PlayerPrefs.GetString(_eventHistoryKey+userId);
            var eventHistoryList = new List<IEvent>();
            if (eventHistory.isNotEmpty())
                eventHistoryList = JsonConvert.DeserializeObject<List<IEvent>>(eventHistory);
            eventHistoryList.RemoveAll(item => item.id == eventId);
            var newEventHistory = JsonConvert.SerializeObject(eventHistoryList);
            PlayerPrefs.SetString(_eventHistoryKey+userId, newEventHistory);
            PlayerPrefs.Save();
            return eventHistoryList;
        }
        
        public static void deleteAllEventHistory(string userId= _visitorId) {
            if (PlayerPrefs.HasKey(_eventHistoryKey+userId))
                PlayerPrefs.DeleteKey(_eventHistoryKey+userId);
        }
        
        public static List<string> searchHistoryList(string userId= _visitorId) {
            var searchHistory = PlayerPrefs.GetString(_searchHistoryKey+userId);
            var searchHistoryList = new List<string>();
            if (searchHistory.isNotEmpty())
                searchHistoryList = JsonConvert.DeserializeObject<List<string>>(searchHistory);
            return searchHistoryList;
        }
        
        public static List<string> saveSearchHistoryList(string keyword,string userId= _visitorId) {
            var searchHistory = PlayerPrefs.GetString(_searchHistoryKey+userId);
            var searchHistoryList = new List<string>();
            if (searchHistory.isNotEmpty())
                searchHistoryList = JsonConvert.DeserializeObject<List<string>>(searchHistory);
            if (searchHistoryList.Contains(keyword)) searchHistoryList.Remove(keyword);
            searchHistoryList.Insert(0, keyword);
            if (searchHistoryList.Count > 5) searchHistoryList.RemoveRange(5, searchHistoryList.Count - 5);
            var newSearchHistory = JsonConvert.SerializeObject(searchHistoryList);
            PlayerPrefs.SetString(_searchHistoryKey+userId, newSearchHistory);
            PlayerPrefs.Save();
            return searchHistoryList;
        }
        
        public static List<string> deleteSearchHistoryList(string keyword,string userId= _visitorId) {
            var searchHistory = PlayerPrefs.GetString(_searchHistoryKey+userId);
            var searchHistoryList = new List<string>();
            if (searchHistory.isNotEmpty())
                searchHistoryList = JsonConvert.DeserializeObject<List<string>>(searchHistory);
            if (searchHistoryList.Contains(keyword)) searchHistoryList.Remove(keyword);
            var newSearchHistory = JsonConvert.SerializeObject(searchHistoryList);
            PlayerPrefs.SetString(_searchHistoryKey+userId, newSearchHistory);
            PlayerPrefs.Save();
            return searchHistoryList;
        }
        
        public static void deleteAllSearchHistory(string userId= _visitorId) {
            if (PlayerPrefs.HasKey(_searchHistoryKey+userId))
                PlayerPrefs.DeleteKey(_searchHistoryKey+userId);
        }
    }
}