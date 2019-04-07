using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.models;
using Unity.UIWidgets.Redux;
using UnityEngine;

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

    public class ClearSearchArticleResultAction : BaseAction {}

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
    
    public static partial class Actions {
        public static object searchArticles(string keyword, int pageNumber)
        {
            return new ThunkAction<AppState>((dispatcher, getState) => {                
                return SearchApi.SearchArticle(keyword, pageNumber)
                    .Then(searchResponse => {
                        dispatcher.dispatch(new SearchArticleSuccessAction {
                            keyword = keyword,
                            pageNumber = pageNumber,
                            searchResponse = searchResponse
                        });
                    })
                    .Catch(Debug.Log);
            });
        }
    }
}