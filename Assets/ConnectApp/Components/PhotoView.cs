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
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.widgets.Image;

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
            {"ConnectAppVersion", Config.versionNumber},
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
                    Debug.Log($"Scale: {scale}");
                    this.setState(() => {
                        this.locked = scale > 1.01f;
                    });
                },
                onOverScroll: (offset) => {
                });
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

        public ImageWrapper(
            Key key = null,
            string url = null,
            float maxScale = 2.0f,
            float minScale = 1.0f,
            bool useCachedNetworkImage = false,
            Dictionary<string, string> headers = null,
            OnScaleChangedCallback onScaleChanged = null,
            OnOverScrollCallback onOverScroll = null) : base(key: key) {
            D.assert(minScale >= 0.0f && minScale <= 1.0f);
            D.assert(maxScale >= 1.0f);
            this.url = url;
            this.useCachedNetworkImage = useCachedNetworkImage;
            this.headers = headers;
            this.maxScale = maxScale;
            this.minScale = minScale;
            this.onScaleChanged = onScaleChanged;
            this.onOverScroll = onOverScroll;
        }

        public override State createState() {
            return new _ImageWrapperState();
        }
    }

    class _ImageWrapperState : TickerProviderStateMixin<ImageWrapper> {
        AnimationController _scaleAnimationController;
        Animation<float> _scaleAnimation;
        float _initialScale = 1.0f;
        Size _size = new Size(1, 1);

        AnimationController _positionAnimationController;
        Animation<Offset> _positionAnimation;
        Offset _initialPosition = Offset.zero;
        bool scaling = false;

        public override void initState() {
            base.initState();
            
            this._size = MediaQuery.of(this.context).size; 
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
        }

        public override void dispose() {
            this._scaleAnimationController.dispose();
            this._positionAnimationController.dispose();
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            Widget result = this.widget.useCachedNetworkImage
                ? (Widget) new CachedNetworkImage(
                    this.widget.url,
                    new CustomActivityIndicator(loadingColor: LoadingColor.white),
                    fit: BoxFit.contain,
                    headers: this.widget.headers)
                : Image.network(this.widget.url,
                    fit: BoxFit.contain,
                    headers: this.widget.headers);
            
            result = new ScaleTransition(
                scale: this._scaleAnimation, child: result);
            
            result = new FractionalTranslation(
                translation: this._positionAnimation.value,
                child: result);
            
            result = new GestureDetector(
                onScaleStart: this._onScaleStart,
                onScaleUpdate: this._onScaleUpdate,
                onScaleEnd: this._onScaleEnd,
                child: result);
            
            return result;
        }

        public Offset toFractional(Offset offset) {
            return new Offset(offset.dx / this._size.width, offset.dy / this._size.height);
        }

        static Offset _clampPosition(Offset position, float scale) {
            if (scale <= 1.0f) {
                return Offset.zero;
            }

            float max = (scale - 1.0f) / 2;
            return new Offset(position.dx.clamp(-max, max), position.dy.clamp(-max, max));
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

            var newPosition = _clampPosition(this.toFractional(scaleUpdateDetails.focalPoint) - this._initialPosition,
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
            if (this._scaleAnimation.value > this.widget.maxScale) {
                this._scaleAnimation = new FloatTween(
                        begin: this._scaleAnimation.value,
                        end: this.widget.maxScale)
                    .animate(this._scaleAnimationController);
                this._positionAnimation = new OffsetTween(
                    begin: this._positionAnimation.value,
                    end: _clampPosition(
                        position: this._positionAnimation.value,
                        scale: this.widget.maxScale))
                    .animate(this._positionAnimationController);
            }
            else if (this._scaleAnimation.value < this.widget.minScale) {
                this._scaleAnimation = new FloatTween(
                        begin: this._scaleAnimation.value,
                        end: this.widget.minScale)
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