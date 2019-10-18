using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.widgets.Image;
using Rect = Unity.UIWidgets.ui.Rect;

namespace ConnectApp.Utils {
    public static class CachedNetworkImageProvider {
        
        /*
         * use CachedNetworkImageProvider.cachedNetworkImage to replace Image.network so that:
         * (1) the image file will be stored locally after fetching from the internet when first load
         * (2) the image will be loaded from the cached file in (1) instead of internet when the cache is available
         */
        public static Image cachedNetworkImage(
            string src,
            Key key = null,
            float scale = 1.0f,
            float? width = null,
            float? height = null,
            Color color = null,
            BlendMode colorBlendMode = BlendMode.srcIn,
            BoxFit? fit = null,
            Alignment alignment = null,
            ImageRepeat repeat = ImageRepeat.noRepeat,
            Rect centerSlice = null,
            bool gaplessPlayback = false,
            FilterMode filterMode = FilterMode.Bilinear,
            IDictionary<string, string> headers = null
        ) {
            var networkImage = new CachedNetworkImage(src, scale, headers);
            return new Image(
                key,
                networkImage,
                width,
                height,
                color,
                colorBlendMode,
                fit,
                alignment,
                repeat,
                centerSlice,
                gaplessPlayback,
                filterMode
            );
        }
    }
}