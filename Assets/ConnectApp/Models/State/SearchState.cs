using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class SearchState : IEquatable<SearchState> {
        public bool loading { get; set; }
        public string keyword { get; set; }
        public List<Article> searchArticles { get; set; }
        public int currentPage { get; set; }
        public List<int> pages { get; set; }
        public List<string> searchHistoryList { get; set; }

        public bool Equals(SearchState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return loading == other.loading && string.Equals(keyword, other.keyword) && Equals(searchArticles, other.searchArticles) && currentPage == other.currentPage && Equals(pages, other.pages) && Equals(searchHistoryList, other.searchHistoryList);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SearchState) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = loading.GetHashCode();
                hashCode = (hashCode * 397) ^ (keyword != null ? keyword.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (searchArticles != null ? searchArticles.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ currentPage;
                hashCode = (hashCode * 397) ^ (pages != null ? pages.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (searchHistoryList != null ? searchHistoryList.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    [Serializable]
    public class PopularSearchState {
        public List<PopularSearch> popularSearchs { get; set; }
    }
}