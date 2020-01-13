using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class LeaderBoardScreenViewModel {
        public bool collectionLoading;
        public bool columnLoading;
        public bool bloggerLoading;
        public List<RankList> collectionRankList;
        public List<RankList> columnRankList;
        public List<string> bloggerIds;
        public bool collectionHasMore;
        public bool columnHasMore;
        public bool bloggerHasMore;
        public int collectionPageNumber;
        public int columnPageNumber;
        public int bloggerPageNumber;
        public Dictionary<string, User> userDict;
        public Dictionary<string, FavoriteTag> favoriteTagDict;
        public Dictionary<string, FavoriteTagArticle> favoriteTagArticleDict;
    }
}