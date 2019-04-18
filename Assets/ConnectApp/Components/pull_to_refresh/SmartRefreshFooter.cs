using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components.pull_to_refresh {
    public class SmartRefreshFooter : StatelessWidget {
        private readonly int mode;
        private readonly EdgeInsets padding;

        public SmartRefreshFooter(
            int mode,
            EdgeInsets padding = null,
            Key key = null
        ) : base(key) {
            this.mode = mode;
            this.padding = padding ?? EdgeInsets.symmetric(16.0f);
        }

        public override Widget build(BuildContext context) {
            if (mode == 0) return new Container(height:DefaultConstants.default_VisibleRange);
            AnimatingType animatingType = AnimatingType.stop;
            if (mode == 2) animatingType = AnimatingType.repeat;
            if (mode == 3) animatingType = AnimatingType.stop;
            if (mode == 0) animatingType = AnimatingType.reset;
            
            return new Container(
                padding: padding,
                child:  new CustomActivityIndicator(
                    animating: animatingType
                )
            );
        }
    }
}