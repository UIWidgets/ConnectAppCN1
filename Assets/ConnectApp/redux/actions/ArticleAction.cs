using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class FetchArticlesAction : RequestAction {
        public int pageNumber = 0;
    }

    public class FetchArticleSuccessAction : BaseAction {
        public List<string> articleList;
        public Dictionary<string, Article> articleDict;
        public int total;
    }

    public class FetchArticleDetailAction : RequestAction {
        public string articleId;
    }

    public class FetchArticleDetailSuccessAction : BaseAction {
        public Project articleDetail;
    }

    public class GetArticleHistoryAction : BaseAction {
    }

    public class SaveArticleHistoryAction : BaseAction {
        public Article article;
    }

    public class DeleteArticleHistoryAction : BaseAction {
        public string articleId;
    }

    public class DeleteAllArticleHistoryAction : BaseAction {
    }

    public class FetchArticleCommentsAction : RequestAction {
        public string channelId;
        public string currOldestMessageId = "";
    }

    public class FetchArticleCommentsSuccessAction : BaseAction {
        public string channelId;
        public FetchCommentsResponse commentsResponse;
        public string currOldestMessageId;
        public bool hasMore;
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

    public class LikeCommentSuccessAction : BaseAction {
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
        public Message message;
    }
}