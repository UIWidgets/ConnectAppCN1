using System;
using System.Collections.Generic;
using ConnectApp.components.refresh;
using ConnectApp.constants;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class RefreshPage : StatefulWidget {
        public override State createState() {
            return new _RefreshPageState();
        }

        public RefreshPage(Key key = null) : base(key) {
        }
    }

    internal class _RefreshPageState : State<RefreshPage>, TickerProvider {
        private List<Widget> _list = new List<Widget> {
        };

        private Promise onFooterRefresh() {
            var promise = new Promise();

            return promise;
        }


        private Promise onHeaderRefresh() {
            var promise = new Promise();

            return promise;
        }

        public override Widget build(BuildContext context) {
            return new SafeArea(
                child: new Container(
                    margin: EdgeInsets.only(top: 50, bottom: 50),
                    color: CColors.White,
                    child: new Refresh(
                        onFooterRefresh: onFooterRefresh,
                        onHeaderRefresh: onHeaderRefresh,
                        child: new ListView(
                            children: _list
                        )))
            );
        }

        public Ticker createTicker(TickerCallback onTick) {
            Ticker _ticker = new Ticker(onTick, debugLabel: $"created by {this}");
            return _ticker;
        }
    }
}