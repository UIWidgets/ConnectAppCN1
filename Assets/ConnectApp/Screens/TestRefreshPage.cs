using System;
using System.Collections.Generic;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class RefreshPage : StatefulWidget {
        public override State createState() {
            return new _RefreshPageState();
        }

        public RefreshPage(Key key = null) : base(key) {
        }
    }

    internal class _RefreshPageState : State<RefreshPage> {
        RefreshController _refreshController;
        ScrollController _scrollController;
        private List<Widget> _list = new List<Widget> {
            new Text("1"),
            new Text("2"),
            new Text("3")
        };
        void enterRefresh() {
            _refreshController.requestRefresh(true);
        }

        void _onOffsetCallback(bool isUp, double offset) {
            // if you want change some widgets state ,you should rewrite the callback
        }

        public override void initState()
        {
            _scrollController = new ScrollController();
            _refreshController = new RefreshController();
            enterRefresh();
            base.initState();
        }
        Widget _headerCreate(BuildContext context, int mode) {
            return new ClassicIndicator(
                mode: mode,
                refreshingText: "",
                idleIcon: new Container(),
                idleText: "Load more..."
            );
        }
        
        
        public override Widget build(BuildContext context) {
            return new Container(
                margin: EdgeInsets.only(bottom: 49),
                  color:CColors.White,
                  child:new Container(
                      child:new SmartRefresher(
                      controller: _refreshController,
                      enablePullDown: true,
                      enablePullUp: true,
                      headerBuilder: (cxt,mode) =>
                          new SmartRefreshHeader(mode), 
                      footerBuilder: (cxt,mode) =>
                          new SmartRefreshHeader(mode),
                      onOffsetChange: _onOffsetCallback,
                      onRefresh: up =>
                      {
                          if (up)
                          {
                              Promise.Delayed(TimeSpan.FromMilliseconds(1000)).Then(() =>
                              {
                                  
                                  setState(() => {
                                      _list.Add(new Text("new up"));
                                      _list.Add(new Text("new up"));
                                      _list.Add(new Text("new up"));
                                      _list.Add(new Text("new up"));
                                      _list.Add(new Text("new up"));
                                      
                                      _refreshController.sendBack(true, RefreshStatus.completed);
                                  });
                              });
                              
                          }
                          else
                          {
                              Promise.Delayed(TimeSpan.FromMilliseconds(1000)).Then(() =>
                              {
                                  
                                  setState(()=>
                                  {
                                      _list.Add(new Text("new down"));
                                      _list.Add(new Text("new down"));
                                      _list.Add(new Text("new udownp"));
                                      _list.Add(new Text("new udownp"));
                                      _list.Add(new Text("new udownp"));
                                      _list.Add(new Text("new udownp"));
                                      _list.Add(new Text("new udownp"));

                                      _refreshController.sendBack(false, RefreshStatus.idle);
                                  });
                              });
                          }
                      },
                        
                      child:ListView.builder(
                          reverse: false,
                          itemExtent: 100.0f,
                          itemCount: _list.Count,
                          itemBuilder: (cxt, index) => { return _list[index]; }
                      )
                  ) )
                );
        }
    }
}