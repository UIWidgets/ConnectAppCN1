using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Unity.UIWidgets.foundation;

namespace markdown {
    public class InlineParser {
        static List<InlineSyntax> _defaultSyntaxes = new List<InlineSyntax>() {
            new EmailAutolinkSyntax(),
            new AutolinkSyntax(),
            new LineBreakSyntax(),
            new LinkSyntax(),
            new ImageSyntax(),
            // Allow any punctuation to be escaped.
            new EscapeSyntax(),
            // "*" surrounded by spaces is left alone.
            new TextSyntax(@" \* "),
            // "_" surrounded by spaces is left alone.
            new TextSyntax(@" _ "),
            // Parse "**strong**" and "*emphasis*" tags.
            new TagSyntax(@"\*+", requiresDelimiterRun: true),
            // Parse "__strong__" and "_emphasis_" tags.
            new TagSyntax(@"_+", requiresDelimiterRun: true),
            new CodeSyntax(),
            // We will add the LinkSyntax once we know about the specific link resolver.
        };

        static List<InlineSyntax> _htmlSyntaxes =
            new List<InlineSyntax>() {
                // Leave already-encoded HTML entities alone. Ensures we don't turn
                // "&amp;" into "&amp;amp;"
                new TextSyntax(@"&[#a-zA-Z0-9]*;"),

                // Encode "&".
                new TextSyntax(@"&", "&amp;"),

                // Encode "<". 
                new TextSyntax(@"<", "&lt;"),

                // Encode ">".
                new TextSyntax(@">", "&gt:"),
                // We will add the LinkSyntax once we know about the specific link resolver.
            };


        /// The string of Markdown being parsed.
        internal string source;

        /// The Markdown document this parser is parsing.
        internal Document document;

        List<InlineSyntax> syntaxes = new List<InlineSyntax>();

        /// The current read position.
        internal int pos = 0;

        /// Starting position of the last unconsumed text.
        internal int start = 0;

        internal List<TagState> _stack;


        public InlineParser(string source, Document document) {
            this.source = source;
            this.document = document;

            this._stack = new List<TagState>();
            // User specified syntaxes are the first syntaxes to be evaluated.
            this.syntaxes.AddRange(document.inlineSyntaxes);

            var documentHasCustomInlineSyntaxes = document.inlineSyntaxes
                .Any((s) => !document.extensionSet.inlineSyntaxes.Contains(s));

            // This first RegExp matches plain text to accelerate parsing. It's written
            // so that it does not match any prefix of any following syntaxes. Most
            // Markdown is plain text, so it's faster to match one RegExp per 'word'
            // rather than fail to match all the following RegExps at each non-syntax
            // character position.
            if (documentHasCustomInlineSyntaxes) {
                // We should be less aggressive in blowing past "words".
                this.syntaxes.Add(new TextSyntax(@"[A-Za-z0-9]+(?=\s)"));
            }
            else {
                this.syntaxes.Add(new TextSyntax(@"[ \tA-Za-z0-9]*[A-Za-z0-9](?=\s)"));
            }

            this.syntaxes.AddRange(_defaultSyntaxes);

            if (this.document.encodeHtml) {
                this.syntaxes.AddRange(_htmlSyntaxes);
            }

            // Custom link resolvers go after the generic text syntax.
            this.syntaxes.InsertRange(1, new List<InlineSyntax>() {
                new LinkSyntax(document.linkResolver),
                new ImageSyntax(linkResolver: document.imageLinkResolver)
            });
        }

        internal List<Node> parse() {
            // Make a fake top tag to hold the results.
            this._stack.Add(new TagState(0, 0, null, null));

            while (!this.isDone) {
                // See if any of the current tags on the stack match.  This takes
                // priority over other possible matches.
                if (this._stack.reversed().Any((state) => state.syntax != null && state.tryMatch(this))) {
                    continue;
                }

                // See if the current text matches any defined markdown syntax.
                if (this.syntaxes.Any((syntax) => syntax.tryMatch(this))) {
                    continue;
                }

                // If we got here, it's just text.
                this.advanceBy(1);
            }

            // Unwind any unmatched tags and get the results.
            return this._stack[0].close(this, null);
        }

        internal int charAt(int index) {
            return this.source.codeUnitAt(index);
        }


        internal void writeText() {
            this.writeTextRange(this.start, this.pos);
            this.start = this.pos;
        }

        internal void writeTextRange(int start, int end) {
            if (end <= start) {
                return;
            }

            var text = this.source.substring(start, end);
            var nodes = this._stack.last().children;

            // If the previous node is text too, just append.
            if (nodes.isNotEmpty() && nodes.last() is Text) {
                var textNode = nodes.last() as Text;
                nodes[nodes.Count - 1] = new Text(textNode.text + text);
            }
            else {
                nodes.Add(new Text(text));
            }
        }

        /// Add [node] to the last [TagState] on the stack.
        internal void addNode(Node node) {
            this._stack.last().children.Add(node);
        }

        /// Push [state] onto the stack of [TagState]s.
        internal void openTag(TagState state) {
            this._stack.Add(state);
        }

        internal bool isDone {
            get { return this.pos == this.source.Length; }
        }

        internal void advanceBy(int length) {
            this.pos += length;
        }

        internal void consume(int length) {
            this.pos += length;
            this.start = this.pos;
        }
    }

    public abstract class InlineSyntax {
        internal Regex pattern;

        int _startCharacter;

        protected InlineSyntax(string pattern, int startCharacter = -1) {
            this.pattern = new Regex(pattern, RegexOptions.Multiline);
            this._startCharacter = startCharacter;
        }

