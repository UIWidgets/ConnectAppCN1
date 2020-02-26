using System.Collections.Generic;
using System.Linq;

namespace markdown {
    public delegate Node Resolver(string name, List<string> title = null);

    public abstract class Node {
        public abstract void accept(NodeVisitor visitor);

        public abstract string textContent { get; }
    }

    public class Element : Node {
        public string tag;

        public List<Node> children;

        public Dictionary<string, string> attributes;

        public string generatedId;

        public Element(string tag, List<Node> children) {
            this.tag = tag;
            this.children = children;
            this.attributes = new Dictionary<string, string>();
        }

        public static Element empty(string tag) {
            return new Element(tag, null);
        }

        public static Element withTag(string tag) {
            return new Element(tag, new List<Node>());
        }

        public static Element text(string tag, string text) {
            return new Element(tag, new List<Node>() {
                new Text(text)
            });
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

    public class HTML : Text {
        public HTML(string text) : base(text) { }
    }

    public class UnparsedContent : Node {
        public UnparsedContent(string textContent) {
            this.textContent = textContent;
        }

        public override void accept(NodeVisitor visitor) { }

        public override string textContent { get; }
    }

    public abstract class NodeVisitor {
        public abstract void visitText(Text text);

        public abstract bool visitElementBefore(Element element);

        public abstract void visitElementAfter(Element element);
    }
}