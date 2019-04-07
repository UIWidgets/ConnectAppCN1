using System;
using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.Screen
{
    public class ArticlesScreenModel : IEquatable<ArticlesScreenModel>
    {
        public bool articlesLoading;
        public List<string> articleList;
        public Dictionary<string, Article> articleDict;
        public int articleTotal;

        public bool Equals(ArticlesScreenModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return articlesLoading == other.articlesLoading && Equals(articleList, other.articleList) && Equals(articleDict, other.articleDict) && articleTotal == other.articleTotal;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ArticlesScreenModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = articlesLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ (articleList != null ? articleList.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (articleDict != null ? articleDict.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ articleTotal;
                return hashCode;
            }
        }
    }
}