        /// Tries to match at the parser's current position.
        ///
        /// The parser's position can be overriden with [startMatchPos].
        /// Returns whether or not the pattern successfully matched.
        internal virtual bool tryMatch(InlineParser parser, int startMatchPos = -1) {
            if (startMatchPos == -1) {
                startMatchPos = parser.pos;
            }

            if (this._startCharacter != -1 && parser.source.codeUnitAt(startMatchPos) != this._startCharacter) {
                return false;
            }

            var startMatch = this.pattern.matchAsPrefix(parser.source, startMatchPos);
            if (!startMatch.Success) {
                return false;
            }

            // Write any existing plain text up to this point.
            parser.writeText();

            if (this.onMatch(parser, startMatch)) {
                parser.consume(startMatch.Groups[0].Length);
            }

            return true;
        }

        /// Processes [match], adding nodes to [parser] and possibly advancing
        /// [parser].
        ///
        /// Returns whether the caller should advance [parser] by `match[0].length`.
        internal abstract bool onMatch(InlineParser parser, Match match);
    }

    /// Represents a hard line break.
    class LineBreakSyntax : InlineSyntax {
        public LineBreakSyntax() : base(@"(?:\\|  +)\n") { }


        /// Create a void <br> element.
        internal override bool onMatch(InlineParser parser, Match match) {
            parser.addNode(Element.empty("br"));
            return true;
        }
    }

    /// Matches stuff that should just be passed through as straight text.
    class TextSyntax : InlineSyntax {
        string substitute;

        internal TextSyntax(string pattern, string sub = "") : base(pattern) {
            this.substitute = sub;
        }


        internal override bool onMatch(InlineParser parser, Match match) {
            if (string.IsNullOrEmpty(this.substitute)) {
                // Just use the original matched text.
                parser.advanceBy(match.Groups[0].Length);
                return false;
            }

            // Insert the substitution.
            parser.addNode(new Text(this.substitute));
            return true;
        }
    }

    /// Escape punctuation preceded by a backslash.
    class EscapeSyntax : InlineSyntax {
        public EscapeSyntax() : base(@"\\[!""#$%&'()*+,\-./:;<=>?@\[\\\]^_`{|}~]''") { }

        internal override bool onMatch(InlineParser parser, Match match) {
// Insert the substitution.
            parser.addNode(new Text(match.Groups[0].Value[1].ToString()));
            return true;
        }
    }

    /// Leave inline HTML tags alone, from
    /// [CommonMark 0.28](http://spec.commonmark.org/0.28/#raw-html).
    ///
    /// This is not actually a good definition (nor CommonMark's) of an HTML tag,
    /// but it is fast. It will leave text like `<a href='hi">` alone, which is
    /// incorrect.
    ///
    /// TODO(srawlins): improve accuracy while ensuring performance, once
    /// Markdown benchmarking is more mature.
    class InlineHtmlSyntax : TextSyntax {
        public InlineHtmlSyntax() : base(@"<[/!?]?[A-Za-z][A-Za-z0-9-]*(?:\s[^>]*)?>") { }
    }

    /// Matches autolinks like `<foo@bar.example.com>`.
    ///
    /// See <http://spec.commonmark.org/0.28/#email-address>.
    class EmailAutolinkSyntax : InlineSyntax {
        static string _email =
            @"[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*";

        public EmailAutolinkSyntax() : base($"<({_email})>") { }

        internal override bool onMatch(InlineParser parser, Match match) {
            var url = match.Groups[1].Value;
            var anchor = Element.text("a", Utils.escapeHtml(url));
            anchor.attributes["href"] = ("mailto:" + url); // TODO: Uri.encodeFull
            parser.addNode(anchor);
            return true;
        }
    }

    /// Matches autolinks like `<http://foo.com>`.
    class AutolinkSyntax : InlineSyntax {
        public AutolinkSyntax() : base(@"<(([a-zA-Z][a-zA-Z\-\+\.]+):(?://)?[^\s>]*)>") { }

        internal override bool onMatch(InlineParser parser, Match match) {
            var url = match.Groups[1].Value;
            var anchor = Element.text("a", Utils.escapeHtml(url));
            anchor.attributes["href"] = (url); // TODO: Uri.encodeFull
            parser.addNode(anchor);
            return true;
        }
    }

    /// Matches autolinks like `http://foo.com`.
    class AutolinkExtensionSyntax : InlineSyntax {
        /// Broken up parts of the autolink regex for reusability and readability

// Autolinks can only come at the beginning of a line, after whitespace, or
// any of the delimiting characters *, _, ~, and (.
        const string start = @"(?:^|[\s*_~(>])";

// An extended url autolink will be recognized when one of the schemes
// http://, https://, or ftp://, followed by a valid domain
        const string scheme = @"(?:(?:https?|ftp):\/\/|www\.)";

// A valid domain consists of alphanumeric characters, underscores (_),
// hyphens (-) and periods (.). There must be at least one period, and no
// underscores may be present in the last two segments of the domain.
        const string domainPart = @"\w\-";
        static string domain = string.Format("[{0}][{0}.]+", domainPart);

// A valid domain consists of alphanumeric characters, underscores (_),
// hyphens (-) and periods (.).
        const string path = @"[^\s<]*";

// Trailing punctuation (specifically, ?, !, ., ,, :, *, _, and ~) will not
// be considered part of the autolink
        const string truncatingPunctuationPositive = @"[?!.,:*_~]";

        static Regex regExpTrailingPunc =
            new Regex(string.Format("{0}*{1}", truncatingPunctuationPositive, "$"));

