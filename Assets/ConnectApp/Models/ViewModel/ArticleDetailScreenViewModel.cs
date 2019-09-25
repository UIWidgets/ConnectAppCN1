using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ArticleDetailScreenViewModel {
        public string articleId;
        public string loginUserId;
        public bool isLoggedIn;
        public bool articleDetailLoading;
        public Dictionary<string, Article> articleDict;
        public Dictionary<string, List<string>> channelMessageList;
        public Dictionary<string, Dictionary<string, Message>> channelMessageDict;
        public Dictionary<string, User> userDict;
        public Dictionary<string, UserLicense> userLicenseDict;
        public Dictionary<string, Team> teamDict;
        public Dictionary<string, bool> followMap;
    }
}