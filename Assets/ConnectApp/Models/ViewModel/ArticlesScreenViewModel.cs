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
        public int articleTotal;
    }
}