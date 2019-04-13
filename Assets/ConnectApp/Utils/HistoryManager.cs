using System.Collections.Generic;
using ConnectApp.models;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using UnityEngine;

namespace ConnectApp.utils
{
    public class HistoryManager
    {
        public static List<Article> articleHistoryList()
        {
            var articleHistory = PlayerPrefs.GetString("articleHistoryKey");
            var articleHistoryList = new List<Article>();
            if (articleHistory.isNotEmpty())
                articleHistoryList = JsonConvert.DeserializeObject<List<Article>>(articleHistory);
            return articleHistoryList;
        }
        
        public static List<IEvent> eventHistoryList()
        {
            var eventHistory = PlayerPrefs.GetString("eventHistoryKey");
            var eventHistoryList = new List<IEvent>();
            if (eventHistory.isNotEmpty())
                eventHistoryList = JsonConvert.DeserializeObject<List<IEvent>>(eventHistory);
            return eventHistoryList;
        }
        
        public static List<string> searchHistoryList()
        {
            var searchHistory = PlayerPrefs.GetString("searchHistoryKey");
            var searchHistoryList = new List<string>();
            if (searchHistory.isNotEmpty())
                searchHistoryList = JsonConvert.DeserializeObject<List<string>>(searchHistory);
            return searchHistoryList;
        }
    }
}