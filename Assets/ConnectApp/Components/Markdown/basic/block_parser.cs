using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.UIWidgets.foundation;

namespace markdown {
    public class BlockParserUtil {
        /// The line contains only whitespace or is empty.
        public static Regex _emptyPattern = new Regex(@"^(?:[ \t]*)$");

        /// A series of `=` or `-` (on the next line) define setext-style headers.
        public static Regex _setextPattern = new Regex(@"^[ ]{0,3}(=+|-+)\s*$");

        /// Leading (and trailing) `#` define atx-style headers.
        ///
        /// Starts with 1-6 unescaped `#` characters which must not be followed by a
        /// non-space character. Line may end with any number of `#` characters,.
        public static Regex _headerPattern = new Regex(@"^ {0,3}(#{1,6})[ \x09\x0b\x0c](.*?)#*$");

        /// The line starts with `>` with one optional space after.
        public static Regex _blockquotePattern = new Regex(@"^[ ]{0,3}>[ ]?(.*)$");

        /// A line indented four spaces. Used for code blocks and lists.
        public static Regex _indentPattern = new Regex(@"^(?:    | {0,3}\t)(.*)$");

        /// Fenced code block.
        public static Regex _codePattern = new Regex(@"^[ ]{0,3}(`{3,}|~{3,})(.*)$");

        /// Three or more hyphens, asterisks or underscores by themselves. Note that
        /// a line like `----` is valid as both HR and SETEXT. In case of a tie,
        /// SETEXT should win.
        public static Regex _hrPattern = new Regex(@"^ {0,3}([-*_])[ \t]*\1[ \t]*\1(?:\1|[ \t])*$");

        /// One or more whitespace, for compressing.
        public static Regex _oneOrMoreWhitespacePattern = new Regex(@"[ \n\r\t]+");

        /// A line starting with one of these markers: `-`, `*`, `+`. May have up to
        /// three leading spaces before the marker and any number of spaces or tabs
        /// after.
        ///
        /// Contains a dummy group at [2], so that the groups in [_ulPattern] and
        /// [_olPattern] match up; in both, [2] is the length of the number that begins
        /// the list marker.
        public static Regex _ulPattern = new Regex(@"^([ ]{0,3})()([*+-])(([ \t])([ \t]*)(.*))?$");

        /// A line starting with a number like `123.`. May have up to three leading
        /// spaces before the marker and any number of spaces or tabs after.
        public static Regex _olPattern = new Regex(@"^([ ]{0,3})(\d{1,9})([\.)])(([ \t])([ \t]*)(.*))?$");

        /// A line of hyphens separated by at least one pipe.
        public static Regex _tablePattern = new Regex(@"^[ ]{0,3}\|?( *:?\-+:? *\|)+( *:?\-+:? *)?$");
    }

    public class BlockParser {
        public List<string> lines;

        public Document document;

        public List<BlockSyntax> blockSyntaxes = new List<BlockSyntax>();

        int _pos = 0;

        public bool encounteredBlankLine = false;

        List<BlockSyntax> standardBlockSyntaxes = new List<BlockSyntax>() {
            new EmptyBlockSyntax(),
            new BlockTagBlockHtmlSyntax(),
            new LongBlockHtmlSyntax(@"^ {0,3}<pre(?:\s|>|$)", "</pre>"),
            new LongBlockHtmlSyntax(@"^ {0,3}<script(?:\s|>|$)", "</script>"),
            new LongBlockHtmlSyntax(@"^ {0,3}<style(?:\s|>|$)", "</style>"),
            new LongBlockHtmlSyntax(@"^ {0,3}<!--", "-->"),
            new LongBlockHtmlSyntax(@"^ {0,3}<\?", "\\?>"),
            new LongBlockHtmlSyntax(@"^ {0,3}<![A-Z]", ">"),
            new LongBlockHtmlSyntax(@"^ {0,3}<!\[CDATA\[", "\\]\\]>"),
            new OtherTagBlockHtmlSyntax(),
            new SetextHeaderSyntax(),
            new HeaderSyntax(),
            new CodeBlockSyntax(),
            new BlockquoteSyntax(),
            new HorizontalRuleSyntax(),
            new UnorderedListSyntax(),
            new OrderedListSyntax(),
            new ParagraphSyntax()
        };

