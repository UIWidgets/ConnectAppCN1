using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class CustomButton : StatelessWidget {
        public CustomButton(
            Key key = null,
            GestureTapCallback onPressed = null,
            EdgeInsets padding = null,
            Color backgroundColor = null,
            Widget child = null
        ) : base(key) {
            this.onPressed = onPressed;
            this.padding = padding ?? EdgeInsets.all(8.0);
            this.backgroundColor = backgroundColor;
            this.child = child;
        }

        public readonly GestureTapCallback onPressed;
        public readonly EdgeInsets padding;
        public readonly Widget child;
        public readonly Color backgroundColor;

        public override Widget build(BuildContext context) {
            return new GestureDetector(
                onTap: onPressed,
                child: new Container(
                    padding: padding,
                    decoration: new BoxDecoration(backgroundColor),
                    child: child
                )
            );
        }
    }
}