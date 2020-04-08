using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Utils;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public enum ActionType {
        normal,
        cancel,
        destructive
    }

    public class ActionSheetItem {
        public ActionSheetItem(
            string title,
            ActionType type = ActionType.normal,
            VoidCallback onTap = null
        ) {
            D.assert(title != null);
            this.title = title;
            this.type = type;
            this.onTap = onTap;
        }

        public readonly string title;
        public readonly ActionType type;
        public readonly VoidCallback onTap;
    }

    public class ActionSheet : StatelessWidget {
        public ActionSheet(
            Key key = null,
            string title = null,
            List<ActionSheetItem> items = null
        ) : base(key: key) {
            this.title = title;
            this.items = items;
        }

        readonly string title;
        readonly List<ActionSheetItem> items;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    mainAxisSize: MainAxisSize.min,
                    children: new List<Widget> {
                        _buildTitle(title: this.title),
                        _buildButtons(items: this.items),
                        this.title.isNotEmpty() || this.items.isNotNullAndEmpty()
                            ? new Container(
                                height: MediaQuery.of(context: context).padding.bottom
                            )
                            : new Container()
                    }
                )
            );
        }

        static Widget _buildTitle(string title) {
            if (title.isEmpty()) {
                return new Container();
            }

            return new Column(
                children: new List<Widget> {
                    new Container(
                        alignment: Alignment.center,
                        height: 54,
                        child: new Text(
                            data: title,
                            style: CTextStyle.PRegularBody4
                        )
                    ),
                    new CustomDivider(
                        height: 1,
                        color: CColors.Separator2
                    )
                }
            );
        }

        static Widget _buildButtons(List<ActionSheetItem> items) {
            if (items.isNullOrEmpty()) {
                return new Container();
            }

            List<Widget> widgets = new List<Widget>();
            List<Widget> normalWidgets = new List<Widget>();
            List<Widget> destructiveWidgets = new List<Widget>();
            List<Widget> cancelWidgets = new List<Widget>();
            items.ForEach(item => {
                Color titleColor;
                switch (item.type) {
                    case ActionType.normal:
                        titleColor = CColors.TextBody;
                        break;
                    case ActionType.cancel:
                        titleColor = CColors.Cancel;
                        break;
                    case ActionType.destructive:
                        titleColor = CColors.Error;
                        break;
                    default:
                        titleColor = CColors.TextBody;
                        break;
                }

                Widget widget = new GestureDetector(
                    onTap: () => {
                        ActionSheetUtils.hiddenModalPopup();
                        item.onTap?.Invoke();
                    },
                    child: new Container(
                        alignment: Alignment.center,
                        height: 49,
                        padding: EdgeInsets.symmetric(horizontal: 16),
                        color: CColors.White,
                        child: new Text(
                            data: item.title,
                            style: CTextStyle.PLargeBody.copyWith(color: titleColor),
                            maxLines: 1,
                            overflow: TextOverflow.ellipsis
                        )
                    )
                );
                var divider = new CustomDivider(
                    height: 1,
                    color: CColors.Separator2
                );
                if (item.type == ActionType.destructive) {
                    destructiveWidgets.Add(item: widget);
                    destructiveWidgets.Add(item: divider);
                }
                else if (item.type == ActionType.cancel) {
                    cancelWidgets.Add(new CustomDivider(height: 4, color: CColors.Separator2));
                    cancelWidgets.Add(item: widget);
                }
                else {
                    normalWidgets.Add(item: widget);
                    normalWidgets.Add(item: divider);
                }
            });
            widgets.AddRange(collection: normalWidgets);
            widgets.AddRange(collection: destructiveWidgets);
            widgets.AddRange(collection: cancelWidgets);
            if (widgets.isNotEmpty() && widgets.last() is CustomDivider) {
                widgets.removeLast();
            }

            return new Column(
                children: widgets
            );
        }
    }

    public static class ActionSheetUtils {
        public static void showModalActionSheet(
            Widget child,
            Widget overlay = null,
            VoidCallback onPop = null
        ) {
            var route = new _ModalPopupRoute(
                builder: cxt => child,
                overlayBuilder: overlay == null
                    ? (WidgetBuilder) null
                    : ctx => overlay,
                barrierLabel: "Dismiss"
            );
            Router.navigator.push(route: route).Then(_ => onPop?.Invoke());
        }

        public static void hiddenModalPopup() {
            if (Router.navigator.canPop()) {
                Router.navigator.pop();
            }
        }
    }

    class _ModalPopupRoute : PopupRoute {
        public _ModalPopupRoute(
            WidgetBuilder builder = null,
            WidgetBuilder overlayBuilder = null,
            string barrierLabel = "",
            RouteSettings settings = null
        ) : base(settings: settings) {
            this.builder = builder;
            this.barrierLabel = barrierLabel;
            this.overlayBuilder = overlayBuilder;
        }

        readonly WidgetBuilder builder;

        readonly WidgetBuilder overlayBuilder;

        public string barrierLabel { get; }

        public override Color barrierColor {
            get { return new Color(0x6604040F); }
        }

        public override bool barrierDismissible {
            get { return true; }
        }

        public override TimeSpan transitionDuration {
            get { return new TimeSpan(0, 0, 0, 0, 335); }
        }

        AnimationController _animationController;

        Animation<float> _animation;

        Tween<Offset> _offsetTween;

        Tween<float> _opacityTween;

        public override Animation<float> createAnimation() {
            D.assert(this._animation == null);
            this._animation = new CurvedAnimation(
                base.createAnimation(),
                curve: Curves.linearToEaseOut,
                reverseCurve: Curves.linearToEaseOut.flipped
            );
            this._offsetTween = new OffsetTween(
                new Offset(0, 1),
                new Offset(0, 0)
            );
            this._opacityTween = new FloatTween(0, 1);
            return this._animation;
        }

        public override Widget buildPage(BuildContext context, Animation<float> animation,
            Animation<float> secondaryAnimation) {
            return this.builder(context: context);
        }

        public override Widget buildTransitions(BuildContext context, Animation<float> animation,
            Animation<float> secondaryAnimation, Widget child) {
            Widget result = new Align(
                alignment: Alignment.bottomCenter,
                child: new FractionalTranslation(
                    translation: this._offsetTween.evaluate(animation: this._animation),
                    child: child
                )
            );

            if (this.overlayBuilder != null) {
                result = new Stack(
                    children: new List<Widget> {
                        Positioned.fill(
                            child: new Opacity(
                                opacity: this._opacityTween.evaluate(animation: this._animation),
                                child: this.overlayBuilder(context)
                            )
                        ),
                        result
                    });
            }

            return result;
        }
    }
}