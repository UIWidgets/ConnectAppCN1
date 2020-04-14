using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;

namespace ConnectApp.Models.State {
    [Serializable]
    public class AppState {
        public LoginState loginState { get; set; }
        public ServiceConfigState serviceConfigState { get; set; }
        public ArticleState articleState { get; set; }
        public EventState eventState { get; set; }
        public PopularSearchState popularSearchState { get; set; }
        public SearchState searchState { get; set; }
        public NotificationState notificationState { get; set; }
        public UserState userState { get; set; }
        public TeamState teamState { get; set; }
        public PlaceState placeState { get; set; }
        public FollowState followState { get; set; }
        public LikeState likeState { get; set; }
        public MineState mineState { get; set; }
        public MessageState messageState { get; set; }
        public SettingState settingState { get; set; }
        public ReportState reportState { get; set; }
        public FeedbackState feedbackState { get; set; }
        public ChannelState channelState { get; set; }
        public NetworkState networkState { get; set; }
        public TabBarState tabBarState { get; set; }
        public FavoriteState favoriteState { get; set; }
        public LeaderBoardState leaderBoardState { get; set; }
        public GameState gameState { get; set; }

        public static AppState initialState() {
            var loginInfo = UserInfoManager.getUserInfo();
            var isLogin = UserInfoManager.isLogin();

            return new AppState {
                loginState = new LoginState {
                    email = "",
                    password = "",
                    loginInfo = loginInfo,
                    isLoggedIn = isLogin,
                    loading = false,
                    newNotifications = isLogin
                        ? NewNotificationManager.getNewNotification(loginInfo.userId)
                        : null
                },
                serviceConfigState = new ServiceConfigState {
                    showFirstEgg = false,
                    scanEnabled = false,
                    nationalDayEnabled = false,
                },
                articleState = new ArticleState {
                    recommendArticleIds = new List<string>(),
                    followArticleIdDict = new Dictionary<string, List<string>>(),
                    hotArticleIdDict = new Dictionary<string, List<string>>(),
                    articleDict = new Dictionary<string, Article>(),
                    userArticleDict = new Dictionary<string, UserArticle>(),
                    articlesLoading = false,
                    followArticlesLoading = false,
                    articleDetailLoading = false,
                    hottestHasMore = true,
                    feedHasNew = false,
                    feedIsFirst = false,
                    followArticleHasMore = false,
                    hotArticleHasMore = false,
                    hotArticlePage = 0,
                    beforeTime = "",
                    afterTime = "",
                    articleHistory = HistoryManager.articleHistoryList(isLogin ? loginInfo.userId : null),
                    blockArticleList = HistoryManager.blockArticleList(isLogin ? loginInfo.userId : null),
                    homeSliderIds = new List<string>(),
                    homeTopCollectionIds = new List<string>(),
                    homeCollectionIds = new List<string>(),
                    homeBloggerIds = new List<string>(),
                    recommendLastRefreshArticleId = "",
                    recommendHasNewArticle = true
                },
                eventState = new EventState {
                    ongoingEvents = new List<string>(),
                    eventsDict = new Dictionary<string, IEvent>(),
                    ongoingEventTotal = 0,
                    completedEvents = new List<string>(),
                    completedEventTotal = 0,
                    homeEvents = new List<string>(),
                    ongoingPageNumber = 1,
                    completedPageNumber = 1,
                    homeEventPageNumber = 1,
                    homeEventHasMore = false,
                    eventsOngoingLoading = false,
                    eventsCompletedLoading = false,
                    homeEventsLoading = false,
                    eventHistory = HistoryManager.eventHistoryList(isLogin ? loginInfo.userId : null),
                    channelId = ""
                },
                popularSearchState = new PopularSearchState {
                    popularSearchArticles = new List<PopularSearch>(),
                    popularSearchUsers = new List<PopularSearch>()
                },
                searchState = new SearchState {
                    searchArticleLoading = false,
                    searchUserLoading = false,
                    searchTeamLoading = false,
                    searchFollowingLoading = false,
                    keyword = "",
                    searchFollowingKeyword = "",
                    searchArticleIdDict = new Dictionary<string, List<string>>(),
                    searchUserIdDict = new Dictionary<string, List<string>>(),
                    searchTeamIdDict = new Dictionary<string, List<string>>(),
                    searchFollowings = new List<User>(),
                    searchArticleCurrentPage = 0,
                    searchArticlePages = new List<int>(),
                    searchUserHasMore = false,
                    searchTeamHasMore = false,
                    searchFollowingHasMore = false,
                    searchArticleHistoryList =
                        HistoryManager.searchArticleHistoryList(isLogin ? loginInfo.userId : null)
                },
                notificationState = new NotificationState {
                    loading = false,
                    page = 1,
                    pageTotal = 1,
                    notifications = new List<Notification>(),
                    mentions = new List<User>()
                },
                userState = new UserState {
                    userLoading = false,
                    userArticleLoading = false,
                    userLikeArticleLoading = false,
                    followingLoading = false,
                    followingUserLoading = false,
                    followingTeamLoading = false,
                    followerLoading = false,
                    userDict = UserInfoManager.getUserInfoDict(),
                    slugDict = new Dictionary<string, string>(),
                    userLicenseDict = new Dictionary<string, UserLicense>(),
                    fullName = "",
                    title = "",
                    jobRole = new JobRole(),
                    place = "",
                    blockUserIdSet = HistoryManager.blockUserIdSet(isLogin ? loginInfo.userId : null)
                },
                teamState = new TeamState {
                    teamLoading = false,
                    teamArticleLoading = false,
                    followerLoading = false,
                    memberLoading = false,
                    teamDict = new Dictionary<string, Team>(),
                    slugDict = new Dictionary<string, string>()
                },
                placeState = new PlaceState {
                    placeDict = new Dictionary<string, Place>()
                },
                followState = new FollowState {
                    followDict = new Dictionary<string, Dictionary<string, bool>>()
                },
                likeState = new LikeState {
                    likeDict = new Dictionary<string, Dictionary<string, bool>>()
                },
                mineState = new MineState {
                    futureEventIds = new List<string>(),
                    pastEventIds = new List<string>(),
                    futureListLoading = false,
                    pastListLoading = false,
                    futureEventTotal = 0,
                    pastEventTotal = 0
                },
                messageState = new MessageState {
                    channelMessageDict = new Dictionary<string, Dictionary<string, Message>>(),
                    channelMessageList = new Dictionary<string, List<string>>()
                },
                settingState = new SettingState {
                    hasReviewUrl = false,
                    vibrate = true,
                    reviewUrl = ""
                },
                reportState = new ReportState {
                    loading = false
                },
                feedbackState = new FeedbackState {
                    feedbackType = FeedbackType.Advice,
                    loading = false
                },
                channelState = new ChannelState {
                    channelLoading = false,
                    publicChannels = new List<string>(),
                    joinedChannels = new List<string>(),
                    createChannelFilterIds = new List<string>(),
                    discoverPage = 1,
                    discoverHasMore = true,
                    totalUnread = 0,
                    totalMention = 0,
                    channelInfoLoadingDict = new Dictionary<string, bool>(),
                    channelMessageLoadingDict = new Dictionary<string, bool>(),
                    channelDict = new Dictionary<string, ChannelView>(),
                    messageDict = new Dictionary<string, ChannelMessageView>(),
                    localMessageDict = new Dictionary<string, ChannelMessageView>(),
                    channelTop = new Dictionary<string, bool>(),
                    socketConnected = true,
                    mentionSuggestions = new Dictionary<string, List<ChannelMember>>(),
                    lastMentionQuery = null
                },
                tabBarState = new TabBarState {
                    currentTabIndex = 0
                },
                favoriteState = new FavoriteState {
                    favoriteTagLoading = false,
                    followFavoriteTagLoading = false,
                    favoriteDetailLoading = false,
                    favoriteTagIdDict = new Dictionary<string, List<string>>(),
                    followFavoriteTagIdDict = new Dictionary<string, List<string>>(),
                    favoriteDetailArticleIdDict = new Dictionary<string, List<string>>(),
                    favoriteTagHasMore = false,
                    followFavoriteTagHasMore = false,
                    favoriteDetailHasMore = false,
                    favoriteTagDict = new Dictionary<string, FavoriteTag>(),
                    favoriteTagArticleDict = new Dictionary<string, FavoriteTagArticle>(),
                    collectedTagMap = new Dictionary<string, Dictionary<string, bool>>(),
                    collectedTagChangeMap = new Dictionary<string, string>()
                },
                leaderBoardState = new LeaderBoardState {
                    collectionLoading = false,
                    columnLoading = false,
                    bloggerLoading = false,
                    homeBloggerLoading = false,
                    collectionIds = new List<string>(),
                    columnIds = new List<string>(),
                    bloggerIds = new List<string>(),
                    homeBloggerIds = new List<string>(),
                    collectionHasMore = false,
                    columnHasMore = false,
                    bloggerHasMore = false,
                    homeBloggerHasMore = false,
                    collectionPageNumber = 1,
                    columnPageNumber = 1,
                    bloggerPageNumber = 1,
                    homeBloggerPageNumber = 1,
                    rankDict = new Dictionary<string, RankData>(),
                    detailLoading = false,
                    detailHasMore = false,
                    columnDict = new Dictionary<string, List<string>>(),
                    collectionDict = new Dictionary<string, List<string>>(),
                    detailCollectLoading = false
                },
                gameState = new GameState {
                    gameLoading = false,
                    gameDetailLoading = false,
                    gameIds = new List<string>(),
                    gamePage = 1,
                    gameHasMore = false
                },
                networkState = new NetworkState {
                    networkConnected = true,
                    dismissNoNetworkBanner = true
                }
            };
        }
    }
}