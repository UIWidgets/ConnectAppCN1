using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.components {
    public class PlaceholderImage : StatelessWidget {
        public PlaceholderImage(
            string imageUrl,
            float? width = null,
            float? height = null,
            float? borderRadius = null,
            BoxFit? fit = null,
            Key key = null
        ) : base(key) {
            D.assert(imageUrl != null);
            D.assert(borderRadius == null || borderRadius >= 0);
            this.imageUrl = imageUrl;
            this.width = width;
            this.height = height;
            this.borderRadius = borderRadius;
            this.fit = fit;
        }

        readonly string imageUrl;
        readonly float? width;
        readonly float? height;
        readonly float? borderRadius;
        readonly BoxFit? fit;

        public override Widget build(BuildContext context) {
            Widget child;
            if (this.imageUrl == null || this.imageUrl.Length <= 0) {
                child = new Container(
                    width: this.width,
                    height: this.height,
                    color: new Color(0xFFD8D8D8)
                );
            }
            else {
                child = new Container(
                    width: this.width,
                    height: this.height,
                    color: new Color(0xFFD8D8D8),
                    child: Image.network(this.imageUrl,
                        width: this.width,
                        height: this.height,
                        fit: this.fit
                    )
                );
            }

            return new ClipRRect(
                borderRadius: BorderRadius.all(this.borderRadius == null ? 0 : (float) this.borderRadius),
                child: child
            );
        }
    }
}