using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.components {
    public class CustomSafeArea : StatelessWidget {
        
        public CustomSafeArea(
            Key key = null,
            Widget child = null
        ) : base(key: key) {
            D.assert(child != null);
            this.child = child;
        }

        private readonly Widget child;
        
        public override Widget build(BuildContext context) {
            return new SafeArea(
                top: Application.platform != RuntimePlatform.Android,
                child: child
            );
        }
    }
}