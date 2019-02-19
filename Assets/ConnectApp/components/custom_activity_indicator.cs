using System;
using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.components
{

    public enum AnimatingType
    {
        repeat,
        stop,
        reset
    }


    public class CustomActivityIndicator : StatefulWidget
    {
        public CustomActivityIndicator(
            Key key = null,
            AnimatingType animating = AnimatingType.repeat,
            double radius = 16.0
            ) : base(key)
        {
            animating = this.animating;
            radius = this.radius;
        }


        public readonly AnimatingType animating;
        public readonly double radius;
        
        public override State createState()
        {
            return new _CustomActivityIndicatorState();
        }
    }


    public class _CustomActivityIndicatorState : State<CustomActivityIndicator>, TickerProvider
    {
        private AnimationController _controller;


        public override void initState()
        {
            base.initState();
            
            _controller = new AnimationController(
                duration: new TimeSpan(0,0,2),
                vsync: this
                );
            if (widget.animating == AnimatingType.repeat) _controller.repeat();
        }

        public Ticker createTicker(TickerCallback onTick)
        {
            Ticker _ticker = new Ticker(onTick, debugLabel: $"created by {this}");
            return _ticker;
        }

        public void didUpdateWidget(CustomActivityIndicator oldWidget)
        {
            base.didUpdateWidget(oldWidget);
            if (widget.animating != oldWidget.animating) {
                if (widget.animating == AnimatingType.repeat)
                    _controller.repeat();
                else if (widget.animating == AnimatingType.stop)
                    _controller.stop();
                else if (widget.animating == AnimatingType.reset) _controller.reset();
            }
        }

        public override Widget build(BuildContext context)
        {
            return new RotationTransition(
                turns: _controller,
                child: new Column(mainAxisAlignment: MainAxisAlignment.center,
                    children:new List<Widget>
                    {
                        Image.asset("loading",width:36,height:36)
                    })
            );
        }

        public override void dispose()
        {
            base.dispose();
            _controller.dispose();
        }
    }

}