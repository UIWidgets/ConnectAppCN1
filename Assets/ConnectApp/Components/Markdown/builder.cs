using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Image = Unity.UIWidgets.widgets.Image;

namespace markdown {
    public static class builderUtil {
        private static readonly List<string> _kBlockTags = new List<string>() {
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

        private static readonly List<string> _kListTags = new List<string>() {
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

        private bool _isInBlockquote = false;

        public MarkdownBuilder(
            IMarkdownBuilderDelegate dele,
            bool selectable,
            MarkdownStyleSheet styleSheet,
            string imageDirectory,
            MarkdownImageBuilder imageBuilder,
            MarkdownCheckboxBuilder checkboxBuilder,
            bool fitContent = false
        ) {
            this.builderDelegate = dele;
            this.selectable = selectable;
            this.styleSheet = styleSheet;
            this.imageDirectory = imageDirectory;
            this.imageBuilder = imageBuilder;
            this.checkboxBuilder = checkboxBuilder;
            this.fitContent = fitContent;
        }

        public List<Widget> build(List<Node> nodes) {
            _listIndents.Clear();
            _blocks.Clear();
            _tables.Clear();
            _inlines.Clear();
            _linkHandlers.Clear();
            _isInBlockquote = false;

            _blocks.Add(new _BlockElement(null));

            foreach (var node in nodes) {
                Debug.Assert(_blocks.Count == 1);
                node.accept(this);
            }

            Debug.Assert(_tables.isEmpty());
            Debug.Assert(_inlines.isEmpty());
            Debug.Assert(!_isInBlockquote);
            return _blocks.Single().children;
        }

        public override bool visitElementBefore(Element element) {
            var tag = element.tag;
            if (builderUtil._isBlockTag(tag)) {
                _addAnonymousBlockIfNeeded();
                if (builderUtil._isListTag(tag)) {
                    _listIndents.Add(tag);
                }
                else if (tag == "blockquote") {
                    _isInBlockquote = true;
                }
                else if (tag == "table") {
                    _tables.Add(new _TableElement());
                }
                else if (tag == "tr") {
                    var length = _tables.Single().rows.Count;
                    var decoration = styleSheet.tableCellsDecoration;
                    if (length == 0 || length % 2 == 1) decoration = null;
                    _tables.Single().rows.Add(new TableRow(
                        decoration: decoration,
                        children: new List<Widget>()
                    ));
                }

                _blocks.Add(new _BlockElement(tag));
            }
            else {
                _addParentInlineIfNeeded(_blocks.Last().tag);

                var parentStyle = _inlines.Last().style;
                _inlines.Add(new _InlineElement(
                    tag,
                    style: parentStyle.merge(styleSheet.styles.ContainsKey(tag) ? styleSheet.styles[tag] : null)
                ));
            }

            if (tag == "a") {
                _linkHandlers.Add(builderDelegate.createLink(element.attributes["href"]));
            }

            return true;
        }

        public override void visitText(Text text) {
            if (_blocks.Last().tag == null) return;

            _addParentInlineIfNeeded(_blocks.Last().tag);

            Widget child;
            if (_blocks.last().tag == "pre") {
                child = new Scrollbar(
                    child: new SingleChildScrollView(
                        scrollDirection: Axis.horizontal,
                        padding: styleSheet.codeblockPadding,
                        child: _buildRichText(builderDelegate.formatText(styleSheet, text.text))
                    )
                );
            }
            else {
                child = _buildRichText(new TextSpan(
                    style: _isInBlockquote ? _inlines.Last().style.merge(styleSheet.blockquote) : _inlines.Last().style,
                    text: text.text,
                    recognizer: _linkHandlers.isNotEmpty() ? _linkHandlers.Last() : null
                ));
            }

            _inlines.Last().children.Add(child);
        }

        public override void visitElementAfter(Element element) {
            var tag = element.tag;

            if (builderUtil._isBlockTag(tag)) {
                _addAnonymousBlockIfNeeded();

                var current = _blocks.removeLast();
                Widget child;

                if (current.children.isNotEmpty()) {
                    child = new Column(
                        crossAxisAlignment: fitContent ? CrossAxisAlignment.start : CrossAxisAlignment.stretch,
                        children: current.children
                    );
                }
                else {
                    child = new SizedBox();
                }

                if (builderUtil._isListTag(tag)) {
                    Debug.Assert(_listIndents.isNotEmpty());
                    _listIndents.removeLast();
                }
                else if (tag == "li") {
                    if (_listIndents.isNotEmpty()) {
                        if (element.children.Count == 0) {
                            element.children.Add(new Text(""));
                        }

                        Widget bullet;
                        var el = element.children[0];
                        string elType;
                        if (el is Element && (el as Element).attributes.TryGetValue("type", out elType) &&
                            elType == "checkbox") {
                            var val = (el as Element).attributes["checked"] != "false";
                            bullet = _buildCheckBox(val);
                        }
                        else {
                            bullet = _buildBullet(_listIndents.last());
                        }

                        child = new Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                new SizedBox(
                                    width: styleSheet.listIndent,
                                    child: bullet
                                ),
                                new Expanded(child: child)
                            }
                        );
                    }
                }
                else if (tag == "table") {
                    child = new Table(
                        defaultColumnWidth: styleSheet.tableColumnWidth,
                        defaultVerticalAlignment: TableCellVerticalAlignment.middle,
                        border: styleSheet.tableBorder,
                        children: _tables.removeLast().rows
                    );
                }
                else if (tag == "blockquote") {
                    _isInBlockquote = false;
                    child = new DecoratedBox(
                        decoration: styleSheet.blockquoteDecoration,
                        child: new Padding(
                            padding: styleSheet.blockquotePadding,
                            child: child
                        )
                    );
                }
                else if (tag == "pre") {
                    child = new DecoratedBox(
                        decoration: styleSheet.codeblockDecoration,
                        child: child
                    );
                }
                else if (tag == "hr") {
                    child = new DecoratedBox(
                        decoration: styleSheet.horizontalRuleDecoration,
                        child: child
                    );
                }

                _addBlockChild(child);
            }
            else {
                var current = _inlines.removeLast();
                var parent = _inlines.last();

                if (tag == "img") {
                    // create an image widget for this image
                    current.children.Add(_buildImage(element.attributes["src"]));
                }
                else if (tag == "br") {
                    current.children.Add(_buildRichText(new TextSpan(text: "\n")));
                }
                else if (tag == "th" || tag == "td") {
                    TextAlign align = TextAlign.left;
                    String style;
                    element.attributes.TryGetValue("style", out style);
                    if (style == null || style.isEmpty()) {
                        align = tag == "th" ? styleSheet.tableHeadAlign : TextAlign.left;
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

                    Widget child = _buildTableCell(
                        _mergeInlineChildren(current.children),
                        textAlign: align
                    );
                    _tables.Single().rows.last().children.Add(child);
                }
                else if (tag == "a") {
                    _linkHandlers.removeLast();
                }

                if (current.children.isNotEmpty()) {
                    parent.children.AddRange(current.children);
                }
            }
        }

        // _buildImage, buildcheckbox, build bullet, buildTablecell
        Widget _buildImage(string src) {
            var parts = src.Split('#');
            if (parts.isEmpty()) return new SizedBox();

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
                child = _handleDataSchemeUri(uri, width, height);
            }
            else if (uri.Scheme == "resource") {
                //TODO:
                child = Image.asset(path.Substring(9), null, null, width, height);
            }
            else {
                string filePath = imageDirectory == null
                    ? uri.ToString()
                    : System.IO.Path.Combine(imageDirectory, uri.ToString());
                child = Image.file(filePath, null, 1, width, height);
            }

            if (_linkHandlers.isNotEmpty()) {
                TapGestureRecognizer recognizer = _linkHandlers.last() as TapGestureRecognizer;
                return new GestureDetector(null, child, null, null, recognizer.onTap);
            }
            else {
                return child;
            }
        }

