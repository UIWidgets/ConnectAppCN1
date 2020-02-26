// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

#region

using System;
using System.Diagnostics;

#endregion

// ReSharper disable InconsistentNaming

namespace HtmlAgilityPack {
    /// <summary>
    /// Represents an HTML attribute.
    /// </summary>
    [DebuggerDisplay("Name: {OriginalName}, Value: {Value}")]
    public class HtmlAttribute : IComparable {
        #region Fields

        int _line;
        internal int _lineposition;
        internal string _name;
        internal int _namelength;
        internal int _namestartindex;
        internal HtmlDocument _ownerdocument; // attribute can exists without a node
        internal HtmlNode _ownernode;
        AttributeValueQuote _quoteType = AttributeValueQuote.DoubleQuote;
        internal int _streamposition;
        internal string _value;
        internal int _valuelength;
        internal int _valuestartindex;

        #endregion

        #region Constructors

        internal HtmlAttribute(HtmlDocument ownerdocument) {
            this._ownerdocument = ownerdocument;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the line number of this attribute in the document.
        /// </summary>
        public int Line {
            get { return this._line; }
            internal set { this._line = value; }
        }

        /// <summary>
        /// Gets the column number of this attribute in the document.
        /// </summary>
        public int LinePosition {
            get { return this._lineposition; }
        }

        /// <summary>
        /// Gets the qualified name of the attribute.
        /// </summary>
        public string Name {
            get {
                if (this._name == null) {
                    this._name = this._ownerdocument.Text.Substring(this._namestartindex, this._namelength);
                }

                return this._name.ToLower();
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }

                this._name = value;
                if (this._ownernode != null) {
                    this._ownernode.SetChanged();
                }
            }
        }

        /// <summary>
        /// Name of attribute with original case
        /// </summary>
        public string OriginalName {
            get { return this._name; }
        }

        /// <summary>
        /// Gets the HTML document to which this attribute belongs.
        /// </summary>
        public HtmlDocument OwnerDocument {
            get { return this._ownerdocument; }
        }

        /// <summary>
        /// Gets the HTML node to which this attribute belongs.
        /// </summary>
        public HtmlNode OwnerNode {
            get { return this._ownernode; }
        }

        /// <summary>
        /// Specifies what type of quote the data should be wrapped in
        /// </summary>
        public AttributeValueQuote QuoteType {
            get { return this._quoteType; }
            set { this._quoteType = value; }
        }

        /// <summary>
        /// Gets the stream position of this attribute in the document, relative to the start of the document.
        /// </summary>
        public int StreamPosition {
            get { return this._streamposition; }
        }

        /// <summary>
        /// Gets or sets the value of the attribute.
        /// </summary>
        public string Value {
            get {
                if (this._value == null) {
                    this._value = this._ownerdocument.Text.Substring(this._valuestartindex, this._valuelength);
                }

                return this._value;
            }
            set {
                this._value = value;
                if (this._ownernode != null) {
                    this._ownernode.SetChanged();
                }
            }
        }

        internal string XmlName {
            get { return HtmlDocument.GetXmlName(this.Name); }
        }

        internal string XmlValue {
            get { return this.Value; }
        }

        /// <summary>
        /// Gets a valid XPath string that points to this Attribute
        /// </summary>
        public string XPath {
            get {
                string basePath = (this.OwnerNode == null) ? "/" : this.OwnerNode.XPath + "/";
                return basePath + this.GetRelativeXpath();
            }
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares the current instance with another attribute. Comparison is based on attributes' name.
        /// </summary>
        /// <param name="obj">An attribute to compare with this instance.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the names comparison.</returns>
        public int CompareTo(object obj) {
            HtmlAttribute att = obj as HtmlAttribute;
            if (att == null) {
                throw new ArgumentException("obj");
            }

            return this.Name.CompareTo(att.Name);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a duplicate of this attribute.
        /// </summary>
        /// <returns>The cloned attribute.</returns>
        public HtmlAttribute Clone() {
            HtmlAttribute att = new HtmlAttribute(this._ownerdocument);
            att.Name = this.Name;
            att.Value = this.Value;
            return att;
        }

        /// <summary>
        /// Removes this attribute from it's parents collection
        /// </summary>
        public void Remove() {
            this._ownernode.Attributes.Remove(this);
        }

        #endregion

        #region Private Methods

        string GetRelativeXpath() {
            if (this.OwnerNode == null) {
                return this.Name;
            }

            int i = 1;
            foreach (HtmlAttribute node in this.OwnerNode.Attributes) {
                if (node.Name != this.Name) {
                    continue;
                }

                if (node == this) {
                    break;
                }

                i++;
            }

            return "@" + this.Name + "[" + i + "]";
        }

        #endregion
    }

    /// <summary>
    /// An Enum representing different types of Quotes used for surrounding attribute values
    /// </summary>
    public enum AttributeValueQuote {
        /// <summary>
        /// A single quote mark '
        /// </summary>
        SingleQuote,

        /// <summary>
        /// A double quote mark "
        /// </summary>
        DoubleQuote
    }
}