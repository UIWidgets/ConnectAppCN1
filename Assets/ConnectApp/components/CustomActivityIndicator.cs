using System;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public enum AnimatingType {
        repeat,
        stop,
        reset
    }

    public enum AnimationImage {
        white,
        black
    }

    public class CustomActivityIndicator : StatefulWidget {
        public CustomActivityIndicator(
            Key key = null,
            AnimatingType animating = AnimatingType.repeat,
            AnimationImage animationImage = AnimationImage.black,
            float size = 24
        ) : base(key) {
            this.animating = animating;
            this.animationImage = animationImage;
            this.size = size;
        }

        public readonly AnimatingType animating;
        public readonly AnimationImage animationImage;
        public readonly float size;

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
            Ticker _ticker = new Ticker(onTick, $"created by {this}");
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
            var imageName = widget.animationImage == AnimationImage.white ? "white-loading" : "black-loading";
            return new RotationTransition(
                turns: _controller,
                child: new Column(mainAxisAlignment: MainAxisAlignment.center,
                    children: new List<Widget> {
                        Image.asset(imageName, width: widget.size, height: widget.size)
                    })
            );
        }

        public override void dispose() {
            base.dispose();
            _controller.dispose();
        }
    }
}