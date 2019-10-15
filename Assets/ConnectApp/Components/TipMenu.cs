using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public static class TipMenuConstant {
        public static readonly Color tipMenuBackgroundColor = new Color(-0x34000000);
        public static readonly Color dividerColor = new Color(-0x65000001);
        public static readonly TextStyle tipMenuTextStyle = new TextStyle(
            fontSize: 14,
            fontFamily: "Roboto-Regular",
            color: CColors.White
        );
        public const float tipMenuRadius = 8;
        public const float triangleWidth = 18;
        public const float triangleHeight = 9;
    }

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

        BoxDecoration _buildBoxDecoration(int index) {
            BorderRadius borderRadius;
            if (this.tipMenuItems.Count == 1) {
                borderRadius = BorderRadius.all(radius: TipMenuConstant.tipMenuRadius);
            } else if (index == 0) {
                borderRadius = BorderRadius.only(
                    Radius.circular(radius: TipMenuConstant.tipMenuRadius),
                    bottomLeft: Radius.circular(radius: TipMenuConstant.tipMenuRadius)
                );
            } else if (index == this.tipMenuItems.Count - 1) {
                borderRadius = BorderRadius.only(
                    topRight: Radius.circular(radius: TipMenuConstant.tipMenuRadius),
                    bottomRight: Radius.circular(radius: TipMenuConstant.tipMenuRadius)
                );
            }
            else {
                borderRadius = BorderRadius.zero;
            }
            return new BoxDecoration(
                borderRadius: borderRadius,
                color: TipMenuConstant.tipMenuBackgroundColor
            );
        }

        BoxDecoration _buildForegroundBoxDecoration(int index) {
            if (index == this.tipMenuItems.Count - 1) {
                return null;
            }

            return new BoxDecoration(
                border: new Border(
                    right: new BorderSide(
                        color: TipMenuConstant.dividerColor,
                        0.5f
                    )
                )
            );
        }

        float _getTipMenuHeight(BuildContext context) {
            if (this.tipMenuItems == null || this.tipMenuItems.Count == 0) {
                return 0;
            }

            var width = MediaQuery.of(context: context).size.width;
            var textHeight = CTextUtils.CalculateTextHeight(
                text: this.tipMenuItems[0].title,
                textStyle: TipMenuConstant.tipMenuTextStyle,
                textWidth: width,
                1
            );
            return textHeight + 20;
        }

        void _createTipMenu(BuildContext context, CornerPosition cornerPosition, Offset position, Size size) {
            dismiss();
            float triangleY;
            float contentY;
            if (cornerPosition == CornerPosition.Top) {
                triangleY = position.dy - TipMenuConstant.triangleHeight;
                contentY = triangleY - this._getTipMenuHeight(context: context);
            }
            else {
                triangleY = position.dy;
                contentY = triangleY + TipMenuConstant.triangleHeight;
            }
            _overlayState = Overlay.of(context: context);
            _overlayEntry = new OverlayEntry(
                _context => Positioned.fill(
                    new GestureDetector(
                        onTap: dismiss,
                        child: new Container(
                            color: CColors.Transparent,
                            child: new Stack(
                                children: new List<Widget> {
                                    new Positioned(
                                        top: triangleY,
                                        left: size.width / 2.0f + position.dx,
                                        child: new CustomPaint(
                                            painter: new TrianglePainter(
                                                cornerPosition: cornerPosition,
                                                width: TipMenuConstant.triangleWidth,
                                                height: TipMenuConstant.triangleHeight
                                            )
                                        )
                                    ),
                                    new Positioned(
                                        top: contentY,
                                        left: size.width / 2.0f + position.dx - 25 * this.tipMenuItems.Count,
                                        child: new Row(
                                            mainAxisSize: MainAxisSize.min,
                                            children: this._buildTipMenus()
                                        )
                                    )
                                }
                            )
                        )
                    )
                )
            );
            _isVisible = true;
            _overlayState.insert(entry: _overlayEntry);
        }

        static void dismiss() {
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
                    var renderBox = (RenderBox) this._tipMenuKey.currentContext.findRenderObject();
                    var position = renderBox.localToGlobal(point: Offset.zero);
                    CornerPosition cornerPosition;
                    if (position.dy <= 
                        44 
                        + CCommonUtils.getSafeAreaTopPadding(context: context) 
                        + TipMenuConstant.triangleHeight
                        + this._getTipMenuHeight(context: context)) {
                        position = new Offset(dx: position.dx, position.dy + renderBox.size.height);
                        cornerPosition = CornerPosition.Bottom;
                    }
                    else {
                        cornerPosition = CornerPosition.Top;
                    }
                    // var position = renderBox.localToGlobal(new Offset(0, dy: renderBox.size.height));
                    this._createTipMenu(
                        context: context,
                        cornerPosition: cornerPosition,
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

        List<Widget> _buildTipMenus() {
            if (this.tipMenuItems == null || this.tipMenuItems.Count == 0) {
                return new List<Widget>();
            }

            var widgets = new List<Widget>();
            this.tipMenuItems.ForEach(tipMenuItem => {
                var index = this.tipMenuItems.IndexOf(item: tipMenuItem);
                var widget = new GestureDetector(
                    child: new Container(
                        padding: EdgeInsets.fromLTRB(12, 8, 0, 8),
                        child: new Container(
                            padding: EdgeInsets.fromLTRB(3, 2, 15, 2),
                            forgroundDecoration: this._buildForegroundBoxDecoration(index: index),
                            child: new Text(
                                data: tipMenuItem.title,
                                style: TipMenuConstant.tipMenuTextStyle
                            )
                        ),
                        decoration: this._buildBoxDecoration(index: index)
                    ),
                    onTap: () => {
                        tipMenuItem.onTap();
                        dismiss();
                    }
                );
                widgets.Add(item: widget);
            });

            return widgets;
        }
    }

    public enum CornerPosition {
        Top,
        Bottom
    }

    public class TrianglePainter : AbstractCustomPainter {
        public TrianglePainter(
            Color color = null,
            CornerPosition cornerPosition = CornerPosition.Bottom,
            float width = 8,
            float height = 8
        ) {
            this.color = color ?? TipMenuConstant.tipMenuBackgroundColor;
            this.cornerPosition = cornerPosition;
            this.width = width;
            this.height = height;
        }

        readonly Color color;
        readonly CornerPosition cornerPosition;
        readonly float width;
        readonly float height;

        public override void paint(Canvas canvas, Size size) {
            Path path = new Path();
            Paint paint = new Paint {
                color = this.color,
                style = PaintingStyle.fill
            };
            if (this.cornerPosition == CornerPosition.Bottom) {
                path.moveTo(0, y: this.height);
                path.lineTo(x: this.width, y: this.height);
                path.lineTo(this.width / 2, 0);
            }
            else {
                path.moveTo(0, 0);
                path.lineTo(x: this.width, 0);
                path.lineTo(this.width / 2, y: this.height);
            }
            
            path.close();
            canvas.drawPath(path: path, paint: paint);
        }

        public override bool shouldRepaint(CustomPainter oldDelegate) {
            return true;
        }
    }
}