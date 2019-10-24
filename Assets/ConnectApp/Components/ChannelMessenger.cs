using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
    public class ImageMessage : StatefulWidget {
        public ImageMessage(
            string url,
            float size,
            float ratio,
            float srcWidth = 0,
            float srcHeight = 0,
            Dictionary<string, string> headers = null,
            float radius = 10,
            Key key = null
        ) : base(key: key) {
            this.url = url;
            this.size = size;
            this.ratio = ratio;
            this.radius = radius;
            this.headers = headers;
            this.srcWidth = srcWidth;
            this.srcHeight = srcHeight;
        }

        public readonly string url;
        public readonly float size;
        public readonly float ratio;
        public readonly float radius;
        public readonly float srcWidth;
        public readonly float srcHeight;
        public readonly Dictionary<string, string> headers;

        public Size srcSize {
            get {
                if (this.srcWidth != 0 && this.srcHeight != 0) {
                    return new Size(width: this.srcWidth, height: this.srcHeight);
                }

                return null;
            }
        }

        public override State createState() {
            return new _ImageMessageState();
        }
    }

    class _ImageMessageState : State<ImageMessage> {
        Image image;
        Size size;
        ImageStream stream;

        Size _finalSize(Size size) {
            if (size.width > size.height * this.widget.ratio) {
                return new Size(
                    width: this.widget.size,
                    height: this.widget.size / this.widget.ratio);
            }

            if (size.width > size.height) {
                return new Size(
                    width: this.widget.size,
                    height: this.widget.size / size.width * size.height);
            }

            if (size.width > size.height / this.widget.ratio) {
                return new Size(
                    width: this.widget.size / size.height * size.width,
                    height: this.widget.size);
            }

            return new Size(
                width: this.widget.size / this.widget.ratio,
                height: this.widget.size);
        }

        void _updateSize(ImageInfo info, bool _) {
            this.size = this._finalSize(new Size(info.image.width, info.image.height));

            this.setState(() => { });
        }

        static Image _getImage(string content, Dictionary<string, string> headers = null) {
            return content.StartsWith("http")
                ? CachedNetworkImageProvider.cachedNetworkImage(
                    src: CImageUtils.SizeToScreenImageUrl(content),
                    headers: headers)
                : Image.memory(Convert.FromBase64String(s: content));
        }

        public override void initState() {
            base.initState();
            if (this.widget.srcSize == null) {
                this.image = _getImage(this.widget.url, this.widget.headers);
                this.stream = this.image.image
                    .resolve(new ImageConfiguration());
                this.stream.addListener(this._updateSize);
            }
            else {
                this.size = this._finalSize(this.widget.srcSize);
            }
        }

        public override void dispose() {
            this.stream?.removeListener(this._updateSize);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            return this.size == null || this.widget.url == null
                ? new Container(
                    width: this.widget.size,
                    height: this.widget.size,
                    decoration: new BoxDecoration(
                        color: CColors.Disable,
                        borderRadius: BorderRadius.all(this.widget.radius)
                    ))
                : (Widget) new ClipRRect(
                    borderRadius: BorderRadius.all(this.widget.radius),
                    child: new Container(
                        width: this.size.width,
                        height: this.size.height,
                        color: CColors.Disable,
                        child: _getImage(this.widget.url, this.widget.headers))
                );
        }
    }
}