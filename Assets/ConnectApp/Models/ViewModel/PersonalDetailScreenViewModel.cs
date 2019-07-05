using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class PersonalDetailScreenViewModel {
        public string personalId;
        public bool personalLoading;
        public bool personalArticleLoading;
        public bool followUserLoading;
        public Personal personal;
        public Dictionary<string, bool> followMap;
        public int articleOffset;
        public string currentUserId;
        public bool isLoggedIn;
    }
}