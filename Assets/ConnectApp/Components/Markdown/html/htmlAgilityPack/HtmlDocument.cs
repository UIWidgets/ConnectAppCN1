// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace HtmlAgilityPack {
    /// <summary>
    /// Represents a complete HTML document.
    /// </summary>
    public partial class HtmlDocument {
        #region Fields

        int _c;
        Crc32 _crc32;
        HtmlAttribute _currentattribute;
        HtmlNode _currentnode;
        Encoding _declaredencoding;
        HtmlNode _documentnode;
        bool _fullcomment;
        int _index;
        internal Dictionary<string, HtmlNode> Lastnodes = new Dictionary<string, HtmlNode>();
        HtmlNode _lastparentnode;
        int _line;
        int _lineposition, _maxlineposition;
        internal Dictionary<string, HtmlNode> Nodesid;
        ParseState _oldstate;
        bool _onlyDetectEncoding;
        internal Dictionary<int, HtmlNode> Openednodes;
        List<HtmlParseError> _parseerrors = new List<HtmlParseError>();
        string _remainder;
        int _remainderOffset;
        ParseState _state;
        Encoding _streamencoding;
        internal string Text;

        // public props

        /// <summary>
        /// Adds Debugging attributes to node. Default is false.
        /// </summary>
        public bool OptionAddDebuggingAttributes;

        /// <summary>
        /// Defines if closing for non closed nodes must be done at the end or directly in the document.
        /// Setting this to true can actually change how browsers render the page. Default is false.
        /// </summary>
        public bool OptionAutoCloseOnEnd; // close errors at the end

        /// <summary>
        /// Defines if non closed nodes will be checked at the end of parsing. Default is true.
        /// </summary>
        public bool OptionCheckSyntax = true;

        /// <summary>
        /// Defines if a checksum must be computed for the document while parsing. Default is false.
        /// </summary>
        public bool OptionComputeChecksum;


        /// <summary>
        /// Defines the default stream encoding to use. Default is System.Text.Encoding.Default.
        /// </summary>
        public Encoding OptionDefaultStreamEncoding;

        /// <summary>
        /// Defines if source text must be extracted while parsing errors.
        /// If the document has a lot of errors, or cascading errors, parsing performance can be dramatically affected if set to true.
        /// Default is false.
        /// </summary>
        public bool OptionExtractErrorSourceText;

        // turning this on can dramatically slow performance if a lot of errors are detected

        /// <summary>
        /// Defines the maximum length of source text or parse errors. Default is 100.
        /// </summary>
        public int OptionExtractErrorSourceTextMaxLength = 100;

        /// <summary>
        /// Defines if LI, TR, TH, TD tags must be partially fixed when nesting errors are detected. Default is false.
        /// </summary>
        public bool OptionFixNestedTags; // fix li, tr, th, td tags

        /// <summary>
        /// Defines if output must conform to XML, instead of HTML.
        /// </summary>
        public bool OptionOutputAsXml;

        /// <summary>
        /// Defines if attribute value output must be optimized (not bound with double quotes if it is possible). Default is false.
        /// </summary>
        public bool OptionOutputOptimizeAttributeValues;

        /// <summary>
        /// Defines if name must be output with it's original case. Useful for asp.net tags and attributes
        /// </summary>
        public bool OptionOutputOriginalCase;

        /// <summary>
        /// Defines if name must be output in uppercase. Default is false.
        /// </summary>
        public bool OptionOutputUpperCase;

        /// <summary>
        /// Defines if declared encoding must be read from the document.
        /// Declared encoding is determined using the meta http-equiv="content-type" content="text/html;charset=XXXXX" html node.
        /// Default is true.
        /// </summary>
        public bool OptionReadEncoding = true;

        /// <summary>
        /// Defines the name of a node that will throw the StopperNodeException when found as an end node. Default is null.
        /// </summary>
        public string OptionStopperNodeName;

        /// <summary>
        /// Defines if the 'id' attribute must be specifically used. Default is true.
        /// </summary>
        public bool OptionUseIdAttribute = true;

        /// <summary>
        /// Defines if empty nodes must be written as closed during output. Default is false.
        /// </summary>
        public bool OptionWriteEmptyNodes;

        #endregion

        #region Static Members

        internal static readonly string HtmlExceptionRefNotChild = "Reference node must be a child of this node";

        internal static readonly string HtmlExceptionUseIdAttributeFalse =
            "You need to set UseIdAttribute property to true to enable this feature";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of an HTML document.
        /// </summary>
        public HtmlDocument() {
            this._documentnode = this.CreateNode(HtmlNodeType.Document, 0);
#if NETSTANDARD1_4
            OptionDefaultStreamEncoding = Encoding.UTF8;
#else
            this.OptionDefaultStreamEncoding = Encoding.Default;
#endif
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the document CRC32 checksum if OptionComputeChecksum was set to true before parsing, 0 otherwise.
        /// </summary>
        public int CheckSum {
            get { return this._crc32 == null ? 0 : (int) this._crc32.CheckSum; }
        }

        /// <summary>
        /// Gets the document's declared encoding.
        /// Declared encoding is determined using the meta http-equiv="content-type" content="text/html;charset=XXXXX" html node.
        /// </summary>
        public Encoding DeclaredEncoding {
            get { return this._declaredencoding; }
        }

        /// <summary>
        /// Gets the root node of the document.
        /// </summary>
        public HtmlNode DocumentNode {
            get { return this._documentnode; }
        }

        /// <summary>
        /// Gets the document's output encoding.
        /// </summary>
        public Encoding Encoding {
            get { return this.GetOutEncoding(); }
        }

        /// <summary>
        /// Gets a list of parse errors found in the document.
        /// </summary>
        public IEnumerable<HtmlParseError> ParseErrors {
            get { return this._parseerrors; }
        }

        /// <summary>
        /// Gets the remaining text.
        /// Will always be null if OptionStopperNodeName is null.
        /// </summary>
        public string Remainder {
            get { return this._remainder; }
        }

        /// <summary>
        /// Gets the offset of Remainder in the original Html text.
        /// If OptionStopperNodeName is null, this will return the length of the original Html text.
        /// </summary>
        public int RemainderOffset {
            get { return this._remainderOffset; }
        }

        /// <summary>
        /// Gets the document's stream encoding.
        /// </summary>
        public Encoding StreamEncoding {
            get { return this._streamencoding; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a valid XML name.
        /// </summary>
        /// <param name="name">Any text.</param>
        /// <returns>A string that is a valid XML name.</returns>
        public static string GetXmlName(string name) {
            string xmlname = string.Empty;
            bool nameisok = true;
            for (int i = 0; i < name.Length; i++) {
                // names are lcase
                // note: we are very limited here, too much?
                if (((name[i] >= 'a') && (name[i] <= 'z')) ||
                    ((name[i] >= '0') && (name[i] <= '9')) ||
                    //					(name[i]==':') || (name[i]=='_') || (name[i]=='-') || (name[i]=='.')) // these are bads in fact
                    (name[i] == '_') || (name[i] == '-') || (name[i] == '.')) {
                    xmlname += name[i];
                }
                else {
                    nameisok = false;
                    byte[] bytes = Encoding.UTF8.GetBytes(new char[] {name[i]});
                    for (int j = 0; j < bytes.Length; j++) {
                        xmlname += bytes[j].ToString("x2");
                    }

                    xmlname += "_";
                }
            }

            if (nameisok) {
                return xmlname;
            }

            return "_" + xmlname;
        }

        /// <summary>
        /// Applies HTML encoding to a specified string.
        /// </summary>
        /// <param name="html">The input string to encode. May not be null.</param>
        /// <returns>The encoded string.</returns>
        public static string HtmlEncode(string html) {
            if (html == null) {
                throw new ArgumentNullException("html");
            }

            // replace & by &amp; but only once!
            Regex rx = new Regex("&(?!(amp;)|(lt;)|(gt;)|(quot;))", RegexOptions.IgnoreCase);
            return rx.Replace(html, "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }

        /// <summary>
        /// Determines if the specified character is considered as a whitespace character.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>true if if the specified character is considered as a whitespace character.</returns>
        public static bool IsWhiteSpace(int c) {
            if ((c == 10) || (c == 13) || (c == 32) || (c == 9)) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Creates an HTML attribute with the specified name.
        /// </summary>
        /// <param name="name">The name of the attribute. May not be null.</param>
        /// <returns>The new HTML attribute.</returns>
        public HtmlAttribute CreateAttribute(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            HtmlAttribute att = this.CreateAttribute();
            att.Name = name;
            return att;
        }

        /// <summary>
        /// Creates an HTML attribute with the specified name.
        /// </summary>
        /// <param name="name">The name of the attribute. May not be null.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <returns>The new HTML attribute.</returns>
        public HtmlAttribute CreateAttribute(string name, string value) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            HtmlAttribute att = this.CreateAttribute(name);
            att.Value = value;
            return att;
        }

        /// <summary>
        /// Creates an HTML comment node.
        /// </summary>
        /// <returns>The new HTML comment node.</returns>
        public HtmlCommentNode CreateComment() {
            return (HtmlCommentNode) this.CreateNode(HtmlNodeType.Comment);
        }

        /// <summary>
        /// Creates an HTML comment node with the specified comment text.
        /// </summary>
        /// <param name="comment">The comment text. May not be null.</param>
        /// <returns>The new HTML comment node.</returns>
        public HtmlCommentNode CreateComment(string comment) {
            if (comment == null) {
                throw new ArgumentNullException("comment");
            }

            HtmlCommentNode c = this.CreateComment();
            c.Comment = comment;
            return c;
        }

        /// <summary>
        /// Creates an HTML element node with the specified name.
        /// </summary>
        /// <param name="name">The qualified name of the element. May not be null.</param>
        /// <returns>The new HTML node.</returns>
        public HtmlNode CreateElement(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            HtmlNode node = this.CreateNode(HtmlNodeType.Element);
            node.Name = name;
            return node;
        }

        /// <summary>
        /// Creates an HTML text node.
        /// </summary>
        /// <returns>The new HTML text node.</returns>
        public HtmlTextNode CreateTextNode() {
            return (HtmlTextNode) this.CreateNode(HtmlNodeType.Text);
        }

        /// <summary>
        /// Creates an HTML text node with the specified text.
        /// </summary>
        /// <param name="text">The text of the node. May not be null.</param>
        /// <returns>The new HTML text node.</returns>
        public HtmlTextNode CreateTextNode(string text) {
            if (text == null) {
                throw new ArgumentNullException("text");
            }

            HtmlTextNode t = this.CreateTextNode();
            t.Text = text;
            return t;
        }

        /// <summary>
        /// Detects the encoding of an HTML stream.
        /// </summary>
        /// <param name="stream">The input stream. May not be null.</param>
        /// <returns>The detected encoding.</returns>
        public Encoding DetectEncoding(Stream stream) {
            if (stream == null) {
                throw new ArgumentNullException("stream");
            }

            return this.DetectEncoding(new StreamReader(stream));
        }


        /// <summary>
        /// Detects the encoding of an HTML text provided on a TextReader.
        /// </summary>
        /// <param name="reader">The TextReader used to feed the HTML. May not be null.</param>
        /// <returns>The detected encoding.</returns>
        public Encoding DetectEncoding(TextReader reader) {
            if (reader == null) {
                throw new ArgumentNullException("reader");
            }

            this._onlyDetectEncoding = true;
            if (this.OptionCheckSyntax) {
                this.Openednodes = new Dictionary<int, HtmlNode>();
            }
            else {
                this.Openednodes = null;
            }

            if (this.OptionUseIdAttribute) {
                this.Nodesid = new Dictionary<string, HtmlNode>();
            }
            else {
                this.Nodesid = null;
            }

            StreamReader sr = reader as StreamReader;
            if (sr != null) {
                this._streamencoding = sr.CurrentEncoding;
            }
            else {
                this._streamencoding = null;
            }

            this._declaredencoding = null;

            this.Text = reader.ReadToEnd();
            this._documentnode = this.CreateNode(HtmlNodeType.Document, 0);

            // this is almost a hack, but it allows us not to muck with the original parsing code
            try {
                this.Parse();
            }
            catch (EncodingFoundException ex) {
                return ex.Encoding;
            }

            return null;
        }


        /// <summary>
        /// Detects the encoding of an HTML text.
        /// </summary>
        /// <param name="html">The input html text. May not be null.</param>
        /// <returns>The detected encoding.</returns>
        public Encoding DetectEncodingHtml(string html) {
            if (html == null) {
                throw new ArgumentNullException("html");
            }

            using (StringReader sr = new StringReader(html)) {
                Encoding encoding = this.DetectEncoding(sr);
                return encoding;
            }
        }

        /// <summary>
        /// Gets the HTML node with the specified 'id' attribute value.
        /// </summary>
        /// <param name="id">The attribute id to match. May not be null.</param>
        /// <returns>The HTML node with the matching id or null if not found.</returns>
        public HtmlNode GetElementbyId(string id) {
            if (id == null) {
                throw new ArgumentNullException("id");
            }

            if (this.Nodesid == null) {
                throw new Exception(HtmlExceptionUseIdAttributeFalse);
            }

            return this.Nodesid.ContainsKey(id.ToLower()) ? this.Nodesid[id.ToLower()] : null;
        }

        /// <summary>
        /// Loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        public void Load(Stream stream) {
            this.Load(new StreamReader(stream, this.OptionDefaultStreamEncoding));
        }

        /// <summary>
        /// Loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
        public void Load(Stream stream, bool detectEncodingFromByteOrderMarks) {
            this.Load(new StreamReader(stream, detectEncodingFromByteOrderMarks));
        }

        /// <summary>
        /// Loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public void Load(Stream stream, Encoding encoding) {
            this.Load(new StreamReader(stream, encoding));
        }

        /// <summary>
        /// Loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
        public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks) {
            this.Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks));
        }

        /// <summary>
        /// Loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
        /// <param name="buffersize">The minimum buffer size.</param>
        public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize) {
            this.Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks, buffersize));
        }


        /// <summary>
        /// Loads the HTML document from the specified TextReader.
        /// </summary>
        /// <param name="reader">The TextReader used to feed the HTML data into the document. May not be null.</param>
        public void Load(TextReader reader) {
            // all Load methods pass down to this one
            if (reader == null) {
                throw new ArgumentNullException("reader");
            }

            this._onlyDetectEncoding = false;

            if (this.OptionCheckSyntax) {
                this.Openednodes = new Dictionary<int, HtmlNode>();
            }
            else {
                this.Openednodes = null;
            }

            if (this.OptionUseIdAttribute) {
                this.Nodesid = new Dictionary<string, HtmlNode>();
            }
            else {
                this.Nodesid = null;
            }

            StreamReader sr = reader as StreamReader;
            if (sr != null) {
                try {
                    // trigger bom read if needed
                    sr.Peek();
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch (Exception)
                    // ReSharper restore EmptyGeneralCatchClause
                {
                    // void on purpose
                }

                this._streamencoding = sr.CurrentEncoding;
            }
            else {
                this._streamencoding = null;
            }

            this._declaredencoding = null;

            this.Text = reader.ReadToEnd();
            this._documentnode = this.CreateNode(HtmlNodeType.Document, 0);
            this.Parse();

            if (!this.OptionCheckSyntax || this.Openednodes == null) {
                return;
            }

            foreach (HtmlNode node in this.Openednodes.Values) {
                if (!node._starttag) // already reported
                {
                    continue;
                }

                string html;
                if (this.OptionExtractErrorSourceText) {
                    html = node.OuterHtml;
                    if (html.Length > this.OptionExtractErrorSourceTextMaxLength) {
                        html = html.Substring(0, this.OptionExtractErrorSourceTextMaxLength);
                    }
                }
                else {
                    html = string.Empty;
                }

                this.AddError(
                    HtmlParseErrorCode.TagNotClosed,
                    node._line, node._lineposition,
                    node._streamposition, html,
                    "End tag </" + node.Name + "> was not found");
            }

            // we don't need this anymore
            this.Openednodes.Clear();
        }

        /// <summary>
        /// Loads the HTML document from the specified string.
        /// </summary>
        /// <param name="html">String containing the HTML document to load. May not be null.</param>
        public void LoadHtml(string html) {
            if (html == null) {
                throw new ArgumentNullException("html");
            }

            using (StringReader sr = new StringReader(html)) {
                this.Load(sr);
            }
        }

        /// <summary>
        /// Saves the HTML document to the specified stream.
        /// </summary>
        /// <param name="outStream">The stream to which you want to save.</param>
        public void Save(Stream outStream) {
            StreamWriter sw = new StreamWriter(outStream, this.GetOutEncoding());
            this.Save(sw);
        }

        /// <summary>
        /// Saves the HTML document to the specified stream.
        /// </summary>
        /// <param name="outStream">The stream to which you want to save. May not be null.</param>
        /// <param name="encoding">The character encoding to use. May not be null.</param>
        public void Save(Stream outStream, Encoding encoding) {
            if (outStream == null) {
                throw new ArgumentNullException("outStream");
            }

            if (encoding == null) {
                throw new ArgumentNullException("encoding");
            }

            StreamWriter sw = new StreamWriter(outStream, encoding);
            this.Save(sw);
        }


        /// <summary>
        /// Saves the HTML document to the specified StreamWriter.
        /// </summary>
        /// <param name="writer">The StreamWriter to which you want to save.</param>
        public void Save(StreamWriter writer) {
            this.Save((TextWriter) writer);
        }

        /// <summary>
        /// Saves the HTML document to the specified TextWriter.
        /// </summary>
        /// <param name="writer">The TextWriter to which you want to save. May not be null.</param>
        public void Save(TextWriter writer) {
            if (writer == null) {
                throw new ArgumentNullException("writer");
            }

            this.DocumentNode.WriteTo(writer);
            writer.Flush();
        }

        /// <summary>
        /// Saves the HTML document to the specified XmlWriter.
        /// </summary>
        /// <param name="writer">The XmlWriter to which you want to save.</param>
        public void Save(XmlWriter writer) {
            this.DocumentNode.WriteTo(writer);
            writer.Flush();
        }

        #endregion

        #region Internal Methods

        internal HtmlAttribute CreateAttribute() {
            return new HtmlAttribute(this);
        }

        internal HtmlNode CreateNode(HtmlNodeType type) {
            return this.CreateNode(type, -1);
        }

        internal HtmlNode CreateNode(HtmlNodeType type, int index) {
            switch (type) {
                case HtmlNodeType.Comment:
                    return new HtmlCommentNode(this, index);

                case HtmlNodeType.Text:
                    return new HtmlTextNode(this, index);

                default:
                    return new HtmlNode(type, this, index);
            }
        }

        internal Encoding GetOutEncoding() {
            // when unspecified, use the stream encoding first
            return this._declaredencoding ?? (this._streamencoding ?? this.OptionDefaultStreamEncoding);
        }

        internal HtmlNode GetXmlDeclaration() {
            if (!this._documentnode.HasChildNodes) {
                return null;
            }

            foreach (HtmlNode node in this._documentnode._childnodes) {
                if (node.Name == "?xml") // it's ok, names are case sensitive
                {
                    return node;
                }
            }

            return null;
        }

        internal void SetIdForNode(HtmlNode node, string id) {
            if (!this.OptionUseIdAttribute) {
                return;
            }

            if ((this.Nodesid == null) || (id == null)) {
                return;
            }

            if (node == null) {
                this.Nodesid.Remove(id.ToLower());
            }
            else {
                this.Nodesid[id.ToLower()] = node;
            }
        }

        internal void UpdateLastParentNode() {
            do {
                if (this._lastparentnode.Closed) {
                    this._lastparentnode = this._lastparentnode.ParentNode;
                }
            } while ((this._lastparentnode != null) && (this._lastparentnode.Closed));

            if (this._lastparentnode == null) {
                this._lastparentnode = this._documentnode;
            }
        }

        #endregion

        #region Private Methods

        void AddError(HtmlParseErrorCode code, int line, int linePosition, int streamPosition, string sourceText,
            string reason) {
            HtmlParseError err = new HtmlParseError(code, line, linePosition, streamPosition, sourceText, reason);
            this._parseerrors.Add(err);
            return;
        }

        void CloseCurrentNode() {
            if (this._currentnode.Closed) // text or document are by def closed
            {
                return;
            }

            bool error = false;
            HtmlNode prev = Utilities.GetDictionaryValueOrNull(this.Lastnodes, this._currentnode.Name);

            // find last node of this kind
            if (prev == null) {
                if (HtmlNode.IsClosedElement(this._currentnode.Name)) {
                    // </br> will be seen as <br>
                    this._currentnode.CloseNode(this._currentnode);

                    // add to parent node
                    if (this._lastparentnode != null) {
                        HtmlNode foundNode = null;
                        Stack<HtmlNode> futureChild = new Stack<HtmlNode>();
                        for (HtmlNode node = this._lastparentnode.LastChild;
                            node != null;
                            node = node.PreviousSibling) {
                            if ((node.Name == this._currentnode.Name) && (!node.HasChildNodes)) {
                                foundNode = node;
                                break;
                            }

                            futureChild.Push(node);
                        }

                        if (foundNode != null) {
                            while (futureChild.Count != 0) {
                                HtmlNode node = futureChild.Pop();
                                this._lastparentnode.RemoveChild(node);
                                foundNode.AppendChild(node);
                            }
                        }
                        else {
                            this._lastparentnode.AppendChild(this._currentnode);
                        }
                    }
                }
                else {
                    // node has no parent
                    // node is not a closed node

                    if (HtmlNode.CanOverlapElement(this._currentnode.Name)) {
                        // this is a hack: add it as a text node
                        HtmlNode closenode = this.CreateNode(HtmlNodeType.Text, this._currentnode._outerstartindex);
                        closenode._outerlength = this._currentnode._outerlength;
                        ((HtmlTextNode) closenode).Text = ((HtmlTextNode) closenode).Text.ToLower();
                        if (this._lastparentnode != null) {
                            this._lastparentnode.AppendChild(closenode);
                        }
                    }
                    else {
                        if (HtmlNode.IsEmptyElement(this._currentnode.Name)) {
                            this.AddError(
                                HtmlParseErrorCode.EndTagNotRequired, this._currentnode._line,
                                this._currentnode._lineposition, this._currentnode._streamposition,
                                this._currentnode.OuterHtml,
                                "End tag </" + this._currentnode.Name + "> is not required");
                        }
                        else {
                            // node cannot overlap, node is not empty
                            this.AddError(
                                HtmlParseErrorCode.TagNotOpened, this._currentnode._line,
                                this._currentnode._lineposition, this._currentnode._streamposition,
                                this._currentnode.OuterHtml,
                                "Start tag <" + this._currentnode.Name + "> was not found");
                            error = true;
                        }
                    }
                }
            }
            else {
                if (this.OptionFixNestedTags) {
                    if (this.FindResetterNodes(prev, this.GetResetters(this._currentnode.Name))) {
                        this.AddError(
                            HtmlParseErrorCode.EndTagInvalidHere, this._currentnode._line,
                            this._currentnode._lineposition, this._currentnode._streamposition,
                            this._currentnode.OuterHtml,
                            "End tag </" + this._currentnode.Name + "> invalid here");
                        error = true;
                    }
                }

                if (!error) {
                    this.Lastnodes[this._currentnode.Name] = prev._prevwithsamename;
                    prev.CloseNode(this._currentnode);
                }
            }


            // we close this node, get grandparent
            if (!error) {
                if ((this._lastparentnode != null) &&
                    ((!HtmlNode.IsClosedElement(this._currentnode.Name)) ||
                     (this._currentnode._starttag))) {
                    this.UpdateLastParentNode();
                }
            }
        }

        string CurrentNodeName() {
            return this.Text.Substring(this._currentnode._namestartindex, this._currentnode._namelength);
        }


        void DecrementPosition() {
            this._index--;
            if (this._lineposition == 1) {
                this._lineposition = this._maxlineposition;
                this._line--;
            }
            else {
                this._lineposition--;
            }
        }

        HtmlNode FindResetterNode(HtmlNode node, string name) {
            HtmlNode resetter = Utilities.GetDictionaryValueOrNull(this.Lastnodes, name);
            if (resetter == null) {
                return null;
            }

            if (resetter.Closed) {
                return null;
            }

            if (resetter._streamposition < node._streamposition) {
                return null;
            }

            return resetter;
        }

        bool FindResetterNodes(HtmlNode node, string[] names) {
            if (names == null) {
                return false;
            }

            for (int i = 0; i < names.Length; i++) {
                if (this.FindResetterNode(node, names[i]) != null) {
                    return true;
                }
            }

            return false;
        }

        void FixNestedTag(string name, string[] resetters) {
            if (resetters == null) {
                return;
            }

            HtmlNode prev = Utilities.GetDictionaryValueOrNull(this.Lastnodes, this._currentnode.Name);
            // if we find a previous unclosed same name node, without a resetter node between, we must close it
            if (prev == null || (this.Lastnodes[name].Closed)) {
                return;
            }

            // try to find a resetter node, if found, we do nothing
            if (this.FindResetterNodes(prev, resetters)) {
                return;
            }

            // ok we need to close the prev now
            // create a fake closer node
            HtmlNode close = new HtmlNode(prev.NodeType, this, -1);
            close._endnode = close;
            prev.CloseNode(close);
        }

        void FixNestedTags() {
            // we are only interested by start tags, not closing tags
            if (!this._currentnode._starttag) {
                return;
            }

            string name = this.CurrentNodeName();
            this.FixNestedTag(name, this.GetResetters(name));
        }

        string[] GetResetters(string name) {
            switch (name) {
                case "li":
                    return new string[] {"ul"};

                case "tr":
                    return new string[] {"table"};

                case "th":
                case "td":
                    return new string[] {"tr", "table"};

                default:
                    return null;
            }
        }

        void IncrementPosition() {
            if (this._crc32 != null) {
                // REVIEW: should we add some checksum code in DecrementPosition too?
                this._crc32.AddToCRC32(this._c);
            }

            this._index++;
            this._maxlineposition = this._lineposition;
            if (this._c == 10) {
                this._lineposition = 1;
                this._line++;
            }
            else {
                this._lineposition++;
            }
        }

        bool NewCheck() {
            if (this._c != '<') {
                return false;
            }

            if (this._index < this.Text.Length) {
                if (this.Text[this._index] == '%') {
                    switch (this._state) {
                        case ParseState.AttributeAfterEquals:
                            this.PushAttributeValueStart(this._index - 1);
                            break;

                        case ParseState.BetweenAttributes:
                            this.PushAttributeNameStart(this._index - 1);
                            break;

                        case ParseState.WhichTag:
                            this.PushNodeNameStart(true, this._index - 1);
                            this._state = ParseState.Tag;
                            break;
                    }

                    this._oldstate = this._state;
                    this._state = ParseState.ServerSideCode;
                    return true;
                }
            }

            if (!this.PushNodeEnd(this._index - 1, true)) {
                // stop parsing
                this._index = this.Text.Length;
                return true;
            }

            this._state = ParseState.WhichTag;
            if ((this._index - 1) <= (this.Text.Length - 2)) {
                if (this.Text[this._index] == '!') {
                    this.PushNodeStart(HtmlNodeType.Comment, this._index - 1);
                    this.PushNodeNameStart(true, this._index);
                    this.PushNodeNameEnd(this._index + 1);
                    this._state = ParseState.Comment;
                    if (this._index < (this.Text.Length - 2)) {
                        if ((this.Text[this._index + 1] == '-') &&
                            (this.Text[this._index + 2] == '-')) {
                            this._fullcomment = true;
                        }
                        else {
                            this._fullcomment = false;
                        }
                    }

                    return true;
                }
            }

            this.PushNodeStart(HtmlNodeType.Element, this._index - 1);
            return true;
        }

        void Parse() {
            int lastquote = 0;
            if (this.OptionComputeChecksum) {
                this._crc32 = new Crc32();
            }

            this.Lastnodes = new Dictionary<string, HtmlNode>();
            this._c = 0;
            this._fullcomment = false;
            this._parseerrors = new List<HtmlParseError>();
            this._line = 1;
            this._lineposition = 1;
            this._maxlineposition = 1;

            this._state = ParseState.Text;
            this._oldstate = this._state;
            this._documentnode._innerlength = this.Text.Length;
            this._documentnode._outerlength = this.Text.Length;
            this._remainderOffset = this.Text.Length;

            this._lastparentnode = this._documentnode;
            this._currentnode = this.CreateNode(HtmlNodeType.Text, 0);
            this._currentattribute = null;

            this._index = 0;
            this.PushNodeStart(HtmlNodeType.Text, 0);
            while (this._index < this.Text.Length) {
                this._c = this.Text[this._index];
                this.IncrementPosition();

                switch (this._state) {
                    case ParseState.Text:
                        if (this.NewCheck()) {
                            continue;
                        }

                        break;

                    case ParseState.WhichTag:
                        if (this.NewCheck()) {
                            continue;
                        }

                        if (this._c == '/') {
                            this.PushNodeNameStart(false, this._index);
                        }
                        else {
                            this.PushNodeNameStart(true, this._index - 1);
                            this.DecrementPosition();
                        }

                        this._state = ParseState.Tag;
                        break;

                    case ParseState.Tag:
                        if (this.NewCheck()) {
                            continue;
                        }

                        if (IsWhiteSpace(this._c)) {
                            this.PushNodeNameEnd(this._index - 1);
                            if (this._state != ParseState.Tag) {
                                continue;
                            }

                            this._state = ParseState.BetweenAttributes;
                            continue;
                        }

                        if (this._c == '/') {
                            this.PushNodeNameEnd(this._index - 1);
                            if (this._state != ParseState.Tag) {
                                continue;
                            }

                            this._state = ParseState.EmptyTag;
                            continue;
                        }

                        if (this._c == '>') {
                            this.PushNodeNameEnd(this._index - 1);
                            if (this._state != ParseState.Tag) {
                                continue;
                            }

                            if (!this.PushNodeEnd(this._index, false)) {
                                // stop parsing
                                this._index = this.Text.Length;
                                break;
                            }

                            if (this._state != ParseState.Tag) {
                                continue;
                            }

                            this._state = ParseState.Text;
                            this.PushNodeStart(HtmlNodeType.Text, this._index);
                        }

                        break;

                    case ParseState.BetweenAttributes:
                        if (this.NewCheck()) {
                            continue;
                        }

                        if (IsWhiteSpace(this._c)) {
                            continue;
                        }

                        if ((this._c == '/') || (this._c == '?')) {
                            this._state = ParseState.EmptyTag;
                            continue;
                        }

                        if (this._c == '>') {
                            if (!this.PushNodeEnd(this._index, false)) {
                                // stop parsing
                                this._index = this.Text.Length;
                                break;
                            }

                            if (this._state != ParseState.BetweenAttributes) {
                                continue;
                            }

                            this._state = ParseState.Text;
                            this.PushNodeStart(HtmlNodeType.Text, this._index);
                            continue;
                        }

                        this.PushAttributeNameStart(this._index - 1);
                        this._state = ParseState.AttributeName;
                        break;

                    case ParseState.EmptyTag:
                        if (this.NewCheck()) {
                            continue;
                        }

                        if (this._c == '>') {
                            if (!this.PushNodeEnd(this._index, true)) {
                                // stop parsing
                                this._index = this.Text.Length;
                                break;
                            }

                            if (this._state != ParseState.EmptyTag) {
                                continue;
                            }

                            this._state = ParseState.Text;
                            this.PushNodeStart(HtmlNodeType.Text, this._index);
                            continue;
                        }

                        this._state = ParseState.BetweenAttributes;
                        break;

                    case ParseState.AttributeName:
                        if (this.NewCheck()) {
                            continue;
                        }

                        if (IsWhiteSpace(this._c)) {
                            this.PushAttributeNameEnd(this._index - 1);
                            this._state = ParseState.AttributeBeforeEquals;
                            continue;
                        }

                        if (this._c == '=') {
                            this.PushAttributeNameEnd(this._index - 1);
                            this._state = ParseState.AttributeAfterEquals;
                            continue;
                        }

                        if (this._c == '>') {
                            this.PushAttributeNameEnd(this._index - 1);
                            if (!this.PushNodeEnd(this._index, false)) {
                                // stop parsing
                                this._index = this.Text.Length;
                                break;
                            }

                            if (this._state != ParseState.AttributeName) {
                                continue;
                            }

                            this._state = ParseState.Text;
                            this.PushNodeStart(HtmlNodeType.Text, this._index);
                            continue;
                        }

                        break;

                    case ParseState.AttributeBeforeEquals:
                        if (this.NewCheck()) {
                            continue;
                        }

                        if (IsWhiteSpace(this._c)) {
                            continue;
                        }

                        if (this._c == '>') {
                            if (!this.PushNodeEnd(this._index, false)) {
                                // stop parsing
                                this._index = this.Text.Length;
                                break;
                            }

                            if (this._state != ParseState.AttributeBeforeEquals) {
                                continue;
                            }

                            this._state = ParseState.Text;
                            this.PushNodeStart(HtmlNodeType.Text, this._index);
                            continue;
                        }

                        if (this._c == '=') {
                            this._state = ParseState.AttributeAfterEquals;
                            continue;
                        }

                        // no equals, no whitespace, it's a new attrribute starting
                        this._state = ParseState.BetweenAttributes;
                        this.DecrementPosition();
                        break;

                    case ParseState.AttributeAfterEquals:
                        if (this.NewCheck()) {
                            continue;
                        }

                        if (IsWhiteSpace(this._c)) {
                            continue;
                        }

                        if ((this._c == '\'') || (this._c == '"')) {
                            this._state = ParseState.QuotedAttributeValue;
                            this.PushAttributeValueStart(this._index, this._c);
                            lastquote = this._c;
                            continue;
                        }

                        if (this._c == '>') {
                            if (!this.PushNodeEnd(this._index, false)) {
                                // stop parsing
                                this._index = this.Text.Length;
                                break;
                            }

                            if (this._state != ParseState.AttributeAfterEquals) {
                                continue;
                            }

                            this._state = ParseState.Text;
                            this.PushNodeStart(HtmlNodeType.Text, this._index);
                            continue;
                        }

                        this.PushAttributeValueStart(this._index - 1);
                        this._state = ParseState.AttributeValue;
                        break;

                    case ParseState.AttributeValue:
                        if (this.NewCheck()) {
                            continue;
                        }

                        if (IsWhiteSpace(this._c)) {
                            this.PushAttributeValueEnd(this._index - 1);
                            this._state = ParseState.BetweenAttributes;
                            continue;
                        }

                        if (this._c == '>') {
                            this.PushAttributeValueEnd(this._index - 1);
                            if (!this.PushNodeEnd(this._index, false)) {
                                // stop parsing
                                this._index = this.Text.Length;
                                break;
                            }

                            if (this._state != ParseState.AttributeValue) {
                                continue;
                            }

                            this._state = ParseState.Text;
                            this.PushNodeStart(HtmlNodeType.Text, this._index);
                            continue;
                        }

                        break;

                    case ParseState.QuotedAttributeValue:
                        if (this._c == lastquote) {
                            this.PushAttributeValueEnd(this._index - 1);
                            this._state = ParseState.BetweenAttributes;
                            continue;
                        }

                        if (this._c == '<') {
                            if (this._index < this.Text.Length) {
                                if (this.Text[this._index] == '%') {
                                    this._oldstate = this._state;
                                    this._state = ParseState.ServerSideCode;
                                    continue;
                                }
                            }
                        }

                        break;

                    case ParseState.Comment:
                        if (this._c == '>') {
                            if (this._fullcomment) {
                                if ((this.Text[this._index - 2] != '-') ||
                                    (this.Text[this._index - 3] != '-')) {
                                    continue;
                                }
                            }

                            if (!this.PushNodeEnd(this._index, false)) {
                                // stop parsing
                                this._index = this.Text.Length;
                                break;
                            }

                            this._state = ParseState.Text;
                            this.PushNodeStart(HtmlNodeType.Text, this._index);
                            continue;
                        }

                        break;

                    case ParseState.ServerSideCode:
                        if (this._c == '%') {
                            if (this._index < this.Text.Length) {
                                if (this.Text[this._index] == '>') {
                                    switch (this._oldstate) {
                                        case ParseState.AttributeAfterEquals:
                                            this._state = ParseState.AttributeValue;
                                            break;

                                        case ParseState.BetweenAttributes:
                                            this.PushAttributeNameEnd(this._index + 1);
                                            this._state = ParseState.BetweenAttributes;
                                            break;

                                        default:
                                            this._state = this._oldstate;
                                            break;
                                    }

                                    this.IncrementPosition();
                                }
                            }
                        }

                        break;

                    case ParseState.PcData:
                        // look for </tag + 1 char

                        // check buffer end
                        if ((this._currentnode._namelength + 3) <= (this.Text.Length - (this._index - 1))) {
                            if (string.Compare(this.Text.Substring(this._index - 1, this._currentnode._namelength + 2),
                                "</" + this._currentnode.Name, StringComparison.OrdinalIgnoreCase) == 0) {
                                int c = this.Text[this._index - 1 + 2 + this._currentnode.Name.Length];
                                if ((c == '>') || (IsWhiteSpace(c))) {
                                    // add the script as a text node
                                    HtmlNode script = this.CreateNode(HtmlNodeType.Text,
                                        this._currentnode._outerstartindex + this._currentnode._outerlength);
                                    script._outerlength = this._index - 1 - script._outerstartindex;
                                    this._currentnode.AppendChild(script);


                                    this.PushNodeStart(HtmlNodeType.Element, this._index - 1);
                                    this.PushNodeNameStart(false, this._index - 1 + 2);
                                    this._state = ParseState.Tag;
                                    this.IncrementPosition();
                                }
                            }
                        }

                        break;
                }
            }

            // finish the current work
            if (this._currentnode._namestartindex > 0) {
                this.PushNodeNameEnd(this._index);
            }

            this.PushNodeEnd(this._index, false);

            // we don't need this anymore
            this.Lastnodes.Clear();
        }

        void PushAttributeNameEnd(int index) {
            this._currentattribute._namelength = index - this._currentattribute._namestartindex;
            this._currentnode.Attributes.Append(this._currentattribute);
        }

        void PushAttributeNameStart(int index) {
            this._currentattribute = this.CreateAttribute();
            this._currentattribute._namestartindex = index;
            this._currentattribute.Line = this._line;
            this._currentattribute._lineposition = this._lineposition;
            this._currentattribute._streamposition = index;
        }

        void PushAttributeValueEnd(int index) {
            this._currentattribute._valuelength = index - this._currentattribute._valuestartindex;
        }

        void PushAttributeValueStart(int index) {
            this.PushAttributeValueStart(index, 0);
        }

        void PushAttributeValueStart(int index, int quote) {
            this._currentattribute._valuestartindex = index;
            if (quote == '\'') {
                this._currentattribute.QuoteType = AttributeValueQuote.SingleQuote;
            }
        }

        bool PushNodeEnd(int index, bool close) {
            this._currentnode._outerlength = index - this._currentnode._outerstartindex;

            if ((this._currentnode._nodetype == HtmlNodeType.Text) ||
                (this._currentnode._nodetype == HtmlNodeType.Comment)) {
                // forget about void nodes
                if (this._currentnode._outerlength > 0) {
                    this._currentnode._innerlength = this._currentnode._outerlength;
                    this._currentnode._innerstartindex = this._currentnode._outerstartindex;
                    if (this._lastparentnode != null) {
                        this._lastparentnode.AppendChild(this._currentnode);
                    }
                }
            }
            else {
                if ((this._currentnode._starttag) && (this._lastparentnode != this._currentnode)) {
                    // add to parent node
                    if (this._lastparentnode != null) {
                        this._lastparentnode.AppendChild(this._currentnode);
                    }

                    this.ReadDocumentEncoding(this._currentnode);

                    // remember last node of this kind
                    HtmlNode prev = Utilities.GetDictionaryValueOrNull(this.Lastnodes, this._currentnode.Name);

                    this._currentnode._prevwithsamename = prev;
                    this.Lastnodes[this._currentnode.Name] = this._currentnode;

                    // change parent?
                    if ((this._currentnode.NodeType == HtmlNodeType.Document) ||
                        (this._currentnode.NodeType == HtmlNodeType.Element)) {
                        this._lastparentnode = this._currentnode;
                    }

                    if (HtmlNode.IsCDataElement(this.CurrentNodeName())) {
                        this._state = ParseState.PcData;
                        return true;
                    }

                    if ((HtmlNode.IsClosedElement(this._currentnode.Name)) ||
                        (HtmlNode.IsEmptyElement(this._currentnode.Name))) {
                        close = true;
                    }
                }
            }

            if ((close) || (!this._currentnode._starttag)) {
                if ((this.OptionStopperNodeName != null) && (this._remainder == null) &&
                    (string.Compare(this._currentnode.Name, this.OptionStopperNodeName,
                        StringComparison.OrdinalIgnoreCase) == 0)) {
                    this._remainderOffset = index;
                    this._remainder = this.Text.Substring(this._remainderOffset);
                    this.CloseCurrentNode();
                    return false; // stop parsing
                }

                this.CloseCurrentNode();
            }

            return true;
        }

        void PushNodeNameEnd(int index) {
            this._currentnode._namelength = index - this._currentnode._namestartindex;
            if (this.OptionFixNestedTags) {
                this.FixNestedTags();
            }
        }

        void PushNodeNameStart(bool starttag, int index) {
            this._currentnode._starttag = starttag;
            this._currentnode._namestartindex = index;
        }

        void PushNodeStart(HtmlNodeType type, int index) {
            this._currentnode = this.CreateNode(type, index);
            this._currentnode._line = this._line;
            this._currentnode._lineposition = this._lineposition;
            if (type == HtmlNodeType.Element) {
                this._currentnode._lineposition--;
            }

            this._currentnode._streamposition = index;
        }

        void ReadDocumentEncoding(HtmlNode node) {
            if (!this.OptionReadEncoding) {
                return;
            }
            // format is 
            // <meta http-equiv="content-type" content="text/html;charset=iso-8859-1" />

            // when we append a child, we are in node end, so attributes are already populated
            if (node._namelength != 4) // quick check, avoids string alloc
            {
                return;
            }

            if (node.Name != "meta") // all nodes names are lowercase
            {
                return;
            }

            HtmlAttribute att = node.Attributes["http-equiv"];
            if (att == null) {
                return;
            }

            if (string.Compare(att.Value, "content-type", StringComparison.OrdinalIgnoreCase) != 0) {
                return;
            }

            HtmlAttribute content = node.Attributes["content"];
            if (content != null) {
                string charset = NameValuePairList.GetNameValuePairsValue(content.Value, "charset");
                if (!string.IsNullOrEmpty(charset)) {
                    // The following check fixes the the bug described at: http://htmlagilitypack.codeplex.com/WorkItem/View.aspx?WorkItemId=25273
                    if (string.Equals(charset, "utf8", StringComparison.OrdinalIgnoreCase)) {
                        charset = "utf-8";
                    }

                    try {
                        this._declaredencoding = Encoding.GetEncoding(charset);
                    }
                    catch (ArgumentException) {
                        this._declaredencoding = null;
                    }

                    if (this._onlyDetectEncoding) {
                        throw new EncodingFoundException(this._declaredencoding);
                    }

                    if (this._streamencoding != null) {
#if NETSTANDARD1_4
						if (_declaredencoding.WebName != _streamencoding.WebName)
#else
                        if (this._declaredencoding != null) {
                            if (this._declaredencoding.WindowsCodePage != this._streamencoding.WindowsCodePage)
#endif
                            {
                                this.AddError(
                                    HtmlParseErrorCode.CharsetMismatch, this._line, this._lineposition, this._index,
                                    node.OuterHtml,
                                    "Encoding mismatch between StreamEncoding: " + this._streamencoding.WebName +
                                    " and DeclaredEncoding: " + this._declaredencoding.WebName);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Nested type: ParseState

        enum ParseState {
            Text,
            WhichTag,
            Tag,
            BetweenAttributes,
            EmptyTag,
            AttributeName,
            AttributeBeforeEquals,
            AttributeAfterEquals,
            AttributeValue,
            Comment,
            QuotedAttributeValue,
            ServerSideCode,
            PcData
        }

        #endregion
    }
}