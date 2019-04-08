using System;
using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.Screen
{
    public class SearchScreenModel : IEquatable<SearchScreenModel>
    {
        public bool searchLoading;
        public string searchKeyword;
        public List<Article> searchArticles;
        public int currentPage;
        public List<int> pages;
        public List<string> searchHistoryList;
        public List<PopularSearch> popularSearchs;

        public bool Equals(SearchScreenModel other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return searchLoading == other.searchLoading && string.Equals(searchKeyword, other.searchKeyword) && Equals(searchArticles, other.searchArticles) && currentPage == other.currentPage && Equals(pages, other.pages) && Equals(searchHistoryList, other.searchHistoryList) && Equals(popularSearchs, other.popularSearchs);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SearchScreenModel) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = searchLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ (searchKeyword != null ? searchKeyword.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (searchArticles != null ? searchArticles.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ currentPage;
                hashCode = (hashCode * 397) ^ (pages != null ? pages.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (searchHistoryList != null ? searchHistoryList.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (popularSearchs != null ? popularSearchs.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}