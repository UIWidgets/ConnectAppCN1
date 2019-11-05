using System;
using System.Collections.Generic;
using System.Linq;
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
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
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
            this.controller = controller ?? new PageController(index);
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

        AnimationController _scaleAnimationController;
        Animation<float> _scaleAnimation;
        float _initialScale = 1.0f;

        AnimationController _positionAnimationController;
        Animation<Offset> _positionAnimation;
        Offset _initialPosition = Offset.zero;

        bool locked = false;
        bool scaling = false;

        public override void initState() {
            base.initState();
            this.currentIndex = this.widget.index;
            this._scaleAnimationController = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this);
            this._scaleAnimation = new FloatTween(1.0f, 1.0f).animate(this._scaleAnimationController);
            this._positionAnimationController = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this);
            this._positionAnimation = new OffsetTween(Offset.zero, Offset.zero).animate(this._scaleAnimationController);
            this._positionAnimationController.addListener(
                () => {
                    if (this._scaleAnimation.value <= 1.0f) {
                        if (!this.scaling) {
                            this.locked = false;
                        }
                    }
                    else {
                        this.locked = this._positionAnimation.value.dx > -(this._scaleAnimation.value - 1) / 2 &&
                                      this._positionAnimation.value.dx < (this._scaleAnimation.value - 1) / 2;
                    }
                }
            );
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            this._scaleAnimationController.dispose();
            base.dispose();
        }

        Widget _buildItem(string url) {
            Widget result = this.widget.useCachedNetworkImage
                ? CachedNetworkImageProvider.cachedNetworkImage(
                    url,
                    fit: BoxFit.contain,
                    headers: this.widget.headers ?? this._defaultHeaders)
                : Image.network(url,
                    fit: BoxFit.contain,
                    headers: this.widget.headers ?? this._defaultHeaders);
            
            result = new ScaleTransition(
                scale: this._scaleAnimation, child: result);
            
            result = new FractionalTranslation(
                translation: this._positionAnimation.value,
                child: result);
            
            return result;
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
            this._initialPosition = scaleStartDetails.focalPoint - this._positionAnimation.value;
            this.scaling = true;
        }
        
        void _onScaleUpdate(ScaleUpdateDetails scaleUpdateDetails) {
            this._scaleAnimation = new FloatTween(
                begin: this._scaleAnimation.value,
                end: this._initialScale * scaleUpdateDetails.scale)
                .animate(this._scaleAnimationController);
            this._scaleAnimationController.setValue(0);
            this._scaleAnimationController.animateTo(1);

            this._positionAnimation = new OffsetTween(
                    begin: this._positionAnimation.value,
                    end: _clampPosition(
                        position: scaleUpdateDetails.focalPoint - this._initialPosition,
                        scale: this._initialScale * scaleUpdateDetails.scale))
                .animate(this._positionAnimationController);
            this._positionAnimationController.setValue(0);
            this._positionAnimationController.animateTo(1);

            if (scaleUpdateDetails.scale > 1.0f) {
                this.locked = true;
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

        public override Widget build(BuildContext context) {
            var headers = this.widget.headers ?? this._defaultHeaders;
            var pageView = new PageView(
                controller: this.widget.controller,
                onPageChanged: index => { this.setState(() => { this.currentIndex = index; }); },
                physics: this.locked ? (ScrollPhysics) new NeverScrollableScrollPhysics() : new PageScrollPhysics(), 
                children: this.widget.urls.Select(this._buildItem).ToList());
            return new GestureDetector(
                onTap: () => { StoreProvider.store.dispatcher.dispatch(new MainNavigatorPopAction()); },
                onLongPress: this._pickImage,
                onScaleStart: this._onScaleStart,
                onScaleUpdate: this._onScaleUpdate,
                onScaleEnd: this._onScaleEnd,
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
                        })));
        }

        void _pickImage() {
            var imageUrl = this.widget.urls[this.currentIndex];
            var imagePath = SQLiteDBManager.instance.GetCachedFilePath(imageUrl);
            if (imagePath.isEmpty()) {
                return;
            }

            var items = new List<ActionSheetItem> {
                new ActionSheetItem(
                    "保存图片",
                    onTap: () => {
                        if (imagePath.isNotEmpty()) {
                            var imageStr = CImageUtils.readImage(imagePath);
                            PickImagePlugin.SaveImage(imagePath, imageStr);
                        }
                    }
                ),
                new ActionSheetItem("取消", type: ActionType.cancel)
            };

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                title: "",
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
}