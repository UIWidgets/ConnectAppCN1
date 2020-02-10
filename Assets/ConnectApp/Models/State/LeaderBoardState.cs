using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class LeaderBoardState {
        public bool collectionLoading { get; set; }
        public bool columnLoading { get; set; }
        public bool bloggerLoading { get; set; }
        public bool homeBloggerLoading { get; set; }
        public List<string> collectionIds { get; set; }
        public List<string> columnIds { get; set; }
        public List<string> bloggerIds { get; set; }
        public List<string> homeBloggerIds { get; set; }
        public bool collectionHasMore { get; set; }
        public bool columnHasMore { get; set; }
        public bool bloggerHasMore { get; set; }
        public bool homeBloggerHasMore { get; set; }
        public int collectionPageNumber { get; set; }
        public int columnPageNumber { get; set; }
        public int bloggerPageNumber { get; set; }
        public int homeBloggerPageNumber { get; set; }
        public Dictionary<string, RankData> rankDict { get; set; }
        public bool detailCollectLoading { get; set; }
        public bool detailLoading { get; set; }
        public bool detailHasMore { get; set; }
        public Dictionary<string, List<string>> columnDict { get; set; }
        public Dictionary<string, List<string>> collectionDict { get; set; }
    }
}