        static Regex regExpEndsWithColon = new Regex(@"\&[a - zA - Z0 - 9] +;$");
        static Regex regExpWhiteSpace = new Regex(@"\s");

        public AutolinkExtensionSyntax() : base(string.Format("{0}(({1})({2})({3}))", start, scheme, domain, path)) { }

        internal override bool tryMatch(InlineParser parser, int startMatchPos = 0) {
            return base.tryMatch(parser, parser.pos > 0 ? parser.pos - 1 : 0);
        }

        internal override bool onMatch(InlineParser parser, Match match) {
            var url = match.Groups[1].Value;
            var href = url;
            var matchLength = url.Length;
            if (url[0] == '>' || url.startsWith(regExpWhiteSpace)) {
                url = url.substring(1, url.Length - 1);
                href = href.substring(1, href.Length - 1);
                parser.pos++;
                matchLength--;
            }

// Prevent accidental standard autolink matches
            if (url.endsWith('>') && parser.source[parser.pos - 1] == '<') {
                return false;
            }

// When an autolink ends in ), we scan the entire autolink for the total
// number of parentheses. If there is a greater number of closing
// parentheses than opening ones, we don’t consider the last character
// part of the autolink, in order to facilitate including an autolink
// inside a parenthesis:
// https://github.github.com/gfm/#example-600
            if (url.endsWith(')')) {
                int opening = this._countChars(url, '(');
                int closing = this._countChars(url, ')');
                if (closing > opening) {
                    url = url.substring(0, url.Length - 1);
                    href = href.substring(0, href.Length - 1);
                    matchLength--;
                }
            }

// Trailing punctuation (specifically, ?, !, ., ,, :, *, _, and ~) will
// not be considered part of the autolink, though they may be included
// in the interior of the link:
// https://github.github.com/gfm/#example-599
            var trailingPunc = regExpTrailingPunc.Match(url);
            if (trailingPunc.Success) {
                url = url.substring(0, url.Length - trailingPunc.Groups[0].Length);
                href = href.substring(0, href.Length - trailingPunc.Groups[0].Length);
                matchLength -= trailingPunc.Groups[0].Length;
            }

// If an autolink ends in a semicolon (;), we check to see if it appears
// to resemble an
// [entity reference](https://github.github.com/gfm/#entity-references);
// if the preceding text is & followed by one or more alphanumeric
// characters. If so, it is excluded from the autolink:
// https://github.github.com/gfm/#example-602
            if (url.endsWith(';')) {
                var entityRef = regExpEndsWithColon.Match(url);
                if (entityRef.Success) {
// Strip out HTML entity reference
                    url = url.substring(0, url.Length - entityRef.Groups[0].Length);
                    href = href.substring(0, href.Length - entityRef.Groups[0].Length);
                    matchLength -= entityRef.Groups[0].Length;
                }
            }

// The scheme http will be inserted automatically
            if (!href.startsWith("http://") &&
                !href.startsWith("https://") &&
                !href.startsWith("ftp://")) {
                href = "http://" + href;
            }

            var anchor = Element.text("a", Utils.escapeHtml(url));
            anchor.attributes["href"] = (href); //TODO: url encode!!!
            parser.addNode(anchor);
            parser.consume(matchLength);
            return false;
        }

        int _countChars(string input, char cha) {
            var count = 0;
            for (var i = 0; i < input.Length; i++) {
                if (input[i] == cha) {
                    count++;
                }
            }

            return count;
        }
    }


    class _DelimiterRun {
        static string punctuation = @"!""#$%&'()*+,-./:;<=>?@[\]^_`{|}~";

        // TODO(srawlins): Unicode whitespace
        static string whitespace = " \t\r\n";
        int cha;
        internal int length;
        bool isLeftFlanking;
        internal bool isRightFlanking;
        bool isPrecededByPunctuation;
        bool isFollowedByPunctuation;

        _DelimiterRun(int cha, int length, bool isLeftFlanking, bool isRightFlanking, bool isPrecededByPunctuation,
            bool isFollowedByPunctuation) {
            this.cha = cha;
            this.length = length;
            this.isLeftFlanking = isLeftFlanking;
            this.isRightFlanking = isRightFlanking;
            this.isPrecededByPunctuation = isPrecededByPunctuation;
            this.isFollowedByPunctuation = isFollowedByPunctuation;
        }


        internal static _DelimiterRun tryParse(InlineParser parser, int runStart, int runEnd) {
            bool leftFlanking,
                rightFlanking,
                precededByPunctuation,
                followedByPunctuation;

            string preceding, following;
            if (runStart == 0) {
                rightFlanking = false;
                preceding = "\n";
            }
            else {
                preceding = parser.source.substring(runStart - 1, runStart);
            }

            precededByPunctuation = punctuation.Contains(preceding);

            if (runEnd == parser.source.Length - 1) {
                leftFlanking = false;
                following = "\n";
            }
            else {
                following = parser.source.substring(runEnd + 1, runEnd + 2);
            }

            followedByPunctuation = punctuation.Contains(following);

// http://spec.commonmark.org/0.28/#left-flanking-delimiter-run
            if (whitespace.Contains(following)) {
                leftFlanking = false;
            }
            else {
                leftFlanking = !followedByPunctuation ||
                               whitespace.Contains(preceding) ||
                               precededByPunctuation;
            }

// http://spec.commonmark.org/0.28/#right-flanking-delimiter-run
            if (whitespace.Contains(preceding)) {
                rightFlanking = false;
            }
            else {
                rightFlanking = !precededByPunctuation ||
                                whitespace.Contains(following) ||
                                followedByPunctuation;
            }

            if (!leftFlanking && !rightFlanking) {
// Could not parse a delimiter run.
                return null;
            }

            return
                new _DelimiterRun(
                    parser.charAt(runStart),
                    runEnd - runStart + 1,
                    leftFlanking,
                    rightFlanking,
                    precededByPunctuation,
                    followedByPunctuation);
        }

