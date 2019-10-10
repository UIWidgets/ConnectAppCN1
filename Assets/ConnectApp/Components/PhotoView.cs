using System.Collections.Generic;
using System.Linq;
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

        public PhotoView(
            List<string> urls = null,
            PageController controller = null,
            Dictionary<string, string> headers = null,
            bool useCachedNetworkImage = false,
            Key key = null) : base(key: key) {
            D.assert(urls != null);
            D.assert(urls.isNotEmpty());
            this.urls = urls;
            this.controller = controller;
            this.headers = headers;
            this.useCachedNetworkImage = useCachedNetworkImage;
        }

        public override State createState() {
            return new _PhotoViewState();
        }
    }

    class _PhotoViewState : State<PhotoView> {
        public override Widget build(BuildContext context) {
            return new Container(
                color: Colors.black,
                child: new PageView(
                    controller: this.widget.controller,
                    children: this.widget.urls.Select<string, Widget>(url => {
                        return this.widget.useCachedNetworkImage
                            ? CachedNetworkImageProvider.cachedNetworkImage(
                                url,
                                fit: BoxFit.contain,
                                headers: this.widget.headers)
                            : Image.network(url,
                                fit: BoxFit.contain,
                                headers: this.widget.headers);
                    }).ToList()));
        }
    }
}