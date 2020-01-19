using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Plugins;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class SplashScreen : StatefulWidget {
        public SplashScreen(
            Key key = null
        ) : base(key: key) {
        }

        public override State createState() {
            return new _SplashScreenState();
        }
    }

    class _SplashScreenState : State<SplashScreen> {
        bool _isShow;
        Timer _timer;
        int _lastSecond = 5;
        BuildContext _context;
        uint hexColor;

        public override void initState() {
            base.initState();
            StatusBarManager.hideStatusBar(true);
            this._isShow = SplashManager.isExistSplash();
            if (this._isShow) {
                this._lastSecond = SplashManager.getSplash().duration;
                this._timer = Window.instance.run(TimeSpan.FromSeconds(1), this.timeDown, true);
            }

            var isShowLogo = SplashManager.getSplash().isShowLogo;
            var hexColorStr = SplashManager.getSplash().color;
            if (isShowLogo) {
                this.hexColor = 0xFFFFFFFF;
                try {
                    this.hexColor = Convert.ToUInt32(value: hexColorStr, 16);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
        }

        public override void dispose() {
            this._timer?.Dispose();
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            this._context = context;
            if (!this._isShow) {
                return new MainScreen();
            }

            var topPadding = Application.platform != RuntimePlatform.Android
                ? MediaQuery.of(context: context).padding.top
                : 0f;

            var isShowLogo = SplashManager.getSplash()?.isShowLogo ?? false;
            Widget logoWidget;
            if (isShowLogo) {
                logoWidget = new Positioned(
                    top: topPadding + 24,
                    left: 16,
                    child: new Icon(
                        icon: Icons.LogoWithUnity,
                        size: 35,
                        color: new Color(value: this.hexColor)
                    )
                );
            }
            else {
                logoWidget = new Container();
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    top: false,
                    child: new Stack(
                        children: new List<Widget> {
                            new Column(
                                mainAxisAlignment: MainAxisAlignment.end,
                                children: new List<Widget> {
                                    new GestureDetector(
                                        child: new Container(
                                            width: MediaQuery.of(context).size.width,
                                            height: MediaQuery.of(context).size.height - 116 -
                                                    CCommonUtils.getSafeAreaBottomPadding(context: context),
                                            child: Image.memory(SplashManager.readImage(), fit: BoxFit.cover)
                                        ),
                                        onTap: this.pushPage
                                    ),
                                    new Container(
                                        width: 182,
                                        height: 32,
                                        margin: EdgeInsets.only(top: 36),
                                        child: Image.asset("image/iOS/unityConnectBlack.imageset/unityConnectBlack")
                                    ),
                                    new Container(
                                        width: 101,
                                        height: 22,
                                        margin: EdgeInsets.only(top: 6, bottom: 20),
                                        child: Image.asset("image/iOS/madeWithUnity.imageset/madeWithUnity")
                                    )
                                }
                            ),
                            new Positioned(
                                top: topPadding + 24,
                                right: 16,
                                child: new GestureDetector(
                                    child: new Container(
                                        decoration: new BoxDecoration(
                                            Color.fromRGBO(0, 0, 0, 0.5f),
                                            borderRadius: BorderRadius.all(16)
                                        ),
                                        width: 65,
                                        height: 32,
                                        alignment: Alignment.center,
                                        child: new Text($"跳过 {this._lastSecond}", style: new TextStyle(
                                            fontSize: 14,
                                            fontFamily: "Roboto-Regular",
                                            color: CColors.White
                                        ))
                                    ),
                                    onTap: this.pushCallback
                                )
                            ),
                            logoWidget
                        }
                    ))
            );
        }

        void pushPage() {
            this.cancelTimer();
            var splash = SplashManager.getSplash();
            AnalyticsManager.ClickSplashPage(splash.id, splash.name, splash.url);
            Router.navigator.pushReplacementNamed(MainNavigatorRoutes.Main);
            JPushPlugin.openUrlScheme(splash.url);
        }

        void pushCallback() {
            this.cancelTimer();
            var splash = SplashManager.getSplash();
            AnalyticsManager.ClickSkipSplashPage(splash.id, splash.name, splash.url);
            StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushReplaceMainAction());
        }

        void cancelTimer() {
            this._timer?.cancel();
        }

        void timeDown() {
            using (WindowProvider.of(this._context).getScope()) {
                this.setState(() => { this._lastSecond -= 1; });
                if (this._lastSecond < 1) {
                    this.pushCallback();
                }
            }
        }
    }
}