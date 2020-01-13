using System.Collections.Generic;

namespace markdown {
    public class Document {
        public Dictionary<string, LinkReference> linkReferences = new Dictionary<string, LinkReference>();
        public ExtensionSet extensionSet;
        public Resolver linkResolver;
        public Resolver imageLinkResolver;
        public bool encodeHtml;
        public List<BlockSyntax> _blockSyntaxes = new List<BlockSyntax>();
        public List<InlineSyntax> _inlineSyntaxes = new List<InlineSyntax>();

        public IEnumerable<BlockSyntax> blockSyntaxes {
            get { return this._blockSyntaxes; }
        }

        public IEnumerable<InlineSyntax> inlineSyntaxes {
            get { return this._inlineSyntaxes; }
        }

        public Document(
            IEnumerable<BlockSyntax> blockSyntaxes = null,
            IEnumerable<InlineSyntax> inlineSyntaxes = null,
            ExtensionSet extensionSet = null,
            Resolver linkResolver = null,
            Resolver imageLinkResolver = null,
            bool encodeHtml = true
        ) {
            this.linkResolver = linkResolver;
            this.imageLinkResolver = imageLinkResolver;
            this.extensionSet = extensionSet ?? ExtensionSet.commanMark;
//            this.extensionSet = extensionSet ?? ExtensionSet.githubWeb;
            this.encodeHtml = encodeHtml;

            this._blockSyntaxes.AddRange(blockSyntaxes ?? new List<BlockSyntax>());
            this._blockSyntaxes.AddRange(this.extensionSet.blockSyntaxes);

            this._inlineSyntaxes.AddRange(inlineSyntaxes ?? new List<InlineSyntax>());
            this._inlineSyntaxes.AddRange(this.extensionSet.inlineSyntaxes);
        }

        public List<Node> parseLines(List<string> lines) {
            var nodes = new BlockParser(lines, this).parseLines();
            this._parseInlineContent(nodes);
            return nodes;
        }

        public List<Node> parseInline(string text) {
            var parser = new InlineParser(text, this);
            return parser.parse();
        }

        public void _parseInlineContent(List<Node> nodes) {
            for (int i = 0; i < nodes.Count; i++) {
                var node = nodes[i];
                if (node is UnparsedContent) {
                    var inlineNodes = this.parseInline(node.textContent);
                    nodes.RemoveAt(i);
                    nodes.InsertRange(i, inlineNodes);
                    i += inlineNodes.Count - 1;
                }
                else if (node is Element && (node as Element).children != null) {
                    this._parseInlineContent((node as Element).children);
                }
            }
        }
    }

    public class LinkReference {
        public string label;
        public string destination;
        public string title;

        public LinkReference(string label, string destination, string title) {
            this.label = label;
            this.destination = destination;
            this.title = title;
        }
    }
}