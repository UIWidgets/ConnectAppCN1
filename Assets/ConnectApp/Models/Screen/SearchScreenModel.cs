using System;
using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.Screen
{
    public class SearchScreenModel : IEquatable<PersonalScreenModel>
    {
        public bool searchLoading;
        public string searchKeyword;
        public List<Article> searchArticles;
        public int currentPage;
        public List<int> pages;
        public List<string> searchHistoryList;
    }
}