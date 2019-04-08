using System;
using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.screens {
    
    public class SearchScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, SearchScreenModel>(
                pure: true,
                converter: state => new SearchScreenModel {
                    searchLoading = state.searchState.loading,
                    searchKeyword = state.searchState.keyword,
                    searchArticles = state.searchState.searchArticles,
                    currentPage = state.searchState.currentPage,
                    pages = state.searchState.pages,
                    searchHistoryList = state.searchState.searchHistoryList,
                    popularSearchs = state.popularSearchState.popularSearchs
                },
                builder: (context1, viewModel, dispatcher) => {
                    return new SearchScreen(
                        viewModel,
                        () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        articleId => dispatcher.dispatch(
                                new MainNavigatorPushToArticleDetailAction {articleId = articleId}),
                        (keyword, pageNumber) => dispatcher.dispatch<IPromise>(
                            Actions.searchArticles(keyword, pageNumber)),
                        () => dispatcher.dispatch(new PopularSearchAction()),
                        () => dispatcher.dispatch(new ClearSearchArticleResultAction()),
                        keyword => dispatcher.dispatch(new SaveSearchHistoryAction {keyword = keyword}),
                        keyword => dispatcher.dispatch(new DeleteSearchHistoryAction {keyword = keyword}),
                        () => dispatcher.dispatch(new DeleteAllSearchHistoryAction())
                    );
                }
            );
        }
    }
    
    public class SearchScreen : StatefulWidget {
        public SearchScreen(
            SearchScreenModel screenModel = null,
            Action mainRouterPop = null,
            Action<string> pushToArticleDetail = null,
            Func<string, int, IPromise> searchArticle = null,
            Action fetchPopularSearch = null,
            Action clearSearchArticleResult = null,
            Action<string> saveSearchHistory = null,
            Action<string> deleteSearchHistory = null,
            Action deleteAllSearchHistory = null,
            Key key = null
        ) : base(key)
        {
            this.screenModel = screenModel;
            this.mainRouterPop = mainRouterPop;
            this.pushToArticleDetail = pushToArticleDetail;
            this.searchArticle = searchArticle;
            this.fetchPopularSearch = fetchPopularSearch;
            this.clearSearchArticleResult = clearSearchArticleResult;
            this.saveSearchHistory = saveSearchHistory;
            this.deleteSearchHistory = deleteSearchHistory;
            this.deleteAllSearchHistory = deleteAllSearchHistory;
        }

        public SearchScreenModel screenModel;
        public Action mainRouterPop;
        public Action<string> pushToArticleDetail;
        public Func<string, int, IPromise> searchArticle;
        public Action fetchPopularSearch;
        public Action clearSearchArticleResult;
        public Action<string> saveSearchHistory;
        public Action<string> deleteSearchHistory;
        public Action deleteAllSearchHistory;

        public override State createState() {
            return new _SearchScreenState();
        }
    }

    internal class _SearchScreenState : State<SearchScreen> {
        private readonly TextEditingController _controller = new TextEditingController("");
        private int _pageNumber;
        private RefreshController _refreshController;

        public override void initState() {
            base.initState();
            _pageNumber = 0;
            _refreshController = new RefreshController();
            widget.fetchPopularSearch();
        }

        public override void dispose() {
            widget.clearSearchArticleResult();
            base.dispose();
        }

        private void _searchArticle(string text) {
            if (text.isEmpty()) return;
            widget.saveSearchHistory(text);
            _controller.text = text;
            widget.searchArticle(text, 0);
        }

        private void _onRefresh(bool up) {
            if (up)
                _pageNumber = 0;
            else
                _pageNumber++;
            widget.searchArticle(widget.screenModel.searchKeyword, _pageNumber)
                .Then(() => _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => _refreshController.sendBack(up, RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            object child = new Container();
            if (widget.screenModel.searchLoading) {
                child = new GlobalLoading();
            }
            else if (widget.screenModel.searchKeyword.Length > 0) {
                    if (widget.screenModel.searchArticles.Count > 0) {
                        var currentPage = widget.screenModel.currentPage;
                        var pages = widget.screenModel.pages;
                        child = new SmartRefresher(
                            controller: _refreshController,
                            enablePullDown: true,
                            enablePullUp: currentPage != pages.Count - 1,
                            headerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                            footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                            onRefresh: _onRefresh,
                            child: ListView.builder(
                                physics: new AlwaysScrollableScrollPhysics(),
                                itemCount: widget.screenModel.searchArticles.Count,
                                itemBuilder: (cxt, index) => {
                                    var searchArticle = widget.screenModel.searchArticles[index];
                                    return new RelatedArticleCard(
                                        searchArticle,
                                        () => widget.pushToArticleDetail(searchArticle.id)
                                    );
                                }
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
                        _buildSearchHistory(widget.screenModel.searchHistoryList),
                        _buildHotSearch()
                    }
                ); 
            }
            
            
            
            return new Container(
                color: CColors.White,
                child: new SafeArea(
                    child: new Container(
                        child: new Column(
                            children: new List<Widget> {
                                _buildSearchBar(context),
                                new Flexible(
                                    child: (Widget)child
                                )
                            }
                        )
                    )
                )
            );
        }

        private Widget _buildSearchBar(BuildContext context) {
            return new Container(
                height: 140,
                padding: EdgeInsets.only(16, 0, 16, 12),
                color: CColors.White,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.end,
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.only(8, 8, 0, 8),
                            onPressed: () => widget.mainRouterPop(),
                            child: new Text(
                                "取消",
                                style: CTextStyle.PLargeBlue
                            )
                        ),
                        new InputField(
                            controller: _controller,
                            style: CTextStyle.H2,
                            autofocus: true,
                            hintText: "搜索",
                            hintStyle: CTextStyle.H2Body4,
                            cursorColor: CColors.PrimaryBlue,
                            textInputAction: TextInputAction.search,
                            clearButtonMode: InputFieldClearButtonMode.whileEditing,
                            onChanged: text => {
                                if (text == null || text.Length <= 0) widget.clearSearchArticleResult();
                            },
                            onSubmitted: _searchArticle
                        )
                    }
                )
            );
        }

        private Widget _buildHotSearch() {
            if (widget.screenModel.popularSearchs.Count <= 0) 
                return new Container();
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
                            children: _buildPopularSearchItem(widget.screenModel.popularSearchs)
                        )
                    }
                )
            );
        }

        private List<Widget> _buildPopularSearchItem(List<PopularSearch> popularSearch) {
            List<Widget> widgets = new List<Widget>();
            popularSearch.ForEach(item => {
                Widget widget = new GestureDetector(
                    onTap: () => _searchArticle(item.keyword),
                    child: new Container(
                        decoration: new BoxDecoration(
                            CColors.Separator2,
                            borderRadius: BorderRadius.circular(16)
                        ),
                        height: 32,
                        padding: EdgeInsets.symmetric(horizontal: 16, vertical: 4),
                        child: new Text(
                            item.keyword,
                            style: CTextStyle.PLargeBody
                        )
                    )
                );
                widgets.Add(widget);
            });
            return widgets;
        }

        private Widget _buildSearchHistory(List<string> searchHistoryList) {
            if (searchHistoryList == null || searchHistoryList.Count <= 0) return new Container();

            var widgets = new List<Widget>();
            widgets.Add(
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
                                                    () => widget.deleteAllSearchHistory()),
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
            );
            searchHistoryList.ForEach(item => {
                var child = new GestureDetector(
                    onTap: () => { _searchArticle(item); },
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
                                    onPressed: () => widget.deleteSearchHistory(item),
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