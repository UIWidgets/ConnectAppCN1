using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class FetchArticlesAction : RequestAction {
        public int pageNumber = 1;
    }

    public class FetchArticleSuccessAction : BaseAction {
        public List<string> ArticleList;
        public Dictionary<string, Article> ArticleDict;
    }

    public class FetchArticleDetailAction : RequestAction {
        public string articleId;
    }

    public class FetchArticleDetailSuccessAction : BaseAction
    {
        public ArticleDetail articleDetail;
    }

    public class FetchArticleCommentsAction : RequestAction {
        public string channelId;
        public string currOldestMessageId = "";
    }

    public class FetchArticleCommentsSuccessAction : BaseAction {
    }

    public class LikeArticleAction : RequestAction {
        public string articleId;
    }

    public class LikeArticleSuccessAction : BaseAction {
    }

    public class LikeCommentAction : RequestAction {
        public string messageId;
    }

    public class LikeCommentSuccessAction : BaseAction {
    }

    public class RemoveLikeCommentAction : RequestAction {
        public string messageId;
    }

    public class RemoveLikeSuccessAction : BaseAction {
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