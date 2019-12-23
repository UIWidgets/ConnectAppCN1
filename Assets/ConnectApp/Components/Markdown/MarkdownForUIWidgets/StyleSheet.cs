using System.Collections.Generic;
using Unity.UIWidgets.cupertino;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;

namespace markdown {
    public class MarkdownStyleSheet {
        public MarkdownStyleSheet(
            TextStyle a,
            TextStyle p,
            TextStyle code,
            TextStyle h1,
            TextStyle h2,
            TextStyle h3,
            TextStyle h4,
            TextStyle h5,
            TextStyle h6,
            TextStyle em,
            TextStyle strong,
            TextStyle del,
            TextStyle blockquote,
            TextStyle img,
            TextStyle checkbox,
            float blockSpacing,
            float listIndent,
            TextStyle listBullet,
            TextStyle tableHead,
            TextStyle tableBody,
            TextAlign tableHeadAlign,
            TableBorder tableBorder,
            TableColumnWidth tableColumnWidth,
            EdgeInsets tableCellsPadding,
            Decoration tableCellsDecoration,
            EdgeInsets blockquotePadding,
            Decoration blockquoteDecoration,
            EdgeInsets codeblockPadding,
            Decoration codeblockDecoration,
            Decoration horizontalRuleDecoration,
            float textScaleFactor = 1.0f) {
            this._styles = new Dictionary<string, TextStyle> {
                {"a", a},
                {"p", p},
                {"li", p},
                {"code", code},
                {"pre", p},
                {"h1", h1},
                {"h2", h2},
                {"h3", h3},
                {"h4", h4},
                {"h5", h5},
                {"h6", h6},
                {"em", em},
                {"strong", strong},
                {"del", del},
                {"blockquote", blockquote},
                {"img", img},
                {"table", p},
                {"th", tableHead},
                {"tr", tableBody},
                {"td", tableBody},
                {"ul", p}
            };

            this.a = a;
            this.p = p;
            this.code = code;
            this.h1 = h1;
            this.h2 = h2;
            this.h3 = h3;
            this.h4 = h4;
            this.h5 = h5;
            this.h6 = h6;
            this.em = em;
            this.strong = strong;
            this.del = del;
            this.blockquote = blockquote;
            this.img = img;
            this.checkbox = checkbox;
            this.blockSpacing = blockSpacing;
            this.listIndent = listIndent;
            this.listBullet = listBullet;
            this.tableHead = tableHead;
            this.tableBody = tableBody;
            this.tableHeadAlign = tableHeadAlign;
            this.tableBorder = tableBorder;
            this.tableColumnWidth = tableColumnWidth;
            this.tableCellsPadding = tableCellsPadding;
            this.tableCellsDecoration = tableCellsDecoration;
            this.blockquotePadding = blockquotePadding;
            this.blockquoteDecoration = blockquoteDecoration;
            this.codeblockPadding = codeblockPadding;
            this.codeblockDecoration = codeblockDecoration;
            this.horizontalRuleDecoration = horizontalRuleDecoration;
            this.textScaleFactor = textScaleFactor;
        }

        public TextStyle a,
            p,
            code,
            h1,
            h2,
            h3,
            h4,
            h5,
            h6,
            em,
            strong,
            del,
            blockquote,
            img,
            checkbox,
            listBullet,
            tableHead,
            tableBody;

        public TextAlign tableHeadAlign;
        public TableBorder tableBorder;
        public TableColumnWidth tableColumnWidth;
        public EdgeInsets tableCellsPadding, blockquotePadding, codeblockPadding;

        public float blockSpacing,
            listIndent,
            textScaleFactor;

        public Decoration blockquoteDecoration, codeblockDecoration, horizontalRuleDecoration, tableCellsDecoration;


        Dictionary<string, TextStyle> _styles;

        public TextStyle styles(string tag) {
            if (this._styles.ContainsKey(tag)) {
                return this._styles[tag];
            }

            return null;
        }

        /// Creates a [MarkdownStyleSheet] from the [TextStyle]s in the provided [ThemeData].
        public static MarkdownStyleSheet fromTheme(ThemeData theme) {
            D.assert(theme?.textTheme?.body1?.fontSize != null);
            return new MarkdownStyleSheet(
                new TextStyle(color: Colors.blue, decoration: TextDecoration.underline,
                    decorationColor: Colors.blue),
                theme.textTheme.body1,
                theme.textTheme.body1.copyWith(
                    backgroundColor: Colors.grey.shade200,
                    fontFamily: "monospace",
                    fontSize: theme.textTheme.body1.fontSize * 0.85f
                ),
                theme.textTheme.headline,
                theme.textTheme.title,
                theme.textTheme.subhead,
                theme.textTheme.body2,
                theme.textTheme.body2,
                theme.textTheme.body2,
                new TextStyle(fontStyle: FontStyle.italic),
                new TextStyle(fontWeight: FontWeight.bold),
                new TextStyle(decoration: TextDecoration.lineThrough),
                theme.textTheme.body1,
                theme.textTheme.body1,
                theme.textTheme.body1.copyWith(
                    color: theme.primaryColor
                ),
                8,
                24,
                theme.textTheme.body1,
                new TextStyle(fontWeight: FontWeight.w600),
                theme.textTheme.body1,
                TextAlign.center,
                TableBorder.all(color: Colors.grey.shade300, width: 0),
                new FlexColumnWidth(),
                EdgeInsets.fromLTRB(16, 8, 16, 8),
                new BoxDecoration(color: Colors.grey.shade50),
                EdgeInsets.all(8.0f),
                new BoxDecoration(
                    color: Colors.blue.shade100,
                    borderRadius: BorderRadius.circular(2.0f)
                ),
                EdgeInsets.all(8.0f),
                new BoxDecoration(
                    color: Colors.grey.shade200,
                    borderRadius: BorderRadius.circular(2.0f)
                ),
                new BoxDecoration(
                    border: new Border(
                        top: new BorderSide(width: 5.0f, color: Colors.grey.shade300)
                    )
                )
            );
        }


