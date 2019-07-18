using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Utils;
using Newtonsoft.Json;
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
        public string userId;
        public string errorCode;
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
        public string followUserId;
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
        public string unFollowUserId;
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
                        dispatcher.dispatch(new PlaceMapAction {placeMap = userProfileResponse.placeMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = userProfileResponse.teamMap});
                        var userMap = new Dictionary<string, User>();
                        (userProfileResponse.followings ?? new List<User>()).ForEach(followingUser => {
                            if (userMap.ContainsKey(key: followingUser.id)) {
                                userMap[key: followingUser.id] = followingUser;
                            }
                            else {
                                userMap.Add(key: followingUser.id, value: followingUser);
                            }
                        });
                        (userProfileResponse.followers ?? new List<User>()).ForEach(followerUser => {
                            if (userMap.ContainsKey(key: followerUser.id)) {
                                userMap[key: followerUser.id] = followerUser;
                            }
                            else {
                                userMap.Add(key: followerUser.id, value: followerUser);
                            }
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        if (userProfileResponse.followMap != null) {
                            dispatcher.dispatch(new FollowMapAction {
                                followMap = userProfileResponse.followMap
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
                    })
                    .Catch(error => {
                        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(error.Message);
                        var errorCode = errorResponse.errorCode;
                        dispatcher.dispatch(new FetchUserProfileFailureAction {
                            userId = userId,
                            errorCode = errorCode
                        });
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
                                articles.Add(item: article);
                            }
                        });
                        dispatcher.dispatch(new PlaceMapAction {placeMap = userArticleResponse.placeMap});
                        dispatcher.dispatch(new UserMapAction {userMap = userArticleResponse.userMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = userArticleResponse.teamMap});
                        dispatcher.dispatch(new FollowMapAction {followMap = userArticleResponse.followMap});
                        dispatcher.dispatch(new LikeMapAction {likeMap = userArticleResponse.likeMap});
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
                            dispatcher.dispatch(new FetchFollowUserFailureAction {followUserId = followUserId});
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
                            dispatcher.dispatch(new FetchUnFollowUserFailureAction {unFollowUserId = unFollowUserId});
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
                                followMap = followingResponse.followMap
                            });
                        }
                        var userMap = new Dictionary<string, User>();
                        followingResponse.followings.ForEach(following => {
                            userMap.Add(key: following.id, value: following);
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
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
                                followMap = followerResponse.followMap
                            });
                        }
                        var userMap = new Dictionary<string, User>();
                        followerResponse.followers.ForEach(follower => {
                            userMap.Add(key: follower.id, value: follower);
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
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
                        var oldLoginInfo = getState().loginState.loginInfo;
                        var loginInfo = new LoginInfo {
                            LSKey = oldLoginInfo.LSKey,
                            userId = oldLoginInfo.userId,
                            userFullName = editPersonalInfoResponse.user.fullName,
                            userAvatar = editPersonalInfoResponse.user.avatar,
                            authId = oldLoginInfo.authId,
                            anonymous = oldLoginInfo.anonymous,
                            title = editPersonalInfoResponse.user.title,
                            coverImageWithCDN = editPersonalInfoResponse.user.coverImage
                        };
                        UserInfoManager.saveUserInfo(loginInfo: loginInfo);
                    })
                    .Catch(error => Debug.Log($"{error}")
                    );
            });
        }
    }
}