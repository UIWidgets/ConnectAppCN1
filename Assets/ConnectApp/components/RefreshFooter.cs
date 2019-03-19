using ConnectApp.components.refresh;
using Unity.UIWidgets.widgets;
using Unity.UIWidgets.foundation;

namespace ConnectApp.components {
    public class RefreshFooter : RefreshChild {
        public RefreshFooter(
            RefreshWidgetController controller,
            Key key = null
        ) : base(controller, key) {
        }

        public override State createState() => new _RefreshFooterState();
    }
    
    internal class _RefreshFooterState : State<RefreshFooter> {
        
        private RefreshState _state = RefreshState.drag;
        
        public override void didUpdateWidget(StatefulWidget oldWidget) {
            if (oldWidget is RefreshFooter newWidget) {
                if (widget.controller != newWidget.controller) {
                    newWidget.controller.removeStateListener(_updateState);
                    widget.controller.addStateListener(_updateState);
                }
            }
            base.didUpdateWidget(oldWidget);
        }
        
        public override void didChangeDependencies() {
            widget.controller.addStateListener(_updateState);
            base.didChangeDependencies();
        }

        public override void dispose() {
            widget.controller.removeStateListener(_updateState);
            base.dispose();
        }
        
        private void _updateState() {
            setState(() => { _state = widget.controller.state; });
        }
        
        public override Widget build(BuildContext context) {
            var animatingType = _state == RefreshState.loading ? AnimatingType.repeat : AnimatingType.stop;
            return new Container(
                child: new CustomActivityIndicator(
                    animating: animatingType,
                    size: 32
                )
            );
        }
    }
}