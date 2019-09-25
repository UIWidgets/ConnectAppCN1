using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.pull_to_refresh {
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
            CrossFadeState? crossFadeState;
            string refreshText;
            AnimatingType type;
            switch (this.mode) {
                case 0: {
                    refreshText = "探索新鲜内容";
                    type = AnimatingType.stop;
                    crossFadeState = CrossFadeState.showFirst;
                    break;
                }
                case 1: {
                    refreshText = "探索新鲜内容";
                    type = AnimatingType.stop;
                    crossFadeState = CrossFadeState.showFirst;
                    break;
                }
                case 2: {
                    refreshText = "";
                    type = AnimatingType.repeat;
                    crossFadeState = CrossFadeState.showSecond;
                    break;
                }
                case 3: {
                    refreshText = "刷新成功";
                    type = AnimatingType.stop;
                    crossFadeState = CrossFadeState.showFirst;
                    break;
                }
                case 4: {
                    refreshText = "刷新失败";
                    type = AnimatingType.stop;
                    crossFadeState = CrossFadeState.showFirst;
                    break;
                }
                default: {
                    refreshText = "";
                    type = AnimatingType.stop;
                    crossFadeState = null;
                    break;
                }
            }

            List<string> images = new List<string>();
            for (int index = 0; index < 237; index++) {
                images.Add($"image/refresh-loading/refresh-loading{index}");
            }

            Widget child = new AnimatedCrossFade(
                firstChild: _buildText(text: refreshText),
                secondChild: new FrameAnimationImage(
                    images: images,
                    type: type
                ),
                duration: TimeSpan.FromMilliseconds(300),
                crossFadeState: crossFadeState,
                alignment: Alignment.center,
                layoutBuilder: _layoutBuilder
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

        static Widget _layoutBuilder(Widget topChild, Key topChildKey, Widget bottomChild, Key bottomChildKey) {
            return new Stack(
                overflow: Overflow.visible,
                children: new List<Widget> {
                    new Positioned(
                        key: bottomChildKey,
                        left: 0.0f,
                        top: 0.0f,
                        right: 0.0f,
                        bottom: 0.0f,
                        child: bottomChild),
                    new Positioned(
                        key: topChildKey,
                        child: topChild)
                }
            );
        }
    }
}