        public BlockParser(List<string> lines, Document document) {
            this.lines = lines;
            this.document = document;

            this.blockSyntaxes.AddRange(document.blockSyntaxes);
            this.blockSyntaxes.AddRange(this.standardBlockSyntaxes);
        }

        public string current {
            get {
                if (this._pos < this.lines.Count && this._pos >= 0) {
                    return this.lines[this._pos];
                }
                else {
                    return null;
                }
            }
        }

        public string next {
            get {
                if (this._pos >= this.lines.Count - 1) {
                    return null;
                }

                return this.lines[this._pos + 1];
            }
        }

        public string peek(int linesAhead) {
            if (linesAhead < 0) {
                throw new ArgumentException(string.Format("Invalid linesAhead: {0}; must be >= 0.", linesAhead));
            }

            if (this._pos >= this.lines.Count - linesAhead) {
                return null;
            }

            return this.lines[this._pos + linesAhead];
        }

        public void advance() {
            this._pos++;
        }

        public bool isDone {
            get { return this._pos >= this.lines.Count; }
        }

        public bool matches(Regex regex) {
            if (this.isDone) {
                return false;
            }

            return regex.Match(this.current).Success;
        }

        public bool matchesNext(Regex regex) {
            if (this.next == null) {
                return false;
            }

            return regex.Match(this.next).Success;
        }

        public List<Node> parseLines() {
            var blocks = new List<Node>();
            while (!this.isDone) {
                foreach (var syntax in this.blockSyntaxes) {
                    if (syntax.canParse(this)) {
                        var block = syntax.parse(this);
                        if (block != null) {
                            blocks.Add(block);
                        }

                        break;
                    }
                }
            }

            return blocks;
        }
    }

    public abstract class BlockSyntax {
        public virtual Regex pattern {
            get { return null; }
        }

        public virtual bool canEndBlock {
            get { return true; }
        }

        public virtual bool canParse(BlockParser parser) {
            return this.pattern.hasMatch(parser.current);
        }

        public abstract Node parse(BlockParser parser);

        public virtual List<string> parseChildLines(BlockParser parser) {
            var childLines = new List<string>();

            while (!parser.isDone) {
                var match = this.pattern.Match(parser.current);
                if (!match.Success) {
                    break;
                }

                childLines.Add(match.Groups[1].Value);

                parser.advance();
            }

            return childLines;
        }

        protected static bool isAtBlockEnd(BlockParser parser) {
            if (parser.isDone) {
                return true;
            }

            return parser.blockSyntaxes.Any(s => s.canParse(parser) && s.canEndBlock);
        }

        public static string generateAnchorHash(Element element) {
            var str = element.children.first().textContent.ToLower().Trim();
            var regStr = @"[^a-z0-9 _-]";
            return Regex.Replace(Regex.Replace(str, regStr, ""), @"\s", "-");
        }
    }

    class EmptyBlockSyntax : BlockSyntax {
        public override Regex pattern {
            get { return BlockParserUtil._emptyPattern; }
        }

        public override Node parse(BlockParser parser) {
            parser.encounteredBlankLine = true;
            parser.advance();

            return null;
        }
    }

    class SetextHeaderSyntax : BlockSyntax {
        public override bool canParse(BlockParser parser) {
            if (!this._interperableAsParagraph(parser.current)) {
                return false;
            }

            var i = 1;
            while (true) {
                var nextLine = parser.peek(i);
                if (nextLine == null) {
                    return false;
                }

                if (BlockParserUtil._setextPattern.hasMatch(nextLine)) {
                    return true;
                }

                if (!this._interperableAsParagraph(nextLine)) {
                    return false;
                }

                i++;
            }
        }

