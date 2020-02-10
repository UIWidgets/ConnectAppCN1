using System;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class RotationAnimation : StatefulWidget {
        public RotationAnimation(
            Key key = null,
            Widget child = null,
            float? degrees = null,
            TimeSpan? duration = null,
            AnimatingType animating = AnimatingType.repeat
        ) : base(key: key) {
            D.assert(child != null);
            this.child = child;
            this.degrees = degrees;
            this.duration = duration ?? TimeSpan.FromMilliseconds(600);
            this.animating = animating;
        }

        public readonly Widget child;
        public readonly float? degrees;
        public readonly TimeSpan duration;
        public readonly AnimatingType animating;

        public override State createState() {
            return new _RotationAnimationState();
        }
    }

    class _RotationAnimationState : SingleTickerProviderStateMixin<RotationAnimation> {
        AnimationController _rotationController;
        Animation<float> _rotationAnimation;

        public override void initState() {
            base.initState();
            this._rotationController = new AnimationController(duration: this.widget.duration, vsync: this);
            if (this.widget.degrees != null) {
                this._rotationAnimation =
                    new FloatTween(0, (float) this.widget.degrees).animate(parent: this._rotationController);
            }

            this._startAnimating();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget: oldWidget);
            var _oldWidget = (RotationAnimation) oldWidget;
            if (this.widget.animating != _oldWidget.animating) {
                this._startAnimating();
            }
        }

        public override void dispose() {
            this._rotationController.dispose();
            base.dispose();
        }

        void _startAnimating() {
            switch (this.widget.animating) {
                case AnimatingType.repeat: {
                    this._rotationController.setValue(0);
                    this._rotationController.repeat();
                    break;
                }
                case AnimatingType.stop:
                    this._rotationController.stop();
                    break;
                case AnimatingType.reset:
                    this._rotationController.reset();
                    break;
                case AnimatingType.forward: {
                    this._rotationController.forward();
                    break;
                }
            }
        }

        public override Widget build(BuildContext context) {
            return new RotationTransition(
                turns: this.widget.degrees != null ? this._rotationAnimation : this._rotationController,
                child: this.widget.child
            );
        }
    }
}