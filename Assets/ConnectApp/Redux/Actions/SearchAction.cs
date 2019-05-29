using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public class PopularSearchSuccessAction : RequestAction {
        public List<PopularSearch> popularSearch;
    }

    public class StartSearchArticleAction : RequestAction {
    }

    public class SearchArticleSuccessAction : BaseAction {
        public string keyword;
        public int pageNumber = 0;
        public FetchSearchResponse searchResponse;
    }

    public class SearchArticleFailureAction : BaseAction {
        public string keyword;
    }

    public class ClearSearchArticleResultAction : BaseAction {
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
        public static object searchArticles(string keyword, int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return SearchApi.SearchArticle(keyword, pageNumber)
                    .Then(searchResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = searchResponse.userMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = searchResponse.teamMap});
                        dispatcher.dispatch(new SearchArticleSuccessAction {
                            keyword = keyword,
                            pageNumber = pageNumber,
                            searchResponse = searchResponse
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new SearchArticleFailureAction {keyword = keyword});
                        Debug.Log(error);
                    });
            });
        }

        public static object popularSearch() {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return SearchApi.PopularSearch()
                    .Then(popularSearch => {
                        dispatcher.dispatch(new PopularSearchSuccessAction {
                            popularSearch = popularSearch
                        });
                    })
                    .Catch(error => { Debug.Log(error); });
            });
        }
    }
}