using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components
{
    public delegate void ChangeCallback(float relative);
    public class ProgressBar : StatefulWidget
    {
        public ProgressBar(
            float relative,
            VoidCallback onDragStart = null,
            ChangeCallback changeCallback = null,
            ProgressColors colors = null,
            Key key = null
        ) : base(key)
        {
            this.relative = relative;
            this.colors = colors??new ProgressColors();
            this.onDragStart = onDragStart;
            this.changeCallback = changeCallback;
        }

        public override State createState()
        {
            return new _ProgressBarState();
        }
        
        public float relative;
        public readonly ProgressColors colors;
        public readonly VoidCallback onDragStart;
        public readonly ChangeCallback changeCallback;
        
        
    }

    internal class _ProgressBarState : State<ProgressBar>
    {

        public override Widget build(BuildContext context)
        {
            float seekToRelativePosition(Offset globalPosition) {
                var box = (RenderBox)context.findRenderObject();
                var tapPos = box.globalToLocal(globalPosition);
                var relative = tapPos.dx / box.size.width;
                relative = relative > 1 ? 1 : relative;
                relative = relative < 0 ? 0 : relative;
                return relative;
            }
            return new GestureDetector(
                child:new Container(
                    height: MediaQuery.of(context).size.height,
                    width: MediaQuery.of(context).size.width,
                    color: Colors.transparent,
                    child: new CustomPaint(
                        painter: new _ProgressBarPainter(
                            widget.relative,
                            widget.colors
                        )
                    )
                    
                ),
                onHorizontalDragStart: (DragStartDetails details) => { widget.onDragStart(); },
                onHorizontalDragUpdate:(DragUpdateDetails details) =>
                {
                    var relative = seekToRelativePosition(details.globalPosition);
                    widget.changeCallback(relative);
                    setState(() => { });
                },
                onTapDown: (TapDownDetails details) =>
                {
                    var relative = seekToRelativePosition(details.globalPosition);
                    setState(() => { });
                    widget.changeCallback(relative);
                }
                
            );
        }
    }


    public class _ProgressBarPainter : CustomPainter
    {
        public _ProgressBarPainter(
            float value,
            ProgressColors colors)
        {
            this.value = value;
            this.colors = colors;
        }

        private readonly float value;
        private readonly ProgressColors colors;


        public void paint(Canvas canvas, Size size)
        {
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
                rReact,
                colors.backgroundPaint
            );
            
            var playedPart = value * size.width;
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
            paint.color = Colors.white;
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
                ),
                colors.playedPaint
            );

            canvas.drawCircle(
                new Offset(playedPart, size.height / 2),
                btnRadius,
                colors.handlePaint
            );
        }

        public bool shouldRepaint(CustomPainter oldDelegate)
        {
            return true;
        }

        public bool? hitTest(Offset position)
        {
            return false;
        }

        public void addListener(VoidCallback listener)
        {
            
        }

        public void removeListener(VoidCallback listener)
        {
            
        }
    }




    public class ProgressColors
    {
        public ProgressColors()
        {
            playedPaint = new Paint();
            bufferedPaint = new Paint();
            handlePaint = new Paint();
            backgroundPaint = new Paint();

            playedPaint.color = CColors.SecondaryPink;
            bufferedPaint.color = new Color(0xFFFFFFFF);
            handlePaint.color = new Color(0xFFFFFFFF);
            backgroundPaint.color = new Color(0xFFFFFFFF);
        }

        public readonly Paint playedPaint;
        private readonly Paint bufferedPaint;
        public readonly Paint handlePaint;
        public readonly Paint backgroundPaint;

    }
}