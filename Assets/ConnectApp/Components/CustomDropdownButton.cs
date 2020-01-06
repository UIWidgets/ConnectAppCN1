using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    class _CustomDropdownRoute<T> : PopupRoute where T : class {
        public _CustomDropdownRoute(
            List<CustomDropdownMenuItem<T>> items = null,
            Offset position = null,
            int? selectedIndex = null,
            int elevation = 10,
            bool isAnimation = false,
            Widget headerWidget = null,
            Widget footerWidget = null,
            TextStyle style = null
        ) {
            D.assert(style != null);
            this.items = items;
            this.position = position;
            this.selectedIndex = selectedIndex;
            this.elevation = elevation;
            this.isAnimation = isAnimation;
            this.headerWidget = headerWidget;
            this.footerWidget = footerWidget;
            this.style = style;
        }

        readonly List<CustomDropdownMenuItem<T>> items;
        readonly Offset position;
        readonly int? selectedIndex;
        readonly int elevation;
        readonly bool isAnimation;
        readonly Widget headerWidget;
        readonly Widget footerWidget;
        readonly TextStyle style;

        public override TimeSpan transitionDuration {
            get { return this.isAnimation ? TimeSpan.FromMilliseconds(300) : TimeSpan.Zero; }
        }

        public override bool barrierDismissible {
            get { return true; }
        }

        public override Color barrierColor {
            get { return null; }
        }

        Animation<float> _animation;
        Animation<float> _sizeAnimation;

        public override Animation<float> createAnimation() {
            D.assert(this._animation == null);
            this._animation = new CurvedAnimation(
                base.createAnimation(),
                curve: Curves.linearToEaseOut,
                reverseCurve: Curves.linearToEaseOut.flipped
            );
            this._sizeAnimation = new FloatTween(0, 1).animate(parent: this._animation);
            return this._animation;
        }

        public override Widget buildPage(BuildContext context, Animation<float> animation,
            Animation<float> secondaryAnimation) {
            List<Widget> children = new List<Widget>();
            if (this.headerWidget != null) {
                children.Add(item: this.headerWidget);
            }
            this.items.ForEach(item => {
                var currentIndex = this.items.IndexOf(item: item);
                var isSelected = currentIndex == (this.selectedIndex ?? 0);
                children.Add(new DefaultTextStyle(
                    style: isSelected ? this.style.copyWith(color: CColors.PrimaryBlue) : this.style,
                    child: new GestureDetector(
                        onTap: () => Navigator.pop(
                            context: context,
                            result: item.value
                        ),
                        child: new Container(
                            height: 44,
                            padding: EdgeInsets.only(16),
                            color: CColors.White,
                            alignment: Alignment.centerLeft,
                            child: item.child
                        )
                    )
                ));
            });
            if (this.footerWidget != null) {
                children.Add(item: this.footerWidget);
            }

            Widget child;
            if (this.isAnimation) {
                child = new SizeTransition(
                    sizeFactor: this._sizeAnimation,
                    child: new Column(
                        mainAxisSize: MainAxisSize.min,
                        children: children
                    )
                );
            }
            else {
                child = new Column(
                    mainAxisSize: MainAxisSize.min,
                    children: children
                );
            }

            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        new GestureDetector(
                            onTap: () => Navigator.pop(context: context),
                            child: new Container(
                                height: this.position.dy,
                                color: CColors.Transparent
                            )
                        ),
                        new Container(
                            decoration: new BoxDecoration(
                                boxShadow: new List<BoxShadow> {
                                    new BoxShadow(
                                        CColors.Black.withOpacity(0.2f),
                                        blurRadius: this.elevation,
                                        spreadRadius: 0,
                                        offset: new Offset(0, 2))
                                }
                            ),
                            child: child
                        )
                    }
                )
            );
        }

        public void dismiss() {
            this.navigator?.removeRoute(this);
        }
    }

    public class CustomDropdownMenuItem<T> where T : class {
        public CustomDropdownMenuItem(
            Widget child,
            T value
        ) {
            D.assert(child != null);
            this.child = child;
            this.value = value;
        }

        public readonly Widget child;
        public readonly T value;
    }

    public class CustomDropdownButton<T> : StatefulWidget where T : class {
        public CustomDropdownButton(
            List<CustomDropdownMenuItem<T>> items,
            T value = null,
            ValueChanged<T> onChanged = null,
            int elevation = 10,
            bool isAnimation = false,
            Widget headerWidget = null,
            Widget footerWidget = null,
            TextStyle style = null,
            Key key = null
        ) : base(key: key) {
            D.assert(items != null);
            D.assert(items.Count >= 0);
            this.items = items;
            this.value = value;
            this.onChanged = onChanged;
            this.elevation = elevation;
            this.isAnimation = isAnimation;
            this.headerWidget = headerWidget;
            this.footerWidget = footerWidget;
            this.style = style ?? CTextStyle.PLargeBody;
        }

        public readonly List<CustomDropdownMenuItem<T>> items;
        public readonly T value;
        public readonly ValueChanged<T> onChanged;
        public readonly int elevation;
        public readonly bool isAnimation;
        public readonly Widget headerWidget;
        public readonly Widget footerWidget;
        public readonly TextStyle style;

        public override State createState() {
            return new _CustomDropdownButtonState<T>();
        }
    }

    class _CustomDropdownButtonState<T> : SingleTickerProviderStateMixin<CustomDropdownButton<T>> where T : class {
        int? _selectedIndex;
        _CustomDropdownRoute<T> _dropdownRoute;
        readonly GlobalKey _dropdownKey = GlobalKey.key("drop-down");
        AnimationController _rotationController;
        Animation<float> _rotationAnimation;
        bool _isDropdown;

        public override void initState() {
            base.initState();
            this._updateSelectedIndex();
            this._isDropdown = false;
            if (this.widget.isAnimation) {
                this._rotationController = new AnimationController(duration: TimeSpan.FromMilliseconds(300), vsync: this);
                this._rotationAnimation = new FloatTween(0, 0.5f).animate(parent: this._rotationController);
            }
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget: oldWidget);
            this._updateSelectedIndex();
        }

        public override void dispose() {
            this._removeDropdownRoute();
            if (this.widget.isAnimation) {
                this._rotationController.dispose();
            }
            base.dispose();
        }

        void _removeDropdownRoute() {
            this._dropdownRoute?.dismiss();
            this._dropdownRoute = null;
        }

        void _updateSelectedIndex() {
            this._selectedIndex = null;
            this.widget.items.ForEach(item => {
                if (item.value == this.widget.value) {
                    this._selectedIndex = this.widget.items.IndexOf(item: item);
                }
            });
        }

        void _handleTap() {
            if (this.widget.isAnimation) {
                this._rotationController.forward();
            }
            else {
                if (!this._isDropdown) {
                    this.setState(() => this._isDropdown = true);
                }
            }
            var renderBox = (RenderBox) this._dropdownKey.currentContext.findRenderObject();
            var position = renderBox.localToGlobal(new Offset(0, dy: renderBox.size.height));
            this._dropdownRoute = new _CustomDropdownRoute<T>(
                items: this.widget.items,
                position: position,
                this._selectedIndex ?? 0,
                elevation: this.widget.elevation,
                isAnimation: this.widget.isAnimation,
                headerWidget: this.widget.headerWidget,
                footerWidget: this.widget.footerWidget,
                style: this.widget.style
            );

            Navigator.push(context: this.context, route: this._dropdownRoute).Then(newValue => {
                if (this.widget.isAnimation) {
                    this._rotationController.reverse();
                }
                else {
                    if (this._isDropdown) {
                        this.setState(() => this._isDropdown = false);
                    }
                }
                this._dropdownRoute = null;
                if (!this.mounted || newValue == null) {
                    return;
                }

                this.widget.onChanged?.Invoke((T) newValue);
            });
        }

        public override Widget build(BuildContext context) {
            Widget iconWidget;
            if (this.widget.isAnimation) {
                iconWidget = new RotationTransition(
                    turns: this._rotationAnimation,
                    child: new Icon(icon: Icons.keyboard_arrow_down)
                );
            }
            else {
                iconWidget = new Icon(this._isDropdown ? Icons.keyboard_arrow_up : Icons.keyboard_arrow_down);
            }
            Widget result = new DefaultTextStyle(
                style: new TextStyle(
                    fontSize: 14,
                    fontFamily: "Roboto-Medium",
                    color: CColors.TextBody5
                ),
                child: new Container(
                    height: 44.0f,
                    child: new Row(
                        mainAxisSize: MainAxisSize.min,
                        children: new List<Widget> {
                            this.widget.items[this._selectedIndex ?? 0].child,
                            new IconTheme(
                                data: new IconThemeData(
                                    color: CColors.Icon,
                                    size: 20
                                ),
                                child: iconWidget
                            )
                        }
                    )
                )
            );

            return new GestureDetector(
                onTap: this._handleTap,
                behavior: HitTestBehavior.opaque,
                child: new Container(
                    key: this._dropdownKey,
                    child: result
                )
            );
        }
    }
}