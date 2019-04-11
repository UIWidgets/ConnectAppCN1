using System;
using System.Collections;
using System.Collections.Generic;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;
using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.ui.Image;
using ImageUtils = Unity.UIWidgets.widgets.ImageUtils;

namespace ConnectApp.components {
    public enum ImagePhase {
        start,
        waiting,
        fadeOut,
        fadeIn,
        completed
    }

    internal delegate void _ImageProviderResolverListener();

    public delegate void ErrorListener();

    public class CachedNetworkImage : StatefulWidget {
        public static List<object> _registeredErrors = new List<object>();

        public CachedNetworkImage(
            string imageUrl,
            Widget placeholder = null,
            Widget errorWidget = null,
            TimeSpan fadeOutDuration = new TimeSpan(),
            Curve fadeOutCurve = null,
            TimeSpan fadeInDuration = new TimeSpan(),
            Curve fadeInCurve = null,
            float? width = null,
            float? height = null,
            BoxFit? fit = null,
            Alignment alignment = null,
            ImageRepeat repeat = ImageRepeat.noRepeat,
            Dictionary<string, string> httpHeaders = null,
            Key key = null
        ) : base(key) {
            D.assert(imageUrl != null);
            this.imageUrl = imageUrl;
            this.placeholder = placeholder;
            this.errorWidget = errorWidget;
            this.fadeOutDuration = fadeOutDuration;
            this.fadeOutCurve = fadeOutCurve ?? Curves.easeOut;
            this.fadeInDuration = fadeInDuration;
            this.fadeInCurve = fadeInCurve ?? Curves.easeIn;
            this.width = width;
            this.height = height;
            this.fit = fit;
            this.alignment = alignment ?? Alignment.center;
            this.repeat = repeat;
            this.httpHeaders = httpHeaders;
        }

        public readonly string imageUrl;
        public readonly Widget placeholder;
        public readonly Widget errorWidget;
        public readonly TimeSpan fadeOutDuration;
        public readonly Curve fadeOutCurve;
        public readonly TimeSpan fadeInDuration;
        public readonly Curve fadeInCurve;
        public readonly float? width;
        public readonly float? height;
        public readonly BoxFit? fit;
        public readonly Alignment alignment;
        public readonly ImageRepeat repeat;
        public readonly Dictionary<string, string> httpHeaders;

        public override State createState() {
            return new _CachedNetworkImageState();
        }
    }

    internal class _CachedNetworkImageState : State<CachedNetworkImage>, TickerProvider {
        private _ImageProviderResolver _imageResolver;
        private CachedNetworkImageProvider _imageProvider;

        private AnimationController _controller;
        private Animation<float> _animation;

        private ImagePhase _phase = ImagePhase.start;
        public ImagePhase phase => _phase;

        private bool _hasError;

        public override void initState() {
            _hasError = false;
            _imageProvider = new CachedNetworkImageProvider(
                widget.imageUrl,
                headers: widget.httpHeaders,
                errorListener: _imageLoadingFailed
            );
            _imageResolver = new _ImageProviderResolver(
                this,
                _updatePhase
            );

            _controller = new AnimationController(
                1,
                vsync: this
            );
            _controller.addListener(() => {
                setState(() => {
                    // Trigger rebuild to update opacity value.
                });
            });
            _controller.addStatusListener(status => { _updatePhase(); });
            base.initState();
        }

        public override void didChangeDependencies() {
            _imageProvider.obtainCachedNetworkImageKey(ImageUtils.createLocalImageConfiguration(context))
                .Then(key => {
                    if (CachedNetworkImage._registeredErrors.Contains(key)) setState(() => _hasError = true);
                });

            _resolveImage();
            base.didChangeDependencies();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            if (oldWidget is CachedNetworkImage newWidget)
                if (widget.imageUrl != newWidget.imageUrl ||
                    widget.placeholder != newWidget.placeholder) {
                    _imageProvider = new CachedNetworkImageProvider(widget.imageUrl,
                        errorListener: _imageLoadingFailed);
                    _resolveImage();
                }
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick, $"created by {this}");
        }

