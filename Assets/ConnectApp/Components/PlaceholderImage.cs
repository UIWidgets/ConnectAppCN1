using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using Unity.UIWidgets.ui;

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

        private readonly string imageUrl;
        private readonly float? width;
        private readonly float? height;
        private readonly float? borderRadius;
        private readonly BoxFit? fit;
        
        public override Widget build(BuildContext context) {
            Widget child;
            if (imageUrl == null || imageUrl.Length <= 0) 
                child = new Container(
                    width: width,
                    height: height,
                    color: new Color(0xFFD8D8D8)
                );
            else 
                child = new CachedNetworkImage(
                    imageUrl,
                    new Container(
                        color: new Color(0xFFD8D8D8)
                    ),
                    new Container(
                        color: new Color(0xFFD8D8D8)
                    ),
                    width: width,
                    height: height,
                    fit: fit
                );
            
            return new ClipRRect(
                borderRadius: BorderRadius.all(borderRadius == null ? 0 : (float) borderRadius),
                child: child
            );
        }
    }
}