using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class LoginParameter {
        public string email;
        public string password;
    }

    [Serializable]
    public class LikeArticleParameter {
        public string type;
        public string itemId;
    }

    [Serializable]
    public class ReportArticleParameter {
        public string itemType;
        public string itemId;
        public List<string> reasons;
    }
}