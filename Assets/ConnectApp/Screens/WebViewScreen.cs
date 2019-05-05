using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens
{ 
    public class WebViewScreen : StatefulWidget
    {
        public WebViewScreen(
            string url = null ,
            Key key = null
        ) : base(key)
        {
            this.url = url;
        }

        public readonly string url;

        public override State createState()
        {
            return new _WebViewScreenState();
        }
    }
    public class _WebViewScreenState : State<WebViewScreen>
    {
        private WebViewObject _webViewObject = null;
        public override void initState()
        {
            base.initState();
            if (!Application.isEditor)
            {
                _webViewObject=WebViewManager.instance.webViewObject; 
                _webViewObject.Init(ua:"",enableWKWebView:true);
                _webViewObject.EvaluateJS(@"document.cookie = '" + HttpManager.getCookie() + "';");
                _webViewObject.LoadURL(widget.url);
                _webViewObject.SetVisibility(true);
            }
            
        }

        public override Widget build(BuildContext context)
        {
            if (!Application.isEditor)
            {
                var ratio = Window.instance.devicePixelRatio;
                var top = (int) ( 44 * ratio);
                if (Application.platform != RuntimePlatform.Android)
                {
                    top = (int) ((MediaQuery.of(context).padding.top + 44) * ratio);
                }
                var bottom = (int) (MediaQuery.of(context).padding.bottom * ratio);
                _webViewObject.SetMargins(0, top,0, bottom);
            }
            var child = new Container(
                color: CColors.background3,
                child: new Column(
                    children: new List<Widget> {
                        _buildNavigationBar(context)
                        
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
        private Widget _buildNavigationBar(BuildContext context) {
            return new Container(
                height: 44,
                color: CColors.White,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new GestureDetector(
                            onTap: () =>
                            {
                                Navigator.pop(context);
                                if (!Application.isEditor)
                                {
                                    _webViewObject.SetVisibility(false);
                                }
                            },
                            child: new Container(
                                padding: EdgeInsets.symmetric(10, 16),
                                color: CColors.Transparent,
                                child: new Icon(Icons.arrow_back, size: 24, color: CColors.icon3))
                        )
                    }
                )
            );
        }

        public override void dispose()
        {
            base.dispose();
        }
    }



}