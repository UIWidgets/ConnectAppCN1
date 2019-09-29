using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public abstract class SlideAction : StatelessWidget {
        protected SlideAction(
            float width = 80,
            EdgeInsets padding = null,
            GestureTapCallback onTap = null,
            bool closeOnTap = true,
            Key key = null
        ) : base(key: key) {
            this.width = width;
            this.padding = padding;
            this.onTap = onTap;
            this.closeOnTap = closeOnTap;
        }

        readonly float width;
        readonly EdgeInsets padding;
        readonly GestureTapCallback onTap;
        readonly bool closeOnTap;

        void _handleCloseAfterTap(BuildContext context) {
            this.onTap();
            CustomDismissible.of(context: context)?.close();
        }

        public override Widget build(BuildContext context) {
            return new GestureDetector(
                onTap: !this.closeOnTap ? this.onTap : () => this._handleCloseAfterTap(context: context),
                child: new Container(
                    color: CColors.Separator2,
                    width: this.width,
                    alignment: Alignment.center,
                    padding: this.padding,
                    child: new Container(
                        width: 44,
                        height: 44,
                        alignment: Alignment.center,
                        decoration: new BoxDecoration(
                            color: CColors.White,
                            borderRadius: BorderRadius.circular(22)
                        ),
                        child: this.buildAction(context: context)
                    )
                )
            );
        }

        protected abstract Widget buildAction(BuildContext context);
    }

    public class DeleteActionButton : SlideAction {
        public DeleteActionButton(
            float width = 80,
            EdgeInsets padding = null,
            GestureTapCallback onTap = null,
            Key key = null
        ) : base(width: width, padding: padding, onTap: onTap, key: key) {
        }

        protected override Widget buildAction(BuildContext context) {
            return new Icon(icon: Icons.delete, size: 28, color: CColors.Error);
        }
    }

    public class EditActionButton : SlideAction {
        public EditActionButton(
            float width = 80,
            EdgeInsets padding = null,
            GestureTapCallback onTap = null,
            Key key = null
        ) : base(width: width, padding: padding, onTap: onTap, key: key) {
        }

        protected override Widget buildAction(BuildContext context) {
            return new Icon(icon: Icons.edit, size: 28, color: CColors.PrimaryBlue);
        }
    }
}