        public static MarkdownStyleSheet fromCupertinoTheme(CupertinoThemeData theme) {
            D.assert(theme?.textTheme?.textStyle?.fontSize != null);
            return new MarkdownStyleSheet(
                new TextStyle(color: CupertinoColors.activeBlue, decoration: TextDecoration.underline,
                    decorationColor: CupertinoColors.activeBlue),
                theme.textTheme.textStyle,
                theme.textTheme.textStyle.copyWith(
                    backgroundColor: CupertinoColors.darkBackgroundGray,
                    fontFamily: "monospace",
                    fontSize: theme.textTheme.textStyle.fontSize * 0.85f
                ),
                new TextStyle(
                    fontWeight: FontWeight.w500,
                    fontSize: theme.textTheme.textStyle.fontSize + 10
                ),
                new TextStyle(
                    fontWeight: FontWeight.w500,
                    fontSize: theme.textTheme.textStyle.fontSize + 8
                ),
                new TextStyle(
                    fontWeight: FontWeight.w500,
                    fontSize: theme.textTheme.textStyle.fontSize + 6
                ),
                new TextStyle(
                    fontWeight: FontWeight.w500,
                    fontSize: theme.textTheme.textStyle.fontSize + 4
                ),
                new TextStyle(
                    fontWeight: FontWeight.w500,
                    fontSize: theme.textTheme.textStyle.fontSize + 2
                ),
                new TextStyle(
                    fontWeight: FontWeight.w500,
                    fontSize: theme.textTheme.textStyle.fontSize
                ),
                new TextStyle(fontStyle: FontStyle.italic),
                new TextStyle(fontWeight: FontWeight.bold),
                new TextStyle(decoration: TextDecoration.lineThrough),
                theme.textTheme.textStyle,
                theme.textTheme.textStyle,
                theme.textTheme.textStyle.copyWith(
                    color: theme.primaryColor
                ),
                8,
                24,
                theme.textTheme.textStyle,
                new TextStyle(fontWeight: FontWeight.w600),
                theme.textTheme.textStyle,
                TextAlign.center,
                TableBorder.all(color: Colors.grey.shade300, width: 0),
                new FlexColumnWidth(),
                EdgeInsets.fromLTRB(16, 8, 16, 8),
                new BoxDecoration(color: CupertinoColors.darkBackgroundGray),
                EdgeInsets.all(16),
                new BoxDecoration(
                    color: CupertinoColors.darkBackgroundGray,
                    border: new Border(
                        left: new BorderSide(
                            color: CupertinoColors.darkBackgroundGray,
                            width: 4
                        )
                    )
                ),
                EdgeInsets.all(8.0f),
                new BoxDecoration(
                    color: CupertinoColors.darkBackgroundGray
                ),
                new BoxDecoration(
                    border: new Border(
                        top: new BorderSide(color: CupertinoColors.darkBackgroundGray)
                    )
                )
            );
        }

        public static MarkdownStyleSheet largeFromTheme(ThemeData theme) {
            return new MarkdownStyleSheet(
                new TextStyle(color: Colors.blue, decoration: TextDecoration.underline,
                    decorationColor: Colors.blue),
                theme.textTheme.body1,
                theme.textTheme.body1.copyWith(
                    backgroundColor: Colors.grey.shade200,
                    fontFamily: "monospace",
                    fontSize: theme.textTheme.body1.fontSize * 0.85f
                ),
                theme.textTheme.display3,
                theme.textTheme.display2,
                theme.textTheme.display1,
                theme.textTheme.headline,
                theme.textTheme.title,
                theme.textTheme.subhead,
                new TextStyle(fontStyle: FontStyle.italic),
                new TextStyle(fontWeight: FontWeight.bold),
                new TextStyle(decoration: TextDecoration.lineThrough),
                theme.textTheme.body1,
                theme.textTheme.body1,
                theme.textTheme.body1.copyWith(
                    color: theme.primaryColor
                ),
                8,
                24,
                theme.textTheme.body1,
                new TextStyle(fontWeight: FontWeight.w600),
                theme.textTheme.body1,
                TextAlign.center,
                TableBorder.all(color: Colors.grey.shade300),
                new FlexColumnWidth(),
                EdgeInsets.fromLTRB(16, 8, 16, 8),
                new BoxDecoration(color: Colors.grey.shade50),
                EdgeInsets.all(8.0f),
                new BoxDecoration(
                    color: Colors.blue.shade100,
                    borderRadius: BorderRadius.circular(2.0f)
                ),
                EdgeInsets.all(8.0f),
                new BoxDecoration(
                    color: Colors.grey.shade200,
                    borderRadius: BorderRadius.circular(2.0f)
                ),
                new BoxDecoration(
                    border: new Border(
                        top: new BorderSide(width: 5.0f, color: Colors.grey.shade300)
                    )
                )
            );
        }


