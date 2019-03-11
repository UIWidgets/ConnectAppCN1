using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class CustomDivider : StatelessWidget {
        public CustomDivider(
            Key key = null,
            Color color = null,
            float height = 16.0f
        ) : base(key) {
            D.assert(height >= 0.0);
            this.color = color ?? CColors.Black;
            this.height = height;
        }

        private readonly Color color;
        private readonly float height;

        public override Widget build(BuildContext context) {
            return new Container(
                decoration: new BoxDecoration(color),
                height: height
            );
        }
    }
}