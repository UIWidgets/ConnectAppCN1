using System;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.widgets.Image;
using Rect = Unity.UIWidgets.ui.Rect;

namespace ConnectApp.Components {
    public delegate void OnImageResolved(ImageInfo imageInfo);
    /*
     * use CachedNetworkImageProvider.cachedNetworkImage to replace Image.network so that:
     * (1) the image file will be stored locally after fetching from the internet when first load
     * (2) the image will be loaded from the cached file in (1) instead of internet when the cache is available
     */
    public class CachedNetworkImage : StatefulWidget {
        public CachedNetworkImage(
            string src,
            Widget placeholder = null,
            float scale = 1.0f,
            float? width = null,
            float? height = null,
            Color color = null,
            BlendMode colorBlendMode = BlendMode.modulate,
            BoxFit? fit = null,
            Alignment alignment = null,
            ImageRepeat repeat = ImageRepeat.noRepeat,
            Rect centerSlice = null,
            FilterMode filterMode = FilterMode.Bilinear,
            IDictionary<string, string> headers = null,
            OnImageResolved onImageResolved = null,
            Key key = null
        ) : base(key: key) {
            this.src = src;
            this.placeholder = placeholder;
            this.scale = scale;
            this.width = width;
            this.height = height;
            this.color = color;
            this.colorBlendMode = colorBlendMode;
            this.fit = fit;
            this.alignment = alignment;
            this.repeat = repeat;
            this.centerSlice = centerSlice;
            this.filterMode = filterMode;
            this.headers = headers;
            this.onImageResolved = onImageResolved;
        }

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
            var imageProvider = new CachedNetworkImageProvider(url: src, scale: scale, headers: headers);
            return new Image(
                key: key,
                image: imageProvider,
                width: width,
                height: height,
                color: color,
                colorBlendMode: colorBlendMode,
                fit: fit,
                alignment: alignment,
                repeat: repeat,
                centerSlice: centerSlice,
                gaplessPlayback: gaplessPlayback,
                filterMode: filterMode
            );
        }

        public readonly string src;
        public readonly Widget placeholder;
        public readonly float scale;
        public readonly float? width;
        public readonly float? height;
        public readonly Color color;
        public readonly BlendMode colorBlendMode;
        public readonly BoxFit? fit;
        public readonly Alignment alignment;
        public readonly ImageRepeat repeat;
        public readonly Rect centerSlice;
        public readonly FilterMode filterMode;
        public readonly IDictionary<string, string> headers;
        public readonly OnImageResolved onImageResolved;

        public override State createState() {
            return new _CachedNetworkImageState();
        }
    }

    class _CachedNetworkImageState : TickerProviderStateMixin<CachedNetworkImage> {
        _CachedImageProviderResolver _imageResolver;
        CachedNetworkImageProvider _imageProvider;

        AnimationController _controller;
        Animation<float> _animation;

        CachedImagePhase _phase = CachedImagePhase.start;

        public override void initState() {
            this._imageProvider = new CachedNetworkImageProvider(
                url: this.widget.src,
                scale: this.widget.scale,
                headers: this.widget.headers
            );
            this._imageResolver = new _CachedImageProviderResolver(this, listener: this._updatePhase);
            this._controller = new AnimationController(
                1.0f,
                vsync: this
            );
            this._controller.addListener(() => this.setState(() => { }));
            this._controller.addStatusListener(status => this._updatePhase());
            base.initState();
        }

        public override void didChangeDependencies() {
            this._resolveImage();
            base.didChangeDependencies();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget: oldWidget);
            CachedNetworkImage cachedImage = oldWidget as CachedNetworkImage;
            if (this.widget.src != cachedImage.src || this.widget.placeholder != cachedImage.placeholder) {
                this._imageProvider = new CachedNetworkImageProvider(url: this.widget.src,
                    scale: this.widget.scale, headers: this.widget.headers);

                this._resolveImage();
            }
        }

        void _resolveImage() {
            this._imageResolver.resolve(provider: this._imageProvider);

            if (this._phase == CachedImagePhase.start) {
                this._updatePhase();
            }
        }

        void _updatePhase() {
            this.setState(() => {
                switch (this._phase) {
                    case CachedImagePhase.start:
                        this._phase = this._imageResolver._imageInfo != null
                            ? CachedImagePhase.completed
                            : CachedImagePhase.waiting;
                        break;
                    case CachedImagePhase.waiting:
                        if (this._imageResolver._imageInfo != null) {
                            if (this.widget.placeholder == null) {
                                this._startFadeIn();
                            }
                            else {
                                this._startFadeOut();
                            }
                        }

                        break;
                    case CachedImagePhase.fadeOut:
                        if (this._controller.status == AnimationStatus.dismissed) {
                            this._startFadeIn();
                        }

                        break;
                    case CachedImagePhase.fadeIn:
                        if (this._controller.status == AnimationStatus.completed) {
                            this._phase = CachedImagePhase.completed;
                        }

                        break;
                    case CachedImagePhase.completed:
                        break;
                }

                if (this._imageResolver._imageInfo != null) {
                    this.widget.onImageResolved?.Invoke(imageInfo: this._imageResolver._imageInfo);
                }
            });
        }

        void _startFadeOut() {
            this._controller.duration = TimeSpan.FromMilliseconds(300);
            this._animation = new CurvedAnimation(
                parent: this._controller,
                curve: Curves.easeOut
            );
            this._phase = CachedImagePhase.fadeOut;
            this._controller.reverse(1.0f);
        }

        void _startFadeIn() {
            this._controller.duration = TimeSpan.FromMilliseconds(700);
            this._animation = new CurvedAnimation(
                parent: this._controller,
                curve: Curves.easeIn
            );
            this._phase = CachedImagePhase.fadeIn;
            this._controller.forward(0.0f);
        }

        public override void dispose() {
            this._imageResolver.stopListening();
            this._controller.dispose();
            base.dispose();
        }

        bool _isShowingPlaceholder {
            get {
                switch (this._phase) {
                    case CachedImagePhase.start:
                    case CachedImagePhase.waiting:
                    case CachedImagePhase.fadeOut:
                        return true;
                    case CachedImagePhase.fadeIn:
                    case CachedImagePhase.completed:
                        return false;
                    default:
                        return false;
                }
            }
        }

        public override Widget build(BuildContext context) {
            D.assert(this._phase != CachedImagePhase.start);
            if (this._isShowingPlaceholder && this.widget.placeholder != null) {
                return new Opacity(
                    this._animation?.value ?? 1.0f,
                    child: this.widget.placeholder
                );
            }

            ImageInfo imageInfo = this._imageResolver._imageInfo;
            return new RawImage(
                image: imageInfo?.image,
                width: this.widget.width,
                height: this.widget.height,
                color: this.widget.color,
                colorBlendMode: this.widget.colorBlendMode,
                fit: this.widget.fit,
                alignment: this.widget.alignment,
                repeat: this.widget.repeat,
                centerSlice: this.widget.centerSlice,
                filterMode: this.widget.filterMode
            );
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties: properties);
            properties.add(new EnumProperty<CachedImagePhase>("phase", value: this._phase));
            properties.add(new DiagnosticsProperty<ImageInfo>("pixels", value: this._imageResolver._imageInfo));
            properties.add(
                new DiagnosticsProperty<ImageStream>("image stream", value: this._imageResolver._imageStream));
        }
    }
}