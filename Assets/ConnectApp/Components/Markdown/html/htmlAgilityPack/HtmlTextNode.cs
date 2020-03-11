// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

namespace HtmlAgilityPack {
    /// <summary>
    /// Represents an HTML text node.
    /// </summary>
    public class HtmlTextNode : HtmlNode {
        #region Fields

        string _text;

        #endregion

        #region Constructors

        internal HtmlTextNode(HtmlDocument ownerdocument, int index)
            :
            base(HtmlNodeType.Text, ownerdocument, index) { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the HTML between the start and end tags of the object. In the case of a text node, it is equals to OuterHtml.
        /// </summary>
        public override string InnerHtml {
            get { return this.OuterHtml; }
            set { this._text = value; }
        }

        /// <summary>
        /// Gets or Sets the object and its content in HTML.
        /// </summary>
        public override string OuterHtml {
            get {
                if (this._text == null) {
                    return base.OuterHtml;
                }

                return this._text;
            }
        }

        /// <summary>
        /// Gets or Sets the text of the node.
        /// </summary>
        public string Text {
            get {
                if (this._text == null) {
                    return base.OuterHtml;
                }

                return this._text;
            }
            set { this._text = value; }
        }

        #endregion
    }
}