        public override Node parse(BlockParser parser) {
            var lines = new List<string>();
            var tag = string.Empty;
            while (!parser.isDone) {
                var match = BlockParserUtil._setextPattern.Match(parser.current);
                if (!match.Success) {
                    lines.Add(parser.current);
                    parser.advance();
                    continue;
                }
                else {
                    tag = match.Groups[1].Value[0] == '=' ? "h1" : "h2";
                    parser.advance();
                    break;
                }
            }

            var contents = new UnparsedContent(lines.join('\n'));
            return new Element(tag, new List<Node>() {contents});
        }

        public bool _interperableAsParagraph(string line) {
            return !(BlockParserUtil._indentPattern.hasMatch(line) ||
                     BlockParserUtil._codePattern.hasMatch(line) ||
                     BlockParserUtil._headerPattern.hasMatch(line) ||
                     BlockParserUtil._blockquotePattern.hasMatch(line) ||
                     BlockParserUtil._hrPattern.hasMatch(line) ||
                     BlockParserUtil._ulPattern.hasMatch(line) ||
                     BlockParserUtil._olPattern.hasMatch(line) ||
                     BlockParserUtil._emptyPattern.hasMatch(line));
        }
    }

    class SetextHeaderWithIdSyntax : SetextHeaderSyntax {
        public override Node parse(BlockParser parser) {
            var element = base.parse(parser) as Element;
            element.generatedId = generateAnchorHash(element);
            return element;
        }
    }

    class HeaderSyntax : BlockSyntax {
        public override Regex pattern {
            get { return BlockParserUtil._headerPattern; }
        }

        public override Node parse(BlockParser parser) {
            var match = this.pattern.Match(parser.current);
            parser.advance();
            var level = match.Groups[1].Length;
            var contents = new UnparsedContent(match.Groups[2].Value.Trim());
            return new Element("h" + level, new List<Node>() {contents});
        }
    }

    class HeaderWithIdSyntax : HeaderSyntax {
        public override Node parse(BlockParser parser) {
            var element = base.parse(parser) as Element;
            element.generatedId = generateAnchorHash(element);
            return element;
        }
    }

    class BlockquoteSyntax : BlockSyntax {
        public override Regex pattern {
            get { return BlockParserUtil._blockquotePattern; }
        }

        public override List<string> parseChildLines(BlockParser parser) {
            var childLines = new List<string>();

            while (!parser.isDone) {
                var match = this.pattern.Match(parser.current);
                if (match.Success) {
                    childLines.Add(match.Groups[1].Value);
                    parser.advance();
                    continue;
                }

                if (parser.blockSyntaxes.First(s => s.canParse(parser)) is ParagraphSyntax) {
                    childLines.Add(parser.current);
                    parser.advance();
                }
                else {
                    break;
                }
            }

            return childLines;
        }

        public override Node parse(BlockParser parser) {
            var childLines = this.parseChildLines(parser);

            var children = new BlockParser(childLines, parser.document).parseLines();

            return new Element("blockquote", children);
        }
    }

    class CodeBlockSyntax : BlockSyntax {
        public override Regex pattern {
            get { return BlockParserUtil._indentPattern; }
        }

        public override bool canEndBlock {
            get { return false; }
        }

        public override List<string> parseChildLines(BlockParser parser) {
            var childLines = new List<string>();

            while (!parser.isDone) {
                var match = this.pattern.Match(parser.current);
                if (match.Success) {
                    childLines.Add(match.Groups[1].Value);
                    parser.advance();
                }
                else {
                    // If there's a codeblock, then a newline, then a codeblock, keep the
                    // code blocks together.
                    var nextMatch =
                        parser.next != null ? this.pattern.Match(parser.next) : null;
                    if (parser.current.Trim() == "" && nextMatch != null && nextMatch.Success) {
                        childLines.Add("");
                        childLines.Add(nextMatch.Groups[1].Value);
                        parser.advance();
                        parser.advance();
                    }
                    else {
                        break;
                    }
                }
            }

            return childLines;
        }

        public override Node parse(BlockParser parser) {
            var childLines = this.parseChildLines(parser);

            // The Markdown tests expect a trailing newline.
            childLines.Add("");

            // todo 
            var escaped = Utils.escapeHtml(childLines.@join("\n"));

            return new Element("pre", new List<Node>() {Element.text("code", escaped)});
        }
    }

