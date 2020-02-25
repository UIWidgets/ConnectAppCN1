// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

#pragma warning disable 0649
namespace HtmlAgilityPack {
    /// <summary>
    /// Represents an HTML navigator on an HTML document seen as a data store.
    /// </summary>
    public class HtmlNodeNavigator : XPathNavigator {
        #region Fields

        int _attindex;
        HtmlNode _currentnode;
        readonly HtmlDocument _doc = new HtmlDocument();
        readonly HtmlNameTable _nametable = new HtmlNameTable();

        internal bool Trace;

        #endregion

        #region Constructors

        internal HtmlNodeNavigator() {
            this.Reset();
        }

        internal HtmlNodeNavigator(HtmlDocument doc, HtmlNode currentNode) {
            if (currentNode == null) {
                throw new ArgumentNullException("currentNode");
            }

            if (currentNode.OwnerDocument != doc) {
                throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
            }

            this.InternalTrace(null);

            this._doc = doc;
            this.Reset();
            this._currentnode = currentNode;
        }

        HtmlNodeNavigator(HtmlNodeNavigator nav) {
            if (nav == null) {
                throw new ArgumentNullException("nav");
            }

            this.InternalTrace(null);

            this._doc = nav._doc;
            this._currentnode = nav._currentnode;
            this._attindex = nav._attindex;
            this._nametable = nav._nametable; // REVIEW: should we do this?
        }

        /// <summary>
        /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        public HtmlNodeNavigator(Stream stream) {
            this._doc.Load(stream);
            this.Reset();
        }

        /// <summary>
        /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
        public HtmlNodeNavigator(Stream stream, bool detectEncodingFromByteOrderMarks) {
            this._doc.Load(stream, detectEncodingFromByteOrderMarks);
            this.Reset();
        }

        /// <summary>
        /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public HtmlNodeNavigator(Stream stream, Encoding encoding) {
            this._doc.Load(stream, encoding);
            this.Reset();
        }

        /// <summary>
        /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
        public HtmlNodeNavigator(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks) {
            this._doc.Load(stream, encoding, detectEncodingFromByteOrderMarks);
            this.Reset();
        }

        /// <summary>
        /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
        /// <param name="buffersize">The minimum buffer size.</param>
        public HtmlNodeNavigator(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks,
            int buffersize) {
            this._doc.Load(stream, encoding, detectEncodingFromByteOrderMarks, buffersize);
            this.Reset();
        }