        public override void dispose() {
            _imageResolver.stopListening();
            _controller.dispose();
            base.dispose();
        }

        private void _resolveImage() {
            _imageResolver.resolve(_imageProvider);
            if (_phase == ImagePhase.start) _updatePhase();
        }

        private void _updatePhase() {
            setState(() => {
                switch (_phase) {
                    case ImagePhase.start:
                        if (_imageResolver._imageInfo != null || _hasError)
                            _phase = ImagePhase.completed;
                        else
                            _phase = ImagePhase.waiting;
                        break;
                    case ImagePhase.waiting:
                        if (_hasError && widget.errorWidget == null) {
                            _phase = ImagePhase.completed;
                            return;
                        }

                        if (_imageResolver._imageInfo != null || _hasError) {
                            if (widget.placeholder == null)
                                _startFadeIn();
                            else
                                _startFadeOut();
                        }

                        break;
                    case ImagePhase.fadeOut:
                        if (_controller.status == AnimationStatus.dismissed) _startFadeIn();
                        break;
                    case ImagePhase.fadeIn:
                        if (_controller.status == AnimationStatus.completed) _phase = ImagePhase.completed;
                        break;
                    case ImagePhase.completed:
                        // Nothing to do.
                        break;
                }
            });
        }

        private void _startFadeOut() {
            _controller.duration = widget.fadeOutDuration;
            _animation = new CurvedAnimation(
                _controller,
                widget.fadeOutCurve
            );
            _phase = ImagePhase.fadeOut;
            _controller.reverse(1);
        }

        private void _startFadeIn() {
            _controller.duration = widget.fadeInDuration;
            _animation = new CurvedAnimation(
                _controller,
                widget.fadeInCurve
            );
            _phase = ImagePhase.fadeIn;
            _controller.forward(0);
        }

        private bool _isShowingPlaceholder {
            get {
                D.assert(_phase != null);
                switch (_phase) {
                    case ImagePhase.start:
                    case ImagePhase.waiting:
                    case ImagePhase.fadeOut:
                        return true;
                    case ImagePhase.fadeIn:
                    case ImagePhase.completed:
                        return _hasError && widget.errorWidget == null;
                }

                return false;
            }
        }

        private void _imageLoadingFailed() {
            _imageProvider.obtainCachedNetworkImageKey(ImageUtils.createLocalImageConfiguration(context))
                .Then(key => {
                    if (!CachedNetworkImage._registeredErrors.Contains(key))
                        CachedNetworkImage._registeredErrors.Add(key);
                });
            _hasError = true;
            _updatePhase();
        }

        public override Widget build(BuildContext context) {
            D.assert(_phase != ImagePhase.start);
            if (_isShowingPlaceholder && widget.placeholder != null) return _buildFadedWidget(widget.placeholder);

            if (_hasError && widget.errorWidget != null) return _buildFadedWidget(widget.errorWidget);

            var imageInfo = _imageResolver._imageInfo;
            return new RawImage(
                image: imageInfo?.image,
                width: widget.width,
                height: widget.height,
                scale: imageInfo?.scale ?? 1,
                color: Color.fromRGBO(255, 255, 255, _animation?.value ?? 1),
                colorBlendMode: BlendMode.modulate,
                fit: widget.fit,
                alignment: widget.alignment,
                repeat: widget.repeat
            );
        }

