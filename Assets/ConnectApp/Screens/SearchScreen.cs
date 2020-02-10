using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
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
        public SearchScreenConnector(
            Key key = null
        ) : base(key: key) { 
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, SearchScreenViewModel>(
                converter: state => new SearchScreenViewModel {
                    searchKeyword = state.searchState.keyword,
                    searchArticleIds = state.searchState.searchArticleIdDict.ContainsKey(key: state.searchState.keyword)
                        ? state.searchState.searchArticleIdDict[key: state.searchState.keyword]
                        : new List<string>(),
                    searchArticleHistoryList = state.searchState.searchArticleHistoryList,
                    searchSuggest = state.articleState.searchSuggest,
                    popularSearchArticleList = state.popularSearchState.popularSearchArticles,
                    searchUserIds = state.searchState.searchUserIdDict.ContainsKey(key: state.searchState.keyword)
                        ? state.searchState.searchUserIdDict[key: state.searchState.keyword]
                        : new List<string>(),
                    searchTeamIds = state.searchState.searchTeamIdDict.ContainsKey(key: state.searchState.keyword)
                        ? state.searchState.searchTeamIdDict[key: state.searchState.keyword]
                        : new List<string>()
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new SearchScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        fetchPopularSearch = () => dispatcher.dispatch<IPromise>(Actions.popularSearchArticle()),
                        startSearchArticle = keyword => dispatcher.dispatch(new StartSearchArticleAction {
                            keyword = keyword
                        }),
                        searchArticle = (keyword, pageNumber) => dispatcher.dispatch<IPromise>(
                            Actions.searchArticles(keyword, pageNumber)),
                        startSearchUser = () => dispatcher.dispatch(new StartSearchUserAction()),
                        searchUser = (keyword, pageNumber) => dispatcher.dispatch<IPromise>(
                            Actions.searchUsers(keyword, pageNumber)),
                        startSearchTeam = () => dispatcher.dispatch(new StartSearchTeamAction()),
                        searchTeam = (keyword, pageNumber) => dispatcher.dispatch<IPromise>(
                            Actions.searchTeams(keyword, pageNumber)),
                        clearSearchResult = () => dispatcher.dispatch(new ClearSearchResultAction()),
                        saveSearchArticleHistory = keyword =>
                            dispatcher.dispatch(new SaveSearchArticleHistoryAction {keyword = keyword}),
                        deleteSearchArticleHistory = keyword => 
                            dispatcher.dispatch(new DeleteSearchArticleHistoryAction {keyword = keyword}),
                        deleteAllSearchArticleHistory = () =>
                            dispatcher.dispatch(new DeleteAllSearchArticleHistoryAction())
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
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly SearchScreenViewModel viewModel;
        public readonly SearchScreenActionModel actionModel;

        public override State createState() {
            return new _SearchScreenState();
        }
    }

    class _SearchScreenState : State<SearchScreen>, RouteAware {
        readonly TextEditingController _controller = new TextEditingController("");
        FocusNode _focusNode;
        int _selectedIndex;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._focusNode = new FocusNode();
            this._selectedIndex = 0;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (this.widget.viewModel.searchKeyword.Length > 0
                    || this.widget.viewModel.searchArticleIds.Count > 0
                    || this.widget.viewModel.searchUserIds.Count > 0
                    || this.widget.viewModel.searchTeamIds.Count > 0) {
                    this.widget.actionModel.clearSearchResult();
                }

                this.widget.actionModel.fetchPopularSearch();
            });
        }
        
        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(this.context));
        }

        public override void dispose() {
            this._controller.dispose();
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        void _searchResult(string text) {
            if (text.isEmpty() && this.widget.viewModel.searchSuggest.isEmpty()) {
                return;
            }
            
            var searchKey = "";
            if (this.widget.viewModel.searchSuggest.isNotEmpty()) {
                searchKey = this.widget.viewModel.searchSuggest;
            }
            if (text.isNotEmpty()) {
                searchKey = text; 
            }

            if (this._focusNode.hasFocus) {
                this._focusNode.unfocus();
            }
            this._controller.text = searchKey;

            if (this._selectedIndex == 0) {
                this._searchArticle(text: searchKey);
            }
            if (this._selectedIndex == 1) {
                this._searchUser(text: searchKey);
            }
            if (this._selectedIndex == 2) {
                this._searchTeam(text: searchKey);
            }
        }

        void _searchArticle(string text) {
            this.widget.actionModel.saveSearchArticleHistory(obj: text);
            this.widget.actionModel.startSearchArticle(obj: text);
            this.widget.actionModel.searchArticle(arg1: text, 0);
        }
        
        void _searchUser(string text) {
            this.widget.actionModel.startSearchUser();
            this.widget.actionModel.searchUser(arg1: text, 1);
        }

        void _searchTeam(string text) {
            this.widget.actionModel.startSearchTeam();
            this.widget.actionModel.searchTeam(arg1: text, 1);
        }

        public override Widget build(BuildContext context) {
            Widget child;
            if (this.widget.viewModel.searchKeyword.Length > 0) {
                child = this._buildSearchResult();
            }
            else {
                child = new ListView(
                    children: new List<Widget> {
                        this._buildSearchHistory(),
                        this._buildPopularSearch()
                    }
                );
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        child: new Column(
                            children: new List<Widget> {
                                this._buildSearchBar(),
                                new Flexible(
                                    child: new NotificationListener<ScrollNotification>(
                                        onNotification: notification => {
                                            if (this._focusNode.hasFocus) {
                                                this._focusNode.unfocus();
                                            }

                                            return true;
                                        },
                                        child: child
                                    )
                                )
                            }
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
                            onPressed: () => this.widget.actionModel.mainRouterPop(),
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
                            hintText: this.widget.viewModel.searchSuggest ?? "搜索",
                            hintStyle: CTextStyle.H2Body4,
                            cursorColor: CColors.PrimaryBlue,
                            textInputAction: TextInputAction.search,
                            clearButtonMode: InputFieldClearButtonMode.whileEditing,
                            onChanged: text => {
                                if (text == null || text.Length <= 0) {
                                    this._selectedIndex = 0;
                                    this.widget.actionModel.clearSearchResult();
                                }
                            },
                            onSubmitted: this._searchResult
                        )
                    }
                )
            );
        }

        Widget _buildSearchResult() {
            return new CustomSegmentedControl(
                new List<object> {"文章", "用户", "公司"},
                new List<Widget> {
                    new SearchArticleScreenConnector(),
                    new SearchUserScreenConnector(),
                    new SearchTeamScreenConnector()
                },
                newValue => {
                    this._selectedIndex = newValue;
                    this._searchResult(text: this.widget.viewModel.searchKeyword);
                }
            );
        }

        Widget _buildPopularSearch() {
            if (this.widget.viewModel.popularSearchArticleList.Count <= 0) {
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
                            children: this._buildPopularSearchItem()
                        )
                    }
                )
            );
        }

        List<Widget> _buildPopularSearchItem() {
            var popularSearch = this.widget.viewModel.popularSearchArticleList;
            List<Widget> widgets = new List<Widget>();
            popularSearch.ForEach(item => {
                Widget widget = new GestureDetector(
                    onTap: () => this._searchResult(item.keyword),
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

        Widget _buildSearchHistory() {
            var searchHistoryList = this.widget.viewModel.searchArticleHistoryList;
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
                                                    () => this.widget.actionModel.deleteAllSearchArticleHistory()),
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
                    onTap: () => this._searchResult(item),
                    child: new Container(
                        height: 44,
                        color: CColors.White,
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget> {
                                new Expanded(
                                    child: new Text(
                                        data: item,
                                        maxLines: 1,
                                        overflow: TextOverflow.ellipsis,
                                        style: CTextStyle.PLargeBody
                                    )
                                ),
                                new CustomButton(
                                    padding: EdgeInsets.only(8, 8, 0, 8),
                                    onPressed: () => this.widget.actionModel.deleteSearchArticleHistory(item),
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
        
        public void didPopNext() {
            StatusBarManager.statusBarStyle(false);
        }

        public void didPush() {
        }

        public void didPop() {
        }

        public void didPushNext() {
        }
    }
}