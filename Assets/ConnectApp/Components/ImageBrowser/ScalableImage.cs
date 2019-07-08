using System;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.ui.Image;
using ImageUtils = Unity.UIWidgets.widgets.ImageUtils;

namespace ConnectApp.Components.ImageBrowser {
    public class ScalableImage : StatefulWidget {
        public ScalableImage(
            ImageProvider imageProvider,
            bool wrapInAspect = false,
            bool enableScaling = true,
            float maxScale = 16.0f,
            float dragSpeed = 8.0f,
            Size size = null,
            Key key = null
        ) : base(key) {
            this.imageProvider = imageProvider;
            this.wrapInAspect = wrapInAspect;
            this.enableScaling = enableScaling;
            this.maxScale = maxScale;
            this.dragSpeed = dragSpeed;
            this.size = size ?? Size.square(float.PositiveInfinity);
        }

        public readonly ImageProvider imageProvider;
        public readonly bool wrapInAspect;
        public readonly bool enableScaling;
        public readonly float maxScale;
        public readonly float dragSpeed;
        public readonly Size size;

        public override State createState() {
            return new _ScalableImageState();
        }
    }

    class _ScalableImageState : State<ScalableImage> {
        ImageStream _imageStream;
        ImageInfo _imageInfo;
        float _scale = 1.0f;
        float _lastEndScale = 1.0f;
        Offset _offset = Offset.zero;
        Offset _lastFocalPoint;
        Size _imageSize;
        Offset _targetPointPixelSpace;
        Offset _targetPointDrawSpace;


        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            this._getImage();
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            this._getImage();
        }

        public override void dispose() {
            this._imageStream.removeListener(this._handleImageChanged);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            if (this._imageInfo == null) {
                return new Container(
                    alignment: Alignment.center,
                    child: new FractionallySizedBox(
                        widthFactor: 0.1f,
                        child: new AspectRatio(
                            aspectRatio: 1.0f,
                            child: new CustomActivityIndicator(loadingColor: LoadingColor.white)
                        )
                    )
                );
            }
            else {
                Widget painter = new CustomPaint(
                    size: this.widget.size,
                    painter: new _ScalableImagePainter(this._imageInfo.image, this._offset, this._scale),
                    willChange: true
                );
                if (this.widget.wrapInAspect) {
                    painter = new AspectRatio(
                        aspectRatio: this._imageSize.width / this._imageSize.height,
                        child: painter);
                }

                if (this.widget.enableScaling) {
                    return new GestureDetector(
                        child: painter,
                        onScaleUpdate: this._handleScaleUpdate,
                        onScaleEnd: this._handleScaleEnd,
                        onScaleStart: this._handleScaleStart,
                        onDoubleTap: this._handleDoubleTap
                    );
                }
                else {
                    return painter;
                }
            }
        }


        void _handleImageChanged(ImageInfo imageInfo, bool synchronousCall) {
            this.setState(() => {
                this._imageInfo = imageInfo;
                this._imageSize = this._imageInfo == null
                    ? null
                    : new Size(this._imageInfo.image.width, this._imageInfo.image.height);
            });
        }

        void _getImage() {
            ImageStream oldImageStream = this._imageStream;
            this._imageStream =
                this.widget.imageProvider.resolve(ImageUtils.createLocalImageConfiguration(this.context));
            if (this._imageStream.key != oldImageStream?.key) {
                oldImageStream?.removeListener(this._handleImageChanged);
                this._imageStream.addListener(this._handleImageChanged);
            }
        }

        void _handleDoubleTap(DoubleTapDetails details) {
           //TODO
        }

        void _handleScaleStart(ScaleStartDetails start) {
            this._lastFocalPoint = start.focalPoint;
            this._targetPointDrawSpace = (this.context.findRenderObject() as RenderBox).globalToLocal(start.focalPoint);
            this._targetPointPixelSpace = this.drawSpaceToPixelSpace(this._targetPointDrawSpace, this.context.size,
                this._offset, this._imageSize, this._scale);
        }

        void _handleScaleEnd(ScaleEndDetails end) {
            this._lastEndScale = this._scale;
        }

