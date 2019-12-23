using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using markdown;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Element = markdown.Element;
using Image = Unity.UIWidgets.widgets.Image;
using Text = markdown.Text;

namespace markdownRender {
    public class builder {
        static List<string> _kBlockTags = new List<string> {
            "p",
            "h1",
            "h2",
            "h3",
            "h4",
            "h5",
            "h6",
            "li",
            "blockquote",
            "pre",
            "ol",
            "ul",
            "hr",
            "table",
            "thead",
            "tbody",
            "tr"
        };


        static readonly List<string> _kListTags = new List<string>() {
            "ul",
            "ol"
        };

        static bool _isBlockTag(string tag) {
            return _kBlockTags.Contains(tag);
        }

        static bool _isListTag(string tag) {
            return _kListTags.Contains(tag);
        }

        class _BlockElement {
            public string tag;
            public List<Widget> children = new List<Widget>();
            public int nextListIndex = 0;

            public _BlockElement(string tag) {
                this.tag = tag;
            }
        }

        class _TableElement {
            public List<TableRow> rows = new List<TableRow>();
        }

        /// A collection of widgets that should be placed adjacent to (inline with)
        /// other inline elements in the same parent block.
        ///
        /// Inline elements can be textual (a/em/strong) represented by [RichText]
        /// widgets or images (img) represented by [Image.network] widgets.
        ///
        /// Inline elements can be nested within other inline elements, inheriting their
        /// parent's style along with the style of the block they are in.
        ///
        /// When laying out inline widgets, first, any adjacent RichText widgets are
        /// merged, then, all inline widgets are enclosed in a parent [Wrap] widget.
        class _InlineElement {
            public string tag;

            /// Created by merging the style defined for this element's [tag] in the
            /// delegate's [MarkdownStyleSheet] with the style of its parent.
            public TextStyle style;

            public _InlineElement(string tag, TextStyle style) {
                this.tag = tag;
                this.style = style;
            }

            public List<Widget> children = new List<Widget>();
        }

        /// A delegate used by [MarkdownBuilder] to control the widgets it creates.
        public interface IMarkdownBuilderDelegate {
            /// Returns a gesture recognizer to use for an `a` element with the given
            /// `href` attribute.
            GestureRecognizer createLink(string href);

            /// The `styleSheet` is the value of [MarkdownBuilder.styleSheet].
            TextSpan formatText(MarkdownStyleSheet styleSheet, string code);
        }

        /// Builds a [Widget] tree from parsed Markdown.
        ///
        /// See also:
        ///
        ///  * [Markdown], which is a widget that parses and displays Markdown.
        public class MarkdownBuilder : NodeVisitor {
            /// A delegate that controls how link and `pre` elements behave.
            IMarkdownBuilderDelegate myDelegate;

            /// If true, the text is selectable.
            ///
            /// Defaults to false.
            bool selectable;

            /// Defines which [TextStyle] objects to use for each type of element.
            MarkdownStyleSheet styleSheet;

            /// The base directory holding images referenced by Img tags with local file paths.
            string imageDirectory;

            /// Call when build an image widget.
            MarkdownImageBuilder imageBuilder;

            /// Call when build a checkbox widget.
            MarkdownCheckboxBuilder checkboxBuilder;

            /// Whether to allow the widget to fit the child content.
            bool fitContent;

            List<string> _listIndents = new List<string>();
            List<_BlockElement> _blocks = new List<_BlockElement>();
            List<_TableElement> _tables = new List<_TableElement>();
            List<_InlineElement> _inlines = new List<_InlineElement>();
            List<GestureRecognizer> _linkHandlers = new List<GestureRecognizer>();
            bool _isInBlockquote = false;

            /// Creates an object that builds a [Widget] tree from parsed Markdown.
            public MarkdownBuilder(
                IMarkdownBuilderDelegate myDelegate,
                bool selectable,
                MarkdownStyleSheet styleSheet,
                string imageDirectory,
                MarkdownImageBuilder imageBuilder,
                MarkdownCheckboxBuilder checkboxBuilder,
                bool fitContent = false) {
                this.myDelegate = myDelegate;
                this.selectable = selectable;
                this.styleSheet = styleSheet;
                this.imageDirectory = imageDirectory;
                this.imageBuilder = imageBuilder;
                this.checkboxBuilder = checkboxBuilder;
                this.fitContent = fitContent;
            }

