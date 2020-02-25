using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using Rect = Unity.UIWidgets.ui.Rect;

namespace html {
    public class ImageProperties {
        public readonly string semanticLabel;
        public readonly float width;
        public readonly float height;
        public readonly Color color;
        public readonly BlendMode colorBlendMode;
        public readonly BoxFit fit;
        public readonly Alignment alignment;
        public readonly ImageRepeat repeat;
        public readonly Rect centerSlice;
        public readonly bool matchTextDirection;
        public readonly FilterMode filterQuality;
        public readonly float scale;

        public ImageProperties(
            float scale = 1,
            string semanticLabel = null,
            float width = 0.0f,
            float height = 0.0f,
            Color color = null,
            BlendMode colorBlendMode = BlendMode.srcOver,
            BoxFit fit = BoxFit.fill,
            Alignment alignment = null,
            ImageRepeat repeat = ImageRepeat.noRepeat,
            Rect centerSlice = null,
            bool matchTextDirection = false,
            FilterMode filterQuality = FilterMode.Bilinear
        ) {
            this.scale = scale;
            this.semanticLabel = semanticLabel;
            this.width = width;
            this.height = height;
            this.color = color;
            this.colorBlendMode = colorBlendMode;
            this.fit = fit;
            this.alignment = alignment ?? Alignment.center;
            this.repeat = repeat;
            this.centerSlice = centerSlice;
            this.matchTextDirection = matchTextDirection;
            this.filterQuality = filterQuality;
        }
    }
}