using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using html;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Image = Unity.UIWidgets.widgets.Image;
using Path = System.IO.Path;

namespace markdown {
    public static class builderUtil {
        static readonly List<string> _kBlockTags = new List<string>() {
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

        public static bool _isBlockTag(string tag) {
            return _kBlockTags.Contains(tag);
        }

        public static bool _isListTag(string tag) {
            return _kListTags.Contains(tag);
        }
    }

    public class _BlockElement {
        public string tag;
        public List<Widget> children = new List<Widget>();
        public int nextListIndex = 0;

        public _BlockElement(string tag) {
            this.tag = tag;
        }
    }

    public class _TableElement {
        public List<TableRow> rows = new List<TableRow>();
    }

    public class _InlineElement {
        public string tag;
        public TextStyle style;
        public List<Widget> children = new List<Widget>();

        public _InlineElement(string tag, TextStyle style) {
            this.tag = tag;
            this.style = style;
        }
    }

    public interface IMarkdownBuilderDelegate {
        GestureRecognizer createLink(string href);
        TextSpan formatText(MarkdownStyleSheet styleSheet, string code);
    }

    public class MarkdownBuilder : NodeVisitor {
        public IMarkdownBuilderDelegate builderDelegate;
        public bool selectable;
        public MarkdownStyleSheet styleSheet;
        public string imageDirectory;
        public MarkdownImageBuilder imageBuilder;
        public MarkdownCheckboxBuilder checkboxBuilder;
        public bool fitContent;
        public List<string> _listIndents = new List<string>();
        public List<_BlockElement> _blocks = new List<_BlockElement>();
        public List<_TableElement> _tables = new List<_TableElement>();
        public List<_InlineElement> _inlines = new List<_InlineElement>();
        public List<GestureRecognizer> _linkHandlers = new List<GestureRecognizer>();
        public bool enableHTML;

        bool _isInBlockquote = false;

        public MarkdownBuilder(
            IMarkdownBuilderDelegate dele,
            bool selectable,
            MarkdownStyleSheet styleSheet,
            string imageDirectory,
            MarkdownImageBuilder imageBuilder,
            MarkdownCheckboxBuilder checkboxBuilder,
            bool fitContent = false,
            bool enableHTML = true
        ) {
            this.builderDelegate = dele;
            this.selectable = selectable;
            this.styleSheet = styleSheet;
            this.imageDirectory = imageDirectory;
            this.imageBuilder = imageBuilder;
            this.checkboxBuilder = checkboxBuilder;
            this.fitContent = fitContent;
            this.enableHTML = enableHTML;
        }

        public List<Widget> build(List<Node> nodes) {
            this._listIndents.Clear();
            this._blocks.Clear();
            this._tables.Clear();
            this._inlines.Clear();
            this._linkHandlers.Clear();
            this._isInBlockquote = false;

            this._blocks.Add(new _BlockElement(null));

            foreach (var node in nodes) {
                Debug.Assert(this._blocks.Count == 1);
                node.accept(this);
            }

            Debug.Assert(this._tables.isEmpty());
            Debug.Assert(this._inlines.isEmpty());
            Debug.Assert(!this._isInBlockquote);
            return this._blocks.Single().children;
        }