            /// Returns widgets that display the given Markdown nodes.
            ///
            /// The returned widgets are typically used as children in a [ListView].
            public List<Widget> build(List<Node> nodes) {
                this._listIndents.Clear();
                this._blocks.Clear();
                this._tables.Clear();
                this._inlines.Clear();
                this._linkHandlers.Clear();
                this._isInBlockquote = false;

                this._blocks.Add(new _BlockElement(null));

                foreach (var node in nodes) {
                    D.assert(this._blocks.Count == 1);
                    node.accept(this);
                }

                D.assert(this._tables.isEmpty());
                D.assert(this._inlines.isEmpty());
                D.assert(!this._isInBlockquote);

                return this._blocks.Single().children;
            }

            public override void visitText(Text text) {
                // Don't allow text directly under the root.
                if (this._blocks.last().tag == null) {
                    return;
                }

                this._addParentInlineIfNeeded(this._blocks.last().tag);
                Widget child;
                if (this._blocks.last().tag == "pre") {
                    child = new Scrollbar(
                        child: new SingleChildScrollView(
                            scrollDirection: Axis.horizontal,
                            padding: this.styleSheet.codeblockPadding,
                            child: this._buildRichText(this.myDelegate.formatText(this.styleSheet, text.text))
                        )
                    );
                }
                else {
                    child = this._buildRichText(new TextSpan(
                        style: this._isInBlockquote
                            ? this._inlines.last().style.merge(this.styleSheet.blockquote)
                            : this._inlines.last().style,
                        text: text.text,
                        recognizer: this._linkHandlers.isNotEmpty() ? this._linkHandlers.last() : null
                    ));
                }

                this._inlines.last().children.Add(child);
            }

            public override bool visitElementBefore(Element element) {
                string tag = element.tag;
                if (_isBlockTag(tag)) {
                    this._addAnonymousBLockIfNeeded();
                    if (_isListTag(tag)) {
                        this._listIndents.Add(tag);
                    }
                    else if (tag == "blockquote") {
                        this._isInBlockquote = true;
                    }
                    else if (tag == "table") {
                        this._tables.Add(new _TableElement());
                    }
                    else if (tag == "tr") {
                        var length = this._tables.Single().rows.Count;
                        var decoration = this.styleSheet.tableCellsDecoration;
                        if (length == 0 || length % 2 == 1) {
                            decoration = null;
                        }

                        this._tables.Single().rows.Add(new TableRow(
                            decoration: decoration,
                            children: new List<Widget>()
                        ));
                    }

                    this._blocks.Add(new _BlockElement(tag));
                }
                else {
                    this._addParentInlineIfNeeded(this._blocks.last().tag);
                    var parentStyle = this._inlines.last().style;
                    this._inlines.Add(new _InlineElement(tag, parentStyle.merge(this.styleSheet.styles(tag))));
                }

                if (tag == "a") {
                    this._linkHandlers.Add(this.myDelegate.createLink(element.attributes["href"]));
                }

                return true;
            }


