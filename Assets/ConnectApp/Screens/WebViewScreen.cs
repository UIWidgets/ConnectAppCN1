using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    public class WebViewScreen : StatefulWidget {
        public WebViewScreen(
            string url = null,
            bool landscape = false,
            bool fullScreen = false,
            bool showOpenInBrowser = true,
            Key key = null
        ) : base(key: key) {
            this.url = url;
            this.landscape = landscape;
            this.fullScreen = fullScreen || landscape;
            this.showOpenInBrowser = showOpenInBrowser;
        }

        public readonly string url;
        public readonly bool landscape;
        public readonly bool fullScreen;
        public readonly bool showOpenInBrowser;

        public override State createState() {
            return new _WebViewScreenState();
        }
    }

    public class _WebViewScreenState : State<WebViewScreen>, RouteAware {
        WebViewObject _webViewObject;
        float _progress;
        bool _onClose;
        Timer _timer;

        public override void initState() {
            base.initState();
            if (!Application.isEditor) {
                this._webViewObject = WebViewManager.instance.getWebView();
                this._webViewObject.Init(
                    ua: "",
                    enableWKWebView: true,
                    transparent: true,
                    started: start => {
                        using (WindowProvider.of(this.context).getScope()) {
                            this.startProgress();
                        }
                    },
                    ld: ld => {
                        using (WindowProvider.of(this.context).getScope()) {
                            this.stopProgress();
                        }
                    }
                );
                this._webViewObject.ClearCookies();
                if (HttpManager.getCookie().isNotEmpty()) {
                    this._webViewObject.AddCustomHeader("Cookie", HttpManager.getCookie());
                }

                this._webViewObject.LoadURL(this.widget.url);
                this._webViewObject.SetVisibility(true);
            }

            this._progress = 0;
            this._onClose = false;
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            if (this._timer != null) {
                this._timer.cancel();
                this._timer.Dispose();
            }

            if (!Application.isEditor) {
                this._webViewObject.SetVisibility(false);
                WebViewManager.destroyWebView();
                if (this.widget.landscape) {
                    Screen.orientation = ScreenOrientation.Portrait;
                }

                if (this.widget.fullScreen) {
                    Screen.fullScreen = false;
                }
            }

            Router.routeObserve.unsubscribe(this);

            base.dispose();
        }

        void startProgress() {
            if (this._timer != null) {
                this._timer.cancel();
                this._timer = null;
            }

            this._timer = Window.instance.run(TimeSpan.FromMilliseconds(60), () => {
                if (this._progress < 0.9f) {
                    this._progress += 0.03f;
                    this.setState(() => { });
                }
                else {
                    this._timer.cancel();
                }
            }, true);
        }

        void stopProgress() {
            if (this._timer != null) {
                this._timer.cancel();
                this._timer = null;
            }

            this._progress = 1;
            this.setState(() => { });
        }

        public override Widget build(BuildContext context) {
            Widget progressWidget;
            if (this._progress < 1.0f) {
                progressWidget = new CustomProgress(this._progress,
                    CColors.White
                );
            }
            else {
                progressWidget = new Container();
            }

            if (!Application.isEditor) {
                var ratio = Window.instance.devicePixelRatio;
                var navigationBarHeight = this.widget.fullScreen ? 0 : 44;
                var top = (int) (navigationBarHeight * ratio);
                if (Application.platform != RuntimePlatform.Android) {
                    top = (int) ((MediaQuery.of(context).padding.top + navigationBarHeight) * ratio);
                }

                var left = this.widget.landscape ? (int) (80 * ratio) : 0;
                var right = left;

                var bottom = (int) (MediaQuery.of(context).padding.bottom * ratio);
                this._webViewObject.SetMargins(left, top, right, bottom);

                if (this.widget.landscape && this._progress == 1) {
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                    Screen.fullScreen = true;
                }
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    top: !this.widget.fullScreen,
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                new Container(
                                    child: new Stack(
                                        children: new List<Widget> {
                                            this._buildNavigationBar(),
                                            new Align(
                                                alignment: Alignment.bottomCenter,
                                                child: progressWidget
                                            )
                                        }
                                    )
                                ),
                                new Expanded(
                                    child: new Container(
                                        color: this.widget.landscape ? CColors.Black : null,
                                        child: new Center(
                                            child: new Text(
                                                this._onClose ? "正在关闭..." : "",
                                                style: CTextStyle.PXLarge
                                            )
                                        )
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomAppBar(
                () => {
                    this._onClose = true;
                    this.setState(() => { });
                    if (Router.navigator.canPop()) {
                        Router.navigator.pop();
                    }

                    if (!Application.isEditor) {
                        this._webViewObject.SetVisibility(false);
                        WebViewManager.destroyWebView();
                    }
                },
                rightWidget: this.widget.showOpenInBrowser ?
                    (Widget) new CustomButton(
                        onPressed: () => StoreProvider.store.dispatcher.dispatch(new OpenUrlAction {url = this.widget.url}),
                        child: new Icon(
                            icon: Icons.open_in_browser,
                            size: 24,
                            color: CColors.Icon
                        )
                    )
                    : new Container(),
                backgroundColor: this._progress == 1 && this.widget.fullScreen ? CColors.Black : null,
                bottomSeparatorColor: this._progress == 1 && this.widget.fullScreen ? CColors.Black : null
            );
        }

        public void didPopNext() {
            if (!Application.isEditor) {
                this._webViewObject.SetVisibility(true);
            }
        }

        public void didPush() {
            if (!Application.isEditor) {
                this._webViewObject.SetVisibility(true);
            }
        }

        public void didPop() {
            if (!Application.isEditor) {
                this._webViewObject.SetVisibility(false);
                WebViewManager.destroyWebView();
            }
        }

        public void didPushNext() {
            if (!Application.isEditor) {
                this._webViewObject.SetVisibility(false);
            }
        }
    }
}