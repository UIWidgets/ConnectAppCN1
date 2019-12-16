using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Api {
    [Serializable]
    public class LoginParameter {
        public string email;
        public string password;
    }

    [Serializable]
    public class WechatLoginParameter {
        public string code;
    }

    [Serializable]
    public class QRLoginParameter {
        public string token;
        public string action;
    }

    [Serializable]
    public class HandleArticleParameter {
        public string type;
        public string itemId;
        public List<string> tagIds;
    }

    [Serializable]
    public class ReactionParameter {
        public string reactionType;
    }

    [Serializable]
    public class SendCommentParameter {
        public string content;
        public string parentMessageId;
        public string upperMessageId;
        public string nonce;
        public bool app;
    }

    [Serializable]
    public class ReportParameter {
        public string itemType;
        public string itemId;
        public List<string> reasons;
    }

    [Serializable]
    public class FeedbackParameter {
        public string type;
        public string name;
        public string contact;
        public string content;
        public string data;
    }

    [Serializable]
    public class FollowParameter {
        public string type;
        public string followeeId;
    }

    [Serializable]
    public class EditPersonalParameter {
        public string fullName;
        public string title;
        public string jobRoleId;
        public string placeId;
    }

    [Serializable]
    public class OpenAppParameter {
        public string userId;
        public string device;
        public string store;
        public string eventType;
        public DateTime appTime;
        public List<Dictionary<string, string>> extraData;
    }

    [Serializable]
    public class UpdateAvatarParameter {
        public string avatar;
    }

    [Serializable]
    public class RegisterTokenParameter {
        public string token;
        public string userId;
    }
}