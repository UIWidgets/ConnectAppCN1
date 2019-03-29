using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components.pull_to_refresh
{
    public class SmartRefreshHeader : StatelessWidget
    {
        public readonly int mode;

        public SmartRefreshHeader(
            int mode,
            Key key=null) : base(key)
        {
            this.mode = mode;
        }

        public override Widget build(BuildContext context)
        {
            AnimatingType animatingType = AnimatingType.stop;
            if (mode == 2) {
                animatingType = AnimatingType.repeat;
            }
            if (mode == 3) {
                animatingType = AnimatingType.stop;
            }
            if (mode == 0) {
                animatingType = AnimatingType.reset;
            }
            return new Container(
                height: 44.0f,
                child:new CustomActivityIndicator(
                    animating: animatingType,
                    size:24
                )
            );
        }
    }
}