using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public delegate void ChangeCallback(float relative);

    public class ProgressBar : StatefulWidget {
        public ProgressBar(
            float relative,
            VoidCallback onDragStart = null,
            ChangeCallback changeCallback = null,
            ProgressColors colors = null,
            Key key = null
        ) : base(key) {
            this.relative = relative;
            this.colors = colors ?? new ProgressColors();
            this.onDragStart = onDragStart;
            this.changeCallback = changeCallback;
        }

        public override State createState() {
            return new _ProgressBarState();
        }

        public float relative;
        public readonly ProgressColors colors;
        public readonly VoidCallback onDragStart;
        public readonly ChangeCallback changeCallback;
    }

    class _ProgressBarState : State<ProgressBar> {
        public override Widget build(BuildContext context) {
            float seekToRelativePosition(Offset globalPosition) {
                var box = (RenderBox) context.findRenderObject();
                var tapPos = box.globalToLocal(globalPosition);
                var relative = tapPos.dx / box.size.width;
                relative = relative > 1 ? 1 : relative;
                relative = relative < 0 ? 0 : relative;
                return relative;
            }

            return new GestureDetector(
                child: new Container(
                    height: MediaQuery.of(context).size.height,
                    width: MediaQuery.of(context).size.width,
                    color: CColors.Transparent,
                    child: new CustomPaint(
                        painter: new _ProgressBarPainter(this.widget.relative, this.widget.colors
                        )
                    )
                ),
                onHorizontalDragStart: (DragStartDetails details) => { this.widget.onDragStart(); },
                onHorizontalDragUpdate: (DragUpdateDetails details) => {
                    this.widget.relative = seekToRelativePosition(details.globalPosition);
                    this.setState(() => { });
                    this.widget.changeCallback(this.widget.relative);
                },
                onHorizontalDragEnd: (DragEndDetails details) => { this.setState(() => { }); },
                onTapDown: (TapDownDetails details) => {
                    this.widget.relative = seekToRelativePosition(details.globalPosition);
                    this.setState(() => { });
                    this.widget.changeCallback(this.widget.relative);
                }
            );
        }
    }


    public class _ProgressBarPainter : CustomPainter {
        public _ProgressBarPainter(
            float value,
            ProgressColors colors) {
            this.value = value;
            this.colors = colors;
        }

        readonly float value;
        readonly ProgressColors colors;


        public void paint(Canvas canvas, Size size) {
            float height = 4.0f;
            float btnRadius = 6.0f;
            float borderWidth = 0.5f;
            float startY = (size.height - height) / 2;
            float endY = (size.height + height) / 2;
            var rReact = RRect.fromRectAndRadius(
                Rect.fromPoints(
                    new Offset(0.0f, (size.height - height) / 2),
                    new Offset(size.width, (size.height + height) / 2)
                ),
                Radius.circular(0.0f)
            );
            canvas.drawRRect(
                rReact, this.colors.backgroundPaint
            );

            var playedPart = this.value * size.width;
            var borderReact = RRect.fromRectAndRadius(
                Rect.fromPoints(
                    new Offset(playedPart, startY + borderWidth),
                    new Offset(size.width, endY - borderWidth)
                ),
                Radius.circular(0.0f)
            );

            var path = new Path();
            path.addRRect(borderReact);

            var paint = new Paint();
            paint.color = CColors.White;
            paint.style = PaintingStyle.stroke;
            paint.strokeWidth = borderWidth;

            canvas.drawPath(path, paint);
            canvas.drawRRect(
                RRect.fromRectAndRadius(
                    Rect.fromPoints(
                        new Offset(0.0f, startY),
                        new Offset(playedPart, endY)
                    ),
                    Radius.circular(0.0f)
                ), this.colors.playedPaint
            );

            canvas.drawCircle(
                new Offset(playedPart, size.height / 2),
                btnRadius, this.colors.handlePaint
            );
        }

        public bool shouldRepaint(CustomPainter oldDelegate) {
            return true;
        }

        public bool? hitTest(Offset position) {
            return false;
        }

        public void addListener(VoidCallback listener) {
        }

        public void removeListener(VoidCallback listener) {
        }
    }


    public class ProgressColors {
        public ProgressColors() {
            this.playedPaint = new Paint();
            this.bufferedPaint = new Paint();
            this.handlePaint = new Paint();
            this.backgroundPaint = new Paint();

            this.playedPaint.color = CColors.SecondaryPink;
            this.bufferedPaint.color = new Color(0xFFFFFFFF);
            this.handlePaint.color = new Color(0xFFFFFFFF);
            this.backgroundPaint.color = new Color(0xFFFFFFFF);
        }

        public readonly Paint playedPaint;
        readonly Paint bufferedPaint;
        public readonly Paint handlePaint;
        public readonly Paint backgroundPaint;
    }
}