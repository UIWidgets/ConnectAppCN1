using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class Bubble : StatefulWidget {
        public Bubble(
            string title = null,
            float? triangleLeft = null,
            float? triangleTop = null,
            float? contentLeft = null,
            float? contentRight = null,
            Key key = null
        ) : base(key: key) {
            this.title = title;
            this.triangleLeft = triangleLeft;
            this.triangleTop = triangleTop;
            this.contentLeft = contentLeft;
            this.contentRight = contentRight;
        }

        public readonly string title;
        public readonly float? triangleLeft;
        public readonly float? triangleTop;
        public readonly float? contentLeft;
        public readonly float? contentRight;

        public override State createState() {
            return new _BubbleState();
        }
    }

    public class _BubbleState : State<Bubble> {
        static readonly Size triangleSize = new Size(14.0f, 7.0f);
        static readonly TimeSpan duration = TimeSpan.FromMilliseconds(700);
        static readonly Curve curve = Curves.easeInOut;
        bool _isUp;

        public override void initState() {
            base.initState();
            this._isUp = false;
        }

        public override Widget build(BuildContext context) {
            if (!this._isUp) {
                Promise.Delayed(duration: duration).Then(() => {
                    if (this.mounted) {
                        this._isUp = true;
                        this.setState(() => {});
                    }
                });
            }
            else {
                Promise.Delayed(duration: duration).Then(() => {
                    if (this.mounted) {
                        this._isUp = false;
                        this.setState(() => {});
                    }
                });
            }

            var triangleTop = this._isUp ? this.widget.triangleTop - 4 : this.widget.triangleTop;
            return new Container(
                child: new Stack(
                    children: new List<Widget> {
                        new AnimatedPositioned(
                            left: this.widget.triangleLeft,
                            top: triangleTop,
                            duration: duration,
                            curve: curve,
                            child: _buildTriangle()
                        ),
                        new AnimatedPositioned(
                            left: this.widget.contentLeft,
                            right: this.widget.contentRight,
                            top: triangleTop + triangleSize.height,
                            duration: duration,
                            curve: curve,
                            child: this._buildContent()
                        )
                    }
                )
            );
        }

        static Widget _buildTriangle() {
            return SizedBox.fromSize(
                size: triangleSize,
                child: new CustomPaint(
                    painter: new TrianglePainter(
                        arrowDirection: ArrowDirection.up,
                        color: CColors.PrimaryBlue
                    )
                )
            );
        }

        Widget _buildContent() {
            return new Container(
                padding: EdgeInsets.symmetric(8, 16),
                decoration: new BoxDecoration(
                    color: CColors.PrimaryBlue,
                    borderRadius: BorderRadius.all(4)
                ),
                child: new Text(
                    data: this.widget.title,
                    style: CTextStyle.PMediumWhite.copyWith(height: 1.46f)
                )
            );
        }
    }
}