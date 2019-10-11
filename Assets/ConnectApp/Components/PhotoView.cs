using System.Collections.Generic;
using System.Linq;
using ConnectApp.Constants;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

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

    class _PhotoViewState : State<PhotoView> {
        int currentIndex;

        public override void initState() {
            base.initState();
            this.currentIndex = this.widget.index;
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
                        ? CachedNetworkImageProvider.cachedNetworkImage(
                            url,
                            fit: BoxFit.contain,
                            headers: headers)
                        : Image.network(url,
                            fit: BoxFit.contain,
                            headers: headers);
                }).ToList());
            return new GestureDetector(
                onTap: () => { StoreProvider.store.dispatcher.dispatch(new MainNavigatorPopAction()); },
                child: new Container(
                    color: Colors.black,
                    child: new Stack(
                        children: new List<Widget> {
                            pageView,
                            new Positioned(
                                bottom: 30,
                                left: 0,
                                right: 0,
                                child: new Center(
                                    child: new Text($"{this.currentIndex + 1}/{this.widget.urls.Count}",
                                        style: CTextStyle.H4White)))
                        })));
        }
    }
}