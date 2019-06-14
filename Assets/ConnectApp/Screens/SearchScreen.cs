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
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class SearchScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, SearchScreenViewModel>(
                converter: state => new SearchScreenViewModel {
                    searchLoading = state.searchState.loading,
                    searchKeyword = state.searchState.keyword,
                    searchArticles = state.searchState.searchArticles,
                    currentPage = state.searchState.currentPage,
                    pages = state.searchState.pages,
                    searchHistoryList = state.searchState.searchHistoryList,
                    popularSearchList = state.popularSearchState.popularSearchs,
                    userDict = state.userState.userDict,
                    teamDict = state.teamState.teamDict,
                    blockArticleList = state.articleState.blockArticleList
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new SearchScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToArticleDetail = articleId => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {articleId = articleId}),
                        startSearchArticle = () => dispatcher.dispatch(new StartSearchArticleAction()),
                        searchArticle = (keyword, pageNumber) => dispatcher.dispatch<IPromise>(
                            Actions.searchArticles(keyword, pageNumber)),
                        fetchPopularSearch = () => dispatcher.dispatch<IPromise>(Actions.popularSearch()),
                        clearSearchArticleResult = () => dispatcher.dispatch(new ClearSearchArticleResultAction()),
                        saveSearchHistory = keyword =>
                            dispatcher.dispatch(new SaveSearchHistoryAction {keyword = keyword}),
                        deleteSearchHistory = keyword =>
                            dispatcher.dispatch(new DeleteSearchHistoryAction {keyword = keyword}),
                        deleteAllSearchHistory = () => dispatcher.dispatch(new DeleteAllSearchHistoryAction())
                    };
                    return new SearchScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class SearchScreen : StatefulWidget {
        public SearchScreen(
            SearchScreenViewModel viewModel = null,
            SearchScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly SearchScreenViewModel viewModel;
        public readonly SearchScreenActionModel actionModel;


        public override State createState() {
            return new _SearchScreenState();
        }
    }

    class _SearchScreenState : State<SearchScreen> {
        readonly TextEditingController _controller = new TextEditingController("");
        int _pageNumber;
        RefreshController _refreshController;
        FocusNode _focusNode;

        public override void initState() {
            base.initState();
            this._pageNumber = 0;
            this._refreshController = new RefreshController();
            this._focusNode = new FocusNode();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (this.widget.viewModel.searchKeyword.Length > 0 || this.widget.viewModel.searchArticles.Count > 0) {
                    this.widget.actionModel.clearSearchArticleResult();
                }

                this.widget.actionModel.fetchPopularSearch();
            });
        }

        public override void dispose() {
            this._controller.dispose();
            base.dispose();
        }

        void _searchArticle(string text) {
            if (text.isEmpty()) {
                return;
            }

            if (this._focusNode.hasFocus) {
                this._focusNode.unfocus();
            }

            this.widget.actionModel.saveSearchHistory(text);
            this._controller.text = text;
            this.widget.actionModel.startSearchArticle();
            this.widget.actionModel.searchArticle(text, 0);
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
            Widget child = new Container();
            if (this.widget.viewModel.searchLoading) {
                child = new GlobalLoading();
            }
            else if (this.widget.viewModel.searchKeyword.Length > 0) {
                if (this.widget.viewModel.searchArticles.Count > 0) {
                    var currentPage = this.widget.viewModel.currentPage;
                    var pages = this.widget.viewModel.pages;
                    child = new Container(
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
                                    itemBuilder: (cxt, index) => {
                                        var searchArticle = this.widget.viewModel.searchArticles[index];
                                        if (this.widget.viewModel.blockArticleList.Contains(searchArticle.id)) {
                                            return new Container();
                                        }

                                        if (searchArticle.ownerType == OwnerType.user.ToString()) {
                                            var user = this.widget.viewModel.userDict[searchArticle.userId];
                                            return RelatedArticleCard.User(searchArticle, user,
                                                () => {
                                                    this.widget.actionModel.pushToArticleDetail(searchArticle.id);
                                                });
                                        }

                                        var team = this.widget.viewModel.teamDict[searchArticle.teamId];
                                        return RelatedArticleCard.Team(searchArticle, team,
                                            () => { this.widget.actionModel.pushToArticleDetail(searchArticle.id); });
                                    }
                                )
                            )
                        )
                    );
                }
                else {
                    child = new BlankView("暂无搜索结果");
                }
            }
            else {
                child = new ListView(
                    children: new List<Widget> {
                        this._buildSearchHistory(this.widget.viewModel.searchHistoryList), this._buildHotSearch()
                    }
                );
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new GestureDetector(
                        onTap: () => this._focusNode.unfocus(),
                        child: new Container(
                            child: new Column(
                                children: new List<Widget> {
                                    this._buildSearchBar(),
                                    new Flexible(
                                        child: new NotificationListener<ScrollNotification>(
                                            onNotification: notification => {
                                                this._focusNode.unfocus();
                                                return true;
                                            },
                                            child: child
                                        )
                                    )
                                }
                            )
                        )
                    )
                )
            );
        }

        Widget _buildSearchBar() {
            return new Container(
                height: 94,
                padding: EdgeInsets.only(16, 0, 16, 12),
                color: CColors.White,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.end,
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.only(8, 8, 0, 8),
                            onPressed: () => { this.widget.actionModel.mainRouterPop(); },
                            child: new Text(
                                "取消",
                                style: CTextStyle.PLargeBlue
                            )
                        ),
                        new InputField(
                            height: 40,
                            controller: this._controller,
                            focusNode: this._focusNode,
                            style: CTextStyle.H2,
                            autofocus: true,
                            hintText: "搜索",
                            hintStyle: CTextStyle.H2Body4,
                            cursorColor: CColors.PrimaryBlue,
                            textInputAction: TextInputAction.search,
                            clearButtonMode: InputFieldClearButtonMode.whileEditing,
                            onChanged: text => {
                                if (text == null || text.Length <= 0) {
                                    this.widget.actionModel.clearSearchArticleResult();
                                }
                            },
                            onSubmitted: this._searchArticle
                        )
                    }
                )
            );
        }

        Widget _buildHotSearch() {
            if (this.widget.viewModel.popularSearchList.Count <= 0) {
                return new Container();
            }

            return new Container(
                padding: EdgeInsets.only(16, 24, 16),
                color: CColors.White,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(bottom: 16),
                            child: new Text(
                                "热门搜索",
                                style: CTextStyle.PXLargeBody4
                            )
                        ),
                        new Wrap(
                            spacing: 8,
                            runSpacing: 20,
                            children: this._buildPopularSearchItem(this.widget.viewModel.popularSearchList)
                        )
                    }
                )
            );
        }

        List<Widget> _buildPopularSearchItem(List<PopularSearch> popularSearch) {
            List<Widget> widgets = new List<Widget>();
            popularSearch.ForEach(item => {
                Widget widget = new GestureDetector(
                    onTap: () => this._searchArticle(item.keyword),
                    child: new Container(
                        decoration: new BoxDecoration(
                            CColors.Separator2,
                            borderRadius: BorderRadius.circular(16)
                        ),
                        height: 32,
                        padding: EdgeInsets.only(16, 7, 16),
                        child: new Text(
                            item.keyword,
                            maxLines: 1,
                            style: new TextStyle(
                                fontSize: 16,
                                fontFamily: "Roboto-Regular",
                                color: CColors.TextBody
                            ),
                            overflow: TextOverflow.ellipsis
                        )
                    )
                );
                widgets.Add(widget);
            });
            return widgets;
        }

        Widget _buildSearchHistory(List<string> searchHistoryList) {
            if (searchHistoryList == null || searchHistoryList.Count <= 0) {
                return new Container();
            }

            var widgets = new List<Widget> {
                new Container(
                    margin: EdgeInsets.only(top: 24, bottom: 10),
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: new List<Widget> {
                            new Text(
                                "搜索历史",
                                style: CTextStyle.PXLargeBody4
                            ),
                            new CustomButton(
                                padding: EdgeInsets.only(8, 8, 0, 8),
                                onPressed: () => {
                                    ActionSheetUtils.showModalActionSheet(
                                        new ActionSheet(
                                            title: "确定清除搜索历史记录？",
                                            items: new List<ActionSheetItem> {
                                                new ActionSheetItem("确定", ActionType.destructive,
                                                    () => this.widget.actionModel.deleteAllSearchHistory()),
                                                new ActionSheetItem("取消", ActionType.cancel)
                                            }
                                        )
                                    );
                                },
                                child: new Text(
                                    "清空",
                                    style: CTextStyle.PRegularBody4
                                )
                            )
                        }
                    )
                )
            };
            searchHistoryList.ForEach(item => {
                var child = new GestureDetector(
                    onTap: () => { this._searchArticle(item); },
                    child: new Container(
                        height: 44,
                        color: CColors.White,
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget> {
                                new Text(
                                    item,
                                    style: CTextStyle.PLargeBody
                                ),
                                new CustomButton(
                                    padding: EdgeInsets.only(8, 8, 0, 8),
                                    onPressed: () => this.widget.actionModel.deleteSearchHistory(item),
                                    child: new Icon(
                                        Icons.close,
                                        size: 16,
                                        color: Color.fromRGBO(199, 203, 207, 1)
                                    )
                                )
                            }
                        )
                    )
                );
                widgets.Add(child);
            });

            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                color: CColors.White,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: widgets
                )
            );
        }
    }
}