            public override void visitElementAfter(Element element) {
                string tag = element.tag;
                if (_isBlockTag(tag)) {
                    this._addAnonymousBLockIfNeeded();

                    _BlockElement current = this._blocks.removeLast();
                    Widget child;

                    if (current.children.isNotEmpty()) {
                        child = new Column(
                            crossAxisAlignment: this.fitContent ? CrossAxisAlignment.start : CrossAxisAlignment.stretch,
                            children: current.children
                        );
                    }
                    else {
                        child = new SizedBox();
                    }

                    if (_isListTag(tag)) {
                        D.assert(this._listIndents.isNotEmpty());
                        this._listIndents.removeLast();
                    }
                    else if (tag == "li") {
                        if (this._listIndents.isNotEmpty()) {
                            if (element.children.Count == 0) {
                                element.children.Add(new Text(""));
                            }

                            Widget bullet;
                            dynamic el = element.children[0];
                            if (el is Element ele && ele.attributes.TryGetValue("type", out string type) &&
                                type == "checkbox") {
                                bool val = element.attributes["checked"] != "false";
                                bullet = this._buildCheckbox(val);
                            }
                            else {
                                bullet = this._buildBullet(this._listIndents.last());
                            }

                            child = new Row(
                                crossAxisAlignment: CrossAxisAlignment.start, // See #147
                                children: new List<Widget> {
                                    new SizedBox(
                                        width: this.styleSheet.listIndent,
                                        child: bullet
                                    ),
                                    new Expanded(child: child)
                                }
                            );
                        }
                    }
                    else if (tag == "table") {
                        child = new Table(
                            defaultColumnWidth: this.styleSheet.tableColumnWidth,
                            defaultVerticalAlignment: TableCellVerticalAlignment.middle,
                            border: this.styleSheet.tableBorder,
                            children: this._tables.removeLast().rows
                        );
                    }
                    else if (tag == "blockquote") {
                        this._isInBlockquote = false;
                        child = new DecoratedBox(
                            decoration: this.styleSheet.blockquoteDecoration,
                            child: new Padding(
                                padding: this.styleSheet.blockquotePadding,
                                child: child
                            )
                        );
                    }
                    else if (tag == "pre") {
                        child = new DecoratedBox(
                            decoration: this.styleSheet.codeblockDecoration,
                            child: child
                        );
                    }
                    else if (tag == "hr") {
                        child = new DecoratedBox(
                            decoration: this.styleSheet.horizontalRuleDecoration,
                            child: child
                        );
                    }

                    this._addBlockChild(child);
                }
                else {
                    _InlineElement current = this._inlines.removeLast();
                    _InlineElement parent = this._inlines.last();

                    if (tag == "img") {
                        current.children.Add(this._buildImage(element.attributes["src"]));
                    }
                    else if (tag == "br") {
                        current.children.Add(this._buildRichText(new TextSpan("\n")));
                    }
                    else if (tag == "th" || tag == "td") {
                        TextAlign align = TextAlign.left;
                        string style = element.attributes["style"];
                        if (style == null) {
                            align = tag == "th" ? this.styleSheet.tableHeadAlign : TextAlign.left;
                        }
                        else {
                            Regex regExp = new Regex("text-align: (left|center|right)");
                            Match match = regExp.matchAsPrefix(style);
                            switch (match.Groups[0].Value) {
                                case "left":
                                    align = TextAlign.left;
                                    break;
                                case "center":
                                    align = TextAlign.center;
                                    break;
                                case "right":
                                    align = TextAlign.right;
                                    break;
                            }
                        }

                        Widget child = this._buildTableCell(this._mergeInlineChildren(current.children),
                            textAlign: align
                        );
                        this._tables.Single().rows.last().children.Add(child);
                    }
                    else if (tag == "a") {
                        this._linkHandlers.removeLast();
                    }

                    if (current.children.isNotEmpty()) {
                        parent.children.AddRange(current.children);
                    }
                }
            }

            Widget _buildImage(string src) {
                var parts = src.Split('#');
                if (parts.isEmpty()) {
                    return SizedBox.expand();
                }

                string path = parts.first();
                float width = 0, height = 0;
                if (parts.Length == 2) {
                    var dimensions = parts.last().Split('x');
                    if (dimensions.Length == 2) {
                        width = float.Parse(dimensions[0]);
                        height = float.Parse(dimensions[1]);
                    }
                }

                Uri uri = new Uri(path);
                Widget child;
                if (this.imageBuilder != null) {
                    child = this.imageBuilder(uri);
                }
                else {
                    child = this._imageBuilder(uri, this.imageDirectory);
                }

                if (this._linkHandlers.isNotEmpty()) {
                    TapGestureRecognizer recognizer = (TapGestureRecognizer) this._linkHandlers.last();
                    return new GestureDetector(child: child, onTap: recognizer.onTap);
                }
                else {
                    return child;
                }
            }


            Widget _imageBuilder(
                Uri uri,
                string imageDirectory
            ) {
                if (uri.Scheme == "http" || uri.Scheme == "https") {
                    return Image.network(uri.ToString());
                }
                else if (uri.Scheme == "resource") {
                    return Image.asset(uri.AbsolutePath);
                }
                else {
                    Uri fileUri = imageDirectory != null ? new Uri(imageDirectory + uri) : uri;
                    if (fileUri.Scheme == "http" || fileUri.Scheme == "https") {
                        return Image.network(fileUri.ToString());
                    }
                    else {
                        return Image.file(fileUri.ToString());
                    }
                }
            }