        string toString() {
            return string.Format("<char: {0}, length: {1}, isLeftFlanking: {2}, isRightFlanking: {3}>", this.cha,
                this.length, this.isLeftFlanking, this.isRightFlanking);
        }

        // Whether a delimiter in this run can open emphasis or strong emphasis.
        internal bool canOpen {
            get {
                return this.isLeftFlanking &&
                       (this.cha == CharCode.asterisk || !this.isRightFlanking || this.isPrecededByPunctuation);
            }
        }

        // Whether a delimiter in this run can close emphasis or strong emphasis.
        internal bool canClose {
            get {
                return this.isRightFlanking &&
                       (this.cha == CharCode.asterisk || !this.isLeftFlanking || this.isFollowedByPunctuation);
            }
        }
    }

    /// Matches syntax that has a pair of tags and becomes an element, like `*` for
    /// `<em>`. Allows nested tags.
    class TagSyntax : InlineSyntax {
        internal Regex endPattern;

        /// Whether this is parsed according to the same nesting rules as [emphasis
        /// delimiters][].
        ///
        /// [emphasis delimiters]: http://spec.commonmark.org/0.28/#can-open-emphasis
        internal bool requiresDelimiterRun;

        internal TagSyntax(string pattern, bool requiresDelimiterRun = false, string end = null) : base(pattern) {
            this.endPattern = new Regex(end ?? pattern, RegexOptions.Multiline);
            this.requiresDelimiterRun = requiresDelimiterRun;
        }


        internal override bool onMatch(InlineParser parser, Match match) {
            var runLength = match.Groups[0].Length;
            var matchStart = parser.pos;
            var matchEnd = parser.pos + runLength - 1;
            if (!this.requiresDelimiterRun) {
                parser.openTag(new TagState(parser.pos, matchEnd + 1, this, null));
                return true;
            }

            var delimiterRun = _DelimiterRun.tryParse(parser, matchStart, matchEnd);
            if (delimiterRun != null && delimiterRun.canOpen) {
                parser
                    .openTag(new TagState(parser.pos, matchEnd + 1, this, delimiterRun));
                return true;
            }
            else {
                parser.advanceBy(runLength);
                return false;
            }
        }

        internal virtual bool onMatchEnd(InlineParser parser, Match match, TagState state) {
            var runLength = match.Groups[0].Length;
            var matchStart = parser.pos;
            var matchEnd = parser.pos + runLength - 1;
            var openingRunLength = state.endPos - state.startPos;

            var delimiterRun = _DelimiterRun.tryParse(parser, matchStart, matchEnd);
            if (openingRunLength == 1 && runLength == 1) {
                parser.addNode(new Element("em", state.children));
            }
            else if (openingRunLength == 1 && runLength > 1) {
                parser.addNode(new Element("em", state.children));
                parser.pos = parser.pos - (runLength - 1);
                parser.start = parser.pos;
            }
            else if (openingRunLength > 1 && runLength == 1) {
                parser.openTag(
                    new TagState(state.startPos, state.endPos - 1, this, delimiterRun));
                parser.addNode(new Element("em", state.children));
            }
            else if (openingRunLength == 2 && runLength == 2) {
                parser.addNode(new Element("strong", state.children));
            }
            else if (openingRunLength == 2 && runLength > 2) {
                parser.addNode(new Element("strong", state.children));
                parser.pos = parser.pos - (runLength - 2);
                parser.start = parser.pos;
            }
            else if (openingRunLength > 2 && runLength == 2) {
                parser.openTag(
                    new TagState(state.startPos, state.endPos - 2, this, delimiterRun));
                parser.addNode(new Element("strong", state.children));
            }
            else if (openingRunLength > 2 && runLength > 2) {
                parser.openTag(
                    new TagState(state.startPos, state.endPos - 2, this, delimiterRun));
                parser.addNode(new Element("strong", state.children));
                parser.pos = parser.pos - (runLength - 2);
                parser.start = parser.pos;
            }

            return true;
        }
    }

    /// Matches strikethrough syntax according to the GFM spec.
    class StrikethroughSyntax : TagSyntax {
        public StrikethroughSyntax() : base("~+", true) { }


        internal override bool onMatchEnd(InlineParser parser, Match match, TagState state) {
            var runLength = match.Groups[0].Length;
            var matchStart = parser.pos;
            var matchEnd = parser.pos + runLength - 1;
            var delimiterRun = _DelimiterRun.tryParse(parser, matchStart, matchEnd);
            if (!delimiterRun.isRightFlanking) {
                return false;
            }

            parser.addNode(new Element("del", state.children));
            return true;
        }
    }


    /// Matches links like `[blah][label]` and `[blah](url)`.
    class LinkSyntax : TagSyntax {
        static Regex _entirelyWhitespacePattern = new Regex(@"^\s*$");

        Resolver linkResolver;

        internal LinkSyntax(Resolver linkResolver = null, string pattern = @"\[") : base(pattern, false, @"\]") {
            this.linkResolver = linkResolver ?? ((string str, List<string> strs) => null);
        }


