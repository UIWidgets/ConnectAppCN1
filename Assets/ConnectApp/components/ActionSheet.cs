using System;
using System.Collections.Generic;
using ConnectApp.constants;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
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
        ) : base(key) {
            this.title = title;
            this.items = items;
        }

        public readonly string title;
        public readonly List<ActionSheetItem> items;

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    mainAxisSize: MainAxisSize.min,
                    children: new List<Widget> {
                        _buildTitle(title),
                        _buildButtons(context, items),
                        new Container(
                            height: MediaQuery.of(context).padding.bottom
                        )
                    }
                )
            );
        }

        private static Widget _buildTitle(string title) {
            if (title == null || title.Length <= 0) return new Container();
            return new Column(
                children: new List<Widget> {
                    new Container(
                        alignment: Alignment.center,
                        height: 100.0f,
                        child: new Text(
                            title,
                            style: new TextStyle(
                                color: Color.fromRGBO(171, 170, 174, 1),
                                fontSize: 16.0f,
                                decoration: TextDecoration.none,
                                fontWeight: FontWeight.w400
                            )
                        )
                    ),
                    new CustomDivider(
                        height: 1.0f
                    )
                }
            );
        }

        private static Widget _buildButtons(BuildContext context, List<ActionSheetItem> items) {
            if (items == null || items.Count <= 0) return new Container();
            List<Widget> widgets = new List<Widget>();
            List<Widget> normalWidgets = new List<Widget>();
            List<Widget> destructiveWidgets = new List<Widget>();
            List<Widget> cancelWidgets = new List<Widget>();
            items.ForEach(item => {
                ActionType type = item.type;
                Color titleColor = Color.fromRGBO(40, 187, 40, 1);
                if (type == ActionType.destructive) titleColor = Color.fromRGBO(240, 81, 60, 1);

                Widget widget = new Column(
                    children: new List<Widget> {
                        new GestureDetector(
                            onTap: () => {
                                Navigator.pop(context);
                                item.onTap();
                            },
                            child: new Container(
                                alignment: Alignment.center,
                                height: 49.0f,
                                color: CColors.White,
                                child: new Text(
                                    item.title,
                                    style: new TextStyle(
                                        color: titleColor,
                                        fontSize: 16.0f,
                                        decoration: TextDecoration.none,
                                        fontWeight: FontWeight.w400
                                    )
                                )
                            )
                        ),
                        new CustomDivider(
                            height: 1.0f
                        )
                    }
                );
                if (type == ActionType.destructive)
                    destructiveWidgets.Add(widget);
                else if (type == ActionType.cancel)
                    cancelWidgets.Add(widget);
                else
                    normalWidgets.Add(widget);
            });
            widgets.AddRange(normalWidgets);
            widgets.AddRange(destructiveWidgets);
            widgets.AddRange(cancelWidgets);
            return new Column(
                children: widgets
            );
        }
    }

    public static class ActionSheetUtils {
        public static IPromise<object> showModalActionSheet(
            BuildContext context,
            Widget child
        ) {
            return Navigator.of(context, true).push(
                new _ModalPopupRoute(
                    cxt => child,
                    "Dismiss"
                )
            );
        }

        private static bool _hiddenModalPopup(BuildContext context) {
            return Navigator.pop(context);
        }
    }

    internal class _ModalPopupRoute : PopupRoute {
        public _ModalPopupRoute(
            WidgetBuilder builder = null,
            string barrierLabel = "",
            RouteSettings settings = null
        ) : base(settings) {
            this.builder = builder;
            this.barrierLabel = barrierLabel;
        }

        private readonly WidgetBuilder builder;

        public string barrierLabel { get; }

        public override Color barrierColor => new Color(0x6604040F);

        public override bool barrierDismissible => true;

        public override TimeSpan transitionDuration => new TimeSpan(0, 0, 0, 0, 335);

        private AnimationController _animationController;

        private Animation<float> _animation;

        private Tween<Offset> _offsetTween;

        public override Animation<float> createAnimation() {
            D.assert(_animation == null);
            _animation = new CurvedAnimation(
                base.createAnimation(),
                Curves.ease,
                Curves.ease.flipped
            );
            _offsetTween = new OffsetTween(
                new Offset(0, 1),
                new Offset(0, 0)
            );
            return _animation;
        }

        public override Widget buildPage(BuildContext context, Animation<float> animation,
            Animation<float> secondaryAnimation) {
            return builder(context);
        }

        public override Widget buildTransitions(BuildContext context, Animation<float> animation,
            Animation<float> secondaryAnimation, Widget child) {
            return new Align(
                alignment: Alignment.bottomCenter,
                child: new FractionalTranslation(
                    translation: _offsetTween.evaluate(_animation),
                    child: child
                )
            );
        }
    }
}