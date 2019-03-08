using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using DialogUtils = Unity.UIWidgets.material.DialogUtils;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;
using RSG;

namespace ConnectApp.components {
    
    public class CustomDialog : StatelessWidget {

        public CustomDialog(
            Key key = null,
            Widget widget = null,
            string message = null
        ) : base(key) {
            this.widget = widget;
            this.message = message;
        }

        public readonly Widget widget;
        public readonly string message;

        public override Widget build(BuildContext context) {
            return new GestureDetector(
                onTap: () => { Navigator.pop(context); },
                child: new Container(
                    color: Color.fromRGBO(0, 0, 0, 0.1f),
                    child: new Center(
                        child: new Container(
                            width: 120,
                            height: 107,
                            decoration: new BoxDecoration(
                                Color.fromRGBO(0, 0, 0, 0.8f),
                                borderRadius: BorderRadius.circular(3.0f)
                            ),
                            child: new Column(
                                mainAxisSize: MainAxisSize.min,
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: new List<Widget> {
                                    widget != null ? widget : new CustomActivityIndicator(),
                                    _buildMessage(message)
                                }
                            )
                        )
                    )
                )
            );
        }
        
        private static Widget _buildMessage(string message) {
            if (message == null || message.Length <= 0) {
                return new Container();
            }
            return new Container(
                margin: EdgeInsets.only(top: 8, left: 8, right: 8),
                child: new Text(
                    message,
                    style: new TextStyle(
                        color: Color.fromRGBO(216, 216, 216, 1),
                        fontSize: 14.0f,
                        decoration: TextDecoration.none,
                        fontWeight: FontWeight.w400
                    ),
                    textAlign: TextAlign.center
                )
            );
        }
    }
    
    public static class CustomDialogUtils {
        public static IPromise<object> showCustomDialog(
            BuildContext context,
            bool barrierDismissible = false,
            Widget child = null
        ) {
            return DialogUtils.showDialog(
                context,
                barrierDismissible,
                cxt => child
            );
        }

        public static bool hiddenCustomDialog(BuildContext context) {
            return Navigator.pop(context);
        }
    }
}