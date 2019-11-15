using System;
using System.Collections.Generic;
using System.Linq;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class TipMenuItem {
        public TipMenuItem(
            string title,
            VoidCallback onTap = null
        ) {
            D.assert(title != null);
            this.title = title;
            this.onTap = onTap;
        }

        public readonly string title;
        public readonly VoidCallback onTap;
    }

    public class TipMenu : StatelessWidget {
        public TipMenu(
            List<TipMenuItem> tipMenuItems,
            Widget child,
            Key key = null
        ) : base(key: key) {
            this.tipMenuItems = tipMenuItems;
            this.child = child;
        }

        readonly List<TipMenuItem> tipMenuItems;
        readonly Widget child;

        readonly GlobalKey _tipMenuKey = GlobalKey.key("tip-menu");
        static OverlayState _overlayState;
        static OverlayEntry _overlayEntry;
        static bool _isVisible;

        float _getTipMenuHeight(BuildContext context) {
            if (this.tipMenuItems == null || this.tipMenuItems.Count == 0) {
                return 0;
            }

            var width = MediaQuery.of(context: context).size.width;
            var textHeight = CTextUtils.CalculateTextHeight(
                this.tipMenuItems.FirstOrDefault()?.title ?? "",
                textStyle: CustomTextSelectionControlsUtils._kToolbarButtonFontStyle,
                textWidth: width,
                1
            );
            return textHeight + 20;
        }

        void _createTipMenu(BuildContext context, ArrowDirection arrowDirection, Offset position, Size size) {
            dismiss();
            var width = MediaQuery.of(context: context).size.width - 32 * this.tipMenuItems.Count;
            float tipMenuHeight = this._getTipMenuHeight(context: context);
            float triangleY = arrowDirection == ArrowDirection.up
                ? position.dy
                : position.dy - CustomTextSelectionControlsUtils._kToolbarTriangleSize.height
                              - tipMenuHeight;
            float left;
            float childCenterX = size.width / 2.0f + position.dx;
            if (childCenterX >= width) {
                left = width - 32 * this.tipMenuItems.Count - 16;
            }
            else {
                left = childCenterX - 32 * this.tipMenuItems.Count;
            }

            List<Widget> children;
            if (arrowDirection == ArrowDirection.down) {
                children = new List<Widget> {
                    new Positioned(
                        top: triangleY,
                        left: left,
                        child: new _TipMenuContent(
                            tipMenuItems: this.tipMenuItems
                        )
                    ),
                    new Positioned(
                        top: triangleY + tipMenuHeight,
                        left: childCenterX,
                        child: new _TipMenuTriangle(
                            arrowDirection: arrowDirection
                        )
                    )
                };
            }
            else {
                children = new List<Widget> {
                    new Positioned(
                        top: triangleY,
                        left: childCenterX,
                        child: new _TipMenuTriangle(
                            arrowDirection: arrowDirection
                        )
                    ),
                    new Positioned(
                        top: triangleY + CustomTextSelectionControlsUtils._kToolbarTriangleSize.height,
                        left: left,
                        child: new _TipMenuContent(
                            tipMenuItems: this.tipMenuItems
                        )
                    )
                };
            }

            _overlayState = Overlay.of(context: context);
            _overlayEntry = new OverlayEntry(
                _context => Positioned.fill(
                    new GestureDetector(
                        onTap: dismiss,
                        child: new Container(
                            color: CColors.Transparent,
                            child: new Stack(
                                children: children
                            )
                        )
                    )
                )
            );
            _isVisible = true;
            _overlayState.insert(entry: _overlayEntry);
        }

        public static void dismiss() {
            if (!_isVisible) {
                return;
            }

            _isVisible = false;
            _overlayEntry?.remove();
        }

        public override Widget build(BuildContext context) {
            if (this.tipMenuItems == null || this.tipMenuItems.Count == 0) {
                return this.child;
            }

            return new GestureDetector(
                onLongPress: () => {
                    var height = MediaQuery.of(context: context).size.height;
                    var renderBox = (RenderBox) this._tipMenuKey.currentContext.findRenderObject();
                    var position = renderBox.localToGlobal(point: Offset.zero);
                    ArrowDirection arrowDirection;
                    if (position.dy >
                        44
                        + CCommonUtils.getSafeAreaTopPadding(context: context)
                        + CustomTextSelectionControlsUtils._kToolbarTriangleSize.height
                        + this._getTipMenuHeight(context: context)) {
                        arrowDirection = ArrowDirection.down;
                    }
                    else if (position.dy + renderBox.size.height < height - 44) {
                        position = new Offset(dx: position.dx, position.dy + renderBox.size.height);
                        arrowDirection = ArrowDirection.up;
                    }
                    else {
                        position = new Offset(dx: position.dx, height / 2.0f);
                        arrowDirection = ArrowDirection.up;
                    }

                    // var position = renderBox.localToGlobal(new Offset(0, dy: renderBox.size.height));
                    this._createTipMenu(
                        context: context,
                        arrowDirection: arrowDirection,
                        position: position,
                        size: renderBox.size
                    );
                },
                child: new Container(
                    key: this._tipMenuKey,
                    child: this.child
                )
            );
        }
    }

    class _TipMenuContent : StatefulWidget {
        public _TipMenuContent(
            List<TipMenuItem> tipMenuItems,
            Key key = null
        ) : base(key: key) {
            this.tipMenuItems = tipMenuItems;
        }

        public readonly List<TipMenuItem> tipMenuItems;

        public override State createState() {
            return new _TipMenuContentState();
        }
    }

    class _TipMenuContentState : State<_TipMenuContent>, TickerProvider {
        Animation<float> _animationOpacity;
        AnimationController _animationController;

        public override void initState() {
            base.initState();
            this._animationController = new AnimationController(
                duration: TimeSpan.FromMilliseconds(150),
                vsync: this
            );
            CurvedAnimation curve = new CurvedAnimation(parent: this._animationController, curve: Curves.linear);
            this._animationOpacity = new FloatTween(0, 1).animate(parent: curve);
            this._animationController.forward();
        }

        public override void dispose() {
            this._animationController.dispose();
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick: onTick, () => $"created by {this}");
        }

        public override Widget build(BuildContext context) {
            List<Widget> items = new List<Widget>();
            Widget onePhysicalPixelVerticalDivider =
                new SizedBox(width: 1.0f / MediaQuery.of(context: context).devicePixelRatio);

            this.widget.tipMenuItems.ForEach(tipMenuItem => {
                items.Add(_buildToolbarButton(text: tipMenuItem.title, onPressed: tipMenuItem.onTap));
                var index = this.widget.tipMenuItems.IndexOf(item: tipMenuItem);
                if (this.widget.tipMenuItems.Count > 1 && index < this.widget.tipMenuItems.Count) {
                    items.Add(item: onePhysicalPixelVerticalDivider);
                }
            });

            Widget toolbar = new ClipRRect(
                borderRadius: CustomTextSelectionControlsUtils._kToolbarBorderRadius,
                child: new DecoratedBox(
                    decoration: new BoxDecoration(
                        color: CustomTextSelectionControlsUtils._kToolbarDividerColor,
                        borderRadius: CustomTextSelectionControlsUtils._kToolbarBorderRadius,
                        border: Border.all(color: CustomTextSelectionControlsUtils._kToolbarBackgroundColor, 0)
                    ),
                    child: new Row(mainAxisSize: MainAxisSize.min, children: items)
                )
            );

            return new AnimatedBuilder(
                animation: this._animationController,
                builder: (cxt, child) => new Opacity(
                    opacity: this._animationOpacity.value,
                    child: toolbar
                )
            );
        }

        static CustomButton _buildToolbarButton(string text, VoidCallback onPressed) {
            return new CustomButton(
                child: new Text(data: text, style: CustomTextSelectionControlsUtils._kToolbarButtonFontStyle),
                decoration: new BoxDecoration(color: CustomTextSelectionControlsUtils._kToolbarBackgroundColor),
                padding: CustomTextSelectionControlsUtils._kToolbarButtonPadding,
                onPressed: () => {
                    onPressed();
                    TipMenu.dismiss();
                }
            );
        }
    }

    class _TipMenuTriangle : StatefulWidget {
        public _TipMenuTriangle(
            ArrowDirection arrowDirection,
            Key key = null
        ) : base(key: key) {
            this.arrowDirection = arrowDirection;
        }

        public readonly ArrowDirection arrowDirection;

        public override State createState() {
            return new _TipMenuTriangleState();
        }
    }

    class _TipMenuTriangleState : State<_TipMenuTriangle>, TickerProvider {
        Animation<float> _animationOpacity;
        AnimationController _animationController;

        public override void initState() {
            base.initState();
            this._animationController = new AnimationController(
                duration: TimeSpan.FromMilliseconds(150),
                vsync: this
            );
            CurvedAnimation curve = new CurvedAnimation(parent: this._animationController, curve: Curves.linear);
            this._animationOpacity = new FloatTween(0, 1).animate(parent: curve);
            this._animationController.forward();
        }

        public override void dispose() {
            this._animationController.dispose();
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick: onTick, () => $"created by {this}");
        }

        public override Widget build(BuildContext context) {
            Widget triangle = SizedBox.fromSize(
                size: CustomTextSelectionControlsUtils._kToolbarTriangleSize,
                child: new CustomPaint(
                    painter: new TrianglePainter(
                        arrowDirection: this.widget.arrowDirection
                    )
                )
            );
            return new AnimatedBuilder(
                animation: this._animationController,
                builder: (cxt, child) => new Opacity(
                    opacity: this._animationOpacity.value,
                    child: triangle
                )
            );
        }
    }
}