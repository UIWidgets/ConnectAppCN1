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
                    searchArticleIds = state.searchState.searchArticleIdDict.ContainsKey(key: state.searchState.keyword)
                        ? state.searchState.searchArticleIdDict[key: state.searchState.keyword]
                        : null,
                    searchArticleCurrentPage = state.searchState.searchArticleCurrentPage,
                    searchArticlePages = state.searchState.searchArticlePages,
                    articleDict = state.articleState.articleDict,
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
                            Actions.searchArticles(keyword: keyword, pageNumber: pageNumber))
                    };
                    return new SearchArticleScreen(viewModel: viewModel, actionModel: actionModel);
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

            this.widget.actionModel.searchArticle(arg1: this.widget.viewModel.searchKeyword, arg2: this._pageNumber)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            var searchArticleIds = this.widget.viewModel.searchArticleIds;
            var searchKeyword = this.widget.viewModel.searchKeyword ?? "";
            Widget child = new Container();
            if (this.widget.viewModel.searchArticleLoading && searchArticleIds == null) {
                child = new GlobalLoading();
            }
            else if (searchKeyword.Length > 0) {
                child = searchArticleIds != null && searchArticleIds.Count > 0
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
            var searchArticleIds = this.widget.viewModel.searchArticleIds;
            var currentPage = this.widget.viewModel.searchArticleCurrentPage;
            var pages = this.widget.viewModel.searchArticlePages;
            var enablePullUp = currentPage != pages.Count - 1;
            return new Container(
                color: CColors.Background,
                child: new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: false,
                    enablePullUp: enablePullUp,
                    onRefresh: this._onRefresh,
                    itemCount: searchArticleIds.Count,
                    itemBuilder: this._buildArticleCard,
                    headerWidget: CustomListViewConstant.defaultHeaderWidget,
                    footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                )
            );
        }
        
        Widget _buildArticleCard(BuildContext context, int index) {
            var searchArticleIds = this.widget.viewModel.searchArticleIds;

            var searchArticleId = searchArticleIds[index: index];
            if (!this.widget.viewModel.articleDict.ContainsKey(key: searchArticleId)) {
                return new Container();
            }

            if (this.widget.viewModel.blockArticleList.Contains(item: searchArticleId)) {
                return new Container();
            }

            var searchArticle = this.widget.viewModel.articleDict[key: searchArticleId];
            var fullName = "";
            if (searchArticle.ownerType == OwnerType.user.ToString()) {
                if (this.widget.viewModel.userDict.ContainsKey(key: searchArticle.userId)) {
                    fullName = this.widget.viewModel.userDict[key: searchArticle.userId].fullName
                        ?? this.widget.viewModel.userDict[key: searchArticle.userId].name;
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
                () => this.widget.actionModel.pushToArticleDetail(obj: searchArticle.id)
            );
        }
    }
}