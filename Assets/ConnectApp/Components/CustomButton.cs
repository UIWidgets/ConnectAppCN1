using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class CustomButton : StatelessWidget {
        public CustomButton(
            Key key = null,
            GestureTapCallback onPressed = null,
            EdgeInsets padding = null,
            float? width = null,
            float? height = null,
            bool enable = true,
            Decoration decoration = null,
            Widget child = null
        ) : base(key: key) {
            this.onPressed = onPressed;
            this.padding = padding ?? EdgeInsets.all(8.0f);
            this.decoration = decoration ?? new BoxDecoration();
            this.width = width;
            this.height = height;
            this.enable = enable;
            this.child = child;
        }

        readonly GestureTapCallback onPressed;
        readonly EdgeInsets padding;
        readonly float? width;
        readonly float? height;
        readonly Widget child;
        readonly Decoration decoration;
        readonly bool enable;

        public override Widget build(BuildContext context) {
            return new GestureDetector(
                onTap: this.enable ? this.onPressed : null,
                child: new Container(
                    width: this.width,
                    height: this.height,
                    padding: this.padding,
                    decoration: this.decoration,
                    child: this.child
                )
            );
        }
    }
}