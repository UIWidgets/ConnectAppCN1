using System.Collections.Generic;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace markdown {
    public class CustomMarkdown : MarkdownWidget {
        public CustomMarkdown(
            Key key = null,
            string data = null,
            bool selectable = false,
            MarkdownStyleSheet markdownStyleSheet = null,
            SyntaxHighlighter syntaxHighlighter = null,
            MarkdownTapLinkCallback onTapLink = null,
            string imageDirectory = null,
            ExtensionSet extensionSet = null,
            MarkdownImageBuilder imageBuilder = null,
            MarkdownCheckboxBuilder checkboxBuilder = null,
            ScrollPhysics physics = null,
            bool shrinkWrap = false,
            Widget contentHead = null,
            Widget relatedArticles = null,
            List<Widget> commentList = null,
            RefreshController refreshController = null,
            bool enablePullDown = false,
            bool enablePullUp = false,
            OnRefresh onRefresh = null,
            NotificationListenerCallback<ScrollNotification> onNotification = null,
            float initialOffset = 0f,
            bool needRebuildWithCachedCommentPosition = false,
            bool isArticleJumpToCommentStateActive = false
        ) : base(key, data, markdownStyleSheet, syntaxHighlighter,
            onTapLink, imageDirectory, extensionSet, imageBuilder, checkboxBuilder, selectable) {
            this.padding = EdgeInsets.all(16);
            this.physics = physics;
            this.shrinkWrap = shrinkWrap;
            this.contentHead = contentHead;
            this.relatedArticles = relatedArticles;
            this.commentList = commentList;
            this.refreshController = refreshController;
            this.enablePullDown = enablePullDown;
            this.enablePullUp = enablePullUp;
            this.onRefresh = onRefresh;
            this.onNotification = onNotification;
            this.initialOffset = initialOffset;
            this.needRebuildWithCachedCommentPosition = needRebuildWithCachedCommentPosition;
            this.isArticleJumpToCommentStateActive = isArticleJumpToCommentStateActive;
        }

        public EdgeInsets padding;

        public ScrollPhysics physics;

        public bool shrinkWrap;
        public Widget contentHead;
        public Widget relatedArticles;
        public List<Widget> commentList;
        public RefreshController refreshController;
        public bool enablePullDown;
        public bool enablePullUp;
        public OnRefresh onRefresh;
        public NotificationListenerCallback<ScrollNotification> onNotification;
        public float initialOffset;
        public bool needRebuildWithCachedCommentPosition;
        public bool isArticleJumpToCommentStateActive;

        public override Widget build(BuildContext context, List<Widget> children) {
            var commentIndex = 0;
            List<Widget> originItems = new List<Widget>();
            if (this.contentHead != null) {
                originItems.Add(this.contentHead);
            }

            List<Widget> paddingWidgets = new List<Widget>();
            children.ForEach(widget => {
                paddingWidgets.Add(new Container(color: CColors.White, padding: EdgeInsets.symmetric(horizontal: 16),
                    child: widget));
            });
            originItems.AddRange(paddingWidgets);
            if (this.relatedArticles != null) {
                originItems.Add(this.relatedArticles);
            }

            commentIndex = originItems.Count;
            if (this.commentList.isNotNullAndEmpty()) {
                originItems.AddRange(this.commentList);
            }

            commentIndex = this.isArticleJumpToCommentStateActive ? commentIndex : 0;


            if (this.needRebuildWithCachedCommentPosition == false && commentIndex != 0) {
                return new CenteredRefresher(
                    controller: this.refreshController,
                    enablePullDown: this.enablePullDown,
                    enablePullUp: this.enablePullUp,
                    onRefresh: this.onRefresh,
                    onNotification: this.onNotification,
                    children: originItems,
                    centerIndex: commentIndex
                );
            }

            return new SmartRefresher(
                initialOffset: this.initialOffset,
                controller: this.refreshController,
                enablePullDown: this.enablePullDown,
                enablePullUp: this.enablePullUp,
                onRefresh: this.onRefresh,
                onNotification: this.onNotification,
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: originItems.Count,
                    itemBuilder: (cxt, index) => originItems[index]
                ));
        }
    }
}