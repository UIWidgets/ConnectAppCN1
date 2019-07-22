using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class CustomDivider : StatelessWidget {
        public CustomDivider(
            Key key = null,
            Color color = null,
            float height = 16.0f
        ) : base(key: key) {
            D.assert(height >= 0.0);
            this.color = color ?? CColors.Separator;
            this.height = height;
        }

        readonly Color color;
        readonly float height;

        public override Widget build(BuildContext context) {
            return new Container(
                color: this.color,
                height: this.height
            );
        }
    }
}