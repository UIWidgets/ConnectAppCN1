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

    public static class CustomDialogUtils {
        public static void showCustomDialog(
            bool barrierDismissible = false,
            Widget child = null
        ) {
            var route = new _DialogRoute(
                (context, animation, secondaryAnimation) => child,
                barrierDismissible,
                Color.fromRGBO(0,0,0,0.01f),
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
                child: new CustomDialog(
                    new Icon(
                        iconData,
                        size: 27,
                        color:Color.fromRGBO(199,203,207,1)
                    ),
                    message,
                    TimeSpan.FromSeconds(2)
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