// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

// ReSharper disable InconsistentNaming
namespace HtmlAgilityPack {
    /// <summary>
    /// Represents an HTML node.
    /// </summary>
    [DebuggerDisplay("Name: {OriginalName}")]
    public partial class HtmlNode {
        #region Fields

        internal HtmlAttributeCollection _attributes;
        internal HtmlNodeCollection _childnodes;
        internal HtmlNode _endnode;

        bool _changed;
        internal string _innerhtml;
        internal int _innerlength;
        internal int _innerstartindex;
        internal int _line;
        internal int _lineposition;
        string _name;
        internal int _namelength;
        internal int _namestartindex;
        internal HtmlNode _nextnode;
        internal HtmlNodeType _nodetype;
        internal string _outerhtml;
        internal int _outerlength;
        internal int _outerstartindex;
        string _optimizedName;
        internal HtmlDocument _ownerdocument;
        internal HtmlNode _parentnode;
        internal HtmlNode _prevnode;
        internal HtmlNode _prevwithsamename;
        internal bool _starttag;
        internal int _streamposition;

        #endregion

        #region Static Members

        /// <summary>
        /// Gets the name of a comment node. It is actually defined as '#comment'.
        /// </summary>
        public static readonly string HtmlNodeTypeNameComment = "#comment";

        /// <summary>
        /// Gets the name of the document node. It is actually defined as '#document'.
        /// </summary>
        public static readonly string HtmlNodeTypeNameDocument = "#document";

        /// <summary>
        /// Gets the name of a text node. It is actually defined as '#text'.
        /// </summary>
        public static readonly string HtmlNodeTypeNameText = "#text";

