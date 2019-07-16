using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public class TeamMapAction : BaseAction {
        public Dictionary<string, Team> teamMap;
    }

    public class StartFetchTeamAction : RequestAction {
    }

    public class FetchTeamSuccessAction : BaseAction {
        public Team team;
        public string teamId;
    }

    public class FetchTeamFailureAction : BaseAction {
    }

    public class StartFetchTeamArticleAction : RequestAction {
    }

    public class FetchTeamArticleSuccessAction : BaseAction {
        public List<Article> articles;
        public bool hasMore;
        public int offset;
        public string teamId;
    }

    public class FetchTeamArticleFailureAction : BaseAction {
    }

    public class StartFetchTeamFollowerAction : RequestAction {
    }

    public class FetchTeamFollowerSuccessAction : BaseAction {
        public List<User> followers;
        public bool followersHasMore;
        public int offset;
        public string teamId;
    }

    public class FetchTeamFollowerFailureAction : BaseAction {
    }

    public class StartFetchFollowTeamAction : RequestAction {
        public string followTeamId;
    }

    public class FetchFollowTeamSuccessAction : BaseAction {
        public bool success;
        public string currentUserId;
        public string followTeamId;
    }

    public class FetchFollowTeamFailureAction : BaseAction {
    }

    public class StartFetchUnFollowTeamAction : RequestAction {
        public string unFollowTeamId;
    }

    public class FetchUnFollowTeamSuccessAction : BaseAction {
        public bool success;
        public string currentUserId;
        public string unFollowTeamId;
    }

    public class FetchUnFollowTeamFailureAction : BaseAction {
    }

    public static partial class Actions {
        public static object fetchTeam(string teamId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return TeamApi.FetchTeam(teamId)
                    .Then(teamResponse => {
                        if (teamResponse.placeMap != null) {
                            dispatcher.dispatch(new PlaceMapAction {placeMap = teamResponse.placeMap});
                        }
                        dispatcher.dispatch(new FetchTeamSuccessAction {
                            team = teamResponse.team,
                            teamId = teamId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchTeamFailureAction());
                        Debug.Log(error);
                    }
                );
            });
        }

        public static object fetchTeamArticle(string teamId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return TeamApi.FetchTeamArticle(teamId, offset)
                    .Then(teamArticleResponse => {
                        dispatcher.dispatch(new FetchTeamArticleSuccessAction {
                            articles = teamArticleResponse.projects,
                            hasMore = teamArticleResponse.projectsHasMore,
                            offset = offset,
                            teamId = teamId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchTeamArticleFailureAction());
                            Debug.Log(error);
                        }
                    );
            });
        }

        public static object fetchTeamFollower(string teamId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return TeamApi.FetchTeamFollower(teamId, offset)
                    .Then(teamFollowerResponse => {
                        if (teamFollowerResponse.followMap != null) {
                            dispatcher.dispatch(new FollowMapAction {
                                followMap = teamFollowerResponse.followMap
                            });
                        }
                        var userMap = new Dictionary<string, User>();
                        teamFollowerResponse.followers.ForEach(follower => {
                            userMap.Add(key: follower.id, value: follower);
                        });
                        dispatcher.dispatch(new UserMapAction { userMap = userMap });
                        dispatcher.dispatch(new FetchTeamFollowerSuccessAction {
                            followers = teamFollowerResponse.followers,
                            followersHasMore = teamFollowerResponse.followersHasMore,
                            offset = offset,
                            teamId = teamId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchTeamFollowerFailureAction());
                            Debug.Log(error);
                        }
                    );
            });
        }

        public static object fetchFollowTeam(string followTeamId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return TeamApi.FetchFollowTeam(followTeamId)
                    .Then(success => {
                        dispatcher.dispatch(new FetchFollowTeamSuccessAction {
                            success = success,
                            currentUserId = getState().loginState.loginInfo.userId ?? "",
                            followTeamId = followTeamId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchFollowTeamFailureAction ());
                            Debug.Log(error);
                        }
                    );
            });
        }

        public static object fetchUnFollowTeam(string unFollowTeamId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return TeamApi.FetchUnFollowTeam(unFollowTeamId)
                    .Then(success => {
                        dispatcher.dispatch(new FetchUnFollowTeamSuccessAction {
                            success = success,
                            currentUserId = getState().loginState.loginInfo.userId ?? "",
                            unFollowTeamId = unFollowTeamId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchUnFollowTeamFailureAction());
                            Debug.Log(error);
                        }
                    );
            });
        }
    }
}