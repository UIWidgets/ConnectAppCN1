// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

using System;
using System.Text;

namespace HtmlAgilityPack {
    class EncodingFoundException : Exception {
        #region Fields

        Encoding _encoding;

        #endregion

        #region Constructors

        internal EncodingFoundException(Encoding encoding) {
            this._encoding = encoding;
        }

        #endregion

        #region Properties

        internal Encoding Encoding {
            get { return this._encoding; }
        }

        #endregion
    }
}