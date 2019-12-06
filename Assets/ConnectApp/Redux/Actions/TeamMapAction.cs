using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.Redux;

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
        public int pageNumber;
        public string teamId;
    }

    public class FetchTeamArticleFailureAction : BaseAction {
        public string teamId;
        public string errorCode;
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

    public class StartFetchTeamMemberAction : RequestAction {
    }

    public class FetchTeamMemberSuccessAction : BaseAction {
        public List<TeamMember> members;
        public bool membersHasMore;
        public int pageNumber;
        public string teamId;
    }

    public class FetchTeamMemberFailureAction : BaseAction {
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
        public string followTeamId;
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
        public string unFollowTeamId;
    }

    public static partial class Actions {
        public static object fetchTeam(string teamId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return TeamApi.FetchTeam(teamId)
                    .Then(teamResponse => {
                        dispatcher.dispatch<IPromise>(fetchTeamArticle(teamId: teamResponse.team.id, 1));
                        dispatcher.dispatch(new PlaceMapAction {placeMap = teamResponse.placeMap});
                        dispatcher.dispatch(new FollowMapAction {followMap = teamResponse.followMap});
                        dispatcher.dispatch(new FetchTeamSuccessAction {
                            team = teamResponse.team,
                            teamId = teamId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchTeamFailureAction());
                            Debuger.LogError(message: error);
                        }
                    );
            });
        }

        public static object fetchTeamArticle(string teamId, int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return TeamApi.FetchTeamArticle(teamId: teamId, pageNumber: pageNumber)
                    .Then(teamArticleResponse => {
                        var articles = teamArticleResponse.projects.FindAll(project => "article" == project.type);
                        dispatcher.dispatch(new LikeMapAction {likeMap = teamArticleResponse.likeMap});
                        dispatcher.dispatch(new FetchTeamArticleSuccessAction {
                            articles = articles,
                            hasMore = teamArticleResponse.projectsHasMore,
                            pageNumber = pageNumber,
                            teamId = teamId
                        });
                    })
                    .Catch(error => {
                            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(value: error.Message);
                            var errorCode = errorResponse.errorCode;
                            dispatcher.dispatch(new FetchTeamArticleFailureAction {
                                teamId = teamId,
                                errorCode = errorCode
                            });
                            Debuger.LogError(message: error);
                        }
                    );
            });
        }

        public static object fetchTeamFollower(string teamId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var team = getState().teamState.teamDict.ContainsKey(key: teamId)
                    ? getState().teamState.teamDict[key: teamId]
                    : new Team();
                var followerOffset = (team.followers ?? new List<User>()).Count;
                if (offset != 0 && offset != followerOffset) {
                    offset = followerOffset;
                }

                return TeamApi.FetchTeamFollower(teamId, offset)
                    .Then(teamFollowerResponse => {
                        dispatcher.dispatch(new FollowMapAction {followMap = teamFollowerResponse.followMap});
                        var userMap = new Dictionary<string, User>();
                        teamFollowerResponse.followers.ForEach(follower => {
                            userMap.Add(key: follower.id, value: follower);
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        dispatcher.dispatch(new FetchTeamFollowerSuccessAction {
                            followers = teamFollowerResponse.followers,
                            followersHasMore = teamFollowerResponse.followersHasMore,
                            offset = offset,
                            teamId = teamId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchTeamFollowerFailureAction());
                            Debuger.LogError(message: error);
                        }
                    );
            });
        }

        public static object fetchTeamMember(string teamId, int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return TeamApi.FetchTeamMember(teamId, pageNumber)
                    .Then(teamMemberResponse => {
                        dispatcher.dispatch(new FollowMapAction {followMap = teamMemberResponse.followMap});
                        dispatcher.dispatch(new UserMapAction {userMap = teamMemberResponse.userMap});
                        dispatcher.dispatch(new FetchTeamMemberSuccessAction {
                            members = teamMemberResponse.members,
                            membersHasMore = teamMemberResponse.hasMore,
                            pageNumber = pageNumber,
                            teamId = teamId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchTeamMemberFailureAction());
                            Debuger.LogError(message: error);
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
                            dispatcher.dispatch(new FetchFollowTeamFailureAction {followTeamId = followTeamId});
                            Debuger.LogError(message: error);
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
                            dispatcher.dispatch(new FetchUnFollowTeamFailureAction {unFollowTeamId = unFollowTeamId});
                            Debuger.LogError(message: error);
                        }
                    );
            });
        }
    }
}