        /// <summary>
        /// Gets a collection of flags that define specific behaviors for specific element nodes.
        /// The table contains a DictionaryEntry list with the lowercase tag name as the Key, and a combination of HtmlElementFlags as the Value.
        /// </summary>
        public static Dictionary<string, HtmlElementFlag> ElementsFlags;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize HtmlNode. Builds a list of all tags that have special allowances
        /// </summary>
        static HtmlNode() {
            // tags whose content may be anything
            ElementsFlags = new Dictionary<string, HtmlElementFlag>();
            ElementsFlags.Add("script", HtmlElementFlag.CData);
            ElementsFlags.Add("style", HtmlElementFlag.CData);
            ElementsFlags.Add("noxhtml", HtmlElementFlag.CData);

            // tags that can not contain other tags
            ElementsFlags.Add("base", HtmlElementFlag.Empty);
            ElementsFlags.Add("link", HtmlElementFlag.Empty);
            ElementsFlags.Add("meta", HtmlElementFlag.Empty);
            ElementsFlags.Add("isindex", HtmlElementFlag.Empty);
            ElementsFlags.Add("hr", HtmlElementFlag.Empty);
            ElementsFlags.Add("col", HtmlElementFlag.Empty);
            ElementsFlags.Add("img", HtmlElementFlag.Empty);
            ElementsFlags.Add("param", HtmlElementFlag.Empty);
            ElementsFlags.Add("embed", HtmlElementFlag.Empty);
            ElementsFlags.Add("frame", HtmlElementFlag.Empty);
            ElementsFlags.Add("wbr", HtmlElementFlag.Empty);
            ElementsFlags.Add("bgsound", HtmlElementFlag.Empty);
            ElementsFlags.Add("spacer", HtmlElementFlag.Empty);
            ElementsFlags.Add("keygen", HtmlElementFlag.Empty);
            ElementsFlags.Add("area", HtmlElementFlag.Empty);
            ElementsFlags.Add("input", HtmlElementFlag.Empty);
            ElementsFlags.Add("basefont", HtmlElementFlag.Empty);

            ElementsFlags.Add("form", HtmlElementFlag.CanOverlap | HtmlElementFlag.Empty);

            // they sometimes contain, and sometimes they don 't...
            ElementsFlags.Add("option", HtmlElementFlag.Empty);

            // tag whose closing tag is equivalent to open tag:
            // <p>bla</p>bla will be transformed into <p>bla</p>bla
            // <p>bla<p>bla will be transformed into <p>bla<p>bla and not <p>bla></p><p>bla</p> or <p>bla<p>bla</p></p>
            //<br> see above
            ElementsFlags.Add("br", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
            ElementsFlags.Add("p", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
        }

        /// <summary>
        /// Initializes HtmlNode, providing type, owner and where it exists in a collection
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ownerdocument"></param>
        /// <param name="index"></param>
        public HtmlNode(HtmlNodeType type, HtmlDocument ownerdocument, int index) {
            this._nodetype = type;
            this._ownerdocument = ownerdocument;
            this._outerstartindex = index;

            switch (type) {
                case HtmlNodeType.Comment:
                    this.Name = HtmlNodeTypeNameComment;
                    this._endnode = this;
                    break;

                case HtmlNodeType.Document:
                    this.Name = HtmlNodeTypeNameDocument;
                    this._endnode = this;
                    break;

                case HtmlNodeType.Text:
                    this.Name = HtmlNodeTypeNameText;
                    this._endnode = this;
                    break;
            }

            if (this._ownerdocument.Openednodes != null) {
                if (!this.Closed) {
                    // we use the index as the key

                    // -1 means the node comes from public
                    if (-1 != index) {
                        this._ownerdocument.Openednodes.Add(index, this);
                    }
                }
            }

            if ((-1 != index) || (type == HtmlNodeType.Comment) || (type == HtmlNodeType.Text)) {
                return;
            }

            // innerhtml and outerhtml must be calculated
            this.SetChanged();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collection of HTML attributes for this node. May not be null.
        /// </summary>
        public HtmlAttributeCollection Attributes {
            get {
                if (!this.HasAttributes) {
                    this._attributes = new HtmlAttributeCollection(this);
                }

                return this._attributes;
            }
            internal set { this._attributes = value; }
        }

        /// <summary>
        /// Gets all the children of the node.
        /// </summary>
        public HtmlNodeCollection ChildNodes {
            get { return this._childnodes ?? (this._childnodes = new HtmlNodeCollection(this)); }
            internal set { this._childnodes = value; }
        }

        /// <summary>
        /// Gets a value indicating if this node has been closed or not.
        /// </summary>
        public bool Closed {
            get { return (this._endnode != null); }
        }

        /// <summary>
        /// Gets the collection of HTML attributes for the closing tag. May not be null.
        /// </summary>
        public HtmlAttributeCollection ClosingAttributes {
            get { return !this.HasClosingAttributes ? new HtmlAttributeCollection(this) : this._endnode.Attributes; }
        }

        internal HtmlNode EndNode {
            get { return this._endnode; }
        }

        /// <summary>
        /// Gets the first child of the node.
        /// </summary>
        public HtmlNode FirstChild {
            get { return !this.HasChildNodes ? null : this._childnodes[0]; }
        }

        /// <summary>
        /// Gets a value indicating whether the current node has any attributes.
        /// </summary>
        public bool HasAttributes {
            get {
                if (this._attributes == null) {
                    return false;
                }

                if (this._attributes.Count <= 0) {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this node has any child nodes.
        /// </summary>
        public bool HasChildNodes {
            get {
                if (this._childnodes == null) {
                    return false;
                }

                if (this._childnodes.Count <= 0) {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current node has any attributes on the closing tag.
        /// </summary>
        public bool HasClosingAttributes {
            get {
                if ((this._endnode == null) || (this._endnode == this)) {
                    return false;
                }

                if (this._endnode._attributes == null) {
                    return false;
                }

                if (this._endnode._attributes.Count <= 0) {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets or sets the value of the 'id' HTML attribute. The document must have been parsed using the OptionUseIdAttribute set to true.
        /// </summary>
        public string Id {
            get {
                if (this._ownerdocument.Nodesid == null) {
                    throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
                }

                return this.GetId();
            }
            set {
                if (this._ownerdocument.Nodesid == null) {
                    throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
                }

                if (value == null) {
                    throw new ArgumentNullException("value");
                }

                this.SetId(value);
            }
        }

        /// <summary>
        /// Gets or Sets the HTML between the start and end tags of the object.
        /// </summary>
        public virtual string InnerHtml {
            get {
                if (this._changed) {
                    this.UpdateHtml();
                    return this._innerhtml;
                }

                if (this._innerhtml != null) {
                    return this._innerhtml;
                }

                if (this._innerstartindex < 0) {
                    return string.Empty;
                }

                return this._ownerdocument.Text.Substring(this._innerstartindex, this._innerlength);
            }
            set {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(value);

                this.RemoveAllChildren();
                this.AppendChildren(doc.DocumentNode.ChildNodes);
            }
        }

        /// <summary>
        /// Gets or Sets the text between the start and end tags of the object.
        /// </summary>
        public virtual string InnerText {
            get {
                if (this._nodetype == HtmlNodeType.Text) {
                    return ((HtmlTextNode) this).Text;
                }

                if (this._nodetype == HtmlNodeType.Comment) {
                    return ((HtmlCommentNode) this).Comment;
                }

                // note: right now, this method is *slow*, because we recompute everything.
                // it could be optimized like innerhtml
                if (!this.HasChildNodes) {
                    return string.Empty;
                }

                string s = null;
                foreach (HtmlNode node in this.ChildNodes) {
                    s += node.InnerText;
                }

                return s;
            }
        }

        /// <summary>
        /// Gets the last child of the node.
        /// </summary>
        public HtmlNode LastChild {
            get { return !this.HasChildNodes ? null : this._childnodes[this._childnodes.Count - 1]; }
        }

        /// <summary>
        /// Gets the line number of this node in the document.
        /// </summary>
        public int Line {
            get { return this._line; }
            internal set { this._line = value; }
        }

        /// <summary>
        /// Gets the column number of this node in the document.
        /// </summary>
        public int LinePosition {
            get { return this._lineposition; }
            internal set { this._lineposition = value; }
        }

        /// <summary>
        /// Gets or sets this node's name.
        /// </summary>
        public string Name {
            get {
                if (this._optimizedName == null) {
                    if (this._name == null) {
                        this.Name = this._ownerdocument.Text.Substring(this._namestartindex, this._namelength);
                    }

                    if (this._name == null) {
                        this._optimizedName = string.Empty;
                    }
                    else {
                        this._optimizedName = this._name.ToLower();
                    }
                }

                return this._optimizedName;
            }
            set {
                this._name = value;
                this._optimizedName = null;
            }
        }

        /// <summary>
        /// Gets the HTML node immediately following this element.
        /// </summary>
        public HtmlNode NextSibling {
            get { return this._nextnode; }
            internal set { this._nextnode = value; }
        }

        /// <summary>
        /// Gets the type of this node.
        /// </summary>
        public HtmlNodeType NodeType {
            get { return this._nodetype; }
            internal set { this._nodetype = value; }
        }

        /// <summary>
        /// The original unaltered name of the tag
        /// </summary>
        public string OriginalName {
            get { return this._name; }
        }

        /// <summary>
        /// Gets or Sets the object and its content in HTML.
        /// </summary>
        public virtual string OuterHtml {
            get {
                if (this._changed) {
                    this.UpdateHtml();
                    return this._outerhtml;
                }

                if (this._outerhtml != null) {
                    return this._outerhtml;
                }

                if (this._outerstartindex < 0) {
                    return string.Empty;
                }

                return this._ownerdocument.Text.Substring(this._outerstartindex, this._outerlength);
            }
        }

        /// <summary>
        /// Gets the <see cref="HtmlDocument"/> to which this node belongs.
        /// </summary>
        public HtmlDocument OwnerDocument {
            get { return this._ownerdocument; }
            internal set { this._ownerdocument = value; }
        }

        /// <summary>
        /// Gets the parent of this node (for nodes that can have parents).
        /// </summary>
        public HtmlNode ParentNode {
            get { return this._parentnode; }
            internal set { this._parentnode = value; }
        }

        /// <summary>
        /// Gets the node immediately preceding this node.
        /// </summary>
        public HtmlNode PreviousSibling {
            get { return this._prevnode; }
            internal set { this._prevnode = value; }
        }

        /// <summary>
        /// Gets the stream position of this node in the document, relative to the start of the document.
        /// </summary>
        public int StreamPosition {
            get { return this._streamposition; }
        }

        /// <summary>
        /// Gets a valid XPath string that points to this node
        /// </summary>
        public string XPath {
            get {
                string basePath = (this.ParentNode == null || this.ParentNode.NodeType == HtmlNodeType.Document)
                    ? "/"
                    : this.ParentNode.XPath + "/";
                return basePath + this.GetRelativeXpath();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines if an element node can be kept overlapped.
        /// </summary>
        /// <param name="name">The name of the element node to check. May not be <c>null</c>.</param>
        /// <returns>true if the name is the name of an element node that can be kept overlapped, <c>false</c> otherwise.</returns>
        public static bool CanOverlapElement(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (!ElementsFlags.ContainsKey(name.ToLower())) {
                return false;
            }

            HtmlElementFlag flag = ElementsFlags[name.ToLower()];
            return (flag & HtmlElementFlag.CanOverlap) != 0;
        }

        /// <summary>
        /// Creates an HTML node from a string representing literal HTML.
        /// </summary>
        /// <param name="html">The HTML text.</param>
        /// <returns>The newly created node instance.</returns>
        public static HtmlNode CreateNode(string html) {
            // REVIEW: this is *not* optimum...
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.FirstChild;
        }

        /// <summary>
        /// Determines if an element node is a CDATA element node.
        /// </summary>
        /// <param name="name">The name of the element node to check. May not be null.</param>
        /// <returns>true if the name is the name of a CDATA element node, false otherwise.</returns>
        public static bool IsCDataElement(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (!ElementsFlags.ContainsKey(name.ToLower())) {
                return false;
            }

            HtmlElementFlag flag = ElementsFlags[name.ToLower()];
            return (flag & HtmlElementFlag.CData) != 0;
        }

        /// <summary>
        /// Determines if an element node is closed.
        /// </summary>
        /// <param name="name">The name of the element node to check. May not be null.</param>
        /// <returns>true if the name is the name of a closed element node, false otherwise.</returns>
        public static bool IsClosedElement(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (!ElementsFlags.ContainsKey(name.ToLower())) {
                return false;
            }

            HtmlElementFlag flag = ElementsFlags[name.ToLower()];
            return (flag & HtmlElementFlag.Closed) != 0;
        }

        /// <summary>
        /// Determines if an element node is defined as empty.
        /// </summary>
        /// <param name="name">The name of the element node to check. May not be null.</param>
        /// <returns>true if the name is the name of an empty element node, false otherwise.</returns>
        public static bool IsEmptyElement(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (name.Length == 0) {
                return true;
            }

            // <!DOCTYPE ...
            if ('!' == name[0]) {
                return true;
            }

            // <?xml ...
            if ('?' == name[0]) {
                return true;
            }

            if (!ElementsFlags.ContainsKey(name.ToLower())) {
                return false;
            }

            HtmlElementFlag flag = ElementsFlags[name.ToLower()];
            return (flag & HtmlElementFlag.Empty) != 0;
        }

        /// <summary>
        /// Determines if a text corresponds to the closing tag of an node that can be kept overlapped.
        /// </summary>
        /// <param name="text">The text to check. May not be null.</param>
        /// <returns>true or false.</returns>
        public static bool IsOverlappedClosingElement(string text) {
            if (text == null) {
                throw new ArgumentNullException("text");
            }

            // min is </x>: 4
            if (text.Length <= 4) {
                return false;
            }

            if ((text[0] != '<') ||
                (text[text.Length - 1] != '>') ||
                (text[1] != '/')) {
                return false;
            }

            string name = text.Substring(2, text.Length - 3);
            return CanOverlapElement(name);
        }

        /// <summary>
        /// Returns a collection of all ancestor nodes of this element.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HtmlNode> Ancestors() {
            HtmlNode node = this.ParentNode;
            if (node != null) {
                yield return node; //return the immediate parent node

                //now look at it's parent and walk up the tree of parents
                while (node.ParentNode != null) {
                    yield return node.ParentNode;
                    node = node.ParentNode;
                }
            }
        }

        /// <summary>
        /// Get Ancestors with matching name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<HtmlNode> Ancestors(string name) {
            for (HtmlNode n = this.ParentNode; n != null; n = n.ParentNode) {
                if (n.Name == name) {
                    yield return n;
                }
            }
        }

        /// <summary>
        /// Returns a collection of all ancestor nodes of this element.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HtmlNode> AncestorsAndSelf() {
            for (HtmlNode n = this; n != null; n = n.ParentNode) {
                yield return n;
            }
        }

        /// <summary>
        /// Gets all anscestor nodes and the current node
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<HtmlNode> AncestorsAndSelf(string name) {
            for (HtmlNode n = this; n != null; n = n.ParentNode) {
                if (n.Name == name) {
                    yield return n;
                }
            }
        }

        /// <summary>
        /// Adds the specified node to the end of the list of children of this node.
        /// </summary>
        /// <param name="newChild">The node to add. May not be null.</param>
        /// <returns>The node added.</returns>
        public HtmlNode AppendChild(HtmlNode newChild) {
            if (newChild == null) {
                throw new ArgumentNullException("newChild");
            }

            this.ChildNodes.Append(newChild);
            this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
            this.SetChanged();
            return newChild;
        }

        /// <summary>
        /// Adds the specified node to the end of the list of children of this node.
        /// </summary>
        /// <param name="newChildren">The node list to add. May not be null.</param>
        public void AppendChildren(HtmlNodeCollection newChildren) {
            if (newChildren == null) {
                throw new ArgumentNullException("newChildren");
            }

            foreach (HtmlNode newChild in newChildren) {
                this.AppendChild(newChild);
            }
        }

        /// <summary>
        /// Gets all Attributes with name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<HtmlAttribute> ChildAttributes(string name) {
            return this.Attributes.AttributesWithName(name);
        }

        /// <summary>
        /// Creates a duplicate of the node
        /// </summary>
        /// <returns></returns>
        public HtmlNode Clone() {
            return this.CloneNode(true);
        }

        /// <summary>
        /// Creates a duplicate of the node and changes its name at the same time.
        /// </summary>
        /// <param name="newName">The new name of the cloned node. May not be <c>null</c>.</param>
        /// <returns>The cloned node.</returns>
        public HtmlNode CloneNode(string newName) {
            return this.CloneNode(newName, true);
        }

        /// <summary>
        /// Creates a duplicate of the node and changes its name at the same time.
        /// </summary>
        /// <param name="newName">The new name of the cloned node. May not be null.</param>
        /// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself.</param>
        /// <returns>The cloned node.</returns>
        public HtmlNode CloneNode(string newName, bool deep) {
            if (newName == null) {
                throw new ArgumentNullException("newName");
            }

            HtmlNode node = this.CloneNode(deep);
            node.Name = newName;
            return node;
        }

        /// <summary>
        /// Creates a duplicate of the node.
        /// </summary>
        /// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself.</param>
        /// <returns>The cloned node.</returns>
        public HtmlNode CloneNode(bool deep) {
            HtmlNode node = this._ownerdocument.CreateNode(this._nodetype);
            node.Name = this.Name;

            switch (this._nodetype) {
                case HtmlNodeType.Comment:
                    ((HtmlCommentNode) node).Comment = ((HtmlCommentNode) this).Comment;
                    return node;

                case HtmlNodeType.Text:
                    ((HtmlTextNode) node).Text = ((HtmlTextNode) this).Text;
                    return node;
            }

            // attributes
            if (this.HasAttributes) {
                foreach (HtmlAttribute att in this._attributes) {
                    HtmlAttribute newatt = att.Clone();
                    node.Attributes.Append(newatt);
                }
            }

            // closing attributes
            if (this.HasClosingAttributes) {
                node._endnode = this._endnode.CloneNode(false);
                foreach (HtmlAttribute att in this._endnode._attributes) {
                    HtmlAttribute newatt = att.Clone();
                    node._endnode._attributes.Append(newatt);
                }
            }

            if (!deep) {
                return node;
            }

            if (!this.HasChildNodes) {
                return node;
            }

            // child nodes
            foreach (HtmlNode child in this._childnodes) {
                HtmlNode newchild = child.Clone();
                node.AppendChild(newchild);
            }

            return node;
        }

        /// <summary>
        /// Creates a duplicate of the node and the subtree under it.
        /// </summary>
        /// <param name="node">The node to duplicate. May not be <c>null</c>.</param>
        public void CopyFrom(HtmlNode node) {
            this.CopyFrom(node, true);
        }

        /// <summary>
        /// Creates a duplicate of the node.
        /// </summary>
        /// <param name="node">The node to duplicate. May not be <c>null</c>.</param>
        /// <param name="deep">true to recursively clone the subtree under the specified node, false to clone only the node itself.</param>
        public void CopyFrom(HtmlNode node, bool deep) {
            if (node == null) {
                throw new ArgumentNullException("node");
            }

            this.Attributes.RemoveAll();
            if (node.HasAttributes) {
                foreach (HtmlAttribute att in node.Attributes) {
                    this.SetAttributeValue(att.Name, att.Value);
                }
            }

            if (!deep) {
                this.RemoveAllChildren();
                if (node.HasChildNodes) {
                    foreach (HtmlNode child in node.ChildNodes) {
                        this.AppendChild(child.CloneNode(true));
                    }
                }
            }
        }


        /// <summary>
        /// Gets all Descendant nodes for this node and each of child nodes
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use Descendants() instead, the results of this function will change in a future version")]
        public IEnumerable<HtmlNode> DescendantNodes() {
            foreach (HtmlNode node in this.ChildNodes) {
                yield return node;
                foreach (HtmlNode descendant in node.DescendantNodes()) {
                    yield return descendant;
                }
            }
        }

        /// <summary>
        /// Returns a collection of all descendant nodes of this element, in document order
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use DescendantsAndSelf() instead, the results of this function will change in a future version")]
        public IEnumerable<HtmlNode> DescendantNodesAndSelf() {
            return this.DescendantsAndSelf();
        }

        /// <summary>
        /// Gets all Descendant nodes in enumerated list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HtmlNode> Descendants() {
            foreach (HtmlNode node in this.ChildNodes) {
                yield return node;
                foreach (HtmlNode descendant in node.Descendants()) {
                    yield return descendant;
                }
            }
        }

        /// <summary>
        /// Get all descendant nodes with matching name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<HtmlNode> Descendants(string name) {
            name = name.ToLowerInvariant();
            foreach (HtmlNode node in this.Descendants()) {
                if (node.Name.Equals(name)) {
                    yield return node;
                }
            }
        }

        /// <summary>
        /// Returns a collection of all descendant nodes of this element, in document order
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HtmlNode> DescendantsAndSelf() {
            yield return this;
            foreach (HtmlNode n in this.Descendants()) {
                HtmlNode el = n;
                if (el != null) {
                    yield return el;
                }
            }
        }

        /// <summary>
        /// Gets all descendant nodes including this node
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<HtmlNode> DescendantsAndSelf(string name) {
            yield return this;
            foreach (HtmlNode node in this.Descendants()) {
                if (node.Name == name) {
                    yield return node;
                }
            }
        }

        /// <summary>
        /// Gets first generation child node matching name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HtmlNode Element(string name) {
            foreach (HtmlNode node in this.ChildNodes) {
                if (node.Name == name) {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets matching first generation child nodes matching name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<HtmlNode> Elements(string name) {
            foreach (HtmlNode node in this.ChildNodes) {
                if (node.Name == name) {
                    yield return node;
                }
            }
        }

        /// <summary>
        /// Helper method to get the value of an attribute of this node. If the attribute is not found, the default value will be returned.
        /// </summary>
        /// <param name="name">The name of the attribute to get. May not be <c>null</c>.</param>
        /// <param name="def">The default value to return if not found.</param>
        /// <returns>The value of the attribute if found, the default value if not found.</returns>
        public string GetAttributeValue(string name, string def) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (!this.HasAttributes) {
                return def;
            }

            HtmlAttribute att = this.Attributes[name];
            if (att == null) {
                return def;
            }

            return att.Value;
        }

        /// <summary>
        /// Helper method to get the value of an attribute of this node. If the attribute is not found, the default value will be returned.
        /// </summary>
        /// <param name="name">The name of the attribute to get. May not be <c>null</c>.</param>
        /// <param name="def">The default value to return if not found.</param>
        /// <returns>The value of the attribute if found, the default value if not found.</returns>
        public int GetAttributeValue(string name, int def) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (!this.HasAttributes) {
                return def;
            }

            HtmlAttribute att = this.Attributes[name];
            if (att == null) {
                return def;
            }

            try {
                return Convert.ToInt32(att.Value);
            }
            catch {
                return def;
            }
        }

        /// <summary>
        /// Helper method to get the value of an attribute of this node. If the attribute is not found, the default value will be returned.
        /// </summary>
        /// <param name="name">The name of the attribute to get. May not be <c>null</c>.</param>
        /// <param name="def">The default value to return if not found.</param>
        /// <returns>The value of the attribute if found, the default value if not found.</returns>
        public bool GetAttributeValue(string name, bool def) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (!this.HasAttributes) {
                return def;
            }

            HtmlAttribute att = this.Attributes[name];
            if (att == null) {
                return def;
            }

            try {
                return Convert.ToBoolean(att.Value);
            }
            catch {
                return def;
            }
        }

        /// <summary>
        /// Inserts the specified node immediately after the specified reference node.
        /// </summary>
        /// <param name="newChild">The node to insert. May not be <c>null</c>.</param>
        /// <param name="refChild">The node that is the reference node. The newNode is placed after the refNode.</param>
        /// <returns>The node being inserted.</returns>
        public HtmlNode InsertAfter(HtmlNode newChild, HtmlNode refChild) {
            if (newChild == null) {
                throw new ArgumentNullException("newChild");
            }

            if (refChild == null) {
                return this.PrependChild(newChild);
            }

            if (newChild == refChild) {
                return newChild;
            }

            int index = -1;

            if (this._childnodes != null) {
                index = this._childnodes[refChild];
            }

            if (index == -1) {
                throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
            }

            if (this._childnodes != null) {
                this._childnodes.Insert(index + 1, newChild);
            }

            this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
            this.SetChanged();
            return newChild;
        }

        /// <summary>
        /// Inserts the specified node immediately before the specified reference node.
        /// </summary>
        /// <param name="newChild">The node to insert. May not be <c>null</c>.</param>
        /// <param name="refChild">The node that is the reference node. The newChild is placed before this node.</param>
        /// <returns>The node being inserted.</returns>
        public HtmlNode InsertBefore(HtmlNode newChild, HtmlNode refChild) {
            if (newChild == null) {
                throw new ArgumentNullException("newChild");
            }

            if (refChild == null) {
                return this.AppendChild(newChild);
            }

            if (newChild == refChild) {
                return newChild;
            }

            int index = -1;

            if (this._childnodes != null) {
                index = this._childnodes[refChild];
            }

            if (index == -1) {
                throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
            }

            if (this._childnodes != null) {
                this._childnodes.Insert(index, newChild);
            }

            this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
            this.SetChanged();
            return newChild;
        }

        /// <summary>
        /// Adds the specified node to the beginning of the list of children of this node.
        /// </summary>
        /// <param name="newChild">The node to add. May not be <c>null</c>.</param>
        /// <returns>The node added.</returns>
        public HtmlNode PrependChild(HtmlNode newChild) {
            if (newChild == null) {
                throw new ArgumentNullException("newChild");
            }

            this.ChildNodes.Prepend(newChild);
            this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
            this.SetChanged();
            return newChild;
        }

        /// <summary>
        /// Adds the specified node list to the beginning of the list of children of this node.
        /// </summary>
        /// <param name="newChildren">The node list to add. May not be <c>null</c>.</param>
        public void PrependChildren(HtmlNodeCollection newChildren) {
            if (newChildren == null) {
                throw new ArgumentNullException("newChildren");
            }

            foreach (HtmlNode newChild in newChildren) {
                this.PrependChild(newChild);
            }
        }

        /// <summary>
        /// Removes node from parent collection
        /// </summary>
        public void Remove() {
            if (this.ParentNode != null) {
                this.ParentNode.ChildNodes.Remove(this);
            }
        }

        /// <summary>
        /// Removes all the children and/or attributes of the current node.
        /// </summary>
        public void RemoveAll() {
            this.RemoveAllChildren();

            if (this.HasAttributes) {
                this._attributes.Clear();
            }

            if ((this._endnode != null) && (this._endnode != this)) {
                if (this._endnode._attributes != null) {
                    this._endnode._attributes.Clear();
                }
            }

            this.SetChanged();
        }

        /// <summary>
        /// Removes all the children of the current node.
        /// </summary>
        public void RemoveAllChildren() {
            if (!this.HasChildNodes) {
                return;
            }

            if (this._ownerdocument.OptionUseIdAttribute) {
                // remove nodes from id list
                foreach (HtmlNode node in this._childnodes) {
                    this._ownerdocument.SetIdForNode(null, node.GetId());
                }
            }

            this._childnodes.Clear();
            this.SetChanged();
        }

        /// <summary>
        /// Removes the specified child node.
        /// </summary>
        /// <param name="oldChild">The node being removed. May not be <c>null</c>.</param>
        /// <returns>The node removed.</returns>
        public HtmlNode RemoveChild(HtmlNode oldChild) {
            if (oldChild == null) {
                throw new ArgumentNullException("oldChild");
            }

            int index = -1;

            if (this._childnodes != null) {
                index = this._childnodes[oldChild];
            }

            if (index == -1) {
                throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
            }

            if (this._childnodes != null) {
                this._childnodes.Remove(index);
            }

            this._ownerdocument.SetIdForNode(null, oldChild.GetId());
            this.SetChanged();
            return oldChild;
        }

        /// <summary>
        /// Removes the specified child node.
        /// </summary>
        /// <param name="oldChild">The node being removed. May not be <c>null</c>.</param>
        /// <param name="keepGrandChildren">true to keep grand children of the node, false otherwise.</param>
        /// <returns>The node removed.</returns>
        public HtmlNode RemoveChild(HtmlNode oldChild, bool keepGrandChildren) {
            if (oldChild == null) {
                throw new ArgumentNullException("oldChild");
            }

            if ((oldChild._childnodes != null) && keepGrandChildren) {
                // get prev sibling
                HtmlNode prev = oldChild.PreviousSibling;

                // reroute grand children to ourselves
                foreach (HtmlNode grandchild in oldChild._childnodes) {
                    this.InsertAfter(grandchild, prev);
                }
            }

            this.RemoveChild(oldChild);
            this.SetChanged();
            return oldChild;
        }

        /// <summary>
        /// Replaces the child node oldChild with newChild node.
        /// </summary>
        /// <param name="newChild">The new node to put in the child list.</param>
        /// <param name="oldChild">The node being replaced in the list.</param>
        /// <returns>The node replaced.</returns>
        public HtmlNode ReplaceChild(HtmlNode newChild, HtmlNode oldChild) {
            if (newChild == null) {
                return this.RemoveChild(oldChild);
            }

            if (oldChild == null) {
                return this.AppendChild(newChild);
            }

            int index = -1;

            if (this._childnodes != null) {
                index = this._childnodes[oldChild];
            }

            if (index == -1) {
                throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
            }

            if (this._childnodes != null) {
                this._childnodes.Replace(index, newChild);
            }

            this._ownerdocument.SetIdForNode(null, oldChild.GetId());
            this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
            this.SetChanged();
            return newChild;
        }

        /// <summary>
        /// Helper method to set the value of an attribute of this node. If the attribute is not found, it will be created automatically.
        /// </summary>
        /// <param name="name">The name of the attribute to set. May not be null.</param>
        /// <param name="value">The value for the attribute.</param>
        /// <returns>The corresponding attribute instance.</returns>
        public HtmlAttribute SetAttributeValue(string name, string value) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            HtmlAttribute att = this.Attributes[name];
            if (att == null) {
                return this.Attributes.Append(this._ownerdocument.CreateAttribute(name, value));
            }

            att.Value = value;
            return att;
        }

        /// <summary>
        /// Saves all the children of the node to the specified TextWriter.
        /// </summary>
        /// <param name="outText">The TextWriter to which you want to save.</param>
        public void WriteContentTo(TextWriter outText) {
            if (this._childnodes == null) {
                return;
            }

            foreach (HtmlNode node in this._childnodes) {
                node.WriteTo(outText);
            }
        }

        /// <summary>
        /// Saves all the children of the node to a string.
        /// </summary>
        /// <returns>The saved string.</returns>
        public string WriteContentTo() {
            StringWriter sw = new StringWriter();
            this.WriteContentTo(sw);
            sw.Flush();
            return sw.ToString();
        }

        /// <summary>
        /// Saves the current node to the specified TextWriter.
        /// </summary>
        /// <param name="outText">The TextWriter to which you want to save.</param>
        public void WriteTo(TextWriter outText) {
            string html;
            switch (this._nodetype) {
                case HtmlNodeType.Comment:
                    html = ((HtmlCommentNode) this).Comment;
                    if (this._ownerdocument.OptionOutputAsXml) {
                        outText.Write("<!--" + GetXmlComment((HtmlCommentNode) this) + " -->");
                    }
                    else {
                        outText.Write(html);
                    }

                    break;

                case HtmlNodeType.Document:
                    if (this._ownerdocument.OptionOutputAsXml) {
#if NETSTANDARD1_4
                        outText.Write("<?xml version=\"1.0\" encoding=\"" + _ownerdocument.GetOutEncoding().WebName +
									 "\"?>");
#else
                        outText.Write("<?xml version=\"1.0\" encoding=\"" +
                                      this._ownerdocument.GetOutEncoding().BodyName +
                                      "\"?>");
#endif
                        // check there is a root element
                        if (this._ownerdocument.DocumentNode.HasChildNodes) {
                            int rootnodes = this._ownerdocument.DocumentNode._childnodes.Count;
                            if (rootnodes > 0) {
                                HtmlNode xml = this._ownerdocument.GetXmlDeclaration();
                                if (xml != null) {
                                    rootnodes--;
                                }

                                if (rootnodes > 1) {
                                    if (this._ownerdocument.OptionOutputUpperCase) {
                                        outText.Write("<SPAN>");
                                        this.WriteContentTo(outText);
                                        outText.Write("</SPAN>");
                                    }
                                    else {
                                        outText.Write("<span>");
                                        this.WriteContentTo(outText);
                                        outText.Write("</span>");
                                    }

                                    break;
                                }
                            }
                        }
                    }

                    this.WriteContentTo(outText);
                    break;

                case HtmlNodeType.Text:
                    html = ((HtmlTextNode) this).Text;
                    outText.Write(this._ownerdocument.OptionOutputAsXml ? HtmlDocument.HtmlEncode(html) : html);
                    break;

                case HtmlNodeType.Element:
                    string name = this._ownerdocument.OptionOutputUpperCase ? this.Name.ToUpper() : this.Name;

                    if (this._ownerdocument.OptionOutputOriginalCase) {
                        name = this.OriginalName;
                    }

                    if (this._ownerdocument.OptionOutputAsXml) {
                        if (name.Length > 0) {
                            if (name[0] == '?')
                                // forget this one, it's been done at the document level
                            {
                                break;
                            }

                            if (name.Trim().Length == 0) {
                                break;
                            }

                            name = HtmlDocument.GetXmlName(name);
                        }
                        else {
                            break;
                        }
                    }

                    outText.Write("<" + name);
                    this.WriteAttributes(outText, false);

                    if (this.HasChildNodes) {
                        outText.Write(">");
                        bool cdata = false;
                        if (this._ownerdocument.OptionOutputAsXml && IsCDataElement(this.Name)) {
                            // this code and the following tries to output things as nicely as possible for old browsers.
                            cdata = true;
                            outText.Write("\r\n//<![CDATA[\r\n");
                        }


                        if (cdata) {
                            if (this.HasChildNodes)
                                // child must be a text
                            {
                                this.ChildNodes[0].WriteTo(outText);
                            }

                            outText.Write("\r\n//]]>//\r\n");
                        }
                        else {
                            this.WriteContentTo(outText);
                        }

                        outText.Write("</" + name);
                        if (!this._ownerdocument.OptionOutputAsXml) {
                            this.WriteAttributes(outText, true);
                        }

                        outText.Write(">");
                    }
                    else {
                        if (IsEmptyElement(this.Name)) {
                            if ((this._ownerdocument.OptionWriteEmptyNodes) ||
                                (this._ownerdocument.OptionOutputAsXml)) {
                                outText.Write(" />");
                            }
                            else {
                                if (this.Name.Length > 0 && this.Name[0] == '?') {
                                    outText.Write("?");
                                }

                                outText.Write(">");
                            }
                        }
                        else {
                            outText.Write("></" + name + ">");
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Saves the current node to the specified XmlWriter.
        /// </summary>
        /// <param name="writer">The XmlWriter to which you want to save.</param>
        public void WriteTo(XmlWriter writer) {
            switch (this._nodetype) {
                case HtmlNodeType.Comment:
                    writer.WriteComment(GetXmlComment((HtmlCommentNode) this));
                    break;

                case HtmlNodeType.Document:
#if NETSTANDARD1_4
                    writer.WriteProcessingInstruction("xml",
													  "version=\"1.0\" encoding=\"" +
													  _ownerdocument.GetOutEncoding().WebName + "\"");
#else
                    writer.WriteProcessingInstruction("xml",
                        "version=\"1.0\" encoding=\"" + this._ownerdocument.GetOutEncoding().BodyName + "\"");
#endif

                    if (this.HasChildNodes) {
                        foreach (HtmlNode subnode in this.ChildNodes) {
                            subnode.WriteTo(writer);
                        }
                    }

                    break;

                case HtmlNodeType.Text:
                    string html = ((HtmlTextNode) this).Text;
                    writer.WriteString(html);
                    break;

                case HtmlNodeType.Element:
                    string name = this._ownerdocument.OptionOutputUpperCase ? this.Name.ToUpper() : this.Name;

                    if (this._ownerdocument.OptionOutputOriginalCase) {
                        name = this.OriginalName;
                    }

                    writer.WriteStartElement(name);
                    WriteAttributes(writer, this);

                    if (this.HasChildNodes) {
                        foreach (HtmlNode subnode in this.ChildNodes) {
                            subnode.WriteTo(writer);
                        }
                    }

                    writer.WriteEndElement();
                    break;
            }
        }

        /// <summary>
        /// Saves the current node to a string.
        /// </summary>
        /// <returns>The saved string.</returns>
        public string WriteTo() {
            using (StringWriter sw = new StringWriter()) {
                this.WriteTo(sw);
                sw.Flush();
                return sw.ToString();
            }
        }

        #endregion

        #region Internal Methods

        internal void SetChanged() {
            this._changed = true;
            if (this.ParentNode != null) {
                this.ParentNode.SetChanged();
            }
        }

        void UpdateHtml() {
            this._innerhtml = this.WriteContentTo();
            this._outerhtml = this.WriteTo();
            this._changed = false;
        }

        internal static string GetXmlComment(HtmlCommentNode comment) {
            string s = comment.Comment;
            return s.Substring(4, s.Length - 7).Replace("--", " - -");
        }

        internal static void WriteAttributes(XmlWriter writer, HtmlNode node) {
            if (!node.HasAttributes) {
                return;
            }

            // we use Hashitems to make sure attributes are written only once
            foreach (HtmlAttribute att in node.Attributes.Hashitems.Values) {
                writer.WriteAttributeString(att.XmlName, att.Value);
            }
        }

        internal void CloseNode(HtmlNode endnode) {
            if (!this._ownerdocument.OptionAutoCloseOnEnd) {
                // close all children
                if (this._childnodes != null) {
                    foreach (HtmlNode child in this._childnodes) {
                        if (child.Closed) {
                            continue;
                        }

                        // create a fake closer node
                        HtmlNode close = new HtmlNode(this.NodeType, this._ownerdocument, -1);
                        close._endnode = close;
                        child.CloseNode(close);
                    }
                }
            }

            if (!this.Closed) {
                this._endnode = endnode;

                if (this._ownerdocument.Openednodes != null) {
                    this._ownerdocument.Openednodes.Remove(this._outerstartindex);
                }

                HtmlNode self = Utilities.GetDictionaryValueOrNull(this._ownerdocument.Lastnodes, this.Name);
                if (self == this) {
                    this._ownerdocument.Lastnodes.Remove(this.Name);
                    this._ownerdocument.UpdateLastParentNode();
                }

                if (endnode == this) {
                    return;
                }

                // create an inner section
                this._innerstartindex = this._outerstartindex + this._outerlength;
                this._innerlength = endnode._outerstartindex - this._innerstartindex;

                // update full length
                this._outerlength = (endnode._outerstartindex + endnode._outerlength) - this._outerstartindex;
            }
        }

        internal string GetId() {
            HtmlAttribute att = this.Attributes["id"];
            return att == null ? string.Empty : att.Value;
        }

        internal void SetId(string id) {
            HtmlAttribute att = this.Attributes["id"] ?? this._ownerdocument.CreateAttribute("id");
            att.Value = id;
            this._ownerdocument.SetIdForNode(this, att.Value);
            this.SetChanged();
        }

        internal void WriteAttribute(TextWriter outText, HtmlAttribute att) {
            string name;
            string quote = att.QuoteType == AttributeValueQuote.DoubleQuote ? "\"" : "'";
            if (this._ownerdocument.OptionOutputAsXml) {
                name = this._ownerdocument.OptionOutputUpperCase ? att.XmlName.ToUpper() : att.XmlName;
                if (this._ownerdocument.OptionOutputOriginalCase) {
                    name = att.OriginalName;
                }

                outText.Write(" " + name + "=" + quote + HtmlDocument.HtmlEncode(att.XmlValue) + quote);
            }
            else {
                name = this._ownerdocument.OptionOutputUpperCase ? att.Name.ToUpper() : att.Name;
                if (this._ownerdocument.OptionOutputOriginalCase) {
                    name = att.OriginalName;
                }

                if (att.Name.Length >= 4) {
                    if ((att.Name[0] == '<') && (att.Name[1] == '%') &&
                        (att.Name[att.Name.Length - 1] == '>') && (att.Name[att.Name.Length - 2] == '%')) {
                        outText.Write(" " + name);
                        return;
                    }
                }

                if (this._ownerdocument.OptionOutputOptimizeAttributeValues) {
                    if (att.Value.IndexOfAny(new char[] {(char) 10, (char) 13, (char) 9, ' '}) < 0) {
                        outText.Write(" " + name + "=" + att.Value);
                    }
                    else {
                        outText.Write(" " + name + "=" + quote + att.Value + quote);
                    }
                }
                else {
                    outText.Write(" " + name + "=" + quote + att.Value + quote);
                }
            }
        }

        internal void WriteAttributes(TextWriter outText, bool closing) {
            if (this._ownerdocument.OptionOutputAsXml) {
                if (this._attributes == null) {
                    return;
                }

                // we use Hashitems to make sure attributes are written only once
                foreach (HtmlAttribute att in this._attributes.Hashitems.Values) {
                    this.WriteAttribute(outText, att);
                }

                return;
            }

            if (!closing) {
                if (this._attributes != null) {
                    foreach (HtmlAttribute att in this._attributes) {
                        this.WriteAttribute(outText, att);
                    }
                }

                if (!this._ownerdocument.OptionAddDebuggingAttributes) {
                    return;
                }

                this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_closed", this.Closed.ToString()));
                this.WriteAttribute(outText,
                    this._ownerdocument.CreateAttribute("_children", this.ChildNodes.Count.ToString()));

                int i = 0;
                foreach (HtmlNode n in this.ChildNodes) {
                    this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_child_" + i,
                        n.Name));
                    i++;
                }
            }
            else {
                if (this._endnode == null || this._endnode._attributes == null || this._endnode == this) {
                    return;
                }

                foreach (HtmlAttribute att in this._endnode._attributes) {
                    this.WriteAttribute(outText, att);
                }

                if (!this._ownerdocument.OptionAddDebuggingAttributes) {
                    return;
                }

                this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_closed", this.Closed.ToString()));
                this.WriteAttribute(outText,
                    this._ownerdocument.CreateAttribute("_children", this.ChildNodes.Count.ToString()));
            }
        }

        #endregion

        #region Private Methods

        string GetRelativeXpath() {
            if (this.ParentNode == null) {
                return this.Name;
            }

            if (this.NodeType == HtmlNodeType.Document) {
                return string.Empty;
            }

            int i = 1;
            foreach (HtmlNode node in this.ParentNode.ChildNodes) {
                if (node.Name != this.Name) {
                    continue;
                }

                if (node == this) {
                    break;
                }

                i++;
            }

            return this.Name + "[" + i + "]";
        }

        #endregion
    }
}