        public MarkdownStyleSheet copyWith(TextStyle a,
            TextStyle p = null,
            TextStyle code = null,
            TextStyle h1 = null,
            TextStyle h2 = null,
            TextStyle h3 = null,
            TextStyle h4 = null,
            TextStyle h5 = null,
            TextStyle h6 = null,
            TextStyle em = null,
            TextStyle strong = null,
            TextStyle del = null,
            TextStyle blockquote = null,
            TextStyle img = null,
            TextStyle checkbox = null,
            float blockSpacing = 1.0f,
            float listIndent = 1.0f,
            TextStyle listBullet = null,
            TextStyle tableHead = null,
            TextStyle tableBody = null,
            TextAlign tableHeadAlign = TextAlign.center,
            TableBorder tableBorder = null,
            TableColumnWidth tableColumnWidth = null,
            EdgeInsets tableCellsPadding = null,
            Decoration tableCellsDecoration = null,
            EdgeInsets blockquotePadding = null,
            Decoration blockquoteDecoration = null,
            EdgeInsets codeblockPadding = null,
            Decoration codeblockDecoration = null,
            Decoration horizontalRuleDecoration = null,
            float textScaleFactor = 1.0f) {
            return new MarkdownStyleSheet(
                a: a ?? this.a,
                p: p ?? this.p,
                code: code ?? this.code,
                h1: h1 ?? this.h1,
                h2: h2 ?? this.h2,
                h3: h3 ?? this.h3,
                h4: h4 ?? this.h4,
                h5: h5 ?? this.h5,
                h6: h6 ?? this.h6,
                em: em ?? this.em,
                strong: strong ?? this.strong,
                del: del ?? this.del,
                blockquote: blockquote ?? this.blockquote,
                img: img ?? this.img,
                checkbox: checkbox ?? this.checkbox,
                blockSpacing: blockSpacing,
                listIndent: listIndent,
                listBullet: listBullet ?? this.listBullet,
                tableHead: tableHead ?? this.tableHead,
                tableBody: tableBody ?? this.tableBody,
                tableHeadAlign: tableHeadAlign,
                tableBorder: tableBorder ?? this.tableBorder,
                tableColumnWidth: tableColumnWidth ?? this.tableColumnWidth,
                tableCellsPadding: tableCellsPadding ?? this.tableCellsPadding,
                tableCellsDecoration: tableCellsDecoration ?? this.tableCellsDecoration,
                blockquotePadding: blockquotePadding ?? this.blockquotePadding,
                blockquoteDecoration: blockquoteDecoration ?? this.blockquoteDecoration,
                codeblockPadding: codeblockPadding ?? this.codeblockPadding,
                codeblockDecoration: codeblockDecoration ?? this.codeblockDecoration,
                horizontalRuleDecoration: horizontalRuleDecoration ?? this.horizontalRuleDecoration,
                textScaleFactor: textScaleFactor
            );
        }


        public MarkdownStyleSheet merge(MarkdownStyleSheet other) {
            if (other == null) {
                return this;
            }

            return this.copyWith(
                a: other.a,
                p: other.p,
                code: other.code,
                h1: other.h1,
                h2: other.h2,
                h3: other.h3,
                h4: other.h4,
                h5: other.h5,
                h6: other.h6,
                em: other.em,
                strong: other.strong,
                del: other.del,
                blockquote: other.blockquote,
                img: other.img,
                checkbox: other.checkbox,
                blockSpacing: other.blockSpacing,
                listIndent: other.listIndent,
                listBullet: other.listBullet,
                tableHead: other.tableHead,
                tableBody: other.tableBody,
                tableHeadAlign: other.tableHeadAlign,
                tableBorder: other.tableBorder,
                tableColumnWidth: other.tableColumnWidth,
                tableCellsPadding: other.tableCellsPadding,
                tableCellsDecoration: other.tableCellsDecoration,
                blockquotePadding: other.blockquotePadding,
                blockquoteDecoration: other.blockquoteDecoration,
                codeblockPadding: other.codeblockPadding,
                codeblockDecoration: other.codeblockDecoration,
                horizontalRuleDecoration: other.horizontalRuleDecoration,
                textScaleFactor: other.textScaleFactor
            );
        }
    }
}