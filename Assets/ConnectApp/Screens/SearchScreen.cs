using System;
using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

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
                    popularSearchs = state.popularSearchState.popularSearchs,
                    userDict = state.userState.userDict,
                    teamDict = state.teamState.teamDict
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
                        saveSearchHistory = keyword => dispatcher.dispatch(new SaveSearchHistoryAction {keyword = keyword}),
                        deleteSearchHistory = keyword => dispatcher.dispatch(new DeleteSearchHistoryAction {keyword = keyword}),
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
        ) : base(key)
        {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly SearchScreenViewModel viewModel;
        public readonly SearchScreenActionModel actionModel;
        

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
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                widget.actionModel.fetchPopularSearch();
            });
        }

        private void _searchArticle(string text) {
            if (text.isEmpty()) return;
            widget.actionModel.saveSearchHistory(text);
            _controller.text = text;
            widget.actionModel.startSearchArticle();
            widget.actionModel.searchArticle(text, 0);
        }

        private void _onRefresh(bool up) {
            if (up)
                _pageNumber = 0;
            else
                _pageNumber++;
            widget.actionModel.searchArticle(widget.viewModel.searchKeyword, _pageNumber)
                .Then(() => _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => _refreshController.sendBack(up, RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            object child = new Container();
            if (widget.viewModel.searchLoading) {
                child = new GlobalLoading();
            }
            else if (widget.viewModel.searchKeyword.Length > 0) {
                    if (widget.viewModel.searchArticles.Count > 0) {
                        var currentPage = widget.viewModel.currentPage;
                        var pages = widget.viewModel.pages;
                        child = new SmartRefresher(
                            controller: _refreshController,
                            enablePullDown: true,
                            enablePullUp: currentPage != pages.Count - 1,
                            headerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                            footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                            onRefresh: _onRefresh,
                            child: ListView.builder(
                                physics: new AlwaysScrollableScrollPhysics(),
                                itemCount: widget.viewModel.searchArticles.Count,
                                itemBuilder: (cxt, index) => {
                                    var searchArticle = widget.viewModel.searchArticles[index];
                                    if (searchArticle.ownerType==OwnerType.user.ToString())
                                    {
                                        var user = widget.viewModel.userDict[searchArticle.userId];
                                        return RelatedArticleCard.User(searchArticle,user, () =>
                                            {
                                                widget.actionModel.pushToArticleDetail(searchArticle.id);
                                            });
                                    }
                                    else
                                    {
                                        var team = widget.viewModel.teamDict[searchArticle.teamId];
                                        return RelatedArticleCard.Team(searchArticle,team, () =>
                                        {
                                            widget.actionModel.pushToArticleDetail(searchArticle.id);
                                        });
                                    }
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
                        _buildSearchHistory(widget.viewModel.searchHistoryList),
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
                            onPressed: () => {
                                widget.actionModel.mainRouterPop();
                                widget.actionModel.clearSearchArticleResult();
                            },
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
                                if (text == null || text.Length <= 0) widget.actionModel.clearSearchArticleResult();
                            },
                            onSubmitted: _searchArticle
                        )
                    }
                )
            );
        }

        private Widget _buildHotSearch() {
            if (widget.viewModel.popularSearchs.Count <= 0) 
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
                            children: _buildPopularSearchItem(widget.viewModel.popularSearchs)
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
                                                    () => widget.actionModel.deleteAllSearchHistory()),
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
                                    onPressed: () => widget.actionModel.deleteSearchHistory(item),
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