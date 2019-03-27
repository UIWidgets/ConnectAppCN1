using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.components.refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.screens {
    public class SearchScreen : StatefulWidget {
        public SearchScreen(
            Key key = null
        ) : base(key) {
        }

        public override State createState() {
            return new _SearchScreenState();
        }
    }

    internal class _SearchScreenState : State<SearchScreen> {
        private readonly TextEditingController _controller = new TextEditingController(null);
        private int pageNumber;

        public override void initState() {
            base.initState();
            pageNumber = 0;
            StoreProvider.store.Dispatch(new GetSearchHistoryAction());
        }

        public override void dispose() {
            _clearSearchArticle();
            base.dispose();
        }

        private void _searchArticle(string text) {
            if (text.isEmpty()) return;
            _saveSearchHistory(text);
            _controller.text = text;
            StoreProvider.store.Dispatch(new SearchArticleAction {keyword = text});
        }

        private static void _clearSearchArticle() {
            StoreProvider.store.Dispatch(new ClearSearchArticleAction());
        }

        private static void _saveSearchHistory(string text) {
            if (text.isEmpty()) return;
            StoreProvider.store.Dispatch(new SaveSearchHistoryAction {keyword = text});
        }

        private static void _deleteSearchHistory(string text) {
            if (text.isEmpty()) return;
            StoreProvider.store.Dispatch(new DeleteSearchHistoryAction {keyword = text});
        }

        private static void _deleteAllSearchHistory() {
            StoreProvider.store.Dispatch(new DeleteAllSearchHistoryAction());
        }

        private static IPromise _onRefresh(int pageIndex) {
            var keyword = StoreProvider.store.state.searchState.keyword;
            return SearchApi.SearchArticle(keyword, pageIndex)
                .Then(searchResponse => {
                    StoreProvider.store.Dispatch(new SearchArticleSuccessAction {
                        keyword = keyword,
                        pageNumber = pageIndex,
                        searchResponse = searchResponse.projects
                    });
                })
                .Catch(error => { Debug.Log($"{error}"); });
        }

        public override Widget build(BuildContext context)
        {
            return new Container(
                color: CColors.White,
                child: new SafeArea(
                    child: new Container(
                        color: CColors.White,
                        child: new Column(
                            children: new List<Widget>
                            {
                                _buildSearchBar(context),
                                new Flexible(
                                    child: new StoreConnector<AppState, SearchState>(
                                        converter: (state, dispatch) => state.searchState,
                                        builder: (_context, viewModel) =>
                                        {
                                            if (viewModel.loading) return new GlobalLoading();

                                            if (viewModel.keyword.Length > 0)
                                            {
                                                var searchArticles = viewModel.searchArticles;
                                                if (searchArticles.Count > 0)
                                                    return new Refresh(
                                                        onHeaderRefresh: () =>
                                                        {
                                                            pageNumber = 0;
                                                            return _onRefresh(pageNumber);
                                                        },
                                                        onFooterRefresh: () =>
                                                        {
                                                            pageNumber++;
                                                            return _onRefresh(pageNumber);
                                                        },
                                                        headerBuilder: (cxt, controller) =>
                                                            new RefreshHeader(controller),
                                                        footerBuilder: (cxt, controller) =>
                                                            new RefreshFooter(controller),
                                                        child: ListView.builder(
                                                            physics: new AlwaysScrollableScrollPhysics(),
                                                            itemCount: searchArticles.Count,
                                                            itemBuilder: (cxt, index) =>
                                                            {
                                                                var searchArticle = searchArticles[index];
                                                                return new RelatedArticleCard(
                                                                    searchArticle,
                                                                    () =>
                                                                    {
                                                                        StoreProvider.store.Dispatch(
                                                                            new NavigatorToArticleDetailAction
                                                                                {detailId = searchArticle.id});
                                                                        Router.navigator.pushNamed("/article-detail");
                                                                    }
                                                                );
                                                            }
                                                        )
                                                    );
                                                return new BlankView("暂无搜索结果");
                                            }

                                            return new ListView(
                                                children: new List<Widget>
                                                {
                                                    viewModel.searchHistoryList == null
                                                        ? new Container()
                                                        : _buildSearchHistory(viewModel.searchHistoryList),
                                                    _buildHotSearch()
                                                }
                                            );
                                        }
                                    )
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
                            onPressed: () => { Router.navigator.pop(); },
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
                                if (text == null || text.Length <= 0) _clearSearchArticle();
                            },
                            onSubmitted: _searchArticle
                        )
                    }
                )
            );
        }

        private Widget _buildHotSearch() {
            List<string> hotSearch = new List<string> {
                "Unity", "Animation", "AR", "Icon", "Component", "Flutter", "C#"
            };
            List<Widget> widgets = new List<Widget>();
            hotSearch.ForEach(item => {
                Widget widget = new GestureDetector(
                    onTap: () => _searchArticle(item),
                    child: new Container(
                        decoration: new BoxDecoration(
                            CColors.Separator2,
                            borderRadius: BorderRadius.circular(16)
                        ),
                        height: 32,
                        padding: EdgeInsets.symmetric(horizontal: 16, vertical: 4),
                        child: new Text(
                            item,
                            style: CTextStyle.PLargeBody
                        )
                    )
                );
                widgets.Add(widget);
            });
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
                            children: widgets
                        )
                    }
                )
            );
        }

        private Widget _buildSearchHistory(List<string> searchHistoryList) {
            if (searchHistoryList.Count <= 0) return new Container();

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
                                onPressed: _deleteAllSearchHistory,
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
                var widget = new GestureDetector(
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
                                    onPressed: () => _deleteSearchHistory(item),
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
                widgets.Add(widget);
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