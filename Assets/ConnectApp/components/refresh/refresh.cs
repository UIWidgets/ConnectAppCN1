using System;
using System.Collections.Generic;
using ConnectApp.constants;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components.refresh
{
    public delegate Widget RefreshScrollViewBuilder(BuildContext context,ScrollController controller, ScrollPhysics physics);    
    public class Refresh : StatefulWidget
    {
        public Refresh(
            Widget child,
            RefresherCallback onHeaderRefresh,
            RefresherCallback onFooterRefresh,
            RefreshController controller = null,
            ScrollController scrollController = null,
            RefreshScrollViewBuilder childBuilder = null,
            ScrollPhysics physics = null ,
            Key key = null) : base(key)
        {
            this.onHeaderRefresh = onHeaderRefresh;
            this.onFooterRefresh = onFooterRefresh;
            this.controller = controller;
            this.scrollController = scrollController;
            this.childBuilder = childBuilder;
            this.physics = physics;
            this.child = child;
            
        }

        public RefresherCallback onHeaderRefresh;
        public RefresherCallback onFooterRefresh;
        public RefreshController controller;
        public ScrollController scrollController;
        public RefreshScrollViewBuilder childBuilder;
        public ScrollPhysics physics;
        public Widget child;
        
        static public  ScrollPhysics createScrollPhysics(ScrollPhysics src) {
            if (true) {
                // 判断是否是安卓，这个条件以后加上
                ScrollPhysics physics = new AlwaysScrollableScrollPhysics()
                    .applyTo(new BouncingScrollPhysics());
                if (src != null) {
                    return physics.applyTo(src);
                }
                return physics;
            }
            return src;
        }
        
        
        public override State createState()
        {
            return new _RefreshState();
        }
    }

    delegate void StateHandler(ScrollNotification notification);
    internal class _RefreshState : State<Refresh>
    {
        float _headerRefreshOffset = 50.0f;
        float _footerRefreshOffset = 50.0f;

        RefreshWidgetController _headerValue;
        RefreshWidgetController _footerValue;

        _RefreshHandler _headerHandler;
        _RefreshHandler _footerHandler;
        
        _RefreshHandler _hander;

        ScrollController controller;
        StateHandler _state;

        bool _isAnimation = false;
        bool _animationComplete = false;
        
        public override Widget build(BuildContext context)
        {
            Widget notificationChild = new NotificationListener<ScrollNotification>(
                child: widget.child == null
                    ?widget.childBuilder(context,
                        controller: controller,
                        physics: Refresh.createScrollPhysics(widget.physics))
                    : _cloneChild(widget.child),
                onNotification: _handleScrollNotification
            );

            List<Widget> children = new List<Widget>{notificationChild};

            if (widget.onHeaderRefresh != null) {
                children.Add(new RefreshWidget(
                        height: _headerRefreshOffset,
                        controller: _headerValue,
                        createTween: createTweenForHeader,
                        alignment: Alignment.topCenter,
                        maxOffset: 300,
                        childBuilder:
                        (BuildContext _context, RefreshWidgetController controller) =>{
                            return new DefaultRefreshChild(
                                controller: controller,
                                icon: new Icon(Icons.arrow_downward), 
                                up: true
                            );
                        })
                );
            }

            if (widget.onFooterRefresh != null) {
                children.Add(new RefreshWidget(
                        height: _footerRefreshOffset,
                        controller: _footerValue,
                        createTween: createTweenForFooter,
                        maxOffset: 300,
                        alignment: Alignment.bottomCenter,
                        childBuilder:
                        (BuildContext _context, RefreshWidgetController controller)=> {
                            return new DefaultRefreshChild(
                            controller: controller,
                            icon: new Icon(Icons.arrow_upward),
                            showLastUpdate: false,
                            up: false
                            );
                        })
                );
            }

            return new Stack(
                key: widget.key,
                children: children
            );
        }

        public override void initState()
        {
            base.initState();
            
            _state = _end;
        }

        public override void didUpdateWidget(StatefulWidget oldWidget)
        {
            _updateState();
            base.didUpdateWidget(oldWidget);
        }

        public override void didChangeDependencies()
        {
            _updateState();
            base.didChangeDependencies();
        }
        
        
        public RectTween createTweenForHeader(RefreshWidget widget)
        {
            return new RectTween(
                begin: Unity.UIWidgets.ui.Rect.fromLTRB(0,-widget.height,0,0),
                end: Unity.UIWidgets.ui.Rect.fromLTRB(0, 300, 0, 0)
            );
        } 
        public RectTween createTweenForFooter(RefreshWidget widget)
        {
            return new RectTween(
                begin: Unity.UIWidgets.ui.Rect.fromLTRB(0,0,0,widget.height),
                end: Unity.UIWidgets.ui.Rect.fromLTRB(0, 0, 0, -300)
            );
        } 

        Widget _cloneChild(Widget src) {
            switch (src) {
                case SingleChildScrollView listView:
                {
                    return new SingleChildScrollView(
                        controller: controller,
                        physics: Refresh.createScrollPhysics(listView.physics),
                        key: listView.key,
                        scrollDirection: listView.scrollDirection
                    );
                }
                    break;
                case ListView listView:
                {
                    return ListView.custom(
                        childrenDelegate: listView.childrenDelegate,
                        controller: controller,
                        physics: Refresh.createScrollPhysics(listView.physics),
                        key: listView.key,
                        scrollDirection: listView.scrollDirection
                    );
                }
            }

            return null;
        }
        
        ScrollController _tryGetController(Widget src) {
            switch (src) {
                case ListView listView:
                {
                    return listView.controller;
                }

                case SingleChildScrollView singleChildScrollView:
                    return singleChildScrollView.controller;
            }

            return null;
        }
        
        void _updateState() {
            if (widget.onHeaderRefresh != null) {
                if (_headerValue == null) _headerValue = new RefreshWidgetController();
                if (_headerHandler == null ||
                    _headerHandler.callback != widget.onHeaderRefresh) {
                    _headerHandler = new _RefreshHeaderHandler(
                        controller: _headerValue,
                        callback: widget.onHeaderRefresh,
                        offset: _headerRefreshOffset);
                }
            } else {
                if (_headerValue != null) _headerValue.dispose();
                _headerHandler = null;
            }

            if (widget.onFooterRefresh != null) {
                if (_footerValue == null) _footerValue = new RefreshWidgetController();
                if (_footerHandler == null ||
                    _footerHandler.callback != widget.onFooterRefresh) {
                    _footerHandler = new _RefreshFooterHandler(
                        controller: _footerValue,
                        callback: widget.onFooterRefresh,
                        offset: _footerRefreshOffset);
                }
            } else {
                if (_footerValue != null) _footerValue.dispose();
                _footerHandler = null;
            }

            ScrollController controller = widget.scrollController;
            if (controller == null) {
                if (widget.child != null) {
                    controller = _tryGetController(widget.child);
                }
            }

            if (controller == null) {
                if (this.controller == null) {
                    this.controller = new ScrollController();
                } else {
                    //不动
                }
            } else {
                //不等于,是否要移除什么
                this.controller = controller;
            }
        }
        
        void _scrollToAnimationFirst(float offset) {
            if (!_isAnimation) {
                if (_animationComplete) {
                    controller.jumpTo(offset);
                } else {
                    _isAnimation = true;
                    controller
                        .animateTo(offset,
                            duration: new TimeSpan( 300), curve: Curves.ease)
                        .Done(()=> {
                        _isAnimation = false;
                        _animationComplete = true;
                    });
                }
            }
        }
        void _loading(ScrollNotification notification) {
            if (notification is ScrollUpdateNotification) {
                D.assert(_hander != null);
                _scrollToAnimationFirst(_hander.getScrollOffset(notification.metrics));
            }
        }
        bool _handleScrollNotification(ScrollNotification notification) {
            _state(notification);
            return false;
        }
        void _end(ScrollNotification notification) {
            if (notification is ScrollStartNotification)
            {
                ScrollStartNotification startNotification = (ScrollStartNotification)notification;
                if (startNotification.dragDetails != null) {
                    // print("drag start");
                    _state = _drag;
                    _animationComplete = false;
                }
            } else if (notification is ScrollUpdateNotification) {
                ScrollMetrics metrics = notification.metrics;
                float pixels = metrics.pixels;
                if (pixels < 0) {
                    if (_headerValue != null) {
                        _headerValue.value = -pixels;
                    }
                } else {
                    //
                    if (_footerValue != null) {
                        float extValue = pixels - metrics.maxScrollExtent;
                        _footerValue.value = extValue;
                    }
                }
            }
        }
        void _drag(ScrollNotification notification) {
            if (notification is ScrollUpdateNotification) {
                var metrics = notification.metrics;
                var updateNotification = (ScrollUpdateNotification)notification;
                if (updateNotification.dragDetails != null) {
                    float moveValue = 0.0f;
                    if (_headerHandler != null &&
                        (moveValue = _headerHandler.getRefreshWidgetMoveValue(metrics)) >
                        0) {
                        _hander = _headerHandler;
                    } else if (_footerHandler != null &&
                               (moveValue = _footerHandler.getRefreshWidgetMoveValue(metrics)) >
                               0) {
                        _hander = _footerHandler;
                    } else {
                        _hander = null;
                    }
                    if (_hander != null) {
                        if (_hander.isReady(moveValue)) {
                            _hander.changeState(RefreshState.ready);
                        } else {
                            _hander.changeState(RefreshState.drag);
                        }
                        _hander.controller.value = moveValue;
                    }
                } else {
                    if (_hander != null &&
                        _hander.isReady(_hander.getRefreshWidgetMoveValue(metrics))) {
                        _hander.loading(metrics).Done(()=> {
                            //loading ok
                            D.assert(_hander != null);

                            _hander = null;
                            _state = _end;
                        });
                        _state = _loading;
                        return;
                    }
                    _state = _end;
                }
            }
        }
        
    }

    internal abstract class _RefreshHandler
    {
        public RefreshWidgetController controller;
        public RefresherCallback callback;
        public float offset;
        
        RefreshState _state = RefreshState.drag;

        public _RefreshHandler(
            RefreshWidgetController controller,
            RefresherCallback callback,
            float offset
        )
        {
            this.controller = controller;
            this.callback = callback;
            this.offset = offset;
        }

        public abstract float getScrollOffset(ScrollMetrics metrics);


        public abstract float getRefreshWidgetMoveValue(ScrollMetrics metrics);

        public Promise loading(ScrollMetrics metrics)
        {
            changeState(RefreshState.loading);
            Promise result = callback();
            D.assert(result is Promise,"");
            {
                result.Done(()=>{
                    changeState(RefreshState.drag);
                });
            }
            return result;
        }
        
        public bool isReady(float moveValue) {
            return moveValue > offset;
        }

       public void changeState(RefreshState currentState) {
            if (_state != currentState) {
                //通知状态改变
                _state = currentState;
                controller.state = _state;
            }
        }

        public void cancel(ScrollMetrics metrics)
        {
        }
    }
    
    class _RefreshHeaderHandler : _RefreshHandler {
        public _RefreshHeaderHandler(
            RefreshWidgetController controller,
            RefresherCallback callback,
            float offset = 50.0f)
            : base(controller,callback,offset)
        {
            this.controller = controller;
            this.callback = callback;
            this.offset = offset;
        }

        //When loading start, what value is ScrollView pixils?
   
        public override float getScrollOffset(ScrollMetrics metrics) {
            return -offset;
        }
        public override float getRefreshWidgetMoveValue(ScrollMetrics metrics) {
            return -metrics.pixels;
        }
    
        public void cancel(ScrollMetrics metrics) {
            controller.value = metrics.pixels;
        }
    }
    
    class _RefreshFooterHandler : _RefreshHandler {
        public _RefreshFooterHandler(
            RefreshWidgetController controller,
            RefresherCallback callback,
            float offset = 50.0f)
            : base(controller,callback,offset)
        {
            this.controller = controller;
            this.callback = callback;
            this.offset = offset;
        }

        //When loading start, what value is ScrollView pixils?
   
        public override float getScrollOffset(ScrollMetrics metrics) {
            return metrics.maxScrollExtent + offset;
        }
        public override float getRefreshWidgetMoveValue(ScrollMetrics metrics) {
            return metrics.pixels - metrics.maxScrollExtent;
        }
    
        public void cancel(ScrollMetrics metrics) {
            controller.value = metrics.pixels;
        }
    }

}