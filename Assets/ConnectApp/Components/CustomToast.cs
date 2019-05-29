using System;
using ConnectApp.Constants;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public enum ToastGravity {
        top,
        center,
        bottom
    }

    public class CustomToastItem {
        public CustomToastItem(
            BuildContext context,
            string message,
            TimeSpan? duration = null,
            ToastGravity gravity = ToastGravity.bottom,
            Color backgroundColor = null,
            Color textColor = null,
            float radius = 4
        ) {
            D.assert(context != null);
            D.assert(message != null);
            this.context = context;
            this.message = message;
            this.duration = duration ?? new TimeSpan(0, 0, 1);
            this.gravity = gravity;
            this.backgroundColor = backgroundColor ?? new Color(0xAA000000);
            this.textColor = textColor ?? CColors.White;
            this.radius = radius;
        }

        public readonly BuildContext context;
        public readonly string message;
        public readonly TimeSpan? duration;
        public readonly ToastGravity gravity;
        public readonly Color backgroundColor;
        public readonly Color textColor;
        public readonly float radius;
    }

    public static class CustomToast {
        public static void show(CustomToastItem toastItem) {
            CustomToastView.dismiss();
            CustomToastView.createView(toastItem: toastItem);
        }

        public static void hidden() {
            CustomToastView.dismiss();
        }
    }

    public static class CustomToastView {
        static OverlayState _overlayState;
        static OverlayEntry _overlayEntry;
        static bool _isVisible;
        static Timer _timer;

        public static void createView(CustomToastItem toastItem) {
            _overlayState = Overlay.of(context: toastItem.context);
            _overlayEntry = new OverlayEntry(
                _context => new CustomToastWidget(
                    gravity: toastItem.gravity,
                    new Container(
                        width: MediaQuery.of(context: _context).size.width,
                        alignment: Alignment.center,
                        child: new Container(
                            decoration: new BoxDecoration(
                                color: toastItem.backgroundColor,
                                borderRadius: BorderRadius.circular(radius: toastItem.radius)
                            ),
                            margin: EdgeInsets.symmetric(horizontal: 20),
                            padding: EdgeInsets.fromLTRB(16, 10, 16, 10),
                            child: new Text(
                                data: toastItem.message,
                                style: CTextStyle.PLargeTitle.copyWith(color: toastItem.textColor)
                            )
                        )
                    )
                )
            );
            _isVisible = true;
            _overlayState.insert(entry: _overlayEntry);
            _timer = Window.instance.run((TimeSpan) toastItem.duration, callback: dismiss);
        }

        public static void dismiss() {
            if (!_isVisible) {
                return;
            }

            _isVisible = false;
            if (_overlayEntry != null) {
                _overlayEntry.remove();
            }

            if (_timer != null) {
                _timer.Dispose();
                _timer = null;
            }
        }
    }

    public class CustomToastWidget : StatelessWidget {
        public CustomToastWidget(
            ToastGravity gravity,
            Widget child,
            Key key = null
        ) : base(key: key) {
            this.gravity = gravity;
            this.child = child;
        }

        readonly ToastGravity gravity;
        readonly Widget child;

        public override Widget build(BuildContext context) {
            float? top = null;
            float? bottom = null;
            if (this.gravity == ToastGravity.top) {
                top = 50;
            }

            if (this.gravity == ToastGravity.center) {
            }

            if (this.gravity == ToastGravity.bottom) {
                bottom = 50;
            }

            return new Positioned(
                top: top,
                bottom: bottom,
                child: this.child
            );
        }
    }
}