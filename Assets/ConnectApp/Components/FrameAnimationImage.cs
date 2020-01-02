using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class FrameAnimationImage : StatefulWidget {
        public FrameAnimationImage(
            List<string> images,
            float size = 56,
            float duration = 17,
            AnimatingType type = AnimatingType.repeat,
            Widget defaultWidget = null,
            Key key = null
        ) : base(key: key) {
            D.assert(images != null && images.Count > 0);
            this.images = images;
            this.size = size;
            this.duration = duration;
            this.type = type;
            this.defaultWidget = defaultWidget;
        }

        public readonly List<string> images;
        public readonly float size;
        public readonly float duration;
        public readonly AnimatingType type;
        public readonly Widget defaultWidget;

        public override State createState() {
            return new _FrameAnimationImageState();
        }
    }

    class _FrameAnimationImageState : State<FrameAnimationImage>, TickerProvider {
        AnimationController _controller;
        Animation<int> _animation;

        public override void initState() {
            base.initState();
            this._controller = new AnimationController(
                duration: TimeSpan.FromMilliseconds(this.widget.duration * this.widget.images.Count),
                vsync: this
            );
            this._animation = new IntTween(0, this.widget.images.Count - 1).animate(parent: this._controller);

            this._startAnimating();
        }

        public override void dispose() {
            this._controller.dispose();
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick: onTick, () => $"created by {this}");
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget: oldWidget);
            if (oldWidget is FrameAnimationImage frameAnimationImage) {
                if (this.widget.type != frameAnimationImage.type) {
                    this._startAnimating();
                }
                if (!this.widget.images.equalsList(list: frameAnimationImage.images)) {
                    this._startAnimating();
                }
            }
        }

        void _startAnimating() {
            switch (this.widget.type) {
                case AnimatingType.repeat: {
                    this._controller.setValue(0);
                    this._controller.repeat();
                    break;
                }
                case AnimatingType.stop:
                    this._controller.stop();
                    break;
                case AnimatingType.reset:
                    this._controller.reset();
                    break;
                case AnimatingType.forward: {
                    this._controller.forward(0);
                    break;
                }
            }
        }

        public override Widget build(BuildContext context) {
            return new AnimatedBuilder(
                animation: this._controller,
                builder: (cxt, widget) => {
                    var value = this._animation.value;
                    if (value + 1 == this.widget.images.Count && this.widget.defaultWidget != null) {
                        return this.widget.defaultWidget;
                    }

                    return Image.asset(
                        this.widget.images[index: value],
                        width: this.widget.size,
                        height: this.widget.size,
                        fit: BoxFit.fill,
                        gaplessPlayback: true
                    );
                }
            );
        }
    }
}