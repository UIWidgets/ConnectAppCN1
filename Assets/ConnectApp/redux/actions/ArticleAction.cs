using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class FetchArticlesAction : RequestAction {
        public int pageNumber = 0;
    }

    public class FetchArticleSuccessAction : BaseAction {
        public List<string> ArticleList;
        public Dictionary<string, Article> ArticleDict;
    }

    public class FetchArticleDetailAction : RequestAction {
        public string articleId;
    }

    public class FetchArticleDetailSuccessAction : BaseAction {
        public Project articleDetail;
    }
    
    public class SaveArticleDetailSuccessAction : BaseAction {
        public Project articleDetail;
    }

    public class FetchArticleCommentsAction : RequestAction {
        public string channelId;
        public string currOldestMessageId = "";
    }

    public class FetchArticleCommentsSuccessAction : BaseAction {
        public Dictionary<string, List<string>> channelMessageList;
        public Dictionary<string, Dictionary<string, Message>> channelMessageDict;
        public bool isRefreshList;
    }

    public class LikeArticleAction : RequestAction {
        public string articleId;
    }

    public class LikeArticleSuccessAction : BaseAction {
        public string articleId;
    }

    public class LikeCommentAction : RequestAction {
        public string messageId;
    }

    public class LikeCommentSuccessAction : BaseAction
    {
        public Message message;
    }

    public class RemoveLikeCommentAction : RequestAction {
        public string messageId;
    }

    public class RemoveLikeSuccessAction : BaseAction {
        public Message message;
    }

    public class SendCommentAction : RequestAction {
        public string channelId;
        public string content;
        public string nonce;
        public string parentMessageId = "";
    }

    public class SendCommentSuccessAction : BaseAction {
    }
}