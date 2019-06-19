using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.screens;
using ConnectApp.Utils;
using Unity.UIWidgets.async;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
    public class SplashPage : StatefulWidget {
        public override State createState() {
            return new _SplashPageState();
        }
    }

    class _SplashPageState : State<SplashPage> {
        bool _isShow;
        Timer _timer;
        int _lastSecond = 5;
        BuildContext _context;

        public override void initState() {
            base.initState();
            StatusBarManager.hideStatusBar(true);
            this._isShow = SplashManager.isExistSplash();
            if (this._isShow) {
                this._lastSecond = SplashManager.getSplash().duration;
                this._timer = Window.instance.run(TimeSpan.FromSeconds(1), this.t_Tick, true);
            }

            SplashManager.fetchSplash();
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

            return new Container(
                color: CColors.White,
                child: new Stack(
                    children: new List<Widget> {
                        new Column(
                            children: new List<Widget> {
                                new Container(
                                    width: MediaQuery.of(context).size.width,
                                    height: MediaQuery.of(context).size.height,
                                    child: Image.memory(SplashManager.readImage(), fit: BoxFit.cover)
                                )
                            }
                        ),
                        new Positioned(
                            top: MediaQuery.of(context).padding.top + 24,
                            right: 16,
                            child: new GestureDetector(
                                child: new Container(
                                    decoration: new BoxDecoration(
                                        color: Color.fromRGBO(149, 149, 149, 0.6f),
                                        borderRadius: BorderRadius.all(19)
                                    ),
                                    width: 80,
                                    height: 38,
                                    alignment: Alignment.center,
                                    padding: EdgeInsets.all(2),
                                    child: new Text($"跳过 {this._lastSecond}", style: new TextStyle(
                                        fontSize: 16,
                                        fontFamily: "PingFangSC-Regular",
                                        color: CColors.White
                                    ))
                                ),
                                onTap: pushCallback
                            )
                        )
                    }
                )
            );
        }

        static void pushCallback() {
            Router.navigator.pushReplacementNamed(MainNavigatorRoutes.Main);
        }

        void t_Tick() {
            using (WindowProvider.of(this._context).getScope()) {
                this.setState(() => { this._lastSecond -= 1; });
                if (this._lastSecond < 1) {
                    pushCallback();
                }
            }
        }
    }
}