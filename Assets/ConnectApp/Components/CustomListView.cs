using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public static class CustomListViewConstant {
        public static readonly Widget defaultHeaderWidget = new CustomDivider(color: CColors.White);
        public static readonly Widget defaultFooterWidget = new EndView();
    }

    public class CustomListView : StatelessWidget {
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

        readonly RefreshController controller;
        readonly bool enablePullUp;
        readonly bool enablePullDown;
        readonly OnRefresh onRefresh;
        readonly bool hasBottomMargin;
        readonly int? itemCount;
        readonly IndexedWidgetBuilder itemBuilder;
        readonly Widget headerWidget;
        readonly Widget footerWidget;
        readonly bool hasScrollBar;
        readonly bool hasRefresh;

        bool _hasHeaderWidget() {
            return this.headerWidget != null;
        }

        bool _hasFooterWidget() {
            return this.footerWidget != null;
        }

        public override Widget build(BuildContext context) {
            int itemCount = this.itemCount ?? 0;
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
                        return this.headerWidget;
                    }

                    if (this._hasFooterWidget() && index == itemCount - 1) {
                        return this.footerWidget;
                    }

                    var newIndex = this._hasHeaderWidget() ? index - 1 : index;
                    return this.itemBuilder(context: cxt, index: newIndex);
                }
            );

            Widget refreshWidget = this._buildRefreshWidget(listView: listView);
            Widget scrollBarWidget = this._buildScrollBarWidget(widget: refreshWidget);

            return scrollBarWidget;
        }

        Widget _buildRefreshWidget(ScrollView listView) {
            if (this.hasRefresh) {
                return new SmartRefresher(
                    controller: this.controller,
                    enablePullDown: this.enablePullDown,
                    enablePullUp: this.enablePullUp,
                    onRefresh: this.onRefresh,
                    hasBottomMargin: this.hasBottomMargin,
                    child: listView
                );
            }
            return listView;
        }

        Widget _buildScrollBarWidget(Widget widget) {
            if (this.hasScrollBar) {
                return new CustomScrollbar(
                    child: widget
                );
            }
            return widget;
        }
    }
}