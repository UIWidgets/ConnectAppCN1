using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class PopularSearchAction : RequestAction {
    }
    
    public class PopularSearchSuccessAction : RequestAction {
        public List<PopularSearch> popularSearch;
    }
    
    public class SearchArticleAction : RequestAction {
        public string keyword;
        public int pageNumber = 0;
    }

    public class SearchArticleSuccessAction : BaseAction {
        public string keyword;
        public int pageNumber = 0;
        public FetchSearchResponse searchResponse;
    }
    
    public class SearchArticleFailedAction : BaseAction {
        public string keyword;
    }

    public class ClearSearchArticleAction : BaseAction {
    }

    public class GetSearchHistoryAction : BaseAction {
    }

    public class SaveSearchHistoryAction : BaseAction {
        public string keyword;
    }

    public class DeleteSearchHistoryAction : BaseAction {
        public string keyword;
    }

    public class DeleteAllSearchHistoryAction : BaseAction {
    }
}