            Widget _buildCheckbox(bool hasChecked) {
                if (this.checkboxBuilder != null) {
                    return this.checkboxBuilder(hasChecked);
                }

                return new Padding(
                    padding: EdgeInsets.only(right: 4),
                    child: new Icon(
                        hasChecked ? Icons.check_box : Icons.check_box_outline_blank,
                        size: this.styleSheet.checkbox.fontSize,
                        color: this.styleSheet.checkbox.color
                    )
                );
            }

            Widget _buildBullet(string listTag) {
                if (listTag == "ul") {
                    return new Unity.UIWidgets.widgets.Text("•",
                        textAlign: TextAlign.center,
                        style: this.styleSheet.listBullet
                    );
                }

                int index = this._blocks.last().nextListIndex;
                return new Padding(padding: EdgeInsets.only(right: 4),
                    child: new Unity.UIWidgets.widgets.Text(
                        $"{index + 1}.",
                        textAlign: TextAlign.right,
                        style: this.styleSheet.listBullet
                    ));
            }

            Widget _buildTableCell(List<Widget> children, TextAlign textAlign) {
                return new TableCell(
                    child: new Padding(
                        padding: this.styleSheet.tableCellsPadding,
                        child: new DefaultTextStyle(
                            style: this.styleSheet.tableBody,
                            textAlign: textAlign,
                            child: new Wrap(children: children)
                        )
                    )
                );
            }

            void _addParentInlineIfNeeded(string tag) {
                if (this._inlines.isEmpty()) {
                    this._inlines.Add(new _InlineElement(tag, this.styleSheet.styles(tag)));
                }
            }

            void _addBlockChild(Widget child) {
                _BlockElement parent = this._blocks.last();
                if (parent.children.isNotEmpty()) {
                    parent.children.Add(new SizedBox(height: this.styleSheet.blockSpacing));
                }

                parent.children.Add(child);
                parent.nextListIndex += 1;
            }

            void _addAnonymousBLockIfNeeded() {
                if (this._inlines.isEmpty()) {
                    return;
                }

                _InlineElement inline = this._inlines.Single();
                if (inline.children.isNotEmpty()) {
                    List<Widget> mergedInlines = this._mergeInlineChildren(inline.children);
                    Wrap wrap = new Wrap(null, Axis.horizontal, WrapAlignment.start, 0, WrapAlignment.start, 0,
                        WrapCrossAlignment.start, null, VerticalDirection.down, mergedInlines);
                    this._addBlockChild(wrap);
                    this._inlines.Clear();
                }
            }

            List<Widget> _mergeInlineChildren(List<Widget> children) {
                List<Widget> mergedTexts = new List<Widget>();
                foreach (Widget child in children) {
                    if (mergedTexts.isNotEmpty() && mergedTexts.last() is RichText && child is RichText) {
                        var childText = child as RichText;
                        RichText previous = (RichText) mergedTexts.removeLast();
                        List<TextSpan> textSpans =
                            previous.text.children != null
                                ? previous.text.children
                                : new List<TextSpan> {previous.text};
                        textSpans.Add(childText.text);
                        TextSpan mergedSpan = new TextSpan(children: textSpans);
                        mergedTexts.Add(this._buildRichText(mergedSpan));
                    }
                    else if (mergedTexts.isNotEmpty() &&
                             mergedTexts.last() is SelectableText &&
                             child is SelectableText) {
                        var childText = child as SelectableText;
                        SelectableText previous = (SelectableText) mergedTexts.removeLast();
                        TextSpan previousTextSpan = previous.textSpan;
                        List<TextSpan> textSpans = previousTextSpan.children != null
                            ? previousTextSpan.children
                            : new List<TextSpan> {previous.textSpan};
                        textSpans.Add(childText.textSpan);
                        TextSpan mergedSpan = new TextSpan(children: textSpans);
                        mergedTexts.Add(this._buildRichText(mergedSpan));
                    }
                    else {
                        mergedTexts.Add(child);
                    }
                }

                return mergedTexts;
            }

            Widget _buildRichText(TextSpan text) {
                if (this.selectable) {
                    return SelectableText.rich(
                        text
                    );
                }
                else {
                    return new RichText(
                        text: text,
                        textScaleFactor: this.styleSheet.textScaleFactor
                    );
                }
            }
        }
    }
}