        // The pending [TagState]s, all together, are "active" or "inactive" based on
        // whether a link element has just been parsed.
        //
        // Links cannot be nested, so we must "deactivate" any pending ones. For
        // example, take the following text:
        //
        //     Text [link and [more](links)](links).
        //
        // Once we have parsed `Text [`, there is one (pending) link in the state
        // stack.  It is, by default, active. Once we parse the next possible link,
        // `[more](links)`, as a real link, we must deactive the pending links (just
        // the one, in this case).
        bool _pendingStatesAreActive = true;

        internal override bool onMatch(InlineParser parser, Match match) {
            var matched = base.onMatch(parser, match);
            if (!matched) {
                return false;
            }

            this._pendingStatesAreActive = true;
            return true;
        }

        internal override bool onMatchEnd(InlineParser parser, Match match, TagState state) {
            if (!this._pendingStatesAreActive) {
                return false;
            }

            var text = parser.source.substring(state.endPos, parser.pos);
            // The current character is the `]` that closed the link text. Examine the
            // next character, to determine what type of link we might have (a '('
            // means a possible inline link; otherwise a possible reference link).
            if (parser.pos + 1 >= parser.source.Length) {
                // In this case, the Markdown document may have ended with a shortcut
                // reference link.
                return this._tryAddReferenceLink(parser, state, text);
            }

// Peek at the next character; don't advance, so as to avoid later stepping
// backward.
            var cha = parser.charAt(parser.pos + 1);

            if (cha == CharCode.lparen) {
// Maybe an inline link, like `[text](destination)`.
                parser.advanceBy(1);
                var leftParenIndex = parser.pos;

                var inlineLink = this._parseInlineLink(parser);
                if (inlineLink != null) {
                    return this._tryAddInlineLink(parser, state, inlineLink);
                }

                // Reset the parser position.
                parser.pos = leftParenIndex;

// At this point, we've matched `[...](`, but that `(` did not pan out to
// be an inline link. We must now check if `[...]` is simply a shortcut
// reference link.
                parser.advanceBy(-1);
                return this._tryAddReferenceLink(parser, state, text);
            }

            if (cha == CharCode.lbracket) {
                parser.advanceBy(1);
// At this point, we've matched `[...][`. Maybe a *full* reference link,
// like `[foo][bar]` or a *collapsed* reference link, like `[foo][]`.
                if (parser.pos + 1 < parser.source.Length &&
                    parser.charAt(parser.pos + 1) == '[') {
                    // That opening `[` is not actually part of the link. Maybe a
                    // *shortcut* reference link (followed by a `[`).
                    parser.advanceBy(1);
                    return this._tryAddReferenceLink(parser, state, text);
                }

                var label = this._parseReferenceLinkLabel(parser);
                if (label != null) {
                    return this._tryAddReferenceLink(parser, state, label);
                }

                return false;
            }

// The link text (inside `[...]`) was not followed with a opening `(` nor
// an opening `[`. Perhaps just a simple shortcut reference link (`[...]`).
            return this._tryAddReferenceLink(parser, state, text);
        }