        Widget _handleDataSchemeUri(Uri uri, float widht, float height) {
            //TODO:
            return SizedBox.expand();
        }

        Widget _buildCheckBox(bool check) {
            if (checkboxBuilder != null) {
                return checkboxBuilder(check);
            }

            return new Padding(
                padding: EdgeInsets.only(right: 4),
                child: new Icon(
                    check ? Icons.check_box : Icons.check_box_outline_blank,
                    size: styleSheet.checkbox.fontSize,
                    color: styleSheet.checkbox.color
                )
            );
        }

        Widget _buildBullet(string listTag) {
            if (listTag == "ul") {
                return new Unity.UIWidgets.widgets.Text("•", null, styleSheet.listBullet);
            }

            var index = _blocks.last().nextListIndex;
            return new Padding(
                padding: EdgeInsets.only(right: 4),
                child: new Unity.UIWidgets.widgets.Text(
                    data: (index + 1) + ".",
                    style: styleSheet.listBullet,
                    textAlign: TextAlign.right
                )
            );
        }

        Widget _buildTableCell(List<Widget> children, TextAlign textAlign) {
            return new TableCell(
                child: new Padding(
                    padding: styleSheet.tableCellsPadding,
                    child: new DefaultTextStyle(
                        style: styleSheet.tableBody,
                        textAlign: textAlign,
                        child: new Wrap(children: children)
                    )
                )
            );
        }

        void _addParentInlineIfNeeded(string tag) {
            if (_inlines.isEmpty()) {
                _inlines.Add(new _InlineElement(
                    tag,
                    style: styleSheet.styles[tag]
                ));
            }
        }

        void _addBlockChild(Widget child) {
            var parent = _blocks.Last();
            if (parent.children.isNotEmpty()) {
                parent.children.Add(new SizedBox(height: styleSheet.blockSpacing));
            }

            parent.children.Add(child);
            parent.nextListIndex += 1;
        }

        void _addAnonymousBlockIfNeeded() {
            if (_inlines.isEmpty()) return;

            var inline = _inlines.Single();
            if (inline.children.isNotEmpty()) {
                List<Widget> mergedInlines = _mergeInlineChildren(inline.children);
                var wrap = new Wrap(
                    crossAxisAlignment: WrapCrossAlignment.center,
                    children: mergedInlines
                );
                _addBlockChild(wrap);
                _inlines.Clear();
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
                    mergedTexts.Add(_buildRichText(mergedSpan));
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
                    mergedTexts.Add(_buildRichText(mergedSpan));
                }
                else {
                    mergedTexts.Add(child);
                }
            }

            return mergedTexts;
        }


        Widget _buildRichText(TextSpan text) {
            if (selectable) {
                return SelectableText.rich(
                    text
                    //textScaleFactor: styleSheet.textScaleFactor,
                );
            }
            else {
                return new RichText(
                    text: text,
                    textScaleFactor: styleSheet.textScaleFactor
                );
            }
        }
    }
}