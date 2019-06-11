using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Utils;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    public class WebViewScreen : StatefulWidget {
        public WebViewScreen(
            string url = null,
            Key key = null
        ) : base(key) {
            this.url = url;
        }

        public readonly string url;

        public override State createState() {
            return new _WebViewScreenState();
        }
    }

    public class _WebViewScreenState : State<WebViewScreen> {
        WebViewObject _webViewObject = null;
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
                this._webViewObject.LoadURL(this.widget.url);
                this._webViewObject.ClearCookies();
                if (HttpManager.getCookie().isNotEmpty()) {
#if UNITY_IOS
                    this._webViewObject.AddCustomHeader("Cookie", HttpManager.getCookie());
#endif
                }

                this._webViewObject.SetVisibility(true);
            }

            this._progress = 0;
            this._onClose = false;
        }

        public override void dispose() {
            if (this._timer != null) {
                this._timer.cancel();
                this._timer.Dispose();
            }

            if (!Application.isEditor) {
                this._webViewObject.SetVisibility(false);
                WebViewManager.instance.destroyWebView();
            }

            base.dispose();
        }

        void startProgress() {
            if (this._timer != null) {
                this._timer.cancel();
                this._timer = null;
            }

            this._timer = Window.instance.run(new TimeSpan(0, 0, 0, 0, 60), () => {
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
            Widget progressWidget = new Container();
            if (this._progress < 1.0f) {
                progressWidget = new CustomProgress(this._progress,
                    CColors.White
                );
            }

            if (!Application.isEditor) {
                var ratio = Window.instance.devicePixelRatio;
                var top = (int) (44 * ratio);
                if (Application.platform != RuntimePlatform.Android) {
                    top = (int) ((MediaQuery.of(context).padding.top + 44) * ratio);
                }

                var bottom = (int) (MediaQuery.of(context).padding.bottom * ratio);
                this._webViewObject.SetMargins(0, top, 0, bottom);
            }

            Widget tipsText = new Container();
            if (this._onClose) {
                tipsText = new Text(
                    "正在关闭...",
                    style: CTextStyle.PXLarge
                );
            }
            else {
                tipsText = new Text(
                    "正在加载...",
                    style: CTextStyle.PXLarge
                );
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
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
                                    child: new Center(
                                        child: tipsText
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new Container(
                height: 44,
                color: CColors.White,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new GestureDetector(
                            onTap: () => {
                                this._onClose = true;
                                this.setState(() => { });
                                if (Router.navigator.canPop()) {
                                    Router.navigator.pop();
                                }

                                if (!Application.isEditor) {
                                    this._webViewObject.SetVisibility(false);
                                    WebViewManager.instance.destroyWebView();
                                }
                            },
                            child: new Container(
                                padding: EdgeInsets.symmetric(10, 16),
                                color: CColors.Transparent,
                                child: new Icon(Icons.arrow_back, size: 24, color: CColors.Icon))
                        )
                    }
                )
            );
        }
    }
}