    class FencedCodeBlockSyntax : BlockSyntax {
        public override Regex pattern {
            get { return BlockParserUtil._codePattern; }
        }


        List<string> parseChildLines(BlockParser parser, string endBlock = "") {
            if (endBlock == null) {
                endBlock = "";
            }

            var childLines = new List<string>();
            parser.advance();

            while (!parser.isDone) {
                var match = this.pattern.Match(parser.current);
                if (!match.Success || !match.Groups[1].Value.StartsWith(endBlock)) {
                    childLines.Add(parser.current);
                    parser.advance();
                }
                else {
                    parser.advance();
                    break;
                }
            }

            return childLines;
        }

        public override Node parse(BlockParser parser) {
            // Get the syntax identifier, if there is one.
            var match = this.pattern.Match(parser.current);
            var endBlock = match.Groups[1].Value;
            var infoString = match.Groups[2].Value;

            var childLines = this.parseChildLines(parser, endBlock);

            // The Markdown tests expect a trailing newline.
            childLines.Add("");

            var text = childLines.join('\n');
            if (parser.document.encodeHtml) {
                // Escape the code.
                text = Utils.escapeHtml(text);
            }

            var code = Element.text("code", text);

            // the info-string should be trimmed
            // http://spec.commonmark.org/0.22/#example-100
            infoString = infoString.Trim();
            if (infoString.isNotEmpty()) {
                // only use the first word in the syntax
                // http://spec.commonmark.org/0.22/#example-100
                infoString = infoString.Split(' ').first();
                code.attributes["class"] = "language-" + infoString;
            }

            var element = new Element("pre", new List<Node>() {code});

            return element;
        }
    }


    class HorizontalRuleSyntax : BlockSyntax {
        public override Regex pattern {
            get { return BlockParserUtil._hrPattern; }
        }


        public override Node parse(BlockParser parser) {
            parser.advance();
            return Element.empty("hr");
        }
    }

    abstract class BlockHtmlSyntax : BlockSyntax {
        public override bool canEndBlock {
            get { return true; }
        }
    }

    class BlockTagBlockHtmlSyntax : BlockHtmlSyntax {
        static Regex _pattern = new Regex(
            @"^ {0,3}</?(?:address|article|aside|base|basefont|blockquote|body|caption|center|col|colgroup|dd|details|dialog|dir|div|dl|dt|fieldset|figcaption|figure|footer|form|frame|frameset|h1|head|header|hr|html|iframe|legend|li|link|main|menu|menuitem|meta|nav|noframes|ol|optgroup|option|p|param|section|source|summary|table|tbody|td|tfoot|th|thead|title|tr|track|ul)(?:\s|>|/>|$)");

        public override Regex pattern {
            get { return _pattern; }
        }

        public override Node parse(BlockParser parser) {
            var childLines = new List<string>();

            // Eat until we hit a blank line.
            while (!parser.isDone && !parser.matches(BlockParserUtil._emptyPattern)) {
                childLines.Add(parser.current);
                parser.advance();
            }

            return new HTML(childLines.join("\n"));
        }
    }

    class OtherTagBlockHtmlSyntax : BlockTagBlockHtmlSyntax {
        public override bool canEndBlock {
            get { return true; }
        }

        public override Regex pattern {
            get { return new Regex(@"^ {0,3}</?\w+(?:>|\s+[^>]*>)"); }
        }
    }

    class LongBlockHtmlSyntax : BlockHtmlSyntax {
        Regex _pattern;

        public override Regex pattern {
            get { return this._pattern; }
        }

        public Regex _endPattern;

        public LongBlockHtmlSyntax(string patternStr, string endPatternStr) {
            this._pattern = new Regex(patternStr);
            this._endPattern = new Regex(endPatternStr);
        }

        public override Node parse(BlockParser parser) {
            var childLines = new List<string>();
            // Eat until we hit [endPattern].
            while (!parser.isDone) {
                childLines.Add(parser.current);
                if (parser.matches(this._endPattern)) {
                    break;
                }

                parser.advance();
            }

            parser.advance();
            return new HTML(childLines.join("\n"));
        }
    }

