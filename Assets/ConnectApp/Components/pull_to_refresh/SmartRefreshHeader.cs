using System;
using ConnectApp.constants;
using Unity.UIWidgets.animation;
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
                height: 56.0f,
                child: new CustomActivityIndicator(
                    animating: animatingType
                )
            );
        }
        
        Widget _buildOther() {
            CrossFadeState? crossFadeState = null;
            string refreshText = "";
            if (this.mode == 0 || this.mode == 1) {
                refreshText = "探索新鲜内容";
                crossFadeState = CrossFadeState.showFirst;
            }
            if (this.mode == 2) {
                crossFadeState = CrossFadeState.showSecond;
            }
            if (this.mode == 3) {
                refreshText = "刷新成功";
                crossFadeState = CrossFadeState.showFirst;
            }
            if (this.mode == 4) {
                refreshText = "刷新失败";
                crossFadeState = CrossFadeState.showFirst;
            }
            Widget child = new AnimatedCrossFade(
                firstChild: _buildText(text: refreshText),
                secondChild: Image.asset(
                    "image/loading.gif",
                    width: 235,
                    height: 40
                ),
                firstCurve: Curves.easeIn,
                duration: TimeSpan.FromMilliseconds(400),
                crossFadeState: crossFadeState
            );
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