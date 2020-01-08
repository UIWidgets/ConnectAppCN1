using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.State;
using ConnectApp.Utils;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.Components {
    public enum TabBarItemStatus {
        normalAnimation,
        normal,
        toRefresh,
        toHome,
        refreshToLeave
    }

    public delegate bool SelectTabCallBack(int fromIndex, int toIndex);

    public class CustomTabBarConnector : StatelessWidget {
        public CustomTabBarConnector(
            List<Widget> controllers,
            List<CustomTabBarItem> items,
            Color backgroundColor,
            SelectTabCallBack tapCallBack = null,
            int initialTabIndex = 0,
            Key key = null
        ) : base(key: key) {
            this.tapCallBack = tapCallBack;
            this.backgroundColor = backgroundColor;
            this.controllers = controllers;
            this.items = items;
            this.initialTabIndex = initialTabIndex;
        }

        readonly SelectTabCallBack tapCallBack;
        readonly Color backgroundColor;
        readonly List<Widget> controllers;
        readonly List<CustomTabBarItem> items;
        readonly int initialTabIndex;


        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, List<string>>(
                converter: state => new List<string> {
                    null,
                    null,
                    state.channelState.totalNotification(),
                    state.loginState.newNotifications
                },
                builder: (context1, notifications, dispatcher) => new CustomTabBar(
                    controllers: this.controllers,
                    items: this.items,
                    backgroundColor: this.backgroundColor,
                    notifications: notifications,
                    tapCallBack: this.tapCallBack,
                    initialTabIndex: this.initialTabIndex
                )
            );
        }
    }

    public class CustomTabBar : StatefulWidget {
        public CustomTabBar(
            List<Widget> controllers,
            List<CustomTabBarItem> items,
            List<string> notifications,
            Color backgroundColor,
            SelectTabCallBack tapCallBack = null,
            int initialTabIndex = 0,
            Key key = null
        ) : base(key: key) {
            D.assert(controllers != null && controllers.Count > 1);
            D.assert(items != null && items.Count > 1);
            D.assert(controllers.Count == items.Count);
            this.controllers = controllers;
            this.items = items;
            this.backgroundColor = backgroundColor;
            this.tapCallBack = tapCallBack;
            this.notifications = notifications;
            this.initialTabIndex = initialTabIndex;
        }

        public readonly SelectTabCallBack tapCallBack;
        public readonly Color backgroundColor;
        public readonly List<Widget> controllers;
        public readonly List<CustomTabBarItem> items;
        public readonly List<string> notifications;
        public readonly int initialTabIndex;

        public override State createState() {
            return new CustomTabBarState();
        }
    }

    public class CustomTabBarState : State<CustomTabBar> {
        PageController _pageController;
        int _selectedIndex;
        float _bottomPadding;
        string _articleRefreshSubId;
        TabBarItemStatus _tabBarItemStatus;

        public override void initState() {
            base.initState();
            this._selectedIndex = this.widget.initialTabIndex;
            this._bottomPadding = 0;
            this._pageController = new PageController(initialPage: this._selectedIndex);
            this._articleRefreshSubId = EventBus.subscribe(sName: EventBusConstant.article_refresh, args => {
                if (args.isNotNullAndEmpty()) {
                    TabBarItemStatus status = (TabBarItemStatus) args[0];
                    if (this._tabBarItemStatus != status) {
                        this.setState(() => this._tabBarItemStatus = status);
                    }
                }
            });
        }

        public override Widget build(BuildContext context) {
            if (this._bottomPadding != MediaQuery.of(context).padding.bottom &&
                Application.platform != RuntimePlatform.Android) {
                this._bottomPadding = MediaQuery.of(context).padding.bottom;
            }

            return new Container(
                child: new Stack(
                    children: new List<Widget> {
                        this._buildContentView(),
                        this._buildBottomTabBar()
                    }
                )
            );
        }

        Widget _buildContentView() {
            return new Container(
                color: CColors.Background,
                child: new PageView(
                    physics: new NeverScrollableScrollPhysics(),
                    children: this.widget.controllers,
                    controller: this._pageController,
                    onPageChanged: this._onPageChanged
                )
            );
        }

        Widget _buildBottomTabBar() {
            return new Positioned(
                left: 0,
                right: 0,
                bottom: 0,
                height: CConstant.TabBarHeight + this._bottomPadding,
                child: new BackdropFilter(
                    filter: ImageFilter.blur(25, 25),
                    child: new Container(
                        decoration: new BoxDecoration(
                            border: new Border(new BorderSide(color: CColors.Separator)),
                            color: this.widget.backgroundColor
                        ),
                        child: new Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            children: new List<Widget> {
                                new Flexible(
                                    child: new Row(
                                        mainAxisAlignment: MainAxisAlignment.spaceAround,
                                        children: this._buildItems()
                                    )
                                ),
                                new Container(height: this._bottomPadding)
                            }
                        )
                    )
                )
            );
        }

        List<Widget> _buildItems() {
            var children = new List<Widget>();
            var screenWidth = MediaQuery.of(context: this.context).size.width;
            this.widget.items.ForEach(item => {
                Widget buildItem = new Flexible(
                    child: new Stack(
                        fit: StackFit.expand,
                        children: new List<Widget> {
                            new GestureDetector(
                                onTap: () => {
                                    if (this._selectedIndex != item.index) {
                                        if (this.widget.tapCallBack != null) {
                                            if (this.widget.tapCallBack(this._selectedIndex, item.index)) {
                                                TabBarItemStatus status;
                                                if (this._tabBarItemStatus == TabBarItemStatus.toRefresh) {
                                                    status = TabBarItemStatus.refreshToLeave;
                                                }
                                                else if (this._tabBarItemStatus == TabBarItemStatus.toHome) {
                                                    status = TabBarItemStatus.normalAnimation;
                                                }
                                                else {
                                                    status = this._tabBarItemStatus;
                                                }

                                                this._pageController.animateToPage(page: item.index,
                                                    TimeSpan.FromMilliseconds(1),
                                                    curve: Curves.ease);
                                                this.setState(() => {
                                                    this._tabBarItemStatus = status;
                                                    this._selectedIndex = item.index;
                                                });
                                            }
                                        }
                                    }
                                    else {
                                        if ((this._tabBarItemStatus == TabBarItemStatus.toRefresh ||
                                             this._tabBarItemStatus == TabBarItemStatus.refreshToLeave) &&
                                            item.index == 0) {
                                            EventBus.publish(sName: EventBusConstant.article_tab,
                                                new List<object> {item.index});
                                        }
                                    }
                                },
                                child: new Container(
                                    color: CColors.Transparent,
                                    child: new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        children: new List<Widget> {
                                            new Padding(
                                                padding: EdgeInsets.only(top: 5),
                                                child: this._buildItemIcon(item: item)
                                            ),
                                            new Padding(
                                                padding: EdgeInsets.only(top: 2.5f),
                                                child: new Text(item.title,
                                                    style: new TextStyle(fontSize: 10,
                                                        color: this._selectedIndex == item.index
                                                            ? item.activeColor
                                                            : item.inActiveColor))
                                            )
                                        }
                                    )
                                )
                            ),
                            new Positioned(
                                left: (float) Math.Ceiling(screenWidth / 8) + 1,
                                top: 4,
                                child: new IgnorePointer(
                                    child: new NotificationDot(
                                        this.widget.notifications[index: item.index],
                                        new BorderSide(color: CColors.White, 2)
                                    )
                                )
                            )
                        }
                    )
                );
                children.Add(buildItem);
            });

            return children;
        }

        Widget _buildItemIcon(CustomTabBarItem item) {
            if (this._selectedIndex != item.index) {
                return new Icon(icon: item.normalIcon, size: item.size, color: item.inActiveColor);
            }

            if (item.index != 0) {
                return new FrameAnimationImage(
                    images: item.selectedImages,
                    size: item.size,
                    type: AnimatingType.forward,
                    defaultWidget: new Icon(icon: item.selectedIcon, size: item.size, color: item.activeColor)
                );
            }

            if (this._tabBarItemStatus == TabBarItemStatus.toRefresh) {
                List<string> loadingImages = new List<string>();
                for (int index = 0; index <= 60; index++) {
                    loadingImages.Add($"image/tab-loading/home-to-refresh/home-to-refresh{index}");
                }

                return new FrameAnimationImage(
                    images: loadingImages,
                    size: item.size,
                    type: AnimatingType.forward
                );
            }

            if (this._tabBarItemStatus == TabBarItemStatus.toHome) {
                List<string> loadingImages = new List<string>();
                for (int index = 0; index <= 60; index++) {
                    loadingImages.Add($"image/tab-loading/refresh-to-home/refresh-to-home{index}");
                }

                return new FrameAnimationImage(
                    images: loadingImages,
                    size: item.size,
                    type: AnimatingType.forward,
                    defaultWidget: new Icon(icon: item.selectedIcon, size: item.size, color: item.activeColor)
                );
            }

            if (this._tabBarItemStatus == TabBarItemStatus.refreshToLeave) {
                return new RotationAnimation(
                    child: new Icon(icon: Icons.tab_home_refresh_fill, size: item.size, color: item.activeColor),
                    animating: AnimatingType.forward
                );
            }

            if (this._tabBarItemStatus == TabBarItemStatus.normal) {
                return new Icon(icon: item.selectedIcon, size: item.size, color: item.activeColor);
            }

            return new FrameAnimationImage(
                images: item.selectedImages,
                size: item.size,
                type: AnimatingType.forward,
                defaultWidget: new Icon(icon: item.selectedIcon, size: item.size, color: item.activeColor)
            );
        }

        void _onPageChanged(int page) {
            this._pageController.jumpToPage(page);
        }

        public override void dispose() {
            this._pageController.dispose();
            EventBus.unSubscribe(sName: EventBusConstant.article_refresh, id: this._articleRefreshSubId);
            base.dispose();
        }
    }
}