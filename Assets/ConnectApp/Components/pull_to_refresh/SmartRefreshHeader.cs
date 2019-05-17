using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.components.pull_to_refresh {
    public enum RefreshHeaderType {
        activityIndicator,
        other
    }
    
    public class SmartRefreshHeader : StatelessWidget {
        readonly int mode;
        readonly RefreshHeaderType type;

        public SmartRefreshHeader(
            int mode,
            RefreshHeaderType type = RefreshHeaderType.activityIndicator,
            Key key = null
        ) : base(key: key) {
            this.mode = mode;
            this.type = type;
        }

        public override Widget build(BuildContext context) {
            switch (this.type) {
                case RefreshHeaderType.activityIndicator:
                    return this._buildActivityIndicator();
                case RefreshHeaderType.other:
                    return this._buildOther();
                default:
                    return new Container();
            }
        }

        Widget _buildActivityIndicator() {
            AnimatingType animatingType = AnimatingType.stop;
            if (this.mode == 0) {
                animatingType = AnimatingType.reset;
            }
            if (this.mode == 2) {
                animatingType = AnimatingType.repeat;
            }
            if (this.mode == 3) {
                animatingType = AnimatingType.stop;
            }
            return new Container(
                height: 56.0f,
                child: new CustomActivityIndicator(
                    animating: animatingType
                )
            );
        }
        
        Widget _buildOther() {
            Widget child = new Container();
            if (this.mode == 0 || this.mode == 1) {
                child = _buildText("探索新鲜内容");
            }
            if (this.mode == 2) {
                child = Image.asset(
                    "image/loading.gif",
                    width: 235,
                    height: 40
                );
            }
            if (this.mode == 3) {
                child = _buildText("刷新成功");
            }
            if (this.mode == 4) {
                child = _buildText("刷新失败");
            }
            return new Container(
                height: 56.0f,
                alignment: Alignment.center,
                child: child
            );
        }

        static Widget _buildText(string text) {
            return new Text(
                data: text,
                style: new TextStyle(
                    height: 1.33f,
                    fontSize: 16,
                    fontFamily: "Roboto-Medium",
                    color: CColors.BrownGrey
                )
            );
        }
    }
}