        void _handleScaleUpdate(ScaleUpdateDetails details) {
            //init old values
            float newScale = this._scale;
            Offset newOffset = this._offset;

            if (details.scale == 1.0) {
                //This is a movement
                //Calculate movement since last call
                Offset delta = (this._lastFocalPoint - details.focalPoint) * this.widget.dragSpeed / this._scale;
                //Store the new information
                this._lastFocalPoint = details.focalPoint;
                //And move it
                newOffset += delta;
            }
            else {
                //Round the scale to three points after comma to prevent shaking
                float roundedScale = this._roundAfter(details.scale, 3);
                //Calculate new scale but do not scale to far out or in
                newScale = Math.Min(this.widget.maxScale, Math.Max(1.0f, roundedScale * this._lastEndScale));
                //Move the offset so that the target point stays at the same position after scaling
                newOffset = this._elementwiseDivision(this._targetPointDrawSpace,
                                -this._linearTransformationFactor(this.context.size, this._imageSize, newScale)) +
                            this._targetPointPixelSpace;
            }

            //Don't move to far left
            newOffset = this._elementwiseMax(newOffset, Offset.zero);
            //Nor to far right
            float borderScale = 1.0f - 1.0f / newScale;
            newOffset = this._elementwiseMin(newOffset, this._asOffset(this._imageSize * borderScale));
            if (newScale != this._scale || newOffset != this._offset) {
                this.setState(() => {
                    this._scale = newScale;
                    this._offset = newOffset;
                });
            }
        }


        Offset _linearTransformationFactor(Size drawSpaceSize, Size imageSize, float scale) {
            return new Offset(drawSpaceSize.width / (imageSize.width / scale),
                drawSpaceSize.height / (imageSize.height / scale));
        }


        Offset pixelSpaceToDrawSpace(Offset pixelSpace, Size drawSpaceSize, Offset offset, Size imageSize,
            float scale) {
            return this._elementwiseMultiplication(pixelSpace - offset,
                this._linearTransformationFactor(drawSpaceSize, imageSize, scale));
        }

        Offset drawSpaceToPixelSpace(Offset drawSpace, Size drawSpaceSize, Offset offset, Size imageSize, float scale) {
            return this._elementwiseDivision(drawSpace,
                       this._linearTransformationFactor(drawSpaceSize, imageSize, scale)) + offset;
        }

        float _roundAfter(float number, int position) {
            float shift = (float) Math.Pow(10, position);
            return (number * shift) / shift;
        }

        Offset _elementwiseDivision(Offset dividend, Offset divisor) {
            return dividend.scale(1.0f / divisor.dx, 1.0f / divisor.dy);
        }

        Offset _elementwiseMultiplication(Offset a, Offset b) {
            return a.scale(b.dx, b.dy);
        }

        Offset _elementwiseMin(Offset a, Offset b) {
            return new Offset(Math.Min(a.dx, b.dx), Math.Min(a.dy, b.dy));
        }

        Offset _elementwiseMax(Offset a, Offset b) {
            return new Offset(Math.Max(a.dx, b.dx), Math.Max(a.dy, b.dy));
        }

        Offset _asOffset(Size s) {
            return new Offset(s.width, s.height);
        }
    }

    class _ScalableImagePainter : AbstractCustomPainter {
        public _ScalableImagePainter(
            Image _image,
            Offset offset,
            float scale
        ) {
            this._image = _image;
            this._paint = new Paint();
            this._rect = Rect.fromLTWH(
                offset.dx,
                offset.dy,
                _image.width / scale,
                _image.height / scale
            );
        }

        Image _image;
        Paint _paint;
        Rect _rect;

        public override bool shouldRepaint(CustomPainter oldDelegate) {
            var oldPainter = (_ScalableImagePainter) oldDelegate;
            return this._rect != oldPainter._rect || this._image != oldPainter._image;
        }

        public override void paint(Canvas canvas, Size size) {
            canvas.drawImageRect(
                this._image,
                this._rect,
                Rect.fromLTWH(0.0f, 0.0f, size.width, size.height),
                this._paint);
        }
    }
}