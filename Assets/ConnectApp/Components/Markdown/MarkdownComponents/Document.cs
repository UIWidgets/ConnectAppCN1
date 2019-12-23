using System;
using System.Collections.Generic;
using UnityEngine;

namespace markdown {
    public class Document {
        public Dictionary<string, LinkReference> linkReferences = new Dictionary<string, LinkReference>();
        internal ExtensionSet extensionSet;
        internal Resolver linkResolver;
        internal Resolver imageLinkResolver;
        internal bool encodeHtml;
        List<BlockSyntax> _blockSyntaxes = new List<BlockSyntax>();
        List<InlineSyntax> _inlineSyntaxes = new List<InlineSyntax>();

        internal IEnumerable<BlockSyntax> blockSyntaxes {
            get { return this._blockSyntaxes; }
        }

        internal IEnumerable<InlineSyntax> inlineSyntaxes {
            get { return this._inlineSyntaxes; }
        }


        public Document(
            IEnumerable<BlockSyntax> blockSyntaxes = null,
            IEnumerable<InlineSyntax> inlineSyntaxes = null,
            ExtensionSet extensionSet = null,
            Resolver linkResolver = null,
            Resolver imageLinkResolver = null,
            bool encodeHtml = true) {
            this.linkResolver = linkResolver;
            this.imageLinkResolver = imageLinkResolver;
            this.extensionSet = extensionSet ?? ExtensionSet.commonMark;
            this.encodeHtml = encodeHtml;

            this._blockSyntaxes.AddRange(blockSyntaxes ?? new List<BlockSyntax>());
            this._blockSyntaxes.AddRange(this.extensionSet.blockSyntaxes);

            this._inlineSyntaxes.AddRange(inlineSyntaxes ?? new List<InlineSyntax>());
            this._inlineSyntaxes.AddRange(this.extensionSet.inlineSyntaxes);
        }

        /// Parses the given [lines] of Markdown to a series of AST nodes.
        public List<Node> parseLines(List<string> lines) {
            var nodes = new BlockParser(lines, this).parseLines();
            this._parseInlineContent(nodes);
            return nodes;
        }

        /// Parses the given inline Markdown [text] to a series of AST nodes.
        List<Node> parseInline(string text) {
            try {
                var parser = new InlineParser(text, this);
                var nodes = parser.parse();
                return nodes;
            }
            catch (Exception e) {
                Debug.LogError(e);
            }

            return null;
        }

        void _parseInlineContent(List<Node> nodes) {
            for (int i = 0; i < nodes.Count; i++) {
                var node = nodes[i];

                if (node is UnparsedContent) {
                    var inlineNodes = this.parseInline(node.textContent);
                    nodes.RemoveAt(i);
                    nodes.InsertRange(i, inlineNodes);
                    i += inlineNodes.Count - 1;
                }
                else if (node is Element && node.children != null) {
                    this._parseInlineContent(node.children);
                }
            }
        }
    }

    /// A [link reference
    /// definition](http://spec.commonmark.org/0.28/#link-reference-definitions).
    public class LinkReference {
        /// The [link label](http://spec.commonmark.org/0.28/#link-label).
        ///
        /// Temporarily, this class is also being used to represent the link data for
        /// an inline link (the destination and title), but this should change before
        /// the package is released.
        internal string label;

        /// The [link destination](http://spec.commonmark.org/0.28/#link-destination).
        public string destination;

        /// The [link title](http://spec.commonmark.org/0.28/#link-title).
        internal string title;

        /// Construct a new [LinkReference], with all necessary fields.
        ///
        /// If the parsed link reference definition does not include a title, use
        /// `null` for the [title] parameter.
        public LinkReference(string label, string destination, string title) {
            this.label = label;
            this.destination = destination;
            this.title = title;
        }
    }
}