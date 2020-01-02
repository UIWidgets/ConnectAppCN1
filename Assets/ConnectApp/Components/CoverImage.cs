using System.Collections.Generic;
using ConnectApp.Constants;
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
            var coverImageWidth = MediaQuery.of(context).size.width;
            if (this.coverImage.isNotEmpty()) {
                coverImageWidget = new PlaceholderImage(
                    imageUrl: this.coverImage,
                    height: this.height,
                    width: coverImageWidth,
                    fit: BoxFit.cover
                );
                coverImageColor = Color.fromRGBO(0, 0, 0, 0.4f);
            }
            else {
                coverImageWidget = new Container(
                    color: CColors.Black,
                    child: Image.asset(
                        "image/default-background-cover",
                        height: this.height,
                        width: coverImageWidth,
                        fit: BoxFit.cover
                    )
                );
                coverImageColor = CColors.Transparent;
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

    public class FavoriteTagCoverImage : StatelessWidget {
        public FavoriteTagCoverImage(
            string coverImage,
            Color coverColor,
            float size = 48,
            float borderRadius = 4,
            EdgeInsets margin = null,
            EdgeInsets padding = null,
            Key key = null
        ) : base(key: key) {
            this.coverImage = coverImage;
            this.coverColor = coverColor;
            this.size = size;
            this.borderRadius = borderRadius;
            this.margin = margin;
            this.padding = padding ?? EdgeInsets.all(8);
        }

        readonly string coverImage;
        readonly Color coverColor;
        readonly float size;
        readonly float borderRadius;
        readonly EdgeInsets margin;
        readonly EdgeInsets padding;

        public override Widget build(BuildContext context) {
            return new ClipRRect(
                borderRadius: BorderRadius.all(radius: this.borderRadius),
                child: new Container(
                    width: this.size,
                    height: this.size,
                    margin: this.margin,
                    padding: this.padding,
                    decoration: new BoxDecoration(
                        color: this.coverColor,
                        borderRadius: BorderRadius.all(radius: this.borderRadius)
                    ),
                    child: Image.asset(
                        name: this.coverImage
                    )
                )
            );
        }
    }

    public class CoverImages : StatelessWidget {
        public CoverImages(
            float size = 48,
            float ratioGap = 16,
            float horizontalGap = 16,
            float verticalGap = 16,
            Key key = null
        ) : base(key: key) {
            this.size = size;
            this.ratioGap = ratioGap;
            this.horizontalGap = horizontalGap;
            this.verticalGap = verticalGap;
        }

        readonly float size;
        readonly float ratioGap;
        readonly float horizontalGap;
        readonly float verticalGap;

        public override Widget build(BuildContext context) {
            return new Container(
                width: this.size + this.ratioGap * 2 + this.horizontalGap * 2,
                height: this.size + this.ratioGap * 2 + this.verticalGap * 2,
                child: new Stack(
                    children: new List<Widget> {
                        new Positioned(
                            right: 0,
                            bottom: 0,
                            child: new Container(
                                width: this.size,
                                height: this.size,
                                decoration: new BoxDecoration(
                                    color: CColors.Red,
                                    borderRadius: BorderRadius.all(6)
                                )
                            )
                        ),
                        new Positioned(
                            right: this.horizontalGap,
                            bottom: this.verticalGap,
                            child: new Container(
                                width: this.size + this.ratioGap,
                                height: this.size + this.ratioGap,
                                decoration: new BoxDecoration(
                                    color: CColors.Green,
                                    borderRadius: BorderRadius.all(6)
                                )
                            )
                        ),
                        new Positioned(
                            right: this.horizontalGap + this.horizontalGap,
                            bottom: this.verticalGap + this.verticalGap,
                            child: new Container(
                                width: this.size + this.ratioGap * 2,
                                height: this.size + this.ratioGap * 2,
                                decoration: new BoxDecoration(
                                    color: CColors.PrimaryBlue,
                                    borderRadius: BorderRadius.all(6)
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}