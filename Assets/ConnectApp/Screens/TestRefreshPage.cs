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

    class _RefreshPageState : State<RefreshPage> {
        RefreshController _refreshController;
        ScrollController _scrollController;

        List<Widget> _list = new List<Widget> {
            new Text("1"),
            new Text("2"),
            new Text("3")
        };

        void enterRefresh() {
            this._refreshController.requestRefresh(true);
        }

        void _onOffsetCallback(bool isUp, double offset) {
            // if you want change some widgets state ,you should rewrite the callback
        }

        public override void initState() {
            this._scrollController = new ScrollController();
            this._refreshController = new RefreshController();
            this.enterRefresh();
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
                color: CColors.White,
                child: new Container(
                    child: new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: true,
                        enablePullUp: true,
                        onOffsetChange: this._onOffsetCallback,
                        onRefresh: up => {
                            if (up) {
                                Promise.Delayed(TimeSpan.FromMilliseconds(1000)).Then(() => {
                                    this.setState(() => {
                                        this._list.Add(new Text("new up"));
                                        this._list.Add(new Text("new up"));
                                        this._list.Add(new Text("new up"));
                                        this._list.Add(new Text("new up"));
                                        this._list.Add(new Text("new up"));

                                        this._refreshController.sendBack(true, RefreshStatus.completed);
                                    });
                                });
                            }
                            else {
                                Promise.Delayed(TimeSpan.FromMilliseconds(1000)).Then(() => {
                                    this.setState(() => {
                                        this._list.Add(new Text("new down"));
                                        this._list.Add(new Text("new down"));
                                        this._list.Add(new Text("new udownp"));
                                        this._list.Add(new Text("new udownp"));
                                        this._list.Add(new Text("new udownp"));
                                        this._list.Add(new Text("new udownp"));
                                        this._list.Add(new Text("new udownp"));

                                        this._refreshController.sendBack(false, RefreshStatus.idle);
                                    });
                                });
                            }
                        },
                        child: ListView.builder(
                            reverse: false,
                            itemExtent: 100.0f,
                            itemCount: this._list.Count,
                            itemBuilder: (cxt, index) => { return this._list[index]; }
                        )
                    ))
            );
        }
    }
}