        /// <summary>
        /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a TextReader.
        /// </summary>
        /// <param name="reader">The TextReader used to feed the HTML data into the document.</param>
        public HtmlNodeNavigator(TextReader reader) {
            this._doc.Load(reader);
            this.Reset();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the base URI for the current node.
        /// Always returns string.Empty in the case of HtmlNavigator implementation.
        /// </summary>
        public override string BaseURI {
            get {
                this.InternalTrace(">");
                return this._nametable.GetOrAdd(string.Empty);
            }
        }

        /// <summary>
        /// Gets the current HTML document.
        /// </summary>
        public HtmlDocument CurrentDocument {
            get { return this._doc; }
        }

        /// <summary>
        /// Gets the current HTML node.
        /// </summary>
        public HtmlNode CurrentNode {
            get { return this._currentnode; }
        }

        /// <summary>
        /// Gets a value indicating whether the current node has child nodes.
        /// </summary>
        public override bool HasAttributes {
            get {
                this.InternalTrace(">" + (this._currentnode.Attributes.Count > 0));
                return (this._currentnode.Attributes.Count > 0);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current node has child nodes.
        /// </summary>
        public override bool HasChildren {
            get {
                this.InternalTrace(">" + (this._currentnode.ChildNodes.Count > 0));
                return (this._currentnode.ChildNodes.Count > 0);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current node is an empty element.
        /// </summary>
        public override bool IsEmptyElement {
            get {
                this.InternalTrace(">" + !this.HasChildren);
                // REVIEW: is this ok?
                return !this.HasChildren;
            }
        }

        /// <summary>
        /// Gets the name of the current HTML node without the namespace prefix.
        /// </summary>
        public override string LocalName {
            get {
                if (this._attindex != -1) {
                    this.InternalTrace("att>" + this._currentnode.Attributes[this._attindex].Name);
                    return this._nametable.GetOrAdd(this._currentnode.Attributes[this._attindex].Name);
                }

                this.InternalTrace("node>" + this._currentnode.Name);
                return this._nametable.GetOrAdd(this._currentnode.Name);
            }
        }

        /// <summary>
        /// Gets the qualified name of the current node.
        /// </summary>
        public override string Name {
            get {
                this.InternalTrace(">" + this._currentnode.Name);
                return this._nametable.GetOrAdd(this._currentnode.Name);
            }
        }

        /// <summary>
        /// Gets the namespace URI (as defined in the W3C Namespace Specification) of the current node.
        /// Always returns string.Empty in the case of HtmlNavigator implementation.
        /// </summary>
        public override string NamespaceURI {
            get {
                this.InternalTrace(">");
                return this._nametable.GetOrAdd(string.Empty);
            }
        }

        /// <summary>
        /// Gets the <see cref="XmlNameTable"/> associated with this implementation.
        /// </summary>
        public override XmlNameTable NameTable {
            get {
                this.InternalTrace(null);
                return this._nametable;
            }
        }

        /// <summary>
        /// Gets the type of the current node.
        /// </summary>
        public override XPathNodeType NodeType {
            get {
                switch (this._currentnode.NodeType) {
                    case HtmlNodeType.Comment:
                        this.InternalTrace(">" + XPathNodeType.Comment);
                        return XPathNodeType.Comment;

                    case HtmlNodeType.Document:
                        this.InternalTrace(">" + XPathNodeType.Root);
                        return XPathNodeType.Root;

                    case HtmlNodeType.Text:
                        this.InternalTrace(">" + XPathNodeType.Text);
                        return XPathNodeType.Text;

                    case HtmlNodeType.Element: {
                        if (this._attindex != -1) {
                            this.InternalTrace(">" + XPathNodeType.Attribute);
                            return XPathNodeType.Attribute;
                        }

                        this.InternalTrace(">" + XPathNodeType.Element);
                        return XPathNodeType.Element;
                    }

                    default:
                        throw new NotImplementedException("Internal error: Unhandled HtmlNodeType: " +
                                                          this._currentnode.NodeType);
                }
            }
        }

        /// <summary>
        /// Gets the prefix associated with the current node.
        /// Always returns string.Empty in the case of HtmlNavigator implementation.
        /// </summary>
        public override string Prefix {
            get {
                this.InternalTrace(null);
                return this._nametable.GetOrAdd(string.Empty);
            }
        }

        /// <summary>
        /// Gets the text value of the current node.
        /// </summary>
        public override string Value {
            get {
                this.InternalTrace("nt=" + this._currentnode.NodeType);
                switch (this._currentnode.NodeType) {
                    case HtmlNodeType.Comment:
                        this.InternalTrace(">" + ((HtmlCommentNode) this._currentnode).Comment);
                        return ((HtmlCommentNode) this._currentnode).Comment;

                    case HtmlNodeType.Document:
                        this.InternalTrace(">");
                        return "";

                    case HtmlNodeType.Text:
                        this.InternalTrace(">" + ((HtmlTextNode) this._currentnode).Text);
                        return ((HtmlTextNode) this._currentnode).Text;

                    case HtmlNodeType.Element: {
                        if (this._attindex != -1) {
                            this.InternalTrace(">" + this._currentnode.Attributes[this._attindex].Value);
                            return this._currentnode.Attributes[this._attindex].Value;
                        }

                        return this._currentnode.InnerText;
                    }

                    default:
                        throw new NotImplementedException("Internal error: Unhandled HtmlNodeType: " +
                                                          this._currentnode.NodeType);
                }
            }
        }

        /// <summary>
        /// Gets the xml:lang scope for the current node.
        /// Always returns string.Empty in the case of HtmlNavigator implementation.
        /// </summary>
        public override string XmlLang {
            get {
                this.InternalTrace(null);
                return this._nametable.GetOrAdd(string.Empty);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new HtmlNavigator positioned at the same node as this HtmlNavigator.
        /// </summary>
        /// <returns>A new HtmlNavigator object positioned at the same node as the original HtmlNavigator.</returns>
        public override XPathNavigator Clone() {
            this.InternalTrace(null);
            return new HtmlNodeNavigator(this);
        }

        /// <summary>
        /// Gets the value of the HTML attribute with the specified LocalName and NamespaceURI.
        /// </summary>
        /// <param name="localName">The local name of the HTML attribute.</param>
        /// <param name="namespaceURI">The namespace URI of the attribute. Unsupported with the HtmlNavigator implementation.</param>
        /// <returns>The value of the specified HTML attribute. String.Empty or null if a matching attribute is not found or if the navigator is not positioned on an element node.</returns>
        public override string GetAttribute(string localName, string namespaceURI) {
            this.InternalTrace("localName=" + localName + ", namespaceURI=" + namespaceURI);
            HtmlAttribute att = this._currentnode.Attributes[localName];
            if (att == null) {
                this.InternalTrace(">null");
                return null;
            }

            this.InternalTrace(">" + att.Value);
            return att.Value;
        }

        /// <summary>
        /// Returns the value of the namespace node corresponding to the specified local name.
        /// Always returns string.Empty for the HtmlNavigator implementation.
        /// </summary>
        /// <param name="name">The local name of the namespace node.</param>
        /// <returns>Always returns string.Empty for the HtmlNavigator implementation.</returns>
        public override string GetNamespace(string name) {
            this.InternalTrace("name=" + name);
            return string.Empty;
        }

        /// <summary>
        /// Determines whether the current HtmlNavigator is at the same position as the specified HtmlNavigator.
        /// </summary>
        /// <param name="other">The HtmlNavigator that you want to compare against.</param>
        /// <returns>true if the two navigators have the same position, otherwise, false.</returns>
        public override bool IsSamePosition(XPathNavigator other) {
            HtmlNodeNavigator nav = other as HtmlNodeNavigator;
            if (nav == null) {
                this.InternalTrace(">false");
                return false;
            }

            this.InternalTrace(">" + (nav._currentnode == this._currentnode));
            return (nav._currentnode == this._currentnode);
        }

        /// <summary>
        /// Moves to the same position as the specified HtmlNavigator.
        /// </summary>
        /// <param name="other">The HtmlNavigator positioned on the node that you want to move to.</param>
        /// <returns>true if successful, otherwise false. If false, the position of the navigator is unchanged.</returns>
        public override bool MoveTo(XPathNavigator other) {
            HtmlNodeNavigator nav = other as HtmlNodeNavigator;
            if (nav == null) {
                this.InternalTrace(">false (nav is not an HtmlNodeNavigator)");
                return false;
            }

            this.InternalTrace("moveto oid=" + nav.GetHashCode()
                                             + ", n:" + nav._currentnode.Name
                                             + ", a:" + nav._attindex);

            if (nav._doc == this._doc) {
                this._currentnode = nav._currentnode;
                this._attindex = nav._attindex;
                this.InternalTrace(">true");
                return true;
            }

            // we don't know how to handle that
            this.InternalTrace(">false (???)");
            return false;
        }

        /// <summary>
        /// Moves to the HTML attribute with matching LocalName and NamespaceURI.
        /// </summary>
        /// <param name="localName">The local name of the HTML attribute.</param>
        /// <param name="namespaceURI">The namespace URI of the attribute. Unsupported with the HtmlNavigator implementation.</param>
        /// <returns>true if the HTML attribute is found, otherwise, false. If false, the position of the navigator does not change.</returns>
        public override bool MoveToAttribute(string localName, string namespaceURI) {
            this.InternalTrace("localName=" + localName + ", namespaceURI=" + namespaceURI);
            int index = this._currentnode.Attributes.GetAttributeIndex(localName);
            if (index == -1) {
                this.InternalTrace(">false");
                return false;
            }

            this._attindex = index;
            this.InternalTrace(">true");
            return true;
        }

        /// <summary>
        /// Moves to the first sibling of the current node.
        /// </summary>
        /// <returns>true if the navigator is successful moving to the first sibling node, false if there is no first sibling or if the navigator is currently positioned on an attribute node.</returns>
        public override bool MoveToFirst() {
            if (this._currentnode.ParentNode == null) {
                this.InternalTrace(">false");
                return false;
            }

            if (this._currentnode.ParentNode.FirstChild == null) {
                this.InternalTrace(">false");
                return false;
            }

            this._currentnode = this._currentnode.ParentNode.FirstChild;
            this.InternalTrace(">true");
            return true;
        }

        /// <summary>
        /// Moves to the first HTML attribute.
        /// </summary>
        /// <returns>true if the navigator is successful moving to the first HTML attribute, otherwise, false.</returns>
        public override bool MoveToFirstAttribute() {
            if (!this.HasAttributes) {
                this.InternalTrace(">false");
                return false;
            }

            this._attindex = 0;
            this.InternalTrace(">true");
            return true;
        }

        /// <summary>
        /// Moves to the first child of the current node.
        /// </summary>
        /// <returns>true if there is a first child node, otherwise false.</returns>
        public override bool MoveToFirstChild() {
            if (!this._currentnode.HasChildNodes) {
                this.InternalTrace(">false");
                return false;
            }

            this._currentnode = this._currentnode.ChildNodes[0];
            this.InternalTrace(">true");
            return true;
        }

        /// <summary>
        /// Moves the XPathNavigator to the first namespace node of the current element.
        /// Always returns false for the HtmlNavigator implementation.
        /// </summary>
        /// <param name="scope">An XPathNamespaceScope value describing the namespace scope.</param>
        /// <returns>Always returns false for the HtmlNavigator implementation.</returns>
        public override bool MoveToFirstNamespace(XPathNamespaceScope scope) {
            this.InternalTrace(null);
            return false;
        }

        /// <summary>
        /// Moves to the node that has an attribute of type ID whose value matches the specified string.
        /// </summary>
        /// <param name="id">A string representing the ID value of the node to which you want to move. This argument does not need to be atomized.</param>
        /// <returns>true if the move was successful, otherwise false. If false, the position of the navigator is unchanged.</returns>
        public override bool MoveToId(string id) {
            this.InternalTrace("id=" + id);
            HtmlNode node = this._doc.GetElementbyId(id);
            if (node == null) {
                this.InternalTrace(">false");
                return false;
            }

            this._currentnode = node;
            this.InternalTrace(">true");
            return true;
        }

        /// <summary>
        /// Moves the XPathNavigator to the namespace node with the specified local name. 
        /// Always returns false for the HtmlNavigator implementation.
        /// </summary>
        /// <param name="name">The local name of the namespace node.</param>
        /// <returns>Always returns false for the HtmlNavigator implementation.</returns>
        public override bool MoveToNamespace(string name) {
            this.InternalTrace("name=" + name);
            return false;
        }

        /// <summary>
        /// Moves to the next sibling of the current node.
        /// </summary>
        /// <returns>true if the navigator is successful moving to the next sibling node, false if there are no more siblings or if the navigator is currently positioned on an attribute node. If false, the position of the navigator is unchanged.</returns>
        public override bool MoveToNext() {
            if (this._currentnode.NextSibling == null) {
                this.InternalTrace(">false");
                return false;
            }

            this.InternalTrace("_c=" + this._currentnode.CloneNode(false).OuterHtml);
            this.InternalTrace("_n=" + this._currentnode.NextSibling.CloneNode(false).OuterHtml);
            this._currentnode = this._currentnode.NextSibling;
            this.InternalTrace(">true");
            return true;
        }

        /// <summary>
        /// Moves to the next HTML attribute.
        /// </summary>
        /// <returns></returns>
        public override bool MoveToNextAttribute() {
            this.InternalTrace(null);
            if (this._attindex >= (this._currentnode.Attributes.Count - 1)) {
                this.InternalTrace(">false");
                return false;
            }

            this._attindex++;
            this.InternalTrace(">true");
            return true;
        }

        /// <summary>
        /// Moves the XPathNavigator to the next namespace node.
        /// Always returns falsefor the HtmlNavigator implementation.
        /// </summary>
        /// <param name="scope">An XPathNamespaceScope value describing the namespace scope.</param>
        /// <returns>Always returns false for the HtmlNavigator implementation.</returns>
        public override bool MoveToNextNamespace(XPathNamespaceScope scope) {
            this.InternalTrace(null);
            return false;
        }

        /// <summary>
        /// Moves to the parent of the current node.
        /// </summary>
        /// <returns>true if there is a parent node, otherwise false.</returns>
        public override bool MoveToParent() {
            if (this._currentnode.ParentNode == null) {
                this.InternalTrace(">false");
                return false;
            }

            this._currentnode = this._currentnode.ParentNode;
            this.InternalTrace(">true");
            return true;
        }

        /// <summary>
        /// Moves to the previous sibling of the current node.
        /// </summary>
        /// <returns>true if the navigator is successful moving to the previous sibling node, false if there is no previous sibling or if the navigator is currently positioned on an attribute node.</returns>
        public override bool MoveToPrevious() {
            if (this._currentnode.PreviousSibling == null) {
                this.InternalTrace(">false");
                return false;
            }

            this._currentnode = this._currentnode.PreviousSibling;
            this.InternalTrace(">true");
            return true;
        }

        /// <summary>
        /// Moves to the root node to which the current node belongs.
        /// </summary>
        public override void MoveToRoot() {
            this._currentnode = this._doc.DocumentNode;
            this.InternalTrace(null);
        }

        #endregion

        #region Internal Methods

        [Conditional("TRACE")]
        internal void InternalTrace(object traceValue) {
            if (!this.Trace) {
                return;
            }

            string name = "";
            string nodename = this._currentnode == null ? "(null)" : this._currentnode.Name;
            string nodevalue;
            if (this._currentnode == null) {
                nodevalue = "(null)";
            }
            else {
                switch (this._currentnode.NodeType) {
                    case HtmlNodeType.Comment:
                        nodevalue = ((HtmlCommentNode) this._currentnode).Comment;
                        break;

                    case HtmlNodeType.Document:
                        nodevalue = "";
                        break;

                    case HtmlNodeType.Text:
                        nodevalue = ((HtmlTextNode) this._currentnode).Text;
                        break;

                    default:
                        nodevalue = this._currentnode.CloneNode(false).OuterHtml;
                        break;
                }
            }

            HtmlAgilityPack.Trace.WriteLine(
                string.Format("oid={0},n={1},a={2},v={3},{4}", this.GetHashCode(), nodename, this._attindex, nodevalue,
                    traceValue), "N!" + name);
        }

        #endregion

        #region Private Methods

        void Reset() {
            this.InternalTrace(null);
            this._currentnode = this._doc.DocumentNode;
            this._attindex = -1;
        }

        #endregion
    }
}