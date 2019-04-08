using System;
using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.ViewModel {
    public class ArticleDetailScreenModel : IEquatable<ArticleDetailScreenModel> {
        public string articleId;
        public string loginUserId;
        public bool isLoggedIn;
        public bool articleDetailLoading;
        public Dictionary<string, Article> articleDict;
        public Dictionary<string, List<string>> channelMessageList;
        public Dictionary<string, Dictionary<string, Message>> channelMessageDict;
        public Dictionary<string, User> userDict;
        public Dictionary<string, Team> teamDict;

        public bool Equals(ArticleDetailScreenModel other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(articleId, other.articleId) && string.Equals(loginUserId, other.loginUserId) && isLoggedIn == other.isLoggedIn && articleDetailLoading == other.articleDetailLoading && Equals(articleDict, other.articleDict) && Equals(channelMessageList, other.channelMessageList) && Equals(channelMessageDict, other.channelMessageDict) && Equals(userDict, other.userDict) && Equals(teamDict, other.teamDict);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ArticleDetailScreenModel) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (articleId != null ? articleId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (loginUserId != null ? loginUserId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ isLoggedIn.GetHashCode();
                hashCode = (hashCode * 397) ^ articleDetailLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ (articleDict != null ? articleDict.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (channelMessageList != null ? channelMessageList.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (channelMessageDict != null ? channelMessageDict.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (userDict != null ? userDict.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (teamDict != null ? teamDict.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}