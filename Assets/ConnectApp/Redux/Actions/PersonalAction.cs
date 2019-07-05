using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public class StartFetchPersonalAction : RequestAction {
    }
    
    public class FetchPersonalSuccessAction : BaseAction {
        public Personal personal;
        public string personalId;
    }

    public class FetchPersonalFailureAction : BaseAction {
    }
    
    public class StartFetchPersonalArticleAction : RequestAction {
    }
    
    public class FetchPersonalArticleSuccessAction : BaseAction {
        public List<Article> articles;
        public bool hasMore;
        public int offset;
        public string personalId;
    }

    public class FetchPersonalArticleFailureAction : BaseAction {
    }
    
    public class StartFetchFollowUserAction : RequestAction {
        public string followUserId;
    }
    
    public class FetchFollowUserSuccessAction : BaseAction {
        public bool success;
        public string currentUserId;
        public string followUserId;
    }

    public class FetchFollowUserFailureAction : BaseAction {
    }
    
    public class StartFetchUnFollowUserAction : RequestAction {
        public string unFollowUserId;
    }
    
    public class FetchUnFollowUserSuccessAction : BaseAction {
        public bool success;
        public string currentUserId;
        public string unFollowUserId;
    }

    public class FetchUnFollowUserFailureAction : BaseAction {
    }
    
    public class StartFetchFollowingAction : RequestAction {
    }
    
    public class FetchFollowingSuccessAction : BaseAction {
        public List<User> followings;
        public bool followingsHasMore;
        public int offset;
        public string personalId;
    }

    public class FetchFollowingFailureAction : BaseAction {
    }
    
    public class StartFetchFollowerAction : RequestAction {
    }
    
    public class FetchFollowerSuccessAction : BaseAction {
        public List<User> followers;
        public bool followersHasMore;
        public int offset;
        public string personalId;
    }

    public class FetchFollowerFailureAction : BaseAction {
    }
    
    public class ChangePersonalFullNameAction : BaseAction {
        public string fullName;
    }
    
    public class ChangePersonalTitleAction : BaseAction {
        public string title;
    }
    
    public class ChangePersonalRoleAction : BaseAction {
        public JobRole jobRole;
    }
    
    public class CleanPersonalInfoAction : BaseAction {
    }

    public class EditPersonalInfoSuccessAction : BaseAction {
        public User user;
    }
    
    public static partial class Actions {
        public static object fetchPersonal(string personalId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return PersonalApi.FetchPersonal(personalId)
                    .Then(personalResponse => {
                        dispatcher.dispatch(new StartFetchPersonalArticleAction());
                        dispatcher.dispatch(fetchPersonalArticle(personalId, 0));
                        if (personalResponse.placeMap != null) {
                            dispatcher.dispatch(new PlaceMapAction {placeMap = personalResponse.placeMap});
                        }
                        if (personalResponse.teamMap != null) {
                            dispatcher.dispatch(new TeamMapAction {teamMap = personalResponse.teamMap});
                        }
                        if (personalResponse.followMap != null) {
                            dispatcher.dispatch(new FollowMapAction {
                                followMap = personalResponse.followMap,
                                userId = getState().loginState.loginInfo.userId ?? ""
                            });
                        }
                        var personalDict = getState().personalState.personalDict;
                        var personal = personalDict.ContainsKey(personalId)
                            ? personalDict[personalId]
                            : new Personal();
                        
                        dispatcher.dispatch(new FetchPersonalSuccessAction {
                            personal = new Personal {
                                user = personalResponse.user,
                                followingCount = personalResponse.followingCount,
                                followings = personalResponse.followings,
                                followingsHasMore = personalResponse.followingsHasMore,
                                followers = personalResponse.followers,
                                followersHasMore = personalResponse.followersHasMore,
                                articles = personal.articles ?? new List<Article>(),
                                articlesHasMore = personal.articlesHasMore,
                                currentUserId = personalResponse.currentUserId,
                                jobRoleMap = personalResponse.jobRoleMap
                            },
                            personalId = personalId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchPersonalFailureAction());
                        Debug.Log(error);
                    }
                );
            });
        }
        
        public static object fetchPersonalArticle(string personalId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return PersonalApi.FetchPersonalArticle(personalId, offset)
                    .Then(personalArticleResponse => {
                        var articles = new List<Article>();
                        personalArticleResponse.projectList.ForEach(articleId => {
                            if (personalArticleResponse.projectMap.ContainsKey(articleId)) {
                                var article = personalArticleResponse.projectMap[articleId];
                                articles.Add(article);
                            }
                        });
                        dispatcher.dispatch(new PlaceMapAction {placeMap = personalArticleResponse.placeMap});
                        dispatcher.dispatch(new UserMapAction {userMap = personalArticleResponse.userMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = personalArticleResponse.teamMap});
                        dispatcher.dispatch(new FetchPersonalArticleSuccessAction {
                            articles = articles,
                            hasMore = personalArticleResponse.hasMore,
                            offset = offset,
                            personalId = personalId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchPersonalArticleFailureAction());
                            Debug.Log(error);
                        }
                    );
            });
        }
        
        public static object fetchFollowUser(string followUserId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return PersonalApi.FetchFollowUser(followUserId)
                    .Then(success => {
                        dispatcher.dispatch(new FetchFollowUserSuccessAction {
                            success = success,
                            currentUserId = getState().loginState.loginInfo.userId ?? "",
                            followUserId = followUserId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchFollowUserFailureAction ());
                            Debug.Log(error);
                        }
                    );
            });
        }
        
        public static object fetchUnFollowUser(string unFollowUserId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return PersonalApi.FetchUnFollowUser(unFollowUserId)
                    .Then(success => {
                        dispatcher.dispatch(new FetchUnFollowUserSuccessAction {
                            success = success,
                            currentUserId = getState().loginState.loginInfo.userId ?? "",
                            unFollowUserId = unFollowUserId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchUnFollowUserFailureAction());
                            Debug.Log(error);
                        }
                    );
            });
        }
        
        public static object fetchFollowing(string personalId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return PersonalApi.FetchFollowing(personalId, offset)
                    .Then(followingResponse => {
                        if (followingResponse.followMap != null) {
                            dispatcher.dispatch(new FollowMapAction {
                                followMap = followingResponse.followMap,
                                userId = getState().loginState.loginInfo.userId ?? ""
                            });
                        }
                        dispatcher.dispatch(new FetchFollowingSuccessAction {
                            followings = followingResponse.followings,
                            followingsHasMore = followingResponse.followingsHasMore,
                            offset = offset,
                            personalId = personalId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchFollowingFailureAction());
                            Debug.Log(error);
                        }
                    );
            });
        }

        public static object fetchFollower(string personalId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return PersonalApi.FetchFollower(personalId, offset)
                    .Then(followerResponse => {
                        if (followerResponse.followMap != null) {
                            dispatcher.dispatch(new FollowMapAction {
                                followMap = followerResponse.followMap,
                                userId = getState().loginState.loginInfo.userId ?? ""
                            });
                        }
                        dispatcher.dispatch(new FetchFollowerSuccessAction {
                            followers = followerResponse.followers,
                            followersHasMore = followerResponse.followersHasMore,
                            offset = offset,
                            personalId = personalId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchFollowerFailureAction());
                            Debug.Log(error);
                        }
                    );
            });
        }

        public static object editPersonalInfo(string fullName, string title, string jobRoleId, string placeId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return PersonalApi.EditPersonalInfo(fullName, title, jobRoleId, placeId)
                    .Then(editPersonalInfoResponse => {
                        if (editPersonalInfoResponse.placeMap != null) {
                            dispatcher.dispatch(new PlaceMapAction {placeMap = editPersonalInfoResponse.placeMap});
                        }
                        dispatcher.dispatch(new EditPersonalInfoSuccessAction {
                            user = editPersonalInfoResponse.user
                        });
                    })
                    .Catch(error => Debug.Log($"{error}")
                    );
            });
        }
    }
}