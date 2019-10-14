using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Components {
    public class CustomSafeArea : StatelessWidget {
        public CustomSafeArea(
            Key key = null,
            bool top = true,
            bool bottom = true,
            Widget child = null
        ) : base(key: key) {
            D.assert(child != null);
            this.child = child;
            this.top = top;
            this.bottom = bottom;
        }

        readonly Widget child;
        readonly bool top;
        readonly bool bottom;

        public override Widget build(BuildContext context) {
            bool topValue = Application.platform != RuntimePlatform.Android;
            if (this.top == false) {
                topValue = false;
            }

            return new SafeArea(
                top: topValue,
                bottom: this.bottom,
                child: this.child
            );
        }
    }
}