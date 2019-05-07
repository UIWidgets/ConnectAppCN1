using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    public class WebViewScreenConnector : StatelessWidget {
        public WebViewScreenConnector(
            string url = null,
            Key key = null
        ) : base(key) {
            this.url = url;
        }

        private readonly string url;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, string>(
                converter: state => url,
                builder: (context1, viewModel, dispatcher) => new WebViewScreen(viewModel, () => dispatcher.dispatch(new MainNavigatorPopAction()))
            );
        }
    }
    
    public class WebViewScreen : StatefulWidget {
        public WebViewScreen(
            string url = null,
            Action mainRouterPop = null,
            Key key = null
        ) : base(key) {
            D.assert(url != null);
            this.url = url;
            this.mainRouterPop = mainRouterPop;
        }

        public readonly string url;
        public readonly Action mainRouterPop;

        public override State createState() {
            return new _WebViewScreenState();
        }
    }
    public class _WebViewScreenState : State<WebViewScreen> {
        private WebViewObject _webViewObject;
        private float _progress;
        private Timer _timer;

        public override void initState() {
            base.initState();
            if (!Application.isEditor) {
                _webViewObject = WebViewManager.instance.webViewObject; 
                _webViewObject.Init(ua: "", enableWKWebView: true, ld: obj => {
                    _timer.cancel();
                    _timer.Dispose();
                    setState(() => {
                        _progress = 1.0f;
                    });
                });
                _webViewObject.EvaluateJS(@"document.cookie = '" + HttpManager.getCookie() + "';");
                _webViewObject.LoadURL(widget.url);
                _webViewObject.SetVisibility(true);
            }
            _progress = 0;
            _timer = Window.instance.run(new TimeSpan(0,0,0,0,300), () => {
                if (_progress < 0.8f) {
                    setState(() => {
                        _progress += 0.2f;
                    });
                } else {
                    _timer.cancel();
                    _timer.Dispose();
                }
            }, true);
        }
        
        public override void dispose() {
            _timer.cancel();
            _timer.Dispose();
            if (!Application.isEditor) {
                _webViewObject.SetVisibility(false);
            }
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            Widget progressWidget = new Container();
            var progressHeight = 0;
            if (_progress < 1.0f) {
                progressWidget = new CustomProgress(
                    _progress
                );
                progressHeight = 2;
            }
            if (!Application.isEditor) {
                var ratio = Window.instance.devicePixelRatio;
                var top = (int) (44 * ratio);
                if (Application.platform != RuntimePlatform.Android) {
                    top = (int) ((MediaQuery.of(context).padding.top + 44) * ratio);
                }
                var bottom = (int) (MediaQuery.of(context).padding.bottom * ratio);
                _webViewObject.SetMargins(0, top + progressHeight,0, bottom);
            }
            var child = new Container(
                color: CColors.background3,
                child: new Column(
                    children: new List<Widget> {
                        _buildNavigationBar(),
                        progressWidget
                    }
                )
            );
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: child
                )
            );
        }       
        private Widget _buildNavigationBar() {
            return new Container(
                height: 44,
                color: CColors.White,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new GestureDetector(
                            onTap: () => widget.mainRouterPop(),
                            child: new Container(
                                padding: EdgeInsets.symmetric(10, 16),
                                color: CColors.Transparent,
                                child: new Icon(Icons.arrow_back, size: 24, color: CColors.icon3)
                            )
                        )
                    }
                )
            );
        }
    }
}