        /// Resolve a possible reference link.
        ///
        /// Uses [linkReferences], [linkResolver], and [_createNode] to try to
        /// resolve [label] and [state] into a [Node]. If [label] is defined in
        /// [linkReferences] or can be resolved by [linkResolver], returns a [Node]
        /// that links to the resolved URL.
        ///
        /// Otherwise, returns `null`.
        ///
        /// [label] does not need to be normalized.
        internal Node _resolveReferenceLink(
            string label, TagState state, Dictionary<string, LinkReference> linkReferences) {
            var normalizedLabel = label.ToLower();
            LinkReference linkReference = null;
            if (linkReferences.ContainsKey(normalizedLabel)) {
                linkReference = linkReferences[normalizedLabel];
            }

            if (linkReference != null) {
                return this._createNode(state, linkReference.destination, linkReference.title);
            }
            else {
// This link has no reference definition. But we allow users of the
// library to specify a custom resolver function ([linkResolver]) that
// may choose to handle this. Otherwise, it's just treated as plain
// text.

// Normally, label text does not get parsed as inline Markdown. However,
// for the benefit of the link resolver, we need to at least escape
// brackets, so that, e.g. a link resolver can receive `[\[\]]` as `[]`.
                return this.linkResolver(label.replaceAll(@"\\", @"\").replaceAll(@"\[", '[').replaceAll(@"\]", ']'));
            }
        }

        /// Create the node represented by a Markdown link.
        protected virtual Node _createNode(TagState state, string destination, string title) {
            var element = new Element("a", state.children);
            element.attributes["href"] = Utils.escapeAttribute(destination);
            if (title != null && title.isNotEmpty()) {
                element.attributes["title"] = Utils.escapeAttribute(title);
            }

            return element;
        }

// Add a reference link node to [parser]'s AST.
//
// Returns whether the link was added successfully.
        protected virtual bool _tryAddReferenceLink(InlineParser parser, TagState state, string label) {
            var element = this._resolveReferenceLink(label, state, parser.document.linkReferences);
            if (element == null) {
                return false;
            }

            parser.addNode(element);
            parser.start = parser.pos;
            this._pendingStatesAreActive = false;
            return true;
        }

// Add an inline link node to [parser]'s AST.
//
// Returns whether the link was added successfully.
        bool _tryAddInlineLink(InlineParser parser, TagState state, InlineLink link) {
            var element = this._createNode(state, link.destination, link.title);
            if (element == null) {
                return false;
            }

            parser.addNode(element);
            parser.start = parser.pos;
            this._pendingStatesAreActive = false;
            return true;
        }

        /// Parse a reference link label at the current position.
        ///
        /// Specifically, [parser.pos] is expected to be pointing at the `[` which
        /// opens the link label.
        ///
        /// Returns the label if it could be parsed, or `null` if not.
        string _parseReferenceLinkLabel(InlineParser parser) {
// Walk past the opening `[`.
            parser.advanceBy(1);
            if (parser.isDone) {
                return null;
            }

            var buffer = new StringBuilder();
            while (true) {
                var cha = parser.charAt(parser.pos);
                if (cha == CharCode.backslash) {
                    parser.advanceBy(1);
                    var next = parser.charAt(parser.pos);
                    if (next != CharCode.backslash && next != CharCode.rbracket) {
                        buffer.writeCharCode(cha);
                    }

                    buffer.writeCharCode(next);
                }
                else if (cha == CharCode.rbracket) {
                    break;
                }
                else {
                    buffer.writeCharCode(cha);
                }

                parser.advanceBy(1);
                if (parser.isDone) {
                    return null;
                }

                // TODO(srawlins): only check 999 characters, for performance reasons?
            }

            var label = buffer.ToString();

// A link label must contain at least one non-whitespace character.
            if (_entirelyWhitespacePattern.hasMatch(label)) {
                return null;
            }

            return label;
        }

        /// Parse an inline [InlineLink] at the current position.
        ///
        /// At this point, we have parsed a link's (or image's) opening `[`, and then
        /// a matching closing `]`, and [parser.pos] is pointing at an opening `(`.
        /// This method will then attempt to parse a link destination wrapped in `<>`,
        /// such as `(<http://url>)`, or a bare link destination, such as
        /// `(http://url)`, or a link destination with a title, such as
        /// `(http://url "title")`.
        ///
        /// Returns the [InlineLink] if one was parsed, or `null` if not.
        InlineLink _parseInlineLink(InlineParser parser) {
// Start walking to the character just after the opening `(`.
            parser.advanceBy(1);
            this._moveThroughWhitespace(parser);
            if (parser.isDone) {
                return null; // EOF. Not a link.
            }

            if (parser.charAt(parser.pos) == CharCode.lt) {
// Maybe a `<...>`-enclosed link destination.
                return this._parseInlineBracketedLink(parser);
            }
            else {
                return this._parseInlineBareDestinationLink(parser);
            }
        }

        /// Parse an inline link with a bracketed destination (a destination wrapped
        /// in `<...>`). The current position of the parser must be the first
        /// character of the destination.
        InlineLink _parseInlineBracketedLink(InlineParser parser) {
            int cha;
            parser.advanceBy(1);
            var buffer = new StringBuilder();
            while (true) {
                cha = parser.charAt(parser.pos);
                if (cha == CharCode.backslash) {
                    parser.advanceBy(1);
                    var next = parser.charAt(parser.pos);
                    if (cha == CharCode.space || cha == CharCode.lf || cha == CharCode.cr || cha == CharCode.ff) {
// Not a link (no whitespace allowed within `<...>`).
                        return null;
                    }

// TODO: Follow the backslash spec better here.
// http://spec.commonmark.org/0.28/#backslash-escapes
                    if (next != CharCode.backslash && next != CharCode.gt) {
                        buffer.writeCharCode(cha);
                    }

                    buffer.writeCharCode(next);
                }
                else if (cha == CharCode.space || cha == CharCode.lf || cha == CharCode.cr || cha == CharCode.ff) {
// Not a link (no whitespace allowed within `<...>`).
                    return null;
                }
                else if (cha == CharCode.gt) {
                    break;
                }
                else {
                    buffer.writeCharCode(cha);
                }

                parser.advanceBy(1);
                if (parser.isDone) {
                    return null;
                }
            }

            var destination = buffer.ToString();

            parser.advanceBy(1);

            cha = parser.charAt(parser.pos);

            if (cha == CharCode.space || cha == CharCode.lf || cha == CharCode.cr || cha == CharCode.ff) {
                var title = this._parseTitle(parser);
                if (title == null && parser.charAt(parser.pos) != CharCode.rparen) {
// This looked like an inline link, until we found this $space
// followed by mystery characters; no longer a link.
                    return null;
                }

                return new InlineLink(destination, title: title);
            }
            else if (cha == CharCode.rparen) {
                return new InlineLink(destination);
            }
            else {
// We parsed something like `[foo](<url>X`. Not a link.
                return null;
            }
        }

        /// Parse an inline link with a "bare" destination (a destination _not_
        /// wrapped in `<...>`). The current position of the parser must be the first
        /// character of the destination.
        InlineLink _parseInlineBareDestinationLink(InlineParser parser) {
// According to
// [CommonMark](http://spec.commonmark.org/0.28/#link-destination):
//
// > A link destination consists of [...] a nonempty sequence of
// > characters [...], and includes parentheses only if (a) they are
// > backslash-escaped or (b) they are part of a balanced pair of
// > unescaped parentheses.
//
// We need to count the open parens. We start with 1 for the paren that
// opened the destination.
            var parenCount = 1;
            var buffer = new StringBuilder();

            while (true) {
                var cha = parser.charAt(parser.pos);
                switch (cha) {
                    case CharCode.backslash:
                        parser.advanceBy(1);
                        if (parser.isDone) {
                            return null; // EOF. Not a link.
                        }

                        var next = parser.charAt(parser.pos);
// Parentheses may be escaped.
//
// http://spec.commonmark.org/0.28/#example-467
                        if (next != CharCode.backslash && next != CharCode.lparen && next != CharCode.rparen) {
                            buffer.writeCharCode(cha);
                        }

                        buffer.writeCharCode(next);
                        break;
                    case CharCode.space:
                    case CharCode.lf:
                    case CharCode.cr:
                    case CharCode.ff:
                        var destination = buffer.ToString();
                        var title = this._parseTitle(parser);

                        if (title == null && (parser.isDone || parser.charAt(parser.pos) != CharCode.rparen)) {
// This looked like an inline link, until we found this $space
// followed by mystery characters; no longer a link.
                            return null;
                        }

// [_parseTitle] made sure the title was follwed by a closing `)`
// (but it's up to the code here to examine the balance of
// parentheses).
                        parenCount--;
                        if (parenCount == 0) {
                            return new InlineLink(destination, title: title);
                        }

                        break;
                    case CharCode.lparen:
                        parenCount++;
                        buffer.writeCharCode(cha);
                        break;
                    case CharCode.rparen:
                        parenCount--;
                        if (parenCount == 0) {
                            destination = buffer.ToString();
                            return new InlineLink(destination);
                        }

                        buffer.writeCharCode(cha);
                        break;
                    default:
                        buffer.writeCharCode(cha);
                        break;
                }

                parser.advanceBy(1);
                if (parser.isDone) {
                    return null; // EOF. Not a link.
                }
            }
        }

// Walk the parser forward through any whitespace.
        void _moveThroughWhitespace(InlineParser parser) {
            while (!parser.isDone) {
                var cha = parser.charAt(parser.pos);
                if (cha != CharCode.space &&
                    cha != CharCode.tab &&
                    cha != CharCode.lf &&
                    cha != CharCode.vt &&
                    cha != CharCode.cr &&
                    cha != CharCode.ff) {
                    return;
                }

                parser.advanceBy(1);
                if (parser.isDone) {
                    return;
                }
            }
        }

// Parse a link title in [parser] at it's current position. The parser's
// current position should be a whitespace character that followed a link
// destination.
        string _parseTitle(InlineParser parser) {
            this._moveThroughWhitespace(parser);
            if (parser.isDone) {
                return null;
            }

            // The whitespace should be followed by a title delimiter.
            var delimiter = parser.charAt(parser.pos);
            if (delimiter != CharCode.apostrophe &&
                delimiter != CharCode.quote &&
                delimiter != CharCode.lparen) {
                return null;
            }

            var closeDelimiter = delimiter == CharCode.lparen ? CharCode.rparen : delimiter;
            parser.advanceBy(1);

// Now we look for an un-escaped closing delimiter.
            var buffer = new StringBuilder();
            while (true) {
                var cha = parser.charAt(parser.pos);
                if (cha == CharCode.backslash) {
                    parser.advanceBy(1);
                    var next = parser.charAt(parser.pos);
                    if (next != CharCode.backslash && next != closeDelimiter) {
                        buffer.writeCharCode(cha);
                    }

                    buffer.writeCharCode(next);
                }
                else if (cha == closeDelimiter) {
                    break;
                }
                else {
                    buffer.writeCharCode(cha);
                }

                parser.advanceBy(1);
                if (parser.isDone) {
                    return null;
                }
            }

            var title = buffer.ToString();

// Advance past the closing delimiter.
            parser.advanceBy(1);
            if (parser.isDone) {
                return null;
            }

            this._moveThroughWhitespace(parser);
            if (parser.isDone) {
                return null;
            }

            if (parser.charAt(parser.pos) != CharCode.rparen) {
                return null;
            }

            return title;
        }
    }


    /// Matches images like `![alternate text](url "optional title")` and
    /// `![alternate text][label]`.
    class ImageSyntax : LinkSyntax {
        public ImageSyntax(Resolver linkResolver = null)
            : base(linkResolver, @"!\[") { }

        protected override Node _createNode(TagState state, string destination, string title) {
            var element = Element.empty("img");
            element.attributes["src"] = Utils.escapeHtml(destination);
            element.attributes["alt"] = state?.textContent ?? "";
            if (title != null && title.isNotEmpty()) {
                element.attributes["title"] = Utils.escapeAttribute(title.replaceAll("&", "&amp;"));
            }

            return element;
        }

        // Add an image node to [parser]'s AST.
        //
        // If [label] is present, the potential image is treated as a reference image.
        // Otherwise, it is treated as an inline image.
        //
        // Returns whether the image was added successfully.
        protected override bool _tryAddReferenceLink(InlineParser parser, TagState state, string label) {
            var element = this._resolveReferenceLink(label, state, parser.document.linkReferences);
            if (element == null) {
                return false;
            }

            parser.addNode(element);
            parser.start = parser.pos;
            return true;
        }
    }

    /// Matches backtick-enclosed inline code blocks.
    class CodeSyntax : InlineSyntax {
        // This pattern matches:
        //
        // * a string of backticks (not followed by any more), followed by
        // * a non-greedy string of anything, including newlines, ending with anything
        //   except a backtick, followed by
        // * a string of backticks the same length as the first, not followed by any
        //   more.
        //
        // This conforms to the delimiters of inline code, both in Markdown.pl, and
        // CommonMark.
        static string _pattern = @"(`+(?!`))((?:.|\n)*?[^`])\1(?!`)";

        public CodeSyntax() : base(_pattern) { }

        internal override bool tryMatch(InlineParser parser, int startMatchPos) {
            if (parser.pos > 0 && parser.charAt(parser.pos - 1) == CharCode.backquote) {
                // Not really a match! We can't just sneak past one backtick to try the
                // next character. An example of this situation would be:
                //
                //     before ``` and `` after.
                //             ^--parser.pos
                return false;
            }

            var match = this.pattern.matchAsPrefix(parser.source, parser.pos);
            if (!match.Success) {
                return false;
            }

            parser.writeText();
            if (this.onMatch(parser, match)) {
                parser.consume(match.Groups[0].Length);
            }

            return true;
        }

        internal override bool onMatch(InlineParser parser, Match match) {
            parser.addNode(Element.text("code", Utils.escapeHtml(match.Groups[2].Value.Trim())));
            return true;
        }
    }

    /// Matches GitHub Markdown emoji syntax like `:smile:`.
    ///
    /// There is no formal specification of GitHub's support for this colon-based
    /// emoji support, so this syntax is based on the results of Markdown-enabled
    /// text fields at github.com.
    class EmojiSyntax : InlineSyntax {
        // Emoji "aliases" are mostly limited to lower-case letters, numbers, and
        // underscores, but GitHub also supports `:+1:` and `:-1:`.
        internal EmojiSyntax() : base(@":([a-z0-9_+-]+):") { }

        internal override bool onMatch(InlineParser parser, Match match) {
            var alias = match.Groups[1].Value;
            var emoji = Emojis.emojis[alias];
            if (emoji == null) {
                parser.advanceBy(1);
                return false;
            }

            parser.addNode(new Text(emoji));

            return true;
        }
    }

    /// Keeps track of a currently open tag while it is being parsed.
    ///
    /// The parser maintains a stack of these so it can handle nested tags.
    class TagState {
        /// The point in the original source where this tag started.
        internal int startPos;

        /// The point in the original source where open tag ended.
        internal int endPos;

        /// The syntax that created this node.
        internal TagSyntax syntax;

        /// The children of this node. Will be `null` for text nodes.
        internal List<Node> children;

        _DelimiterRun openingDelimiterRun;

        public TagState(int startPos, int endPos, TagSyntax syntax, _DelimiterRun openingDelimiterRun) {
            this.startPos = startPos;
            this.children = new List<Node>();
            this.endPos = endPos;
            this.syntax = syntax;
            this.openingDelimiterRun = openingDelimiterRun;
        }


        /// Attempts to close this tag by matching the current text against its end
        /// pattern.
        internal bool tryMatch(InlineParser parser) {
            var endMatch = this.syntax.endPattern.matchAsPrefix(parser.source, parser.pos);
            if (!endMatch.Success) {
                return false;
            }

            if (!this.syntax.requiresDelimiterRun) {
                // Close the tag.
                this.close(parser, endMatch);
                return true;
            }

            // TODO: Move this logic into TagSyntax.
            var runLength = endMatch.Groups[0].Length;
            var openingRunLength = this.endPos - this.startPos;
            var closingMatchStart = parser.pos;
            var closingMatchEnd = parser.pos + runLength - 1;
            var closingDelimiterRun =
                _DelimiterRun.tryParse(parser, closingMatchStart, closingMatchEnd);
            if (closingDelimiterRun != null && closingDelimiterRun.canClose) {
                // Emphasis rules #9 and #10:
                var oneRunOpensAndCloses =
                    (this.openingDelimiterRun.canOpen && this.openingDelimiterRun.canClose) ||
                    (closingDelimiterRun.canOpen && closingDelimiterRun.canClose);
                if (oneRunOpensAndCloses &&
                    (openingRunLength + closingDelimiterRun.length) % 3 == 0) {
                    return false;
                }

                // Close the tag.
                this.close(parser, endMatch);
                return true;
            }
            else {
                return false;
            }
        }

        /// Pops this tag off the stack, completes it, and adds it to the output.
        ///
        /// Will discard any unmatched tags that happen to be above it on the stack.
        /// If this is the last node in the stack, returns its children.
        internal List<Node> close(InlineParser parser, Match endMatch) {
            // If there are unclosed tags on top of this one when it's closed, that
            // means they are mismatched. Mismatched tags are treated as plain text in
            // markdown. So for each tag above this one, we write its start tag as text
            // and then adds its children to this one's children.
            var index = parser._stack.IndexOf(this);

            // Remove the unmatched children.
            var unmatchedTags = parser._stack.sublist(index + 1);
            parser._stack.removeRange(index + 1, parser._stack.Count);
            // Flatten them out onto this tag.
            foreach (var unmatched in unmatchedTags) {
                // Write the start tag as text.
                parser.writeTextRange(unmatched.startPos, unmatched.endPos);

                // Bequeath its children unto this tag.
                this.children.AddRange(unmatched.children);
            }

            // Pop this off the stack.
            parser.writeText();
            parser._stack.removeLast();

            // If the stack is empty now, this is the special "results" node.
            if (parser._stack.isEmpty()) {
                return this.children;
            }

            var endMatchIndex = parser.pos;

            // We are still parsing, so add this to its parent's children.
            if (this.syntax.onMatchEnd(parser, endMatch, this)) {
                parser.consume(endMatch.Groups[0].Length);
            }
            else {
                // Didn't close correctly so revert to text.
                parser.writeTextRange(this.startPos, this.endPos);
                parser._stack.last().children.AddRange(this.children);
                parser.pos = endMatchIndex;
                parser.advanceBy(endMatch.Groups[0].Length);
            }

            return null;
        }

        internal string textContent {
            get { return this.children.Select((Node child) => child.textContent).join(""); }
        }
    }

    class InlineLink {
        internal string destination;
        internal string title;

        public InlineLink(string destination, string title = null) {
            this.destination = destination;
            this.title = title;
        }
    }
}