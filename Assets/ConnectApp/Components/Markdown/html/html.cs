using HtmlAgilityPack;
using SyntaxHighlight;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace html {
    public class HtmlView : StatelessWidget {
        public HtmlView(
            Key key = null,
            string data = null,
            EdgeInsets padding = null,
            Color backgroundColor = null,
            TextStyle defaultTextStyle = null,
            OnLinkTap onLinkTap = null,
            bool renderNewlines = false,
            CustomRender customRender = null,
            CustomEdgeInsets customEdgeInsets = null,
            CustomTextStyle customTextStyle = null,
            CustomTextAlign customTextAlign = null,
            float blockSpacing = 14.0f,
            bool useRichText = true,
            ImageErrorListener onImageError = null,
            TextStyle linkStyle = null,
            bool shrinkToFit = false,
            ImageProperties imageProperties = null,
            OnImageTap onImageTap = null,
            bool showImages = true
        ) : base(key: key) {
            D.assert(data != null);
            this.data = data;
            this.padding = padding;
            this.backgroundColor = backgroundColor;
            this.defaultTextStyle = defaultTextStyle;
            this.onLinkTap = onLinkTap;
            this.renderNewlines = renderNewlines;
            this.customRender = customRender;
            this.customEdgeInsets = customEdgeInsets;
            this.customTextStyle = customTextStyle ?? this._customTextStyle;
            this.customTextAlign = customTextAlign;
            this.blockSpacing = blockSpacing;
            this.useRichText = useRichText;
            this.onImageError = onImageError;
            this.linkStyle = linkStyle ?? new TextStyle(
                decoration: TextDecoration.underline,
                color: Colors.blueAccent,
                decorationColor: Colors.blueAccent);
            this.shrinkToFit = shrinkToFit;
            this.imageProperties = imageProperties;
            this.onImageTap = onImageTap;
            this.showImages = showImages;
        }

        public readonly string data;
        public readonly EdgeInsets padding;
        public readonly Color backgroundColor;
        public readonly TextStyle defaultTextStyle;
        public readonly OnLinkTap onLinkTap;
        public readonly bool renderNewlines;
        public readonly float blockSpacing;
        public readonly bool useRichText;
        public readonly ImageErrorListener onImageError;
        public readonly TextStyle linkStyle;
        public readonly bool shrinkToFit;

        /// Properties for the Image widget that gets rendered by the rich text parser
        public readonly ImageProperties imageProperties;

        public readonly OnImageTap onImageTap;
        public readonly bool showImages;

        /// Either return a custom widget for specific node types or return null to
        /// fallback to the default rendering.
        public readonly CustomRender customRender;

        public readonly CustomEdgeInsets customEdgeInsets;
        public readonly CustomTextStyle customTextStyle;
        public readonly CustomTextAlign customTextAlign;

        TextStyle _customTextStyle(HtmlNode node, TextStyle baseStyle) {
            var style = new TextStyle();
            if (node.Attributes["color"] != null) {
                style = style.copyWith(color: new Color(ColorsFromName.ColorFromName(node.Attributes["color"].Value)));
            }

            return baseStyle.merge(style);
        }


        public override Widget build(BuildContext context) {
            float? width = this.shrinkToFit ? (float?) null : MediaQuery.of(context).size.width;

            return new Container(
                padding: this.padding,
                color: this.backgroundColor,
                width: width,
                child: DefaultTextStyle.merge(
                    style: this.defaultTextStyle ?? DefaultTextStyle.of(context).style,
                    child: (this.useRichText)
                        ? (Widget) new HtmlRichTextParser(
                            shrinkToFit: this.shrinkToFit,
                            onLinkTap: this.onLinkTap,
                            renderNewlines: this.renderNewlines,
                            customEdgeInsets: this.customEdgeInsets,
                            customTextStyle: this.customTextStyle,
                            customTextAlign: this.customTextAlign,
                            html: this.data,
                            onImageError: this.onImageError,
                            linkStyle: this.linkStyle,
                            imageProperties: this.imageProperties,
                            onImageTap: this.onImageTap,
                            showImages: this.showImages
                        )
                        : new HtmlOldParser(
                            width: width,
                            onLinkTap: this.onLinkTap,
                            renderNewlines: this.renderNewlines,
                            customRender: this.customRender,
                            html: this.data,
                            blockSpacing: this.blockSpacing,
                            onImageError: this.onImageError,
                            linkStyle: this.linkStyle,
                            showImages: this.showImages
                        )
                )
            );
        }
    }
}