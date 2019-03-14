using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace ConnectApp.models
{
    [Serializable]
    public class Project
    {
        public Article projectData;
        public List<string> pureContentIds;
        public List<Article> projects;
        public Dictionary<string, ContentMap> contentMap;
        public Dictionary<string, User> userMap;
        [FormerlySerializedAs("comments")] public FetchCommentsResponse fetchCommentsResponse;
        public string channelId;
        public bool like;
        public bool edit;
    }   
}