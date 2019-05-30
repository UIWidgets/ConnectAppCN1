using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.pull_to_refresh {
    public class SmartRefreshFooter : StatelessWidget {
        readonly int mode;
        readonly EdgeInsets padding;

        public SmartRefreshFooter(
            int mode,
            EdgeInsets padding = null,
            Key key = null
        ) : base(key: key) {
            this.mode = mode;
            this.padding = padding ?? EdgeInsets.symmetric(16.0f);
        }

        public override Widget build(BuildContext context) {
            if (this.mode == RefreshStatus.idle) {
                return new Container(height: DefaultConstants.default_VisibleRange);
            }

            AnimatingType animatingType;
            switch (this.mode) {
                case RefreshStatus.idle:
                    animatingType = AnimatingType.reset;
                    break;
                case RefreshStatus.refreshing:
                    animatingType = AnimatingType.repeat;
                    break;
                case RefreshStatus.completed:
                    animatingType = AnimatingType.stop;
                    break;
                default:
                    animatingType = AnimatingType.stop;
                    break;
            }

            return new Container(
                padding: this.padding,
                child: new CustomActivityIndicator(
                    animating: animatingType
                )
            );
        }
    }
}