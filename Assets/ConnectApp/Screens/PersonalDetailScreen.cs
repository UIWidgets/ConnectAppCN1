using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class PersonalDetailScreenConnector : StatelessWidget {
        public PersonalDetailScreenConnector(
            string personalId,
            Key key = null
        ) : base(key: key) {
            this.personalId = personalId;
        }

        readonly string personalId;
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, PersonalDetailScreenViewModel>(
                converter: state => {
                    var personal = state.personalState.personalDict.ContainsKey(key: this.personalId)
                        ? state.personalState.personalDict[key: this.personalId] : null;
                    var articleOffset = personal == null ? 0 : personal.articles.Count;
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followDict = state.followState.followDict;
                    var followMap = followDict.ContainsKey(key: currentUserId)
                        ? followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new PersonalDetailScreenViewModel {
                        personalId = this.personalId,
                        personalLoading = state.personalState.personalLoading,
                        personalArticleLoading = state.personalState.personalArticleLoading,
                        followUserLoading = state.personalState.followUserLoading,
                        personal = personal,
                        followMap = followMap,
                        articleOffset = articleOffset,
                        currentUserId = currentUserId,
                        isLoggedIn = state.loginState.isLoggedIn
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new PersonalDetailScreenActionModel {
                        startFetchPersonal = () => dispatcher.dispatch(new StartFetchPersonalAction()),
                        fetchPersonal = () => dispatcher.dispatch<IPromise>(Actions.fetchPersonal(this.personalId)),
                        fetchPersonalArticle = offset => dispatcher.dispatch<IPromise>(Actions.fetchPersonalArticle(this.personalId, offset)),
                        startFollowUser = () => dispatcher.dispatch(new StartFetchFollowUserAction()),
                        followUser = () => dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(this.personalId)),
                        startUnFollowUser = () => dispatcher.dispatch(new StartFetchUnFollowUserAction()),
                        unFollowUser = () => dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(this.personalId)),
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToArticleDetail = id => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {
                                articleId = id
                            }
                        ),
                        pushToReport = (reportId, reportType) => dispatcher.dispatch(
                            new MainNavigatorPushToReportAction {
                                reportId = reportId,
                                reportType = reportType
                            }
                        ),
                        pushToBlock = articleId => {
                            dispatcher.dispatch(new BlockArticleAction {articleId = articleId});
                            dispatcher.dispatch(new DeleteArticleHistoryAction {articleId = articleId});
                        },
                        pushToFollowingUser = userId => dispatcher.dispatch(
                            new MainNavigatorPushToFollowingUserAction {
                                userId = userId
                            }
                        ),
                        pushToFollowerUser = userId => dispatcher.dispatch(
                            new MainNavigatorPushToFollowerUserAction {
                                userId = userId
                            }
                        ),
                        pushToEditPersonalInfo = userId => dispatcher.dispatch(
                            new MainNavigatorPushToEditPersonalInfoAction {
                                userId = userId
                            }
                        )
                    };
                    return new PersonalDetailScreen(viewModel, actionModel);
                }
            );
        }
    }
    
    public class PersonalDetailScreen : StatefulWidget {
        public PersonalDetailScreen(
            PersonalDetailScreenViewModel viewModel = null,
            PersonalDetailScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly PersonalDetailScreenViewModel viewModel;
        public readonly PersonalDetailScreenActionModel actionModel;
        
        public override State createState() {
            return new _PersonalDetailScreenState();
        }
    }

    class _PersonalDetailScreenState : State<PersonalDetailScreen> {
        const float headerHeight = 256;
        const float _transformSpeed = 0.005f;
        int _articleOffset;
        RefreshController _refreshController;
        float _factor = 1;
        public override void initState() {
            base.initState();
            this._articleOffset = 0;
            this._refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchPersonal();
                this.widget.actionModel.fetchPersonal();
            });
        }

        void _scrollListener() {
            var scrollController = this._refreshController.scrollController;
            if (scrollController.offset < 0) {
                this._factor = 1 + scrollController.offset.abs() * _transformSpeed;
                this.setState(() => { });
            } else {
                if (this._factor != 1) {
                    this.setState(() => this._factor = 1);
                }
            }
        }

        void _onRefresh(bool up) {
            this._articleOffset = up ? 0 : this.widget.viewModel.articleOffset;
            this.widget.actionModel.fetchPersonalArticle(this._articleOffset)
                .Then(() => this._refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up, RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            Widget content = new Container();
            if (this.widget.viewModel.personalLoading && this.widget.viewModel.personal == null) {
                content = new GlobalLoading();
            } else if (this.widget.viewModel.personal == null) {
                content = new Container();
            }
            else {
                content = this._buildPersonalContent(context: context);
            }
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Stack(
                        children: new List<Widget> {
                            content,
                            this._buildNavigationBar()
                        }
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new Positioned(
                left: 0,
                top: 0,
                right: 0,
                height: 44,
                child: new Container(
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: new List<Widget> {
                            new GestureDetector(
                                onTap: () => this.widget.actionModel.mainRouterPop(),
                                child: new Container(
                                    padding: EdgeInsets.only(16, 10, 0, 10),
                                    color: CColors.Transparent,
                                    child: new Icon(Icons.arrow_back, size: 24, color: CColors.White)
                                )
                            ),
                            new GestureDetector(
                                child: new Container(
                                    padding: EdgeInsets.only(16, 10, 16, 10),
                                    color: CColors.Transparent,
                                    child: new Icon(Icons.more_horiz, size: 24, color: CColors.White)
                                )
                            )
                        }
                    )
                )
            );
        }
        
        Widget _buildPersonalContent(BuildContext context) {
            var articles = this.widget.viewModel.personal.articles ?? new List<Article>();
            var articlesHasMore = this.widget.viewModel.personal.articlesHasMore;
            return new Container(
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: false,
                        enablePullUp: articlesHasMore,
                        onRefresh: this._onRefresh,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: 2 + (articles.Count == 0 ? 1 : articles.Count),
                            itemBuilder: (cxt, index) => {
                                if (index == 0) {
                                    return Transform.scale(
                                        scale: this._factor,
                                        child: this._buildPersonalInfo()
                                    );
                                }

                                if (index == 1) {
                                    return _buildPersonalArticleTitle();
                                }

                                if (articles.Count == 0 && index == 2) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight - 44;
                                    return new Container(
                                        height: height,
                                        child: new BlankView(
                                            "哎呀，暂无已发布的文章",
                                            "image/default-article"
                                        )
                                    );
                                }

                                var article = articles[index - 2];
                                return new ArticleCard(
                                    article,
                                    () => this.widget.actionModel.pushToArticleDetail(article.id),
                                    () => ReportManager.showReportView(
                                        this.widget.viewModel.isLoggedIn,
                                        article.id,
                                        ReportType.article,
                                        this.widget.actionModel.pushToLogin,
                                        this.widget.actionModel.pushToReport,
                                        this.widget.actionModel.pushToBlock
                                    ),
                                    this.widget.viewModel.personal.user.fullName,
                                    key: new ObjectKey(article.id)
                                );
                            }
                        )
                    )
                )
            );
        }

        Widget _buildPersonalInfo() {
            var personal = this.widget.viewModel.personal;
            var user = personal.user ?? new User();
            Widget rightWidget;
            if (this.widget.viewModel.isLoggedIn
                && this.widget.viewModel.currentUserId == this.widget.viewModel.personalId) {
                rightWidget = new CustomButton(
                    padding: EdgeInsets.zero,
                    child: new Container(
                        width: 100,
                        height: 32,
                        alignment: Alignment.center,
                        decoration: new BoxDecoration(
                            color: CColors.Transparent,
                            borderRadius: BorderRadius.all(4),
                            border: Border.all(
                                color: CColors.White
                            )
                        ),
                        child: new Text("编辑资料", style: CTextStyle.PMediumWhite)
                    ),
                    onPressed: () => {
                        if (this.widget.viewModel.isLoggedIn) {
                            this.widget.actionModel.pushToEditPersonalInfo(this.widget.viewModel.personalId);
                        }
                        else {
                            this.widget.actionModel.pushToLogin();
                        }
                    }
                );
            }
            else {
                bool isFollow = false;
                string followText = "关注";
                Color followBgColor = CColors.PrimaryBlue;
                GestureTapCallback onTap = () => {
                    this.widget.actionModel.startFollowUser();
                    this.widget.actionModel.followUser();
                };
                if (this.widget.viewModel.isLoggedIn
                    && this.widget.viewModel.followMap.ContainsKey(this.widget.viewModel.personalId)) {
                    isFollow = true;
                    followText = "已关注";
                    followBgColor = CColors.Transparent;
                    onTap = () => {
                        this.widget.actionModel.startUnFollowUser();
                        this.widget.actionModel.unFollowUser();
                    };
                }

                Widget rightChild = new Container();
                bool isEnable;
                if (this.widget.viewModel.followUserLoading) {
                    rightChild = new CustomActivityIndicator(
                        loadingColor: LoadingColor.white,
                        size: LoadingSize.small
                    );
                    isEnable = false;
                }
                else {
                    rightChild = new Text(data: followText, style: CTextStyle.PMediumWhite);
                    isEnable = true;
                }
                
                rightWidget = new CustomButton(
                    padding: EdgeInsets.zero,
                    child: new Container(
                        width: 100,
                        height: 32,
                        alignment: Alignment.center,
                        decoration: new BoxDecoration(
                            color: followBgColor,
                            borderRadius: BorderRadius.all(4),
                            border: isFollow ? Border.all(CColors.White) : null
                        ),
                        child: rightChild
                    ),
                    onPressed: () => {
                        if (!isEnable) {
                            return;
                        }
                        if (this.widget.viewModel.isLoggedIn) {
                            onTap();
                        }
                        else {
                            this.widget.actionModel.pushToLogin();
                        }
                    }
                );
            }
            Widget titleWidget = new Container();
            if (user.title != null && user.title.isNotEmpty()) {
                titleWidget = new Text(
                    user.title,
                    style: new TextStyle(
                        height: 1.46f,
                        fontSize: 14,
                        fontFamily: "Roboto-Regular",
                        color: CColors.BgGrey
                    ),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis
                );
            }
            
            Widget bgWidget = new Container(
                color: CColors.Red
            );
            if (user.coverImage.isNotEmpty()) {
                bgWidget = new PlaceholderImage(
                    user.coverImage,
                    height: headerHeight,
                    fit: BoxFit.cover
                );
            }

            return new Container(
                height: headerHeight,
                child: new Stack(
                    children: new List<Widget> {
                        bgWidget,
                        Positioned.fill(
                            new Container(
                                color: Color.fromRGBO(0, 0, 0, 0.08f),
                                padding: EdgeInsets.only(16, 0, 16, 24),
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.end,
                                    children: new List<Widget> {
                                        new Row(
                                            children: new List<Widget> {
                                                new Container(
                                                    margin: EdgeInsets.only(right: 16),
                                                    child: Avatar.User(
                                                        this.widget.viewModel.personalId,
                                                        user,
                                                        80
                                                    )
                                                ),
                                                new Expanded(
                                                    child: new Column(
                                                        crossAxisAlignment: CrossAxisAlignment.start,
                                                        children: new List<Widget> {
                                                            new Text(
                                                                user.fullName,
                                                                style: CTextStyle.H4White,
                                                                maxLines: 1,
                                                                overflow: TextOverflow.ellipsis
                                                            ),
                                                            titleWidget
                                                        }
                                                    )
                                                )
                                            }
                                        ),
                                        new Container(
                                            margin: EdgeInsets.only(top: 16),
                                            child: new Row(
                                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                                children: new List<Widget> {
                                                    new Container(
                                                        child: new Row(
                                                            children: new List<Widget> {
                                                                _buildPersonalButton(
                                                                    "关注",
                                                                    $"{personal.followingCount}",
                                                                    () =>
                                                                        this.widget.actionModel.pushToFollowingUser(
                                                                            this.widget.viewModel.personalId)
                                                                ),
                                                                new SizedBox(width: 16),
                                                                _buildPersonalButton(
                                                                    "粉丝",
                                                                    $"{user.followCount}",
                                                                    () =>
                                                                        this.widget.actionModel.pushToFollowerUser(
                                                                            this.widget.viewModel.personalId)
                                                                )
                                                            }
                                                        )
                                                    ),
                                                    rightWidget
                                                }
                                            )
                                        )
                                    }
                                )
                            )
                        )
                    }
                )
            );
        }

        static Widget _buildPersonalArticleTitle() {
            return new Container(
                padding: EdgeInsets.only(16),
                height: 44,
                decoration: new BoxDecoration(
                    border: new Border(
                        bottom: new BorderSide(
                            CColors.Separator2
                        )
                    )
                ),
                alignment: Alignment.centerLeft,
                child: new Text("文章", style: CTextStyle.PLargeTitle)
            );
        }

        static Widget _buildPersonalButton(string title, string subTitle, GestureTapCallback onTap) {
            return new GestureDetector(
                onTap: onTap,
                child: new Container(
                    height: 32,
                    alignment: Alignment.center,
                    color: CColors.Transparent,
                    child: new Row(
                        children: new List<Widget> {
                            new Text(data: title, style: CTextStyle.PRegularWhite),
                            new SizedBox(width: 2),
                            new Text(
                                data: subTitle,
                                style: new TextStyle(
                                    height: 1.27f,
                                    fontSize: 20,
                                    fontFamily: "Roboto-Bold",
                                    color: CColors.White
                                )
                            )
                        }
                    )
                )
            );
        }
    }
}