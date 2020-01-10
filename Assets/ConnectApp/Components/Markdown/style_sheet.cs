using System.Collections.Generic;
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
            float textScaleFactor = 1.0f
        ) {
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

            this._styles = new Dictionary<string, TextStyle>() {
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
                {"tr", tableHead},
                {"td", tableHead},
            };
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

        public float blockSpacing, listIndent, textScaleFactor;

        public TextAlign tableHeadAlign;

        public TableBorder tableBorder;

        public TableColumnWidth tableColumnWidth;

        public EdgeInsets tableCellsPadding, blockquotePadding, codeblockPadding;

        public Decoration tableCellsDecoration, blockquoteDecoration, codeblockDecoration, horizontalRuleDecoration;

        Dictionary<string, TextStyle> _styles;

        public Dictionary<string, TextStyle> styles {
            get { return this._styles; }
        }

        public static MarkdownStyleSheet fromTheme(ThemeData theme) {
            return new MarkdownStyleSheet(
                a: new TextStyle(true, Colors.blue),
                p: theme.textTheme.body1,
                code: theme.textTheme.body1.copyWith(
                    null,
                    null,
                    Colors.grey.shade200,
                    "monospace",
                    null,
                    theme.textTheme.body1.fontSize * 0.85f,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                ),
                h1: theme.textTheme.headline,
                h2: theme.textTheme.title,
                h3: theme.textTheme.subhead,
                h4: theme.textTheme.body2,
                h5: theme.textTheme.body2,
                h6: theme.textTheme.body2,
                em: new TextStyle(fontStyle: FontStyle.italic),
                strong: new TextStyle(fontWeight: FontWeight.bold),
                del: new TextStyle(decoration: TextDecoration.lineThrough),
                blockquote: theme.textTheme.body1,
                img: theme.textTheme.body1,
                checkbox: theme.textTheme.body1.copyWith(
                    color: theme.primaryColor
                ),
                blockSpacing: 8.0f,
                listIndent: 24.0f,
                listBullet: theme.textTheme.body1,
                tableHead: new TextStyle(fontWeight: FontWeight.w600),
                tableBody: theme.textTheme.body1,
                tableHeadAlign: TextAlign.center,
                tableBorder: TableBorder.all(color: Colors.grey.shade300, width: 0),
                tableColumnWidth: new FlexColumnWidth(),
                tableCellsPadding: EdgeInsets.fromLTRB(16, 8, 16, 8),
                tableCellsDecoration: new BoxDecoration(color: Colors.grey.shade50),
                blockquotePadding: EdgeInsets.all(8.0f),
                blockquoteDecoration: new BoxDecoration(
                    color: Colors.blue.shade100,
                    borderRadius: BorderRadius.circular(2.0f)
                ),
                codeblockPadding: EdgeInsets.all(8.0f),
                codeblockDecoration: new BoxDecoration(
                    color: Colors.grey.shade200,
                    borderRadius: BorderRadius.circular(2.0f)
                ),
                horizontalRuleDecoration: new BoxDecoration(
                    border: new Border(
                        top: new BorderSide(width: 5.0f, color: Colors.grey.shade300)
                    )
                )
            );
        }
    }
}