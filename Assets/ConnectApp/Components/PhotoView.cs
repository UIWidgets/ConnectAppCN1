using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Plugins;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.widgets.Image;
using ImageUtils = Unity.UIWidgets.widgets.ImageUtils;

namespace ConnectApp.Components {
    public delegate void OnScaleChangedCallback(float scale, Offset position, bool scaling);
    public delegate void OnOverScrollCallback(float offset);
    public class PhotoView : StatefulWidget {
        public readonly List<string> urls;
        public readonly PageController controller;
        public readonly Dictionary<string, string> headers;
        public readonly bool useCachedNetworkImage;
        public readonly int index;
        public readonly float maxScale;
        public readonly float minScale;

        public PhotoView(
            List<string> urls = null,
            int index = 0,
            PageController controller = null,
            Dictionary<string, string> headers = null,
            bool useCachedNetworkImage = false,
            float maxScale = 2.0f,
            float minScale = 1.0f,
            Key key = null) : base(key: key) {
            D.assert(urls != null);
            D.assert(urls.isNotEmpty());
            D.assert(minScale >= 0.0f && minScale <= 1.0f);
            D.assert(maxScale >= 1.0f);
            this.urls = urls;
            this.index = index;
            this.controller = controller ?? new PageController(initialPage: index);
            this.headers = headers;
            this.useCachedNetworkImage = useCachedNetworkImage;
            this.maxScale = maxScale;
            this.minScale = minScale;
        }

        public override State createState() {
            return new _PhotoViewState();
        }
    }

    class _PhotoViewState : TickerProviderStateMixin<PhotoView>, RouteAware {
        int currentIndex;
        readonly Dictionary<string, string> _defaultHeaders = new Dictionary<string, string> {
            {HttpManager.COOKIE, HttpManager.getCookie()},
            {"ConnectAppVersion", Config.versionName},
            {"X-Requested-With", "XmlHttpRequest"}
        };

        bool locked;

        public override void initState() {
            base.initState();
            this.currentIndex = this.widget.index;
            this.locked = false;
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        Widget _buildItem(string url) {
            return new ImageWrapper(url: url,
                headers: this.widget.headers ?? this._defaultHeaders,
                useCachedNetworkImage: this.widget.useCachedNetworkImage,
                maxScale: this.widget.maxScale,
                minScale: this.widget.minScale,
                onScaleChanged: (scale, position, scaling) => {
                    Debuger.Log($"Scale: {scale}");
                    this.setState(() => {
                        this.locked = scale > 1.01f;
                    });
                },
                onOverScroll: (offset) => {
                },
                placeholder: new CustomActivityIndicator(loadingColor: LoadingColor.white));
        }

        public override Widget build(BuildContext context) {
            var pageView = new PageView(
                controller: this.widget.controller,
                physics: this.locked ? (ScrollPhysics) new NeverScrollableScrollPhysics() : new PageScrollPhysics(), 
                onPageChanged: index => this.setState(() => this.currentIndex = index),
                itemCount: this.widget.urls.Count,
                itemBuilder: (cxt, index) => {
                    var url = this.widget.urls[index: index];
                    return this._buildItem(url: url);
                }
            );
            return new GestureDetector(
                onTap: () => StoreProvider.store.dispatcher.dispatch(new MainNavigatorPopAction()),
                onLongPress: this._pickImage,
                child: new Container(
                    color: CColors.Black,
                    child: new Stack(
                        alignment: Alignment.center,
                        children: new List<Widget> {
                            pageView,
                            new Positioned(
                                bottom: 30,
                                child: new Container(
                                    height: 40,
                                    padding: EdgeInsets.symmetric(0, 24),
                                    alignment: Alignment.center,
                                    decoration: new BoxDecoration(
                                        color: Color.fromRGBO(0, 0, 0, 0.5f),
                                        borderRadius: BorderRadius.all(20)
                                    ),
                                    child: new Text($"{this.currentIndex + 1}/{this.widget.urls.Count}",
                                        style: CTextStyle.PLargeWhite.copyWith(height: 1))
                                )
                            )
                        }
                    )
                )
            );
        }

        void _pickImage() {
            var imageUrl = this.widget.urls[index: this.currentIndex];
            var imagePath = SQLiteDBManager.instance.GetCachedFilePath(url: imageUrl);
            if (imagePath.isEmpty()) {
                return;
            }

            var items = new List<ActionSheetItem> {
                new ActionSheetItem(
                    "保存图片",
                    onTap: () => {
                        var imageStr = CImageUtils.readImage(path: imagePath);
                        PickImagePlugin.SaveImage(imagePath: imagePath, image: imageStr);
                    }
                ),
                new ActionSheetItem("取消", type: ActionType.cancel)
            };

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                items: items
            ));
        }