    class ListItem {
        internal bool forceBlock = false;
        public List<string> lines;

        public ListItem(List<string> lines) {
            this.lines = lines;
        }
    }

    abstract class ListSyntax : BlockSyntax {
        public override bool canEndBlock {
            get { return true; }
        }

        public abstract string listTag { get; }


        /// A list of patterns that can start a valid block within a list item.
        static List<Regex> blocksInList = new List<Regex>() {
            BlockParserUtil._blockquotePattern,
            BlockParserUtil._headerPattern,
            BlockParserUtil._hrPattern,
            BlockParserUtil._indentPattern,
            BlockParserUtil._ulPattern,
            BlockParserUtil._olPattern
        };

        static Regex _whitespaceRe = new Regex("[ \t]*");

        void endItem(ref List<string> childLines, List<ListItem> items) {
            if (childLines.isNotEmpty()) {
                items.Add(new ListItem(childLines));
                childLines = new List<string>();
            }
        }

        bool tryMatch(Regex pattern, BlockParser parser, ref Match match) {
            match = pattern.Match(parser.current);
            return match.Success;
        }

        public override Node parse(BlockParser parser) {
            var items = new List<ListItem>();
            var childLines = new List<string>();


            Match match = null;


            string listMarker = null;
            string indent = null;
            // In case the first number in an ordered list is not 1, use it as the
            // "start".
            int startNumber = 0;

            while (!parser.isDone) {
                var leadingSpace = _whitespaceRe.matchAsPrefix(parser.current).Groups[0].Value;
                var leadingExpandedTabLength = _expandedTabLength(leadingSpace);
                if (this.tryMatch(BlockParserUtil._emptyPattern, parser, ref match)) {
                    if (BlockParserUtil._emptyPattern.Match(parser.next ?? "").Success) {
                        // Two blank lines ends a list.
                        break;
                    }

                    // Add a blank line to the current list item.
                    childLines.Add("");
                }
                else if (!string.IsNullOrEmpty(indent) && indent.Length <= leadingExpandedTabLength) {
                    // Strip off indent and add to current item.
                    var line = parser.current
                        .replaceFirst(leadingSpace, new string(' ', leadingExpandedTabLength))
                        .replaceFirst(indent, "");
                    childLines.Add(line);
                }
                else if (this.tryMatch(BlockParserUtil._hrPattern, parser, ref match)) {
                    // Horizontal rule takes precedence to a new list item.
                    break;
                }
                else if (this.tryMatch(BlockParserUtil._ulPattern, parser, ref match) ||
                         this.tryMatch(BlockParserUtil._olPattern, parser, ref match)) {
                    var precedingWhitespace = match.Groups[1].Value;
                    var digits = match.Groups[2].Value ?? "";
                    if (startNumber == 0 && digits.isNotEmpty()) {
                        startNumber = int.Parse(digits);
                    }

                    var marker = match.Groups[3];
                    var firstWhitespace = match.Groups[5].Value ?? "";
                    var restWhitespace = match.Groups[6].Value ?? "";
                    var content = match.Groups[7].Value ?? "";
                    var isBlank = content.isEmpty();
                    if (listMarker != null && listMarker != marker.Value) {
                        // Changing the bullet or ordered list delimiter starts a new list.
                        break;
                    }

                    listMarker = marker.Value;
                    var markerAsSpaces = new string(' ', digits.Length + marker.Length);
                    if (isBlank) {
                        // See http://spec.commonmark.org/0.28/#list-items under "3. Item
                        // starting with a blank line."
                        //
                        // If the list item starts with a blank line, the final piece of the
                        // indentation is just a single space.
                        indent = precedingWhitespace + markerAsSpaces + ' ';
                    }
                    else if (restWhitespace.Length >= 4) {
                        // See http://spec.commonmark.org/0.28/#list-items under "2. Item
                        // starting with indented code."
                        //
                        // If the list item starts with indented code, we need to _not_ count
                        // any indentation past the required whitespace character.
                        indent = precedingWhitespace + markerAsSpaces + firstWhitespace;
                    }
                    else {
                        indent = precedingWhitespace +
                                 markerAsSpaces +
                                 firstWhitespace +
                                 restWhitespace;
                    }

                    // End the current list item and start a new one.
                    this.endItem(ref childLines, items);
                    childLines.Add(restWhitespace + content);
                }
                else if (isAtBlockEnd(parser)) {
                    // Done with the list.
                    break;
                }
                else {
                    // If the previous item is a blank line, this means we're done with the
                    // list and are starting a new top-level paragraph.
                    if ((childLines.isNotEmpty()) && (childLines.last() == "")) {
                        parser.encounteredBlankLine = true;
                        break;
                    }

                    // Anything else is paragraph continuation text.
                    childLines.Add(parser.current);
                }

                parser.advance();
            }

            this.endItem(ref childLines, items);
            var itemNodes = new List<Node>();

            items.ForEach(this.removeLeadingEmptyLine);
            var anyEmptyLines = this.removeTrailingEmptyLines(items);
            var anyEmptyLinesBetweenBlocks = false;

            foreach (var item in items) {
                var itemParser = new BlockParser(item.lines, parser.document);
                var children = itemParser.parseLines();
                itemNodes.Add(new Element("li", children));
                anyEmptyLinesBetweenBlocks =
                    anyEmptyLinesBetweenBlocks || itemParser.encounteredBlankLine;
            }

            // Must strip paragraph tags if the list is "tight".
            // http://spec.commonmark.org/0.28/#lists
            var listIsTight = !anyEmptyLines && !anyEmptyLinesBetweenBlocks;

            if (listIsTight) {
                // We must post-process the list items, converting any top-level paragraph
                // elements to just text elements.
                foreach (var item in itemNodes) {
                    var element = item as Element;
                    if (element == null) {
                        continue;
                    }

                    for (var i = 0; i < element.children.Count; i++) {
                        var child = element.children[i];
                        var ele = child as Element;
                        if (ele != null && ele.tag == "p") {
                            element.children.RemoveAt(i);
                            element.children.InsertRange(i, ele.children);
                        }
                    }
                }
            }

            if (this.listTag == "ol" && startNumber != 1) {
                var element = new Element(this.listTag, itemNodes);
                element.attributes["start"] = startNumber.ToString();
                return element;
            }
            else {
                return new Element(this.listTag, itemNodes);
            }
        }

