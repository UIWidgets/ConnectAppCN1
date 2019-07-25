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

    public class StartFetchTeamMemberAction : RequestAction {
    }

    public class FetchTeamMemberSuccessAction : BaseAction {
        public List<Member> members;
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
                        dispatcher.dispatch(new PlaceMapAction {placeMap = teamResponse.placeMap});
                        dispatcher.dispatch(new FollowMapAction {followMap = teamResponse.followMap});
                        var teamDict = getState().teamState.teamDict;
                        var currentTeam = teamDict.ContainsKey(key: teamId)
                            ? teamDict[key: teamId]
                            : new Team();
                        var team = teamResponse.team;
                        dispatcher.dispatch(new FetchTeamSuccessAction {
                            team = currentTeam.Merge(other: team),
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
                var team = getState().teamState.teamDict.ContainsKey(key: teamId)
                    ? getState().teamState.teamDict[key: teamId]
                    : null;
                var articleOffset = team == null ? 0 : team.articleIds == null ? 0 : team.articleIds.Count;
                if (offset != 0 && offset != articleOffset) {
                    offset = articleOffset;
                }
                return TeamApi.FetchTeamArticle(teamId, offset)
                    .Then(teamArticleResponse => {
                        dispatcher.dispatch(new LikeMapAction {likeMap = teamArticleResponse.likeMap});
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
                            dispatcher.dispatch(new FetchFollowTeamFailureAction {followTeamId = followTeamId});
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
                            dispatcher.dispatch(new FetchUnFollowTeamFailureAction {unFollowTeamId = unFollowTeamId});
                            Debug.Log(error);
                        }
                    );
            });
        }
    }
}