using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public class StartFetchUserProfileAction : RequestAction {
    }
    
    public class FetchUserProfileSuccessAction : BaseAction {
        public User user;
        public string userId;
    }

    public class FetchUserProfileFailureAction : BaseAction {
    }
    
    public class StartFetchUserArticleAction : RequestAction {
    }
    
    public class FetchUserArticleSuccessAction : BaseAction {
        public List<Article> articles;
        public bool hasMore;
        public int offset;
        public string userId;
    }

    public class FetchUserArticleFailureAction : BaseAction {
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
        public string userId;
    }

    public class FetchFollowingFailureAction : BaseAction {
    }
    
    public class StartFetchFollowerAction : RequestAction {
    }
    
    public class FetchFollowerSuccessAction : BaseAction {
        public List<User> followers;
        public bool followersHasMore;
        public int offset;
        public string userId;
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
        public static object fetchUserProfile(string userId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return UserApi.FetchUserProfile(userId: userId)
                    .Then(userProfileResponse => {
                        if (userProfileResponse.placeMap != null) {
                            dispatcher.dispatch(new PlaceMapAction {placeMap = userProfileResponse.placeMap});
                        }
                        if (userProfileResponse.teamMap != null) {
                            dispatcher.dispatch(new TeamMapAction {teamMap = userProfileResponse.teamMap});
                        }
                        if (userProfileResponse.followMap != null) {
                            dispatcher.dispatch(new FollowMapAction {
                                followMap = userProfileResponse.followMap,
                                userId = getState().loginState.loginInfo.userId ?? ""
                            });
                        }
                        var userDict = getState().userState.userDict;
                        var currentUser = userDict.ContainsKey(key: userId)
                            ? userDict[key: userId]
                            : new User();
                        var user = userProfileResponse.user;
                        user.followingCount = userProfileResponse.followingCount;
                        user.followings = userProfileResponse.followings;
                        user.followingsHasMore = userProfileResponse.followingsHasMore;
                        user.followers = userProfileResponse.followers;
                        user.followersHasMore = userProfileResponse.followersHasMore;
                        user.articles = currentUser.articles;
                        user.articlesHasMore = currentUser.articlesHasMore;
                        user.jobRoleMap = userProfileResponse.jobRoleMap;
                        dispatcher.dispatch(new FetchUserProfileSuccessAction {
                            user = user,
                            userId = userId
                        });
                        dispatcher.dispatch(new StartFetchUserArticleAction());
                        dispatcher.dispatch(fetchUserArticle(userId, 0));
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchUserProfileFailureAction());
                        Debug.Log(error);
                    }
                );
            });
        }
        
        public static object fetchUserArticle(string userId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return UserApi.FetchUserArticle(userId, offset)
                    .Then(userArticleResponse => {
                        var articles = new List<Article>();
                        userArticleResponse.projectList.ForEach(articleId => {
                            if (userArticleResponse.projectMap.ContainsKey(key: articleId)) {
                                var article = userArticleResponse.projectMap[key: articleId];
                                articles.Add(article);
                            }
                        });
                        dispatcher.dispatch(new PlaceMapAction {placeMap = userArticleResponse.placeMap});
                        dispatcher.dispatch(new UserMapAction {userMap = userArticleResponse.userMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = userArticleResponse.teamMap});
                        dispatcher.dispatch(new FetchUserArticleSuccessAction {
                            articles = articles,
                            hasMore = userArticleResponse.hasMore,
                            offset = offset,
                            userId = userId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchUserArticleFailureAction());
                            Debug.Log(error);
                        }
                    );
            });
        }
        
        public static object fetchFollowUser(string followUserId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return UserApi.FetchFollowUser(followUserId)
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
                return UserApi.FetchUnFollowUser(unFollowUserId)
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
        
        public static object fetchFollowing(string userId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return UserApi.FetchFollowing(userId, offset)
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
                            userId = userId
                        });
                    })
                    .Catch(error => {
                            dispatcher.dispatch(new FetchFollowingFailureAction());
                            Debug.Log(error);
                        }
                    );
            });
        }

        public static object fetchFollower(string userId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return UserApi.FetchFollower(userId, offset)
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
                            userId = userId
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
                return UserApi.EditPersonalInfo(fullName, title, jobRoleId, placeId)
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