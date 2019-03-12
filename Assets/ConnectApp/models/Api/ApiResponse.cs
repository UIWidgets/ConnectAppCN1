using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class FetchArticlesResponse {
        public List<Article> items;
        public Dictionary<string, User> userMap;
    }

    [Serializable]
    public class FetchArticleDetailResponse
    {
        public Project project;
    }
}