using System;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components.refresh
{

    public class DefaultRefreshLocal
    {
        public string loading;
        public string pullDownToRefresh;
        public string pullUpToRefresh;
         public string releaseToRefresh;
         public string lastUpdate;

         DefaultRefreshLocal(
             string loading,
             string pullDownToRefresh,
             string pullUpToRefresh,
             string releaseToRefresh,
             string lastUpdate
         )
         {
             this.loading = loading;
             this.pullDownToRefresh = pullDownToRefresh;
             this.pullUpToRefresh = pullUpToRefresh;
             this.releaseToRefresh = releaseToRefresh;
             this.lastUpdate = lastUpdate;
         }

         static DefaultRefreshLocal en(string loading = "loading...",
             string pullDownToRefresh="pull down to refresh",
             string pullUpToRefresh="pull up to refresh",
             string releaseToRefresh="release to refresh",
             string lastUpdate="last update"){
             return new DefaultRefreshLocal(
                 lastUpdate: lastUpdate,
                 loading: loading,
                 releaseToRefresh: releaseToRefresh,
                 pullDownToRefresh: pullDownToRefresh,
                 pullUpToRefresh: pullUpToRefresh
             );
         }

         public static DefaultRefreshLocal zh(string loading = "加载中...",
             string pullDownToRefresh="下拉刷新",
             string pullUpToRefresh="上拉加载更多",
             string releaseToRefresh="放开加载",
             string lastUpdate="最后更新"){
             return new DefaultRefreshLocal(
                 lastUpdate: lastUpdate,
                 loading: loading,
                 releaseToRefresh: releaseToRefresh,
                 pullDownToRefresh: pullDownToRefresh,
                 pullUpToRefresh: pullUpToRefresh
             );
         }
    }
    
    
    public delegate RefreshChild RefreshChildBuilder(
    BuildContext context, RefreshWidgetController controller);
    
    public abstract class RefreshChild : StatefulWidget
    {
        public RefreshWidgetController controller;
        
        protected RefreshChild(
            RefreshWidgetController controller,
            Key key) : base(key)
        {
            this.controller = controller;
        }
    }

    public class DefaultRefreshChild : RefreshChild
    {
        public DefaultRefreshChild(
            RefreshWidgetController controller,
            Widget icon,
            bool showState  = true,
            bool showLastUpdate = true,
            DefaultRefreshLocal local = null,
            bool up = true,
            Key key = null) : base(controller,key)
        {
            this.icon = icon;
            this.showState = showState;
            this.showLastUpdate = showLastUpdate;
            this.local = local ?? DefaultRefreshLocal.zh();
        }

        public bool showState;
        public bool showLastUpdate;
        public Widget icon;
        public DefaultRefreshLocal local;
        public bool up;

        public override State createState()
        {
            return new _DefaultRefreshHeaderState();
        }
    }

    class _DefaultRefreshHeaderState : State<DefaultRefreshChild>,TickerProvider
    {
        AnimationController _animation;

        Tween<float> _tween;

        DateTime _lastUpdate;

        RefreshState _state = RefreshState.drag;


        public override void initState()
        {
            base.initState();
            _animation = new AnimationController(vsync: this);
            _tween = new FloatTween(
                0.0f,1.0f
            );
            _tween.animate(_animation);
            _lastUpdate = new DateTime();
        }

        
        public override Widget build(BuildContext context)
        {
            var style = new TextStyle(fontSize:14);
            var texts = new List<Widget>();
            if (widget.showState) {
                texts.Add(
                    new Text(
                        _state == RefreshState.loading
                            ? widget.local.loading
                            : (_state == RefreshState.ready
                                ? widget.local.releaseToRefresh
                                : (widget.up
                                    ? widget.local.pullDownToRefresh
                                    : widget.local.pullUpToRefresh)),
                        style: style
                    )
                );
            }
            if (widget.showLastUpdate) {
                texts.Add(new Text(
                    $"{widget.local.lastUpdate}{_formateDate()}",
                    style: style
                ));
            }
            Widget text = texts.Count > 0
                ? new Padding(
                    padding: EdgeInsets.only(left: 10),
                    child: new Column(
                        mainAxisSize: MainAxisSize.min,
                        children: texts
                    )
                )
                : null;
            Widget activity = new SizedBox(
                width: 20,
                height: 20,
                child: new CustomActivityIndicator()
            );
            List<Widget> row = new List<Widget>();
            if (_state == RefreshState.loading)
            {
                row.Add(activity);
            }
            else
            {
                row.Add(new RotationTransition(
                    turns: _animation,
                    child: widget.icon
                ));
              
            }

            if (text != null) {
                row.Add(text);
            }

            return new Row(
                mainAxisSize: MainAxisSize.min,
                children: row
            );
        }

        public override void didUpdateWidget(StatefulWidget oldWidget)
        {
            if (oldWidget is RefreshWidget)
            {
                RefreshWidget refreshWidget = (RefreshWidget) oldWidget;
                if (widget.controller != refreshWidget.controller) {
                    refreshWidget.controller.removeStateListener(_updateState);
                    widget.controller.addStateListener(_updateState);
                }
            }
            base.didUpdateWidget(oldWidget);
        }

        public override void didChangeDependencies()
        {
            widget.controller.addStateListener(_updateState);
            base.didChangeDependencies();
        }

        public override void dispose()
        {
            widget.controller.removeStateListener(_updateState);
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick)
        {
            Ticker _ticker = new Ticker(onTick, debugLabel: $"created by {this}");
            return _ticker;
        }
        
        void _rotate(float value, bool releaseToRefresh) {
            _animation
                .animateTo(value,
                    duration: new TimeSpan(0,0,0,0,200), curve: Curves.ease)
                .Done(()=>{});
        }
        
        void _updateState() {
            switch (widget.controller.state) {
                case RefreshState.ready:
                {
                    _rotate(0.5f, true);
                }

                    break;

                case RefreshState.drag:
                {
                    _rotate(0.0f, true);
                }

                    break;
                case RefreshState.loading:
                {}
                    break;
            }

            setState(()=> {
                _state = widget.controller.state;
            });
        }
        
        
        string _formateDate() {
            DateTime time = _lastUpdate;
            return $"{_twoDigist(time.Hour)}:{_twoDigist(time.Minute)}";
        }
        static string _twoDigist(int num) {
            if (num < 10) return $"0{num}";
            return num.ToString();
        }
    }
    

}