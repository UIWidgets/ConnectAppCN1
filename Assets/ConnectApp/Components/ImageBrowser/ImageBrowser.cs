using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Main;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.ImageBrowser {
    public class ImageBrowser : StatefulWidget {
        public ImageBrowser(
            string image = null,
            List<string> images = null,
            Key key = null
        ) : base(key) {
            this.image = image ?? "https://picsum.photos/250?image=2";
            this.images = images;
        }

        public readonly string image;
        public readonly List<string> images;

        public override State createState() {
            return new _ImageBrowserState();
        }
    }

    class _ImageBrowserState : State<ImageBrowser> {
        int _pageIndex;
        PageController _pageController;

        public override void initState() {
            base.initState();
            this._pageIndex = 1;
            this._pageController = new PageController(initialPage: this._pageIndex);
        }

        public override Widget build(BuildContext context) {
            return new GestureDetector(
                child: new Container(
                    color: CColors.Black,
                    child: new PageView(
                        physics: new BouncingScrollPhysics(),
                        controller: this._pageController,
                        onPageChanged: this._onPageChanged,
                        children: new List<Widget> {
                            new ImagePage("https://picsum.photos/250?image=1"),
                            new ImagePage("https://picsum.photos/250?image=2"),
                            new ImagePage("https://picsum.photos/250?image=3"),
                            new ImagePage("https://picsum.photos/250?image=4"),
                            new ImagePage("https://picsum.photos/250?image=5"),
                            new ImagePage("https://picsum.photos/250?image=6"),
                            new ImagePage("https://picsum.photos/250?image=7")
                        }
                    )
                ),
                onTap: () => {
                    if (Router.navigator.canPop()) {
                        Router.navigator.pop();
                    }
                });
        }

        void _onPageChanged(int page) {
            this._pageController.jumpToPage(page);
        }
    }


    public class ImagePage : StatelessWidget {
        public ImagePage(
            string url,
            Key key = null
        ) : base(key) {
            this.url = url;
        }


        public readonly string url;

        public override Widget build(BuildContext context) {
            return new Container(
                child: new ScalableImage(
                    new NetworkImage(this.url),
                    wrapInAspect: true
                )
            );
        }

        void showActionSheet() {
            var items = new List<ActionSheetItem> {
                new ActionSheetItem(
                    "发送给朋友",
                    ActionType.normal,
                    () => { }
                ),
                new ActionSheetItem(
                    "收藏",
                    ActionType.normal,
                    () => { }
                ),
                new ActionSheetItem(
                    "保存图片",
                    ActionType.normal,
                    () => { }
                ),
                new ActionSheetItem("取消", ActionType.cancel)
            };

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                items: items
            ));
        }
    }
}