        private Widget _buildFadedWidget(Widget w) {
            return new Opacity(
                _animation?.value ?? 1,
                child: w
            );
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder description) {
            base.debugFillProperties(description);
            description.add(new EnumProperty<ImagePhase>("phase", _phase));
            description.add(new DiagnosticsProperty<ImageInfo>(
                "pixels", _imageResolver._imageInfo));
            description.add(new DiagnosticsProperty<ImageStream>(
                "image stream", _imageResolver._imageStream));
        }
    }

    internal class _ImageProviderResolver {
        public _ImageProviderResolver(
            _CachedNetworkImageState state,
            _ImageProviderResolverListener listener
        ) {
            this.state = state;
            this.listener = listener;
        }

        private readonly _CachedNetworkImageState state;
        private readonly _ImageProviderResolverListener listener;

        public ImageStream _imageStream;
        public ImageInfo _imageInfo;

        public CachedNetworkImage widget => state.widget;

        public void resolve(CachedNetworkImageProvider provider) {
            var oldImageStream = _imageStream;
            _imageStream = provider.resolve(ImageUtils.createLocalImageConfiguration(
                state.context,
                widget.width != null && widget.height != null
                    ? new Size((float) widget.width, (float) widget.height)
                    : null)
            );
            if (_imageStream.key != oldImageStream?.key) {
                oldImageStream?.removeListener(_handleImageChanged);
                _imageStream.addListener(_handleImageChanged);
            }
        }

        private void _handleImageChanged(ImageInfo imageInfo, bool synchronousCall) {
            _imageInfo = imageInfo;
            listener();
        }

        public void stopListening() {
            _imageStream?.removeListener(_handleImageChanged);
        }
    }

    public class CachedNetworkImageProvider : ImageProvider<CachedNetworkImageProvider> {
        public CachedNetworkImageProvider(
            string url,
            float scale = 1,
            ErrorListener errorListener = null,
            Dictionary<string, string> headers = null
        ) {
            this.url = url;
            this.scale = scale;
            this.errorListener = errorListener;
            this.headers = headers;
        }

        private readonly string url;
        private readonly float scale;
        private readonly ErrorListener errorListener;
        private readonly Dictionary<string, string> headers;

        protected override IPromise<CachedNetworkImageProvider> obtainKey(ImageConfiguration configuration) {
            return Promise<CachedNetworkImageProvider>.Resolved(this);
        }

        protected override ImageStreamCompleter load(CachedNetworkImageProvider key) {
            return new MultiFrameImageStreamCompleter(
                _loadAsync(key),
                key.scale,
                information => {
                    information.AppendLine($"Image provider: {this}");
                    information.Append($"Image key: {key}");
                }
            );
        }

        public IPromise<CachedNetworkImageProvider> obtainCachedNetworkImageKey(ImageConfiguration configuration) {
            return obtainKey(configuration);
        }

        private IPromise<Codec> _loadAsync(CachedNetworkImageProvider key) {
            var coroutine = Window.instance.startCoroutine(_loadBytes(key));
            return coroutine.promise.Then(obj => {
                if (obj is byte[] bytes) return CodecUtils.getCodec(bytes);
                return CodecUtils.getCodec(new Image((Texture2D) obj));
            });
        }

        private IEnumerator _loadBytes(CachedNetworkImageProvider key) {
            D.assert(key == this);
            var uri = new Uri(key.url);

            if (uri.LocalPath.EndsWith(".gif")) {
                using (var www = UnityWebRequest.Get(uri)) {
                    if (headers != null)
                        foreach (var header in headers)
                            www.SetRequestHeader(header.Key, header.Value);

                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError) {
                        if (errorListener != null) errorListener();
                        throw new Exception($"Failed to load from url \"{uri}\": {www.error}");
                    }

                    var data = www.downloadHandler.data;
                    yield return data;
                }

                yield break;
            }

            using (var www = UnityWebRequestTexture.GetTexture(uri)) {
                if (headers != null)
                    foreach (var header in headers)
                        www.SetRequestHeader(header.Key, header.Value);

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                    throw new Exception($"Failed to load from url \"{uri}\": {www.error}");

                var data = ((DownloadHandlerTexture) www.downloadHandler).texture;
                yield return data;
            }
        }

        public override string ToString() {
            return $"{GetType()}(\"{url}\", scale: {scale})";
        }
    }
}