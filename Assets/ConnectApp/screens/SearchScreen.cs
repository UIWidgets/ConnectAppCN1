using ConnectApp.api;
using ConnectApp.constants;
using ConnectApp.components;
using ConnectApp.components.refresh;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using Unity.UIWidgets.service;
using System.Collections.Generic;
using Unity.UIWidgets.painting;
using TextStyle = Unity.UIWidgets.painting.TextStyle;
using Unity.UIWidgets.rendering;
using UnityEngine;
using RSG;

namespace ConnectApp.screens {
    public class SearchScreen : StatefulWidget {

        public SearchScreen(
            Key key = null
        ) : base(key) {
            
        }
        public override State createState() => new _SearchScreenState();
    }

    internal class _SearchScreenState : State<SearchScreen> {
        
        private readonly TextEditingController _controller = new TextEditingController(null);
        private int pageNumber = 0;
        
        public override void initState() {
            base.initState();
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
            StoreProvider.store.Dispatch(new SearchArticleAction{keyword = text});
        }
        
        private static void _clearSearchArticle() {
            StoreProvider.store.Dispatch(new ClearSearchArticleAction());
        }

        private static void _saveSearchHistory(string text) {
            if (text.isEmpty()) return;
            StoreProvider.store.Dispatch(new SaveSearchHistoryAction{keyword = text});
        }
        
        private static void _deleteSearchHistory(string text) {
            if (text.isEmpty()) return;
            StoreProvider.store.Dispatch(new DeleteSearchHistoryAction{keyword = text});
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

        public override Widget build(BuildContext context) {
            return new SafeArea(
                child: new Container(
                    color: CColors.White,
                    child: new Column(
                        children: new List<Widget> {
                            _buildSearchBar(context),
                            new Flexible(
                                child: new StoreConnector<AppState, SearchState>(
                                    converter: (state, dispatch) => state.searchState,
                                    builder: (_context, viewModel) => {
                                        if (viewModel.loading) return new Container();
        
                                        var searchArticles = viewModel.searchArticles;
                                        if (viewModel.searchArticles.Count > 0) {
                                            return new Refresh(
                                                onHeaderRefresh: () => {
                                                    pageNumber = 0;
                                                    return _onRefresh(pageNumber);
                                                },
                                                onFooterRefresh: () => {
                                                    pageNumber++;
                                                    return _onRefresh(pageNumber);
                                                },
                                                child: ListView.builder(
                                                    physics: new AlwaysScrollableScrollPhysics(),
                                                    itemCount: searchArticles.Count,
                                                    itemBuilder: (cxt, index) => {
                                                        var searchArticle = searchArticles[index];
                                                        return new ArticleCard(
                                                            searchArticle
                                                        );
                                                    }
                                                )
                                            );
                                        }
        
                                        return new Column(
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: new List<Widget> {
                                                _buildHotSearch(),
                                                _buildSearchHistory(viewModel.searchHistoryList)
                                            }
                                        );
                                    }
                                )
                            )
                        }
                    )
                )
            );
        }

        private Widget _buildSearchBar(BuildContext context) {
            return new Container(
                padding: EdgeInsets.only(16, 40, 16, 16),
                color: CColors.White,
                child: new Row(
                    children: new List<Widget> {
                        new Expanded(
                            child: new Container(
                                decoration: new BoxDecoration(
                                    CColors.White,
                                    border: Border.all(CColors.Separator2),
                                    borderRadius: BorderRadius.circular(22)
                                ),
                                height: 44,
                                child: new InputField(
                                    controller: _controller,
                                    style: new TextStyle(
                                        fontSize: 25,
                                        color: CColors.TextThird
                                    ),
                                    autofocus: true,
                                    hintText: "热门搜索",
                                    hintStyle: new TextStyle(
                                        fontSize: 20,
                                        color: CColors.TextThird
                                    ),
                                    cursorColor: CColors.primary,
                                    textInputAction: TextInputAction.search,
                                    onChanged: text => {
                                        if (text == null || text.Length <= 0) {
                                            _clearSearchArticle();
                                        }
                                    },
                                    onSubmitted: _searchArticle
                                )
                            )
                        ),
                        new Container(width: 8),
                        new CustomButton(
                            onPressed: () => { Navigator.pop(context); },
                            child: new Text("取消")
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
                    onTap: () => { _searchArticle(item); },
                    child: new Container(
                        decoration: new BoxDecoration(
                            CColors.background2,
                            borderRadius: BorderRadius.circular(11)
                        ),
                        height: 22,
                        padding: EdgeInsets.symmetric(horizontal: 5, vertical: 3),
                        child: new Text(
                            item, 
                            style: new TextStyle(
                                fontSize: 16,
                                color: CColors.White,
                                fontFamily: "PingFang-Regular"
                            )
                        )
                    )
                );
                widgets.Add(widget);
            });
            return new Container(
                padding: EdgeInsets.only(16, 0, 16, 16),
                color: CColors.White,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(top: 16),
                            child: new Text(
                                "热门搜索",
                                style: new TextStyle(
                                    fontSize: 20,
                                    fontFamily: "PingFang-Regular"
                                )
                            )
                        ),
                        new Wrap(
                            spacing: 10,
                            runSpacing: 20,
                            children: widgets
                        )
                    }
                )
            );
        }
        
        private Widget _buildSearchHistory(List<string> searchHistoryList) {
            if (searchHistoryList.Count <= 0) {
                return new Container();
            }

            var widgets = new List<Widget>();
            widgets.Add(
                new Container(
                    margin: EdgeInsets.only(bottom: 10),
                    child: new Text(
                        "搜索历史",
                        style: new TextStyle(
                            fontSize: 20,
                            fontFamily: "PingFang-Regular"
                        )
                    )
                )
            );
            searchHistoryList.ForEach(item => {
                var widget = new GestureDetector(
                    onTap: () => { _searchArticle(item); },
                    child: new Container(
                        height: 60,
                        decoration: new BoxDecoration(
                            border: new Border(bottom: new BorderSide(CColors.Separator2))
                        ),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget> {
                                new Row(
                                    children: new List<Widget> {
                                        new Icon(
                                            Icons.history,
                                            size: 28,
                                            color: CColors.Black
                                        ),
                                        new Container(
                                            margin: EdgeInsets.only(8),
                                            child: new Text(
                                                item,
                                                style: new TextStyle(
                                                    fontSize: 16,
                                                    fontFamily: "PingFang-Regular"
                                                )
                                            )
                                        )
                                    }
                                ),
                                new CustomButton(
                                    onPressed: () => { _deleteSearchHistory(item); },
                                    child: new Icon(
                                        Icons.close,
                                        size: 28,
                                        color: CColors.Black
                                    )
                                )
                            }
                        )
                    )
                );
                widgets.Add(widget);
            });
            
            widgets.Add(
                new GestureDetector(
                    onTap: _deleteAllSearchHistory,
                    child: new Container(
                        height: 60,
                        child: new Row(
                            children: new List<Widget> {
                                new Icon(
                                    Icons.delete,
                                    size: 28,
                                    color: CColors.Black
                                ),
                                new Container(
                                    margin: EdgeInsets.only(8),
                                    child: new Text(
                                        "清空搜索历史",
                                        style: new TextStyle(
                                            fontSize: 16,
                                            fontFamily: "PingFang-Regular"
                                        )
                                    )
                                )
                            }
                        )
                    )
                )
            );

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