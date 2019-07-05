using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class PersonalState {
        public bool personalLoading;
        public bool personalArticleLoading;
        public bool followUserLoading;
        public bool followingLoading;
        public bool followerLoading;
        public Dictionary<string, Personal> personalDict;
        public string currentFollowId;
        public string fullName;
        public string title;
        public JobRole jobRole;
        public string place;
    }
}