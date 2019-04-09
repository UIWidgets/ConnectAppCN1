using System;
using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.ViewModel
{
    public class ArticlesScreenViewModel
    {
        public bool articlesLoading;
        public List<string> articleList;
        public Dictionary<string, Article> articleDict;
        public Dictionary<string, User> userDict;
        public Dictionary<string, Team> teamDict;
        public int articleTotal;
    }
}