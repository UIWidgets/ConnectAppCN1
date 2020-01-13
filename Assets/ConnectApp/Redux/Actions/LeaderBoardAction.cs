using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class StartFetchLeaderBoardCollectionAction : RequestAction {
    }

    public class FetchLeaderBoardCollectionSuccessAction : BaseAction {
        public List<RankList> rankList;
        public Dictionary<string, FavoriteTagArticle> favoriteTagArticleMap;
        public Dictionary<string, FavoriteTag> favoriteTagMap;
        public bool hasMore;
        public int pageNumber;
    }

    public class FetchLeaderBoardCollectionFailureAction : BaseAction {
    }

    public class StartFetchLeaderBoardColumnAction : RequestAction {
    }

    public class FetchLeaderBoardColumnSuccessAction : BaseAction {
        public List<RankList> rankList;
        public Dictionary<string, UserArticle> userArticleMap;
        public bool hasMore;
        public int pageNumber;
    }

    public class FetchLeaderBoardColumnFailureAction : BaseAction {
    }

    public class StartFetchLeaderBoardBloggerAction : RequestAction {
    }

    public class FetchLeaderBoardBloggerSuccessAction : BaseAction {
        public List<string> bloggerIds;
        public bool hasMore;
        public int pageNumber;
    }

    public class FetchLeaderBoardBloggerFailureAction : BaseAction {
    }

    public class StartFetchLeaderBoardCollectionDetailAction : RequestAction {
    }

    public class FetchLeaderBoardCollectionDetailSuccessAction : BaseAction {
        public List<string> articleIds;
        public string collectionId;
        public bool hasMore;
        public int pageNumber;
    }

    public class FetchLeaderBoardCollectionDetailFailureAction : BaseAction {
    }

    public class StartFetchLeaderBoardColumnDetailAction : RequestAction {
    }

    public class FetchLeaderBoardColumnDetailSuccessAction : BaseAction {
        public List<string> articleIds;
        public string columnId;
        public bool hasMore;
        public int pageNumber;
    }

    public class FetchLeaderBoardColumnDetailFailureAction : BaseAction {
    }

    public static partial class Actions {
        public static object fetchLeaderBoardCollection(int page) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return LeaderBoardApi.FetchLeaderBoardCollection(page: page)
                    .Then(collectionResponse => {
                        dispatcher.dispatch(new FetchLeaderBoardCollectionSuccessAction {
                            rankList = collectionResponse.rankList,
                            favoriteTagArticleMap = collectionResponse.favoriteTagArticleMap,
                            favoriteTagMap = collectionResponse.favoriteTagMap,
                            hasMore = collectionResponse.hasMore,
                            pageNumber = page
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchLeaderBoardCollectionFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchLeaderBoardColumn(int page) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return LeaderBoardApi.FetchLeaderBoardColumn(page: page)
                    .Then(columnResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = columnResponse.userSimpleV2Map});
                        dispatcher.dispatch(new FetchLeaderBoardColumnSuccessAction {
                            rankList = columnResponse.rankList,
                            userArticleMap = columnResponse.userArticleMap,
                            hasMore = columnResponse.hasMore,
                            pageNumber = page
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchLeaderBoardColumnFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchLeaderBoardBlogger(int page) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return LeaderBoardApi.FetchLeaderBoardBlogger(page: page)
                    .Then(bloggerResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = bloggerResponse.userFullMap});
                        dispatcher.dispatch(new FetchLeaderBoardBloggerSuccessAction {
                            bloggerIds = bloggerResponse.bloggerIds,
                            hasMore = bloggerResponse.hasMore,
                            pageNumber = page
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchLeaderBoardBloggerFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchLeaderBoardCollectionDetail(string collectionId, int page) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return LeaderBoardApi.FetchLeaderBoardCollectionDetail(collectionId: collectionId, page: page)
                    .Then(collectionDetailResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = collectionDetailResponse.userSimpleV2Map});
                        dispatcher.dispatch(new TeamMapAction {teamMap = collectionDetailResponse.teamSimpleMap});
                        var articleIds = new List<string>();
                        var articleDict = new Dictionary<string, Article>();
                        collectionDetailResponse.projectSimples.ForEach(project => {
                            articleIds.Add(item: project.id);
                            articleDict.Add(key: project.id, value: project);
                        });
                        dispatcher.dispatch(new ArticleMapAction {articleMap = articleDict});

                        dispatcher.dispatch(new FetchLeaderBoardCollectionDetailSuccessAction {
                            articleIds = articleIds,
                            collectionId = collectionId,
                            hasMore = collectionDetailResponse.hasMore,
                            pageNumber = page
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchLeaderBoardCollectionDetailFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchLeaderBoardColumnDetail(string columnId, int page) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return LeaderBoardApi.FetchLeaderBoardColumnDetail(columnId: columnId, page: page)
                    .Then(columnDetailResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = columnDetailResponse.userSimpleV2Map});
                        var articleIds = new List<string>();
                        var articleDict = new Dictionary<string, Article>();
                        columnDetailResponse.projectSimples.ForEach(project => {
                            articleIds.Add(item: project.id);
                            articleDict.Add(key: project.id, value: project);
                        });
                        dispatcher.dispatch(new ArticleMapAction {articleMap = articleDict});

                        dispatcher.dispatch(new FetchLeaderBoardColumnDetailSuccessAction {
                            articleIds = articleIds,
                            columnId = columnId,
                            hasMore = columnDetailResponse.hasMore,
                            pageNumber = page
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchLeaderBoardColumnDetailFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }
    }
}