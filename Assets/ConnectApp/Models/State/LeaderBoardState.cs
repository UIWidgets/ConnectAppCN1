using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class LeaderBoardState {
        public bool collectionLoading { get; set; }
        public bool columnLoading { get; set; }
        public bool bloggerLoading { get; set; }
        public List<RankData> collectionRankList { get; set; }
        public List<RankData> columnRankList { get; set; }
        public List<string> bloggerIds { get; set; }
        public bool collectionHasMore { get; set; }
        public bool columnHasMore { get; set; }
        public bool bloggerHasMore { get; set; }
        public int collectionPageNumber { get; set; }
        public int columnPageNumber { get; set; }
        public int bloggerPageNumber { get; set; }
    }
}