using System.Collections.Generic;
using ConnectApp.Models.Model;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.Utils {
    public static class HistoryManager {
        const string _homeAfterTimeKey = "homeAfterTimeKey";
        const string _searchArticleHistoryKey = "searchArticleHistoryKey";
        const string _articleHistoryKey = "articleHistoryKey";
        const string _eventHistoryKey = "eventHistoryKey";
        const string _blockArticleKey = "blockArticleKey";
        const string _blockUserKey = "blockUserKey";
        const string _visitorId = "visitor";
        const int _searchArticleHistoryLimit = 5;
        const int _articleHistoryLimit = 50;
        const int _eventHistoryLimit = 50;

        public static string homeAfterTime(string userId) {
            if (!PlayerPrefs.HasKey(_homeAfterTimeKey + userId)) {
                return "";
            }

            return PlayerPrefs.GetString(_homeAfterTimeKey + userId);
        }

        public static void saveHomeAfterTime(string afterTime, string userId) {
            PlayerPrefs.SetString(_homeAfterTimeKey + userId, value: afterTime);
            PlayerPrefs.Save();
        }

        public static void deleteHomeAfterTime(string userId) {
            if (PlayerPrefs.HasKey(_homeAfterTimeKey + userId)) {
                PlayerPrefs.DeleteKey(_homeAfterTimeKey + userId);
            }
        }

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
            if (articleHistoryList.Count > _articleHistoryLimit) {
                articleHistoryList.RemoveRange(_articleHistoryLimit, articleHistoryList.Count - _articleHistoryLimit);
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
            if (eventHistoryList.Count > _eventHistoryLimit) {
                eventHistoryList.RemoveRange(_eventHistoryLimit, eventHistoryList.Count - _eventHistoryLimit);
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
            if (searchArticleHistoryList.Count > _searchArticleHistoryLimit) {
                searchArticleHistoryList.RemoveRange(_searchArticleHistoryLimit,
                    searchArticleHistoryList.Count - _searchArticleHistoryLimit);
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
        
        public static HashSet<string> blockUserIdSet(string currentUserId = null) {
            if (currentUserId == null) {
                return new HashSet<string>();
            }

            var blockUserData = PlayerPrefs.GetString(_blockUserKey + currentUserId);
            var blockUserIdSet = new HashSet<string>();
            if (blockUserData.isNotEmpty()) {
                blockUserIdSet = JsonConvert.DeserializeObject<HashSet<string>>(value: blockUserData);
            }

            return blockUserIdSet;
        }

        public static bool isBlockUser(string userId) {
            return blockUserIdSet(currentUserId: UserInfoManager.getUserInfo().userId).Contains(item: userId);
        }

        public static HashSet<string> updateBlockUserId(string blockUserId, string currentUserId, bool remove) {
            var blockUserData = PlayerPrefs.GetString(_blockUserKey + currentUserId);
            var blockUserIdSet = new HashSet<string>();
            if (blockUserData.isNotEmpty()) {
                blockUserIdSet = JsonConvert.DeserializeObject<HashSet<string>>(value: blockUserData);
            }

            if (remove) {
                blockUserIdSet.Remove(item: blockUserId);
            }
            else {
                blockUserIdSet.Add(item: blockUserId);
            }
            
            var newBlockUserData = JsonConvert.SerializeObject(value: blockUserIdSet);
            PlayerPrefs.SetString(_blockUserKey + currentUserId, value: newBlockUserData);
            PlayerPrefs.Save();
            return blockUserIdSet;
        }
    }
}