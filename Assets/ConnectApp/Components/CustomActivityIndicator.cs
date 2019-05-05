using System;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public enum AnimatingType {
        repeat,
        stop,
        reset
    }

    public enum LoadingColor {
        white,
        black
    }

    public enum LoadingSize {
        normal,
        small
    }

    public class CustomActivityIndicator : StatefulWidget {
        public CustomActivityIndicator(
            Key key = null,
            AnimatingType animating = AnimatingType.repeat,
            LoadingColor loadingColor = LoadingColor.black,
            LoadingSize size = LoadingSize.normal
        ) : base(key) {
            this.animating = animating;
            this.loadingColor = loadingColor;
            this.size = size;
        }

        public readonly AnimatingType animating;
        public readonly LoadingColor loadingColor;
        public readonly LoadingSize size;

        public override State createState() {
            return new _CustomActivityIndicatorState();
        }
    }

    public class _CustomActivityIndicatorState : State<CustomActivityIndicator>, TickerProvider {
        private AnimationController _controller;


        public override void initState() {
            base.initState();

            _controller = new AnimationController(
                duration: new TimeSpan(0, 0, 2),
                vsync: this
            );
            if (widget.animating == AnimatingType.repeat) _controller.repeat();
        }

        public Ticker createTicker(TickerCallback onTick) {
            Ticker _ticker = new Ticker(onTick, () => $"created by {this}");
            return _ticker;
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            if (oldWidget is CustomActivityIndicator) {
                CustomActivityIndicator customActivityIndicator = (CustomActivityIndicator) oldWidget;
                if (widget.animating != customActivityIndicator.animating) {
                    if (widget.animating == AnimatingType.repeat)
                        _controller.repeat();
                    else if (widget.animating == AnimatingType.stop)
                        _controller.stop();
                    else if (widget.animating == AnimatingType.reset) _controller.reset();
                }
            }
        }

        public override Widget build(BuildContext context) {
            string imageName;
            int sideLength;
            if (widget.size == LoadingSize.normal) {
                sideLength = 24;
                imageName = widget.loadingColor == LoadingColor.white ? "white-loading24" : "black-loading24";
            }
            else {
                sideLength = 20;
                imageName = widget.loadingColor == LoadingColor.white ? "white-loading20" : "black-loading20";
            }

            return new RotationTransition(
                turns: _controller,
                child: new Center(
                    child: Image.asset(
                        imageName,
                        width: sideLength,
                        height: sideLength
                    )
                )
            );
        }

        public override void dispose() {
            _controller.dispose();
            base.dispose();
        }
    }
}