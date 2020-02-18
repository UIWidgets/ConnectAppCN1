using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.Redux;

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
        public int pageNumber;
        public string userId;
    }

    public class FetchUserArticleFailureAction : BaseAction {
    }

    public class StartFetchUserLikeArticleAction : RequestAction {
    }

    public class FetchUserLikeArticleSuccessAction : BaseAction {
        public List<Article> articles;
        public bool hasMore;
        public int pageNumber;
        public string userId;
    }

    public class FetchUserLikeArticleFailureAction : BaseAction {
    }

    public class StartFollowUserAction : RequestAction {
        public string followUserId;
    }

    public class FollowUserSuccessAction : BaseAction {
        public bool success;
        public string currentUserId;
        public string followUserId;
    }

    public class FollowUserFailureAction : BaseAction {
        public string followUserId;
    }

    public class StartUnFollowUserAction : RequestAction {
        public string unFollowUserId;
    }

    public class UnFollowUserSuccessAction : BaseAction {
        public bool success;
        public string currentUserId;
        public string unFollowUserId;
    }

    public class UnFollowUserFailureAction : BaseAction {
        public string unFollowUserId;
    }

    public class StartFetchFollowingAction : RequestAction {
    }

    public class FetchFollowingSuccessAction : BaseAction {
        public List<Following> followings;
        public bool followingHasMore;
        public int offset;
        public string userId;
    }

    public class FetchFollowingFailureAction : BaseAction {
    }

    public class StartFetchFollowingUserAction : RequestAction {
    }

    public class FetchFollowingUserSuccessAction : BaseAction {
        public List<User> followingUsers;
        public bool followingUsersHasMore;
        public int offset;
        public string userId;
    }

    public class FetchFollowingUserFailureAction : BaseAction {
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

    public class StartFetchFollowingTeamAction : RequestAction {
    }

    public class FetchFollowingTeamSuccessAction : BaseAction {
        public List<Team> followingTeams;
        public bool followingTeamsHasMore;
        public int offset;
        public string userId;
    }

    public class FetchFollowingTeamFailureAction : BaseAction {
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

    public class UpdateAvatarSuccessAction : BaseAction {
        public string avatar;
    }

    public static partial class Actions {
        public static object fetchUserProfile(string userId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return UserApi.FetchUserProfile(userId: userId)
                    .Then(userProfileResponse => {
                        dispatcher.dispatch<IPromise>(
                            fetchUserArticle(userId: userProfileResponse.user.id, 1));
                        dispatcher.dispatch<IPromise>(action: fetchFavoriteTags(userId: userProfileResponse.user.id,
                            offset: 0));
                        dispatcher.dispatch<IPromise>(fetchFollowFavoriteTags(userId: userProfileResponse.user.id,0));
                        dispatcher.dispatch(new PlaceMapAction {placeMap = userProfileResponse.placeMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = userProfileResponse.teamMap});
                        dispatcher.dispatch(new FollowMapAction {followMap = userProfileResponse.followMap});
                        dispatcher.dispatch(new UserLicenseMapAction
                            {userLicenseMap = userProfileResponse.userLicenseMap});
                        var user = userProfileResponse.user;
                        user.followingUsersCount = userProfileResponse.followingCount;
                        user.followingUsersHasMore = userProfileResponse.followingsHasMore;
                        user.followersHasMore = userProfileResponse.followersHasMore;
                        user.followingTeamsCount = userProfileResponse.followingTeamsCount;
                        user.followingTeams = userProfileResponse.followingTeams;
                        user.followingTeamsHasMore = userProfileResponse.followingTeamsHasMore;
                        user.jobRoleMap = userProfileResponse.jobRoleMap;
                        dispatcher.dispatch(new FetchUserProfileSuccessAction {
                            user = user,
                            userId = userId
                        });
                    })
                    .Catch(error => {
                            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(value: error.Message);
                            var errorCode = errorResponse.errorCode;
                            dispatcher.dispatch(new FetchUserProfileFailureAction {
                                userId = userId,
                                errorCode = errorCode
                            });
                            Debuger.LogError(message: error);
                        }
                    );
            });
        }

        public static object fetchUserArticle(string userId, int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return UserApi.FetchUserArticle(userId: userId, pageNumber: pageNumber)
                    .Then(userArticleResponse => {
                        var articles = new List<Article>();
                        userArticleResponse.projectList.ForEach(articleId => {
                            if (userArticleResponse.projectMap.ContainsKey(key: articleId)) {
                                var article = userArticleResponse.projectMap[key: articleId];
                                if (article.type == "article") {
                                    articles.Add(item: article);
                                }
                            }
                        });
                        dispatcher.dispatch(new PlaceMapAction {placeMap = userArticleResponse.placeMap});
                        dispatcher.dispatch(new FollowMapAction {followMap = userArticleResponse.followMap});
                        dispatcher.dispatch(new LikeMapAction {likeMap = userArticleResponse.likeMap});
                        dispatcher.dispatch(new FetchUserArticleSuccessAction {
                            articles = articles,
                            hasMore = userArticleResponse.hasMore,
                            pageNumber = pageNumber,
                            userId = userId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchUserArticleFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchUserLikeArticle(string userId, int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return UserApi.FetchUserLikeArticle(userId: userId, pageNumber: pageNumber)
                    .Then(userLikeArticleResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = userLikeArticleResponse.userSimpleV2Map});
                        dispatcher.dispatch(new TeamMapAction {teamMap = userLikeArticleResponse.teamSimpleMap});
                        dispatcher.dispatch(new FetchUserLikeArticleSuccessAction {
                            articles = userLikeArticleResponse.projectSimpleList,
                            hasMore = userLikeArticleResponse.projectSimpleList.isNotNullAndEmpty(),
                            pageNumber = userLikeArticleResponse.currentPage,
                            userId = userId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchUserLikeArticleFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchFollowUser(string followUserId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return UserApi.FetchFollowUser(userId: followUserId)
                    .Then(success => {
                        dispatcher.dispatch(new FollowUserSuccessAction {
                            success = success,
                            currentUserId = getState().loginState.loginInfo.userId ?? "",
                            followUserId = followUserId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FollowUserFailureAction {followUserId = followUserId});
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchUnFollowUser(string unFollowUserId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return UserApi.FetchUnFollowUser(userId: unFollowUserId)
                    .Then(success => {
                        dispatcher.dispatch(new UnFollowUserSuccessAction {
                            success = success,
                            currentUserId = getState().loginState.loginInfo.userId ?? "",
                            unFollowUserId = unFollowUserId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new UnFollowUserFailureAction {unFollowUserId = unFollowUserId});
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchFollowing(string userId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return UserApi.FetchFollowing(userId: userId, offset: offset)
                    .Then(followingResponse => {
                        dispatcher.dispatch(new FollowMapAction {followMap = followingResponse.followMap});
                        dispatcher.dispatch(new UserMapAction {userMap = followingResponse.userMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = followingResponse.teamMap});
                        dispatcher.dispatch(new FetchFollowingSuccessAction {
                            followings = followingResponse.followings,
                            followingHasMore = followingResponse.hasMore,
                            offset = offset,
                            userId = userId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchFollowingFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchFollowingUser(string userId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var user = getState().userState.userDict.ContainsKey(key: userId)
                    ? getState().userState.userDict[key: userId]
                    : new User();
                var followingOffset = (user.followingUsers ?? new List<User>()).Count;
                if (offset != 0 && offset != followingOffset) {
                    offset = followingOffset;
                }

                return UserApi.FetchFollowingUser(userId: userId, offset: offset)
                    .Then(followingUserResponse => {
                        dispatcher.dispatch(new FollowMapAction {followMap = followingUserResponse.followMap});
                        var userMap = new Dictionary<string, User>();
                        followingUserResponse.followings.ForEach(followingUser => {
                            userMap.Add(key: followingUser.id, value: followingUser);
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        dispatcher.dispatch(new UserLicenseMapAction
                            {userLicenseMap = followingUserResponse.userLicenseMap});
                        dispatcher.dispatch(new FetchFollowingUserSuccessAction {
                            followingUsers = followingUserResponse.followings,
                            followingUsersHasMore = followingUserResponse.followingsHasMore,
                            offset = offset,
                            userId = userId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchFollowingUserFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchFollower(string userId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var user = getState().userState.userDict.ContainsKey(key: userId)
                    ? getState().userState.userDict[key: userId]
                    : new User();
                var followerOffset = (user.followers ?? new List<User>()).Count;
                if (offset != 0 && offset != followerOffset) {
                    offset = followerOffset;
                }

                return UserApi.FetchFollower(userId: userId, offset: offset)
                    .Then(followerResponse => {
                        dispatcher.dispatch(new FollowMapAction {followMap = followerResponse.followMap});
                        var userMap = new Dictionary<string, User>();
                        followerResponse.followers.ForEach(follower => {
                            userMap.Add(key: follower.id, value: follower);
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        dispatcher.dispatch(new UserLicenseMapAction
                            {userLicenseMap = followerResponse.userLicenseMap});
                        dispatcher.dispatch(new FetchFollowerSuccessAction {
                            followers = followerResponse.followers,
                            followersHasMore = followerResponse.followersHasMore,
                            offset = offset,
                            userId = userId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchFollowerFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchFollowingTeam(string userId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var user = getState().userState.userDict.ContainsKey(key: userId)
                    ? getState().userState.userDict[key: userId]
                    : new User();
                var followingOffset = (user.followingTeams ?? new List<Team>()).Count;
                if (offset != 0 && offset != followingOffset) {
                    offset = followingOffset;
                }

                return UserApi.FetchFollowingTeam(userId: userId, offset: offset)
                    .Then(followingTeamResponse => {
                        dispatcher.dispatch(new FollowMapAction {followMap = followingTeamResponse.followMap});
                        dispatcher.dispatch(new PlaceMapAction {placeMap = followingTeamResponse.placeMap});
                        var teamMap = new Dictionary<string, Team>();
                        followingTeamResponse.followingTeams.ForEach(followingTeam => {
                            teamMap.Add(key: followingTeam.id, value: followingTeam);
                        });
                        dispatcher.dispatch(new TeamMapAction {teamMap = teamMap});
                        dispatcher.dispatch(new FetchFollowingTeamSuccessAction {
                            followingTeams = followingTeamResponse.followingTeams,
                            followingTeamsHasMore = followingTeamResponse.followingTeamsHasMore,
                            offset = offset,
                            userId = userId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchFollowingTeamFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object editPersonalInfo(string fullName, string title, string jobRoleId, string placeId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var userId = getState().loginState.loginInfo.userId ?? "";
                return UserApi.EditPersonalInfo(userId: userId, fullName: fullName, title: title, jobRoleId: jobRoleId,
                        placeId: placeId)
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
                    });
            });
        }

        public static object updateAvatar(string image) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var userId = getState().loginState.loginInfo.userId ?? "";
                return UserApi.UpdateAvatar(userId: userId, avatar: image).Then(response => {
                    StoreProvider.store.dispatcher.dispatch(new UpdateAvatarSuccessAction {
                        avatar = response.avatar
                    });
                });
            });
        }
    }
}