        void removeLeadingEmptyLine(ListItem item) {
            if (item.lines.isNotEmpty() && BlockParserUtil._emptyPattern.hasMatch(item.lines.first())) {
                item.lines.RemoveAt(0);
            }
        }

        /// Removes any trailing empty lines and notes whether any items are separated
        /// by such lines.
        bool removeTrailingEmptyLines(List<ListItem> items) {
            var anyEmpty = false;
            for (var i = 0; i < items.Count; i++) {
                if (items[i].lines.Count == 1) {
                    continue;
                }

                while (items[i].lines.isNotEmpty() &&
                       BlockParserUtil._emptyPattern.hasMatch(items[i].lines.last())) {
                    if (i < items.Count - 1) {
                        anyEmpty = true;
                    }

                    items[i].lines.removeLast();
                }
            }

            return anyEmpty;
        }

        static int _expandedTabLength(string input) {
            var length = 0;
            foreach (var cha in input) {
                length += cha == 0x9 ? 4 - (length % 4) : 1;
            }

            return length;
        }
    }

    class UnorderedListSyntax : ListSyntax {
        public override Regex pattern {
            get { return BlockParserUtil._ulPattern; }
        }

        public override string listTag {
            get { return "ul"; }
        }
    }

    class OrderedListSyntax : ListSyntax {
        public override Regex pattern {
            get { return BlockParserUtil._olPattern; }
        }

        public override string listTag {
            get { return "ol"; }
        }
    }

    class TableSyntax : BlockSyntax {
        static Regex _pipePattern = new Regex(@"\s*\|\s*");
        static Regex _openingPipe = new Regex(@"^\|\s*");
        static Regex _closingPipe = new Regex(@"\s*\|$");

