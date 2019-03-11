using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class FetchArticlesAction : RequestAction {
        public int pageNumber = 1;
    }

    public class FetchArticleSuccessAction : RequestAction
    {
        public List<string> ArticleList;
        public Dictionary<string, Article> ArticleDict;
    }

    public class FetchArticleDetailAction : RequestAction {
        public string articleId;
    }

    public class FetchArticleDetailSuccessAction : RequestAction {
    }
}