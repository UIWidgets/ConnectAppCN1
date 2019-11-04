using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class PopularSearchArticleSuccessAction : RequestAction {
        public List<PopularSearch> popularSearchArticles;
    }

    public class PopularSearchUserSuccessAction : RequestAction {
        public List<PopularSearch> popularSearchUsers;
    }

    public class StartSearchArticleAction : RequestAction {
        public string keyword;
    }

    public class SearchArticleSuccessAction : BaseAction {
        public string keyword;
        public int pageNumber;
        public List<Article> searchArticles;
        public int currentPage;
        public List<int> pages;
    }

    public class SearchArticleFailureAction : BaseAction {
        public string keyword;
    }

    public class ClearSearchResultAction : BaseAction {
    }

    public class ClearSearchFollowingResultAction : BaseAction {
    }

    public class SaveSearchArticleHistoryAction : BaseAction {
        public string keyword;
    }

    public class DeleteSearchArticleHistoryAction : BaseAction {
        public string keyword;
    }

    public class DeleteAllSearchArticleHistoryAction : BaseAction {
    }

    public class StartSearchUserAction : RequestAction {
    }

    public class SearchUserSuccessAction : BaseAction {
        public string keyword;
        public int pageNumber;
        public List<string> searchUserIds;
        public bool hasMore;
    }

    public class SearchUserFailureAction : BaseAction {
        public string keyword;
    }

    public class StartSearchFollowingAction : RequestAction {
    }

    public class SearchFollowingSuccessAction : BaseAction {
        public string keyword;
        public int pageNumber;
        public List<User> users;
        public bool hasMore;
    }

    public class SearchFollowingFailureAction : BaseAction {
        public string keyword;
    }

    public class StartSearchTeamAction : RequestAction {
    }

    public class SearchTeamSuccessAction : BaseAction {
        public string keyword;
        public int pageNumber;
        public List<string> searchTeamIds;
        public bool hasMore;
    }

    public class SearchTeamFailureAction : BaseAction {
        public string keyword;
    }

    public static partial class Actions {
        public static object searchArticles(string keyword, int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return SearchApi.SearchArticle(keyword, pageNumber)
                    .Then(searchArticleResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = searchArticleResponse.userMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = searchArticleResponse.teamMap});
                        dispatcher.dispatch(new PlaceMapAction {placeMap = searchArticleResponse.placeMap});
                        dispatcher.dispatch(new LikeMapAction {likeMap = searchArticleResponse.likeMap});
                        dispatcher.dispatch(new SearchArticleSuccessAction {
                            keyword = keyword,
                            pageNumber = pageNumber,
                            searchArticles = searchArticleResponse.projects,
                            currentPage = searchArticleResponse.currentPage,
                            pages = searchArticleResponse.pages
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new SearchArticleFailureAction {keyword = keyword});
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object popularSearchArticle() {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return SearchApi.PopularSearchArticle()
                    .Then(popularSearchArticles => {
                        dispatcher.dispatch(new PopularSearchArticleSuccessAction {
                            popularSearchArticles = popularSearchArticles
                        });
                    })
                    .Catch(onRejected: Debuger.LogError);
            });
        }

        public static object searchUsers(string keyword, int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return SearchApi.SearchUser(keyword, pageNumber)
                    .Then(searchUserResponse => {
                        dispatcher.dispatch(new FollowMapAction {followMap = searchUserResponse.followingMap});
                        var userMap = new Dictionary<string, User>();
                        var searchUserIds = new List<string>();
                        (searchUserResponse.users ?? new List<User>()).ForEach(searchUser => {
                            searchUserIds.Add(item: searchUser.id);
                            if (userMap.ContainsKey(key: searchUser.id)) {
                                userMap[key: searchUser.id] = searchUser;
                            }
                            else {
                                userMap.Add(key: searchUser.id, value: searchUser);
                            }
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        dispatcher.dispatch(new UserLicenseMapAction
                            {userLicenseMap = searchUserResponse.userLicenseMap});
                        dispatcher.dispatch(new SearchUserSuccessAction {
                            keyword = keyword,
                            pageNumber = pageNumber,
                            searchUserIds = searchUserIds,
                            hasMore = searchUserResponse.hasMore
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new SearchUserFailureAction {keyword = keyword});
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object searchFollowings(string keyword, int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return SearchApi.SearchUser(keyword, pageNumber)
                    .Then(searchFollowingResponse => {
                        dispatcher.dispatch(new SearchFollowingSuccessAction {
                            keyword = keyword,
                            pageNumber = pageNumber,
                            users = searchFollowingResponse.users,
                            hasMore = searchFollowingResponse.hasMore
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new SearchFollowingFailureAction {keyword = keyword});
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object popularSearchUser() {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return SearchApi.PopularSearchUser()
                    .Then(popularSearchUsers => {
                        dispatcher.dispatch(new PopularSearchUserSuccessAction {
                            popularSearchUsers = popularSearchUsers
                        });
                    })
                    .Catch(onRejected: Debuger.LogError);
            });
        }

        public static object searchTeams(string keyword, int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return SearchApi.SearchTeam(keyword, pageNumber)
                    .Then(searchTeamResponse => {
                        dispatcher.dispatch(new FollowMapAction {followMap = searchTeamResponse.followingMap});
                        var teamMap = new Dictionary<string, Team>();
                        var searchTeamIds = new List<string>();
                        (searchTeamResponse.teams ?? new List<Team>()).ForEach(searchTeam => {
                            searchTeamIds.Add(item: searchTeam.id);
                            if (teamMap.ContainsKey(key: searchTeam.id)) {
                                teamMap[key: searchTeam.id] = searchTeam;
                            }
                            else {
                                teamMap.Add(key: searchTeam.id, value: searchTeam);
                            }
                        });
                        dispatcher.dispatch(new TeamMapAction {teamMap = teamMap});
                        dispatcher.dispatch(new SearchTeamSuccessAction {
                            keyword = keyword,
                            pageNumber = pageNumber,
                            searchTeamIds = searchTeamIds,
                            hasMore = searchTeamResponse.hasMore
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new SearchTeamFailureAction {keyword = keyword});
                        Debuger.LogError(message: error);
                    });
            });
        }
    }
}