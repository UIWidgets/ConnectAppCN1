using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
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
                    searchArticles = state.searchState.searchArticles,
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
            if (this.widget.viewModel.searchArticleLoading && !searchArticles.ContainsKey(key: searchKeyword)) {
                child = new GlobalLoading();
            }
            else if (this.widget.viewModel.searchKeyword.Length > 0) {
                var searchArticleList = searchArticles.ContainsKey(key: searchKeyword)
                    ? searchArticles[key: searchKeyword]
                    : new List<Article>();
                child = searchArticleList.Count > 0
                    ? this._buildContent()
                    : new BlankView(
                        "哎呀，换个关键词试试吧",
                        "image/default-search",
                        true,
                        () => {
                            this.widget.actionModel.startSearchArticle(this.widget.viewModel.searchKeyword);
                            this.widget.actionModel.searchArticle(this.widget.viewModel.searchKeyword, 0);
                        }
                    );
            }

            return new Container(
                color: CColors.White,
                child: child
            );
        }

        Widget _buildContent() {
            var currentPage = this.widget.viewModel.searchArticleCurrentPage;
            var pages = this.widget.viewModel.searchArticlePages;
            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: false,
                        enablePullUp: currentPage != pages.Count - 1,
                        onRefresh: this._onRefresh,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: this.widget.viewModel.searchArticles.Count,
                            itemBuilder: this._buildArticleCard
                        )
                    )
                )
            );
        }
        
        Widget _buildArticleCard(BuildContext context, int index) {
            var searchArticle = this.widget.viewModel.searchArticles[this.widget.viewModel.searchKeyword][index];
            if (this.widget.viewModel.blockArticleList.Contains(searchArticle.id)) {
                return new Container();
            }

            if (searchArticle.ownerType == OwnerType.user.ToString()) {
                var user = this.widget.viewModel.userDict[searchArticle.userId];
                return RelatedArticleCard.User(searchArticle, user,
                    () => this.widget.actionModel.pushToArticleDetail(searchArticle.id));
            }

            var team = this.widget.viewModel.teamDict[searchArticle.teamId];
            return RelatedArticleCard.Team(searchArticle, team,
                () => this.widget.actionModel.pushToArticleDetail(searchArticle.id));
        }
    }
}