        public override bool canEndBlock {
            get { return false; }
        }


        public override bool canParse(BlockParser parser) {
            // Note: matches *next* line, not the current one. We're looking for the
            // bar separating the head row from the body rows.
            return parser.matchesNext(BlockParserUtil._tablePattern);
        }

        /// Parses a table into its three parts:
        ///
        /// * a head row of head cells (`<th>` cells)
        /// * a divider of hyphens and pipes (not rendered)
        /// * many body rows of body cells (`<td>` cells)
        public override Node parse(BlockParser parser) {
            var alignments = this.parseAlignments(parser.next);
            var columnCount = alignments.Count;
            var headRow = this.parseRow(parser, alignments, "th");
            if (headRow.children.Count != columnCount) {
                return null;
            }

            var head = new Element("thead", new List<Node>() {headRow});

            // Advance past the divider of hyphens.
            parser.advance();

            var rows = new List<Node>() { };
            while (!parser.isDone && !isAtBlockEnd(parser)) {
                var row = this.parseRow(parser, alignments, "td");
                while (row.children.Count < columnCount) {
                    // Insert synthetic empty cells.
                    row.children.Add(Element.empty("td"));
                }

                while (row.children.Count > columnCount) {
                    row.children.removeLast();
                }

                rows.Add(row);
            }

            if (rows.isEmpty()) {
                return new Element("table", new List<Node>() {head});
            }
            else {
                var body = new Element("tbody", rows);

                return new Element("table", new List<Node>() {head, body});
            }
        }

        List<string> parseAlignments(string line) {
            line = line.replaceFirst(_openingPipe, "").replaceFirst(_closingPipe, "");
            return line.Split('|').Select(column => {
                column = column.Trim();
                if (column.StartsWith(":") && column.EndsWith(":")) {
                    return "center";
                }

                if (column.StartsWith(":")) {
                    return "left";
                }

                if (column.EndsWith(":")) {
                    return "right";
                }

                return null;
            }).ToList();
        }

        Element parseRow(
            BlockParser parser, List<string> alignments, string cellType) {
            var line = parser.current
                .replaceFirst(_openingPipe, "")
                .replaceFirst(_closingPipe, "");
            var cells = line.split(_pipePattern);
            parser.advance();
            var row = new List<Node>();
            string preCell = string.Empty;

            for (var index = 0; index < cells.Length; index++) {
                var cell = cells[index];
                if (preCell != null) {
                    cell = preCell + cell;
                    preCell = null;
                }

                if (cell.EndsWith("\\")) {
                    preCell = cell.Substring(0, cell.Length - 1) + '|';
                    continue;
                }

                var contents = new UnparsedContent(cell);
                row.Add(new Element(cellType, new List<Node>() {contents}));
            }

            for (var i = 0; i < row.Count && i < alignments.Count; i++) {
                if (alignments[i] == null) {
                    continue;
                }

                ((Element) row[i]).attributes["style"] = "text-align: " + alignments[i] + ';';
            }

            return new Element("tr", row);
        }
    }


    /// Parses paragraphs of regular text.
    class ParagraphSyntax : BlockSyntax {
        static Regex _reflinkDefinitionStart = new Regex(@"[ ]{0,3}\[");
        static Regex _whitespacePattern = new Regex(@"^\s*$");

        public override bool canEndBlock {
            get { return false; }
        }

        public override bool canParse(BlockParser parser) {
            return true;
        }

        public override Node parse(BlockParser parser) {
            var childLines = new List<string>();

            // Eat until we hit something that ends a paragraph.
            while (!isAtBlockEnd(parser)) {
                childLines.Add(parser.current);
                parser.advance();
            }

            var paragraphLines = this._extractReflinkDefinitions(parser, childLines);
            if (paragraphLines == null) {
                // Paragraph consisted solely of reference link definitions.
                return new Text("");
            }
            else {
                var contents = new UnparsedContent(paragraphLines.join('\n'));
                return new Element("p", new List<Node>() {contents});
            }
        }

