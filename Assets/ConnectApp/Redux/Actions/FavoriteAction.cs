using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class FavoriteTagMapAction : BaseAction {
        public Dictionary<string, FavoriteTag> favoriteTagMap;
    }

    public class MyFavoriteTagMapAction : BaseAction {
        public Dictionary<string, FavoriteTag> favoriteTagMap;
    }

    public class StartFetchFavoriteTagAction : RequestAction {
    }

    public class FetchFavoriteTagSuccessAction : BaseAction {
        public List<string> favoriteTagIds;
        public string userId;
        public bool hasMore;
        public int offset;
    }

    public class FetchFavoriteTagFailureAction : BaseAction {
    }

    public class StartFetchFollowFavoriteTagAction : RequestAction {
    }

    public class FetchFollowFavoriteTagSuccessAction : BaseAction {
        public List<string> favoriteTagIds;
        public string userId;
        public bool hasMore;
        public int offset;
    }

    public class FetchFollowFavoriteTagFailureAction : BaseAction {
    }

    public class StartFetchFavoriteDetailAction : RequestAction {
    }

    public class FavoriteTagArticleMapAction : BaseAction {
        public Dictionary<string, FavoriteTagArticle> favoriteTagArticleMap;
    }

    public class CollectedTagMapAction : BaseAction {
        public Dictionary<string, bool> collectedTagMap;
    }

    public class FetchFavoriteDetailSuccessAction : BaseAction {
        public List<Favorite> favorites;
        public bool hasMore;
        public string tagId;
        public string userId;
        public int offset;
    }

    public class FetchFavoriteDetailFailureAction : BaseAction {
    }

    public class CreateFavoriteTagSuccessAction : BaseAction {
        public FavoriteTag favoriteTag;
        public bool isCollection;
    }

    public class EditFavoriteTagSuccessAction : BaseAction {
        public FavoriteTag favoriteTag;
    }

    public class DeleteFavoriteTagSuccessAction : BaseAction {
        public FavoriteTag favoriteTag;
    }

    public class ChangeFavoriteTagStateAction : BaseAction {
        public bool isLoading;
    }

    public class CollectFavoriteTagSuccessAction : BaseAction {
        public string myFavoriteTagId;
        public string rankDataId;
        public string itemId;
        public string tagId;
    }

    public class CancelCollectFavoriteTagSuccessAction : BaseAction {
        public string itemId;
    }

    public static partial class Actions {
        public static object fetchFavoriteTags(string userId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var favoriteTagIds = getState().favoriteState.favoriteTagIdDict.ContainsKey(key: userId)
                    ? getState().favoriteState.favoriteTagIdDict[key: userId]
                    : new List<string>();
                var favoriteTagIdCount = favoriteTagIds.Count;
                if (offset != 0 && offset != favoriteTagIdCount) {
                    offset = favoriteTagIdCount;
                }

                return FavoriteApi.FetchMyFavoriteTags(userId: userId, offset: offset)
                    .Then(favoritesResponse => {
                        var newFavoriteTagIds = new List<string>();
                        var favoriteTagMap = new Dictionary<string, FavoriteTag>();
                        favoritesResponse.favoriteTags.ForEach(favoriteTag => {
                            newFavoriteTagIds.Add(item: favoriteTag.id);
                            favoriteTagMap.Add(key: favoriteTag.id, value: favoriteTag);
                        });
                        dispatcher.dispatch(new CollectedTagMapAction {
                            collectedTagMap = favoritesResponse.collectedMap
                        });
                        dispatcher.dispatch(new FavoriteTagMapAction {favoriteTagMap = favoriteTagMap});
                        dispatcher.dispatch(new FetchFavoriteTagSuccessAction {
                            userId = userId,
                            offset = offset,
                            hasMore = favoritesResponse.hasMore,
                            favoriteTagIds = newFavoriteTagIds
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchFavoriteTagFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchFollowFavoriteTags(string userId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var favoriteTagIds = getState().favoriteState.followFavoriteTagIdDict.ContainsKey(key: userId)
                    ? getState().favoriteState.followFavoriteTagIdDict[key: userId]
                    : new List<string>();
                var favoriteTagIdCount = favoriteTagIds.Count;
                if (offset != 0 && offset != favoriteTagIdCount) {
                    offset = favoriteTagIdCount;
                }

                return FavoriteApi.FetchFollowFavoriteTags(userId: userId, offset: offset)
                    .Then(favoritesResponse => {
                        var newFavoriteTagIds = new List<string>();
                        var favoriteTagMap = new Dictionary<string, FavoriteTag>();
                        favoritesResponse.favoriteTags.ForEach(favoriteTag => {
                            newFavoriteTagIds.Add(item: favoriteTag.id);
                            favoriteTagMap.Add(key: favoriteTag.id, value: favoriteTag);
                        });
                        dispatcher.dispatch(new CollectedTagMapAction {
                            collectedTagMap = favoritesResponse.collectedMap
                        });
                        if (favoritesResponse.myFavoriteTagMap.isNotNullAndEmpty()) {
                            dispatcher.dispatch(new MyFavoriteTagMapAction
                                {favoriteTagMap = favoritesResponse.myFavoriteTagMap});
                        }

                        dispatcher.dispatch(new FavoriteTagMapAction {favoriteTagMap = favoriteTagMap});
                        dispatcher.dispatch(new FetchFollowFavoriteTagSuccessAction {
                            userId = userId,
                            offset = offset,
                            hasMore = favoritesResponse.hasMore,
                            favoriteTagIds = newFavoriteTagIds
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchFollowFavoriteTagFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchFavoriteDetail(string userId, string tagId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var favoriteTagId = tagId.isNotEmpty() ? tagId : $"{userId}all";
                var favoriteDetailArticleIds =
                    getState().favoriteState.favoriteDetailArticleIdDict.ContainsKey(key: favoriteTagId)
                        ? getState().favoriteState.favoriteDetailArticleIdDict[key: favoriteTagId]
                        : new List<string>();
                var favoriteDetailArticleCount = favoriteDetailArticleIds.Count;
                if (offset != 0 && offset != favoriteDetailArticleCount) {
                    offset = favoriteDetailArticleCount;
                }

                return FavoriteApi.FetchFavoriteDetail(userId: userId, tagId: tagId, offset: offset)
                    .Then(favoriteDetailResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = favoriteDetailResponse.userMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = favoriteDetailResponse.teamMap});
                        dispatcher.dispatch(new FavoriteTagMapAction {favoriteTagMap = favoriteDetailResponse.tagMap});
                        dispatcher.dispatch(new ArticleMapAction
                            {articleMap = favoriteDetailResponse.projectSimpleMap});
                        dispatcher.dispatch(new FetchFavoriteDetailSuccessAction {
                            favorites = favoriteDetailResponse.favorites,
                            hasMore = favoriteDetailResponse.hasMore,
                            tagId = tagId,
                            userId = userId,
                            offset = offset
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchFavoriteDetailFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object createFavoriteTag(IconStyle iconStyle, string name, string description = "") {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            CustomDialogUtils.showCustomDialog(
                child: new CustomLoadingDialog(
                    message: "新建收藏夹中"
                )
            );
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return FavoriteApi.CreateFavoriteTag(iconStyle: iconStyle, name: name, description: description)
                    .Then(createFavoriteTagResponse => {
                        CustomDialogUtils.hiddenCustomDialog();
                        dispatcher.dispatch(new CreateFavoriteTagSuccessAction {
                            favoriteTag = createFavoriteTagResponse,
                            isCollection = false
                        });
                        dispatcher.dispatch(new MainNavigatorPopAction());
                        AnalyticsManager.AnalyticsHandleFavoriteTag(type: FavoriteTagType.create);
                    })
                    .Catch(error => {
                        CustomDialogUtils.hiddenCustomDialog();
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object editFavoriteTag(IconStyle iconStyle, string tagId, string name, string description = "") {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            CustomDialogUtils.showCustomDialog(
                child: new CustomLoadingDialog(
                    message: "编辑收藏夹中"
                )
            );
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return FavoriteApi
                    .EditFavoriteTag(tagId: tagId, iconStyle: iconStyle, name: name, description: description)
                    .Then(editFavoriteTagResponse => {
                        CustomDialogUtils.hiddenCustomDialog();
                        dispatcher.dispatch(new EditFavoriteTagSuccessAction {
                            favoriteTag = editFavoriteTagResponse
                        });
                        dispatcher.dispatch(new MainNavigatorPopAction());
                        AnalyticsManager.AnalyticsHandleFavoriteTag(type: FavoriteTagType.edit);
                    })
                    .Catch(error => {
                        CustomDialogUtils.hiddenCustomDialog();
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object deleteFavoriteTag(string tagId) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            CustomDialogUtils.showCustomDialog(
                child: new CustomLoadingDialog(
                    message: "删除收藏夹中"
                )
            );
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return FavoriteApi.DeleteFavoriteTag(tagId: tagId)
                    .Then(deleteFavoriteTagResponse => {
                        CustomDialogUtils.hiddenCustomDialog();
                        dispatcher.dispatch(new DeleteFavoriteTagSuccessAction {
                            favoriteTag = deleteFavoriteTagResponse
                        });
                        AnalyticsManager.AnalyticsHandleFavoriteTag(type: FavoriteTagType.delete);
                    })
                    .Catch(error => {
                        CustomDialogUtils.hiddenCustomDialog();
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object collectFavoriteTag(string itemId, string rankDataId = "", string tagId = "") {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            return new ThunkAction<AppState>((dispatcher, getState) => {
                dispatcher.dispatch(new ChangeFavoriteTagStateAction {isLoading = true});
                return FavoriteApi.CollectFavoriteTag(tagId: itemId)
                    .Then(collectFavoriteTagResponse => {
                        dispatcher.dispatch(new CreateFavoriteTagSuccessAction {
                            favoriteTag = collectFavoriteTagResponse.favoriteTag,
                            isCollection = true
                        });
                        dispatcher.dispatch(new CollectFavoriteTagSuccessAction {
                            myFavoriteTagId = collectFavoriteTagResponse.favoriteTag.id,
                            rankDataId = rankDataId,
                            itemId = itemId,
                            tagId = tagId
                        });


                        AnalyticsManager.AnalyticsHandleFavoriteTag(type: FavoriteTagType.collect);
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new ChangeFavoriteTagStateAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object cancelCollectFavoriteTag(string tagId, string itemId) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            return new ThunkAction<AppState>((dispatcher, getState) => {
                dispatcher.dispatch(new ChangeFavoriteTagStateAction {isLoading = true});
                return FavoriteApi.DeleteFavoriteTag(tagId: tagId, itemId.isNotEmpty() ? "" : tagId)
                    .Then(deleteFavoriteTagResponse => {
                        dispatcher.dispatch(new DeleteFavoriteTagSuccessAction {
                            favoriteTag = deleteFavoriteTagResponse
                        });
                        dispatcher.dispatch(new CancelCollectFavoriteTagSuccessAction
                            {itemId = itemId.isEmpty() ? tagId : itemId});
                        AnalyticsManager.AnalyticsHandleFavoriteTag(type: FavoriteTagType.cancelCollect);
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new ChangeFavoriteTagStateAction());
                        Debuger.LogError(message: error);
                    });
            });
        }
    }
}