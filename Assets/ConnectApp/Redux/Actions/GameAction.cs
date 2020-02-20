using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class StartFetchGameAction : RequestAction {
    }

    public class FetchGameSuccessAction : BaseAction {
        public List<string> gameIds;
        public bool hasMore;
        public int pageNumber;
    }

    public class FetchGameFailureAction : BaseAction {
    }

    public static partial class Actions {
        public static object fetchGames(int page) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return GameApi.FetchGames(page: page)
                    .Then(gamesResponse => {
                        dispatcher.dispatch(new RankListAction {rankList = gamesResponse.rankList});
                        var gameIds = new List<string>();
                        gamesResponse.rankList.ForEach(rankData => { gameIds.Add(item: rankData.id); });
                        dispatcher.dispatch(new FetchGameSuccessAction {
                            gameIds = gameIds,
                            hasMore = gamesResponse.hasMore,
                            pageNumber = gamesResponse.currentPage
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchGameFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }
    }
}