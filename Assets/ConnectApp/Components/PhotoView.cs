using System.Collections.Generic;
using System.Linq;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Plugins;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
    public class PhotoView : StatefulWidget {
        public readonly List<string> urls;
        public readonly PageController controller;
        public readonly Dictionary<string, string> headers;
        public readonly bool useCachedNetworkImage;
        public readonly int index;

        public PhotoView(
            List<string> urls = null,
            int index = 0,
            PageController controller = null,
            Dictionary<string, string> headers = null,
            bool useCachedNetworkImage = false,
            Key key = null) : base(key: key) {
            D.assert(urls != null);
            D.assert(urls.isNotEmpty());
            this.urls = urls;
            this.index = index;
            this.controller = controller ?? new PageController(index);
            this.headers = headers;
            this.useCachedNetworkImage = useCachedNetworkImage;
        }

        public override State createState() {
            return new _PhotoViewState();
        }
    }

    class _PhotoViewState : State<PhotoView>, RouteAware {
        int currentIndex;

        public override void initState() {
            base.initState();
            this.currentIndex = this.widget.index;
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            var headers = this.widget.headers ?? new Dictionary<string, string> {
                {HttpManager.COOKIE, HttpManager.getCookie()},
                {"ConnectAppVersion", Config.versionNumber},
                {"X-Requested-With", "XmlHttpRequest"}
            };
            var pageView = new PageView(
                controller: this.widget.controller,
                onPageChanged: index => { this.setState(() => { this.currentIndex = index; }); },
                children: this.widget.urls.Select<string, Widget>(url => {
                    return this.widget.useCachedNetworkImage
                        ? (Widget)new CachedNetworkImage(
                            url,
                            new CustomActivityIndicator(loadingColor: LoadingColor.white),
                            fit: BoxFit.contain,
                            headers: headers)
                        : Image.network(url,
                            fit: BoxFit.contain,
                            headers: headers);
                }).ToList());
            return new GestureDetector(
                onTap: () => { StoreProvider.store.dispatcher.dispatch(new MainNavigatorPopAction()); },
                onLongPress: this._pickImage,
                child: new Container(
                    color: CColors.Black,
                    child: new Stack(
                        alignment: Alignment.center,
                        children: new List<Widget> {
                            pageView,
                            new Positioned(
                                bottom: 30,
                                child: new Container(
                                    height: 40,
                                    padding: EdgeInsets.symmetric(0, 24),
                                    alignment: Alignment.center,
                                    decoration: new BoxDecoration(
                                        color: Color.fromRGBO(0, 0, 0, 0.5f),
                                        borderRadius: BorderRadius.all(20)
                                    ),
                                    child: new Text($"{this.currentIndex + 1}/{this.widget.urls.Count}",
                                        style: CTextStyle.PLargeWhite.copyWith(height: 1))
                                )
                            )
                        })));
        }

        void _pickImage() {
            var imageUrl = this.widget.urls[this.currentIndex];
            var imagePath = SQLiteDBManager.instance.GetCachedFilePath(imageUrl);
            if (imagePath.isEmpty()) {
                return;
            }

            var items = new List<ActionSheetItem> {
                new ActionSheetItem(
                    "保存图片",
                    onTap: () => {
                        if (imagePath.isNotEmpty()) {
                            var imageStr = CImageUtils.readImage(imagePath);
                            PickImagePlugin.SaveImage(imagePath, imageStr);
                        }
                    }
                ),
                new ActionSheetItem("取消", type: ActionType.cancel)
            };

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                title: "",
                items: items
            ));
        }

        public void didPopNext() {
        }

        public void didPush() {
            StatusBarManager.hideStatusBar(true);
        }

        public void didPop() {
            StatusBarManager.hideStatusBar(false);
        }

        public void didPushNext() {
        }
    }
}