        public void didPopNext() {
        }

        public void didPush() {
            StatusBarManager.hideStatusBar(true);
        }

        public void didPop() {
            StatusBarManager.hideStatusBar(false);
        }

        public void didPushNext() {
        }
    }

    public class ImageWrapper : StatefulWidget {

        public readonly string url;
        public readonly bool useCachedNetworkImage;
        public readonly Dictionary<string, string> headers;
        public readonly float maxScale;
        public readonly float minScale;
        public readonly OnScaleChangedCallback onScaleChanged;
        public readonly OnOverScrollCallback onOverScroll;
        public readonly Widget placeholder;

        public ImageWrapper(
            Key key = null,
            string url = null,
            float maxScale = 2.0f,
            float minScale = 1.0f,
            bool useCachedNetworkImage = false,
            Dictionary<string, string> headers = null,
            OnScaleChangedCallback onScaleChanged = null,
            OnOverScrollCallback onOverScroll = null,
            Widget placeholder = null) : base(key: key) {
            D.assert(minScale >= 0.0f && minScale <= 1.0f);
            D.assert(maxScale >= 1.0f);
            this.url = url;
            this.useCachedNetworkImage = useCachedNetworkImage;
            this.headers = headers;
            this.maxScale = maxScale;
            this.minScale = minScale;
            this.onScaleChanged = onScaleChanged;
            this.onOverScroll = onOverScroll;
            this.placeholder = placeholder;
        }

        public override State createState() {
            return new _ImageWrapperState();
        }
    }

    class _ImageWrapperState : TickerProviderStateMixin<ImageWrapper> {
        AnimationController _scaleAnimationController;
        Animation<float> _scaleAnimation;
        float _initialScale = 1.0f;
        float _effectiveMaxScale = 2.0f;
        float _effectiveMinScale = 1.0f;
        Size _size = null;
        ImageInfo _imageInfo;
        ImageStream _imageStream;
        AnimationController _positionAnimationController;
        Animation<Offset> _positionAnimation;
        Offset _initialPosition = Offset.zero;
        bool scaling = false;

        public override void initState() {
            base.initState();
            this._scaleAnimationController = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this);
            this._scaleAnimationController.addListener(() => {
                this.setState(() => {});
                if (this.widget.onScaleChanged != null) {
                    this.widget.onScaleChanged(this._scaleAnimation.value, this._positionAnimation.value, this.scaling);
                }
            });
            this._scaleAnimation = new FloatTween(1.0f, 1.0f).animate(this._scaleAnimationController);
            this._positionAnimationController = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this);
            this._positionAnimation = new OffsetTween(Offset.zero, Offset.zero).animate(this._scaleAnimationController);
            this._positionAnimationController.addListener(
                () => {
                    this.setState(() => {});
                    if (this.widget.onScaleChanged != null) {
                        this.widget.onScaleChanged(this._scaleAnimation.value, this._positionAnimation.value, this.scaling);
                    }
                }
            );
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this._size = MediaQuery.of(this.context).size;
                this._updateEffectiveMaxScale();
            });
            if (!this.widget.useCachedNetworkImage) {
                this._imageStream = new NetworkImage(this.widget.url,
                    headers: this.widget.headers).resolve(ImageUtils.createLocalImageConfiguration(
                    this.context
                ));
                this._imageStream.addListener(this._onImageResolved);
            }

            this._effectiveMaxScale = this.widget.maxScale;
            this._effectiveMinScale = this.widget.minScale;
        }

        public override void dispose() {
            this._scaleAnimationController.dispose();
            this._positionAnimationController.dispose();
            this._imageStream?.removeListener(this._onImageResolved);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            Widget result = this.widget.useCachedNetworkImage
                ? new CachedNetworkImage(
                    this.widget.url,
                    this.widget.placeholder,
                    fit: BoxFit.contain,
                    headers: this.widget.headers,
                    onImageResolved: imageInfo => this._onImageResolved(imageInfo))
                : this._imageInfo != null
                    ? new RawImage(image: this._imageInfo.image, fit: BoxFit.contain)
                    : this.widget.placeholder;
            
            result = new ScaleTransition(
                scale: this._scaleAnimation, child: result);
            
            result = new FractionalTranslation(
                translation: this._positionAnimation.value,
                child: result);
            
            result = new GestureDetector(
                onScaleStart: this._onScaleStart,
                onScaleUpdate: this._onScaleUpdate,
                onScaleEnd: this._onScaleEnd,
                onDoubleTap: this._onDoubleTap,
                child: result);
            
            return result;
        }

        void _updateEffectiveMaxScale() {
            if (this._size != null && this._imageInfo != null) {
                this._effectiveMaxScale = this.widget.maxScale /
                                          (this._size.width * this._imageInfo.image.height >
                                           this._imageInfo.image.width * this._size.height
                                              ? this._size.height / this._imageInfo.image.height
                                              : this._size.width / this._imageInfo.image.width);
            }
        }

        void _onImageResolved(ImageInfo imageInfo, bool synchronousCall = false) {
            this.setState(() => {
                this._imageInfo = imageInfo;
                this._updateEffectiveMaxScale();
            });
        }

        public Offset toFractional(Offset offset) {
            return this._size != null
                ? new Offset(offset.dx / this._size.width, offset.dy / this._size.height)
                : Offset.zero;
        }

        Offset _clampPosition(Offset position, float scale) {
            if (scale <= 1.0f) {
                return Offset.zero;
            }
            
            float horizontalOriginalScale = 1.0f, verticalOriginalScale = 1.0f;
            if (this._size != null && this._imageInfo != null) {
                if (this._size.width * this._imageInfo.image.height >
                    this._imageInfo.image.width * this._size.height) {
                    horizontalOriginalScale = (this._imageInfo.image.width * this._size.height) /
                                              (this._size.width * this._imageInfo.image.height);
                }
                else if (this._size.width * this._imageInfo.image.height <
                         this._imageInfo.image.width * this._size.height) {
                    verticalOriginalScale = (this._size.width * this._imageInfo.image.height) /
                                            (this._imageInfo.image.width * this._size.height);
                }
            }

            float horizontalMax = scale * horizontalOriginalScale <= 1.0f
                ? 0
                : (scale * horizontalOriginalScale - 1.0f) / 2;
            float verticalMax = scale * verticalOriginalScale <= 1.0f
                ? 0
                : (scale * verticalOriginalScale - 1.0f) / 2;
            return new Offset(position.dx.clamp(-horizontalMax, horizontalMax),
                position.dy.clamp(-verticalMax, verticalMax));
        }

        void _onDoubleTap(DoubleTapDetails doubleTapDetails) {
            if (this._scaleAnimation.value < this._effectiveMaxScale) {
                Offset tapPosition = this.toFractional(doubleTapDetails.firstGlobalPosition) - new Offset(0.5f, 0.5f);
                Offset endPosition = (this._positionAnimation.value - tapPosition) * this._effectiveMaxScale /
                                     this._scaleAnimation.value + tapPosition;
                this._scaleAnimation = new FloatTween(
                        begin: this._scaleAnimation.value,
                        end: this._effectiveMaxScale)
                    .animate(this._scaleAnimationController);
                endPosition = this._clampPosition(endPosition, this._effectiveMaxScale);

                this._positionAnimation = new OffsetTween(
                    begin: this._positionAnimation.value,
                    end: endPosition).animate(this._positionAnimationController);
            }
            else {
                this._scaleAnimation = new FloatTween(
                        begin: this._scaleAnimation.value,
                        end: 1)
                    .animate(this._scaleAnimationController);
                this._positionAnimation = new OffsetTween(
                        begin: this._positionAnimation.value,
                        end: Offset.zero)
                    .animate(this._positionAnimationController);
            }
            this._scaleAnimationController.setValue(0);
            this._scaleAnimationController.animateTo(1);
            this._positionAnimationController.setValue(0);
            this._positionAnimationController.animateTo(1);
        }

        void _onScaleStart(ScaleStartDetails scaleStartDetails) {
            this._initialScale = this._scaleAnimation.value;
            this._initialPosition = this.toFractional(scaleStartDetails.focalPoint) - this._positionAnimation.value;
            this.scaling = true;
        }

        void _onScaleUpdate(ScaleUpdateDetails scaleUpdateDetails) {
            var newScale = this._initialScale * scaleUpdateDetails.scale;
            this._scaleAnimation = new FloatTween(begin: newScale, end: newScale)
                .animate(this._scaleAnimationController);
            this._scaleAnimationController.reset();

            var newPosition = this._clampPosition(this.toFractional(scaleUpdateDetails.focalPoint) - this._initialPosition,
                this._initialScale * scaleUpdateDetails.scale);
            this._positionAnimation = new OffsetTween(begin: newPosition, end: newPosition)
                .animate(this._positionAnimationController);
            this._positionAnimationController.reset();

            if (scaleUpdateDetails.scale == 1) {
                // This is panning
                var offset = this.toFractional(scaleUpdateDetails.focalPoint) - this._initialPosition;
                if (offset.dx.abs() > (this._scaleAnimation.value - 1) / 2) {
                    if (this.widget.onOverScroll != null) {
                        if (offset.dx > (this._scaleAnimation.value - 1) / 2) {
                            this.widget.onOverScroll(
                                (offset.dx - (this._scaleAnimation.value - 1) / 2) * this._size.width);
                        }
                        else if(offset.dx < -(this._scaleAnimation.value - 1) / 2) {
                            this.widget.onOverScroll(
                                (offset.dx + (this._scaleAnimation.value - 1) / 2) * this._size.width);
                        }
                    }
                }
            }
        }

        void _onScaleEnd(ScaleEndDetails scaleEndDetails) {
            this.scaling = false;
            if (this._scaleAnimation.value > this._effectiveMaxScale) {
                this._scaleAnimation = new FloatTween(
                        begin: this._scaleAnimation.value,
                        end: this._effectiveMaxScale)
                    .animate(this._scaleAnimationController);
                this._positionAnimation = new OffsetTween(
                    begin: this._positionAnimation.value,
                    end: this._clampPosition(
                        position: this._positionAnimation.value,
                        scale: this._effectiveMaxScale))
                    .animate(this._positionAnimationController);
            }
            else if (this._scaleAnimation.value < this._effectiveMinScale) {
                this._scaleAnimation = new FloatTween(
                        begin: this._scaleAnimation.value,
                        end: this._effectiveMinScale)
                    .animate(this._scaleAnimationController);
                this._positionAnimation = new OffsetTween(
                    begin: this._positionAnimation.value,
                    end: Offset.zero).animate(this._positionAnimationController);
            }
            else {
                return;
            }
            this._scaleAnimationController.setValue(0);
            this._scaleAnimationController.animateTo(1);
            this._positionAnimationController.setValue(0);
            this._positionAnimationController.animateTo(1);
        }
    }
}