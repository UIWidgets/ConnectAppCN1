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
            this.padding = padding ?? EdgeInsets.all(8.0f);
            this.backgroundColor = backgroundColor;
            this.child = child;
        }

        readonly GestureTapCallback onPressed;
        readonly EdgeInsets padding;
        readonly Widget child;
        readonly Color backgroundColor;

        public override Widget build(BuildContext context) {
            return new GestureDetector(
                onTap: this.onPressed,
                child: new Container(
                    padding: this.padding,
                    decoration: new BoxDecoration(this.backgroundColor),
                    child: this.child
                )
            );
        }
    }
}