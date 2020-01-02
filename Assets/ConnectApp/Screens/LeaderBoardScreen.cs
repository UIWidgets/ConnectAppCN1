using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class LeaderBoardScreenConnector : StatelessWidget {
        public LeaderBoardScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, LeaderBoardScreenViewModel>(
                converter: state => new LeaderBoardScreenViewModel(),
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new LeaderBoardScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction())
                    };
                    return new LeaderBoardScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class LeaderBoardScreen : StatefulWidget {
        public LeaderBoardScreen(
            LeaderBoardScreenViewModel viewModel,
            LeaderBoardScreenActionModel actionModel,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly LeaderBoardScreenViewModel viewModel;
        public readonly LeaderBoardScreenActionModel actionModel;

        public override State createState() {
            return new _LeaderBoardScreenState();
        }
    }

    class _LeaderBoardScreenState : TickerProviderStateMixin<LeaderBoardScreen> {
        int _selectedIndex;
        CustomTabController _tabController;
        RefreshController _refreshController;
        bool _isHaveTitle;

        public override void initState() {
            base.initState();
            this._selectedIndex = 0;
            this._tabController = new CustomTabController(3, this, initialIndex: this._selectedIndex);
            this._refreshController = new RefreshController();
            this._isHaveTitle = true;
            this._tabController.addListener(() => {
                if (this._tabController.index != this._selectedIndex) {
                    this.setState(() => this._selectedIndex = this._tabController.index);
                }
            });
        }

        public override void dispose() {
            this._tabController.dispose();
            base.dispose();
        }

        void _onRefresh(bool up) {
        }

        bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            if (pixels >= 44) {
                if (this._isHaveTitle) {
                    this.setState(() => this._isHaveTitle = false);
                }
            }
            else {
                if (!this._isHaveTitle) {
                    this.setState(() => this._isHaveTitle = true);
                }
            }

            return true;
        }

        public override Widget build(BuildContext context) {
            List<Color> colors;
            string patternImage;
            if (this._selectedIndex == 2) {
                colors = new List<Color> {
                    new Color(0xFFFFB84F),
                    new Color(0xFFFF8024)
                };
                patternImage = "image/leaderboard-pattern-curves2";
            }
            else {
                colors = new List<Color> {
                    new Color(0xFF6EC6FF),
                    CColors.PrimaryBlue,
                    CColors.MessageReactionCount
                };
                patternImage = "image/leaderboard-pattern-curves1";
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    top: false,
                    bottom: false,
                    child: new NotificationListener<ScrollNotification>(
                        onNotification: this._onNotification,
                        child: new Container(
                            decoration: new BoxDecoration(
                                gradient: new LinearGradient(
                                    colors: colors,
                                    begin: Alignment.topLeft,
                                    end: Alignment.bottomRight
                                )
                            ),
                            child: new Stack(
                                alignment: Alignment.topLeft,
                                children: new List<Widget> {
                                    this._isHaveTitle ? (Widget) Image.asset(name: patternImage) : new Container(),
                                    Positioned.fill(
                                        new Column(
                                            children: new List<Widget> {
                                                this._buildNavigationBar(context: context),
                                                new Flexible(
                                                    child: new Container(
                                                        child: new CustomListView(
                                                            controller: this._refreshController,
                                                            enablePullDown: false,
                                                            enablePullUp: false,
                                                            onRefresh: this._onRefresh,
                                                            itemCount: 10,
                                                            itemBuilder: this._buildLeaderBoardCard,
                                                            headerWidget: this._buildListViewHeader(context: context),
                                                            footerWidget: CustomListViewConstant.defaultFooterWidget
                                                        )
                                                    )
                                                )
                                            }
                                        )
                                    )
                                }
                            )
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar(BuildContext context) {
            Color navigationBarColor;
            Border border;
            Widget titleWidget;
            Color backColor;
            if (this._isHaveTitle) {
                navigationBarColor = CColors.Transparent;
                border = null;
                titleWidget = new Text(
                    "优选榜单",
                    style: new TextStyle(
                        fontSize: 18,
                        fontFamily: "Roboto-Medium",
                        color: CColors.White
                    )
                );
                backColor = CColors.White;
            }
            else {
                navigationBarColor = CColors.White;
                border = new Border(bottom: new BorderSide(color: CColors.Separator2));
                titleWidget = this._buildTabBarHeader();
                backColor = CColors.TextTitle;
            }

            return new Container(
                height: CustomAppBarUtil.appBarHeight + CCommonUtils.getSafeAreaTopPadding(context: context),
                padding: EdgeInsets.only(top: CCommonUtils.getSafeAreaTopPadding(context: context)),
                decoration: new BoxDecoration(
                    color: navigationBarColor,
                    border: border
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.only(16, 10, 16, 10),
                            onPressed: () => this.widget.actionModel.mainRouterPop(),
                            child: new Icon(
                                icon: Icons.arrow_back,
                                size: 24,
                                color: backColor
                            )
                        ),
                        titleWidget,
                        new Container(width: 56)
                    }
                )
            );
        }

        Widget _buildTabBarHeader() {
            return new Container(
                height: 44,
                color: CColors.Transparent,
                child: new CustomTabBarHeader(
                    tabs: new List<Widget> {
                        new Padding(
                            padding: EdgeInsets.symmetric(10),
                            child: new Text("合辑")
                        ),
                        new Padding(
                            padding: EdgeInsets.symmetric(10),
                            child: new Text("专栏")
                        ),
                        new Padding(
                            padding: EdgeInsets.symmetric(10),
                            child: new Text("博主")
                        )
                    },
                    controller: this._tabController,
                    indicatorSize: CustomTabBarIndicatorSize.fixedOrLabel,
                    indicatorFixedSize: 16,
                    indicatorChangeStyle: CustomTabBarIndicatorChangeStyle.enlarge,
                    indicatorColor: this._isHaveTitle ? CColors.White : CColors.TextTitle,
                    unselectedLabelStyle: this._isHaveTitle ? CTextStyle.PLargeWhite : CTextStyle.PLargeTitle,
                    unselectedLabelColor: this._isHaveTitle ? CColors.White : CColors.TextTitle,
                    labelStyle: this._isHaveTitle ? CTextStyle.PLargeMediumWhite : CTextStyle.PLargeMedium,
                    labelColor: this._isHaveTitle ? CColors.White : CColors.TextTitle,
                    isScrollable: true
                )
            );
        }

        Widget _buildListViewHeader(BuildContext context) {
            Widget centerWidget;
            if (this._selectedIndex == 2) {
                centerWidget = new Container(
                    height: 286,
                    child: new Stack(
                        children: new List<Widget> {
                            Image.asset(
                                "image/leaderboard-podium",
                                width: MediaQuery.of(context: context).size.width - 32,
                                height: 128,
                                fit: BoxFit.fill
                            ),
                            new Positioned(
                                left: 24,
                                bottom: 116,
                                width: 96,
                                child: new Column(
                                    children: new List<Widget> {
                                        new Container(
                                            width: 64,
                                            height: 64,
                                            decoration: new BoxDecoration(
                                                color: CColors.Red,
                                                borderRadius: BorderRadius.all(32)
                                            )
                                        ),
                                        new Padding(
                                            padding: EdgeInsets.only(top: 8),
                                            child: new Text(
                                                "Michael Wang",
                                                maxLines: 1,
                                                overflow: TextOverflow.ellipsis,
                                                style: CTextStyle.PMediumWhite
                                            )
                                        )
                                    }
                                )
                            ),
                            new Positioned(
                                left: (MediaQuery.of(context: context).size.width - 104) / 2 - 6,
                                bottom: 152,
                                width: 104,
                                child: new Column(
                                    children: new List<Widget> {
                                        new Container(
                                            width: 64,
                                            height: 64,
                                            decoration: new BoxDecoration(
                                                color: CColors.Red,
                                                borderRadius: BorderRadius.all(32)
                                            )
                                        ),
                                        new Padding(
                                            padding: EdgeInsets.only(top: 8),
                                            child: new Text(
                                                "Wu Xiaomu",
                                                maxLines: 1,
                                                overflow: TextOverflow.ellipsis,
                                                style: CTextStyle.PMediumWhite
                                            )
                                        )
                                    }
                                )
                            ),
                            new Positioned(
                                right: 24,
                                bottom: 96,
                                width: 96,
                                child: new Column(
                                    children: new List<Widget> {
                                        new Container(
                                            width: 64,
                                            height: 64,
                                            decoration: new BoxDecoration(
                                                color: CColors.Red,
                                                borderRadius: BorderRadius.all(32)
                                            )
                                        ),
                                        new Padding(
                                            padding: EdgeInsets.only(top: 8),
                                            child: new Text(
                                                "樱花兔",
                                                maxLines: 1,
                                                overflow: TextOverflow.ellipsis,
                                                style: CTextStyle.PMediumWhite
                                            )
                                        )
                                    }
                                )
                            )
                        }
                    )
                );
            }
            else {
                centerWidget = new Container(height: 16);
            }

            return new Column(
                children: new List<Widget> {
                    this._buildTabBarHeader(),
                    centerWidget,
                    new Container(
                        height: 54,
                        alignment: Alignment.center,
                        decoration: new BoxDecoration(
                            color: CColors.White,
                            borderRadius: BorderRadius.only(12, 12),
                            border: new Border(bottom: new BorderSide(color: CColors.Separator2))
                        ),
                        child: new Text("每周三更新", style: CTextStyle.PRegularBody4)
                    )
                }
            );
        }

        Widget _buildLeaderBoardCard(BuildContext context, int index) {
            if (this._selectedIndex == 0) {
                return new LeaderBoardCollectionCard(index: index);
            }

            if (this._selectedIndex == 1) {
                return new LeaderBoardColumnCard(index: index);
            }

            if (this._selectedIndex == 2) {
                return new LeaderBoardBloggerCard(index: index);
            }

            return new Container();
        }
    }
}