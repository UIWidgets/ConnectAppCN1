using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ConnectApp.Utils;
using markdownRender;
using Unity.UIWidgets.cupertino;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace markdown {
    /// Signature for callbacks used by [MarkdownWidget] when the user taps a link.
    ///
    /// Used by [MarkdownWidget.onTapLink].
    public delegate void MarkdownTapLinkCallback(string href);

    /// Signature for custom image widget.
    ///
    /// Used by [MarkdownWidget.imageBuilder]
    public delegate Widget MarkdownImageBuilder(Uri uri);

    /// Signature for custom checkbox widget.
    ///
    /// Used by [MarkdownWidget.checkboxBuilder]
    public delegate Widget MarkdownCheckboxBuilder(bool value);

    /// [material] - create MarkdownStyleSheet based on MaterialTheme
    /// [cupertino] - create MarkdownStyleSheet based on CupertinoTheme
    /// [platform] - create MarkdownStyleSheet based on the Platform where the
    /// is running on. Material on Android and Cupertino on iOS
    public enum MarkdownStyleSheetBaseTheme {
        material,
        cupertino,
        platform
    }

    /// A base class for widgets that parse and display Markdown.
    ///
    /// Supports all standard Markdown from the original
    /// [Markdown specification](https://daringfireball.net/projects/markdown/).
    ///
    /// See also:
    ///
    ///  * [Markdown], which is a scrolling container of Markdown.
    ///  * [MarkdownBody], which is a non-scrolling container of Markdown.
    ///  * <https://daringfireball.net/projects/markdown/>
    public abstract class MarkdownWidget : StatefulWidget {
        public Action<string, List<Node>> onParsed;
        public Func<string, List<Node>> getCachedParsed;

        public string data;

        public bool selectable;

        public MarkdownStyleSheet styleSheet;

        public MarkdownStyleSheetBaseTheme styleSheetTheme;

        internal SyntaxHighlighter syntaxHighlighter;

        internal MarkdownTapLinkCallback onTapLink;

        public string imageDirectory;

        public ExtensionSet extensionSet;

        public MarkdownImageBuilder imageBuilder;

        public MarkdownCheckboxBuilder checkboxBuilder;

        public bool fitContent;

        /// Creates a widget that parses and displays Markdown.
        ///
        /// The [data] argument must not be null.
        protected MarkdownWidget(
            string data,
            Key key = null,
            bool selectable = false,
            MarkdownStyleSheet styleSheet = null,
            MarkdownStyleSheetBaseTheme styleSheetTheme = MarkdownStyleSheetBaseTheme.material,
            SyntaxHighlighter syntaxHighlighter = null,
            MarkdownTapLinkCallback onTapLink = null,
            string imageDirectory = null,
            ExtensionSet extensionSet = null,
            MarkdownImageBuilder imageBuilder = null,
            MarkdownCheckboxBuilder checkboxBuilder = null,
            bool fitContent = false
        ) : base(key) {
            D.assert(data != null);
            D.assert(selectable != null);
            this.data = data;
            this.selectable = selectable;
            this.styleSheet = styleSheet;
            this.styleSheetTheme = styleSheetTheme;
            this.syntaxHighlighter = syntaxHighlighter;
            this.onTapLink = onTapLink;
            this.imageDirectory = imageDirectory;
            this.extensionSet = extensionSet;
            this.imageBuilder = imageBuilder;
            this.checkboxBuilder = checkboxBuilder;
            this.fitContent = fitContent;
        }

        /// Subclasses should override this function to display the given children,
        /// which are the parsed representation of [data].
        public abstract Widget build(BuildContext context, List<Widget> children);

        public override State createState() {
            return new _MarkdownWidgetState();
        }
    }

    class _MarkdownWidgetState : State<MarkdownWidget>, builder.IMarkdownBuilderDelegate {
        List<Widget> _children = new List<Widget>();

        List<GestureRecognizer> _recognizers = new List<GestureRecognizer>();

        public override void didChangeDependencies() {
            this._parseMarkdown();
            base.didChangeDependencies();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            var noldWidget = oldWidget as MarkdownWidget;
            if (oldWidget == null) {
                return;
            }

            base.didUpdateWidget(oldWidget);

            if (this.widget.data != noldWidget.data || this.widget.styleSheet != noldWidget.styleSheet) {
                this._parseMarkdown();
            }
        }

        public override void dispose() {
            this._disposeRecongnizer();
            base.dispose();
        }

        void _parseMarkdown() {
            this.updateState(new List<Widget>());

            MarkdownStyleSheet fallbackStyleSheet = this.kFallbackStyle(this.context, this.widget.styleSheetTheme);
            MarkdownStyleSheet styleSheet = fallbackStyleSheet.merge(this.widget.styleSheet);
            this._disposeRecongnizer();

            string[] lines = this.widget.data.split(new Regex("\r?\n"));
            Document document = new Document(extensionSet: this.widget.extensionSet ?? ExtensionSet.githubFlavored,
                inlineSyntaxes: new List<InlineSyntax> {new TaskListSyntax()}, encodeHtml: false);
            builder.MarkdownBuilder builder = new builder.MarkdownBuilder(this, this.widget.selectable, styleSheet,
                this.widget.imageDirectory, this.widget.imageBuilder, this.widget.checkboxBuilder,
                this.widget.fitContent);
            this._children = builder.build(document.parseLines(new List<string>(lines)));
        }

        void updateState(List<Widget> elements) {
            using (WindowProvider.of(this.context).getScope()) {
                this.setState((() => { this._children = elements; }));
            }
        }


        MarkdownStyleSheet kFallbackStyle(
            BuildContext context,
            MarkdownStyleSheetBaseTheme baseTheme
        ) {
            switch (baseTheme) {
                case MarkdownStyleSheetBaseTheme.platform:
                    return CCommonUtils.isIPhone
                        ? MarkdownStyleSheet.fromCupertinoTheme(CupertinoTheme.of(context))
                        : MarkdownStyleSheet.fromTheme(Theme.of(context));
                case MarkdownStyleSheetBaseTheme.cupertino:
                    return MarkdownStyleSheet.fromCupertinoTheme(CupertinoTheme.of(context));
                default:
                    return MarkdownStyleSheet.fromTheme(Theme.of(context));
            }
        }

        void _disposeRecongnizer() {
            if (this._recognizers.isEmpty()) {
                return;
            }

            var localRecognizers = new List<GestureRecognizer>(this._recognizers);
            this._recognizers.Clear();
            foreach (var recognizer in localRecognizers) {
                recognizer.dispose();
            }
        }

        public GestureRecognizer createLink(string href) {
            TapGestureRecognizer recognizer = new TapGestureRecognizer();
            recognizer.onTap = () => {
                if (this.widget.onTapLink != null) {
                    this.widget.onTapLink(href);
                }
            };

            this._recognizers.Add(recognizer);
            return recognizer;
        }

        public TextSpan formatText(MarkdownStyleSheet styleSheet, string code) {
            code = code.replaceAll(new Regex("\n$"), ' ');
            if (this.widget.syntaxHighlighter != null) {
                return this.widget.syntaxHighlighter.format(code);
            }

            return new TextSpan(text: code, style: styleSheet.code);
        }

        public override Widget build(BuildContext context) {
            return this.widget.build(context, this._children);
        }
    }


    /// A scrolling widget that parses and displays Markdown.
    ///
    /// Supports all standard Markdown from the original
    /// [Markdown specification](https://daringfireball.net/projects/markdown/).
    ///
    /// See also:
    ///
    ///  * [MarkdownBody], which is a non-scrolling container of Markdown.
    ///  * <https://daringfireball.net/projects/markdown/>
    public class MarkdownBody : MarkdownWidget {
        /// Creates a scrolling widget that parses and displays Markdown.
        public MarkdownBody(
            string data,
            Key key = null,
            bool selectable = false,
            MarkdownStyleSheet styleSheet = null,
            MarkdownStyleSheetBaseTheme styleSheetTheme = MarkdownStyleSheetBaseTheme.material,
            SyntaxHighlighter syntaxHighlighter = null,
            MarkdownTapLinkCallback onTapLink = null,
            string imageDirectory = null,
            ExtensionSet extensionSet = null,
            MarkdownImageBuilder imageBuilder = null,
            MarkdownCheckboxBuilder checkboxBuilder = null,
            bool shrinkWrap = true,
            bool fitContent = true) : base(data, key, selectable, styleSheet, styleSheetTheme, syntaxHighlighter,
            onTapLink, imageDirectory, extensionSet, imageBuilder, checkboxBuilder) {
            this.shrinkWrap = shrinkWrap;
            this.fitContent = fitContent;
        }

        bool shrinkWrap;
        bool fitContent;

        public override Widget build(BuildContext context, List<Widget> children) {
            if (children.Count == 1) {
                return children.Single();
            }

            return new Column(
                mainAxisSize: this.shrinkWrap ? MainAxisSize.min : MainAxisSize.max,
                crossAxisAlignment: this.fitContent ? CrossAxisAlignment.start : CrossAxisAlignment.stretch,
                children: children
            );
        }
    }


    public class Markdown : MarkdownWidget {
        /// Creates a scrolling widget that parses and displays Markdown.
        public Markdown(
            string data,
            Key key = null,
            bool selectable = false,
            MarkdownStyleSheet styleSheet = null,
            MarkdownStyleSheetBaseTheme styleSheetTheme = MarkdownStyleSheetBaseTheme.material,
            SyntaxHighlighter syntaxHighlighter = null,
            MarkdownTapLinkCallback onTapLink = null,
            string imageDirectory = null,
            ExtensionSet extensionSet = null,
            MarkdownImageBuilder imageBuilder = null,
            MarkdownCheckboxBuilder checkboxBuilder = null,
            EdgeInsets padding = null,
            ScrollPhysics physics = null,
            bool shrinkWrap = false) : base(data, key, selectable, styleSheet, styleSheetTheme, syntaxHighlighter,
            onTapLink, imageDirectory, extensionSet, imageBuilder, checkboxBuilder) {
            this.padding = padding ?? EdgeInsets.all(16);
            this.physics = physics;
            this.shrinkWrap = shrinkWrap;
        }

        /// The amount of space by which to inset the children.
        EdgeInsets padding;

        /// How the scroll view should respond to user input.
        ///
        /// See also: [ScrollView.physics]
        ScrollPhysics physics;

        /// Whether the extent of the scroll view in the scroll direction should be
        /// determined by the contents being viewed.
        ///
        /// See also: [ScrollView.shrinkWrap]
        bool shrinkWrap;

        public override Widget build(BuildContext context, List<Widget> children) {
            return new ListView(
                padding: this.padding,
                physics: this.physics,
                shrinkWrap: this.shrinkWrap,
                children: children
            );
        }
    }


    class TaskListSyntax : InlineSyntax {
        const string _pattern = "^ *\\[([ xX])\\] +";

        public TaskListSyntax() : base(_pattern) {
        }

        internal override bool onMatch(InlineParser parser, Match match) {
            Element el = Element.withTag("input");
            el.attributes["type"] = "checkbox";
            el.attributes["disabled"] = "true";
            el.attributes["checked"] = $"{match.Groups[1].Value.Trim().isNotEmpty()}";
            parser.addNode(el);
            return true;
        }
    }
}