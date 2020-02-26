// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

namespace HtmlAgilityPack {
    /// <summary>
    /// Represents a parsing error found during document parsing.
    /// </summary>
    public class HtmlParseError {
        #region Fields

        HtmlParseErrorCode _code;
        int _line;
        int _linePosition;
        string _reason;
        string _sourceText;
        int _streamPosition;

        #endregion

        #region Constructors

        internal HtmlParseError(
            HtmlParseErrorCode code,
            int line,
            int linePosition,
            int streamPosition,
            string sourceText,
            string reason) {
            this._code = code;
            this._line = line;
            this._linePosition = linePosition;
            this._streamPosition = streamPosition;
            this._sourceText = sourceText;
            this._reason = reason;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of error.
        /// </summary>
        public HtmlParseErrorCode Code {
            get { return this._code; }
        }

        /// <summary>
        /// Gets the line number of this error in the document.
        /// </summary>
        public int Line {
            get { return this._line; }
        }

        /// <summary>
        /// Gets the column number of this error in the document.
        /// </summary>
        public int LinePosition {
            get { return this._linePosition; }
        }

        /// <summary>
        /// Gets a description for the error.
        /// </summary>
        public string Reason {
            get { return this._reason; }
        }

        /// <summary>
        /// Gets the the full text of the line containing the error.
        /// </summary>
        public string SourceText {
            get { return this._sourceText; }
        }

        /// <summary>
        /// Gets the absolute stream position of this error in the document, relative to the start of the document.
        /// </summary>
        public int StreamPosition {
            get { return this._streamPosition; }
        }

        #endregion
    }
}