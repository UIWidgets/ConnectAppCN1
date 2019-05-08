using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.components {
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

        
        private readonly Widget child;
        private readonly bool top;
        private readonly bool bottom;

        
        public override Widget build(BuildContext context)
        {
            bool topValue = Application.platform != RuntimePlatform.Android;
            if (top==false)
            {
                topValue = false;
            }
            return new SafeArea(
                top: topValue,
                bottom: bottom,
                child: child
            );
        }
    }
}