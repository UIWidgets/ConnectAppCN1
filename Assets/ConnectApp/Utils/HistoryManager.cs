using System.Collections.Generic;
using ConnectApp.Models.Model;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.Utils {
    public static class HistoryManager {
        const string _searchArticleHistoryKey = "searchArticleHistoryKey";
        const string _articleHistoryKey = "articleHistoryKey";
        const string _eventHistoryKey = "eventHistoryKey";
        const string _blockArticleKey = "blockArticleKey";
        const string _visitorId = "visitor";

        public static List<Article> articleHistoryList(string userId = _visitorId) {
            var articleHistory = PlayerPrefs.GetString(_articleHistoryKey + userId);
            var articleHistoryList = new List<Article>();
            if (articleHistory.isNotEmpty()) {
                articleHistoryList = JsonConvert.DeserializeObject<List<Article>>(articleHistory);
            }

            var blockArticleList = new List<string>();
            if (userId != _visitorId) {
                blockArticleList = HistoryManager.blockArticleList(userId);
            }

            blockArticleList.ForEach(articleId => {
                articleHistoryList.RemoveAll(article => article.id == articleId);
            });
            return articleHistoryList;
        }

        public static List<Article> saveArticleHistory(Article article, string userId = _visitorId) {
            var articleHistory = PlayerPrefs.GetString(_articleHistoryKey + userId);
            var articleHistoryList = new List<Article>();
            if (articleHistory.isNotEmpty()) {
                articleHistoryList = JsonConvert.DeserializeObject<List<Article>>(articleHistory);
            }

            articleHistoryList.RemoveAll(item => item.id == article.id);
            articleHistoryList.Insert(0, article);
            if (articleHistoryList.Count > 50) {
                articleHistoryList.RemoveRange(50, articleHistoryList.Count - 50);
            }

            var newArticleHistory = JsonConvert.SerializeObject(articleHistoryList);
            PlayerPrefs.SetString(_articleHistoryKey + userId, newArticleHistory);
            PlayerPrefs.Save();
            return articleHistoryList;
        }

        public static List<Article> deleteArticleHistory(string articleId, string userId = _visitorId) {
            var articleHistory = PlayerPrefs.GetString(_articleHistoryKey + userId);
            var articleHistoryList = new List<Article>();
            if (articleHistory.isNotEmpty()) {
                articleHistoryList = JsonConvert.DeserializeObject<List<Article>>(articleHistory);
            }

            articleHistoryList.RemoveAll(item => item.id == articleId);
            var newArticleHistory = JsonConvert.SerializeObject(articleHistoryList);
            PlayerPrefs.SetString(_articleHistoryKey + userId, newArticleHistory);
            PlayerPrefs.Save();
            return articleHistoryList;
        }

        public static void deleteAllArticleHistory(string userId = _visitorId) {
            if (PlayerPrefs.HasKey(_articleHistoryKey + userId)) {
                PlayerPrefs.DeleteKey(_articleHistoryKey + userId);
            }
        }

        public static List<IEvent> eventHistoryList(string userId = _visitorId) {
            var eventHistory = PlayerPrefs.GetString(_eventHistoryKey + userId);
            var eventHistoryList = new List<IEvent>();
            if (eventHistory.isNotEmpty()) {
                eventHistoryList = JsonConvert.DeserializeObject<List<IEvent>>(eventHistory);
            }

            return eventHistoryList;
        }

        public static List<IEvent> saveEventHistoryList(IEvent eventObj, string userId = _visitorId) {
            var eventHistory = PlayerPrefs.GetString(_eventHistoryKey + userId);
            var eventHistoryList = new List<IEvent>();
            if (eventHistory.isNotEmpty()) {
                eventHistoryList = JsonConvert.DeserializeObject<List<IEvent>>(eventHistory);
            }

            eventHistoryList.RemoveAll(item => item.id == eventObj.id);
            eventHistoryList.Insert(0, eventObj);
            if (eventHistoryList.Count > 50) {
                eventHistoryList.RemoveRange(50, eventHistoryList.Count - 50);
            }

            var newEventHistory = JsonConvert.SerializeObject(eventHistoryList);
            PlayerPrefs.SetString(_eventHistoryKey + userId, newEventHistory);
            PlayerPrefs.Save();
            return eventHistoryList;
        }

        public static List<IEvent> deleteEventHistoryList(string eventId, string userId = _visitorId) {
            var eventHistory = PlayerPrefs.GetString(_eventHistoryKey + userId);
            var eventHistoryList = new List<IEvent>();
            if (eventHistory.isNotEmpty()) {
                eventHistoryList = JsonConvert.DeserializeObject<List<IEvent>>(eventHistory);
            }

            eventHistoryList.RemoveAll(item => item.id == eventId);
            var newEventHistory = JsonConvert.SerializeObject(eventHistoryList);
            PlayerPrefs.SetString(_eventHistoryKey + userId, newEventHistory);
            PlayerPrefs.Save();
            return eventHistoryList;
        }

        public static void deleteAllEventHistory(string userId = _visitorId) {
            if (PlayerPrefs.HasKey(_eventHistoryKey + userId)) {
                PlayerPrefs.DeleteKey(_eventHistoryKey + userId);
            }
        }

        public static List<string> searchArticleHistoryList(string userId = _visitorId) {
            var searchArticleHistory = PlayerPrefs.GetString(_searchArticleHistoryKey + userId);
            var searchArticleHistoryList = new List<string>();
            if (searchArticleHistory.isNotEmpty()) {
                searchArticleHistoryList = JsonConvert.DeserializeObject<List<string>>(searchArticleHistory);
            }

            return searchArticleHistoryList;
        }

        public static List<string> saveSearchArticleHistoryList(string keyword, string userId = _visitorId) {
            var searchArticleHistory = PlayerPrefs.GetString(_searchArticleHistoryKey + userId);
            var searchArticleHistoryList = new List<string>();
            if (searchArticleHistory.isNotEmpty()) {
                searchArticleHistoryList = JsonConvert.DeserializeObject<List<string>>(searchArticleHistory);
            }

            if (searchArticleHistoryList.Contains(keyword)) {
                searchArticleHistoryList.Remove(keyword);
            }

            searchArticleHistoryList.Insert(0, keyword);
            if (searchArticleHistoryList.Count > 5) {
                searchArticleHistoryList.RemoveRange(5, searchArticleHistoryList.Count - 5);
            }

            var newSearchHistory = JsonConvert.SerializeObject(searchArticleHistoryList);
            PlayerPrefs.SetString(_searchArticleHistoryKey + userId, newSearchHistory);
            PlayerPrefs.Save();
            return searchArticleHistoryList;
        }

        public static List<string> deleteSearchArticleHistoryList(string keyword, string userId = _visitorId) {
            var searchArticleHistory = PlayerPrefs.GetString(_searchArticleHistoryKey + userId);
            var searchArticleHistoryList = new List<string>();
            if (searchArticleHistory.isNotEmpty()) {
                searchArticleHistoryList = JsonConvert.DeserializeObject<List<string>>(searchArticleHistory);
            }

            if (searchArticleHistoryList.Contains(keyword)) {
                searchArticleHistoryList.Remove(keyword);
            }

            var newSearchHistory = JsonConvert.SerializeObject(searchArticleHistoryList);
            PlayerPrefs.SetString(_searchArticleHistoryKey + userId, newSearchHistory);
            PlayerPrefs.Save();
            return searchArticleHistoryList;
        }

        public static void deleteAllSearchArticleHistory(string userId = _visitorId) {
            if (PlayerPrefs.HasKey(_searchArticleHistoryKey + userId)) {
                PlayerPrefs.DeleteKey(_searchArticleHistoryKey + userId);
            }
        }

        public static List<string> blockArticleList(string userId = null) {
            if (userId == null) {
                return new List<string>();
            }

            var blockArticle = PlayerPrefs.GetString(_blockArticleKey + userId);
            var blockArticleList = new List<string>();
            if (blockArticle.isNotEmpty()) {
                blockArticleList = JsonConvert.DeserializeObject<List<string>>(blockArticle);
            }

            return blockArticleList;
        }

        public static List<string> saveBlockArticleList(string articleId, string userId) {
            var blockArticle = PlayerPrefs.GetString(_blockArticleKey + userId);
            var blockArticleList = new List<string>();
            if (blockArticle.isNotEmpty()) {
                blockArticleList = JsonConvert.DeserializeObject<List<string>>(blockArticle);
            }

            blockArticleList.Insert(0, articleId);
            var newBlockArticle = JsonConvert.SerializeObject(blockArticleList);
            PlayerPrefs.SetString(_blockArticleKey + userId, newBlockArticle);
            PlayerPrefs.Save();
            return blockArticleList;
        }
    }
}