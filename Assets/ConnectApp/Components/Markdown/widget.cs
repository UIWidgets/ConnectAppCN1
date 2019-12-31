using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using UnityEngine;


namespace markdown {
    public delegate void MarkdownTapLinkCallback(string href);

    public delegate Widget MarkdownImageBuilder(Uri uri);

    public delegate Widget MarkdownCheckboxBuilder(bool value);

    public abstract class SyntaxHighlighter {
        // ignore: one_member_abstracts
        /// Returns the formatted [TextSpan] for the given string.
        public abstract TextSpan format(string source);
    }


    public abstract class MarkdownWidget : StatefulWidget {
        protected MarkdownWidget(
            Key key,
            string data,
            MarkdownStyleSheet markdownStyleSheet,
            SyntaxHighlighter syntaxHighlighter1,
            MarkdownTapLinkCallback onTapLink,
            string imageDirectory,
            ExtensionSet extensionSet,
            MarkdownImageBuilder imageBuilder,
            MarkdownCheckboxBuilder checkboxBuilder,
            bool fitContent = false,
            bool selectable = false
        ) : base(key) {
            Debug.Assert(data != null);
            this.data = data;
            this.styleSheet = markdownStyleSheet;
            this.syntaxHighlighter = syntaxHighlighter1;
            this.onTapLink = onTapLink;
            this.extensionSet = extensionSet;
            this.imageDirectory = imageDirectory;
            this.imageBuilder = imageBuilder;
            this.checkboxBuilder = checkboxBuilder;
            this.fitContent = fitContent;
            this.selectable = selectable;
        }

        public string data;

        public bool selectable;

        public MarkdownStyleSheet styleSheet;

//        public MarkdownStyleSheetBaseTheme 

        public SyntaxHighlighter syntaxHighlighter;

        public MarkdownTapLinkCallback onTapLink;

        public string imageDirectory;

        public ExtensionSet extensionSet;

        public MarkdownImageBuilder imageBuilder;

        public MarkdownCheckboxBuilder checkboxBuilder;

        public bool fitContent;

        public abstract Widget build(BuildContext context, List<Widget> children);

        public override State createState() {
            return new _MarkdownWidgetState();
        }
    }

    class _MarkdownWidgetState : State<MarkdownWidget>, IMarkdownBuilderDelegate {
        private List<Widget> _children;

        public List<GestureRecognizer> _recognizers = new List<GestureRecognizer> { };

        public override void didChangeDependencies() {
            _parseMarkdown();
            base.didChangeDependencies();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            if (oldWidget == null) {
                return;
            }

            if (widget.data != (oldWidget as MarkdownWidget)?.data ||
                widget.styleSheet != (oldWidget as MarkdownWidget)?.styleSheet) {
                _parseMarkdown();
            }
        }

        public override void dispose() {
            _disposeRecognizers();
            base.dispose();
        }

        private void _parseMarkdown() {
            // todo different here
            MarkdownStyleSheet styleSheet = widget.styleSheet ?? MarkdownStyleSheet.fromTheme(Theme.of(context));
            
            _disposeRecognizers();
            var lines = widget.data.Replace("\r\n", "\n").Split('\n');
            
            var document = new Document(
                extensionSet: widget.extensionSet ?? ExtensionSet.githubFlavored,
                inlineSyntaxes: new List<InlineSyntax> {new TaskListSyntax()},
                encodeHtml: false
            );
            var builder = new MarkdownBuilder(
                dele: this,
                selectable: widget.selectable,
                styleSheet: styleSheet, // todo merge fallback style sheet
                imageDirectory: widget.imageDirectory,
                imageBuilder: widget.imageBuilder,
                checkboxBuilder: widget.checkboxBuilder,
                fitContent: widget.fitContent
            );
            _children = builder.build(document.parseLines(lines.ToList().remove(string.IsNullOrEmpty)));
        }

        private void _disposeRecognizers() {
            if (_recognizers.isEmpty()) {
                return;
            }

            var localRecognizers = _recognizers.ToArray();
            _recognizers.Clear();
            foreach (var r in localRecognizers) {
                r.dispose();
            }
        }

        public override Widget build(BuildContext context) {
            return widget.build(context, _children);
        }

        public GestureRecognizer createLink(string href) {
            var recognizer = new TapGestureRecognizer();
            recognizer.onTap = () => { widget.onTapLink?.Invoke(href); };

            _recognizers.Add(recognizer);
            return recognizer;
        }

        public TextSpan formatText(MarkdownStyleSheet styleSheet, string code) {
            //TODO: format code!
            if (widget.syntaxHighlighter != null)
                return widget.syntaxHighlighter.format(code);
            return new TextSpan(code, styleSheet.code);
        }
    }

//    public class MarkdownBody : MarkdownWidget {
//        public MarkdownBody(
//            Key key,
//            string data,
//            MarkdownStyleSheet markdownStyleSheet,
//            SyntaxHighlighter syntaxHighlighter1,
//            MarkdownTapLinkCallback onTapLink,
//            string imageDirectory,
//            MarkdownImageBuilder imageBuilder,
//            MarkdownCheckboxBuilder checkboxBuilder,
//            bool fitContent = true
//        ) : base(
//            key,
//            data,
//            markdownStyleSheet,
//            syntaxHighlighter1,
//            onTapLink,
//            imageDirectory,
//            imageBuilder,
//            checkboxBuilder,
//            fitContent) {
//        }
//
//        public override Widget build(BuildContext context, List<Widget> children) {
//            throw new NotImplementedException();
//        }
//    }

    public class Markdown : MarkdownWidget {
        public Markdown(
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
            bool shrinkWrap = false
        ) : base(key, data, markdownStyleSheet, syntaxHighlighter,
            onTapLink, imageDirectory, extensionSet, imageBuilder, checkboxBuilder, selectable) {
            this.padding = EdgeInsets.all(16);
            this.physics = physics;
            this.shrinkWrap = shrinkWrap;
        }

        public EdgeInsets padding;

        public ScrollPhysics physics;

        public bool shrinkWrap;

        public override Widget build(BuildContext context, List<Widget> children) {
            return new ListView(
                padding: padding,
                physics: physics,
                shrinkWrap: shrinkWrap,
                children: children
            );
        }
    }

    class TaskListSyntax : InlineSyntax {
        private static string _pattern = @"^ *\[([ xX])\] +";

        public TaskListSyntax() : base(_pattern) {
        }

        internal override bool onMatch(InlineParser parser, Match match) {
            var el = Element.withTag("input");
            el.attributes["type"] = "checkbox";
            el.attributes["disabled"] = "true";
            el.attributes["checked"] = match.Groups[1].Value.ToLower();
            parser.addNode(el);
            return true;
        }
    }
}