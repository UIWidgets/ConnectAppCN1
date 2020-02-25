// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

namespace HtmlAgilityPack {
    /// <summary>
    /// Represents an HTML comment.
    /// </summary>
    public class HtmlCommentNode : HtmlNode {
        #region Fields

        string _comment;

        #endregion

        #region Constructors

        internal HtmlCommentNode(HtmlDocument ownerdocument, int index)
            :
            base(HtmlNodeType.Comment, ownerdocument, index) { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the comment text of the node.
        /// </summary>
        public string Comment {
            get {
                if (this._comment == null) {
                    return base.InnerHtml;
                }

                return this._comment;
            }
            set { this._comment = value; }
        }

        /// <summary>
        /// Gets or Sets the HTML between the start and end tags of the object. In the case of a text node, it is equals to OuterHtml.
        /// </summary>
        public override string InnerHtml {
            get {
                if (this._comment == null) {
                    return base.InnerHtml;
                }

                return this._comment;
            }
            set { this._comment = value; }
        }

        /// <summary>
        /// Gets or Sets the object and its content in HTML.
        /// </summary>
        public override string OuterHtml {
            get {
                if (this._comment == null) {
                    return base.OuterHtml;
                }

                return "<!--" + this._comment + "-->";
            }
        }

        #endregion
    }
}