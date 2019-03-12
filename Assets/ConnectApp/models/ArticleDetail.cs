using System;
using System.Collections.Generic;

namespace ConnectApp.models
{
    [Serializable]
    public class ArticleDetail
    {
        public Article projectData;
        public List<string> pureContentIds;
        public List<Article> projects;
        public Dictionary<string, ContentMap> contentMap;
        public bool like;
        public bool edit;
    }
}