using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public static class CustomListViewConstant {
        public static readonly Widget defaultHeaderWidget = new CustomDivider(color: CColors.White);
        public static readonly Widget defaultFooterWidget = new EndView();
    }

    public class CustomListView : StatefulWidget {
        public CustomListView(
            RefreshController controller = null,
            bool enablePullUp = DefaultConstants.default_enablePullUp,
            bool enablePullDown = DefaultConstants.default_enablePullDown,
            OnRefresh onRefresh = null,
            bool hasBottomMargin = false,
            int? itemCount = null,
            IndexedWidgetBuilder itemBuilder = null,
            Widget headerWidget = null,
            Widget footerWidget = null,
            bool hasScrollBar = true,
            bool hasRefresh = true,
            Key key = null
        ) : base(key: key) {
            D.assert(() => {
                if (this.hasRefresh) {
                    return this.onRefresh != null;
                }
                return true;
            });
            this.controller = controller;
            this.enablePullUp = enablePullUp;
            this.enablePullDown = enablePullDown;
            this.onRefresh = onRefresh;
            this.hasBottomMargin = hasBottomMargin;
            this.itemCount = itemCount;
            this.itemBuilder = itemBuilder;
            this.headerWidget = headerWidget;
            this.footerWidget = footerWidget;
            this.hasScrollBar = hasScrollBar;
            this.hasRefresh = hasRefresh;
        }

        public readonly RefreshController controller;
        public readonly bool enablePullUp;
        public readonly bool enablePullDown;
        public readonly OnRefresh onRefresh;
        public readonly bool hasBottomMargin;
        public readonly int? itemCount;
        public readonly IndexedWidgetBuilder itemBuilder;
        public readonly Widget headerWidget;
        public readonly Widget footerWidget;
        public readonly bool hasScrollBar;
        public readonly bool hasRefresh;

        public override State createState() {
            return new _CustomListViewState();
        }
    }

    class _CustomListViewState : State<CustomListView> {

        bool _hasHeaderWidget() {
            return this.widget.headerWidget != null;
        }

        bool _hasFooterWidget() {
            return this.widget.footerWidget != null;
        }

        public override Widget build(BuildContext context) {
            int itemCount = this.widget.itemCount ?? 0;
            if (this._hasHeaderWidget()) {
                itemCount += 1;
            }
            if (this._hasFooterWidget()) {
                itemCount += 1;
            }

            var listView = ListView.builder(
                physics: new AlwaysScrollableScrollPhysics(),
                itemCount: itemCount,
                itemBuilder: (cxt, index) => {
                    if (this._hasHeaderWidget() && index == 0) {
                        return this.widget.headerWidget;
                    }

                    if (this._hasFooterWidget() && index == itemCount - 1) {
                        return this.widget.footerWidget;
                    }

                    var newIndex = this._hasHeaderWidget() ? index - 1 : index;
                    return this.widget.itemBuilder(context: cxt, index: newIndex);
                }
            );

            Widget refreshWidget = this._buildRefreshWidget(listView: listView);
            Widget scrollBarWidget = this._buildScrollBarWidget(widget: refreshWidget);

            return scrollBarWidget;
        }

        Widget _buildRefreshWidget(ScrollView listView) {
            if (this.widget.hasRefresh) {
                return new SmartRefresher(
                    controller: this.widget.controller,
                    enablePullDown: this.widget.enablePullDown,
                    enablePullUp: this.widget.enablePullUp,
                    onRefresh: this.widget.onRefresh,
                    hasBottomMargin: this.widget.hasBottomMargin,
                    child: listView
                );
            }
            return listView;
        }

        Widget _buildScrollBarWidget(Widget widget) {
            if (this.widget.hasScrollBar) {
                return new CustomScrollbar(
                    child: widget
                );
            }
            return widget;
        }
    }
}