        public override bool visitElementBefore(Element element) {
            var tag = element.tag;
            if (builderUtil._isBlockTag(tag)) {
                this._addAnonymousBlockIfNeeded();
                if (builderUtil._isListTag(tag)) {
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
                this._addParentInlineIfNeeded(this._blocks.Last().tag);

                var parentStyle = this._inlines.Last().style;
                this._inlines.Add(new _InlineElement(
                    tag,
                    style: parentStyle.merge(this.styleSheet.styles.ContainsKey(tag)
                        ? this.styleSheet.styles[tag]
                        : null)
                ));
            }

            if (tag == "a") {
                this._linkHandlers.Add(this.builderDelegate.createLink(element.attributes["href"]));
            }

            return true;
        }

        public override void visitText(Text text) {
            if (this.enableHTML && text is HTML html) {
                this.visitHTML(html);
                return;
            }

            if (this._blocks.Last().tag == null) {
                return;
            }

            this._addParentInlineIfNeeded(this._blocks.Last().tag);

            Widget child;
            if (this._blocks.last().tag == "pre") {
                child = new Scrollbar(
                    child: new SingleChildScrollView(
                        scrollDirection: Axis.horizontal,
                        padding: this.styleSheet.codeblockPadding,
                        child: this._buildRichText(this.builderDelegate.formatText(this.styleSheet, text.text))
                    )
                );
            }
            else {
                child = this._buildRichText(new TextSpan(
                    style: this._isInBlockquote
                        ? this._inlines.Last().style.merge(this.styleSheet.blockquote)
                        : this._inlines.Last().style,
                    text: text.text,
                    recognizer: this._linkHandlers.isNotEmpty() ? this._linkHandlers.Last() : null
                ));
            }

            this._inlines.Last().children.Add(child);
        }

        void visitHTML(HTML html) {
            this._blocks.Last().children.Add(
                new HtmlView(data: html.textContent)
            );
        }

        public override void visitElementAfter(Element element) {
            var tag = element.tag;

            if (builderUtil._isBlockTag(tag)) {
                this._addAnonymousBlockIfNeeded();

                var current = this._blocks.removeLast();
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

                if (builderUtil._isListTag(tag)) {
                    Debug.Assert(this._listIndents.isNotEmpty());
                    this._listIndents.removeLast();
                }
                else if (tag == "li") {
                    if (this._listIndents.isNotEmpty()) {
                        if (element.children.Count == 0) {
                            element.children.Add(new Text(""));
                        }

                        Widget bullet;
                        var el = element.children[0];
                        string elType;
                        if (el is Element && (el as Element).attributes.TryGetValue("type", out elType) &&
                            elType == "checkbox") {
                            var val = (el as Element).attributes["checked"] != "false";
                            bullet = this._buildCheckBox(val);
                        }
                        else {
                            bullet = this._buildBullet(this._listIndents.last());
                        }

                        child = new Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
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
                var current = this._inlines.removeLast();
                var parent = this._inlines.last();

                if (tag == "img") {
                    // create an image widget for this image
                    current.children.Add(this._buildImage(element.attributes["src"]));
                }
                else if (tag == "br") {
                    current.children.Add(this._buildRichText(new TextSpan(text: "\n")));
                }
                else if (tag == "th" || tag == "td") {
                    TextAlign align = TextAlign.left;
                    string style;
                    element.attributes.TryGetValue("style", out style);
                    if (style == null || style.isEmpty()) {
                        align = tag == "th" ? this.styleSheet.tableHeadAlign : TextAlign.left;
                    }
                    else {
                        var regExp = new Regex(@"text-align: (left|center|right)");
                        Match match = regExp.matchAsPrefix(style);
                        if (match.Success) {
                            switch (match.Groups[1].Value) {
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

        // _buildImage, buildcheckbox, build bullet, buildTablecell
        Widget _buildImage(string src) {
            var parts = src.Split('#');
            if (parts.isEmpty()) {
                return new SizedBox();
            }

            var path = parts.first();
            float width = 0, height = 0;
            if (parts.Length == 2) {
                var dimensions = parts.last().Split('x');
                if (dimensions.Length == 2) {
                    width = float.Parse(dimensions[0]);
                    height = float.Parse(dimensions[1]);
                }
            }

            var uri = new Uri(path);
            Widget child;
            if (uri.Scheme == "http" || uri.Scheme == "https") {
                child = Image.network(uri.ToString(), null, 1);
            }
            else if (uri.Scheme == "data") {
                // todo
                child = this._handleDataSchemeUri(uri, width, height);
            }
            else if (uri.Scheme == "resource") {
                //TODO:
                child = Image.asset(path.Substring(9), null, null, width, height);
            }
            else {
                string filePath = this.imageDirectory == null
                    ? uri.ToString()
                    : Path.Combine(this.imageDirectory, uri.ToString());
                child = Image.file(filePath, null, 1, width, height);
            }

            if (this._linkHandlers.isNotEmpty()) {
                TapGestureRecognizer recognizer = this._linkHandlers.last() as TapGestureRecognizer;
                return new GestureDetector(null, child, null, null, recognizer.onTap);
            }
            else {
                return child;
            }
        }

        Widget _handleDataSchemeUri(Uri uri, float width, float height) {
            //TODO:
            return SizedBox.expand();
        }

        Widget _buildCheckBox(bool check) {
            if (this.checkboxBuilder != null) {
                return this.checkboxBuilder(check);
            }

            return new Padding(
                padding: EdgeInsets.only(right: 4),
                child: new Icon(
                    check ? Icons.check_box : Icons.check_box_outline_blank,
                    size: this.styleSheet.checkbox.fontSize,
                    color: this.styleSheet.checkbox.color
                )
            );
        }

        Widget _buildBullet(string listTag) {
            if (listTag == "ul") {
                return new Unity.UIWidgets.widgets.Text("•", null, this.styleSheet.listBullet);
            }

            var index = this._blocks.last().nextListIndex;
            return new Padding(
                padding: EdgeInsets.only(right: 4),
                child: new Unity.UIWidgets.widgets.Text(
                    data: (index + 1) + ".",
                    style: this.styleSheet.listBullet,
                    textAlign: TextAlign.right
                )
            );
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
                this._inlines.Add(new _InlineElement(
                    tag,
                    style: this.styleSheet.styles[tag]
                ));
            }
        }

        void _addBlockChild(Widget child) {
            var parent = this._blocks.Last();
            if (parent.children.isNotEmpty()) {
                parent.children.Add(new SizedBox(height: this.styleSheet.blockSpacing));
            }

            parent.children.Add(child);
            parent.nextListIndex += 1;
        }

        void _addAnonymousBlockIfNeeded() {
            if (this._inlines.isEmpty()) {
                return;
            }

            var inline = this._inlines.Single();
            if (inline.children.isNotEmpty()) {
                List<Widget> mergedInlines = this._mergeInlineChildren(inline.children);
                var wrap = new Wrap(
                    crossAxisAlignment: WrapCrossAlignment.center,
                    children: mergedInlines
                );
                this._addBlockChild(wrap);
                this._inlines.Clear();
            }
        }

        List<Widget> _mergeInlineChildren(List<Widget> inLineChildren) {
            List<Widget> mergedTexts = new List<Widget>();
            foreach (var child in inLineChildren) {
                if (mergedTexts.isNotEmpty() && mergedTexts.Last() is RichText && child is RichText) {
                    RichText previous = (RichText) mergedTexts.removeLast();
                    TextSpan previousTextSpan = previous.text;
                    List<TextSpan> children = previousTextSpan.children != null
                        ? previousTextSpan.children
                        : new List<TextSpan>() {previousTextSpan};
                    children.Add((child as RichText).text);
                    var mergedSpan = new TextSpan(children: children);
                    mergedTexts.Add(this._buildRichText(mergedSpan));
                }
                else if (mergedTexts.isNotEmpty() &&
                         mergedTexts.Last() is SelectableText &&
                         child is SelectableText) {
                    var previous = (SelectableText) mergedTexts.removeLast();
                    var previousTextSpan = previous.textSpan;
                    List<TextSpan> children = previousTextSpan.children != null
                        ? previousTextSpan.children
                        : new List<TextSpan>() {previousTextSpan};
                    children.Add((child as SelectableText).textSpan);
                    TextSpan mergedSpan = new TextSpan(children: children);
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
                    //textScaleFactor: styleSheet.textScaleFactor,
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