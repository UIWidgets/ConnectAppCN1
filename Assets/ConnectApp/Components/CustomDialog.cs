using System;
using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.constants;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class CustomDialog : StatelessWidget {
        public CustomDialog(
            Key key = null,
            Color backgroundColor = null,
            TimeSpan? insetAnimationDuration = null,
            Curve insetAnimationCurve = null,
            float? radius = null,
            Widget child = null
        ) : base(key) {
            this.backgroundColor = backgroundColor;
            this.insetAnimationDuration = insetAnimationDuration ?? new TimeSpan(0, 0, 0, 0, 100);
            this.insetAnimationCurve = insetAnimationCurve ?? Curves.decelerate;
            this.radius = radius ?? 0;
            this.child = child;
        }

        private readonly Color backgroundColor;
        private readonly TimeSpan insetAnimationDuration;
        private readonly Curve insetAnimationCurve;
        private readonly float? radius;
        private readonly Widget child;

        public override Widget build(BuildContext context) {
            return new AnimatedPadding(
                padding: MediaQuery.of(context).viewInsets + EdgeInsets.symmetric(horizontal: 40.0f, vertical: 24.0f),
                duration: insetAnimationDuration,
                curve: insetAnimationCurve,
                child: MediaQuery.removeViewInsets(
                    removeLeft: true,
                    removeTop: true,
                    removeRight: true,
                    removeBottom: true,
                    context: context,
                    child: new Center(
                        child: new ConstrainedBox(
                            constraints: new BoxConstraints(280.0f),
                            child: new Container(
                                decoration: new BoxDecoration(
                                    backgroundColor,
                                    borderRadius: BorderRadius.all((float)radius)
                                ),
                                child: child
                            )
                        )
                    )
                )
            );
        }
    }
    
    public class CustomLoadingDialog : StatelessWidget {
        public CustomLoadingDialog(
            Widget widget = null,
            string message = null,
            TimeSpan? duration = null,
            Key key = null
        ) : base(key) {
            this.widget = widget;
            this.message = message;
            this.duration = duration;
        }

        private readonly Widget widget;
        private readonly string message;
        private readonly TimeSpan? duration;

        public override Widget build(BuildContext context) {
            if (duration != null)
                Promise.Delayed((TimeSpan) duration)
                    .Then(CustomDialogUtils.hiddenCustomDialog);
            return new GestureDetector(
                onTap: () => { },
                child: new Container(
                    child: new Center(
                        child: new Container(
                            width: 132,
                            height: 110,
                            decoration: new BoxDecoration(
                                Color.fromRGBO(0, 0, 0, 0.8f),
                                borderRadius: BorderRadius.circular(4)
                            ),
                            child: new Column(
                                mainAxisSize: MainAxisSize.min,
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: new List<Widget> {
                                    widget != null
                                        ? widget
                                        : new CustomActivityIndicator(loadingColor: LoadingColor.white),
                                    _buildMessage(message)
                                }
                            )
                        )
                    )
                )
            );
        }

        private static Widget _buildMessage(string message) {
            if (message == null || message.Length <= 0) return new Container();
            return new Container(
                margin: EdgeInsets.only(top: 8, left: 8, right: 8),
                child: new Text(
                    message,
                    style: CTextStyle.PRegularWhite,
                    textAlign: TextAlign.center
                )
            );
        }
    }

    public class CustomAlertDialog : StatelessWidget {
        public CustomAlertDialog(
            string title = null,
            string message = null,
            List<Widget> actions = null,
            Key key = null
        ) : base(key) {
            this.title = title;
            this.message = message;
            this.actions = actions;
        }

        private readonly string title;
        private readonly string message;
        private readonly List<Widget> actions;

        public override Widget build(BuildContext context) {
            var children = new List<Widget>();
            if (title.isNotEmpty()) {
                children.Add(new Container(
                    padding: EdgeInsets.fromLTRB(16, 24, 16, message == null ? 24 : 0),
                    alignment: Alignment.center,
                    child: new Text(
                        title,
                        style: CTextStyle.H5
                    )
                ));
            }
            
            if (message.isNotEmpty()) {
                children.Add(new Container(
                    padding: EdgeInsets.all(16),
                    alignment: Alignment.center,
                    child: new Text(
                        message,
                        style: CTextStyle.PLargeBody
                    )
                ));
            }

            if (actions != null) {
                children.Add(new CustomDivider(
                    height: 1
                ));
                
                var _children = new List<Widget>();
                foreach (var _child in actions) {
                    _children.Add(new Expanded(
                        child: new Stack(
                            fit: StackFit.expand,
                            children: new List<Widget> {
                                _child
                            }
                        )
                    ));
                    var index = actions.IndexOf(_child);
                    if (index < actions.Count - 1) {
                        _children.Add(new Container(
                            width: 1,
                            color: CColors.Separator
                        ));
                    }
                }
                Widget child = new Container(
                    height: 48,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: _children
                    )
                );
                children.Add(child);
            }

            Widget dialogChild = new IntrinsicWidth(
                child: new Column(
                    mainAxisSize: MainAxisSize.min,
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: children
                )
            );

            return new CustomDialog(
                backgroundColor: CColors.White,
                radius: 5,
                child: dialogChild
            );
        }
    }

    public static class CustomDialogUtils {
        public static void showCustomDialog(
            bool barrierDismissible = false,
            Color barrierColor = null,
            Widget child = null
        ) {
            var route = new _DialogRoute(
                (context, animation, secondaryAnimation) => new CustomSafeArea(child: child), 
                barrierDismissible,
                barrierColor ?? Color.fromRGBO(0, 0, 0, 0.01f),
                new TimeSpan(0, 0, 0, 0, 150),
                _transitionBuilder
            );
            Router.navigator.push(route);
        }

        public static void hiddenCustomDialog() {
            if (Router.navigator.canPop()) Router.navigator.pop();
        }

        public static void showToast(string message, IconData iconData) {
            showCustomDialog(
                child: new CustomLoadingDialog(
                    new Icon(
                        iconData,
                        size: 27,
                        color: Color.fromRGBO(199, 203, 207, 1)
                    ),
                    message,
                    TimeSpan.FromSeconds(1)
                )
            );
        }

        private static Widget _transitionBuilder(BuildContext context, Animation<float> animation,
            Animation<float> secondaryAnimation, Widget child) {
            return new FadeTransition(
                opacity: new CurvedAnimation(
                    animation,
                    Curves.easeOut
                ),
                child: child
            );
        }
    }

    internal class _DialogRoute : PopupRoute {
        public _DialogRoute(
            RoutePageBuilder pageBuilder = null,
            bool barrierDismissible = true,
            Color barrierColor = null,
            TimeSpan? transitionDuration = null,
            RouteTransitionsBuilder transitionBuilder = null,
            RouteSettings setting = null
        ) : base(setting) {
            this.pageBuilder = pageBuilder;
            this.barrierDismissible = barrierDismissible;
            this.barrierColor = barrierColor ?? new Color(0x80000000);
            this.transitionDuration = transitionDuration ?? TimeSpan.FromMilliseconds(200);
            this.transitionBuilder = transitionBuilder;
        }

        private readonly RoutePageBuilder pageBuilder;

        public override bool barrierDismissible { get; }

        public override Color barrierColor { get; }

        public override TimeSpan transitionDuration { get; }

        private readonly RouteTransitionsBuilder transitionBuilder;

        public override Widget buildPage(BuildContext context, Animation<float> animation,
            Animation<float> secondaryAnimation) {
            return pageBuilder(context, animation, secondaryAnimation);
        }

        public override Widget buildTransitions(BuildContext context, Animation<float> animation,
            Animation<float> secondaryAnimation, Widget child) {
            if (transitionBuilder == null)
                return new FadeTransition(
                    opacity: new CurvedAnimation(
                        animation,
                        Curves.linear
                    ),
                    child: child
                );
            return transitionBuilder(context, animation, secondaryAnimation, child);
        }
    }
}