        bool lineStartsReflinkDefinition(List<string> lines, int i) {
            if (i < lines.Count && i >= 0) {
                return lines[i].startsWith(_reflinkDefinitionStart);
            }
            else {
                return false;
            }
        }

        /// Extract reference link definitions from the front of the paragraph, and
        /// return the remaining paragraph lines.
        List<string> _extractReflinkDefinitions(
            BlockParser parser, List<string> lines) {
            var i = 0;
            loopOverDefinitions:
            while (true) {
                // Check for reflink definitions.
                if (!this.lineStartsReflinkDefinition(lines, i)) {
                    // It's paragraph content from here on out.
                    break;
                }

                var contents = lines[i];
                var j = i + 1;
                while (j < lines.Count) {
                    // Check to see if the _next_ line might start a new reflink definition.
                    // Even if it turns out not to be, but it started with a '[', then it
                    // is not a part of _this_ possible reflink definition.
                    if (this.lineStartsReflinkDefinition(lines, j)) {
                        // Try to parse [contents] as a reflink definition.
                        if (this._parseReflinkDefinition(parser, contents)) {
                            // Loop again, starting at the next possible reflink definition.
                            i = j;
                            goto loopOverDefinitions;
                        }
                        else {
                            // Could not parse [contents] as a reflink definition.
                            break;
                        }
                    }
                    else {
                        contents = contents + '\n' + lines[j];
                        j++;
                    }
                }

                // End of the block.
                if (this._parseReflinkDefinition(parser, contents)) {
                    i = j;
                    break;
                }

                // It may be that there is a reflink definition starting at [i], but it
                // does not extend all the way to [j], such as:
                //
                //     [link]: url     // line i
                //     "title"
                //     garbage
                //     [link2]: url   // line j
                //
                // In this case, [i, i+1] is a reflink definition, and the rest is
                // paragraph content.
                while (j >= i) {
                    // This isn't the most efficient loop, what with this big ole'
                    // Iterable allocation (`getRange`) followed by a big 'ole String
                    // allocation, but we
                    // must walk backwards, checking each range.
                    contents = lines.getRange(i, j).join('\n');
                    if (this._parseReflinkDefinition(parser, contents)) {
                        // That is the last reflink definition. The rest is paragraph
                        // content.
                        i = j;
                        break;
                    }

                    j--;
                }
                // The ending was not a reflink definition at all. Just paragraph
                // content.

                break;
            }

            if (i == lines.Count) {
                // No paragraph content.
                return null;
            }
            else {
                // Ends with paragraph content.
                return lines.sublist(i);
            }
        }

        // Parse [contents] as a reference link definition.
        //
        // Also adds the reference link definition to the document.
        //
        // Returns whether [contents] could be parsed as a reference link definition.
        bool _parseReflinkDefinition(BlockParser parser, string contents) {
            var pattern =
                new Regex(
                    @"^[ ]{0,3}\[((?:\\\]|[^\]])+)\]:\s*(?:<(\S+)>|(\S+))\s*(""[^""]+""|'[^']+'|\([^)]+\)|)\s*$",
                    RegexOptions.Multiline);

            var match = pattern.Match(contents);
            if (!match.Success) {
                // Not a reference link definition.
                return false;
            }

            if (match.Groups[0].Length < contents.Length) {
                // Trailing text. No good.
                return false;
            }

            var label = match.Groups[1].Value;
            var destination = string.IsNullOrEmpty(match.Groups[2].Value)
                ? match.Groups[3].Value
                : match.Groups[2].Value;
            var title = match.Groups[4].Value;

            // The label must contain at least one non-whitespace character.
            if (_whitespacePattern.hasMatch(label)) {
                return false;
            }

            if (string.IsNullOrEmpty(title)) {
                // No title.
                title = null;
            }
            else {
                // Remove "", '', or ().
                title = title.substring(1, title.Length - 1);
            }

            // References are case-insensitive, and internal whitespace is compressed.
            label =
                label.ToLower().Trim().replaceAll(BlockParserUtil._oneOrMoreWhitespacePattern, ' ');

            parser.document.linkReferences
                .putIfAbsent(label, () => new LinkReference(label, destination, title));
            return true;
        }
    }
}