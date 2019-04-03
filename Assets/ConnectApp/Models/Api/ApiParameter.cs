using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class LoginParameter {
        public string email;
        public string password;
    }
    public class WechatLoginParameter
    {
        public string code;
    }

    [Serializable]
    public class LikeArticleParameter {
        public string type;
        public string itemId;
    }

    [Serializable]
    public class ReactionParameter {
        public string reactionType;
    }

    [Serializable]
    public class SendCommentParameter {
        public string content;
        public string parentMessageId;
        public string nonce;
    }

    [Serializable]
    public class ReportParameter {
        public string itemType;
        public string itemId;
        public List<string> reasons;
    }
}