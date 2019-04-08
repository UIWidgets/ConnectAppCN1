using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class ArticleState : IEquatable<ArticleState> {
        public bool articlesLoading { get; set; }
        public bool articleDetailLoading { get; set; }

        public List<string> articleList { get; set; }

        public int articleTotal { get; set; }
        public int pageNumber { get; set; }
        public Dictionary<string, Article> articleDict { get; set; }

        public List<Article> articleHistory { get; set; }

        public bool Equals(ArticleState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return articlesLoading == other.articlesLoading && articleDetailLoading == other.articleDetailLoading && Equals(articleList, other.articleList) && articleTotal == other.articleTotal && pageNumber == other.pageNumber && Equals(articleDict, other.articleDict) && Equals(articleHistory, other.articleHistory);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ArticleState) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = articlesLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ articleDetailLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ (articleList != null ? articleList.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ articleTotal;
                hashCode = (hashCode * 397) ^ pageNumber;
                hashCode = (hashCode * 397) ^ (articleDict != null ? articleDict.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (articleHistory != null ? articleHistory.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}