using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
    public class CoverImage : StatelessWidget {
        public CoverImage(
            string coverImage,
            float height,
            Widget child,
            Key key = null
        ) : base(key: key) {
            this.coverImage = coverImage;
            this.height = height;
            this.child = child;
        }

        readonly string coverImage;
        readonly float height;
        readonly Widget child;

        public override Widget build(BuildContext context) {
            Widget coverImageWidget;
            Color coverImageColor;
            if (this.coverImage != null && this.coverImage.isNotEmpty()) {
                coverImageWidget = new PlaceholderImage(
                    imageUrl: this.coverImage,
                    height: this.height,
                    fit: BoxFit.cover
                );
                coverImageColor = Color.fromRGBO(0, 0, 0, 0.4f);
            }
            else {
                coverImageWidget = Image.asset(
                    "image/default-cover-image",
                    height: this.height,
                    fit: BoxFit.cover
                );
                coverImageColor = Color.fromRGBO(0, 0, 0, 0.2f);
            }
            return new Stack(
                children: new List<Widget> {
                    coverImageWidget,
                    Positioned.fill(
                        new Container(
                            color: coverImageColor,
                            child: this.child
                        )
                    )
                }
            );
        }
    }
}