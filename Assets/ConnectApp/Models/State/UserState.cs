using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class UserState {
        public bool userLoading { get; set; }
        public bool userArticleLoading { get; set; }
        public bool followingLoading { get; set; }
        public bool followerLoading { get; set; }
        public Dictionary<string, User> userDict { get; set; }
        public string fullName { get; set; }
        public string title { get; set; }
        public JobRole jobRole { get; set; }
        public string place { get; set; }
    }
}