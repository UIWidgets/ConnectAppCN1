using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components.refresh
{
    public class RelativePositionedTransition : AnimatedWidget
    {
        public RelativePositionedTransition(
            Animation<Rect> rect,
            Size size,
            Widget child,
            Key key = null
        ) : base(key, listenable:rect)
        {
            this.rect = rect;
            this.size = size;
            this.child = child;
        }

        protected override Widget build(BuildContext context)
        {
            RelativeRect offsets = RelativeRect.fromSize(rect.value, size);
            return new Positioned(
                top: offsets.top,
                right: offsets.right,
                bottom: offsets.bottom,
                left: offsets.left,
                child: child
            );
        }

        private readonly Animation<Rect> rect;
        private readonly Size size;
        private readonly Widget child;
    }
}