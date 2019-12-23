using System.Collections.Generic;
using System.Linq;

namespace markdown {
    public delegate Node Resolver(string name, List<string> title = null);

    /// Base class for any AST item.
    ///
    /// Roughly corresponds to Node in the DOM. Will be either an Element or Text.
    public abstract class Node {
        public abstract void accept(NodeVisitor visitor);

        public List<Node> children;

        public abstract string textContent { get; }
    }

    public class Element : Node {
        public string tag;

        public Dictionary<string, string> attributes;
        public string generatedId;

        /// Instantiates a [tag] Element with [children].
        public Element(string tag, IEnumerable<Node> children) {
            this.tag = tag;
            this.children = children.ToList();
            this.attributes = new Dictionary<string, string>();
        }


        /// Instantiates an empty, self-closing [tag] Element.
        public static Element empty(string tag) {
            return new Element(tag, new List<Node>());
        }

        public static Element withTag(string tag) {
            return new Element(tag, new List<Node>());
        }

        public static Element text(string tag, string text) {
            return new Element(tag, new List<Node>() {new Text(text)});
        }

        public bool isEmpty {
            get { return this.children == null; }
        }

        public override void accept(NodeVisitor visitor) {
            if (visitor.visitElementBefore(this)) {
                if (this.children != null) {
                    foreach (var child in this.children) {
                        child.accept(visitor);
                    }
                }

                visitor.visitElementAfter(this);
            }
        }

        public override string textContent {
            get { return this.children == null ? "" : string.Join("", this.children.Select(t => t.textContent)); }
        }
    }


    public class Text : Node {
        public string text;

        public Text(string text) {
            this.text = text;
        }

        public override void accept(NodeVisitor visitor) {
            visitor.visitText(this);
        }

        public override string textContent {
            get { return this.text; }
        }
    }

    /// Inline content that has not been parsed into inline nodes (strong, links,
    /// etc).
    ///
    /// These placeholder nodes should only remain in place while the block nodes
    /// of a document are still being parsed, in order to gather all reference link
    /// definitions.
    public class UnparsedContent : Node {
        public override string textContent { get; }

        public UnparsedContent(string textContent) {
            this.textContent = textContent;
        }

        public override void accept(NodeVisitor visitor) {
        }
    }

    /// Visitor pattern for the AST.
    ///
    /// Renderers or other AST transformers should implement this.
    public abstract class NodeVisitor {
        public abstract void visitText(Text txt);
        public abstract bool visitElementBefore(Element element);
        public abstract void visitElementAfter(Element element);
    }
}