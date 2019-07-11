using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class SearchArticleScreenConnector : StatelessWidget {
        public SearchArticleScreenConnector(
            Key key = null
        ) : base(key: key) {
        }
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, SearchScreenViewModel>(
                converter: state => new SearchScreenViewModel {
                    searchArticleLoading = state.searchState.searchArticleLoading,
                    searchKeyword = state.searchState.keyword,
                    searchArticles = state.searchState.searchArticles.ContainsKey(key: state.searchState.keyword)
                        ? state.searchState.searchArticles[key: state.searchState.keyword]
                        : null,
                    searchArticleCurrentPage = state.searchState.searchArticleCurrentPage,
                    searchArticlePages = state.searchState.searchArticlePages,
                    userDict = state.userState.userDict,
                    teamDict = state.teamState.teamDict,
                    blockArticleList = state.articleState.blockArticleList
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new SearchScreenActionModel {
                        pushToArticleDetail = articleId => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {articleId = articleId}),
                        startSearchArticle = keyword => dispatcher.dispatch(new StartSearchArticleAction {
                            keyword = keyword
                        }),
                        searchArticle = (keyword, pageNumber) => dispatcher.dispatch<IPromise>(
                            Actions.searchArticles(keyword, pageNumber))
                    };
                    return new SearchArticleScreen(viewModel, actionModel);
                }
            );
        }
    }
    
    public class SearchArticleScreen : StatefulWidget {
        public SearchArticleScreen(
            SearchScreenViewModel viewModel = null,
            SearchScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly SearchScreenViewModel viewModel;
        public readonly SearchScreenActionModel actionModel;

        public override State createState() {
            return new _SearchArticleScreenState();
        }
    }

    class _SearchArticleScreenState : State<SearchArticleScreen> {
        int _pageNumber;
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            this._pageNumber = 0;
            this._refreshController = new RefreshController();
        }

        void _onRefresh(bool up) {
            if (up) {
                this._pageNumber = 0;
            }
            else {
                this._pageNumber++;
            }

            this.widget.actionModel.searchArticle(this.widget.viewModel.searchKeyword, this._pageNumber)
                .Then(() => this._refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up, RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            var searchArticles = this.widget.viewModel.searchArticles;
            var searchKeyword = this.widget.viewModel.searchKeyword ?? "";
            Widget child = new Container();
            if (this.widget.viewModel.searchArticleLoading && searchArticles == null) {
                child = new GlobalLoading();
            }
            else if (searchKeyword.Length > 0) {
                child = searchArticles != null && searchArticles.Count > 0
                    ? this._buildContent()
                    : new BlankView(
                        "哎呀，换个关键词试试吧",
                        "image/default-search"
                    );
            }

            return new Container(
                color: CColors.White,
                child: child
            );
        }

        Widget _buildContent() {
            var searchArticles = this.widget.viewModel.searchArticles;
            var currentPage = this.widget.viewModel.searchArticleCurrentPage;
            var pages = this.widget.viewModel.searchArticlePages;
            var hasMore = currentPage != pages.Count - 1;
            var itemCount = hasMore ? searchArticles.Count : searchArticles.Count + 1;
            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: false,
                        enablePullUp: hasMore,
                        onRefresh: this._onRefresh,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: itemCount,
                            itemBuilder: this._buildArticleCard
                        )
                    )
                )
            );
        }
        
        Widget _buildArticleCard(BuildContext context, int index) {
            var searchArticles = this.widget.viewModel.searchArticles;
            if (index == searchArticles.Count) {
                return new EndView();
            }
            var searchArticle = searchArticles[index: index];
            if (this.widget.viewModel.blockArticleList.Contains(searchArticle.id)) {
                return new Container();
            }

            var fullName = "";
            if (searchArticle.ownerType == OwnerType.user.ToString()) {
                if (this.widget.viewModel.userDict.ContainsKey(key: searchArticle.userId)) {
                    fullName = this.widget.viewModel.userDict[key: searchArticle.userId].fullName;
                }
            }

            if (searchArticle.ownerType == OwnerType.team.ToString()) {
                if (this.widget.viewModel.teamDict.ContainsKey(key: searchArticle.teamId)) {
                    fullName = this.widget.viewModel.teamDict[key: searchArticle.teamId].name;
                }
            }
            return new RelatedArticleCard(
                article: searchArticle,
                fullName: fullName,
                () => this.widget.actionModel.pushToArticleDetail(obj: searchArticle.id),
                index == 0
            );
        }
    }
}