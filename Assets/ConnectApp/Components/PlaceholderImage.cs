using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
    public class PlaceholderImage : StatelessWidget {
        public PlaceholderImage(
            string imageUrl,
            float? width = null,
            float? height = null,
            float? borderRadius = null,
            BoxFit? fit = null,
            bool useCachedNetworkImage = false,
            Color color = null,
            Key key = null
        ) : base(key: key) {
            D.assert(imageUrl != null);
            D.assert(borderRadius == null || borderRadius >= 0);
            this.imageUrl = imageUrl;
            this.width = width;
            this.height = height;
            this.borderRadius = borderRadius;
            this.fit = fit;
            this.useCachedNetworkImage = useCachedNetworkImage;
            this.color = color ?? CColors.LoadingGrey;
        }

        readonly string imageUrl;
        readonly float? width;
        readonly float? height;
        readonly float? borderRadius;
        readonly BoxFit? fit;
        readonly bool useCachedNetworkImage;
        readonly Color color;

        public override Widget build(BuildContext context) {
            Widget child;
            if (this.imageUrl.isEmpty()) {
                child = new Container(
                    width: this.width,
                    height: this.height,
                    color: this.color
                );
            }
            else {
                child = new Container(
                    width: this.width,
                    height: this.height,
                    color: this.color,
                    child: !this.useCachedNetworkImage
                        ? Image.network(
                            src: this.imageUrl,
                            width: this.width,
                            height: this.height,
                            fit: this.fit
                        ) : (Widget) new CachedNetworkImage(
                            src: this.imageUrl,
                            fit: this.fit
                        )
                );
            }

            return new ClipRRect(
                borderRadius: BorderRadius.all(this.borderRadius ?? 0),
                child: child
            );
        }
    }
}