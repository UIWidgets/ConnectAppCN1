using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Plugins;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class VideoViewScreen : StatefulWidget {
        public VideoViewScreen(
            string url,
            bool needUpdate,
            int limitSeconds,
            Key key = null
        ) : base(key: key) {
            this.url = url;
            this.needUpdate = needUpdate;
            this.limitSeconds = limitSeconds;
        }

        public readonly string url;
        public readonly bool needUpdate;
        public readonly int limitSeconds;

        public override State createState() {
            return new _VideoViewScreenState();
        }
    }

    public class _VideoViewScreenState : State<VideoViewScreen>, RouteAware {
        public override void initState() {
            base.initState();
            StatusBarManager.hideStatusBar(true);
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.Black,
                child: new CustomSafeArea(
                    child: new Container(
                        color: CColors.Black,
                        child: new Stack(
                            children: new List<Widget> {
                                new Positioned(
                                    top: 0, left: 16, right: 0, child: new Container(
                                        child: new Row(
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            children: new List<Widget> {
                                                new CustomButton(
                                                    onPressed: () => Router.navigator.pop(),
                                                    child: new Icon(
                                                        Icons.arrow_back,
                                                        size: 28,
                                                        color: CColors.White
                                                    )
                                                )
                                            }
                                        )
                                    ))
                            }
                        )
                    )
                )
            );
        }

        public void didPopNext() {
            AVPlayerPlugin.showPlayer();
        }

        public void didPush() {
            StatusBarManager.hideStatusBar(true);
            Promise.Delayed(TimeSpan.FromMilliseconds(400)).Then(() => {
                var width = MediaQuery.of(this.context).size.width;
                var height = width * 9 / 16;
                var originY = (MediaQuery.of(this.context).size.height - height) / 2;
                AVPlayerPlugin.initVideoPlayer(this.widget.url, HttpManager.getCookie(), 0, originY, width, height,
                    false,
                    this.widget.needUpdate, this.widget.limitSeconds);
            });
        }

        public void didPop() {
            AVPlayerPlugin.removePlayer();
            StatusBarManager.hideStatusBar(false);
        }

        public void didPushNext() {
            AVPlayerPlugin.hiddenPlayer();
        }
    }
}